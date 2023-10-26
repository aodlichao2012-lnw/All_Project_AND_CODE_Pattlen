using AdaPos.Models.Webservice.Respond.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductTouchGroup
{
    public class cmlResPdtTouchGrpDwn
    {
        public List<cmlResInfoPdtTouchGrp> raPdtTouchGrp { get; set; }
        public List<cmlResInfoPdtTouchGrpLng> raPdtTouchGrpLng { get; set; }
        public List<cmlResInfoImgPdt> raPdtTouchGrpImg { get; set; }  //*Arm 63-02-12
    }
}
