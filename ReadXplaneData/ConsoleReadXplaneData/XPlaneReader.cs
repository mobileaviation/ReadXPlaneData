using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using NLog;

namespace FSPAirnavDatabaseExporter
{
    public class XPlaneReader
    {
        private StreamReader reader;
        private DataTable table;

        public XPlaneReader()
        {
            CreateLogger();
        }

        private static Logger log;
        private static void CreateLogger()
        {
            log = LogManager.GetCurrentClassLogger();
        }

        public DataTable ReadFixFile(String filename)
        {
            reader = new StreamReader(File.OpenRead(filename));
            table = new DataTable();

            table.Columns.Add("latitude_deg", typeof(string));
            table.Columns.Add("longitude_deg", typeof(string));
            table.Columns.Add("ident", typeof(string));

            log.Info("Opened: {0}", filename);

            while (!reader.EndOfStream)
            {
                String lines = reader.ReadLine();
                char[] delimiters = new char[] { ' ' };
                String[] Values = lines.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                if (Values.Length == 3)
                {
                    table.Rows.Add(Values);
                }

            }

            return table;
        }
    }
}
