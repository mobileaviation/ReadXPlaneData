using FSPService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSPService.Classes
{
    public class Helpers
    {
        public static LatLng parseOpenAirLocation(String location)
        {
            //Remove all the text after the * sign

            // replace DP, DB, V X=
            String l = location.Replace("DP", "");
            l = l.Replace("DB", "");
            l = l.Replace("V X=", "");
            l = l.Trim();
            l = l.Replace(":.", ":");
            l = l.Replace(".", ":");
            if (l.IndexOf("*") > -1) l = l.Substring(0, l.IndexOf("*"));

            // 53:40:00 N 006:30:00 E
            String[] loc = Regex.Split(l, "[NSns]");

            LatLng latLng = null;
            String[] lat = loc[0].Split(':');
            Double _lat = (Convert.ToDouble(lat[0]) +
                    (Convert.ToDouble(lat[1]) / 60) +
                    (Convert.ToDouble(lat[2]) / 3600))
                            * ((l.Contains("S")) ? -1 : 1);
            String[] lon = loc[1].Split(':');
            lon[2] = findRegex("[0-9]+", lon[2]);

            Double _lon = (Convert.ToDouble(lon[0]) +
                    (Convert.ToDouble(lon[1]) / 60) +
                    (Convert.ToDouble(lon[2]) / 3600))
                            * ((l.Contains("W")) ? -1 : 1);
            latLng = new LatLng(_lat, _lon);

            return latLng;
        }

        public static String findRegex(String pattern, String input)
        {
            try
            {
                Regex regex = new Regex(pattern);
                Match match = regex.Match(input);
                if (match.Success)
                    return match.Value;
                else
                    return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static AltitudeUnit parseUnit(String value)
        {
            value = value.ToUpper();
            if (value.Equals("MSL")) return AltitudeUnit.F;
            if (value.Equals("AGL")) return AltitudeUnit.F;
            if (value.Equals("FT")) return AltitudeUnit.F;
            if (value.Equals("FL")) return AltitudeUnit.FL;
            if (value.Equals("GND") || value.Equals("SFC")) return AltitudeUnit.F;

            return AltitudeUnit.F;
        }

        public static AltitudeReference parseReference(String value)
        {
            value = value.ToUpper();
            if (value.Equals("MSL")) return AltitudeReference.MSL;
            if (value.Equals("AGL")) return AltitudeReference.AGL;
            if (value.Equals("FT")) return AltitudeReference.MSL;
            if (value.Equals("FL")) return AltitudeReference.STD;
            if (value.Equals("GND") || value.Equals("SFC")) return AltitudeReference.GND;

            return AltitudeReference.MSL;
        }
    }
}
