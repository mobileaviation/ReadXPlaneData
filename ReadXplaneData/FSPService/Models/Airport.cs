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

        public Int32 index;
        public Int32 id;
        public String ident;
        public String type;
        public String name;
        public Double latitude_deg;
        public Double longitude_deg;
        public Double elevation_ft;
        public String continent;
        public String iso_country;
        public String iso_region;
        public String municipality;
        public String scheduled_service;
        public String gps_code;
        public String iata_code;
        public String local_code;
        public String home_link;
        public String wikipedia_link;
        public String keywords;

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
