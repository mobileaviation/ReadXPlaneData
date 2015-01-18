﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using NLog;
using System.Resources;
using System.Data;

namespace ConsoleReadXplaneData
{
    public class Database
    {
        private static Logger log;
        private static void CreateLogger()
        {
            log = LogManager.GetCurrentClassLogger();
        }

        public static Boolean CreateDatabase(string databaseFilename)
        {
            CreateLogger();
            try
            {
                log.Info("Try to create databasefile: {0} ", databaseFilename);
                SQLiteConnection.CreateFile(databaseFilename);
                log.Info("Databasefile: {0} created!", databaseFilename);
            }
            catch (Exception ee)
            {
                log.Error("Exception creating database: {0}", ee.Message);
                return false;
            }
            return true;
        }

        private static SQLiteConnection GetConnection(string databaseFilename)
        {
            CreateLogger();
            log.Info("Get database connection to: {0}", databaseFilename);
            return new SQLiteConnection("Data Source=" + databaseFilename + ";Version=3;");
        }

        public static void CreateTables(string databaseFilename)
        {
            SQLiteConnection con = GetConnection(databaseFilename);
            CreateLogger();
            log.Info("Start creating database tables");

            try
            {
                con.Open();

                string q = "DROP TABLE IF EXISTS tbl_Airports";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS tbl_Country";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS tbl_Continent";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS tbl_Navaids";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS tbl_Runways";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS tbl_Fixes";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS tbl_Region";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS tbl_AirportFrequencies";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS tbl_Properties";
                ExecuteQuery(con, q);
                q = "DROP TABLE IF EXISTS android_metadata";
                ExecuteQuery(con, q);

                q = Tables.CreateAirportTable;
                ExecuteQuery(con, q);
                q = Tables.CreateContinentTable;
                ExecuteQuery(con, q);
                q = Tables.CreateCountryTable;
                ExecuteQuery(con, q);
                q = Tables.CreateFixesTable;
                ExecuteQuery(con, q);
                q = Tables.CreateRunwaysTable;
                ExecuteQuery(con, q);
                q = Tables.CreateRegionsTable;
                ExecuteQuery(con, q);
                q = Tables.CreateNavaidsTable;
                ExecuteQuery(con, q);
                q = Tables.CreateFrequenciesTable;
                ExecuteQuery(con, q);
            }
            catch (Exception ee)
            {
                log.Error("Exception creating tables: {0}", ee.Message);
            }
            finally
            {
                con.Close();

                log.Info("Connection closed");
            }
        }

        public static void CreateAndriodTable(string databaseFilename)
        {
            SQLiteConnection con = GetConnection(databaseFilename);
            CreateLogger();
            log.Info("Start creating database tables");

            try
            {
                con.Open();
                string q = Tables.CreateAndroidMetadata;
                ExecuteQuery(con, q);
                q = Tables.InsertAndroidMetadata;
                ExecuteQuery(con, q);

            }
            catch (Exception ee)
            {
                log.Error("Exception creating android tables: {0}", ee.Message);
            }
            finally
            {
                con.Close();

                log.Info("Connection closed");
            }
        }

        public static void CreatePropertiesTable(string databaseFilename)
        {
            SQLiteConnection con = GetConnection(databaseFilename);
            CreateLogger();
            log.Info("Start creating database tables");

            try
            {
                con.Open();
                string q = Tables.CreateTableProperties;
                ExecuteQuery(con, q);
                q = Tables.InsertProperties;
                ExecuteQuery(con, q);

            }
            catch (Exception ee)
            {
                log.Error("Exception creating properties table: {0}", ee.Message);
            }
            finally
            {
                con.Close();

                log.Info("Connection closed");
            }
        }

        public static void CreateTableIndexen(string databaseFilename)
        {
            SQLiteConnection con = GetConnection(databaseFilename);
            CreateLogger();
            log.Info("Start creating database tables");

            try
            {
                con.Open();
                string q = Tables.CreateLocationAirportTableIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateNameIdentAirportTableIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateFixesLocationIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateFixesNameIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateRunwaysAirportHELocationIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateRunwaysAirportLELocationIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateRunwaysAirportIdentIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateNavaidsIdentNameIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateNavaidsLocationIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateAirportMapLocationIDIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateFixesMapLocationIDIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateNavAidsMapLocationIdIndex;
                ExecuteQuery(con, q);
                q = Tables.CreateFrequenciesAirportIdentIndex;
                ExecuteQuery(con, q);
            }
            catch (Exception ee)
            {
                log.Error("Exception creating tables: {0}", ee.Message);
            }
            finally
            {
                con.Close();

                log.Info("Connection closed");
            }
        }

