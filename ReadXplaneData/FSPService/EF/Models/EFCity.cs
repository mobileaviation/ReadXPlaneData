using ConsoleReadXplaneData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.EF.Models
{
    [Table("tbl_Cities")]
    public class EFCity
    {
        //geonameid,name,asciiname,alernatenames,latitude,longitude,feature_class,feature_code,country_code,cc2," +
        //        "admin1_code,admin2_code,admin3_code,admin4_code,population,elevation,dem,timezone,modification_date

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 geonameid { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        [Index]
        public String name { get; set; }
        public String asciiname { get; set; }
        public String alternatenames { get; set; }
        [Index]
        public Double latitude { get; set; }
        [Index]
        public Double longitude { get; set; }
        public String feature_class { get; set; }
        public String feature_code { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        [Index]
        public String country_code { get; set; }
        public String cc2 { get; set; }
        public String admin1_code { get; set; }
        public String admin2_code { get; set; }
        public String admin3_code { get; set; }
        public String admin4_code { get; set; }
        public Int32 population { get; set; }
        public Int32 elevation { get; set; }
        public String dem { get; set; }
        public String timezone { get; set; }
        public DateTime modification_date { get; set; }

    }

    public static class CityFactory
    {
        public static EFCity GetCityFromDatatable(DataRow cityData)
        {
            try
            {
                EFCity a = new EFCity();

                a.geonameid = Xs.ToInt(cityData, "geonameid", -1);
                a.name = cityData["name"].ToString();
                a.alternatenames = cityData["alternatenames"].ToString();
                a.asciiname = cityData["asciiname"].ToString();
                a.latitude = Xs.ToDouble(cityData, "latitude", 0);
                a.longitude = Xs.ToDouble(cityData, "longitude", 0);
                a.feature_class = cityData["feature_class"].ToString();
                a.feature_code = cityData["feature_code"].ToString();
                a.country_code = cityData["country_code"].ToString();
                a.cc2 = cityData["cc2"].ToString();
                a.admin1_code = cityData["admin1_code"].ToString();
                a.admin2_code = cityData["admin2_code"].ToString();
                a.admin3_code = cityData["admin3_code"].ToString();
                a.admin4_code = cityData["admin4_code"].ToString();
                a.population = Xs.ToInt(cityData, "population", 0);
                a.elevation = Xs.ToInt(cityData, "elevation", 0);
                a.dem = cityData["dem"].ToString();
                a.timezone = cityData["timezone"].ToString();
                a.modification_date = DateTime.ParseExact(cityData["modification_date"].ToString(), "yyyy-MM-dd", null);

                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
