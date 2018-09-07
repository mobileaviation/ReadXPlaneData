using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.EF.Models
{
    [Table("tbl_Links")]
    public class EFLink
    {
        [Key]
        public long id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(32)]
        public String country { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(2)]
        public String countrycode { get; set; }
        public String openaiplink { get; set; }
        public String xsourlink { get; set; }
        public Boolean enabled { get; set; }
        public String weblink { get; set; }
        public Boolean openaip_enabled { get; set; }
        public long openaip_id { get; set; }
        [NotMapped]
        public String local_filename { get; set; }
    }
}
