using ConsoleReadXplaneData.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Regions")]
    public class EFRegion
    {
        [Key]
        public Int32 _id { get; set; }
        public Int32 id { get; set; }
        public String code { get; set; }
        public String local_code { get; set; }
        public String name { get; set; }
        public String continent { get; set; }
        public String iso_country { get; set; }
        public String wikipedia_link { get; set; }
        public String keywords { get; set; }
    }

    public static class RegionFactory
    {
        public static EFRegion GetRegionFromDatatable(DataRow regionData)
        {
            try
            {
                EFRegion a = new EFRegion();

                a.id = Xs.ToInt(regionData, "id", 0);
                a.code = regionData["code"].ToString();
                a.local_code = regionData["local_code"].ToString();
                a.iso_country = regionData["iso_country"].ToString();
                a.name = regionData["name"].ToString();
                a.continent = regionData["continent"].ToString();
                a.wikipedia_link = regionData["wikipedia_link"].ToString();
                a.keywords = regionData["keywords"].ToString();

                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
