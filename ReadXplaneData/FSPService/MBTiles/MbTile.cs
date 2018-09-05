using FSPAirnavDatabaseExporter.MBTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSPAirnavDatabaseExporter
{
    public class MbTile
    {
        public String Name { get; set; }
        public String Region { get; set; }
        public String MbTilesLink { get; set; }
        public String XmlLink { get; set; }
        public Int32 Version { get; set; }
        public MBTileType Type { get; set; }
        public DateTime startValidity { get; set; }
        public DateTime endValidity { get; set; }
    }
}
