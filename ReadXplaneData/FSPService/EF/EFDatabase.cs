using ConsoleReadXplaneData.EF.Models;
using FSPAirnavDatabaseExporter;
using FSPAirnavDatabaseExporter.MBTiles;
using FSPService;
using FSPService.Compression;
using FSPService.EF.Models;
using FSPService.Enums;
using FSPService.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private DataTable citesTable;

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

        private void clearDatabase(AirNavDB airportsDb)
        {
            var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)airportsDb).ObjectContext;
            
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Runways");
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Frequencies");
            objCtx.ExecuteStoreCommand("DELETE FROM tbl_Airports");
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Cities");
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Countries");
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Firs");
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Fixes");
            
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Navaids");
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Regions");
            
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_Tiles");
        }

        private void clearAirspacesTables(AirNavDB airportsDb)
        {
            var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)airportsDb).ObjectContext;
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_ActiveDays");
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_ActivePeriods");
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE tbl_ATCStations");
            objCtx.ExecuteStoreCommand("DELETE FROM tbl_Airspaces");
        }
        

        public void Process(List<ImportTypes> importTypes)
        {
            readCsvFiles(importTypes);
            using (AirNavDB airportsDb = new AirNavDB())
            {
                clearDatabase(airportsDb);

                airportsDb.Configuration.AutoDetectChangesEnabled = false;
                //airportsDb.Configuration.ValidateOnSaveEnabled = false;
                if (importTypes.Contains(ImportTypes.airports))
                {
                    InsertAirports(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.runways))
                {
                    InsertRunways(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.frequencies))
                {
                    InsertFrequnecies(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.navaids))
                {
                    InsertNavaids(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.firs))
                {
                    InsertFirs(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.fixes))
                {
                    InsertFixes(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.countries))
                {
                    InsertCountries(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.regions))
                { 
                    InsertRegions(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.mbtiles))
                {
                    InsertTiles(airportsDb);
                    airportsDb.SaveChanges();
                }
                if (importTypes.Contains(ImportTypes.cities5000))
                {
                    InsertCities(airportsDb);
                    airportsDb.SaveChanges();
                }

            }
        }

        public void ProcessAirspaces(List<EFLink> links)
        {
            using (AirNavDB airportsDb = new AirNavDB())
            {
                clearAirspacesTables(airportsDb);
            }

            foreach (EFLink link in links)
            {
                Airspaces airspaces = new Airspaces();
                AirspaceFileType fileType = (link.openaip_enabled)? AirspaceFileType.openaip : AirspaceFileType.openair;

                airspaces.processAirspaceFile(link.local_filename, link.country, fileType);

                using (AirNavDB airportsDb = new AirNavDB())
                {
                    InsertAirspaces(airportsDb, airspaces);
                }
            }
            
        }

        public void InsertAirports(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            
            foreach (DataRow row in airportsTable.Rows)
            {
                EFAirport airport = AirportFactory.GetAirportFromDatatable(row);
                //var q = from ap in db.airports
                //        where ap.id == airport.id
                //        select ap;
                //var check_airport = q.FirstOrDefault<EFAirport>();

                //if (check_airport == null)
                //{
                    db.airports.Add(airport);
                    log.Info("Add Airport: {0} to database", airport.name);
                //}

                progress = ((float)index++ / (float)airportsTable.Rows.Count) * 100;
                    
                log.Info("Progress: {0}", progress);

                if ((index % 100) == 0) db.SaveChanges();

            }
        }
        public void InsertRunways(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in runwaysTable.Rows)
            {
                EFRunway runway = RunwayFactory.GetRunwayFromDatatable(row);

                //if (!db.runways.Any(o => o.id == runway.id))
                //{
                    db.runways.Add(runway);
                    log.Info("Add Runways: {0} to database", runway.airport_ident);
                //db.SaveChanges();
                //}

                progress = ((float)index++ / (float)runwaysTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);
                if ((index % 100) == 0) db.SaveChanges();

            }
        }
        public void InsertFrequnecies(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in frequenciesTable.Rows)
            {
                EFFrequency frequency = FrequencyFactory.GetFrequencyFromDatatable(row);

                //if (!db.frequencies.Any(o => o.id == frequency.id))
                //{
                    db.frequencies.Add(frequency);
                    log.Info("Add Frequency: {0} to database", frequency.airport_ident);
                //}

                progress = ((float)index++ / (float)frequenciesTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);
                if ((index % 100) == 0) db.SaveChanges();

            }
        }
        public void InsertNavaids(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in navaidsTable.Rows)
            {
                EFNavaid navaid = NavaidFactory.GetNavaidFromDatatable(row);
                //var q = from ap in db.navaids
                //        where ap.id == navaid.id
                //        select ap;
                //var check_navaid = q.FirstOrDefault<EFNavaid>();

                //if (check_navaid == null)
                //{
                    db.navaids.Add(navaid);
                    log.Info("Add Navaid: {0} to database", navaid.filename);
                //}

                progress = ((float)index++ / (float)navaidsTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);
                if ((index % 100) == 0) db.SaveChanges();

            }
        }

        public void InsertFirs(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in firsTable.Rows)
            {
                EFFir fir = FirFactory.GetFirFromDatatable(row);
                //var q = from ap in db.firs
                //        where ap.id == fir.id
                //        select ap;
                //var check_fir = q.FirstOrDefault<EFFir>();

                //if (check_fir == null)
                //{
                    db.firs.Add(fir);
                    log.Info("Add Fir: {0} to database", fir.name);
                //}

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
                if ((index % 100) == 0) db.SaveChanges();

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
                //var q = from ap in db.countries
                //        where ap.code == country.code
                //        select ap;
                //var check_country = q.FirstOrDefault<EFCountry>();

                //if (check_country == null)
                //{
                    db.countries.Add(country);
                    log.Info("Add Country: {0} to database", country.name);
                //}

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
                //var q = from ap in db.regions
                //        where ap.code == region.code
                //        select ap;
                //var check_region = q.FirstOrDefault<EFRegion>();

                //if (check_region == null)
                //{
                    db.regions.Add(region);
                    log.Info("Add Region: {0} to database", region.name);
                //}

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
                //var q = from ap in db.tiles
                //        where ap.name == tile.name
                //        select ap;
                //var check_tile = q.FirstOrDefault<EFTile>();

                //if (check_tile == null)
                //{
                    db.tiles.Add(tile);
                    log.Info("Add Tile: {0} to database", tile.name);
                //}

                progress = ((float)index++ / (float)mbtilesTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
            db.SaveChanges();
        }

        public void InsertCities(AirNavDB db)
        {
            float progress = 0;
            int index = 0;

            foreach (DataRow row in citesTable.Rows)
            {
                EFCity city = CityFactory.GetCityFromDatatable(row);
                //var q = from ap in db.tiles
                //        where ap.name == tile.name
                //        select ap;
                //var check_tile = q.FirstOrDefault<EFTile>();

                //if (check_tile == null)
                //{
                db.cities.Add(city);
                log.Info("Add City: {0} to database", city.name);
                //}

                progress = ((float)index++ / (float)citesTable.Rows.Count) * 100;

                log.Info("Progress: {0}", progress);

            }
            db.SaveChanges();
        }

        public void InsertAirspaces(AirNavDB db, Airspaces airspaces)
        {
            float progress = 0;
            int index = 0;

            foreach (Airspace airspace in airspaces)
            {
                EFAirspace efairspace = AirspaceFactory.GetEFAirspace(airspace);

                foreach (ATCStation atcStation in airspace.atcStations)
                {
                    efairspace.atcStations.Add(AirspaceFactory.GetEFATCStation(atcStation));
                }

                foreach (ActiveDay activeDay in airspace.activeDays)
                {
                    efairspace.activeDays.Add(AirspaceFactory.GetEFActiveDay(activeDay));
                }

                foreach (ActivePeriod activePeriod in airspace.activePeriods)
                {
                    efairspace.activePeriods.Add(AirspaceFactory.GetEFActivePeriod(activePeriod));
                }

                //EFATCStation atcStation = AirspaceFactory.GetEFATCStation()
                //var q = from ap in db.airspaces
                //        where ap.name == efairspace.name
                //        select ap;
                //var check_airspace = q.FirstOrDefault<EFAirspace>();

                //if (check_airspace == null)
                //{
                db.airspaces.Add(efairspace);
                    log.Info("Add Airspace: {0} to database", efairspace.name);
                //}

                progress = ((float)index++ / (float)airspaces.Count) * 100;

                log.Info("Progress: {0}", progress);
                if ((index % 100) == 0) db.SaveChanges();

            }
            db.SaveChanges();
        }
    }
}
