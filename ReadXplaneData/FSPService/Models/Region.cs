using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Region
    {
        public Int32 index { get; set; }
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
        public static Region GetRegionFromDatatable(DataRow regionData, int Index)
        {
            try
            {
                Region a = new Region();

                a.index = Index;
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

        public static String GetJsonRegionFromDatatable(DataRow data, Int32 index)
        {
            Region region = GetRegionFromDatatable(data, index);
            return JsonConvert.SerializeObject(region, Formatting.Indented);
        }

        public static String GetJsonRegionFromDatatable(Region region)
        {
            return JsonConvert.SerializeObject(region, Formatting.Indented);
        }
    }
}
