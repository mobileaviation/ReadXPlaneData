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
            s.NavaidsCount = navaidsTable.Rows.Count;
            s.CountriesCount = countriesTable.Rows.Count;
            s.RegionsCount = regionsTable.Rows.Count;
            s.FixesCount = fixesTable.Rows.Count;
            s.MBTilesCount = mbtilesTable.Rows.Count;
            s.FirsCount = firsTable.Rows.Count;
            s.AirspacesCount = 15976;
            s.PropertiesCount = 0;
            s.Version = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));//20180525;



            return s.getJson();
        }

        private void createJson(String filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                file.WriteLine("{");
                file.WriteLine(((char)34) + "statistics" + ((char)34) + " : " + getStatistics() + ",");

                file.Write(((char)34) + "airports" + ((char)34) + " : {");
                addAirportsToJson(file);
                file.WriteLine("},");

                file.WriteLine(((char)34) + "navaids" + ((char)34) + " : {");
                addNavaidsToJson(file);
                file.WriteLine("},");

                file.WriteLine(((char)34) + "regions" + ((char)34) + " : {");
                addRegionsToJson(file);
                file.WriteLine("},");

                file.WriteLine(((char)34) + "countries" + ((char)34) + " : {");
                addCountriesToJson(file);
                file.WriteLine("},");

                file.WriteLine(((char)34) + "fixes" + ((char)34) + " : {");
                addFixesToJson(file);
                file.WriteLine("},");

                file.WriteLine(((char)34) + "firs" + ((char)34) + " : {");
                addFirsToJson(file);
                file.WriteLine("},");

                file.WriteLine(((char)34) + "tiles" + ((char)34) + " : {");
                addMBTilesToJson(file);

                file.WriteLine("}}");
                file.Close();

                log.Info("Json File: {0} writen", filename);
            }
        }

        private StreamWriter addNavaidsToJson(StreamWriter file)
        {
            float count = navaidsTable.Rows.Count;
            float pos = 0;
            int index = 0;

            foreach (DataRow R in navaidsTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Navaid navaid = NavaidFactory.GetNavaidFromDatatable(R, index);

                String json = "";

                if (navaid != null)
                {
                    json = ((char)34) + index.ToString() + ((char)34) + " : " + NavaidFactory.GetJsonNavaidFromDatatable(navaid);
                    index++;

                    file.Write(json);
                    if (pos < count) file.Write(",");
                }

                //log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), R["ident"].ToString(), R["name"].ToString());
                log.Info("Navaid Progress: {0}%", Math.Abs(progress));
            }

            return file;
        }

        private StreamWriter addRegionsToJson(StreamWriter file)
        {
            float count = regionsTable.Rows.Count;
            float pos = 0;
            int index = 0;

            foreach (DataRow R in regionsTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Region region = RegionFactory.GetRegionFromDatatable(R, index);

                String json = "";

                if (region != null)
                {
                    json = ((char)34) + index.ToString() + ((char)34) + " : " + RegionFactory.GetJsonRegionFromDatatable(region);
                    index++;

                    file.Write(json);
                    if (pos < count) file.Write(",");
                }

                //log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), R["ident"].ToString(), R["name"].ToString());
                log.Info("Region Progress: {0}%", Math.Abs(progress));
            }

            return file;
        }

        private StreamWriter addMBTilesToJson(StreamWriter file)
        {
            float count = mbtilesTable.Rows.Count;
            float pos = 0;
            int index = 0;

            foreach (DataRow R in mbtilesTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Tile tile = TileFactory.GetTileFromDatatable(R, index);

                String json = "";

                if (tile != null)
                {
                    json = ((char)34) + index.ToString() + ((char)34) + " : " + TileFactory.GetJsonTileFromDatatable(tile);
                    index++;

                    file.Write(json);
                    if (pos < count) file.Write(",");
                }

                //log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), R["ident"].ToString(), R["name"].ToString());
                log.Info("Tile Progress: {0}%", Math.Abs(progress));
            }

            return file;
        }

        private StreamWriter addCountriesToJson(StreamWriter file)
        {
            float count = countriesTable.Rows.Count;
            float pos = 0;
            int index = 0;

            foreach (DataRow R in countriesTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Country country = CountryFactory.GetCountryFromDatatable(R, index);

                String json = "";

                if (country != null)
                {
                    json = ((char)34) + index.ToString() + ((char)34) + " : " + CountryFactory.GetJsonCountryFromDatatable(country);
                    index++;

                    file.Write(json);
                    if (pos < count) file.Write(",");
                }

                //log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), R["ident"].ToString(), R["name"].ToString());
                log.Info("Country Progress: {0}%", Math.Abs(progress));
            }

            return file;
        }

        private StreamWriter addFixesToJson(StreamWriter file)
        {
            float count = fixesTable.Rows.Count;
            float pos = 0;
            int index = 0;

            foreach (DataRow R in fixesTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Fix fix = FixFactory.GetFixFromDatatable(R, index);

                String json = "";

                if (fix != null)
                {
                    json = ((char)34) + index.ToString() + ((char)34) + " : " + FixFactory.GetJsonFixFromDatatable(fix);
                    index++;

                    file.Write(json);
                    if (pos < count) file.Write(",");
                }

                //log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), R["ident"].ToString(), R["name"].ToString());
                log.Info("Fix Progress: {0}%", Math.Abs(progress));
            }

            return file;
        }

        private StreamWriter addFirsToJson(StreamWriter file)
        {
            float count = firsTable.Rows.Count;
            float pos = 0;
            int index = 0;

            foreach (DataRow R in firsTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Fir fir = FirFactory.GetFirFromDatatable(R, index);

                String json = "";

                if (fir != null)
                {
                    json = ((char)34) + index.ToString() + ((char)34) + " : " + FirFactory.GetJsonTileFromDatatable(fir);
                    index++;

                    file.Write(json);
                    if (pos < count) file.Write(",");
                }

                //log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), R["ident"].ToString(), R["name"].ToString());
                log.Info("Fir Progress: {0}%", Math.Abs(progress));
            }

            return file;
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
                log.Info("Airports Progress: {0}%", Math.Abs(progress));

            }

            return file;
        }

        private void readCsvFiles()
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
            readFixes();
            log.Info("Fixes Read");
            readMBTiles();
            log.Info("MBTiles Read");
        }


        public void CreateAirportJson(String filename)
        {
            readCsvFiles();
            createJson(filename);
        }

        public void CreateJsonFiles()
        {
            readCsvFiles();

            float count = airportsTable.Rows.Count;
            List<Airport> airports = new List<Airport>();
            float pos = 0;

            foreach (DataRow R in airportsTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Airport airport = AirportFactory.GetAirportFromDatatable(R, 0);
                airports.Add(airport);
            }

            log.Info("Created Airports List");

            String json = JsonConvert.SerializeObject(airports, Formatting.Indented);

            using (StreamWriter file = File.CreateText(basePath + "airports.json"))
            {
                file.Write(json);
                file.Close();
                log.Info("Created Airports.json file");
            }

            count = frequenciesTable.Rows.Count;
            List<Frequency> frequencies = new List<Frequency>();
            pos = 0;

            foreach (DataRow R in frequenciesTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Frequency frequency = FrequencyFactory.GetFrequencyFromDatatable(R);
                frequencies.Add(frequency);
            }

            log.Info("Created Frequencies List");

            json = JsonConvert.SerializeObject(frequencies, Formatting.Indented);

            using (StreamWriter file = File.CreateText(basePath + "frequencies.json"))
            {
                file.Write(json);
                file.Close();
                log.Info("Created frequencies.json file");
            }

            count = runwaysTable.Rows.Count;
            List<Runway> runways = new List<Runway>();
            pos = 0;

            foreach (DataRow R in runwaysTable.Rows)
            {
                float progress = (pos / count) * 100;
                pos = pos + 1;

                Runway runway = RunwayFactory.GetRunwayFromDatatable(R);
                runways.Add(runway);
            }

            log.Info("Created Runways List");

            json = JsonConvert.SerializeObject(runways, Formatting.Indented);

            using (StreamWriter file = File.CreateText(basePath + "runways.json"))
            {
                file.Write(json);
                file.Close();
                log.Info("Created runways.json file");
            }

        }
    }

    public class Statistics
    {
        public Int32 AirportsCount;
        public Int32 AirspacesCount;
        public Int32 NavaidsCount;
        public Int32 FixesCount;
        public Int32 MBTilesCount;
        public Int32 PropertiesCount;
        public Int32 CountriesCount;
        public Int32 RegionsCount;
        public Int32 FirsCount;
        public Int32 Version;

        public String getJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
