using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cRental
    {
        public bool C_PRCbUpdateStatus(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            try
            {

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbPdtPrice");
                cFunction.C_PRCxMQResponsce("RESAJP", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }
    }
}
