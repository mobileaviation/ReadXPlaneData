
using FSPAirnavDatabaseExporter;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ConsoleReadXplaneData.Models;
using System.Data;
using FSPService.EF.Models;
using ConsoleReadXplaneData.EF.Models;

namespace FSPService.MySql
{
    public class MyDatabase
    {
        public MyDatabase(String basepath)
        {
            this.basePath = basepath;
            log = LogManager.GetCurrentClassLogger();
            csvDatabases = new CsvDatabases(basepath);
        }

        private Logger log;
        private String basePath;
        private CsvDatabases csvDatabases;

        public void Process(List<ImportTypes> importTypes)
        {
            csvDatabases.ProcessCsvFiles(importTypes);

            String connStr = "server=localhost;port=3306;database=airnavdb;uid=root;password=@Ko218493";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            if (importTypes.Contains(ImportTypes.airports))
            {
                InsertAirports(conn);
            }
            if (importTypes.Contains(ImportTypes.runways))
            {
                InsertRunways(conn);
            }
            if (importTypes.Contains(ImportTypes.frequencies))
            {
                InsertFrequnecies(conn);
            }
            if (importTypes.Contains(ImportTypes.navaids))
            {
                InsertNavaids(conn);
            }
            if (importTypes.Contains(ImportTypes.firs))
            {
                InsertFirs(conn);
            }
            if (importTypes.Contains(ImportTypes.fixes))
            {
                InsertFirs(conn);
            }
            if (importTypes.Contains(ImportTypes.countries))
            {
                InsertCountries(conn);
            }
            if (importTypes.Contains(ImportTypes.regions))
            {
                InsertRegions(conn);
            }
            if (importTypes.Contains(ImportTypes.mbtiles))
            {
                InsertTiles(conn);
            }
            if (importTypes.Contains(ImportTypes.cities5000))
            {
                InsertCities(conn);
            }

            conn.Close();
        }

        private void InsertCities(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertCities;

            foreach (DataRow row in csvDatabases.citesTable.Rows)
            {
                EFCity city = CityFactory.GetCityFromDatatable(row);
                log.Info("Add City: {0} to database", city.name);
                conn.Execute(insertSql, city);
                progress = ((float)index++ / (float)csvDatabases.citesTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }

        private void InsertTiles(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertTiles;

            foreach (DataRow row in csvDatabases.mbtilesTable.Rows)
            {
                EFTile tile = ConsoleReadXplaneData.EF.Models.TileFactory.GetTileFromDatatable(row);
                log.Info("Add Tile: {0} to database", tile.name);
                conn.Execute(insertSql, tile);
                progress = ((float)index++ / (float)csvDatabases.mbtilesTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }

        private void InsertRegions(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertRegions;

            foreach (DataRow row in csvDatabases.regionsTable.Rows)
            {
                EFRegion region = ConsoleReadXplaneData.EF.Models.RegionFactory.GetRegionFromDatatable(row);
                log.Info("Add Region: {0} to database", region.name);
                conn.Execute(insertSql, region);
                progress = ((float)index++ / (float)csvDatabases.regionsTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }

        private void InsertCountries(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertCountries;

            foreach (DataRow row in csvDatabases.countriesTable.Rows)
            {
                EFCountry country = ConsoleReadXplaneData.EF.Models.CountryFactory.GetCountryFromDatatable(row);
                log.Info("Add Country: {0} to database", country.name);
                conn.Execute(insertSql, country);
                progress = ((float)index++ / (float)csvDatabases.countriesTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }

        private void InsertFirs(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertFirs;

            foreach (DataRow row in csvDatabases.firsTable.Rows)
            {
                EFFir fir = ConsoleReadXplaneData.EF.Models.FirFactory.GetFirFromDatatable(row);
                log.Info("Add Fir: {0} to database", fir.name);
                conn.Execute(insertSql, fir);
                progress = ((float)index++ / (float)csvDatabases.firsTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }

        private void InsertNavaids(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertNavaids;

            foreach (DataRow row in csvDatabases.navaidsTable.Rows)
            {
                EFNavaid navaid = ConsoleReadXplaneData.EF.Models.NavaidFactory.GetNavaidFromDatatable(row);
                log.Info("Add Navaid: {0} to database", navaid.name);
                
                conn.Execute(insertSql, navaid);
                progress = ((float)index++ / (float)csvDatabases.navaidsTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }

        private void InsertFrequnecies(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertFrequencies;

            foreach (DataRow row in csvDatabases.frequenciesTable.Rows)
            {
                Frequency frequency = ConsoleReadXplaneData.Models.FrequencyFactory.GetFrequencyFromDatatable(row);
                log.Info("Add Frequency: {0} to database", frequency.airport_ident);
                conn.Execute(insertSql, frequency);
                progress = ((float)index++ / (float)csvDatabases.frequenciesTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }

        private void InsertRunways(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertRumways;

            foreach (DataRow row in csvDatabases.runwaysTable.Rows)
            {
                Runway runway = ConsoleReadXplaneData.Models.RunwayFactory.GetRunwayFromDatatable(row);
                log.Info("Add Runway: {0} to database", runway.airport_ident);
                conn.Execute(insertSql, runway);
                progress = ((float)index++ / (float)csvDatabases.runwaysTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }

        private void InsertAirports(MySqlConnection conn)
        {
            float progress = 0;
            int index = 0;
            String insertSql = Commands.InsertAirports;

            foreach (DataRow row in csvDatabases.airportsTable.Rows)
            {
                Airport airport = ConsoleReadXplaneData.Models.AirportFactory.GetAirportFromDatatable(row, index);
                log.Info("Add Airport: {0} to database", airport.name);
                conn.Execute(insertSql, airport);
                progress = ((float)index++ / (float)csvDatabases.airportsTable.Rows.Count) * 100;
                log.Info("Progress: {0}", progress);
            }
        }
    }
}
