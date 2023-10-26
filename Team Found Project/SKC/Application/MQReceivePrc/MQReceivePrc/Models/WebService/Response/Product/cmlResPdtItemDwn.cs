using MQReceivePrc.Models.WebService.Response.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Product
{
    public class cmlResPdtItemDwn
    {
        public List<cmlResInfoPdt> raPdt { get; set; }
        public List<cmlResInfoPdtLng> raPdtLng { get; set; }
        public List<cmlResInfoPdtPackSize> raPdtPackSize { get; set; }
        public List<cmlResInfoPdtBar> raPdtBar { get; set; }
        public List<cmlResInfoImgPdt> raImgPdt { get; set; }    //*Em 62-06-24
        public List<cmlResTCNMPdtSpcBch> raTCNMPdtSpcBch { get; set; }  //*Em 62-09-09

        public List<cmlResInfoPdtDrug> raTCNMPdtDrug { get; set; }  //*Arm 63-01-30
        public List<cmlResInfoPdtSpcWah> raTCNMPdtSpcWah { get; set; }  //*Arm 63-01-30
    }
}
