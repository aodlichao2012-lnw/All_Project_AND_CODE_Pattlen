using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Zone
{
    public class cmlResInfoZoneLng
    {
        public string rtZneCode { get; set; }
        public int rnLngID { get; set; }
        public string rtZneName { get; set; }
        public string rtZneRmk { get; set; }
        public string rtZneChain { get; set; }      //*Arm 63-01-30
        public string rtZneChainName { get; set; }
        
    }
}
