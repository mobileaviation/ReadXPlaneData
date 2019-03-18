using FSPAirnavDatabaseExporter;
using FSPAirnavDatabaseExporter.MBTiles;
using FSPService.Compression;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService
{
    public class CsvDatabases
    {
        public CsvDatabases(String basePath)
        {
            this.basePath = basePath;
        }

        public void ProcessCsvFiles(List<ImportTypes> importTypes)
        {
            readCsvFiles(importTypes);
        }

        private Logger log;
        private String basePath;

        public DataTable airportsTable;
        public DataTable runwaysTable;
        public DataTable frequenciesTable;
        public DataTable countriesTable;
        public DataTable regionsTable;
        public DataTable firsTable;
        public DataTable navaidsTable;
        public DataTable mbtilesTable;
        public DataTable fixesTable;
        public DataTable citesTable;

        private void readAirports()
        {
            CsvReader csvReader = new CsvReader(delimiter.comma);
            airportsTable = csvReader.ReadFile(basePath + "airports.csv");
        }

        private void readRunways()
        {
            CsvReader csvReader = new CsvReader(delimiter.comma);
            runwaysTable = csvReader.ReadFile(basePath + "runways.csv");
        }

        private void readFrequencies()
        {
            CsvReader csvReader = new CsvReader(delimiter.comma);
            frequenciesTable = csvReader.ReadFile(basePath + "airport-frequencies.csv");
        }

        private void readRegions()
        {
            CsvReader csvReader = new CsvReader(delimiter.comma);
            regionsTable = csvReader.ReadFile(basePath + "regions.csv");
        }

        private void readCountries()
        {
            CsvReader csvReader = new CsvReader(delimiter.comma);
            countriesTable = csvReader.ReadFile(basePath + "countries.csv");
        }

        private void readFirs()
        {
            CsvReader csvReader = new CsvReader(delimiter.comma);
            firsTable = csvReader.ReadFile(basePath + "fir.csv");
        }

        private void readNavaids()
        {
            CsvReader csvReader = new CsvReader(delimiter.comma);
            navaidsTable = csvReader.ReadFile(basePath + "navaids.csv");
        }

        private void readCities()
        {
            CsvReader csvReader = new CsvReader(delimiter.tab);
            csvReader.prepColumns("geonameid,name,asciiname,alternatenames,latitude,longitude,feature_class,feature_code,country_code,cc2," +
                "admin1_code,admin2_code,admin3_code,admin4_code,population,elevation,dem,timezone,modification_date");
            if (File.Exists(basePath + "cities5000.txt")) File.Delete(basePath + "cities5000.txt");
            List<String> files = Unzip.unzipFile(basePath + "cities5000.zip");
            if (files.Contains(basePath + "cities5000.txt"))
                citesTable = csvReader.ReadFile(basePath + "cities5000.txt");
        }

        private void readMBTiles()
        {
            ReadMBTiles reader = new ReadMBTiles();
            mbtilesTable = reader.Process();
            int test = 0;
        }

        private void readFixes()
        {
            XPlaneReader xplaneReader;
            xplaneReader = new XPlaneReader();
            fixesTable = xplaneReader.ReadFixFile(basePath + "earth_fix.dat");
        }

        private void readCsvFiles(List<ImportTypes> importTypes)
        {
            if (importTypes.Contains(ImportTypes.airports))
            {
                readAirports();
            }
            if (importTypes.Contains(ImportTypes.runways))
            {
                readRunways();
            }
            if (importTypes.Contains(ImportTypes.frequencies))
            {
                readFrequencies();
            }
            if (importTypes.Contains(ImportTypes.navaids))
            {
                readNavaids();
            }
            if (importTypes.Contains(ImportTypes.firs))
            {
                readFirs();
            }
            if (importTypes.Contains(ImportTypes.fixes))
            {
                readFixes();
            }
            if (importTypes.Contains(ImportTypes.countries))
            {
                readCountries();
            }
            if (importTypes.Contains(ImportTypes.regions))
            {
                readRegions();
            }
            if (importTypes.Contains(ImportTypes.mbtiles))
            {
                readMBTiles();
            }
            if (importTypes.Contains(ImportTypes.cities5000))
            {
                readCities();
            }
        }
    }
}
