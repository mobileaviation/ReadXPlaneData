using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Frequency
    {
        public Int32 id { get; set; }
        public Int32 airport_ref { get; set; }
        public String airport_ident { get; set; }
        public String type { get; set; }
        public String description { get; set; }
        public Double frequency_mhz { get; set; }
    }

    public static class FrequencyFactory
    {
        public static Frequency GetFrequencyFromDatatable(DataRow data)
        {
            try
            {
                Frequency a = new Frequency();

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

        public static String GetJsonRunwayFromDatatable(DataRow data)
        {
            Frequency frequency = GetFrequencyFromDatatable(data);
            return JsonConvert.SerializeObject(frequency, Formatting.Indented);
        }

    }
}
