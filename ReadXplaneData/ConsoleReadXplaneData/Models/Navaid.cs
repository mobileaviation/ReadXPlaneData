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
        public static Navaid GetBNavaidFromDatatable(DataRow navaidData, int Index)
        {
            try
            {
                Navaid a = new Navaid();
                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
