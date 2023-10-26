using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cPurchase
    {
        public bool C_PRCbPurchaseInvoice(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
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

                cFunction.C_PRCxMQResponsce("RESPPI", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = Assembly.GetExecutingAssembly().GetName().Name},
                new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESPPI", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxPurchaseInvPrc", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TAPTPiHD with(rowlock)");
                    oSql.AppendLine("SET FTXphStaPrcStk = '1'");
                    oSql.AppendLine(",FTXphStaApv = '1'");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXphDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    if (nRowAffect == 0)
                    {
                        cFunction.C_PRCxMQResponsce("RESPPI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                        return false;
                    }
                    cFunction.C_PRCxMQResponsce("RESPPI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);


                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {

                        //*Arm 62-11-20  [UPLOADSTKBAL / UPLOADSTKCRD]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "PURCHASEINV";
                        oDocApv.ptUser = "MQReceivePrc";
                        oDocApv.ptConnStr = poDocApv.ptConnStr;

                        string tMsgStock = Newtonsoft.Json.JsonConvert.SerializeObject(oDocApv);
                        cFunction.C_PRCxMQPublish("UPLOADSTKCRD", tMsgStock, out tErrMsg);
                        cFunction.C_PRCxMQPublish("UPLOADSTKBAL", tMsgStock, out tErrMsg);

                        //**********************************
                    }
                    return true;
                }
                else
                {
                    cFunction.C_PRCxMQResponsce("RESPPI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbPurchaseInvoice");
                cFunction.C_PRCxMQResponsce("RESPPI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }
        public bool C_PRCbPurchaseCN(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
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

                cFunction.C_PRCxMQResponsce("RESPCN", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = Assembly.GetExecutingAssembly().GetName().Name},
                new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESPCN", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxPurchaseCNPrc", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TAPTPcHD with(rowlock)");
                    oSql.AppendLine("SET FTXphStaPrcStk = '1'");
                    oSql.AppendLine(",FTXphStaApv = '1'");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXphDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    if (nRowAffect == 0)
                    {
                        cFunction.C_PRCxMQResponsce("RESPCN", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                        return false;
                    }
                    cFunction.C_PRCxMQResponsce("RESPCN", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);


                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {
                        //*Arm 62-11-20  [UPLOADSTKBAL / UPLOADSTKCRD]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "PURCHASECN";
                        oDocApv.ptUser = "MQReceivePrc";
                        oDocApv.ptConnStr = poDocApv.ptConnStr;

                        string tMsgStock = Newtonsoft.Json.JsonConvert.SerializeObject(oDocApv);
                        cFunction.C_PRCxMQPublish("UPLOADSTKCRD", tMsgStock, out tErrMsg);
                        cFunction.C_PRCxMQPublish("UPLOADSTKBAL", tMsgStock, out tErrMsg);

                        //**********************************
                    }
                    return true;
                }
                else
                {
                    cFunction.C_PRCxMQResponsce("RESPCN", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbPurchaseCN");
                cFunction.C_PRCxMQResponsce("RESPCN", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }
    }
}
