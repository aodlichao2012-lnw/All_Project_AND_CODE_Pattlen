using API2PSSale.Class;
using API2PSSale.Class.ServiceDwn;
using API2PSSale.Class.Standard;
using API2PSSale.Models.WebService.Request;
using API2PSSale.Models.WebService.Response.Base;
using API2PSSale.Models.WebService.Response.SalDT;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace API2PSSale.Controllers
{
    /// <summary>
    /// Controller Service Download
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/Service")]

    public class cServicesDwnController : ApiController
    {

        /// <summary>
        /// Download Sale DT
        /// </summary>
        /// <param name="poPara">cmlReqSalDT</param>
        /// <returns>
        ///&#8195;     1   : Success.<br/>
        ///&#8195;     701 : Validate parameter model false.<br/>
        ///&#8195;     800 : Data not found.<br/>
        ///&#8195;     900 : Service process false.<br/>
        ///&#8195;     904 : Key not allowed to use method.<br/>
        ///&#8195;     905 : Cannot connect database.<br/>
        /// </returns>
        [Route("Data/DwnSalDT")]
        [HttpPost]
        public cmlResItem<cmlResSalDT> POST_DWNoSaleDT(cmlReqSalDT poPara)
        {
            cmlResItem<cmlResSalDT> oResult;
            cmlResSalDT oSalDT;
            List<cmlResTPSTSalDT> aoTPSTSalDT;
            cServiceDwn oServiceDwn;
            cDatabase oDatabase;
            cMS oMsg;
            cSP oSP;
            string tErrCode, tErrDesc, tErrAPI;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResItem<cmlResSalDT>();
                oSalDT = new cmlResSalDT();
                aoTPSTSalDT = new List<cmlResTPSTSalDT>();

                oServiceDwn = new cServiceDwn();
                oDatabase = new cDatabase();
                oMsg = new cMS();
                oSP = new cSP();
               
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

                #region Get TPSTSalDT 
                if (!oServiceDwn.C_GETbSalDT(poPara,out aoTPSTSalDT,out tErrCode,out tErrDesc))
                {
                    oResult.rtCode = tErrCode;
                    oResult.rtDesc = tErrDesc;
                    return oResult;
                }
                #endregion

                #region Set Response
                oSalDT.raSalDT = aoTPSTSalDT;

                oResult.roItem = oSalDT;
                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;
                #endregion
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSalDT>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oMsg = null;
                oResult = null;
                oDatabase = null;
                oResult = null;
                oSalDT = null;
                aoTPSTSalDT = null;
                oServiceDwn = null;
                oDatabase = null;
                oMsg = null;
                oSP = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }
    }
}
