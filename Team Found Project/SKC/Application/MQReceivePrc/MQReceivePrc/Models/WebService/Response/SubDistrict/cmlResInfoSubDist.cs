using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SubDistrict
{
    public class cmlResInfoSubDist
    {
        public string rtSudCode { get; set; }
        public string rtDstCode { get; set; }
        public string rtSudLatitude { get; set; }
        public string rtSudLongitude { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
