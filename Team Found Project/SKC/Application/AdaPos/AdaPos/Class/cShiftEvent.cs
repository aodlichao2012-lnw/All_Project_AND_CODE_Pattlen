using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cShiftEvent
    {
        public cShiftEvent()
        {

        }

        /// <summary>
        /// Get Amount / Frequency Deposit
        /// </summary>
        public cmlTPSTShiftEvent C_GEToShiftEvent()
        {
            StringBuilder oSql;
            cmlTPSTShiftEvent oEvn = null;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT SUM(FNSvnQty) AS FNSvnQty, SUM(FCSvnAmt) AS FCSvnAmt");
                oSql.AppendLine("FROM TPSTShiftEvent WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTEvnCode = '" + cVB.tVB_EvnCode + "'");
                oSql.AppendLine("   AND FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("   AND FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("   AND FTPosCode = '" + cVB.tVB_PosCode + "'");    //*Em 62-01-03  เพิ่ม FTPosCode

                oEvn = new cDatabase().C_GEToDataQuery<cmlTPSTShiftEvent>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShiftEvent", "C_GEToShiftEvent : " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return oEvn;
        }

        /// <summary>
        /// Insert Deposit
        /// </summary>
        public void C_INSxShiftEvent(cmlTPSTShiftEvent poEvent)
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO TPSTShiftEvent WITH(ROWLOCK)");
                oSql.AppendLine("(");
                //oSql.AppendLine("   FTBchCode, FTShfCode, FNSdtSeqNo,");
                oSql.AppendLine("   FTBchCode, FTShfCode, FTPosCode, FNSdtSeqNo,"); //*Em 62-01-03  เพิ่ม FTPosCode
                oSql.AppendLine("   FDHisDateTime, FTEvnCode, FNSvnQty,");
                oSql.AppendLine("   FCSvnAmt, FTRsnCode, FTSvnApvCode, ");
                oSql.AppendLine("   FTSvnRemark");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES");
                oSql.AppendLine("(");
                //oSql.AppendLine("   '" + poEvent.FTBchCode + "', '" + poEvent.FTShfCode + "', '" + poEvent.FNSdtSeqNo + "',");
                oSql.AppendLine("   '" + poEvent.FTBchCode + "', '" + poEvent.FTShfCode + "', '" + poEvent.FTPosCode + "', '" + poEvent.FNSdtSeqNo + "',"); //*Em 62-01-03  เพิ่ม FTPosCode
                oSql.AppendLine("   '" + Convert.ToDateTime(poEvent.FDHisDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', '" + poEvent.FTEvnCode + "', '" + poEvent.FNSvnQty + "',");
                oSql.AppendLine("   '" + poEvent.FCSvnAmt + "', '" + poEvent.FTRsnCode + "', '" + poEvent.FTSvnApvCode + "',");
                oSql.AppendLine("   '" + poEvent.FTSvnRemark + "'");
                oSql.AppendLine(")");

                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShiftEvent", "C_INSxDepositWithdraw : " + oEx.Message); }
            finally
            {
                poEvent = null;
                oSql = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get event code
        /// </summary>
        public void C_GETxEventCode(string ptFuncRef)
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTEvnCode");
                oSql.AppendLine("FROM TSysShiftEvent_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTEvnFuncRef = '" + ptFuncRef + "'");
                oSql.AppendLine("AND FTEvnStaUsed = '1'");
                //oSql.AppendLine("AND FNLngID = " + cVB.nVB_Language);     //*Em 62-01-04  Get Code ไม่ต้องใส่เงื่อนไขภาษา

                cVB.tVB_EvnCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShiftEvent", "C_GETxEventCode : " + oEx.Message); }
            finally
            {
                ptFuncRef = null;
                oSql = null;
                new cSP().SP_CLExMemory();

            }
        }

        /// <summary>
        /// Get Shift Balance
        /// </summary>
        public decimal C_GETcShiftBalance()
        {
            StringBuilder oSql;
            decimal cDeposit,cWithdraw,cSale;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ISNULL(SUM(FCSvnAmt),0)");
                oSql.AppendLine("FROM TPSTShiftEvent WITH(NOLOCK)");
                oSql.AppendLine($"WHERE FTShfCode='{cVB.tVB_ShfCode}' AND  FTBchCode='{cVB.tVB_BchCode}' AND FTPosCode='{cVB.tVB_PosCode}' AND FTEvnCode='001'");
                cDeposit= new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(SUM(FCSvnAmt),0)");
                oSql.AppendLine("FROM TPSTShiftEvent WITH(NOLOCK)");
                oSql.AppendLine($"WHERE FTShfCode='{cVB.tVB_ShfCode}' AND  FTBchCode='{cVB.tVB_BchCode}' AND FTPosCode='{cVB.tVB_PosCode}' AND FTEvnCode='002'");
                cWithdraw= new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("WITH Sal AS (SELECT HD.FNXshDocType,RC.FCXrcNet");
                oSql.AppendLine("FROM TPSTSalRC RC WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSTSalHD HD WITH(NOLOCK) ON RC.FTBchCode=HD.FTBchCode AND RC.FTXshDocNo=HD.FTXshDocNo");
                oSql.AppendLine("INNER JOIN TFNMRcv RCV ON RC.FTRcvCode=RCV.FTRcvCode AND RCV.FTFmtCode='001'");
                oSql.AppendLine($"WHERE HD.FTShfCode='{cVB.tVB_ShfCode}' AND  HD.FTBchCode='{cVB.tVB_BchCode}' AND HD.FTPosCode='{cVB.tVB_PosCode}')");
                oSql.AppendLine("");
                oSql.AppendLine("SELECT DISTINCT ((SELECT ISNULL(SUM(SAL.FCXrcNet),0) FCXrcNet FROM SAL WHERE SAL.FNXshDocType=1)-");
                oSql.AppendLine("(SELECT ISNULL(SUM(SAL.FCXrcNet),0) FCXrcNet FROM SAL WHERE SAL.FNXshDocType=9)) AS FCXrcNet");
                oSql.AppendLine("FROM SAL");
                cSale = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                return cDeposit + cSale - cWithdraw;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShiftEvent", "C_GETcShiftBalance : " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();

            }
            return 0m;
        }
    }
}
