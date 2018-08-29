using System;
using ConsoleReadXplaneData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Firs")]
    public class EFFir
    {
        [Key]
        public Int32 _id { get; set; }
        public Int32 id { get; set; }
        public String ident { get; set; }
        public String name { get; set; }
        public String position { get; set; }
        public String polygon { get; set; }
    }

    public static class FirFactory
    {
        public static EFFir GetFirFromDatatable(DataRow firData)
        {
            try
            {
                EFFir a = new EFFir();

                a.id = Xs.ToInt(firData, "id", 0);
                a.name = firData["name"].ToString();
                a.ident = firData["ident"].ToString();
                a.polygon = firData["polygon"].ToString();
                a.position = firData["position"].ToString();

                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
