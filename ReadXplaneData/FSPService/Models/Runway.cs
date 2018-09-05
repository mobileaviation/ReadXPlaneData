using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Runway
    {
        public Int32 id;
        public Int32 airport_ref;
        public String airport_ident;
        public Int32 length_ft;
        public Int32 width_ft;
        public Int32 lighted;
        public Int32 closed;
        public String surface;
        public String le_ident;
        public Double le_latitude_deg;
        public Double le_longitude_deg;
        public Int32 le_elevation_ft;
        public Double le_heading_degT;
        public Int32 le_displaced_threshold_ft;
        public String he_ident;
        public Double he_latitude_deg;
        public Double he_longitude_deg;
        public Int32 he_elevation_ft;
        public Double he_heading_degT;
        public Int32 he_displaced_threshold_ft;
    }

    public static class RunwayFactory
    {
        public static Runway GetRunwayFromDatatable(DataRow data)
        {
            try
            {
                Runway a = new Runway();

                a.id = Xs.ToInt(data, "id", -1); 
                a.airport_ref = Xs.ToInt(data, "airport_ref", -1); 
                a.airport_ident = data["airport_ident"].ToString();
                a.length_ft = Xs.ToInt(data, "length_ft", -1); 
                a.width_ft = Xs.ToInt(data, "width_ft", -1); 
                a.lighted = Xs.ToInt(data, "lighted", -1); 
                a.closed = Xs.ToInt(data, "closed", -1); 
                a.surface = data["surface"].ToString();
                a.le_ident = data["le_ident"].ToString();
                a.le_latitude_deg = Xs.ToDouble(data, "le_latitude_deg", 0); 
                a.le_longitude_deg = Xs.ToDouble(data, "le_longitude_deg", 0); 
                a.le_elevation_ft = Xs.ToInt(data, "le_elevation_ft", -1); 
                a.le_heading_degT = Xs.ToDouble(data, "le_heading_degT", 0);
                a.le_displaced_threshold_ft = Xs.ToInt(data, "le_displaced_threshold_ft", -1);
                a.he_ident = data["he_ident"].ToString();
                a.he_latitude_deg = Xs.ToDouble(data, "he_latitude_deg", 0); 
                a.he_longitude_deg = Xs.ToDouble(data, "he_longitude_deg", 0);
                a.he_elevation_ft = Xs.ToInt(data, "he_elevation_ft", -1); 
                a.he_heading_degT = Xs.ToDouble(data, "he_heading_degT", 0);
                a.he_displaced_threshold_ft = Xs.ToInt(data, "he_displaced_threshold_ft", -1); 


                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        public static String GetJsonRunwayFromDatatable(DataRow data)
        {
            Runway runway = GetRunwayFromDatatable(data);
            return JsonConvert.SerializeObject(runway, Formatting.Indented);
        }

    }
}
