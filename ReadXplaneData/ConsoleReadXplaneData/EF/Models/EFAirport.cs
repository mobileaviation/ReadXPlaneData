using ConsoleReadXplaneData.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Airports")]
    public class EFAirport
    {
        [Key]
        public Int32 _id { get; set; }
        public Int32 id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        [Index]
        public String ident { get; set; }
        public String type { get; set; }
        public String name { get; set; }
        [Index]
        public Double latitude_deg { get; set; }
        [Index]
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
    }

    public static class AirportFactory
    {
        public static EFAirport GetAirportFromDatatable(DataRow airportData)
        {
            try
            {
                EFAirport a = new EFAirport();

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
    }
}
