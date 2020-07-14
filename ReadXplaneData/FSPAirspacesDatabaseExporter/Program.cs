using FSPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ConsoleReadXplaneData.EF;
using FSPService.Classes;
using System.Globalization;
using FSPService.Enums;
using ConsoleReadXplaneData;

namespace FSPAirspacesDatabaseExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            Boolean test = false;

            ExportType exportType = ExportType.MsSql;
            //ExportType exportType = ExportType.MongoDB;
            //ExportType exportType = ExportType.FirebaseJson;
            //ExportType exportType = ExportType.MySql;

            if (!test)
            {
                log.Info("Start importing Airspaces");
                String basepath = Properties.Settings.Default.DownloadPath;


                Downloader downloader = new Downloader("");
                //Downloader downloader = new Downloader("NL");
                if (downloader.DownloadXSourLinks(basepath, exportType))
                {
                    if (exportType == ExportType.MsSql)
                    {
                        EFDatabase database = new EFDatabase(basepath);
                        database.ProcessAirspaces(downloader.Links, exportType);
                        //database.ProcessAirspaces(downloader.Links, ExportType.GeoJson);
                    }
                    if (exportType == ExportType.MySql)
                    {
                        log.Info("MYSQL Export Selected");
                    }
                }
            }
            else
            {
                Airspaces airspaces = new Airspaces();
                airspaces.processAirspaceFile(@"C:\Users\rob.verhoef\Downloads\openaip_airspace_netherlands_nl.aip", "Austria", AirspaceFileType.openaip);
            }

            Console.ReadKey();
        }
    }
}
