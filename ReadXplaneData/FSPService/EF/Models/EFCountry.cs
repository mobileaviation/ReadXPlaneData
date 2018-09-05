using System;
using ConsoleReadXplaneData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Countries")]
    public class EFCountry
    {
        [Key]
        public Int32 _id { get; set; }
        public Int32 id { get; set; }
        public String code { get; set; }
        public String name { get; set; }
        public String continent { get; set; }
        public String wikipedia_link { get; set; }
        public String keywords { get; set; }
    }

    public static class CountryFactory
    {
        public static EFCountry GetCountryFromDatatable(DataRow countryData)
        {
            try
            {
                EFCountry a = new EFCountry();

                a.id = Xs.ToInt(countryData, "id", 0);
                a.code = countryData["code"].ToString();
                a.name = countryData["name"].ToString();
                a.continent = countryData["continent"].ToString();
                a.wikipedia_link = countryData["wikipedia_link"].ToString();
                a.keywords = countryData["keywords"].ToString();

                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
