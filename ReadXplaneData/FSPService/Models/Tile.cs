using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Tile
    {
        public Int32 index { get; set; }
        public String name { get; set; }
        public String region { get; set; }
        public String type { get; set; }
        public String mbtileslink { get; set; }
        public String xmllink { get; set; }
        public String version { get; set; }
        public long startValidity { get; set; }
        public long endValidity { get; set; }
    }

    public static class TileFactory
    {
        public static Tile GetTileFromDatatable(DataRow tileData, int Index)
        {
            try
            {
                Tile a = new Tile();

                a.index = Index;
                a.mbtileslink = tileData["mbtileslink"].ToString();
                a.name = tileData["name"].ToString();
                a.region = tileData["region"].ToString();
                a.type = tileData["type"].ToString();
                a.xmllink = tileData["xmllink"].ToString();
                a.version = tileData["version"].ToString();
                a.startValidity = Xs.ToLong(tileData, "startValidity", 0);
                a.endValidity = Xs.ToLong(tileData, "endValidity", 0);

                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        public static String GetJsonTileFromDatatable(DataRow data, Int32 index)
        {
            Tile tile = GetTileFromDatatable(data, index);
            return JsonConvert.SerializeObject(tile, Formatting.Indented);
        }

        public static String GetJsonTileFromDatatable(Tile tile)
        {
            return JsonConvert.SerializeObject(tile, Formatting.Indented);
        }
    }
}
