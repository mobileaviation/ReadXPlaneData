using ConsoleReadXplaneData.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Frequencies")]
    public class EFFrequency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 id { get; set; }
        [Index]
        [ForeignKey("airport")]
        public Int32 airport_ref { get; set; }
        public EFAirport airport { get; set; }
        public String airport_ident { get; set; }
        public String type { get; set; }
        public String description { get; set; }
        public Double frequency_mhz { get; set; }
    }

    public static class FrequencyFactory
    {
        public static EFFrequency GetFrequencyFromDatatable(DataRow data)
        {
            try
            {
                EFFrequency a = new EFFrequency();

                a.id = Convert.ToInt32(data["id"]);
                a.airport_ref = Convert.ToInt32(data["airport_ref"]);
                a.airport_ident = data["airport_ident"].ToString();
                a.type = data["type"].ToString();
                a.description = data["description"].ToString();
                a.frequency_mhz = Xs.ToDouble(data, "frequency_mhz", 0);



                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
