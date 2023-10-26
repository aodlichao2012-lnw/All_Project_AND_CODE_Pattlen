using API2PSSale.Class;
using API2PSSale.Class;
using API2PSSale.Class.Standard;
using API2PSSale.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace API2PSSale.Controllers
{
    /// <summary>
    ///     Service upload.
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/Service")]
    public class cServicesController : ApiController
    {
        [Route("Upload/Sale")]
        [HttpPost]
        public cResult<int> POST_UPLoSaleUpload(cmlTPSTSal poSale)
        {
            //TransactionScope oTrans = null/* TODO Change to default(_) if this is not a reference type */;
            cResult<int> oResult = new cResult<int>();
            cMS oMsg;
            cSP oSP;
            string tErrCode, tErrDesc, tErrAPI;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oMsg = new cMS();
                oSP = new cSP();

                oResult.roItem = 0;
                oResult.rnCount = 0;
                oResult.rtMsg = "";
                oResult.rtCode = "";

                if (poSale == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtMsg = new cMS().tMS_RespDesc700;
                    return oResult;
                }
                if (poSale.aoTPSTSalHD == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtMsg = new cMS().tMS_RespDesc700;
                    return oResult;
                }

                #region Check API Key and check comnnect database
                //if (oSP.SP_CHKbKeyApi(HttpContext.Current, out tErrAPI) == false)
                if (oSP.SP_CHKbKeyApiConfig(HttpContext.Current, out tErrAPI) == false) //*Arm 63-07-31 ยกมาจาก Moshi
                {
                    if (tErrAPI == "-1")
                    {
                        oResult.rtCode = oMsg.tMS_RespCode905;
                        oResult.rtMsg = oMsg.tMS_RespDesc905;
                        return oResult;
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtMsg = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                #endregion

                cRabbitMQ oRQ = new cRabbitMQ();
                oRQ.C_GETbLoadConfigMQ();
                if (!string.IsNullOrEmpty(cRabbitMQ.tC_HostName))
                {
                    if (!string.IsNullOrEmpty(cRabbitMQ.tC_QueueSale))
                    {
                        string tConnStr = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        string tMsgJson = oRQ.C_CRTtMsgDataUpload(Newtonsoft.Json.JsonConvert.SerializeObject(poSale), tConnStr);
                        if (oRQ.C_PRCbSendData2Srv(tMsgJson, cRabbitMQ.tC_QueueSale))
                        {
                            oResult.rtCode = new cMS().tMS_RespCode001;
                            oResult.rtMsg = new cMS().tMS_RespDesc001;
                        }
                        else
                        {
                            oResult.rtCode = new cMS().tMS_RespCode907;
                            oResult.rtMsg = new cMS().tMS_RespDesc907;
                        }
                    }
                    else
                    {
                        oResult.rtCode = new cMS().tMS_RespCode907;
                        oResult.rtMsg = new cMS().tMS_RespDesc907;
                    }
                }
                else
                {
                    oResult.rtCode = new cMS().tMS_RespCode907;
                    oResult.rtMsg = new cMS().tMS_RespDesc907;
                }
            }
            catch (Exception oEx)
            {
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtMsg = oEx.Message.ToString();
            }
            finally
            {
                oMsg = null;
                oSP = null;
            }
            return oResult;

        }

        [Route("Upload/Tax")]
        [HttpPost]
        public cResult<int> POST_UPLoTaxUpload(cmlTPSTTax poTax)
        {
            //TransactionScope oTrans = null/* TODO Change to default(_) if this is not a reference type */;
            cResult<int> oResult = new cResult<int>();
            cMS oMsg;
            cSP oSP;
            string tErrCode, tErrDesc, tErrAPI;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oMsg = new cMS();
                oSP = new cSP();

                oResult.roItem = 0;
                oResult.rnCount = 0;
                oResult.rtMsg = "";
                oResult.rtCode = "";

                if (poTax == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtMsg = new cMS().tMS_RespDesc700;
                    return oResult;
                }
                if (poTax.aoTPSTTaxHD == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtMsg = new cMS().tMS_RespDesc700;
                    return oResult;
                }

                #region Check API Key and check comnnect database
                //if (oSP.SP_CHKbKeyApi(HttpContext.Current, out tErrAPI) == false)
                if (oSP.SP_CHKbKeyApiConfig(HttpContext.Current, out tErrAPI) == false) //*Arm 63-07-31 ยกมาจาก Moshi
                {
                    if (tErrAPI == "-1")
                    {
                        oResult.rtCode = oMsg.tMS_RespCode905;
                        oResult.rtMsg = oMsg.tMS_RespDesc905;
                        return oResult;
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtMsg = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                #endregion

                cRabbitMQ oRQ = new cRabbitMQ();
                oRQ.C_GETbLoadConfigMQ();
                if (!string.IsNullOrEmpty(cRabbitMQ.tC_HostName))
                {
                    if (!string.IsNullOrEmpty(cRabbitMQ.tC_QueueTax))
                    {
                        string tConnStr = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        string tMsgJson = oRQ.C_CRTtMsgDataUpload(Newtonsoft.Json.JsonConvert.SerializeObject(poTax), tConnStr);
                        if (oRQ.C_PRCbSendData2Srv(tMsgJson, cRabbitMQ.tC_QueueTax))
                        {
                            oResult.rtCode = new cMS().tMS_RespCode001;
                            oResult.rtMsg = new cMS().tMS_RespCode001;
                        }
                        else
                        {
                            oResult.rtCode = new cMS().tMS_RespCode907;
                            oResult.rtMsg = new cMS().tMS_RespDesc907;
                        }
                    }
                    else
                    {
                        oResult.rtCode = new cMS().tMS_RespCode907;
                        oResult.rtMsg = new cMS().tMS_RespDesc907;
                    }
                }
                else
                {
                    oResult.rtCode = new cMS().tMS_RespCode907;
                    oResult.rtMsg = new cMS().tMS_RespDesc907;
                }
            }
            catch (Exception oEx)
            {
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtMsg = oEx.Message.ToString();
            }
            finally
            {
                oMsg = null;
                oSP = null;
            }
            return oResult;
        }

        [Route("Upload/ShiftSale")]
        [HttpPost]
        public cResult<int> POST_UPLoShiftUpload(cmlTPSTShift poShift)
        {
            //TransactionScope oTrans = null/* TODO Change to default(_) if this is not a reference type */;
            cResult<int> oResult = new cResult<int>();
            cMS oMsg;
            cSP oSP;
            string tErrCode, tErrDesc, tErrAPI;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oMsg = new cMS();
                oSP = new cSP();

                oResult.roItem = 0;
                oResult.rnCount = 0;
                oResult.rtMsg = "";
                oResult.rtCode = "";

                if (poShift == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtMsg = new cMS().tMS_RespDesc700;
                    return oResult;
                }
                if (poShift.aoTPSTShiftHD == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtMsg = new cMS().tMS_RespDesc700;
                    return oResult;
                }

                #region Check API Key and check comnnect database
                //if (oSP.SP_CHKbKeyApi(HttpContext.Current, out tErrAPI) == false)
                if (oSP.SP_CHKbKeyApiConfig(HttpContext.Current, out tErrAPI) == false) //*Arm 63-07-31
                {
                    if (tErrAPI == "-1")
                    {
                        oResult.rtCode = oMsg.tMS_RespCode905;
                        oResult.rtMsg = oMsg.tMS_RespDesc905;
                        return oResult;
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtMsg = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                #endregion

                cRabbitMQ oRQ = new cRabbitMQ();
                oRQ.C_GETbLoadConfigMQ();
                if (!string.IsNullOrEmpty(cRabbitMQ.tC_HostName))
                {
                    if (!string.IsNullOrEmpty(cRabbitMQ.tC_QueueShift))
                    {
                        string tConnStr = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        string tMsgJson = oRQ.C_CRTtMsgDataUpload(Newtonsoft.Json.JsonConvert.SerializeObject(poShift), tConnStr);
                        if (oRQ.C_PRCbSendData2Srv(tMsgJson, cRabbitMQ.tC_QueueShift))
                        {
                            oResult.rtCode = new cMS().tMS_RespCode001;
                            oResult.rtMsg = new cMS().tMS_RespCode001;
                        }
                        else
                        {
                            oResult.rtCode = new cMS().tMS_RespCode907;
                            oResult.rtMsg = new cMS().tMS_RespDesc907;
                        }
                    }
                    else
                    {
                        oResult.rtCode = new cMS().tMS_RespCode907;
                        oResult.rtMsg = new cMS().tMS_RespDesc907;
                    }
                }
                else
                {
                    oResult.rtCode = new cMS().tMS_RespCode907;
                    oResult.rtMsg = new cMS().tMS_RespDesc907;
                }
            }
            catch (Exception oEx)
            {
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtMsg = oEx.Message.ToString();
            }
            finally
            {
                oMsg = null;
                oSP = null;
            }
            return oResult;
        }

        [Route("Upload/Void")]
        [HttpPost]
        public cResult<int> POST_UPLoVoidUpload(cmlTPSTVoid poVoid)
        {
            //TransactionScope oTrans = null/* TODO Change to default(_) if this is not a reference type */;
            cResult<int> oResult = new cResult<int>();
            cMS oMsg;
            cSP oSP;
            string tErrCode, tErrDesc, tErrAPI;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oMsg = new cMS();
                oSP = new cSP();

                oResult.roItem = 0;
                oResult.rnCount = 0;
                oResult.rtMsg = "";
                oResult.rtCode = "";

                if (poVoid == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtMsg = new cMS().tMS_RespDesc700;
                    return oResult;
                }
                if (poVoid.aoTPSTVoidDT == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode700;
                    oResult.rtMsg = new cMS().tMS_RespDesc700;
                    return oResult;
                }

                #region Check API Key and check comnnect database
                //if (oSP.SP_CHKbKeyApi(HttpContext.Current, out tErrAPI) == false)
                if (oSP.SP_CHKbKeyApiConfig(HttpContext.Current, out tErrAPI) == false) //*Arm 63-07-31 ยกมาจาก Moshi
                {
                    if (tErrAPI == "-1")
                    {
                        oResult.rtCode = oMsg.tMS_RespCode905;
                        oResult.rtMsg = oMsg.tMS_RespDesc905;
                        return oResult;
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtMsg = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                #endregion

                cRabbitMQ oRQ = new cRabbitMQ();
                oRQ.C_GETbLoadConfigMQ();
                if (!string.IsNullOrEmpty(cRabbitMQ.tC_HostName))
                {
                    if (!string.IsNullOrEmpty(cRabbitMQ.tC_QueueVoid))
                    {
                        string tConnStr = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        string tMsgJson = oRQ.C_CRTtMsgDataUpload(Newtonsoft.Json.JsonConvert.SerializeObject(poVoid), tConnStr);
                        if (oRQ.C_PRCbSendData2Srv(tMsgJson, cRabbitMQ.tC_QueueVoid))
                        {
                            oResult.rtCode = new cMS().tMS_RespCode001;
                            oResult.rtMsg = new cMS().tMS_RespCode001;
                        }
                        else
                        {
                            oResult.rtCode = new cMS().tMS_RespCode907;
                            oResult.rtMsg = new cMS().tMS_RespDesc907;
                        }
                    }
                    else
                    {
                        oResult.rtCode = new cMS().tMS_RespCode907;
                        oResult.rtMsg = new cMS().tMS_RespDesc907;
                    }
                }
                else
                {
                    oResult.rtCode = new cMS().tMS_RespCode907;
                    oResult.rtMsg = new cMS().tMS_RespDesc907;
                }
            }
            catch (Exception oEx)
            {
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtMsg = oEx.Message.ToString();
            }
            finally
            {
                oMsg = null;
                oSP = null;
            }
            return oResult;
        }
    }
}
