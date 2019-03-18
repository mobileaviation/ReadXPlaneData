using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Classes
{
    public class GeoJsonPoint
    {
        public GeoJsonPoint(Double lat, Double lon)
        {
            type = GeoJsonType.Point;
            coordinates = new Double[2];
            coordinates[0] = lon;
            coordinates[1] = lat;
        }

        public GeoJsonType type { get; set; }
        public Double[] coordinates { get; set; }


        public BsonDocument getPointElement()
        {
            BsonDocument doc = new BsonDocument();
            doc.Add("type", type.ToString());
            doc.Add("coordinates", new BsonArray(coordinates));
            return doc;
        }
    }
}
