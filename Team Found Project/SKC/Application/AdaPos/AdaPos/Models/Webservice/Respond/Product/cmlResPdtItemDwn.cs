using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Product
{
    public class cmlResPdtItemDwn
    {
        public List<cmlResInfoPdt> raPdt { get; set; }
        public List<cmlResInfoPdtLng> raPdtLng { get; set; }
        public List<cmlResInfoPdtPackSize> raPdtPackSize { get; set; }
        public List<cmlResInfoPdtBar> raPdtBar { get; set; }
        public List<cmlResInfoImgPdt> raImgPdt { get; set; }    //*Em 62-06-24
        public List<cmlResTCNMPdtSpcBch> raTCNMPdtSpcBch { get; set; }  //*Em 62-09-09
    }
}
