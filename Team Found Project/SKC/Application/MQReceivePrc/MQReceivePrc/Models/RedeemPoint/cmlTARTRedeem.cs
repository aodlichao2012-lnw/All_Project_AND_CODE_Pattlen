using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.RedeemPoint
{
    public class cmlTARTRedeem
    {
        public List<cmlTARTRedeemHD> aoTARTRedeemHD { get; set; }
        public List<cmlTARTRedeemHD_L> aoTARTRedeemHD_L { get; set; }
        public List<cmlTARTRedeemHDBch> aoTARTRedeemHDBch { get; set; }
        public List<cmlTARTRedeemHDCstPri> aoTARTRedeemHDCstPri { get; set; }
        public List<cmlTARTRedeemDT> aoTARTRedeemDT { get; set; }
        public List<cmlTARTRedeemCD> aoTARTRedeemCD { get; set; }
    }
}
