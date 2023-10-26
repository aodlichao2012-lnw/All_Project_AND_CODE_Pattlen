using AdaTask.Class;
using AdaTask.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Event
{
    class cEvent9
    {
        public void C_PROxReqStockTransfer(string ptFunction,string ptBchCode)
        {
            new cLog().C_WRTxLog("cEvent9", "C_PROxReqStockTransfer : RabbitPublish");
            string tQueueName = "LK_QImportMaster";
            cmlRequestTranferSAP oModel;
            cmlTranferSAPData oModelData;
            string tDate;
            try
            {
                Console.WriteLine("cEvent9 > C_PROxReqStockTransfer : Send an ExportSell error notification");
                new cLog().C_WRTxLog("cEvent9", "C_PROxReqStockTransfer : Send an ExportSell error notification ");

               
                oModelData = new cmlTranferSAPData();
                tDate = DateTime.Now.ToString("yyyyMMdd");
                oModelData.ptFilter = ptBchCode;
                oModelData.ptDateFrm = tDate;
                oModelData.ptDateTo = tDate;

                oModel = new cmlRequestTranferSAP();
                oModel.ptFunction = ptFunction;
                oModel.ptSource = "BQ Process";
                oModel.ptDest = "HQ.AdaStoreBack";
                oModel.ptData = oModelData;
                string tJson = JsonConvert.SerializeObject(oModel);

                new cSP().C_PRCxMQPublish(tQueueName, tJson, cVB.tVB_VHostSale);
                new cLog().C_WRTxLog("cEvent9", "C_PROxReqStockTransfer : Send VisualHost/Queue Name: " + cVB.tVB_VHostSale + "/" + tQueueName);
                Console.WriteLine("cEvent9 > C_PROxReqStockTransfer : Send an ExportSell error notification Success");
                new cLog().C_WRTxLog("cEvent9", "C_PROxReqStockTransfer : Send an ExportSell error notification Success");
            }
            catch (Exception oEx)
            {
                Console.WriteLine("cEvent9 > C_PROxReqStockTransfer : " + oEx.Message.ToString());
                new cLog().C_WRTxLog("cEvent9", "C_PROxReqStockTransfer : " + oEx.Message.ToString());
            }
        }
    }
}
