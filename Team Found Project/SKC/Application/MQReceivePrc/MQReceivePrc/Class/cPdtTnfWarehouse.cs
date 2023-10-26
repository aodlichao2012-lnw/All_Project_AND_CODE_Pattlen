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
    public class cPdtTnfWarehouse
    {
        public bool C_PRCbTnfWahOut(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
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

                cFunction.C_PRCxMQResponsce("RESTWO", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                    new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                    new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value =poDocApv.ptUser},
                    new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESTWO", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);

                //if (oDB.C_SQLbExecutStoreProcedure("STP_DOCxWahPdtTnfOut", oPara, "@FNResult"))
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxWahPdtTnfOut", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNTPdtTwoHD with(rowlock)");
                    oSql.AppendLine("SET FTXthStaApv = '1'");
                    oSql.AppendLine(",FTXthStaPrcStk = '1' ");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '"+ poDocApv.ptUser +"' ");
                    oSql.AppendLine("WHERE FTBchCode = '"+ poDocApv.ptBchCode +"'");
                    oSql.AppendLine("AND FTXthDocNo = '"+ poDocApv.ptDocNo +"'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                    //if (nRowAffect == 0)
                    //{
                    //    cFunction.C_PRCxMQResponsce("RESTWO", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    //    return false;
                    //}
                    cFunction.C_PRCxMQResponsce("RESTWO", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);


                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {

                        //*Arm 62-11-20  [UPLOADSTKBAL]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "TNFWAREHOSEOUT";
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
                    cFunction.C_PRCxMQResponsce("RESTWO", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbTnfWahOut");
                cFunction.C_PRCxMQResponsce("RESTWO", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }

        public bool C_PRCbTnfWahIn(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
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
                cFunction.C_PRCxMQResponsce("RESTWI", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                    new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                    new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = poDocApv.ptUser},
                    new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESTWI", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                //if (oDB.C_SQLbExecutStoreProcedure("STP_DOCxWahPdtTnfIn", oPara, "@FNResult"))
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxWahPdtTnfIn", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNTPdtTwiHD with(rowlock)");
                    oSql.AppendLine("SET FTXthStaApv = '1'");
                    oSql.AppendLine(",FTXthStaPrcStk = '1' ");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXthDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                    //if (nRowAffect == 0)
                    //{
                    //    cFunction.C_PRCxMQResponsce("RESTWI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    //    return false;
                    //}
                    cFunction.C_PRCxMQResponsce("RESTWI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);

                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {

                        //*Arm 62-11-20  [UPLOADSTKBAL]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "TNFWAREHOSEIN";
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
                    cFunction.C_PRCxMQResponsce("RESTWI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbTnfWahIn");
                cFunction.C_PRCxMQResponsce("RESTWI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }

        public bool C_PRCbTnfWah(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
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
                cFunction.C_PRCxMQResponsce("RESTFW", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                    new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                    new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = poDocApv.ptUser},
                    new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESTFW", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                //if (oDB.C_SQLbExecutStoreProcedure("STP_DOCxPricePrc", oPara, "@FNResult"))
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxWahPdtTnf", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNTPdtTwxHD with(rowlock)");
                    oSql.AppendLine("SET FTXthStaApv = '1'");
                    oSql.AppendLine(",FTXthStaPrcStk = '1' ");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXthDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    //if (nRowAffect == 0)
                    //{
                    //    cFunction.C_PRCxMQResponsce("RESTFW", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    //    return false;
                    //}

                    cFunction.C_PRCxMQResponsce("RESTFW", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);

                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {

                        //*Arm 62-11-20  [UPLOADSTKBAL]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "TNFWAREHOSE";
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
                    cFunction.C_PRCxMQResponsce("RESTFW", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbTnfWah");
                cFunction.C_PRCxMQResponsce("RESTFW", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }

        public bool C_PRCbTnfWahVD(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
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
                cFunction.C_PRCxMQResponsce("RESTFWVD", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                    new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                    new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = poDocApv.ptUser},
                    new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESTFWVD", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                //if (oDB.C_SQLbExecutStoreProcedure("STP_DOCxPricePrc", oPara, "@FNResult"))
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxWahPdtTnfVD", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TVDTPdtTwxHD with(rowlock)");
                    oSql.AppendLine("SET FTXthStaApv = '1'");
                    oSql.AppendLine(",FTXthStaPrcStk = '1' ");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXthDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    //if (nRowAffect == 0)
                    //{
                    //    cFunction.C_PRCxMQResponsce("RESTFWVD", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    //    return false;
                    //}
                    cFunction.C_PRCxMQResponsce("RESTFWVD", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);

                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {

                        //*Arm 62-11-20  [UPLOADSTKBAL / UPLOADSTKCRD]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "TNFWAREHOSEVD";
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
                    cFunction.C_PRCxMQResponsce("RESTFWVD", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbTnfWahVD");
                cFunction.C_PRCxMQResponsce("RESTFWVD", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }
    }
}
