using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using NLog;
using System.Data;
using FSPAirnavDatabaseExporter.MBTiles;
using ConsoleReadXplaneData.Firebase;
using ConsoleReadXplaneData;

namespace FSPAirnavDatabaseExporter
{
    class Program
    {
        static string databaseFilename;
        static string filesPath;
        static List<string> mapLocationList;
        static Logger log;
        static Boolean spatialEnabled = false;

        static List<ImportTypes> importTypes;

        static void Main(string[] args)
        {
            databaseFilename = String.Format(ConsoleReadXplaneData.Properties.Settings.Default.Database + "_V{0:0000}{1:00}{2:00}.db",
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            log = LogManager.GetCurrentClassLogger();
            log.Info("Start Airnav Database create program");
            filesPath = ConsoleReadXplaneData.Properties.Settings.Default.InputDataDir;

            Boolean test = false;
            Boolean useFirebase = true;
            Boolean useSQLite = false;

            DataDownloader downloader = new DataDownloader(filesPath);
            downloader.DownloadFiles();

            if (!test)
            {
                importTypes = new List<ImportTypes>() 
                {
                    //ImportTypes.mbtiles
                    ImportTypes.airports
                    ////,ImportTypes.continents 
                    //,ImportTypes.countries
                    //,ImportTypes.fixes
                    //,ImportTypes.frequencies
                    //,ImportTypes.navaids
                    //,ImportTypes.regions
                    //,ImportTypes.runways
                    //,ImportTypes.firs
                    ////ImportTypes.test
                };

                Database.CreateDatabase(databaseFilename);
                Database.CreateTables(databaseFilename, spatialEnabled);

                mapLocationList = new List<string>();

                log.Info("Tables created");
                log.Info("*********************************************");

                //// *************************************************************************************
                XPlaneReader xplaneReader;

                xplaneReader = new XPlaneReader();

                if (importTypes.Contains(ImportTypes.mbtiles))
                {
                    ReadMBTiles reader = new ReadMBTiles();
                    DataTable mbTilesTable = reader.Process();
                    Database.InsertTableIntoDatabase(mbTilesTable, "tbl_MbTiles", databaseFilename, mapLocationList, false);
                }

                if (importTypes.Contains(ImportTypes.fixes))
                {
                    log.Info("start reading xplane-fix");
                    DataTable fixTable = xplaneReader.ReadFixFile(@"data\" + "earth_fix.dat");

                    log.Info("xplane-fix file read");
                    xplaneReader = new XPlaneReader();

                    log.Info("*********************************************");

                    log.Info("Insert xplane-fix in database...");

                    mapLocationList.Add("tbl_Fixes");
                    Database.InsertFixTableIntoDatabase(fixTable, databaseFilename, spatialEnabled);
                    if (spatialEnabled) Database.AddgeomPoint("tbl_Fixes", databaseFilename, "", spatialEnabled);
                    log.Info("xplane-fix inserted in database!");
                    log.Info("*********************************************");
                }

                // *************************************************************************************

                CsvReader csvReader;
                // *************************************************************************************



                if (importTypes.Contains(ImportTypes.regions))
                {
                    csvReader = new CsvReader();
                    log.Info("start reading regions");
                    DataTable regionsTable = csvReader.ReadFile(filesPath + "regions.csv");

                    log.Info("Regions file read!");
                    log.Info("*********************************************");

                    log.Info("Insert regions in database...");

                    mapLocationList.Add("tbl_Region");
                    Database.InsertTableIntoDatabase(regionsTable, "tbl_Region", databaseFilename, mapLocationList, spatialEnabled);
                    log.Info("Regions inserted in database!");
                    log.Info("*********************************************");
                }

                // *************************************************************************************
                if (importTypes.Contains(ImportTypes.firs))
                {
                    csvReader = new CsvReader();
                    log.Info("start reading firs");
                    DataTable regionsTable = csvReader.ReadFile(@"data\" + "fir.csv");

                    log.Info("Regions file read!");
                    log.Info("*********************************************");

                    log.Info("Insert Firs in database...");

                    Database.InsertTableIntoDatabase(regionsTable, "tbl_Firs", databaseFilename, mapLocationList, spatialEnabled);
                    log.Info("Firs inserted in database!");
                    log.Info("*********************************************");
                }


                // *************************************************************************************
                if (importTypes.Contains(ImportTypes.countries))
                {
                    log.Info("start reading countries");
                    csvReader = new CsvReader();
                    DataTable countriesTable = csvReader.ReadFile(filesPath + "countries.csv");

                    log.Info("Countries file read!");
                    log.Info("*********************************************");

                    log.Info("Insert countries in database...");

                    Database.InsertTableIntoDatabase(countriesTable, "tbl_Country", databaseFilename, mapLocationList, spatialEnabled);
                    log.Info("Countries inserted in database!");
                    log.Info("*********************************************");
                }

                // *************************************************************************************
                if (importTypes.Contains(ImportTypes.airports))
                {
                    log.Info("start reading airports");
                    

                    

                    log.Info("airports file read!");
                    log.Info("*********************************************");

                    log.Info("Insert airports in database...");

                    if (useSQLite)
                    {
                        csvReader = new CsvReader();
                        DataTable airportsTable = csvReader.ReadFile(filesPath + "airports.csv");
                        //mapLocationList.Add("tbl_Airports");
                        Database.InsertTableIntoDatabase(airportsTable, "tbl_Airports", databaseFilename, mapLocationList, spatialEnabled);
                        if (spatialEnabled) Database.AddgeomPoint("tbl_Airports", databaseFilename, "", spatialEnabled);
                    }

                    if (useFirebase)
                    {
                        //FirebaseDBClient.WriteData(airportsTable, ImportTypes.airports);
                        //String filename = filesPath + "airports.json";
                        //FirebaseDBClient.SaveAsJson(airportsTable, ImportTypes.airports, filename);
                        AirportsExport airportsExport = new AirportsExport(filesPath);
                        airportsExport.CreateAirportJson(filesPath + "airports.json");
                    }

                    log.Info("airports inserted in database!");
                    log.Info("*********************************************");
                }

                // *************************************************************************************

                if (importTypes.Contains(ImportTypes.navaids))
                {
                    log.Info("start reading Navaids");
                    csvReader = new CsvReader();
                    DataTable navaidsTable = csvReader.ReadFile(filesPath + "navaids.csv");

                    log.Info("Navaids file read!");
                    log.Info("*********************************************");

                    log.Info("Insert Navaids in database...");

                    mapLocationList.Add("tbl_Navaids");
                    Database.InsertTableIntoDatabase(navaidsTable, "tbl_Navaids", databaseFilename, mapLocationList, spatialEnabled);
                    if (spatialEnabled) Database.AddgeomPoint("tbl_Navaids", databaseFilename, "", spatialEnabled);
                    if (spatialEnabled) Database.AddgeomPoint("tbl_Navaids", databaseFilename, "dme_", spatialEnabled);
                    log.Info("Navaids inserted in database!");
                    log.Info("*********************************************");
                }

                // *************************************************************************************
                if (importTypes.Contains(ImportTypes.runways))
                {
                    log.Info("start reading runways");
                    csvReader = new CsvReader();
                    DataTable runwaysTable = csvReader.ReadFile(filesPath + "runways.csv");

                    log.Info("Runways file read!");
                    log.Info("*********************************************");

                    log.Info("Insert Runways in database...");

                    Database.InsertTableIntoDatabase(runwaysTable, "tbl_Runways", databaseFilename, mapLocationList, spatialEnabled);
                    if (spatialEnabled) Database.AddgeomPoint("tbl_Runways", databaseFilename, "le_", spatialEnabled);
                    if (spatialEnabled) Database.AddgeomPoint("tbl_Runways", databaseFilename, "he_", spatialEnabled);
                    log.Info("Runways inserted in database!");
                    log.Info("*********************************************");
                }

                // ************************************************************************************

                if (importTypes.Contains(ImportTypes.frequencies))
                {
                    log.Info("start reading frequencies");
                    csvReader = new CsvReader();
                    DataTable frequenciesTable = csvReader.ReadFile(filesPath + "airport-frequencies.csv");

                    log.Info("Frequencies file read!");
                    log.Info("*********************************************");

                    log.Info("Insert Frequencies in database...");

                    Database.InsertTableIntoDatabase(frequenciesTable, "tbl_AirportFrequencies", databaseFilename, mapLocationList, spatialEnabled);
                    log.Info("Frequencies inserted in database!");
                    log.Info("*********************************************");
                }

                if (importTypes.Contains(ImportTypes.test))
                {
                    log.Info("start test insert");
                    Database.testInsert("tbl_Airports", databaseFilename, spatialEnabled);
                }

                Database.CreateTableIndexen(databaseFilename, spatialEnabled);
                log.Info("Tables Indexen created");

                // ************************************************************************************

                log.Info("Insert Andriod in database...");
                Database.CreateAndriodTable(databaseFilename, spatialEnabled);
                log.Info("Andriod table inserted in database!");
                log.Info("*********************************************");

                log.Info("Insert tbl_Properties in database...");
                Database.CreatePropertiesTable(databaseFilename, spatialEnabled);
                log.Info("tbl_Properties table inserted in database!");
                log.Info("*********************************************");

            }
            else
            {
                Test();
            }

            Console.ReadKey();
        }

        public static void Test()
        {
            log.Info("Start Test function");

            //Database.testSpatialDllLoad(databaseFilename);
        }

        
    }
}
