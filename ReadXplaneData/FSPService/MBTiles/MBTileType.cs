using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSPAirnavDatabaseExporter.MBTiles
{
    public enum MBTileType
    {
        ofm,
        unknown
    }

    public static class MBTileTypeHelper
    {
        public static MBTileType Parse(String type)
        {
            if (type == "ofm") return MBTileType.ofm;
            return MBTileType.unknown;
        }
    }
}
