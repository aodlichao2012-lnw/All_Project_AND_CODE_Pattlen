using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Coupon
{
    public class cmlResCpnDwn
    {
        public List<cmlResInfoCpn> raCpn { get; set; }
        public List<cmlResInfoCpnLng> raCpnLng { get; set; }
        
        //*Arm 63-03-28
        public List<cmlResInfoCpnHD> raCpnHD { get; set; }
        public List<cmlResInfoCpnHD_L> raCpnHD_L { get; set; }
        public List<cmlResInfoCpnHDBch> raCpnHDBch { get; set; }
        public List<cmlResInfoCpnHDCstPri> raCpnHDCstPri { get; set; }
        public List<cmlResInfoCpnHDPdt> raCpnHDPdt { get; set; }
        public List<cmlResInfoCpnDT> raCpnDT { get; set; }
        public List<cmlResInfoCpnTypeLng> raCpnType_L { get; set; }
        //++++++++++++++++

        public List<cmlResInfoCpnType> raCpnType { get; set; }
        public List<cmlResInfoCpnTypeLng> raCpnTypeLng { get; set; }
    }
}
