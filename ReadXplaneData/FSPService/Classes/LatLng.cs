using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Classes
{
    public class LatLng
    {
        public LatLng()
        {
            latitude = 0;
            longitude = 0;
        }

        public LatLng(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public double latitude;
        public double longitude;

        public Boolean equals(LatLng latLng)
        {
            return ((this.longitude == latLng.longitude) && (this.latitude == latLng.latitude));
        }

        public double bearingTo(LatLng to)
        {


            double longitude1 = this.longitude;
            double longitude2 = to.longitude;
            double latitude1 = LatLng.DegreeToRadian(this.latitude);
            double latitude2 = LatLng.DegreeToRadian(to.latitude);
            double longDiff = LatLng.DegreeToRadian(longitude2 - longitude1);
            double y = Math.Sin(longDiff) * Math.Cos(latitude2);
            double x = Math.Cos(latitude1) * Math.Sin(latitude2) - Math.Sin(latitude1) * Math.Cos(latitude2) * Math.Cos(longDiff);

            return (LatLng.RadianToDegree(Math.Atan2(y, x)) + 360) % 360;
        }

        public double distanceTo(LatLng to, String unit)
        {
            double theta = this.longitude - to.longitude;
            double dist = Math.Sin(LatLng.DegreeToRadian(this.latitude)) *
                    Math.Sin(LatLng.DegreeToRadian(to.latitude)) +
                    Math.Cos(LatLng.DegreeToRadian(this.latitude)) *
                            Math.Cos(LatLng.DegreeToRadian(to.latitude)) *
                            Math.Cos(LatLng.DegreeToRadian(theta));

            dist = Math.Acos(dist);
            dist = LatLng.RadianToDegree(dist);
            dist = dist * 60 * 1.1515;
            if (unit == "K")
            {
                dist = dist * 1.609344;
            }
            else if (unit == "N")
            {
                dist = dist * 0.8684;
            }
            else if (unit == "M")
            {
                dist = dist * 1609.344;
            }

            return (dist);
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}