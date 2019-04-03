using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Fir
    {
        public Int32 index { get; set; }
        public Int32 id { get; set; }
        public String ident { get; set; }
        public String name { get; set; }
        public String position { get; set; }
        public String polygon { get; set; }
    }

    public static class FirFactory
    {
        public static Fir GetFirFromDatatable(DataRow firData, int Index)
        {
            try
            {
                Fir a = new Fir();

                a.index = Index;
                a.id = Xs.ToInt(firData,"id",0);
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

        public static String GetJsonTileFromDatatable(DataRow data, Int32 index)
        {
            Fir fir = GetFirFromDatatable(data, index);
            return JsonConvert.SerializeObject(fir, Formatting.Indented);
        }

        public static String GetJsonTileFromDatatable(Fir fir)
        {
            return JsonConvert.SerializeObject(fir, Formatting.Indented);
        }
    }
}
