using FSPService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.EF.Models
{
    [Table("tbl_Airspaces")]
    public class EFAirspace
    {
        [Key]
        public long id { get; set; }
        public String name { get; set; }
        public String version { get; set; }
        public String category { get; set; }
        public long airspace_id { get; set; }
        public String country { get; set; }
        public long altLimit_top { get; set; }
        public String altLimit_top_unit { get; set; }
        public String altLimit_top_ref { get; set; }
        public long altLimit_bottom { get; set; }
        public String altLimit_bottom_unit { get; set; }
        public String altLimit_bottom_ref { get; set; }
        public DbGeometry geometry { get; set; }
    }

    public class AirspaceFactory
    {
        public static EFAirspace GetEFAirspace(Airspace airspace)
        {
            EFAirspace a = new EFAirspace();

            a.airspace_id = airspace.airspace_id;
            a.altLimit_bottom = airspace.altLimit_bottom;
            a.altLimit_bottom_ref = airspace.altLimit_bottom_ref.ToString();
            a.altLimit_bottom_unit = airspace.altLimit_bottom_unit.ToString();
            a.altLimit_top = airspace.altLimit_top;
            a.altLimit_top_ref = airspace.altLimit_top_ref.ToString();
            a.altLimit_top_unit = airspace.altLimit_top_unit.ToString();
            a.category = airspace.category.ToString();
            a.name = airspace.name;
            a.version = airspace.version;
            a.country = airspace.country;
            a.geometry = airspace.GetDBGeometry();

            return a;
        }
    }
}
