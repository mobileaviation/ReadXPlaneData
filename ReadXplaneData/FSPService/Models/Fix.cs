using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Fix
    {
        public Int32 index { get; set; }
        public String ident { get; set; }
        public Double latitude_deg { get; set; }
        public Double longitude_deg { get; set; }
    }

    public static class FixFactory
    {
        public static Fix GetFixFromDatatable(DataRow fixData, int Index)
        {
            try
            {
                Fix a = new Fix();

                a.index = Index;
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

        public static String GetJsonFixFromDatatable(DataRow data, Int32 index)
        {
            Fix fix = GetFixFromDatatable(data, index);
            return JsonConvert.SerializeObject(fix, Formatting.Indented);
        }

        public static String GetJsonFixFromDatatable(Fix fix)
        {
            return JsonConvert.SerializeObject(fix, Formatting.Indented);
        }

    }
}
