using ConsoleReadXplaneData.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Navaids")]
    public class EFNavaid
    {
        [Key]
        public Int32 _id { get; set; }
        public Int32 id { get; set; }
        public String filename { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        [Index]
        public String ident { get; set; }
        public String name { get; set; }
        public String type { get; set; }
        public Double frequency_khz { get; set; }
        [Index]
        public Double latitude_deg { get; set; }
        [Index]
        public Double longitude_deg { get; set; }
        public Int32 elevation_ft { get; set; }
        public String iso_country { get; set; }
        public String continent { get; set; }
        public Double dme_frequency_khz { get; set; }
        public Double dme_channel { get; set; }
        public Double dme_latitude_deg { get; set; }
        public Double dme_longitude_deg { get; set; }
        public Int32 dme_elevation_ft { get; set; }
        public Double slaved_variation_deg { get; set; }
        public Double magnetic_variation_deg { get; set; }
        public String usageType { get; set; }
        public String power { get; set; }
        public String associated_airport { get; set; }
        public Int32 associated_airport_id { get; set; }
    }

    public static class NavaidFactory
    {
        public static EFNavaid GetNavaidFromDatatable(DataRow navaidData)
        {
            try
            {
                EFNavaid a = new EFNavaid();

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
    }
}
