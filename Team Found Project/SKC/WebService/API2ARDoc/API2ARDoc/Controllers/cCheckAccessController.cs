using API2ARDoc.Class;
using API2ARDoc.Class.Standard;
using API2ARDoc.Class.Online;
using API2ARDoc.Models.WebService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace API2ARDoc.Controllers
{
    /// <summary>
    /// Information Class online
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/CheckAccess")]
    public class cCheckAccessController : ApiController
    {
        /// <summary>
        /// Function check online 
        /// </summary>
        /// <returns>
        /// System process status.<br/>
        ///&#8195;     1   : success.<br/>    
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        /// </returns>
        [Route("IsAccess")]
        [HttpPost]
        public cmlResIsOnline POST_CHKoIsOnline()
        {
            cMS oMsg = new cMS(); //*Arm 63-02-19 [ปรับ Standrad]
            cOnline oOnline;
            cmlResIsOnline oResult;
            string tErrCode, tErrDesc, tErrAPI;
            bool bChk;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResIsOnline();
                oOnline = new cOnline();


                bChk = oOnline.C_CHKbOnline(out tErrCode, out tErrDesc);
                if (bChk == true)
                {
                    #region Check API Key
                    if (cSP.SP_CHKbKeyApi(HttpContext.Current, out tErrAPI) == false)
                    {
                        if (tErrAPI == "-1")
                        {
                            //oResult.tCode = cMS.tMS_RespCode905;
                            //oResult.tDesc = cMS.tMS_RespDesc905;
                            oResult.tCode = oMsg.tMS_RespCode905;   //*Arm 63-02-19 [ปรับ Standrad]
                            oResult.tDesc = oMsg.tMS_RespDesc905;   //*Arm 63-02-19 [ปรับ Standrad]
                            return oResult;
                        }
                        else
                        {
                            //oResult.tCode = cMS.tMS_RespCode904;
                            //oResult.tDesc = cMS.tMS_RespDesc904;
                            oResult.tCode = oMsg.tMS_RespCode904;   //*Arm 63-02-19 [ปรับ Standrad]
                            oResult.tDesc = oMsg.tMS_RespDesc904;   //*Arm 63-02-19 [ปรับ Standrad]
                            return oResult;
                        }
                    }
                    #endregion

                    oResult.tResult = "API : Allow Access";
                    oResult.tCode = oMsg.tMS_RespCode001;   //*Arm 63-02-19 [ปรับ Standrad]
                    oResult.tDesc = oMsg.tMS_RespDesc001;   //*Arm 63-02-19 [ปรับ Standrad]
                    return oResult;
                }
                else
                {
                    oResult.tCode = tErrCode;
                    oResult.tDesc = tErrDesc;
                    return oResult;
                }
            }
            catch (Exception)
            {
                oResult = new cmlResIsOnline();
                //oResult.tCode = cMS.tMS_RespCode900;
                //oResult.tDesc = cMS.tMS_RespDesc900;
                oResult.tCode = oMsg.tMS_RespCode900;   //*Arm 63-02-19 [ปรับ Standrad]
                oResult.tDesc = oMsg.tMS_RespDesc900;   //*Arm 63-02-19 [ปรับ Standrad]
                return oResult;
            }
            finally
            {
                oOnline = null;
                oResult = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
