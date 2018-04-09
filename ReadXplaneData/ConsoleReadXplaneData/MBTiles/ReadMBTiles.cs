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
            DataTable dataTiles = getMbTilesTable();

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
                            tilerow["type"] = tile.Type.ToString();
                            tilerow["mbtileslink"] = tile.MbTilesLink;
                            tilerow["xmllink"] = tile.XmlLink;
                            tilerow["version"] = tile.Version;
                            tilerow["startValidity"] = getUnixTimeStamp(tile.startValidity);
                            tilerow["endValidity"] = getUnixTimeStamp(tile.endValidity);
                            dataTiles.Rows.Add(tilerow);
                        }
                    }
                }
            }

            connection.Close();
            return dataTiles;
        }

        private DataTable getMbTilesTable()
        {
            DataTable dataTiles = new DataTable();
            dataTiles.Columns.Add("name", typeof(string));
            dataTiles.Columns.Add("region", typeof(string));
            dataTiles.Columns.Add("type", typeof(string));
            dataTiles.Columns.Add("mbtileslink", typeof(string));
            dataTiles.Columns.Add("xmllink", typeof(string));
            dataTiles.Columns.Add("version", typeof(int));
            dataTiles.Columns.Add("startValidity", typeof(long));
            dataTiles.Columns.Add("endValidity", typeof(long));
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
                int id = Convert.ToInt32(reader["_id"].ToString());
                String _region = reader["region"].ToString();
                int _version = Convert.ToInt32(reader["version"].ToString());
                String _name = reader["name"].ToString();
                //DateTime _startValidity = Convert.ToDateTime(reader["startValidity"].ToString());
                //DateTime _endValidity = Convert.ToDateTime(reader["endValidity"].ToString());

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
                    updateVersion(id, tile.Version, tile.startValidity, tile.endValidity);
                }

                tile.Region = _region;
                tile.Name = _name;
                tile.XmlLink = link;

                // Store tile in airnav DB
                if (tile.MbTilesLink!=null) tiles.Add(tile);
            }
            return tiles;
        }

        private void updateVersion(int id, int version, DateTime startValidity, DateTime endValidity)
        {
            String query = "UPDATE tbl_regions SET version=@version, startValidity=@startValidity, endValidity=@endValidity WHERE _id=@id;";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@version", version);
            cmd.Parameters.AddWithValue("@startValidity", getUnixTimeStamp(startValidity));
            cmd.Parameters.AddWithValue("@endValidity", getUnixTimeStamp(endValidity));
            cmd.ExecuteNonQuery();
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
            MbTile cycleMax = null;
            if (cycles.Count()>0)
                cycleMax = cycles.First(x => x.Version == cycles.Max(xx => xx.Version));
            else
            {
                cycleMax = new MbTile();
                cycleMax.Version = curVersion;
                cycleMax.startValidity = DateTime.Now;
                cycleMax.startValidity = DateTime.Now.AddDays(20);
            }

            if (cycleMax.Version>curVersion)
            {
                return cycleMax;
            }

            var download_item = from d in doc.Root.Elements("item")
                                where d.Attribute("type").Value == "downloads"
                                select d;
            var section = from m in download_item.Elements("section")
                          where m.Attribute("header_english").Value.ToLower() == "application formats"
                          select m;
            var product = from p in section.Elements("product")
                          where p.Attribute("title_english").Value.ToLower() == "mapbox tiles"
                          select p;
            var mbdownload = from d in product.Elements("download").Elements("variant")
                             select d;

            if (mbdownload.Count() > 0) 
                cycleMax.MbTilesLink = (mbdownload.Count() > 1) ?
                    (from m in mbdownload where m.Attribute("filter1_english").Value.ToLower().Contains("double") select m).First().Attribute("URL").Value :
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


        private long getUnixTimeStamp(DateTime date)
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            return ((date.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
        }
        
    }
}
