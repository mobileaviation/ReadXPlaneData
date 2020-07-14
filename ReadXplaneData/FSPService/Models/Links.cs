using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Models
{
    public class Link
    {
        public long id { get; set; }
        public String country { get; set; }
        public String countrycode { get; set; }
        public String openaiplink { get; set; }
        public String xsourlink { get; set; }
        public Boolean enabled { get; set; }
        public String weblink { get; set; }
        public Boolean openaip_enabled { get; set; }
        public long openaip_id { get; set; }
        public String local_filename { get; set; }
    }
}
