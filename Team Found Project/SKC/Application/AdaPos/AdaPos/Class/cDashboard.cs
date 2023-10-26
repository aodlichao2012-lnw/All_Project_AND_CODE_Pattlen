using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cDashboard
    {
        /// <summary>
        /// Get cashier shift summary.
        /// </summary>
        /// 
        /// <param name="ptBchCode">Branch code.</param>
        /// <param name="ptShfCode">Shift code.</param>
        /// <param name="ptPosCode">Pos code.</param>
        /// <param name="pnLng">Language.</param>
        /// 
        /// <returns>Cashier sale summary.</returns>
        public List<cmlShiftSummary> C_GETaCashierShiftSummary(string ptBchCode, string ptShfCode, string ptPosCode, int pnLng)
        {
            List<cmlShiftSummary> aoSaleSummary;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT EVN.FTEvnFuncRef, SUM(SVN.FNSvnQty) AS FNSvnQty, SUM(SVN.FCSvnAmt) AS FCSvnAmt");
                oSql.AppendLine("FROM TSysShiftEvent_L EVN WITH(NOLOCK)");
                oSql.AppendLine("	LEFT JOIN TPSTShiftEvent SVN  WITH(NOLOCK) ON EVN.FTEvnCode = SVN.FTEvnCode");
                oSql.AppendLine("	AND EVN.FNLngID = " + pnLng);
                oSql.AppendLine("WHERE EVN.FTEvnStaUsed = '1'");
                oSql.AppendLine("AND SVN.FTBchCode = '" + ptBchCode + "'");
                oSql.AppendLine("AND SVN.FTShfCode = '" + ptShfCode + "'");
                oSql.AppendLine("AND SVN.FTPosCode = '" + ptPosCode + "'");
                oSql.AppendLine("GROUP BY EVN.FTEvnFuncRef");
                oSql.AppendLine("");

                oSql.AppendLine("UNION ALL");
                oSql.AppendLine("");

                oSql.AppendLine("SELECT 'TICKET' AS FTEvnFuncRef, 0 AS FNSvnQty, 0 AS FCSvnAmt");
                oSql.AppendLine("");

                oSql.AppendLine("UNION ALL");
                oSql.AppendLine("");

                oSql.AppendLine("SELECT");
                oSql.AppendLine("	CASE");
                oSql.AppendLine("		WHEN FTCthDocType = '1' THEN 'TOPUP'");
                oSql.AppendLine("		WHEN FTCthDocType = '5' THEN 'CANCEL TOPUP'");
                oSql.AppendLine("	END AS FTEvnFuncRef,");
                oSql.AppendLine("	CASE");
                oSql.AppendLine("		WHEN FTCthDocType = '1' THEN COUNT(FTCthDocNo)");
                oSql.AppendLine("		WHEN FTCthDocType = '5' THEN COUNT(FTCthDocNo)");
                oSql.AppendLine("		ELSE 0");
                oSql.AppendLine("	END AS FNSvnQty,");
                oSql.AppendLine("	CASE");
                oSql.AppendLine("		WHEN FTCthDocType = '1' THEN SUM(FCCthTotalTP)");
                oSql.AppendLine("		WHEN FTCthDocType = '5' THEN SUM(FCCthTotalTP)");
                oSql.AppendLine("		ELSE 0");
                oSql.AppendLine("	END ASFCSvnAmt");
                oSql.AppendLine("FROM TFNTCrdTopUpHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTCthDocType IN ('1','5')");
                oSql.AppendLine("AND FTCthStaDoc = '1'");
                oSql.AppendLine("AND FTCthStaPrcDoc = '1'");
                oSql.AppendLine("AND FTBchCode = '" + ptBchCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + ptShfCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + ptPosCode + "'");
                oSql.AppendLine("GROUP BY FTCthDocType");

                aoSaleSummary = new cDatabase().C_GETaDataQuery<cmlShiftSummary>(oSql.ToString());

                return aoSaleSummary;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);

                return null;
            }
            finally
            {
                oSql = null;
                aoSaleSummary = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get cashire payment type.
        /// </summary>
        /// 
        /// <param name="ptBchCode">Branch code.</param>
        /// <param name="ptShfCode">Shift code.</param>
        /// <param name="ptPosCode">Pos code.</param>
        /// <param name="pnLng">Language.</param>
        /// 
        /// <returns>Cashier payment type.</returns>
        public List<cmlPaymentType> C_GETaCashierPaymentType(string ptBchCode, string ptShfCode, string ptPosCode, int pnLng)
        {
            List<cmlPaymentType> aoPaymentType;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RCV.FTRcvCode, RCVL.FTRcvName, ISNULL(TOPUP.FCXrcNet, 0) AS FCXrcNet");
                oSql.AppendLine("FROM TFNMRcv RCV WITH(NOLOCK)");
                oSql.AppendLine("	LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode");
                oSql.AppendLine("	AND RCVL.FNLngID = " + pnLng);
                oSql.AppendLine("	INNER JOIN");
                oSql.AppendLine("	(");
                oSql.AppendLine("		SELECT XRC.FTRcvCode,");
                oSql.AppendLine("			SUM(CASE");
                oSql.AppendLine("				WHEN CTH.FTCthDocType = '1' THEN XRC.FCXrcNet");
                oSql.AppendLine("				WHEN CTH.FTCthDocType = '5' THEN XRC.FCXrcNet *-1");
                oSql.AppendLine("				ELSE 0");
                oSql.AppendLine("			END) AS FCXrcNet");
                oSql.AppendLine("		FROM TFNTCrdTopUpRC XRC WITH(NOLOCK) ");
                oSql.AppendLine("			INNER JOIN TFNTCrdTopUpHD CTH WITH(NOLOCK) ON XRC.FTXrcRetDocRef = CTH.FTCthDocNo ");
                oSql.AppendLine("		WHERE CTH.FTCthDocType IN ('1','5')");
                oSql.AppendLine("		AND CTH.FTCthStaDoc = '1'");
                oSql.AppendLine("		AND CTH.FTCthStaPrcDoc = '1'");
                oSql.AppendLine("		AND CTH.FTBchCode = '" + ptBchCode + "'");
                oSql.AppendLine("		AND CTH.FTShfCode = '" + ptShfCode + "'");
                oSql.AppendLine("		AND CTH.FTPosCode = '" + ptPosCode + "'");
                oSql.AppendLine("		GROUP BY XRC.FTRcvCode");
                oSql.AppendLine("	) TOPUP ON RCV.FTRcvCode = TOPUP.FTRcvCode");
                oSql.AppendLine("WHERE RCV.FTRcvStaUse = '1'");

                aoPaymentType = new cDatabase().C_GETaDataQuery<cmlPaymentType>(oSql.ToString());

                return aoPaymentType;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);

                return null;
            }
            finally
            {
                oSql = null;
                aoPaymentType = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get store shift summary.
        /// </summary>
        /// 
        /// <param name="ptBchCode">Branch code.</param>
        /// <param name="ptShfCode">Shift code.</param>
        /// <param name="ptPosCode">Pos code.</param>
        /// <param name="pnLng">Language.</param>
        /// 
        /// <returns>Store sale summary.</returns>
        public List<cmlShiftSummary> C_GETaStoreShiftSummary(string ptBchCode, string ptShfCode, string ptPosCode, int pnLng)
        {
            List<cmlShiftSummary> aoSaleSummary;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT EVN.FTEvnFuncRef, SUM(SVN.FNSvnQty) AS FNSvnQty, SUM(SVN.FCSvnAmt) AS FCSvnAmt");
                oSql.AppendLine("FROM TSysShiftEvent_L EVN WITH(NOLOCK)");
                oSql.AppendLine("	LEFT JOIN TPSTShiftEvent SVN  WITH(NOLOCK) ON EVN.FTEvnCode = SVN.FTEvnCode");
                oSql.AppendLine("	AND EVN.FNLngID = " + pnLng);
                oSql.AppendLine("WHERE EVN.FTEvnStaUsed = '1'");
                oSql.AppendLine("AND SVN.FTBchCode = '" + ptBchCode + "'");
                oSql.AppendLine("AND SVN.FTShfCode = '" + ptShfCode + "'");
                oSql.AppendLine("AND SVN.FTPosCode = '" + ptPosCode + "'");
                oSql.AppendLine("GROUP BY EVN.FTEvnFuncRef");
                oSql.AppendLine("");

                oSql.AppendLine("UNION ALL");
                oSql.AppendLine("");

                oSql.AppendLine("SELECT");
                oSql.AppendLine("	CASE");
                oSql.AppendLine("		WHEN FNXshDocType = '1' THEN 'SALE'");
                oSql.AppendLine("		WHEN FNXshDocType = '2' THEN 'RETURN_SALE'");
                oSql.AppendLine("		WHEN FNXshDocType = '3' THEN 'RETAIN'");
                oSql.AppendLine("		WHEN FNXshDocType = '4' THEN 'RETURN_RETAIN'");
                oSql.AppendLine("	END AS FTEvnFuncRef,");
                oSql.AppendLine("	CASE");
                oSql.AppendLine("		WHEN FNXshDocType = '1' THEN COUNT(FTXshDocNo)");
                oSql.AppendLine("		WHEN FNXshDocType = '2' THEN COUNT(FTXshDocNo)");
                oSql.AppendLine("		WHEN FNXshDocType = '3' THEN COUNT(FTXshDocNo)");
                oSql.AppendLine("		WHEN FNXshDocType = '4' THEN COUNT(FTXshDocNo)");
                oSql.AppendLine("		ELSE 0");
                oSql.AppendLine("	END AS FNSvnQty,");
                oSql.AppendLine("	CASE");
                oSql.AppendLine("		WHEN FNXshDocType = '1' THEN SUM(ISNULL(FCXshGrand,0) - ISNULL(FCXshRnd,0))");
                oSql.AppendLine("		WHEN FNXshDocType = '2' THEN SUM(ISNULL(FCXshGrand,0) - ISNULL(FCXshRnd,0))");
                oSql.AppendLine("		WHEN FNXshDocType = '3' THEN SUM(ISNULL(FCXshGrand,0) - ISNULL(FCXshRnd,0))");
                oSql.AppendLine("		WHEN FNXshDocType = '4' THEN SUM(ISNULL(FCXshGrand,0) - ISNULL(FCXshRnd,0))");
                oSql.AppendLine("		ELSE 0");
                oSql.AppendLine("	END ASFCSvnAmt");
                oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FNXshDocType IN ('1','2','3','4')");
                oSql.AppendLine("AND FTXshStaDoc = '1'");
                oSql.AppendLine("AND FTBchCode = '" + ptBchCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + ptShfCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + ptPosCode + "'");
                oSql.AppendLine("GROUP BY FNXshDocType");

                aoSaleSummary = new cDatabase().C_GETaDataQuery<cmlShiftSummary>(oSql.ToString());

                return aoSaleSummary;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);

                return null;
            }
            finally
            {
                oSql = null;
                aoSaleSummary = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get store payment type.
        /// </summary>
        /// 
        /// <param name="ptBchCode">Branch code.</param>
        /// <param name="ptShfCode">Shift code.</param>
        /// <param name="ptPosCode">Pos code.</param>
        /// <param name="pnLng">Language.</param>
        /// 
        /// <returns>Store payment type.</returns>
        public List<cmlPaymentType> C_GETaStorePaymentType(string ptBchCode, string ptShfCode, string ptPosCode, int pnLng)
        {
            List<cmlPaymentType> aoPaymentType;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RCV.FTRcvCode, RCVL.FTRcvName, ISNULL(SAL.FCXrcNet, 0) AS FCXrcNet");
                oSql.AppendLine("FROM TFNMRcv RCV WITH(NOLOCK)");
                oSql.AppendLine("	LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode");
                oSql.AppendLine("	AND RCVL.FNLngID = " + pnLng);
                oSql.AppendLine("	INNER JOIN");
                oSql.AppendLine("	(");
                oSql.AppendLine("		SELECT SRC.FTRcvCode,");
                oSql.AppendLine("			SUM(CASE");
                oSql.AppendLine("				WHEN SHD.FNXshDocType IN('1','3') THEN SRC.FCXrcNet");
                oSql.AppendLine("				WHEN SHD.FNXshDocType IN('2','4') THEN SRC.FCXrcNet *-1");
                oSql.AppendLine("				ELSE 0");
                oSql.AppendLine("			END) AS FCXrcNet");
                oSql.AppendLine("		FROM TPSTSalRC SRC WITH(NOLOCK)");
                oSql.AppendLine("			INNER JOIN TPSTSalHD SHD WITH(NOLOCK) ON SRC.FTXshDocNo = SHD.FTXshDocNo");
                oSql.AppendLine("		WHERE SHD.FNXshDocType IN ('1','2','3','4')");
                oSql.AppendLine("		AND SHD.FTXshStaDoc = '1'");
                oSql.AppendLine("		AND SHD.FTBchCode = '" + ptBchCode + "'");
                oSql.AppendLine("		AND SHD.FTShfCode = '" + ptShfCode + "'");
                oSql.AppendLine("		AND SHD.FTPosCode = '" + ptPosCode + "'");
                oSql.AppendLine("		GROUP BY SRC.FTRcvCode");
                oSql.AppendLine("	) SAL ON RCV.FTRcvCode = SAL.FTRcvCode");
                oSql.AppendLine("WHERE RCV.FTRcvStaUse = '1'");

                aoPaymentType = new cDatabase().C_GETaDataQuery<cmlPaymentType>(oSql.ToString());

                return aoPaymentType;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);

                return null;
            }
            finally
            {
                oSql = null;
                aoPaymentType = null;
                new cSP().SP_CLExMemory();
            }
        }

    }
}
