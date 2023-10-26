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
    class cEvent5
    {
        public void C_IMPxDataBySKC(string ptFunction, string ptBchCode)
        {
            new cLog().C_WRTxLog("cEvent5", "C_IMPxDataBySKC : RabbitPublish");
            string tQueueName = "LK_QImportMaster";
            cmlRequestTranferSAP oModel;
            cmlTranferSAPData oModelData;
            string tDate;
            try
            {
                Console.WriteLine("cEvent5 > C_IMPxDataBySKC : Send an ExportSell error notification");
                new cLog().C_WRTxLog("cEvent5", "C_IMPxDataBySKC : Send an ExportSell error notification ");


                oModelData = new cmlTranferSAPData();
                tDate = DateTime.Now.ToString("yyyyMMdd");
                oModelData.ptFilter = ptBchCode;
                oModelData.ptDateFrm = tDate;
                oModelData.ptDateTo = tDate;

                oModel = new cmlRequestTranferSAP();
                oModel.ptFunction = ptFunction;
                oModel.ptSource = "BQ Process";
                oModel.ptDest = "HQ.AdaLink";
                oModel.ptData = oModelData;
                string tJson = JsonConvert.SerializeObject(oModel);

                new cSP().C_PRCxMQPublish(tQueueName, tJson, cVB.tVB_VHostMaster);
                new cLog().C_WRTxLog("cEvent5", "C_IMPxDataBySKC : Send VisualHost/Queue Name: " + cVB.tVB_VHostMaster + "/" + tQueueName);
                Console.WriteLine("cEvent5 > C_IMPxDataBySKC : Send an Export error notification Success");
                new cLog().C_WRTxLog("cEvent5", "C_IMPxDataBySKC : Send an Export error notification Success");
            }
            catch (Exception oEx)
            {
                Console.WriteLine("cEvent5 > C_IMPxDataBySKC : " + oEx.Message.ToString());
                new cLog().C_WRTxLog("cEvent5", "C_IMPxDataBySKC : " + oEx.Message.ToString());
            }
        }
    }
}
