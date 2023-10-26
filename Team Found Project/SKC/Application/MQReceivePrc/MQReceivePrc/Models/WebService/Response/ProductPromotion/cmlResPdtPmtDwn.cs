using MQReceivePrc.Models.WebService.Response.ProductPromotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductPromotion
{
    public class cmlResPdtPmtDwn
    {
        public List<cmlResInfoPdtPmtHD> raPdtPmtHD { get; set; }
        public List<cmlResInfoPdtPmtDT> raPdtPmtDT { get; set; }
        public List<cmlResInfoPdtPmtCD> raPdtPmtCD { get; set; }
        public List<cmlResInfoPdtPmtCB> raPdtPmtCB { get; set; }    //*Arm 63-03-27       
        public List<cmlResInfoPdtPmtCG> raPdtPmtCG { get; set; }    //*Arm 63-03-27
        public List<cmlResInfoPdtPmtHD_L> raPdtPmtHD_L { get; set; }    //*Arm 63-03-27
        public List<cmlResInfoPdtPmtHDBch> raPdtPmtHDBch { get; set; }  //*Arm 63-03-27
        public List<cmlResInfoPdtPmtHDCst> raPdtPmtHDCst { get; set; }  //*Arm 63-03-27
        public List<cmlResInfoPdtPmtHDCstPri> raPdtPmtHDCstPri { get; set; }    //*Arm 63-03-27

    }
}
