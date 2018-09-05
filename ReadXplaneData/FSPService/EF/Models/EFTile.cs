using ConsoleReadXplaneData.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ConsoleReadXplaneData.EF.Models
{
    [Table("tbl_Tiles")]
    public class EFTile
    {
        [Key]
        public Int32 _id { get; set; }
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
        public static EFTile GetTileFromDatatable(DataRow tileData)
        {
            try
            {
                EFTile a = new EFTile();

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
    }
}
