using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Classes
{
    public class GeometricHelpers
    {
        public static List<Coordinate> drawCircle(LatLng center, Double radius)
        {
            // Get the distance to begin or end point. (distance in meters)
            // Multiply by 2 for diameter
            Double distance = (radius * 1853) * 2;
            // Calculate Lateral and longitudinal degrees from the distance in meters
            Double latTraveledDeg = (1 / 110.54) * (distance / 1000);
            Double longTraveledDeg = (1 / (111.320 * Math.Cos(LatLng.DegreeToRadian(center.latitude)))) * (distance / 1000);

            GeometricShapeFactory geometricShapeFactory = new GeometricShapeFactory();
            geometricShapeFactory.Centre = new Coordinate(center.longitude, center.latitude);
            geometricShapeFactory.Height = latTraveledDeg;
            geometricShapeFactory.Width = longTraveledDeg;
            geometricShapeFactory.NumPoints = 50;

            IPolygon coordinates;
            coordinates = geometricShapeFactory.CeateEllipse();

            return coordinates.Coordinates.ToList();
        }

        public static List<Coordinate> drawArc(Double startAngle, Double endAngle, Double radiusNM, LatLng center, Boolean cw)
        {
            // Calculate distance (diameter) in meters
            Double distance = (radiusNM * 1851) * 2;
            // Calculate Lateral and longitudinal degrees from the distance in meters
            Double latTraveledDeg = (1 / 110.54) * (distance / 1000);
            Double longTraveledDeg = (1 / (111.320 * Math.Cos(LatLng.DegreeToRadian(center.latitude)))) * (distance / 1000);

            Double arcBegin = 360 - startAngle + 90;
            Double arcEnd = 360 - endAngle + 90;
            Double arcSize = arcEnd - arcBegin;

            if ((arcSize > 0) && cw) arcSize = 360 - arcSize;
            else
            if ((arcSize < 0) && cw) arcSize = -1 * arcSize;
            else
            if ((arcSize < 0) && !cw) arcSize = 360 + arcSize;

            GeometricShapeFactory geometricShapeFactory = new GeometricShapeFactory();
            geometricShapeFactory.Centre = new Coordinate(center.longitude, center.latitude);
            geometricShapeFactory.Height = latTraveledDeg;
            geometricShapeFactory.Width = longTraveledDeg;
            geometricShapeFactory.NumPoints = 50;

            // because the arc is drawn counter clockwise the arcEnd is actually the startpoint
            Coordinate[] coordinates;
            if (cw)
                coordinates = geometricShapeFactory.CreateArc(LatLng.DegreeToRadian(arcEnd), LatLng.DegreeToRadian(arcSize)).Coordinates;
            else
                coordinates = geometricShapeFactory.CreateArc(LatLng.DegreeToRadian(arcBegin), LatLng.DegreeToRadian(arcSize)).Coordinates;

            List<Coordinate> list = coordinates.ToList();
            list.Reverse();
            if (cw) list.Reverse();
            return list;
        }

        public static List<Coordinate> drawArc(LatLng start, LatLng end, LatLng center, Boolean cw)
        {
            // Get Location class from LatLng Class

            // Get the distance to begin or end point. (distance in meters)
            // Multiply by 2 for diameter
            Double distance = center.distanceTo(end, "M") * 2;
            // Calculate Lateral and longitudinal degrees from the distance in meters
            Double latTraveledDeg = (1 / 110.54) * (distance / 1000);
            Double longTraveledDeg = (1 / (111.320 * Math.Cos(LatLng.DegreeToRadian(center.latitude)))) * (distance / 1000);

            // find the first angle to the begin point
            // The 0 degree line for the arc is horizontal and the first point is left
            // So recalculate the angles
            // angles should all be positive

            Double arcBegin = 360 - center.bearingTo(start) + 90;
            Double arcEnd = 360 - center.bearingTo(end) + 90;
            Double arcSize = arcEnd - arcBegin;

            if ((arcSize > 0) && cw) arcSize = 360 - arcSize;
            else
            if ((arcSize < 0) && cw) arcSize = -1 * arcSize;
            else
            if ((arcSize < 0) && !cw) arcSize = 360 + arcSize;

            GeometricShapeFactory geometricShapeFactory = new GeometricShapeFactory();
            geometricShapeFactory.Centre = new Coordinate(center.longitude, center.latitude);
            geometricShapeFactory.Height = latTraveledDeg;
            geometricShapeFactory.Width = longTraveledDeg;
            geometricShapeFactory.NumPoints = 50;

            // because the arc is drawn counter clockwise the arcEnd is actually the startpoint
            Coordinate[] coordinates;
            if (cw)
                coordinates = geometricShapeFactory.CreateArc(LatLng.DegreeToRadian(arcEnd), LatLng.DegreeToRadian(arcSize)).Coordinates;
            else
                coordinates = geometricShapeFactory.CreateArc(LatLng.DegreeToRadian(arcBegin), LatLng.DegreeToRadian(arcSize)).Coordinates;

            List<Coordinate> list = coordinates.ToList();
            if (cw) list.Reverse();
            return list;
        }
    }
}
