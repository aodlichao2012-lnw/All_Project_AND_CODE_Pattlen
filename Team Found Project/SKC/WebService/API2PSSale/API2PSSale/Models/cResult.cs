using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models
{
    public class cResult<Model>
    {
        public string rtCode { get; set; }
        public string rtMsg { get; set; }
        public IEnumerable<Model> raItems { get; set; }
        public Model roItem { get; set; }
        public int rnCount { get; set; }
    }
}