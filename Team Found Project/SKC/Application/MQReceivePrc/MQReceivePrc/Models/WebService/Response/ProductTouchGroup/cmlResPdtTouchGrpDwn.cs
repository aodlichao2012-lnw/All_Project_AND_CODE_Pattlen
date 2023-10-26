using MQReceivePrc.Models.Webservice.Response.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductTouchGroup
{
    public class cmlResPdtTouchGrpDwn
    {
        public List<cmlResInfoPdtTouchGrp> raPdtTouchGrp { get; set; }
        public List<cmlResInfoPdtTouchGrpLng> raPdtTouchGrpLng { get; set; }
        public List<cmlResInfoImgPdt> raPdtTouchGrpImg { get; set; }  //*Arm 63-02-12
    }
}
