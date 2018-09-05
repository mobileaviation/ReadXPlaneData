using FSPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace FSPAirspacesDatabaseExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger log = LogManager.GetCurrentClassLogger();

            log.Info("Start importing Airspaces");
            Airspaces airspaces = new Airspaces();
            airspaces.processAirspaceFile(@"C:\AirnavData\Airspaces\EHv18_3.txt", "NLD");

            Console.ReadKey();
        }
    }
}
