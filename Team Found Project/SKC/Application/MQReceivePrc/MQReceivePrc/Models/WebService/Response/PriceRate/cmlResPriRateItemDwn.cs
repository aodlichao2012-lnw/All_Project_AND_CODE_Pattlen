using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.PriceRate
{
    public class cmlResPriRateItemDwn
    {
        public List<cmlResPriRateHD> raTRTMPriRateHD { get; set; }
        public List<cmlResPriRateHD_L> raTRTMPriRateHD_L { get; set; }
        public List<cmlResPriRateDT> raTRTMPriRateDT { get; set; }
        public List<cmlResPdtPriDOW> raTRTMPdtPriDOW { get; set; }
        public List<cmlResPdtPriHoliday> raTRTMPdtPriHoliday { get; set; }
    }
}
