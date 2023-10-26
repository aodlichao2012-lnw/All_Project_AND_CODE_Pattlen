using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.EF;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.Base;
using API2PSMaster.Models.WebService.Request.Document;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Document;
using Dapper;
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
using System.Transactions;
using System.Web;
using System.Web.Http;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Document transfer.
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/Document")]
    public class cDocPdtTransfersController : ApiController
    {
        private AdaAccEntities oC_AdaAccDB;

        /// <summary>
        /// Download document transfers.
        /// </summary>
        /// <param name="pdDate">Last date update</param>
        /// <param name="ptBchCode">for branch</param>
        /// <returns></returns>
        [Route("Transfers/Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtTnfDwn> GET_DOCoDownloadPdtTnf(DateTime pdDate,string ptBchCode)
        {

            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtTnfDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtTnfDwn oPdtTnfDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtTnfDwn>();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

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

                //tKeyCache = "ProductPromotion" + string.Format("{0:yyyyMMdd}", pdDate);
                //if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                //{
                //    // ถ้ามี key อยุ่ใน cache
                //    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResPdtTnfDwn>>(tKeyCache);
                //    aoResult.rtCode = oMsg.tMS_RespCode001;
                //    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                //    return aoResult;
                //}

                // Get data
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTXthDocNo AS rtXthDocNo, FTShpCode AS rtShpCode, FTXthTnfType AS rtXthTnfType,");
                //oSql.AppendLine("FNXthDocType AS rnXthDocType, FDXthDocDate AS rdXthDocDate, FTXthVATInOrEx AS rtXthVATInOrEx, FTXthCshOrCrd AS rtXthCshOrCrd,");
                //oSql.AppendLine("FTXthOther AS rtXthOther, FTDptCode AS rtDptCode, FTXthBchFrm AS rtXthBchFrm, FTXthBchTo AS rtXthBchTo,");
                //oSql.AppendLine("FTXthShopFrm AS rtXthShopFrm, FTXthShopTo AS rtXthShopTo, FTXthWhFrm AS rtXthWhFrm, FTXthWhTo AS rtXthWhTo,");
                //oSql.AppendLine("FTUsrCode AS rtUsrCode, FTSpnCode AS rtSpnCode, FTXthApvCode AS rtXthApvCode, FTSplCode AS rtSplCode,");
                //oSql.AppendLine("FTXthRefExt AS rtXthRefExt, FDXthRefExtDate AS rdXthRefExtDate, FTXthRefInt AS rtXthRefInt, FDXthRefIntDate AS rdXthRefIntDate,");
                //oSql.AppendLine("FNXthDocPrint AS rnXthDocPrint, FTRteCode AS rtRteCode, FCXthRteFac AS rcXthRteFac, FTVatCode AS rtVatCode,");
                //oSql.AppendLine("FCXthVATRate AS rcXthVATRate, FCXthTotal AS rcXthTotal, FCXtVatNoDisChg AS rcXtVatNoDisChg, FCXthNoVatNoDisChg AS rcXthNoVatNoDisChg,");
                //oSql.AppendLine("FCXthVatDisChgAvi AS rcXthVatDisChgAvi, FCXthNoVatDisChgAvi AS rcXthNoVatDisChgAvi, FTXthDisChgTxt AS rtXthDisChgTxt, FCXthDis AS rcXthDis,");
                //oSql.AppendLine("FCXthChg AS rcXthChg, FCXthRefAEAmt AS rcXthRefAEAmt, FCXthVatAfDisChg AS rcXthVatAfDisChg, FCXthNoVatAfDisChg AS rcXthNoVatAfDisChg,");
                //oSql.AppendLine("FCXthAfDisChgAE AS rcXthAfDisChgAE, FTXthWpCode AS rtXthWpCode, FCXthVat AS rcXthVat, FCXthVatable AS rcXthVatable,");
                //oSql.AppendLine("FCXthGrandB4Wht AS rcXthGrandB4Wht, FCXthWpTax AS rcXthWpTax, FCXthGrand AS rcXthGrand, FCXthRnd AS rcXthRnd,");
                //oSql.AppendLine("FTXthGndText AS rtXthGndText, FCXthPaid AS rcXthPaid, FCXthLeft AS rcXthLeft, FTXthStaRefund AS rtXthStaRefund,");
                //oSql.AppendLine("FTXthRmk AS rtXthRmk, FTXthStaDoc AS rtXthStaDoc, FTXthStaApv AS rtXthStaApv, FTXthStaPrcStk AS rtXthStaPrcStk,");
                //oSql.AppendLine("FTXthStaPaid AS rtXthStaPaid, FNXthStaDocAct AS rnXthStaDocAct, FNXthStaRef AS rnXthStaRef, FTRsnCode AS rtRsnCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNTPdtTnfHD with(nolock)");
                //*Em 62-06-18
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTXthDocNo AS rtXthDocNo, FDXthDocDate AS rdXthDocDate, FTXthVATInOrEx AS rtXthVATInOrEx,");
                oSql.AppendLine("FTDptCode AS rtDptCode, FTXthBchFrm AS rtXthBchFrm, FTXthBchTo AS rtXthBchTo, FTXthMerchantFrm AS rtXthMerchantFrm,");
                oSql.AppendLine("FTXthMerchantTo AS rtXthMerchantTo, FTXthShopFrm AS rtXthShopFrm, FTXthShopTo AS rtXthShopTo, FTXthWhFrm AS rtXthWhFrm,");
                oSql.AppendLine("FTXthWhTo AS rtXthWhTo, FTUsrCode AS rtUsrCode, FTSpnCode AS rtSpnCode, FTXthApvCode AS rtXthApvCode,");
                oSql.AppendLine("FTXthRefExt AS rtXthRefExt, FDXthRefExtDate AS rdXthRefExtDate, FTXthRefInt AS rtXthRefInt, FDXthRefIntDate AS rdXthRefIntDate,");
                oSql.AppendLine("FNXthDocPrint AS rnXthDocPrint, FCXthTotal AS rcXthTotal, FCXthVat AS rcXthVat, FCXthVatable AS rcXthVatable,");
                oSql.AppendLine("FTXthRmk AS rtXthRmk, FTXthStaDoc AS rtXthStaDoc, FTXthStaApv AS rtXthStaApv, FTXthStaPrcStk AS rtXthStaPrcStk,");
                oSql.AppendLine("FTXthStaDelMQ AS rtXthStaDelMQ, FNXthStaDocAct AS rnXthStaDocAct, FNXthStaRef AS rnXthStaRef, FTRsnCode AS rtRsnCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNTPdtTbxHD with(nolock)");
                //+++++++++++++
                oSql.AppendLine("WHERE ISNULL(FTXthStaDoc,'') = '1' AND ISNULL(FTXthStaApv,'') = '1'");
                oSql.AppendLine("AND ISNULL(FTXthBchTo,'') = '"+ ptBchCode +"'");
                oSql.AppendLine("AND CONVERT(VARCHAR(19), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", pdDate) + "'");

                aoResult.roItem = new cmlResPdtTnfDwn();
                oPdtTnfDwn = new cmlResPdtTnfDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())    //*Em 62-06-18
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oPdtTnfDwn.raHD = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTnfHD>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    oPdtTnfDwn.raHD = oConn.Query<cmlResInfoPdtTnfHD>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-18
                    if (oPdtTnfDwn.raHD.Count > 0)
                    {
                        //Header refer
                        //oSql = new StringBuilder();
                        //oSql.AppendLine("SELECT TCNTPdtTnfHDRef.FTBchCode AS rtBchCode, TCNTPdtTnfHDRef.FTXthDocNo AS rtXthDocNo, TCNTPdtTnfHDRef.FTXphDstPaid AS rtXphDstPaid,");
                        //oSql.AppendLine("TCNTPdtTnfHDRef.FNXphCrTerm AS rnXphCrTerm, TCNTPdtTnfHDRef.FDXphDueDate AS rdXphDueDate, TCNTPdtTnfHDRef.FDXphBillDue AS rdXphBillDue,");
                        //oSql.AppendLine("TCNTPdtTnfHDRef.FTXphCtrName AS rtXphCtrName, TCNTPdtTnfHDRef.FDXphTnfDate AS rdXphTnfDate, TCNTPdtTnfHDRef.FTXphRefTnfID AS rtXphRefTnfID,");
                        //oSql.AppendLine("TCNTPdtTnfHDRef.FTXphRefVehID AS rtXphRefVehID, TCNTPdtTnfHDRef.FTXphRefInvNo AS rtXphRefInvNo, TCNTPdtTnfHDRef.FTXphQtyAndTypeUnit AS rtXphQtyAndTypeUnit,");
                        //oSql.AppendLine("TCNTPdtTnfHDRef.FNXphShipAdd AS rnXphShipAdd, TCNTPdtTnfHDRef.FNXphTaxAdd AS rnXphTaxAdd, TCNTPdtTnfHDRef.FTViaCode AS rtViaCode");
                        //oSql.AppendLine("FROM TCNTPdtTnfHDRef with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNTPdtTnfHD with(nolock) ON TCNTPdtTnfHDRef.FTXthDocNo = TCNTPdtTnfHD.FTXthDocNo");
                        //oSql.AppendLine("WHERE ISNULL(TCNTPdtTnfHD.FTXthStaDoc,'') = '1' AND ISNULL(TCNTPdtTnfHD.FTXthStaApv,'') = '1'");
                        //oSql.AppendLine("AND ISNULL(TCNTPdtTnfHD.FTXthBchTo,'') = '" + ptBchCode + "'");
                        //oSql.AppendLine("AND CONVERT(VARCHAR(19), TCNTPdtTnfHD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtTnfDwn.raHDRef = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTnfHDRef>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}

                        //*Em 62-06-18
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNTPdtTbxHDRef.FTBchCode AS rtBchCode, TCNTPdtTbxHDRef.FTXthDocNo AS rtXthDocNo, TCNTPdtTbxHDRef.FTXthCtrName AS rtXthCtrName,");
                        oSql.AppendLine("TCNTPdtTbxHDRef.FDXthTnfDate AS rdXthTnfDate, TCNTPdtTbxHDRef.FTXthRefTnfID AS rtXthRefTnfID, TCNTPdtTbxHDRef.FTXthRefVehID AS rtXthRefVehID,");
                        oSql.AppendLine("TCNTPdtTbxHDRef.FTXthQtyAndTypeUnit AS rtXthQtyAndTypeUnit, TCNTPdtTbxHDRef.FNXthShipAdd AS rnXthShipAdd, TCNTPdtTbxHDRef.FTViaCode AS rtViaCode");
                        oSql.AppendLine("FROM TCNTPdtTbxHDRef with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNTPdtTbxHD with(nolock) ON TCNTPdtTbxHDRef.FTXthDocNo = TCNTPdtTbxHD.FTXthDocNo");
                        oSql.AppendLine("WHERE ISNULL(TCNTPdtTbxHD.FTXthStaDoc,'') = '1' AND ISNULL(TCNTPdtTnfHD.FTXthStaApv,'') = '1'");
                        oSql.AppendLine("AND ISNULL(TCNTPdtTbxHD.FTXthBchTo,'') = '" + ptBchCode + "'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(19), TCNTPdtTbxHD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", pdDate) + "'");
                        oPdtTnfDwn.raHDRef = oConn.Query<cmlResInfoPdtTnfHDRef>(oSql.ToString(), nCmdTme).ToList();    
                        //+++++++++++++++++++++++++

                        //Detail
                        //oSql = new StringBuilder();
                        //oSql.AppendLine("SELECT TCNTPdtTnfDT.FTBchCode AS rtBchCode, TCNTPdtTnfDT.FTXthDocNo AS rtXthDocNo, TCNTPdtTnfDT.FNXtdSeqNo AS rnXtdSeqNo,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FTPdtCode AS rtPdtCode, TCNTPdtTnfDT.FTXtdPdtName AS rtXtdPdtName, TCNTPdtTnfDT.FTXtdStkCode AS rtXtdStkCode,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FCXtdStkFac AS rcXtdStkFac, TCNTPdtTnfDT.FTPunCode AS rtPunCode, TCNTPdtTnfDT.FTPunName AS rtPunName,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FCXtdFactor AS rcXtdFactor, TCNTPdtTnfDT.FTXtdBarCode AS rtXtdBarCode, TCNTPdtTnfDT.FTXtdVatType AS rtXtdVatType,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FTVatCode AS rtVatCode, TCNTPdtTnfDT.FCXtdVatRate AS rcXtdVatRate, TCNTPdtTnfDT.FTXtdSaleType AS rtXtdSaleType,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FCXtdSalePrice AS rcXtdSalePrice, TCNTPdtTnfDT.FCXtdQty AS rcXtdQty, TCNTPdtTnfDT.FCXtdQtyAll AS rcXtdQtyAll,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FCXtdSetPrice AS rcXtdSetPrice, TCNTPdtTnfDT.FCXtdAmt AS rcXtdAmt, TCNTPdtTnfDT.FCXtdDisChgAvi AS rcXtdDisChgAvi,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FTXtdDisChgTxt AS rtXtdDisChgTxt, TCNTPdtTnfDT.FCXtdDis AS rcXtdDis, TCNTPdtTnfDT.FCXtdChg AS rcXtdChg,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FCXtdNet AS rcXtdNet, TCNTPdtTnfDT.FCXtdNetAfHD AS rcXtdNetAfHD, TCNTPdtTnfDT.FCXtdNetEx AS rcXtdNetEx,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FCXtdVat AS rcXtdVat, TCNTPdtTnfDT.FCXtdVatable AS rcXtdVatable, TCNTPdtTnfDT.FCXtdWhtAmt AS rcXtdWhtAmt,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FTXtdWhtCode AS rtXtdWhtCode, TCNTPdtTnfDT.FCXtdWhtRate AS rcXtdWhtRate, TCNTPdtTnfDT.FCXtdCostIn AS rcXtdCostIn,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FCXtdCostEx AS rcXtdCostEx, TCNTPdtTnfDT.FTXtdStaPdt AS rtXtdStaPdt, TCNTPdtTnfDT.FCXtdQtyLef AS rcXtdQtyLef,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FCXtdQtyRfn AS rcXtdQtyRfn, TCNTPdtTnfDT.FTXtdStaPrcStk AS rtXtdStaPrcStk, TCNTPdtTnfDT.FTXtdStaAlwDis AS rtXtdStaAlwDis,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FNXtdPdtLevel AS rnXtdPdtLevel, TCNTPdtTnfDT.FTXtdPdtParent AS rtXtdPdtParent, TCNTPdtTnfDT.FCXtdQtySet AS rcXtdQtySet,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FTXtdPdtStaSet AS rtXtdPdtStaSet, TCNTPdtTnfDT.FTXtdRmk AS rtXtdRmk,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FDLastUpdOn AS rdLastUpdOn, TCNTPdtTnfDT.FTLastUpdBy AS rtLastUpdBy,");
                        //oSql.AppendLine("TCNTPdtTnfDT.FDCreateOn AS rdCreateOn, TCNTPdtTnfDT.FTCreateBy AS rtCreateBy");
                        //oSql.AppendLine("FROM TCNTPdtTnfDT with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNTPdtTnfHD with(nolock) ON TCNTPdtTnfDT.FTXthDocNo = TCNTPdtTnfHD.FTXthDocNo");
                        //oSql.AppendLine("WHERE ISNULL(TCNTPdtTnfHD.FTXthStaDoc,'') = '1' AND ISNULL(TCNTPdtTnfHD.FTXthStaApv,'') = '1'");
                        //oSql.AppendLine("AND ISNULL(TCNTPdtTnfHD.FTXthBchTo,'') = '" + ptBchCode + "'");
                        //oSql.AppendLine("AND CONVERT(VARCHAR(19), TCNTPdtTnfHD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtTnfDwn.raDT = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTnfDT>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}

                        //*Em 62-06-18
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNTPdtTbxDT.FTBchCode AS rtBchCode, TCNTPdtTbxDT.FTXthDocNo AS rtXthDocNo, TCNTPdtTbxDT.FNXtdSeqNo AS rnXtdSeqNo,");
                        oSql.AppendLine("TCNTPdtTbxDT.FTPdtCode AS rtPdtCode, TCNTPdtTbxDT.FTXtdPdtName AS rtXtdPdtName, TCNTPdtTbxDT.FTPunCode AS rtPunCode,");
                        oSql.AppendLine("TCNTPdtTbxDT.FTPunName AS rtPunName, TCNTPdtTbxDT.FCXtdFactor AS rcXtdFactor, TCNTPdtTbxDT.FTXtdBarCode AS rtXtdBarCode,");
                        oSql.AppendLine("TCNTPdtTbxDT.FTXtdVatType AS rtXtdVatType, TCNTPdtTbxDT.FTVatCode AS rtVatCode, TCNTPdtTbxDT.FCXtdVatRate AS rcXtdVatRate,");
                        oSql.AppendLine("TCNTPdtTbxDT.FCXtdQty AS rcXtdQty, TCNTPdtTbxDT.FCXtdQtyAll AS rcXtdQtyAll, TCNTPdtTbxDT.FCXtdSetPrice AS rcXtdSetPrice,");
                        oSql.AppendLine("TCNTPdtTbxDT.FCXtdAmt AS rcXtdAmt, TCNTPdtTbxDT.FCXtdVat AS rcXtdVat, TCNTPdtTbxDT.FCXtdVatable AS rcXtdVatable,");
                        oSql.AppendLine("TCNTPdtTbxDT.FCXtdNet AS rcXtdNet, TCNTPdtTbxDT.FCXtdCostIn AS rcXtdCostIn, TCNTPdtTbxDT.FCXtdCostEx AS rcXtdCostEx,");
                        oSql.AppendLine("TCNTPdtTbxDT.FTXtdStaPrcStk AS rtXtdStaPrcStk, TCNTPdtTbxDT.FNXtdPdtLevel AS rnXtdPdtLevel, TCNTPdtTbxDT.FTXtdPdtParent AS rtXtdPdtParent,");
                        oSql.AppendLine("TCNTPdtTbxDT.FCXtdQtySet AS rcXtdQtySet, TCNTPdtTbxDT.FTXtdPdtStaSet AS rtXtdPdtStaSet, TCNTPdtTbxDT.FTXtdRmk AS rtXtdRmk,");
                        oSql.AppendLine("TCNTPdtTbxDT.FDLastUpdOn AS rdLastUpdOn, TCNTPdtTbxDT.FTLastUpdBy AS rtLastUpdBy,");
                        oSql.AppendLine("TCNTPdtTbxDT.FDCreateOn AS rdCreateOn, TCNTPdtTbxDT.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNTPdtTbxDT with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNTPdtTbxHD with(nolock) ON TCNTPdtTbxDT.FTXthDocNo = TCNTPdtTbxHD.FTXthDocNo");
                        oSql.AppendLine("WHERE ISNULL(TCNTPdtTbxHD.FTXthStaDoc,'') = '1' AND ISNULL(TCNTPdtTbxHD.FTXthStaApv,'') = '1'");
                        oSql.AppendLine("AND ISNULL(TCNTPdtTbxHD.FTXthBchTo,'') = '" + ptBchCode + "'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(19), TCNTPdtTbxHD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", pdDate) + "'");
                        oPdtTnfDwn.raDT = oConn.Query<cmlResInfoPdtTnfDT>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++++++++++++++

                        //Serial detail
                        //oSql = new StringBuilder();
                        //oSql.AppendLine("SELECT TCNTPdtTnfDTSrn.FTBchCode AS rtBchCode, TCNTPdtTnfDTSrn.FTXthDocNo AS rtXthDocNo, TCNTPdtTnfDTSrn.FNXtdSeqNo AS rnXtdSeqNo,");
                        //oSql.AppendLine("TCNTPdtTnfDTSrn.FTPdtCode AS rtPdtCode, TCNTPdtTnfDTSrn.FTSrnCode AS rtSrnCode, TCNTPdtTnfDTSrn.FDXtdSDate AS rdXtdSDate,");
                        //oSql.AppendLine("TCNTPdtTnfDTSrn.FCPtsCost AS rcPtsCost");
                        //oSql.AppendLine("FROM TCNTPdtTnfDTSrn with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNTPdtTnfHD with(nolock) ON TCNTPdtTnfDTSrn.FTXthDocNo = TCNTPdtTnfHD.FTXthDocNo");
                        //oSql.AppendLine("WHERE ISNULL(TCNTPdtTnfHD.FTXthStaDoc,'') = '1' AND ISNULL(TCNTPdtTnfHD.FTXthStaApv,'') = '1'");
                        //oSql.AppendLine("AND ISNULL(TCNTPdtTnfHD.FTXthBchTo,'') = '" + ptBchCode + "'");
                        //oSql.AppendLine("AND CONVERT(VARCHAR(19), TCNTPdtTnfHD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtTnfDwn.raDTSrn = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTnfDTSrn>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}

                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oPdtTnfDwn;
                // เก็บ KeyApi ลง Cache
                //oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtTnfDwn>();
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

        [Route("Transfers/Upload")]
        [HttpPost]
        public cmlResult<int> POST_DOCoUploadPdtTransfer(cmlParaEnc poPara)
        {
            TransactionScope oTrans = null/* TODO Change to default(_) if this is not a reference type */;
            cmlParaUld oParaDec = new cmlParaUld();
            cTCNTPdtTnf oParaData = new cTCNTPdtTnf();
            cmlResult<int> oResult = new cmlResult<int>();
            Thread oThred;
            string tFuncName, tModelErr, tKeyApi, tCode;
            int nRowEff, nCmdTme;
            cSP oFunc;
            List<cmlTSysConfig> aoSysConfig;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();

                oResult.oItem = 0;
                oResult.nCount = 0;
                oResult.tMsg = "";

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    //oResult.rtCode = oMsg.tMS_RespCode701;
                    //oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    //return oResult;
                    oResult.tMsg = "701";
                    return oResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext.Current) == false)
                {
                    // Key not allowed to use method.
                    //oResult.rtCode = oMsg.tMS_RespCode904;
                    //oResult.rtDesc = oMsg.tMS_RespDesc904;
                    //return oResult;
                    oResult.tMsg = "904";
                    return oResult;
                }

                if (poPara == null)
                {
                    oResult.tMsg = "700";
                    return oResult;
                }

                oParaDec = cSP.SP_DAToAES128Decrypt<cmlParaUld>(poPara.tUnknown, cCS.tCS_AESKey, cCS.tCS_AESIV);
                if (oParaDec == null)
                {
                    oResult.tMsg = "903";
                    return oResult;
                }

                oParaData = Newtonsoft.Json.JsonConvert.DeserializeObject<cTCNTPdtTnf>(oParaDec.tData);
                if (oParaData == null)
                {
                    oResult.tMsg = "701";
                    return oResult;
                }

                oTrans = new TransactionScope();
                var oQryTnfHD = oC_AdaAccDB.TCNTPdtTnfHDs.Where(c => c.FTXthDocNo == oParaData.oTCNTPdtTnfHD.FTXthDocNo).FirstOrDefault();
                if (oQryTnfHD != null)
                {

                    oC_AdaAccDB.TCNTPdtTnfHDs.Remove(oQryTnfHD);
                    if (oParaData.aTCNTPdtTnfHDRef != null && oParaData.aTCNTPdtTnfHDRef.ToList().Count > 0)
                    {
                        var oQryTnfHDRef = oC_AdaAccDB.TCNTPdtTnfHDRefs.Where(c => c.FTXthDocNo == oParaData.oTCNTPdtTnfHD.FTXthDocNo).ToList();
                        foreach (var oItemTnfHDRef in oQryTnfHDRef) oC_AdaAccDB.TCNTPdtTnfHDRefs.Remove(oItemTnfHDRef);
                    }

                    if (oParaData.aTCNTPdtTnfDT != null && oParaData.aTCNTPdtTnfDT.ToList().Count > 0)
                    {
                        var oQryTnfDT = oC_AdaAccDB.TCNTPdtTnfDTs.Where(c => c.FTXthDocNo == oParaData.oTCNTPdtTnfHD.FTXthDocNo).ToList();
                        foreach (var oItemTnfDT in oQryTnfDT) oC_AdaAccDB.TCNTPdtTnfDTs.Remove(oItemTnfDT);
                    }

                    if (oParaData.aTCNTPdtTnfDTSrn != null && oParaData.aTCNTPdtTnfDTSrn.ToList().Count > 0)
                    {
                        var oQryTnfDTSrn = oC_AdaAccDB.TCNTPdtTnfDTSrns.Where(c => c.FTXthDocNo == oParaData.oTCNTPdtTnfHD.FTXthDocNo).ToList();
                        foreach (var oItemTnfDTSrn in oQryTnfDTSrn) oC_AdaAccDB.TCNTPdtTnfDTSrns.Remove(oItemTnfDTSrn);
                    }
                }

                if (oParaData.aTCNTPdtTnfDTSrn != null && oParaData.aTCNTPdtTnfDTSrn.ToList().Count > 0)
                    foreach (var oItemTnfDTSrn in oParaData.aTCNTPdtTnfDTSrn) oC_AdaAccDB.TCNTPdtTnfDTSrns.Add(oItemTnfDTSrn);
                if (oParaData.aTCNTPdtTnfDT != null && oParaData.aTCNTPdtTnfDT.ToList().Count > 0)
                    foreach (var oItemTnfDT in oParaData.aTCNTPdtTnfDT) oC_AdaAccDB.TCNTPdtTnfDTs.Add(oItemTnfDT);
                if (oParaData.aTCNTPdtTnfHDRef != null && oParaData.aTCNTPdtTnfHDRef.ToList().Count > 0)
                    foreach (var oItemTnfHDRef in oParaData.aTCNTPdtTnfHDRef) oC_AdaAccDB.TCNTPdtTnfHDRefs.Add(oItemTnfHDRef);

                oC_AdaAccDB.TCNTPdtTnfHDs.Add(oParaData.oTCNTPdtTnfHD);
                oC_AdaAccDB.SaveChanges();
                oTrans.Complete();

                oResult.tMsg = "1";
            }
            catch (Exception oEx)
            {
                oResult.tMsg = "900";
            }
            finally
            {
                if (oTrans != null)
                {
                    oTrans.Dispose();
                }
            }
            return oResult;
        }
    }
}