        private static void ExecuteQuery(SQLiteConnection con, string Query)
        {
            SQLiteCommand cmd = new SQLiteCommand(Query, con);
            cmd.ExecuteNonQuery();
        }

        private static SQLiteCommand CreateInsertCmd(DataRow row, string tableName)
        {
            DataTable table = row.Table;

            string v = "VALUES(";
            string q = "INSERT INTO " + tableName +
                "(";
            foreach (DataColumn C in table.Columns)
            {
                q = q + C.ColumnName.Replace(@"""", "") + ",";
                v = v + "@" + C.ColumnName.Replace(@"""", "") + ",";
  
            }

            q = q.Substring(0, q.Length - 1) + ")";
            v = v.Substring(0, v.Length - 1) + ")";

            q = q + v;

            SQLiteCommand cmd = new SQLiteCommand(q);
            foreach (DataColumn C in table.Columns)
            {
                cmd.Parameters.AddWithValue("@" + C.ColumnName.Replace(@"""", ""),
                    row[C.ColumnName.Replace(@"""", "")]);
            }

            return cmd;
        }


        //
        // UPDATE tbl_Airports SET MapLocation_ID = (90 *  CAST(((latitude_deg + 90)/2) as integer)) + CAST(((longitude_deg + 180)/4) as integer)   
        //
        private static void UpdateMapLocationID(String tablename, SQLiteConnection con)
        {
            log.Info("Updating MapLocation_ID for: {0}", tablename);
            try
            {
                String q = "UPDATE "+tablename+" SET MapLocation_ID = (90 *  CAST(((latitude_deg + 90)/2) as integer)) + CAST(((longitude_deg + 180)/4) as integer)";
                SQLiteCommand cmd = new SQLiteCommand(q, con);

                cmd.ExecuteNonQuery();
                log.Info("MapLocation_ID for: {0} is updated!!", tablename);
            }
            catch (Exception ee)
            {
                log.Error("Error updating MapLocation_ID for {0} Error: {1}", tablename, ee.Message);
            }
        }

        public static void InsertFixTableIntoDatabase(DataTable table, String databaseFilename)
        {
            String tableName = "tbl_Fixes";
            SQLiteConnection con = GetConnection(databaseFilename);

            try
            {
                con.Open();
                CreateLogger();
                float count = table.Rows.Count;
                float pos = 0;

                foreach (DataRow R in table.Rows)
                {
                    String q = "INSERT INTO " + tableName + " (name, ident, latitude_deg, longitude_deg)" +
                        " VALUES(@name, @ident, @latitude_deg, @longitude_deg);";
                    SQLiteCommand cmd = new SQLiteCommand(q, con);
                    cmd.Parameters.AddWithValue("@ident", R["ident"].ToString());
                    cmd.Parameters.AddWithValue("@name", R["ident"].ToString());
                    cmd.Parameters.AddWithValue("@latitude_deg", R["latitude_deg"].ToString());
                    cmd.Parameters.AddWithValue("@longitude_deg", R["longitude_deg"].ToString());
                    cmd.ExecuteNonQuery();

                    

                    float progress = (pos / count) * 100;
                    log.Info("Inserted: {0} in {1}, progress: {2}", R[0], tableName, Math.Abs(progress));

                    pos = pos + 1;
                }

                UpdateMapLocationID(tableName, con);

            }
            catch (Exception ee)
            {
                log.Error("Insert exception: {0}", ee.Message);
            }
            finally
            {
                con.Close();
            }
        }   

        public static void InsertTableIntoDatabase(DataTable table, String tableName, String databaseFilename, List<string> mapLocations)
        {
            SQLiteConnection con = GetConnection(databaseFilename);

            try
            {
                con.Open();
                CreateLogger();
                float count = table.Rows.Count;
                float pos = 0;

                foreach (DataRow R in table.Rows)
                {
                    SQLiteCommand cmd = CreateInsertCmd(R, tableName);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();

                    float progress = (pos / count) * 100;
                    log.Info("Inserted: {0} in {1}, progress: {2}", R[0], tableName, Math.Abs(progress));

                    pos = pos + 1;
                }

                if (mapLocations.Contains(tableName))
                    UpdateMapLocationID(tableName, con);
            }
            catch (Exception ee)
            {
                log.Error("Insert exception: {0}", ee.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}