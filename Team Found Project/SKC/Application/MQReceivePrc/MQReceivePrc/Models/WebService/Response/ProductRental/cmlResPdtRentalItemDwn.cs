using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ProductRental
{
    public class cmlResPdtRentalItemDwn
    {
        public List<cmlResPdtPrice4PDT> raTRTTPdtPrice4PDT { get; set; }
        public List<cmlResPdtPrice4CST> raTRTTPdtPrice4CST { get; set; }
        public List<cmlResPdtRental> raTRTMPdtRental { get; set; }
    }
}
