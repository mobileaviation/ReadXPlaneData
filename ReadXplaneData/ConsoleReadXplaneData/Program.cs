using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using NLog;
using System.Data;
using System.Data.Entity;
using FSPAirnavDatabaseExporter.MBTiles;
using ConsoleReadXplaneData.Firebase;
using ConsoleReadXplaneData;
using ConsoleReadXplaneData.EF;
using FSPService;
using FSPService.Mongo;

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
            Boolean download = true;

            //ExportType exportType = ExportType.MsSql;
            ExportType exportType = ExportType.MongoDB;
            //ExportType exportType = ExportType.FirebaseJson;

            if (download)
            {
                DataDownloader downloader = new DataDownloader(filesPath);
                downloader.DownloadFiles();
            }

            if (!test)
            {
                importTypes = new List<ImportTypes>();
                foreach (String t in ConsoleReadXplaneData.Properties.Settings.Default.InportTypes)
                {
                    ImportTypes type;
                    if (Enum.TryParse(t, out type)) importTypes.Add(type);
                }

                switch (exportType)
                {
                    case ExportType.FirebaseJson:
                        {
                            log.Info("Start export to firebase compatible json file");
                            AirportsExport airportsExport = new AirportsExport(filesPath);
                            airportsExport.CreateAirportJson(filesPath + "airports.json");
                            break;
                        }
                    case ExportType.GeoJson:
                        {
                            break;
                        }
                    case ExportType.Json:
                        {
                            log.Info("Start export to individual json files");
                            Console.ReadKey();
                            AirportsExport airportsExport = new AirportsExport(filesPath);
                            airportsExport.CreateJsonFiles();
                            break;
                        }
                    case ExportType.MsSql:
                        {
                            EFDatabase eFDatabase = new EFDatabase(filesPath);
                            eFDatabase.Process(importTypes);
                            break;
                        }
                    case ExportType.SqLiteDatabase:
                        {
                            SQLiteDatabase.StoreInSqliteDatabase(importTypes, databaseFilename, filesPath, spatialEnabled);
                            break;
                        }
                    case ExportType.MongoDB:
                        {
                            MongoDatabase mongoDatabase = new MongoDatabase(filesPath);
                            mongoDatabase.StartProcess(importTypes);
                            break;
                        }

                }
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
