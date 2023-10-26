using MQReceivePrc.Models.WebService.Response.ProductRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.PriceRate
{
    public class cmlResRTPdtPriceDwn
    {
        public List<cmlResPdtPrice4PDT> raTRTTPdtPrice4PDT { get; set; }
        public List<cmlResPdtPrice4CST> raTRTTPdtPrice4CST { get; set; }
    }
}
