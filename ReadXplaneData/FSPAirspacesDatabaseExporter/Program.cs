using FSPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ConsoleReadXplaneData.EF;

namespace FSPAirspacesDatabaseExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger log = LogManager.GetCurrentClassLogger();

            log.Info("Start importing Airspaces");
            String basepath = @"C:\AirnavData\Airspaces\";

            EFDatabase database = new EFDatabase(basepath);

            Downloader downloader = new Downloader();
            if (downloader.DownloadXSourLinks(basepath))
                database.ProcessAirspaces(downloader.Links);

            Console.ReadKey();
        }
    }
}
