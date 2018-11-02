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
        private delimiter delimiter;

        public CsvReader(delimiter delimiter)
        {
            CreateLogger();
            this.delimiter = delimiter;
        }

        public void prepColumns(String columns)
        {
            this.columns = columns.Split(',');
        }
        private String[] columns;

        private static Logger log;
        private static void CreateLogger()
        {
            log = LogManager.GetCurrentClassLogger();
        }

        public DataTable ReadFile(String filename)
        {
            reader = new StreamReader(File.OpenRead(filename));
            table = new DataTable();
            bool addHeaders = true;;
            char splitchar = ',';
            if (delimiter == delimiter.comma) splitchar = ',';
            if (delimiter == delimiter.tab) splitchar = Convert.ToChar(9);

            if (columns != null)
            {
                addHeaders = false;
                foreach (string s in columns)
                {
                    table.Columns.Add(s, typeof(string));
                }
            }

            log.Info("Opened: {0}", filename);

            while (!reader.EndOfStream)
            {
                String lines = reader.ReadLine();
                String[] values = lines.Split(splitchar);

                if (addHeaders)
                {
                    foreach (string s in values)
                    {
                        if (s != "")
                        table.Columns.Add(s.Replace(@"""", ""), typeof(string));
                    }
                    addHeaders = false;
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

    public enum delimiter
    {
        comma,
        tab
    }
}