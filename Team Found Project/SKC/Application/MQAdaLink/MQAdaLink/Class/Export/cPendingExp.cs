using MQAdaLink.Model.ExportSale;
using MQAdaLink.Model.Receive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQAdaLink.Class.Export
{
    class cPendingExp
    {
        
        public bool C_PRCbPendingExp(string ptData, ref string ptErrMsg)
        {
            cmlRcvData oRcv;
            cmlDataExpSale oData;
            string tErrMsg = "";
            string tTask = "Pending";
            try
            {
                ptErrMsg = "";
                if (string.IsNullOrEmpty(ptData))
                {
                    new cLog().C_PRCxLogMonitor("cPendingExp","C_PRCbPendingExp : false : Parameter is Empty.", tTask);
                    ptErrMsg = "Parameter is Empty.";
                    return false;
                }
                
                oRcv = JsonConvert.DeserializeObject<cmlRcvData>(ptData);
                oData = JsonConvert.DeserializeObject<cmlDataExpSale>(oRcv.ptData);

                new cLog().C_PRCxLogMonitor("cPendingExp", "C_PRCbPendingExp : Pending send Document No." + oData.ptDocNoFrm + " Start...", tTask);

                if (Convert.ToInt32(oData.ptRound) <= cVB.nVB_ToSnd)
                {
                    //รอการส่งตามที่กำหนด
                    new cLog().C_PRCxLogMonitor("cPendingExp", "C_PRCbPendingExp : Wait Time Send " + cVB.nVB_Pending + " s.", tTask);
                    Thread.Sleep(cVB.nVB_Pending * 1000);
                    new cLog().C_PRCxLogMonitor("cPendingExp", "C_PRCbPendingExp : Process Ready...", tTask);

                    // ส่งเข้า LK_QSale2Vender
                    new cLog().C_PRCxLogMonitor("cPendingExp", "C_PRCbPendingExp : Publish  Document No." + oData.ptDocNoFrm + " to RabbitMQ in Queue Name LK_QSale2Vender. ", tTask);
                    cFunction.C_PRCxMQPublish("LK_QSale2Vender", false, ptData, out tErrMsg, tTask);

                    //Result
                    if (string.IsNullOrEmpty(tErrMsg))
                    {
                        new cLog().C_PRCxLogMonitor("cPendingExp", "C_PRCbPendingExp : Re-Send " + oData.ptDocNoFrm + " Success...", tTask);
                    }
                    else
                    {
                        new cLog().C_PRCxLogMonitor("cPendingExp", "C_PRCbPendingExp : Re-Send " + oData.ptDocNoFrm + " false : " + tErrMsg, tTask);
                        ptErrMsg = tErrMsg;
                        return false;
                    }
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("cPendingExp", "C_PRCbPendingExp : More than the specified cycle, Stop Process Send.", tTask);
                }
                return true;
            }
            catch(Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_PRCxLog("cPendingExp", "C_PRCbPendingExp : Error (Exception)/ " + ptErrMsg);
                new cLog().C_PRCxLogMonitor("cPendingExp", "C_PRCbPendingExp : Error/ " + ptErrMsg, tTask); //*Arm 63-08-27
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
