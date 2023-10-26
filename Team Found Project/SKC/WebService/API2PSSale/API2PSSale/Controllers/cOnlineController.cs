using API2PSSale.Class;
using API2PSSale.Class.Online;
using API2PSSale.Class.Standard;
using API2PSSale.Models.WebService.Response.Online;
using System;
using System.Collections.Generic;
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
    /// Controller check online
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/Online")]
    public class cOnlineController : ApiController
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
        [Route("Check")]
        [HttpPost]
        public cmlResIsOnline POST_CHKoIsOnline()
        {
            cMS oMsg;
            cOnline oOnline;
            cmlResIsOnline oResult;
            string tErrCode, tErrDesc, tDBName, tErrAPI;
            bool bChk;
            cDatabase oDatabase;
            cSP oSP;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResIsOnline();
                oMsg = new cMS();
                oOnline = new cOnline();
                oSP = new cSP();

                #region Check API Key
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

                bChk = oOnline.C_CHKbOnline(out tErrCode, out tErrDesc);
                if (bChk == true)
                {
                    oDatabase = new cDatabase();
                    tDBName = oDatabase.C_GETtSQLScalarString("SELECT NAME FROM SYS.sysdatabases WHERE DBID=db_id()");
                    oResult.rtResult = tDBName + " :Ready";
                    oResult.rtCode = oMsg.tMS_RespCode001;
                    oResult.rtDesc = oMsg.tMS_RespDesc001;
                    return oResult;
                }
                else
                {
                    oResult.rtCode = tErrCode;
                    oResult.rtDesc = tErrDesc;
                    return oResult;
                }
            }
            catch (Exception)
            {
                oResult = new cmlResIsOnline();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oMsg = null;
                oResult = null;
                oDatabase = null;
                tDBName = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }
        
    }
}
