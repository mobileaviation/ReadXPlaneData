using FSPService.Enums;
using GeoAPI.Geometries;
using GeoJSON.Net.Geometry;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Models
{
    public class Airspace
    {
        public Airspace()
        {
            coordinates = new List<Coordinate>();
            atcStations = new List<ATCStation>();
            activePeriods = new List<ActivePeriod>();
            activeDays = new List<ActiveDay>();
        }

        public long id;
        public String name;
        public String version;
        public AirspaceCategory category;
        public long airspace_id;
        public String country;
        public long altLimit_top;
        public AltitudeUnit altLimit_top_unit;
        public AltitudeReference altLimit_top_ref;
        public long altLimit_bottom;
        public AltitudeUnit altLimit_bottom_unit;
        public AltitudeReference altLimit_bottom_ref;
        public String transponder_mandatory_code;
        public IGeometry geometry { get; set; }
        public List<Coordinate> coordinates;

        public List<ATCStation> atcStations;
        public List<ActivePeriod> activePeriods;
        public List<ActiveDay> activeDays;


        public DbGeometry GetDBGeometry()
        {
            getGeometry();
            DbGeometry g = null;
            if (geometry!= null)
            {
                g = DbGeometry.FromText(new WKTWriter().Write(geometry));
            }
            return g;
        }

        public void getGeometry()
        {
            if (geometry == null)
            {
                if (coordinates.Count() == 0)
                {
                    return;
                }
                if ((coordinates[0].X != coordinates[coordinates.Count() - 1].X) ||
                        (coordinates[0].Y != coordinates[coordinates.Count() - 1].Y))
                {
                    coordinates.Add(coordinates[0]);
                }
                coordinates.ToArray();
                Coordinate[] c = coordinates.ToArray();

                try
                {
                    geometry = new GeometryFactory().CreatePolygon(c);
                }
                catch (Exception e)
                {
                    
                }
            }
        }

        public List<Position> GetPositions()
        {
            List<Position> positions = null;

            if (geometry == null)
            {
                positions = new List<Position>();

                if (coordinates.Count() == 0)
                {
                    return null;
                }
                if ((coordinates[0].X != coordinates[coordinates.Count() - 1].X) ||
                        (coordinates[0].Y != coordinates[coordinates.Count() - 1].Y))
                {
                    coordinates.Add(coordinates[0]);
                }
                
                foreach (Coordinate c in coordinates)
                {
                    positions.Add(new Position(c.Y, c.X));
                }

            }

            return positions;
        }
    }
    


    public class ATCStation
    {
        public string frequency;
        public string stationname;
    }

    public class ActivePeriod
    {
        public DateTime start;
        public DateTime end;
        public String timeZone;
    }

    public class ActiveDay
    {
        public string day;
        public TimeSpan start;
        public TimeSpan end;
        public String timeZone;
    }
}
