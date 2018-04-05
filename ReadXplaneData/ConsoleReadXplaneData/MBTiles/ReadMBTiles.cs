using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

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

        }

        private SQLiteConnection connection;

        public void Process()
        {
            String database = @"data\mbtiles.db";
            connection = Database.getConnection(database);
            connection.Open();

            SQLiteDataReader typeReader = getTypes();
            while(typeReader.Read())
            {
                MBTileType _type = MBTileTypeHelper.Parse(typeReader["type"].ToString());
                String _typeDescription = typeReader["description"].ToString();
                SQLiteDataReader urlReader = getMbTileUrls(_type);
                if (urlReader.Read())
                {

                    if (_type == MBTileType.ofm)
                    {
                        String _xmlLink = urlReader["link_xml"].ToString();
                        int i = 1;
                    }
                }
            }

            connection.Close();
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
