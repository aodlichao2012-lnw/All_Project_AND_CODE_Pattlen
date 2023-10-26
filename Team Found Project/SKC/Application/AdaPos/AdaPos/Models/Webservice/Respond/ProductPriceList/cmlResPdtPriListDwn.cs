using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductPriceList
{
    public class cmlResPdtPriListDwn
    {
        public List<cmlResInfoPdtPriList> raPdtPriList { get; set; }
        public List<cmlResInfoPdtPriListLng> raPdtPriListLng { get; set; }
    }
}
