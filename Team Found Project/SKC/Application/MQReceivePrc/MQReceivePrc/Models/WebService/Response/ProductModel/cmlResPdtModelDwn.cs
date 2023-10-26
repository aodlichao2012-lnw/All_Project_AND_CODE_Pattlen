using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductModel
{
    public class cmlResPdtModelDwn
    {
        public List<cmlResInfoPdtModel> raPdtModel { get; set; }
        public List<cmlResInfoPdtModelLng> raPdtModelLng { get; set; }
    }
}
