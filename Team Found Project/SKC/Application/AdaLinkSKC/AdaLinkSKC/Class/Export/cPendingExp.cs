using AdaLinkSKC.Model.ExportSale;
using AdaLinkSKC.Model.Receive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class.Export
{
    class cPendingExp
    {
        public bool C_PRCbPendingExp(string ptData, ref string ptErrMsg)
        {
            cmlRcvData oRcv;
            cmlDataExpSale oData;
            string tErrMsg = "";
            try
            {
                ptErrMsg = "";
                if (string.IsNullOrEmpty(ptData))
                {
                    new cLog().C_PRCxLogMonitor("C_PRCbPendingExp", "false : Parameter is Empty.");
                    ptErrMsg = "Parameter is Empty.";
                    return false;
                }
                
                oRcv = JsonConvert.DeserializeObject<cmlRcvData>(ptData);
                oData = JsonConvert.DeserializeObject<cmlDataExpSale>(oRcv.ptData);

                new cLog().C_PRCxLogMonitor("C_PRCbPendingExp", "Pending send Document No." + oData.ptDocNoFrm + " Start...");

                if (Convert.ToInt32(oData.ptRound) <= cVB.nVB_ToSnd)
                {
                    //รอการส่งตามที่กำหนด
                    new cLog().C_PRCxLogMonitor("C_PRCbPendingExp", "Wait Time Send "+ cVB.nVB_Pending + " s.");
                    Thread.Sleep(cVB.nVB_Pending);
                    new cLog().C_PRCxLogMonitor("C_PRCbPendingExp", "Process Ready...");

                    // ส่งเข้า LK_QSale2Vender
                    new cLog().C_PRCxLogMonitor("C_PRCbPendingExp", "Publish  Document No." + oData.ptDocNoFrm + " to RabbitMQ in Queue Name LK_QSale2Vender. ");
                    cFunction.C_PRCxMQPublish("LK_QSale2Vender", false, ptData, out tErrMsg);

                    //Result
                    if (string.IsNullOrEmpty(tErrMsg))
                    {
                        new cLog().C_PRCxLogMonitor("C_PRCbPendingExp", "Re-Send " + oData.ptDocNoFrm + " Success...");
                    }
                    else
                    {
                        new cLog().C_PRCxLogMonitor("C_PRCbPendingExp", "Re-Send " + oData.ptDocNoFrm + " false : " + tErrMsg);
                        ptErrMsg = tErrMsg;
                        return false;
                    }
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("C_PRCbPendingExp", "More than the specified cycle, Stop Process Send.");
                }
                return true;
            }
            catch(Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_PRCxLog("C_PRCbPendingExp", "Error: (Exception) "+ ptErrMsg);
                return false;
            }
            finally
            {
                oRcv = null;
                oData = null;
                new cSP().SP_CLExMemory();
            }
        }
        
    }
}
