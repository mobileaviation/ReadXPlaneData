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
        public EFAirspace()
        {
            activePeriods = new List<EFActivePeriod>();
            activeDays = new List<EFActiveDay>();
            atcStations = new List<EFATCStation>();
        }

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
        public ICollection<EFActivePeriod> activePeriods { get; set; }
        public ICollection<EFActiveDay> activeDays { get; set; }
        public ICollection<EFATCStation> atcStations { get; set; }
    }

    [Table("tbl_ATCStations")]
    public class EFATCStation
    {
        [Key]
        public long id { get; set; }
        public string frequency;
        public string stationname;
    }

    [Table("tbl_ActivePeriods")]
    public class EFActivePeriod
    {
        [Key]
        public long id { get; set; }
        public DateTime start;
        public DateTime end;
        public TimeZone timeZone;
    }

    [Table("tbl_ActiveDays")]
    public class EFActiveDay
    {
        [Key]
        public long id { get; set; }
        public string day;
        public TimeSpan period;
        public TimeZone TimeZone;
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

        public static EFATCStation GetEFATCStation(ATCStation atcStation)
        {
            EFATCStation efAtcStation = new EFATCStation();

            efAtcStation.frequency = atcStation.frequency;
            efAtcStation.stationname = atcStation.stationname;

            return efAtcStation;
        }

        public static EFActivePeriod GetEFActivePeriod(ActivePeriod activePeriod)
        {
            EFActivePeriod _activePeriod = new EFActivePeriod();

            _activePeriod.start = activePeriod.start;
            _activePeriod.end = activePeriod.end;
            _activePeriod.timeZone = activePeriod.timeZone;

            return _activePeriod;
        }

        public static EFActiveDay GetEFActiveDay(ActiveDay activeDay)
        {
            EFActiveDay eFActiveDay = new EFActiveDay();

            eFActiveDay.day = activeDay.day;
            eFActiveDay.period = activeDay.period;
            eFActiveDay.TimeZone = activeDay.TimeZone;

            return eFActiveDay;
        }
    }
}
