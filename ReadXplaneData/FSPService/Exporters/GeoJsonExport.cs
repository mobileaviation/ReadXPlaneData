using FSPService.EF.Models;
using FSPService.Models;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Exporters
{
    public class GeoJsonExport
    {
        public GeoJsonExport(Link link, Airspaces airspaces)
        {
            this.airspaces = airspaces;
            this.link = link;
        }

        private Link link;
        private Airspaces airspaces;


        public void exportToGeoJsonFile(String path)
        {
            String filename = path + @"\" + link.country + ".json";

            var airspacesData = new List<Feature>();
            FeatureCollection airspaceData = new FeatureCollection(airspacesData);

            foreach(Airspace airspace in airspaces)
            {
                var polygon = new Polygon(new List<LineString>
                {
                    new LineString(airspace.GetPositions())
                });

                var props = new Dictionary<string, object>();
                props.Add("name", airspace.name);
                props.Add("category", airspace.category);
                props.Add("altLimit_bottom", airspace.altLimit_bottom);
                props.Add("altLimit_bottom_ref", airspace.altLimit_bottom_ref);
                props.Add("altLimit_bottom_unit", airspace.altLimit_bottom_unit);
                props.Add("altLimit_top", airspace.altLimit_top);
                props.Add("altLimit_top_ref", airspace.altLimit_top_ref);
                props.Add("altLimit_top_unit", airspace.altLimit_top_unit);

                var feature = new Feature(polygon, props);

                airspaceData.Features.Add(feature);
            }


            String json = JsonConvert.SerializeObject(airspaceData, Formatting.Indented);
            System.IO.File.WriteAllText(filename, json);

        }


    }
}
