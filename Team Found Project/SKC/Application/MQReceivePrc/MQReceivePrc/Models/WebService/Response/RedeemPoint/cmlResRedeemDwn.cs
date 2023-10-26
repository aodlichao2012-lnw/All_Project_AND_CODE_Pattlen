using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.RedeemPoint
{
    public class cmlResRedeemDwn
    {
        public List<cmlResInfoRedeemHD> raRedeemHD { get; set; }
        public List<cmlResInfoRedeemHDLng> raRedeemHDLng { get; set; }
        public List<cmlResInfoRedeemHDBch> raRedeemHDBch { get; set; }
        public List<cmlResInfoRedeemHDCstPri> raRedeemHDCstPri { get; set; }
        public List<cmlResInfoRedeemDT> raRedeemDT { get; set; }
        public List<cmlResInfoRedeemCD> raRedeemCD { get; set; }

    }
}
