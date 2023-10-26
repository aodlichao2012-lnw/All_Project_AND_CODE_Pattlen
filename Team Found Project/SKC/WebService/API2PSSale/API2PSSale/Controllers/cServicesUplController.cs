using API2PSSale.Class;
using API2PSSale.Class.ServiceUpd;
using API2PSSale.Class.Standard;
using API2PSSale.Models;
using API2PSSale.Models.UpdSaleVD;
using API2PSSale.Models.WebService.Response.UpdSaleVD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace API2PSSale.Controllers
{
    /// <summary>
    /// Controller Service Upload
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/Service")]
    public class cServicesUplController : ApiController
    {
        /// <summary>
        /// Upload vending
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
        [Route("Data/UplSaleVD")]
        [HttpPost]
        public cmlResUpdSaleVD POST_UploSaleVD(cmlParaEnc poPara)
        {
            cmlResUpdSaleVD oResult;
            cServiceUpd oServiceUpd;
            cDatabase oDatabase;
            cMS oMsg;
            cSP oSP;
            cmlParaUpd oParaDec;
            cmlSaleVD oSaleVD;
            cmlTVDTSalHD oTVDTSalHD;
            List<cmlTVDTSalDT> aoTVDTSalDT;
            List<cmlTVDTSalDTVD> aoTVDTSalDTVD;
            List<cmlTVDTSalRC> aoTVDTSalRC;
            string tErrCode, tErrDesc, tErrAPI;
            bool bPrc;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResUpdSaleVD();
                oServiceUpd = new cServiceUpd();
                oDatabase = new cDatabase();
                oParaDec = new cmlParaUpd();
                oSaleVD = new cmlSaleVD();
                oTVDTSalHD = new cmlTVDTSalHD();
                aoTVDTSalDT = new List<cmlTVDTSalDT>();
                aoTVDTSalDTVD = new List<cmlTVDTSalDTVD>();
                aoTVDTSalRC = new List<cmlTVDTSalRC>();
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

                #region Decrypt parameter
                oParaDec = oSP.SP_DAToAES128Decrypt<cmlParaUpd>(poPara.tUnknown, cCS.tCS_AESKey, cCS.tCS_AESIV);
                if (oParaDec == null)
                {
                    oResult.rtCode = new cMS().tMS_RespCode903;
                    oResult.rtDesc = new cMS().tMS_RespDesc903;
                    return oResult;
                }
                #endregion

                #region Covert โครงสร้าง Json to model SaveVD
                oSaleVD = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlSaleVD>(oParaDec.tData);
                #endregion

                bPrc = oServiceUpd.C_UPLbSaleVD(out tErrCode, out tErrDesc, oSaleVD);
                if (bPrc == true)
                {
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
                // Return error.
                oResult = new cmlResUpdSaleVD();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oServiceUpd = null;
                oDatabase = null;
                oSP = null;
                oParaDec = null;
                oSaleVD = null;
                oTVDTSalHD = null;
                aoTVDTSalDT = null;
                aoTVDTSalDTVD = null;
                aoTVDTSalRC = null;
                oMsg = null;
                oResult = null;
                oDatabase = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }

    }
}
