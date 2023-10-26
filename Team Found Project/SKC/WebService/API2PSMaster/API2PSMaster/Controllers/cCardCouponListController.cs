using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.EF;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Coupon;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Manage card coupon list.
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/PAY/CrdCpnList")]
    public class cCardCouponListController : ApiController
    {
        /// <summary>
        ///     Download card coupon list information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResCrdCpnListDwn> GET_PDToDownloadCardCouponList(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResCrdCpnListDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResCrdCpnListDwn oCrdCpnListDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResCrdCpnListDwn>();
                //aoResult = new cmlResPdtItemDwn();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(21600, 21600, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    aoResult.rtCode = oMsg.tMS_RespCode701;
                    aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return aoResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext.Current) == false)
                {
                    // Key not allowed to use method.
                    aoResult.rtCode = oMsg.tMS_RespCode904;
                    aoResult.rtDesc = oMsg.tMS_RespDesc904;
                    return aoResult;
                }

                tKeyCache = "PAYCardCouponList" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResCrdCpnListDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCclCode AS rtCclCode, FCCclAmt AS rcCclAmt, FDCclStartDate AS rdCclStartDate,");
                oSql.AppendLine("FDCclEndDate AS rdCclEndDate, FTCclStaUse AS rtCclStaUse,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TFNMCrdCpnList with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResCrdCpnListDwn();
                oCrdCpnListDwn = new cmlResCrdCpnListDwn();
                using (AdaAccEntities oDB = new AdaAccEntities())
                {
                    using (DbConnection oConn = oDB.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        oCmd.CommandText = oSql.ToString();
                        using (DbDataReader oDR = oCmd.ExecuteReader())
                        {
                            oCrdCpnListDwn.raCrdCpnList = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCrdCpnList>(oDR).ToList();
                            ((IDisposable)oDR).Dispose();
                        }

                        if (oCrdCpnListDwn.raCrdCpnList.Count > 0)
                        {
                            //Languague
                            oSql = new StringBuilder();
                            oSql.AppendLine("SELECT TFNMCrdCpnList_L.FTCclCode AS rtCclCode, TFNMCrdCpnList_L.FNLngID AS rnLngID,");
                            oSql.AppendLine("TFNMCrdCpnList_L.FTCclName AS rtCclName, TFNMCrdCpnList_L.FTCclPrnCond AS rtCclPrnCond");
                            oSql.AppendLine("FROM TFNMCrdCpnList_L with(nolock)");
                            oSql.AppendLine("INNER JOIN TFNMCrdCpnList with(nolock) ON TFNMCrdCpnList_L.FTCclCode = TFNMCrdCpnList.FTCclCode");
                            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMCrdCpnList.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                            oCmd.CommandText = oSql.ToString();
                            using (DbDataReader oDR = oCmd.ExecuteReader())
                            {
                                oCrdCpnListDwn.raCrdCpnListLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCrdCpnListLng>(oDR).ToList();
                                ((IDisposable)oDR).Dispose();
                            }
                        }
                        else
                        {
                            aoResult.rtCode = oMsg.tMS_RespCode800;
                            aoResult.rtDesc = oMsg.tMS_RespDesc800;
                            return aoResult;
                        }
                    }
                }

                aoResult.roItem = oCrdCpnListDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCrdCpnListDwn>();
                //aoResult = new cmlResPdtItemDwn();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oExcept.Message.ToString();
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oCS = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
    }
}
