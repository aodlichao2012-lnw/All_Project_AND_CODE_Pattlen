using API2PSSale.Class;
using API2PSSale.Class.Standard;
using API2PSSale.Models;
using API2PSSale.Models.UpdSaleVD;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSSale.Class.ServiceUpd
{
    /// <summary>
    /// Class sevice upload
    /// </summary>
    public class cServiceUpd
    {
        /// <summary>
        /// function upload sale vending
        /// </summary>
        /// <param name="ptErrCode"></param>
        /// <param name="ptErrDesc"></param>
        /// <param name="poSaleVD"></param>
        /// <returns></returns>
        public bool C_UPLbSaleVD(out string ptErrCode, out string ptErrDesc, cmlSaleVD poSaleVD)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            ArrayList atSql = new ArrayList();
            bool bResult;
            cRabbitMQ oRQ;
            cmlRcvDataUpload oRcvDataUpload;
            string tJsonSale, tJsonDataUpd;
            try
            {
                oDatabase = new cDatabase();
                oSql = new StringBuilder();
                oRQ = new cRabbitMQ();
                oRQ.C_GETbLoadConfigMQ();   //*Em 62-08-30
                oRcvDataUpload = new cmlRcvDataUpload();

                tJsonSale = JsonConvert.SerializeObject(poSaleVD);
                oRcvDataUpload.ptData = tJsonSale;
                oRcvDataUpload.ptConnStr = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                tJsonDataUpd = JsonConvert.SerializeObject(oRcvDataUpload);
                bResult = oRQ.C_PRCbSendData2Srv(tJsonDataUpd, "UPLOADSALEVD");
                if (bResult == false)
                {
                    ptErrCode = oMsg.tMS_RespCode907;
                    ptErrDesc = oMsg.tMS_RespDesc907;
                    return false;
                }
                ptErrCode = "";
                ptErrDesc = "";
                return true;
            }
            catch (Exception oEx)
            {
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                atSql = null;
                oRQ = null;
                oRcvDataUpload = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }
    }
}