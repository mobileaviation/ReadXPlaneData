using System;
using ConsoleReadXplaneData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Fixes")]
    public class EFFix
    {
        [Key]
        public Int32 _id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        [Index]
        public String ident { get; set; }
        [Index]
        public Double latitude_deg { get; set; }
        [Index]
        public Double longitude_deg { get; set; }
    }

    public static class FixFactory
    {
        public static EFFix GetFixFromDatatable(DataRow fixData)
        {
            try
            {
                EFFix a = new EFFix();

                a.ident = fixData["ident"].ToString();
                a.latitude_deg = Xs.ToDouble(fixData, "latitude_deg", 0);
                a.longitude_deg = Xs.ToDouble(fixData, "longitude_deg", 0);

                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
