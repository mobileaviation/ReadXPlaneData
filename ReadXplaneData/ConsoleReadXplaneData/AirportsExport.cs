using ConsoleReadXplaneData.Models;
using FSPAirnavDatabaseExporter;
using FSPAirnavDatabaseExporter.MBTiles;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData
{
    public class AirportsExport
    {
        public AirportsExport(String basepath)
        {
            this.basePath = basepath;
            log = LogManager.GetCurrentClassLogger();
        }

        private Logger log;

        private String basePath;

        private DataTable airportsTable;
        private DataTable runwaysTable;
        private DataTable frequenciesTable;
        private DataTable countriesTable;
        private DataTable regionsTable;
        private DataTable firsTable;
        private DataTable navaidsTable;
        private DataTable mbtilesTable;
        private DataTable fixesTable;

        private void readAirports()
        {
            CsvReader csvReader = new CsvReader();
            airportsTable = csvReader.ReadFile(basePath + "airports.csv");
        }

        private void readRunways()
        {
            CsvReader csvReader = new CsvReader();
            runwaysTable = csvReader.ReadFile(basePath + "runways.csv");
        }

        private void readFrequencies()
        {
            CsvReader csvReader = new CsvReader();
            frequenciesTable = csvReader.ReadFile(basePath + "airport-frequencies.csv");
        }

        private void readRegions()
        {
            CsvReader csvReader = new CsvReader();
            regionsTable = csvReader.ReadFile(basePath + "regions.csv");
        }

        private void readCountries()
        {
            CsvReader csvReader = new CsvReader();
            countriesTable = csvReader.ReadFile(basePath + "countries.csv");
        }

        private void readFirs()
        {
            CsvReader csvReader = new CsvReader();
            firsTable = csvReader.ReadFile(basePath + "fir.csv");
        }

        private void readNavaids()
        {
            CsvReader csvReader = new CsvReader();
            navaidsTable = csvReader.ReadFile(basePath + "navaids.csv");
        }

        private void readMBTiles()
        {
            ReadMBTiles reader = new ReadMBTiles();
            mbtilesTable = reader.Process();
        }

        private void readFixes()
        {
            XPlaneReader xplaneReader;
            xplaneReader = new XPlaneReader();
            fixesTable = xplaneReader.ReadFixFile(basePath + "earth_fix.dat");
        }

        private String getStatistics()
        {
            Statistics s = new Statistics();
            s.AirportsCount = airportsTable.Rows.Count;

            return s.getJson();
        }

        private void createJson(String filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                file.Write("{");
                file.Write(((char)34) + "statistics" + ((char)34) + " : " + getStatistics() + ",");

                file.Write(((char)34) + "airports" + ((char)34) + " : {");
                addAirportsToJson(file);



                file.Write("} }");
                file.Close();

                //airportsJson = airportsJson.TrimEnd(',') + "}";
                //File.WriteAllText(jsonFile, airportsJson);
                log.Info("Json File: {0} writen", filename);
            }
        }

        private StreamWriter addAirportsToJson(StreamWriter file)
        {
            float count = airportsTable.Rows.Count;
            float pos = 0;
            int index = 0;

            foreach (DataRow R in airportsTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Airport airport = AirportFactory.GetAirportFromDatatable(R, index);

                String json = "";

                if (airport != null)
                {
                    var runways = from rw in runwaysTable.AsEnumerable()
                                  where rw.Field<String>("airport_ref") == airport.id.ToString()
                                  select rw;

                    airport = AirportFactory.AddRunways(runways, airport);

                    var frequencies = from rw in frequenciesTable.AsEnumerable()
                                      where rw.Field<String>("airport_ref") == airport.id.ToString()
                                      select rw;

                    airport = AirportFactory.AddFrequencies(frequencies, airport);

                    json = ((char)34) + index.ToString() + ((char)34) + " : " + AirportFactory.GetJsonAirportFromDatatable(airport);
                    index++;


                    file.Write(json);
                    if (pos < count) file.Write(",");
                }

                //log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), R["ident"].ToString(), R["name"].ToString());
                log.Info("Progress: {0}%", Math.Abs(progress));

            }

            return file;
        }


        public void CreateAirportJson(String filename)
        {
            log.Info("Read Airports");
            readAirports();
            log.Info("Airports Read");
            log.Info("Read Runways");
            readRunways();
            log.Info("Runways Read");
            log.Info("Read Frequencies");
            readFrequencies();
            log.Info("Frequencies Read");
            readCountries();
            log.Info("Countries Read");
            readRegions();
            log.Info("Regions Read");
            readFirs();
            log.Info("Firs Read");
            readNavaids();
            log.Info("Navaids Read");

            createJson(filename);
        }

    }

    public class Statistics
    {
        public Int32 AirportsCount;
        public Int32 NavaidsCount;
        public Int32 FixesCount;
        public Int32 MBTilesCount;
        public Int32 PropertiesCount;

        public String getJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
