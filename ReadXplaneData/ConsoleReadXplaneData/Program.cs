using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using NLog;
using System.Data;

namespace ConsoleReadXplaneData
{
    class Program
    {
        static string databaseFilename = "airnav.db";
        static string filesPath = @"C:\Users\Rob Verhoef\Documents\NavAirDataFiles\";
        static List<string> mapLocationList;

        static List<ImportTypes> importTypes;

        static void Main(string[] args)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            log.Info("Start Airnav Database create program");

            importTypes = new List<ImportTypes>() {
                ImportTypes.airports,
                ImportTypes.continents, 
                ImportTypes.countries,
                ImportTypes.fixes,
                ImportTypes.frequencies,
                ImportTypes.navaids,
                ImportTypes.regions,
                ImportTypes.runways
            };

            Database.CreateDatabase(databaseFilename);
            Database.CreateTables(databaseFilename);

            mapLocationList = new List<string>();

            log.Info("Tables created");
            log.Info("*********************************************");

            //// *************************************************************************************
            XPlaneReader xplaneReader;
            
            xplaneReader = new XPlaneReader();

            if (importTypes.Contains(ImportTypes.fixes))
            {
                log.Info("start reading xplane-fix");
                DataTable fixTable = xplaneReader.ReadFixFile(filesPath + "earth_fix.dat");

                log.Info("xplane-fix file read");
                xplaneReader = new XPlaneReader();

                log.Info("*********************************************");

                log.Info("Insert xplane-fix in database...");

                mapLocationList.Add("tbl_Fixes");
                Database.InsertFixTableIntoDatabase(fixTable, databaseFilename);
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
                Database.InsertTableIntoDatabase(regionsTable, "tbl_Region", databaseFilename, mapLocationList);
                log.Info("Regions inserted in database!");
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

                Database.InsertTableIntoDatabase(countriesTable, "tbl_Country", databaseFilename, mapLocationList);
                log.Info("Countries inserted in database!");
                log.Info("*********************************************");
            }

            // *************************************************************************************
            if (importTypes.Contains(ImportTypes.airports))
            {
                log.Info("start reading airports");
                csvReader = new CsvReader();
                DataTable airportsTable = csvReader.ReadFile(filesPath + "airports.csv");

                log.Info("airports file read!");
                log.Info("*********************************************");

                log.Info("Insert airports in database...");

                mapLocationList.Add("tbl_Airports");
                Database.InsertTableIntoDatabase(airportsTable, "tbl_Airports", databaseFilename, mapLocationList);
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
                Database.InsertTableIntoDatabase(navaidsTable, "tbl_Navaids", databaseFilename, mapLocationList);
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

                Database.InsertTableIntoDatabase(runwaysTable, "tbl_Runways", databaseFilename, mapLocationList);
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

                Database.InsertTableIntoDatabase(frequenciesTable, "tbl_AirportFrequencies", databaseFilename, mapLocationList);
                log.Info("Frequencies inserted in database!");
                log.Info("*********************************************");
            }

            Database.CreateTableIndexen(databaseFilename);
            log.Info("Tables Indexen created");

            // ************************************************************************************

            log.Info("Insert Andriod in database...");
            Database.CreateAndriodTable(databaseFilename);
            log.Info("Andriod table inserted in database!");
            log.Info("*********************************************");

            log.Info("Insert tbl_Properties in database...");
            Database.CreatePropertiesTable(databaseFilename);
            log.Info("tbl_Properties table inserted in database!");
            log.Info("*********************************************");


            Console.ReadKey();
        }

        
    }
}
