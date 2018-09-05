using ConsoleReadXplaneData.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Runways")]
    public class EFRunway
    {
        [Key]
        public Int32 _id { get; set; }
        public Int32 id { get; set; }
        [Index]
        public Int32 airport_ref { get; set; }
        public String airport_ident { get; set; }
        public Int32 length_ft { get; set; }
        public Int32 width_ft { get; set; }
        public Int32 lighted { get; set; }
        public Int32 closed { get; set; }
        public String surface { get; set; }
        public String le_ident { get; set; }
        [Index]
        public Double le_latitude_deg { get; set; }
        [Index]
        public Double le_longitude_deg { get; set; }
        public Int32 le_elevation_ft { get; set; }
        public Double le_heading_degT { get; set; }
        public Int32 le_displaced_threshold_ft { get; set; }
        public String he_ident { get; set; }
        [Index]
        public Double he_latitude_deg { get; set; }
        [Index]
        public Double he_longitude_deg { get; set; }
        public Int32 he_elevation_ft { get; set; }
        public Double he_heading_degT { get; set; }
        public Int32 he_displaced_threshold_ft { get; set; }
    }

    public static class RunwayFactory
    {
        public static EFRunway GetRunwayFromDatatable(DataRow data)
        {
            try
            {
                EFRunway a = new EFRunway();

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
    }
}
