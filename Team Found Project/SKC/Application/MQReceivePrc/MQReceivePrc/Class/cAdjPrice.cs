using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Class;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Class.Standard;

namespace MQReceivePrc.Class
{
    public class cAdjPrice
    {
        public bool C_PRCbPdtPrice(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            DataTable odtTmp = new DataTable();
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            try
            {
                ptErrMsg = "";

                //1.ตรวจสอบข้อมูล
                if (poDocApv == null) return false;

                cFunction.C_PRCxMQResponsce("RESAJP", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = Assembly.GetExecutingAssembly().GetName().Name},
                new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESAJP", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr,"STP_DOCxPricePrc",ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNTPdtAdjPriHD with(rowlock)");
                    oSql.AppendLine("SET FTXphStaPrcDoc = '1'");
                    oSql.AppendLine(",FTXphStaApv = '1'");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXphDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr,oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    if (nRowAffect == 0)
                    {
                        cFunction.C_PRCxMQResponsce("RESAJP", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                        return false;
                    }
                    cFunction.C_PRCxMQResponsce("RESAJP", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    C_PRCxClearPdtPrice4Pdt(poDocApv, poShopDB);    //*Em 63-06-22 //*Net 63-08-03 ยกมาจาก Moshi
                    return true;
                }
                else
                {
                    cFunction.C_PRCxMQResponsce("RESAJP", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbPdtPrice");
                cFunction.C_PRCxMQResponsce("RESAJP", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }

        public bool C_PRCbPdtPriceRT(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            DataTable odtTmp = new DataTable();
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            try
            {
                ptErrMsg = "";

                //1.ตรวจสอบข้อมูล
                if (poDocApv == null) return false;

                cFunction.C_PRCxMQResponsce("RESAJPRT", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = Assembly.GetExecutingAssembly().GetName().Name},
                new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESAJPRT", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxPriceRTPrc", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNTPdtAdjPriHD with(rowlock)");
                    oSql.AppendLine("SET FTXphStaPrcDoc = '1'");
                    oSql.AppendLine(",FTXphStaApv = '1'");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXphDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    if (nRowAffect == 0)
                    {
                        cFunction.C_PRCxMQResponsce("RESAJPRT", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                        return false;
                    }
                    cFunction.C_PRCxMQResponsce("RESAJPRT", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return true;
                }
                else
                {
                    cFunction.C_PRCxMQResponsce("RESAJPRT", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbPdtPriceRT");
                cFunction.C_PRCxMQResponsce("RESAJPRT", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }

        //*Net 63-08-03 ยกมาจาก Moshi
        private void C_PRCxClearPdtPrice4Pdt(cmlRcvDocApv poDocApv, cmlShopDB poShopDB)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            int nRowAffect = 0;
            try
            {
                oSql.AppendLine("DELETE PDT ");
                oSql.AppendLine("FROM TCNTPdtPrice4PDT PDT");
                oSql.AppendLine("WHERE NOT EXISTS");
                oSql.AppendLine("   (SELECT FTPdtCode FROM");
                oSql.AppendLine("	    (SELECT DISTINCT FTPdtCode,FTPunCode,ISNULL(FTPplCode,'') FTPplCode,FTPghDocType,FTPghStaAdj,MAX(FDPghDStart + ' ' + FTPghTStart) AS FDPghDStart ");
                oSql.AppendLine("	    FROM TCNTPdtPrice4PDT WITH(NOLOCK)");
                oSql.AppendLine("       WHERE (FTPghTStart LIKE '00:%' AND FTPghTStop LIKE '23:%')");   //*Em 63-06-24
                oSql.AppendLine("       AND CONVERT(VARCHAR(10),FDPghDStart,121) <= CONVERT(VARCHAR(10),GETDATE(),121)");       //*Em 63-09-01
                oSql.AppendLine("	    GROUP BY FTPdtCode,FTPunCode,ISNULL(FTPplCode,''),FTPghDocType,FTPghStaAdj) TMP");
                oSql.AppendLine("	WHERE FTPdtCode = PDT.FTPdtCode AND FTPunCode = PDT.FTPunCode AND FTPplCode = ISNULL(FTPplCode,'')");
                oSql.AppendLine("	AND FTPghDocType = PDT.FTPghDocType AND FTPghStaAdj = PDT.FTPghStaAdj ");
                oSql.AppendLine("	AND CONVERT(VARCHAR(19),FDPghDStart,121) = CONVERT(VARCHAR(19),(PDT.FDPghDStart + ' ' + PDT.FTPghTStart),121))");
                oSql.AppendLine("AND (FTPghTStart LIKE '00:%' AND FTPghTStop LIKE '23:%')");       //*Em 63-06-24
                oSql.AppendLine("AND CONVERT(VARCHAR(10),FDPghDStart,121) <= CONVERT(VARCHAR(10),GETDATE(),121)");       //*Em 63-09-01
                oSql.AppendLine("");
                oSql.AppendLine("DELETE FROM TCNTPdtPrice4PDT"); //*Em 63-06-24
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDPghDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)"); //*Em 63-06-24
                oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPdt", "C_PRCxClearPdtPrice4Pdt : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
        }
    }
}
