using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace FSPAirnavDatabaseExporter.MBTiles
{
    public class ReadMBTiles
    {
        // First read the types
        // Loop through the types and get the URLs for each type
        // Get the regions for each type
        // Depending on the type
        // ofm : Create the link to the XML with the current version
        //       get the XML with the current version
        //       Check the version update and update db if necessary
        //       Get the XML for the new version if necessary
        //       Get the MBTiles download links from the XML
        //       Store the information in the tbl_mbtiles table of the airnav database

        public ReadMBTiles()
        {
            log = LogManager.GetCurrentClassLogger();
        }

        private Logger log;
        private SQLiteConnection connection;

        public DataTable Process()
        {
            String database = @"data\mbtiles.db";
            connection = Database.getConnection(database);
            connection.Open();
            DataTable dataTiles = new DataTable();

            SQLiteDataReader typeReader = getTypes();
            while(typeReader.Read())
            {
                MBTileType _type = MBTileTypeHelper.Parse(typeReader["type"].ToString());
                String _typeDescription = typeReader["description"].ToString();
                log.Info("Found type: {0} Description: {1}", _type, _typeDescription);

                SQLiteDataReader urlReader = getMbTileUrls(_type);
                if (urlReader.Read())
                {

                    if (_type == MBTileType.ofm)
                    {
                        String _xmlLink = urlReader["link_xml"].ToString();
                        List<MbTile> tiles = parseOfmRegions(_xmlLink);
                        foreach(MbTile tile in tiles)
                        {
                            DataRow tilerow = dataTiles.NewRow();
                            tilerow["name"] = tile.Name;
                            tilerow["region"] = tile.Region;
                            tilerow["type "] = tile.Type.ToString();
                            tilerow["mbtileslink "] = tile.MbTilesLink;
                            tilerow["xmllink "] = tile.XmlLink;
                            tilerow["version "] = tile.Version;
                            tilerow["startValidity  "] = tile.startValidity.Millisecond;
                            tilerow["endValidity  "] = tile.endValidity.Millisecond;
                        }
                    }
                }
            }

            connection.Close();
            return dataTiles;
        }

        private List<MbTile> parseOfmRegions(String xmlLink)
        {
            List<MbTile> tiles = new List<MbTile>();

            String query = "SELECT * FROM tbl_regions WHERE type=@type;";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@type", MBTileType.ofm.ToString());
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                String _region = reader["region"].ToString();
                int _version = Convert.ToInt32(reader["version"].ToString());
                String _name = reader["name"].ToString();
                long _startValidity = Convert.ToInt64(reader["startValidity"].ToString());
                long _ebdValidity = Convert.ToInt64(reader["endValidity"].ToString());

                String link = createLink(_region, _version, xmlLink);

                log.Info("Reading: {0} with XML link {1}.", _name, link);
                String xml = downloadXml(link);
                log.Info("XML Downloaded");
                MbTile tile = parseOfmXml(xml, _version);
                if (tile.Version>_version)
                {
                    log.Info("Found newer version {0} than in database {1}", _version, tile.Version);
                    _version = tile.Version;
                    link = createLink(_region, _version, xmlLink);
                    log.Info("Reading new: {0} with XML link {1}.", _name, link);
                    xml = downloadXml(link);
                    tile = parseOfmXml(xml, _version);
                    // Update the source database
                }

                tile.Region = _region;
                tile.Name = _name;
                tile.XmlLink = link;

                // Store tile in airnav DB
                tiles.Add(tile);
            }
            return tiles;
        }

        private String createLink(String region, Int32 version, String Inlink)
        {
            String link = Inlink.Replace("#VERSION#", version.ToString());
            link = link.Replace("#REGION#", region);
            return link;
        }

        private MbTile parseOfmXml(String xml, Int32 curVersion)
        {
            XDocument doc = XDocument.Parse(xml);

            var cycles = from v in doc.Root.Elements("nearCycles").Elements("cycle")
                         select new MbTile
                         {
                             Version = Convert.ToInt32(v.Attribute("id").Value),
                             startValidity = Convert.ToDateTime(v.Attribute("startValidity").Value),
                             endValidity = Convert.ToDateTime(v.Attribute("endValidity").Value)
                         };

            var cycleMax = cycles.First(x => x.Version == cycles.Max(xx => xx.Version));

            if (cycleMax.Version>curVersion)
            {
                return cycleMax;
            }

            var download_item = from d in doc.Root.Elements("item")
                                where d.Attribute("type").Value == "downloads"
                                select d;
            var section = from m in download_item.Elements("section")
                          where m.Attribute("header_english").Value == "Application Formats"
                          select m;
            var product = from p in section.Elements("product")
                          where p.Attribute("title_english").Value == "Mapbox Tiles"
                          select p;
            var mbdownload = from d in product.Elements("download").Elements("variant")
                             select d;

            cycleMax.MbTilesLink = (mbdownload.Count() > 1) ?
                (from m in mbdownload where m.Attribute("filter1_english").Value == "double resolution (@2x)" select m).First().Attribute("URL").Value :
                mbdownload.First().Attribute("URL").Value;

            return cycleMax;
        }

        private String downloadXml(string address)
        {
            String xml = "";

            using (var client = new WebClient())
            {
                xml = client.DownloadString(address);
            }

            return xml;
        }

        private SQLiteDataReader getTypes()
        {
            String query = "SELECT * FROM tbl_type;";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        private SQLiteDataReader getMbTileUrls(MBTileType type)
        {
            String query = "SELECT * FROM tbl_mbtiles WHERE type=@type;";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@type", type.ToString());
            SQLiteDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        
    }
}
