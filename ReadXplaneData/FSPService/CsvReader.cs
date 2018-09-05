using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using NLog;

namespace FSPAirnavDatabaseExporter
{
    public class CsvReader
    {
        private StreamReader reader;
        private DataTable table;

        public CsvReader()
        {
            CreateLogger();
        }

        private static Logger log;
        private static void CreateLogger()
        {
            log = LogManager.GetCurrentClassLogger();
        }

        public DataTable ReadFile(String filename)
        {
            reader = new StreamReader(File.OpenRead(filename));
            table = new DataTable();
            bool addRows = true;

            log.Info("Opened: {0}", filename);

            while (!reader.EndOfStream)
            {
                String lines = reader.ReadLine();
                String[] values = lines.Split(',');

                if (addRows)
                {
                    foreach (string s in values)
                    {
                        if (s != "")
                        table.Columns.Add(s.Replace(@"""", ""), typeof(string));
                    }
                    addRows = false;
                }
                else
                {
                    DataRow row = table.NewRow();
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        row[i] = values[i].Replace(@"""", "");
                    }

                    table.Rows.Add(row);

                    log.Info("Add Row: {0}", values[0]);
                }

            }
            return table;
        }
    }
}