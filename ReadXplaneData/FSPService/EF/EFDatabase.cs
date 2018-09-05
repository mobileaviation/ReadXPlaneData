using ConsoleReadXplaneData.EF.Models;
using FSPAirnavDatabaseExporter;
using FSPAirnavDatabaseExporter.MBTiles;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.EF
{
    public class EFDatabase
    {
        public EFDatabase(String basepath)
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

        private void readCsvFiles()
        {
            //log.Info("Read Airports");
            //readAirports();
            //log.Info("Airports Read");
            //log.Info("Read Runways");
            //readRunways();
            //log.Info("Runways Read");
            //log.Info("Read Frequencies");
            //readFrequencies();
            //log.Info("Frequencies Read");
            readCountries();
            //log.Info("Countries Read");
            readRegions();
            //log.Info("Regions Read");
            readFirs();
            //log.Info("Firs Read");

            //readNavaids();
            //log.Info("Navaids Read");
            //readFixes();
            //log.Info("Fixes Read");
            readMBTiles();
            //log.Info("MBTiles Read");
        }

        public void Process()
        {
            readCsvFiles();
            using (AirNavDB airportsDb = new AirNavDB())
            {
                //InsertAirports(airportsDb);
                //airportsDb.SaveChanges();
                //InsertRunways(airportsDb);
                //airportsDb.SaveChanges();
                //InsertFrequnecies(airportsDb);
                //airportsDb.SaveChanges();
                //InsertNavaids(airportsDb);
                //airportsDb.SaveChanges();
                InsertFirs(airportsDb);
                airportsDb.SaveChanges();
                //InsertFixes(airportsDb);
                //airportsDb.SaveChanges();
                InsertCountries(airportsDb);
                airportsDb.SaveChanges();
                InsertRegions(airportsDb);
                airportsDb.SaveChanges();
                InsertTiles(airportsDb);
                airportsDb.SaveChanges();
            }
        }

        public void InsertAirports(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in airportsTable.Rows)
            {
                EFAirport airport = AirportFactory.GetAirportFromDatatable(row);
                var q = from ap in db.airports
                        where ap.id == airport.id
                        select ap;
                var check_airport = q.FirstOrDefault<EFAirport>();

                if (check_airport == null)
                {
                    db.airports.Add(airport);
                    log.Info("Add Airport: {0} to database", airport.name);
                }

                progress = ((float)index++ / (float)airportsTable.Rows.Count) * 100;
                    
                log.Info("Progress: {0}", progress);
                    
            }
        }
        public void InsertRunways(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in runwaysTable.Rows)
            {
                EFRunway runway = RunwayFactory.GetRunwayFromDatatable(row);
                var q = from ap in db.runways
                        where ap.id == runway.id
                        select ap;
                var check_runway = q.FirstOrDefault<EFRunway>();

                if (check_runway == null)
                {
                    db.runways.Add(runway);
                    log.Info("Add Runways: {0} to database", runway.airport_ident);
                }

                progress = ((float)index++ / (float)runwaysTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
        }
        public void InsertFrequnecies(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in frequenciesTable.Rows)
            {
                EFFrequency frequency = FrequencyFactory.GetFrequencyFromDatatable(row);
                var q = from ap in db.frequencies
                        where ap.id == frequency.id
                        select ap;
                var check_frequency = q.FirstOrDefault<EFFrequency>();

                if (check_frequency == null)
                {
                    db.frequencies.Add(frequency);
                    log.Info("Add Frequency: {0} to database", frequency.airport_ident);
                }

                progress = ((float)index++ / (float)frequenciesTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
        }
        public void InsertNavaids(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in navaidsTable.Rows)
            {
                EFNavaid navaid = NavaidFactory.GetNavaidFromDatatable(row);
                var q = from ap in db.navaids
                        where ap.id == navaid.id
                        select ap;
                var check_navaid = q.FirstOrDefault<EFNavaid>();

                if (check_navaid == null)
                {
                    db.navaids.Add(navaid);
                    log.Info("Add Navaid: {0} to database", navaid.filename);
                }

                progress = ((float)index++ / (float)navaidsTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
        }
        public void InsertFirs(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in firsTable.Rows)
            {
                EFFir fir = FirFactory.GetFirFromDatatable(row);
                var q = from ap in db.firs
                        where ap.id == fir.id
                        select ap;
                var check_fir = q.FirstOrDefault<EFFir>();

                if (check_fir == null)
                {
                    db.firs.Add(fir);
                    log.Info("Add Fir: {0} to database", fir.name);
                }

                progress = ((float)index++ / (float)firsTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
            db.SaveChanges();
        }
        public void InsertFixes(AirNavDB db)
        {
            float progress = 0;
            int index = 0;
            List<EFFix> fixes = new List<EFFix>();

            

            foreach (DataRow row in fixesTable.Rows)
            {
                EFFix fix = FixFactory.GetFixFromDatatable(row);
                //var q = from ap in db.fixes
                //        where ap.ident == fix.ident
                //        select ap;
                //var check_fix = q.FirstOrDefault<EFFix>();

                //if (check_fix == null)
                //{
                fixes.Add(fix);
                    log.Info("Add Fix: {0} to database", fix.ident);
                //}

                progress = ((float)index++ / (float)fixesTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }

            db.fixes.AddRange(fixes);
            db.SaveChanges();

        }
        public void InsertCountries(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in countriesTable.Rows)
            {
                EFCountry country = CountryFactory.GetCountryFromDatatable(row);
                var q = from ap in db.countries
                        where ap.code == country.code
                        select ap;
                var check_country = q.FirstOrDefault<EFCountry>();

                if (check_country == null)
                {
                    db.countries.Add(country);
                    log.Info("Add Country: {0} to database", country.name);
                }

                progress = ((float)index++ / (float)countriesTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
            db.SaveChanges();
        }
        public void InsertRegions(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in regionsTable.Rows)
            {
                EFRegion region = RegionFactory.GetRegionFromDatatable(row);
                var q = from ap in db.regions
                        where ap.code == region.code
                        select ap;
                var check_region = q.FirstOrDefault<EFRegion>();

                if (check_region == null)
                {
                    db.regions.Add(region);
                    log.Info("Add Region: {0} to database", region.name);
                }

                progress = ((float)index++ / (float)regionsTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
            db.SaveChanges();
        }
        public void InsertTiles(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in mbtilesTable.Rows)
            {
                EFTile tile = TileFactory.GetTileFromDatatable(row);
                var q = from ap in db.tiles
                        where ap.name == tile.name
                        select ap;
                var check_tile = q.FirstOrDefault<EFTile>();

                if (check_tile == null)
                {
                    db.tiles.Add(tile);
                    log.Info("Add Tile: {0} to database", tile.name);
                }

                progress = ((float)index++ / (float)mbtilesTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
            db.SaveChanges();
        }
    }
}
