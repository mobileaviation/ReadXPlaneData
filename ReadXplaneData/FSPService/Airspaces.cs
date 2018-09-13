using FSPService.Classes;
using FSPService.Enums;
using FSPService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using GeoAPI.Geometries;
using System.IO;
using System.Globalization;

namespace FSPService
{
    public class Airspaces: List<Airspace>
    {
        public Airspaces()
        {
            log = LogManager.GetCurrentClassLogger();
        }

        public void processAirspaceFile(String filename, String country)
        {
            String filecontent = File.ReadAllText(filename);
            readOpenAirText(filecontent, country);
        }

        private Logger log;

        public String[] lines;
        private void readOpenAirText(String text, String country)
        {
            try
            {
                //text = text.replace("\n", "\r\n");
                //lines = text.split("\r\n");
                //String textNoComments =  text.replaceAll("(?m)^\\*.*", "");
                String textNoComments = Regex.Replace(text, "(?m)^\\*.*", "");
                //lines = Regex.Split(textNoComments, @"(?=\\*)|(?=\\bAC)|(?=\\bAN)|(?=\\bAH)|(?=\\bAL)|(?=\\bAF)|(?=\\bAG)|(?=\\bDP)|(?=\\bDB)|(?=\\bDC\\s[0-9]+(\\.\\d{1,2})?)|(?=\\bV\\sX=[0-9])|(?=\\bV\\sD)|(?=\\bDA)");
                lines = Regex.Split(textNoComments, "(?=(?m)^.)");
                //lines = text.split("(?=\\*)|(?=\\bAC)|(?=\\bAN)|(?=\\bAH)|(?=\\bAL)|(?=\\bAF)|(?=\\bAG)|(?=\\bDP)|(?=\\bDB)|(?=\\bDC\\s[0-9]+(\\.\\d{1,2})?)|(?=\\bV\\sX=[0-9])|(?=\\bV\\sD)|(?=\\bDA)");
                Airspace airspace = null;
                LatLng location = null;
                LatLng center = null;
                Boolean circle = false;
                Boolean cw = true;
                //Boolean newAirspace = false;
                foreach (String l in lines)
                {

                    try
                    {
                        if (!l.StartsWith("*"))
                        {
                            // Check is first char = * then discard this split[*]
                            // Read the first line for the Airspace Category (AC)
                            // Read the line with starts with AN, Following string is Name
                            // -- AH, unit (non if FT), top level limit, folowed by reference (MSL)
                            // -- AL, unit (non if FT), bottom level limit, folowed by reference (MSL)

                            //

                            if (l.StartsWith("AC "))
                            {
                                if ((airspace != null) && (airspace.coordinates.Count() > 3) && !circle
                                        && !airspace.coordinates[0].Equals(airspace.coordinates[airspace.coordinates.Count() - 1]))
                                    airspace.coordinates.Add(airspace.coordinates[0]);
                                if ((airspace != null) && (airspace.name == null))
                                    this.Remove(airspace);
                                if ((airspace != null) && (airspace.coordinates.Count() < 4))
                                    this.Remove(airspace);
                                airspace = new Airspace();
                                airspace.country = country;
                                cw = true;
                                //newAirspace = false;
                                this.Add(airspace);
                                airspace.version = "0";
                                airspace.id = 0;
                                String c = l.Replace("AC ", "").Trim();
                                c = Helpers.findRegex("[A-Za-z]+\\w|[A-Za-z]", c);
                                airspace.category = (Enum.IsDefined(typeof(AirspaceCategory), c)) ? 
                                    (AirspaceCategory)Enum.Parse(typeof(AirspaceCategory), c) : AirspaceCategory.UKN;
                                //airspace.category = (AirspaceCategory)Enum.Parse(typeof(AirspaceCategory), Helpers.findRegex("[A-Za-z]+\\w|[A-Za-z]", c), true);
                            }
                            if (l.StartsWith("AN "))
                            {
                                if (airspace != null)
                                {
                                    airspace.name = l.Replace("AN ", "");
                                    //newAirspace = true;
                                    log.Info("Airspace: " + airspace.name + " added Index: " + (this.Count() - 1).ToString() + " Country: " + airspace.country);
                                }

                            }
                            if (l.StartsWith("AH"))
                            {
                                if (airspace != null)
                                {
                                    //String ss = Helpers.findRegex("\\d+", l);
                                    //Integer sss = Integer.parseInt(ss);
                                    airspace.altLimit_top = Convert.ToInt64("0" + Helpers.findRegex("\\d+", l));
                                    String m = Helpers.findRegex("(\\bMSL)|(\\bFL)|(\\bFT)|(\\bSFC)|(\\bUNLIM)|(\\bAGL)", l);
                                    if (m.Equals("UNLIM")) airspace.altLimit_top = 100000;
                                    airspace.altLimit_top_ref = Helpers.parseReference(m);
                                    airspace.altLimit_top_unit = Helpers.parseUnit(m);
                                }
                            }
                            if (l.StartsWith("AL "))
                            {
                                if (airspace != null)
                                {
                                    airspace.altLimit_bottom = Convert.ToInt64("0" + Helpers.findRegex("\\d+", l));
                                    String m = Helpers.findRegex("(\\bMSL)|(\\bFL)|(\\bFT)|(\\bSFC)|(\\bUNLIM)|(\\bAGL)|(\\bGND)", l);
                                    if (m.Equals("UNLIM")) airspace.altLimit_top = 100000;
                                    airspace.altLimit_bottom_ref = Helpers.parseReference(m);
                                    airspace.altLimit_bottom_unit = Helpers.parseUnit(m);
                                }
                            }
                            if (l.StartsWith("V D"))
                            {
                                cw = (Helpers.findRegex("\\+", l).Equals("+"));
                                int i = 0;
                            }
                            if (l.StartsWith("V X"))
                            {
                                center = Helpers.parseOpenAirLocation(l);
                            }
                            if (l.StartsWith("DA "))
                            {
                                String[] be = l.Split(',');
                                Double begin = double.Parse(Helpers.findRegex("([0-9.]+\\w)|([0-9])", be[1]), CultureInfo.InvariantCulture);
                                Double end = double.Parse(Helpers.findRegex("([0-9.]+\\w)|([0-9])", be[2]), CultureInfo.InvariantCulture);
                                Double distance = double.Parse(Helpers.findRegex("([0-9.]+\\w)|([0-9])", be[0]), CultureInfo.InvariantCulture);
                                airspace.coordinates.AddRange(GeometricHelpers.drawArc(begin, end, distance, center, cw));
                                circle = false;
                            }
                            if (l.StartsWith("DB "))
                            {
                                String[] be = l.Split(',');
                                LatLng begin = Helpers.parseOpenAirLocation(be[0]);
                                LatLng end = Helpers.parseOpenAirLocation(be[1]);
                                airspace.coordinates.AddRange(GeometricHelpers.drawArc(begin, end, center, cw));
                                circle = false;
                            }
                            if (l.StartsWith("DP "))
                            {
                                location = Helpers.parseOpenAirLocation(l);
                                airspace.coordinates.Add(new Coordinate(location.longitude, location.latitude));
                                circle = false;
                            }
                            if (l.StartsWith("DC "))
                            {
                                if (airspace != null)
                                {
                                    String m = Helpers.findRegex("([0-9.]+\\w)|([0-9])", l);
                                    airspace.coordinates.AddRange(GeometricHelpers.drawCircle(center, double.Parse(m, CultureInfo.InvariantCulture)));
                                    circle = true;
                                }
                            }
                            //                if (l.startsWith("SP"))
                            //                {
                            //                    // What if SP becomes before AN ??????????????
                            //
                            //                    // We need to check if the SP is just a pen setting which means this iy does not belong to a specific airspace
                            //                    // If it does not belong to an airspace than delete this airspace
                            //                    if (!newAirspace) {
                            //                        if (airspace != null) {
                            //                            this.remove(airspace);
                            //                            airspace = null;
                            //                        }
                            //                    }
                            //                }

                        }
                    }
                    catch (Exception e)
                    {
                        if (airspace != null)
                            this.Remove(airspace);
                        log.Error(e, "Parsing error: ");
                    }
                }
                if ((airspace != null) && (airspace.coordinates.Count() > 0)) airspace.coordinates.Add(airspace.coordinates[0]);
            }
            catch (Exception e)
            {
                log.Error(e, "Airspaces Error");
                int i = 0;
            }
        }
    }
}
