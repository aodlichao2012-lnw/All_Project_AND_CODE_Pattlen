using AdaPos.Models.Webservice.Respond.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductGroup
{
    public class cmlResPdtGrpDwn
    {
        public List<cmlResInfoPdtGrp> raPdtGrp { get; set; }
        public List<cmlResInfoPdtGrpLng> raPdtGrpLng { get; set; }
        public List<cmlResInfoImgPdt> raPdtGrpImg { get; set; }    //*Arm 62-11-18 Image Group Product
    }
}
