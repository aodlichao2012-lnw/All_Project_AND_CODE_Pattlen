using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.EF;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Rate;
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
    ///     Rate information.
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/PAY/Rate")]
    public class cRateController : ApiController
    {
        /// <summary>
        ///     Download rate information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResRateDwn> GET_PDToDownloadRate(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResRateDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResRateDwn oRateDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResRateDwn>();
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

                tKeyCache = "PAYRate" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResRateDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRteCode AS rtRteCode, ISNULL(FCRteRate,0) AS rcRteRate, ISNULL(FCRteFraction,0) AS rcRteFraction, FTRteType AS rtRteType,");
                oSql.AppendLine("FTRteTypeChg AS rtRteTypeChg, FTRteSign AS rtRteSign, FTRteStaLocal AS rtRteStaLocal, FTRteStaUse AS rtRteStaUse,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TFNMRate with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResRateDwn();
                oRateDwn = new cmlResRateDwn();
                using (AdaAccEntities oDB = new AdaAccEntities())
                {
                    using (DbConnection oConn = oDB.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        oCmd.CommandText = oSql.ToString();
                        using (DbDataReader oDR = oCmd.ExecuteReader())
                        {
                            oRateDwn.raRate = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRate>(oDR).ToList();
                            ((IDisposable)oDR).Dispose();
                        }

                        if (oRateDwn.raRate.Count > 0)
                        {
                            //Languague
                            oSql = new StringBuilder();
                            oSql.AppendLine("SELECT TFNMRate_L.FTRteCode AS rtRteCode, TFNMRate_L.FNLngID AS rnLngID, TFNMRate_L.FTRteName AS rtRteName,");
                            oSql.AppendLine("TFNMRate_L.FTRteShtName AS rtRteShtName, TFNMRate_L.FTRteNameText AS rtRteNameText, TFNMRate_L.FTRteDecText AS rtRteDecText");
                            oSql.AppendLine("FROM TFNMRate_L with(nolock)");
                            oSql.AppendLine("INNER JOIN TFNMRate with(nolock) ON TFNMRate_L.FTRteCode = TFNMRate.FTRteCode");
                            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRate.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                            oCmd.CommandText = oSql.ToString();
                            using (DbDataReader oDR = oCmd.ExecuteReader())
                            {
                                oRateDwn.raRateLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRateLng>(oDR).ToList();
                                ((IDisposable)oDR).Dispose();
                            }

                            //Rate Unit
                            oSql = new StringBuilder();
                            oSql.AppendLine("SELECT TFNMRateUnit.FTRteCode AS rtRteCode, TFNMRateUnit.FNRtuSeq AS rnRtuSeq, TFNMRateUnit.FCRtuFac AS rcRtuFac");
                            oSql.AppendLine("FROM TFNMRateUnit with(nolock)");
                            oSql.AppendLine("INNER JOIN TFNMRate with(nolock) ON TFNMRateUnit.FTRteCode = TFNMRate.FTRteCode");
                            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRate.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                            oCmd.CommandText = oSql.ToString();
                            using (DbDataReader oDR = oCmd.ExecuteReader())
                            {
                                oRateDwn.raRateUnit = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRateUnit>(oDR).ToList();
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

                aoResult.roItem = oRateDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRateDwn>();
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
