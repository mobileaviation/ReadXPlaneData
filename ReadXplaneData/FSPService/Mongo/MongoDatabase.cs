using ConsoleReadXplaneData.Models;
using FSPAirnavDatabaseExporter;
using MongoDB.Bson;
using MongoDB.Driver;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Mongo
{
    public class MongoDatabase
    {
        private Logger log;

        public MongoDatabase(String basePath)
        {
            log = LogManager.GetCurrentClassLogger();
            String connectionString = "mongodb://localhost:27016";

            try
            {
                mongoClient = new MongoClient(connectionString);
                log.Info("MongoDB connected to: {0}", connectionString);
                csvDatabases = new CsvDatabases(basePath);
            }
            catch(Exception ee)
            {
                log.Error(ee, "Problem creating an instance off MongoClient");
            }
        }

        private MongoClient mongoClient;
        private CsvDatabases csvDatabases;

        public void StartProcess(List<ImportTypes> importTypes)
        {
            csvDatabases.ProcessCsvFiles(importTypes);
            IMongoDatabase db = mongoClient.GetDatabase("airnavdb");
            InsertAirports(db);
        }

        private void InsertAirports(IMongoDatabase db)
        {
            float progress = 0;
            int index = 0;

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("airports");

            foreach (DataRow row in csvDatabases.airportsTable.Rows)
            {
                try
                {
                    var doc = AirportFactory.GetAirportBsonDocFromDatatable(row, index++);
                    collection.InsertOne(doc);
                    log.Info("Inserted Airport: {0}", row["name"].ToString());
                }
                catch(Exception ee)
                {
                    log.Error(ee, "Error insering airport");
                }
            }
        }
    }
}
