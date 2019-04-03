using FSPService.Classes;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Airport
    {
        public Airport()
        {
            runways = new List<Runway>();
            frequencies = new List<Frequency>();
        }

        public Int32 index { get; set; }
        public Int32 id { get; set; }
        public String ident { get; set; }
        public String type { get; set; }
        public String name { get; set; }
        public Double latitude_deg { get; set; }
        public Double longitude_deg { get; set; }
        public Double elevation_ft { get; set; }
        public String continent { get; set; }
        public String iso_country { get; set; }
        public String iso_region { get; set; }
        public String municipality { get; set; }
        public String scheduled_service { get; set; }
        public String gps_code { get; set; }
        public String iata_code { get; set; }
        public String local_code { get; set; }
        public String home_link { get; set; }
        public String wikipedia_link { get; set; }
        public String keywords { get; set; }

        public List<Runway> runways;
        public List<Frequency> frequencies;
    }

    public static class AirportFactory
    {
        public static Airport GetAirportFromDatatable(DataRow airportData, int Index)
        {
            try
            {
                Airport a = new Airport();

                a.index = Index;
                a.id = Xs.ToInt(airportData, "id", -1); 
                a.ident = airportData["ident"].ToString();
                a.type = airportData["type"].ToString();
                a.name = airportData["name"].ToString();
                a.latitude_deg = Xs.ToDouble(airportData, "latitude_deg", 0); 
                a.longitude_deg = Xs.ToDouble(airportData, "longitude_deg", 0); 
                a.elevation_ft = Xs.ToDouble(airportData, "elevation_ft", 0);
                a.continent = airportData["continent"].ToString();
                a.iso_country = airportData["iso_country"].ToString();
                a.iso_region = airportData["iso_region"].ToString();
                a.municipality = airportData["municipality"].ToString();
                a.scheduled_service = airportData["scheduled_service"].ToString();
                a.gps_code = airportData["gps_code"].ToString();
                a.iata_code = airportData["iata_code"].ToString();
                a.local_code = airportData["local_code"].ToString();
                a.home_link = airportData["home_link"].ToString();
                a.wikipedia_link = airportData["wikipedia_link"].ToString();
                a.keywords = airportData["keywords"].ToString();


                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        public static BsonDocument GetAirportBsonDocFromDatatable(DataRow airportData, int Index)
        {
            try
            {
                BsonDocument doc = new BsonDocument();

                doc.Add("index", Index);
                doc.Add("id", Xs.ToInt(airportData, "id", -1));
                doc.Add("ident", airportData["ident"].ToString());
                doc.Add("type", airportData["type"].ToString());
                doc.Add("name", airportData["name"].ToString());
                doc.Add("latitude_deg", Xs.ToDouble(airportData, "latitude_deg", 0));
                doc.Add("longitude_deg", Xs.ToDouble(airportData, "longitude_deg", 0));
                GeoJsonPoint p = new GeoJsonPoint(Xs.ToDouble(airportData, "latitude_deg", 0),
                     Xs.ToDouble(airportData, "longitude_deg", 0));

                doc.Add("location", p.getPointElement());
                doc.Add("elevation_ft", Xs.ToDouble(airportData, "elevation_ft", 0));
                doc.Add("continent", airportData["continent"].ToString());
                doc.Add("iso_country", airportData["iso_country"].ToString());
                doc.Add("iso_region", airportData["iso_region"].ToString());
                doc.Add("municipality", airportData["municipality"].ToString());
                doc.Add("scheduled_service", airportData["scheduled_service"].ToString());
                doc.Add("gps_code", airportData["gps_code"].ToString());
                doc.Add("iata_code", airportData["iata_code"].ToString());
                doc.Add("local_code", airportData["local_code"].ToString());
                doc.Add("home_link", airportData["home_link"].ToString());
                doc.Add("wikipedia_link", airportData["wikipedia_link"].ToString());
                doc.Add("keywords", airportData["keywords"].ToString());


                return doc;
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        public static Airport AddRunways(EnumerableRowCollection runwayRows, Airport airport)
        {
            foreach(DataRow R in runwayRows)
            {
                Runway runway = RunwayFactory.GetRunwayFromDatatable(R);
                airport.runways.Add(runway);
            }

            return airport;
        }

        public static Airport AddFrequencies(EnumerableRowCollection frequenciesRows, Airport airport)
        {
            foreach (DataRow R in frequenciesRows)
            {
                Frequency frequency = FrequencyFactory.GetFrequencyFromDatatable(R);
                airport.frequencies.Add(frequency);
            }

            return airport;
        }



        public static String GetJsonAirportFromDatatable(DataRow data, Int32 index)
        {
            Airport airport = GetAirportFromDatatable(data, index);
            return JsonConvert.SerializeObject(airport, Formatting.Indented);
        }

        public static String GetJsonAirportFromDatatable(Airport airport)
        {
            return JsonConvert.SerializeObject(airport, Formatting.Indented);
        }

    }
}
