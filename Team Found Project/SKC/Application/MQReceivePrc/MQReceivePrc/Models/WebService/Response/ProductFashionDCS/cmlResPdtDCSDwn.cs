using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductFashionDCS
{
    public class cmlResPdtDCSDwn
    {
        public List<cmlResInfoPdtDCS> raPdtDCS { get; set; }
        public List<cmlResInfoPdtDCSLng> raPdtDCSLng { get; set; }
    }
}
