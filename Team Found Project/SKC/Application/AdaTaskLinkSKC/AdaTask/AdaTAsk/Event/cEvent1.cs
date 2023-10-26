using AdaTask.Class;
using AdaTask.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Event
{
    class cEvent1
    {
        public void C_PROxRequestTranferSAPByJSON()
        {
            new cLog().C_WRTxLog("cEvent1", "C_PROxRequestTranferSAPByJSON : RabbitPublish");
            cSP oC_CSP = new cSP();
            cmlRequestTranferSAP oModel;
            cmlTranferSAPData oModelData;
            try
            {
                oModelData = new cmlTranferSAPData();
                oModelData.ptFilter = "";
                oModelData.ptDateFrm = "";
                oModelData.ptDateTo = "";

                oModel = new cmlRequestTranferSAP();
                oModel.ptFunction = "IMP";
                oModel.ptSource = "HQ.AdaStoreBack";
                oModel.ptDest = "BQ Process";
                oModel.ptData = oModelData;

                string tJson = JsonConvert.SerializeObject(oModel, Formatting.Indented);

                new cLog().C_WRTxLog("cEvent1", "C_PROxRequestTranferSAPByJSON : Import");
                Console.WriteLine("cEvent1 > C_PROxRequestTranferSAPByJSON : Import");
                string tQueueName = cVB.oVB_RabbitConfig.tQueueName;
                //oC_CSP.cSP_RabbitPublish(tExchangeName, tJson);
                oC_CSP.C_PRCxMQPublish(tQueueName, tJson,cVB.tVB_VHostMaster); //*Arm 63-07-06
                new cLog().C_WRTxLog("cEvent1", "C_PROxRequestTranferSAPByJSON : Success.");
                Console.WriteLine("cEvent1 > C_PROxRequestTranferSAPByJSON : Success.");

            }
            catch (Exception oEx)
            {
                Console.WriteLine("cEvent1 > C_PROxRequestTranferSAPByJSON() : " + oEx.Message);
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
