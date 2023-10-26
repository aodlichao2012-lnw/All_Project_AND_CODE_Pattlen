using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Pos
{
    public class cmlResListOfcmlTCNMPosAds
    {
        public List<cmlTCNMPosAds> raItems { get; set; }
        public int rnCurrentPage { get; set; }
        public int rnAllPage { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
}
