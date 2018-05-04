using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Navaid
    {
        public Int32 index;
        public Int32 id;
        public String filename;
        public String ident;
        public String name;
        public String type;
        public Double frequency_khz;
        public Double latitude_deg;
        public Double longitude_deg;
        public Int32 elevation_ft;
        public String iso_country;
        public Double dme_frequency_khz;
        public Double dme_channel;
        public Double dme_latitude_deg;
        public Double dme_longitude_deg;
        public Int32 dme_elevation_ft;
        public Double slaved_variation_deg;
        public Double magnetic_variation_deg;
        public String usageType;
        public String power;
        public String associated_airport;
        public Int32 associated_airport_id;
    }

    public static class NavaidFactory
    {
        public static Navaid GetNavaidFromDatatable(DataRow navaidData, int Index)
        {
            try
            {
                Navaid a = new Navaid();

                a.index = Index;
                a.id = Xs.ToInt(navaidData, "id", 0);
                a.filename = navaidData["filename"].ToString();
                a.ident = navaidData["ident"].ToString();
                a.name = navaidData["name"].ToString();
                a.type = navaidData["type"].ToString();
                a.frequency_khz = Xs.ToDouble(navaidData, "frequency_khz", 0);
                a.latitude_deg = Xs.ToDouble(navaidData, "latitude_deg", 0);
                a.longitude_deg = Xs.ToDouble(navaidData, "longitude_deg", 0);
                a.elevation_ft = Xs.ToInt(navaidData, "elevation_ft", 0);
                a.iso_country = navaidData["iso_country"].ToString();
                a.dme_frequency_khz = Xs.ToDouble(navaidData, "dme_frequency_khz", 0);
                a.dme_channel = Xs.ToDouble(navaidData, "dme_channel", 0);
                a.dme_latitude_deg = Xs.ToDouble(navaidData, "dme_latitude_deg", 0);
                a.dme_longitude_deg = Xs.ToDouble(navaidData, "dme_longitude_deg", 0);
                a.dme_elevation_ft = Xs.ToInt(navaidData, "dme_elevation_ft", 0); 
                a.slaved_variation_deg = Xs.ToDouble(navaidData, "slaved_variation_deg", 0);
                a.magnetic_variation_deg = Xs.ToDouble(navaidData, "magnetic_variation_deg", 0);
                a.usageType = navaidData["usageType"].ToString();
                a.power = navaidData["power"].ToString();
                a.associated_airport = navaidData["associated_airport"].ToString();
                a.associated_airport_id = Xs.ToInt(navaidData, "associated_airport_id", 0); 

                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        public static String GetJsonNavaidFromDatatable(DataRow data, Int32 index)
        {
            Navaid navaid = GetNavaidFromDatatable(data, index);
            return JsonConvert.SerializeObject(navaid, Formatting.Indented);
        }

        public static String GetJsonNavaidFromDatatable(Navaid navaid)
        {
            return JsonConvert.SerializeObject(navaid, Formatting.Indented);
        }
    }
}
