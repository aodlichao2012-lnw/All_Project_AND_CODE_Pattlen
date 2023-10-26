using MQReceivePrc.Models.WebService.Response.PriceRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ProductPrice
{
    public class cmlResItemOfcmlResPdtPriceDwn
    {
        public cmlResRTPdtPriceDwn roItem { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
}
