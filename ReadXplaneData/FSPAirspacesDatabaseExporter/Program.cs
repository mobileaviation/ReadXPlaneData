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

namespace FSPAirspacesDatabaseExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            Boolean test = false;

            if (!test)
            {
                log.Info("Start importing Airspaces");
                String basepath = @"C:\AirnavData\Airspaces\";

                EFDatabase database = new EFDatabase(basepath);

                Downloader downloader = new Downloader("");
                if (downloader.DownloadXSourLinks(basepath))
                    database.ProcessAirspaces(downloader.Links);
            }
            else
            {
                String t = "DC 15";
                String tt = Helpers.findRegex("([0-9.]+\\w)|([0-9])", t);
                Double ttt = double.Parse(tt, CultureInfo.InvariantCulture);
                int i = 0;
            }

            Console.ReadKey();
        }
    }
}
