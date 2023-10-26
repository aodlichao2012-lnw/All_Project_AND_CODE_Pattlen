using API2PSSale.Class;
using API2PSSale.Class.Standard;
using API2PSSale.Models.PdtStkCrd;
using API2PSSale.Models.UpdProductStk;
using API2PSSale.Models.WebService.Response.ResProductStk;
using API2PSSale.Models.WebService.Response.UpdPdtStkCrd;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace API2PSSale.Controllers
{
    [RoutePrefix(cCS.tCS_APIVer + "/PdtStock")]
    public class cProductStkUplController : ApiController
    {
        /// <summary>
        /// Upload Pdt Stock Card To MQ
        /// </summary>
        /// <returns>
        /// System process status.<br/>
        ///&#8195;     1   : Success.<br/>
        ///&#8195;     801 : Data is duplicate.<br/>    
        ///&#8195;     900 : Service process false.<br/>
        ///&#8195;     903 : Validate parameter encrypt false..<br/>
        ///&#8195;     904 : Key not allowed to use method.<br/>
        ///&#8195;     905 : Cannot connect database.<br/>
        /// </returns>
        //*BOY 62-11-19
        [Route("Data/UplPdtStkCrd")]
        [HttpPost]
        public cmlResUpdPdtStkCrd POST_UPLoStkCrdUpload(cmlPdtStkCrd poPdtStkCrd)
        {
            //TransactionScope oTrans = null/* TODO Change to default(_) if this is not a reference type */;
            cmlResUpdPdtStkCrd oResult = new cmlResUpdPdtStkCrd();
            cMS oMsg;
            cSP oSP;
            string tErrCode, tErrDesc, tErrAPI;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oMsg = new cMS();
                oSP = new cSP();

                oResult.rtCode = "";
                oResult.rtDesc = "";

                if (poPdtStkCrd == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtDesc = new cMS().tMS_RespDesc700;
                    return oResult;
                }

                #region Check API Key and check comnnect database
                if (oSP.SP_CHKbKeyApi(HttpContext.Current, out tErrAPI) == false)
                {
                    if (tErrAPI == "-1")
                    {
                        oResult.rtCode = oMsg.tMS_RespCode905;
                        oResult.rtDesc = oMsg.tMS_RespDesc905;
                        return oResult;
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtDesc = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                #endregion

                cRabbitMQ oRQ = new cRabbitMQ();
                oRQ.C_GETbLoadConfigMQ();
                if (!string.IsNullOrEmpty(cRabbitMQ.tC_HostName))
                {
                    if (!string.IsNullOrEmpty(cRabbitMQ.tC_QueueStkCrd))
                    {
                        string tConnStr = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        string tMsgJson = oRQ.C_CRTtMsgDataUpload(Newtonsoft.Json.JsonConvert.SerializeObject(poPdtStkCrd), tConnStr);
                        if (oRQ.C_PRCbSendData2Srv(tMsgJson, cRabbitMQ.tC_QueueStkCrd))
                        {
                            oResult.rtCode = new cMS().tMS_RespCode001;
                            oResult.rtDesc = new cMS().tMS_RespDesc001;
                        }
                        else
                        {
                            oResult.rtCode = new cMS().tMS_RespCode907;
                            oResult.rtDesc = new cMS().tMS_RespDesc907;
                        }
                    }
                    else
                    {
                        oResult.rtCode = new cMS().tMS_RespCode907;
                        oResult.rtDesc = new cMS().tMS_RespDesc907;
                    }
                }
                else
                {
                    oResult.rtCode = new cMS().tMS_RespCode907;
                    oResult.rtDesc = new cMS().tMS_RespDesc907;
                }
            }
            catch (Exception oEx)
            {
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = oEx.Message.ToString();
            }
            finally
            {
                oMsg = null;
                oSP = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            return oResult;
        }

        /// <summary>
        /// UPload Product Stock Bal To MQ
        /// </summary>
        /// <param name="poPdtStkBal"></param>
        /// <returns></returns>
        //*BOY 62-11-19
        [Route("Data/UplPdtStkBal")]
        [HttpPost]
        public cmlResUpdPdtStkBal POST_UPLoStkBalUpload(cmlPdtStkBal poPdtStkBal)
        {
            //TransactionScope oTrans = null/* TODO Change to default(_) if this is not a reference type */;
            cmlResUpdPdtStkBal oResult = new cmlResUpdPdtStkBal();
            cMS oMsg;
            cSP oSP;
            string tErrCode, tErrDesc, tErrAPI;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oMsg = new cMS();
                oSP = new cSP();

                oResult.rtCode = "";
                oResult.rtDesc = "";

                if (poPdtStkBal == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtDesc = new cMS().tMS_RespDesc700;
                    return oResult;
                }

                #region Check API Key and check comnnect database
                if (oSP.SP_CHKbKeyApi(HttpContext.Current, out tErrAPI) == false)
                {
                    if (tErrAPI == "-1")
                    {
                        oResult.rtCode = oMsg.tMS_RespCode905;
                        oResult.rtDesc = oMsg.tMS_RespDesc905;
                        return oResult;
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtDesc = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                #endregion

                cRabbitMQ oRQ = new cRabbitMQ();
                oRQ.C_GETbLoadConfigMQ();
                if (!string.IsNullOrEmpty(cRabbitMQ.tC_HostName))
                {
                    if (!string.IsNullOrEmpty(cRabbitMQ.tC_QueueStkBal))
                    {
                        string tConnStr = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        string tMsgJson = oRQ.C_CRTtMsgDataUpload(Newtonsoft.Json.JsonConvert.SerializeObject(poPdtStkBal), tConnStr);
                        if (oRQ.C_PRCbSendData2Srv(tMsgJson, cRabbitMQ.tC_QueueStkBal))
                        {
                            oResult.rtCode = new cMS().tMS_RespCode001;
                            oResult.rtDesc = new cMS().tMS_RespDesc001;
                        }
                        else
                        {
                            oResult.rtCode = new cMS().tMS_RespCode907;
                            oResult.rtDesc = new cMS().tMS_RespDesc907;
                        }
                    }
                    else
                    {
                        oResult.rtCode = new cMS().tMS_RespCode907;
                        oResult.rtDesc = new cMS().tMS_RespDesc907;
                    }
                }
                else
                {
                    oResult.rtCode = new cMS().tMS_RespCode907;
                    oResult.rtDesc = new cMS().tMS_RespDesc907;
                }
            }
            catch (Exception oEx)
            {
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = oEx.Message.ToString();
            }
            finally
            {
                oMsg = null;
                oSP = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            return oResult;
        }
    }
}

