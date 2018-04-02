using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleReadXplaneData
{
    public class OfmMbTile
    {
        public String Region { get; set; }
        public String MbTilesLink { get; set; }
        public String XmlLink { get; set; }
        public String Version { get; set; }
        public DateTime startValidity { get; set; }
        public DateTime endValidity { get; set; }
    }
}
