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
    class cEvent2
    {
        public void C_PROxRequestTranferSAPByJSON(string ptBchCode)
        {
            new cLog().C_WRTxLog("cEvent2", "C_PROxRequestTranferSAPByJSON : RabbitPublish");
            cSP oC_CSP = new cSP();
            cmlRequestTranferSAP oModel;
            cmlTranferSAPData oModelData;
            string tDate;
            
            try
            { 
                oModelData = new cmlTranferSAPData();
                tDate = DateTime.Now.ToString("yyyyMMdd");
                oModelData.ptFilter = ptBchCode;
                oModelData.ptDateFrm = tDate;
                oModelData.ptDateTo = tDate;

                oModel = new cmlRequestTranferSAP();
                oModel.ptFunction = "EXPSALE";
                oModel.ptSource = "BQ Process";
                oModel.ptDest = "HQ.AdaStoreBack";
                oModel.ptData = oModelData;

                string tJson = JsonConvert.SerializeObject(oModel, Formatting.Indented);
                


                new cLog().C_WRTxLog("cEvent2", "C_PROxRequestTranferSAPByJSON : ExportSale");
                Console.WriteLine("cEvent2 > C_PROxRequestTranferSAPByJSON : ExportSale");
                string tQueueName = cVB.oVB_RabbitConfig.tQueueName;
                //oC_CSP.cSP_RabbitPublish(tExchangeName, tJson);
                oC_CSP.C_PRCxMQPublish(tQueueName, tJson, cVB.tVB_VHostMaster); //*Arm 63-07-06
                new cLog().C_WRTxLog("cEvent2", "C_PROxRequestTranferSAPByJSON : Success.");
                Console.WriteLine("cEvent2 > C_PROxRequestTranferSAPByJSON : Success.");

            }
            catch (Exception oEx)
            {
                Console.WriteLine("cEvent2 > C_PROxRequestTranferSAPByJSON() : " + oEx.Message);
            }
            finally
            {
                oC_CSP.SP_CLExMemory();
                oC_CSP = null;
                //aoShop = null;
            }
        }
    }
}
