using API2PSSale.Class;
using API2PSSale.Class.Standard;
using API2PSSale.Models.WebService.Request.GenDocNo;
using API2PSSale.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace API2PSSale.Controllers
{
    /// <summary>
    ///     Gen Document No.
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/GenDocNo")]
    public class cGenDocNoController : ApiController
    {
        /// <summary>
        /// Gen. เลขที่เอกสารใบกำกับภาษี
        /// </summary>
        /// <param name="poParam"></param>
        /// <returns>
        ///&#8195;     001 : Success.<br/>
        ///&#8195;     701 : Validate parameter model false.<br/>
        ///&#8195;     900 : Service process false.<br/>
        ///&#8195;     904 : Key not allowed to use method.<br/>
        ///&#8195;     905 : Cannot connect database.<br/>
        /// </returns>
        [Route("TaxDocNo")]
        [HttpPost]
        public cmlResItem<string> POS_GENtDocNo(cmlReqGenDocNo poParam)
        {
            cmlResItem<string> oResult;
            cDatabase oDB;
            StringBuilder oSql;
            DataTable odtTmp;
            cMS oMsg;
            cSP oSP;
            string tErrAPI, tModelErr;
            
            int nMax = 0;
            string tDocNo = "";
            string tDocFmt = "";
            string tDocFmtChr = "";
            string tDocFmtBch = "";
            string tDocFmtPosShp = "";
            string tDocFmtYear = "";
            string tDocFmtMonth = "";
            string tDocFmtDay = "";
            string tDocFmtSep = "";
            string tDocFmtNum = "";
            string tDocFmtLeft = "";
            int nDocRuningLength = 0;
            int nResult = 0;    //*Em 63-06-07
            int nRetry = 0;    //*Em 63-06-07
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<string>();
                oDB = new cDatabase();
                oMsg = new cMS();
                oSP = new cSP();

                // Validate parameter.
                tModelErr = "";
                if (oSP.SP_CHKbParaModel(ref tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
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
                
                string tDocLeft = "";
                int nDocType = (poParam.pnSaleType == 9 ? 5 : 4);
                oSql = new StringBuilder();
                
                // # GEN DocNo
                // ================================================================
                tDocFmtYear = string.Format("{0:yy}", DateTime.Now.Date);
                if (nDocType == 5)
                {
                    tDocFmtChr = "R";
                }
                else
                {
                    tDocFmtChr = "S";
                }
                tDocFmtLeft = tDocFmtChr + tDocFmtYear + poParam.ptBchCode ;   //*Arm 63-02-17 - ปรับ Fomat DocNo
                nDocRuningLength = 7;
                string tFmt = new string('0', nDocRuningLength);
                tDocLeft = tDocFmtLeft;
 ReGen:
                oSql.Clear();
                oSql.AppendLine("SELECT FTXshDocNo ");
                oSql.AppendLine("FROM TPSTTaxNo"); //*Arm 63-06-07
                oSql.AppendLine("WHERE FTBchCode = '" + poParam.ptBchCode + "' AND FNXshDocType = "+ nDocType + " ");
                
                odtTmp = new DataTable();
                odtTmp = oDB.C_GEToQuerySQLTbl(oSql.ToString());
                //*Arm 63-06-07
                if(odtTmp == null || odtTmp.Rows.Count == 0)
                {
                    tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", 1);
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TPSTTaxNo (FTBchCode,FTXshDocNo,FNXshDocType) VALUES ('" + poParam.ptBchCode + "', '" + tDocNo + "', " + nDocType + " )");
                    nResult = oDB.C_GETnQuerySQL(oSql.ToString());
                    if (nResult < 1 && nRetry < 5)
                    {
                        nRetry++;
                        goto ReGen;
                    }
                }
                else
                {
                    string tDoc = odtTmp.Rows[0].Field<string>("FTXshDocNo");
                    if(string.IsNullOrEmpty(tDoc))
                    {
                        tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", 1);
                    }
                    else
                    {
                        tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", Convert.ToInt32(tDoc.Substring(8,7)) + 1);
                    }
                    
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TPSTTaxNo with(rowlock) SET FTXshDocNo = '" + tDocNo + "' WHERE FTBchCode = '" + poParam.ptBchCode + "' AND FNXshDocType = " + nDocType +" ");
                    oSql.AppendLine("AND FTXshDocNo = '"+ tDoc + "'");
                    nResult = oDB.C_GETnQuerySQL(oSql.ToString());
                    if (nResult < 1 && nRetry < 5)
                    {
                        nRetry++;
                        goto ReGen;
                    } 
                }

                if (nRetry < 3)
                {
                    oResult.roItem = tDocNo;
                    oResult.rtCode = oMsg.tMS_RespCode001;
                    oResult.rtDesc = oMsg.tMS_RespDesc001;
                    return oResult;
                }
                else
                {
                    oResult.roItem = "";
                    oResult.rtCode = oMsg.tMS_RespCode803;
                    oResult.rtDesc = oMsg.tMS_RespDesc803;
                    return oResult;
                }
                

            }
            catch (Exception)
            {
                oResult = new cmlResItem<string>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oDB = null;
                oMsg = null;
                oSP = null;
                oResult = null;
                oSql = null;
                odtTmp = null;
            }
        }
    }
}