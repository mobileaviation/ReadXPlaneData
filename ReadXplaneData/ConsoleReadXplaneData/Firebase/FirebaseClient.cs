using ConsoleReadXplaneData.Models;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FSPAirnavDatabaseExporter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.IO;

namespace ConsoleReadXplaneData.Firebase
{
    public static class FirebaseDBClient
    {
        private static IFirebaseConfig GetConfig()
        {
            return new FirebaseConfig
            {
                AuthSecret = "",
                BasePath = "https://flightsimplanner-202711.firebaseio.com/"
            };
        }

        private static FirebaseClient GetClient(IFirebaseConfig config)
        {
            return new FirebaseClient(config);
        }

        public static void WriteData(DataTable table, ImportTypes type)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            float count = table.Rows.Count;
            float pos = 0;
            int index = 0;

            if (type == ImportTypes.airports)
            {
                FirebaseClient client = GetClient(GetConfig());
                foreach (DataRow R in table.Rows)
                {
                    Airport airport = AirportFactory.GetAirportFromDatatable(R, index);
                    index++;
                    float progress = (pos / count) * 100;
                    pos = pos + 1;
                    if (airport != null)
                    {
                        SetResponse response = client.Set("airports/" + airport.ident, airport);

                        Airport inserted = response.ResultAs<Airport>();

                        log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), inserted.ident, inserted.name);
                    }
                    else
                    {
                        log.Error("Airport parse error: {0}", R[0].ToString());
                    }

                     

                    
                }
            }
        }

        public static void SaveAsJson(DataTable table, ImportTypes type, String jsonFile)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            float count = table.Rows.Count;
            float pos = 0;
            int index = 0;
            //String airportsJson = "{" + ((char)34) + "airports"+ ((char)34) +"{";

            if (type == ImportTypes.airports)
            {
                using (StreamWriter file = File.CreateText(jsonFile))
                {
                    file.Write( "{" + ((char)34) + "airports" + ((char)34) + " : {" );

                    foreach (DataRow R in table.Rows)
                    {
                        float progress = (pos / count) * 100;
                        pos = pos + 1;
                        String json = ((char)34) + index.ToString() + ((char)34) + " : " + AirportFactory.GetJsonAirportFromDatatable(R, index);
                        index++;
                        //airportsJson = airportsJson + json + ",";

                        file.Write(json);
                        if (pos<count) file.Write(",");

                        //log.Info("Progress: {0}% Inserted Airport: {1} name: {2}", Math.Abs(progress), R["ident"].ToString(), R["name"].ToString());
                        log.Info("Progress: {0}%", Math.Abs(progress));

                    }

                    file.Write("} }");
                    file.Close();

                    //airportsJson = airportsJson.TrimEnd(',') + "}";
                    //File.WriteAllText(jsonFile, airportsJson);
                    log.Info("Json File: {0} writen", jsonFile);
                }
            }
        }
    }
}
