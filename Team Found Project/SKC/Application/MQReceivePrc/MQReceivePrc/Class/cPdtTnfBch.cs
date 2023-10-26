using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Bch2Bch;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cPdtTnfBch
    {
        public bool C_PRCbTnfBch(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
        {
            DataTable odtTmp = new DataTable();
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            string tBchTo = "";
            string tMsgMQ = "";
            cmlRabbitMQ oMQ = new cmlRabbitMQ();
            ConnectionFactory oFactory;

            try
            {
                ptErrMsg = "";

                //1.ตรวจสอบข้อมูล
                if (poDocApv == null) return false;
                cFunction.C_PRCxMQResponsce("RESTBX", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                    new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                    new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = poDocApv.ptUser},
                    new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESTBX", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxBchPdtTnf", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNTPdtTbxHD with(rowlock)");
                    oSql.AppendLine("SET FTXthStaApv = '1'");
                    oSql.AppendLine(",FTXthStaPrcStk = '1' ");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXthDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    //if (nRowAffect == 0)
                    //{
                    //    cFunction.C_PRCxMQResponsce("RESTBX", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    //    return false;
                    //}

                    //*Em 62-10-16
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 FTXthBchTo FROM TCNTPdtTbxHD WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '"+ poDocApv.ptBchCode +"' AND FTXthDocNo = '"+ poDocApv.ptDocNo +"'");
                    tBchTo = oDB.C_DAToExecuteQuery<string>(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);
                    if (!string.IsNullOrEmpty(tBchTo) && string.IsNullOrEmpty(tBchTo) != string.IsNullOrEmpty(cVB.tVB_BchCode))     //*Em 63-03-26  Moshi
                    {
                        oMQ = new cSP().SP_GEToMQBranch(poDocApv.ptBchCode, "MQDocument", poDocApv.ptConnStr, (int)poShopDB.nCommandTimeOut);
                        if (oMQ != null && oMQ.tMQHostName != null) //*Em 63-03-26  Moshi
                        {
                            oFactory = new ConnectionFactory();
                            oFactory.HostName = oMQ.tMQHostName;
                            oFactory.UserName = oMQ.tMQUserName;
                            oFactory.Password = oMQ.tMQPassword;
                            oFactory.VirtualHost = oMQ.tMQVirtualHost;
                            using (var oConn = oFactory.CreateConnection())
                            {
                                using (var oChannel = oConn.CreateModel())
                                {
                                    cmlCallSendBch oCallSendBch = new cmlCallSendBch();
                                    oCallSendBch.ptBchFrm = poDocApv.ptBchCode;
                                    oCallSendBch.ptBchTo = tBchTo;
                                    oCallSendBch.ptConnStr = poDocApv.ptConnStr;
                                    oCallSendBch.ptDocNo = poDocApv.ptDocNo;
                                    oCallSendBch.ptDocType = "1";
                                    tMsgMQ = JsonConvert.SerializeObject(oCallSendBch);
                                    var body = Encoding.UTF8.GetBytes(tMsgMQ);
                                    oChannel.QueueDeclare("CALLSENDBCH", false, false, false, null);
                                    oChannel.BasicPublish("", "CALLSENDBCH", false, null, body);
                                    ptErrMsg = "";
                                }
                            }
                        }
                    }
                    //++++++++++++++++++

                    cFunction.C_PRCxMQResponsce("RESTBX", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);

                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {

                        //*Arm 62-11-20  [UPLOADSTKBAL / UPLOADSTKCRD]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "TNFBRANCH";
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
                    cFunction.C_PRCxMQResponsce("RESTBX", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbTnfBch");
                cFunction.C_PRCxMQResponsce("RESTBX", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }

        public bool C_PRCbTnfBchOut(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
        {
            DataTable odtTmp = new DataTable();
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            string tBchTo = "";
            string tMsgMQ = "";
            cmlRabbitMQ oMQ = new cmlRabbitMQ();
            ConnectionFactory oFactory;

            try
            {
                ptErrMsg = "";

                //1.ตรวจสอบข้อมูล
                if (poDocApv == null) return false;
                cFunction.C_PRCxMQResponsce("RESTBO", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                    new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                    new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = poDocApv.ptUser},
                    new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESTBO", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxBchPdtTnfOut", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNTPdtTboHD with(rowlock)");
                    oSql.AppendLine("SET FTXthStaApv = '1'");
                    oSql.AppendLine(",FTXthStaPrcStk = '1' ");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXthDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    //if (nRowAffect == 0)
                    //{
                    //    cFunction.C_PRCxMQResponsce("RESTBO", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    //    return false;
                    //}

                    //*Em 62-10-16
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 FTXthBchTo FROM TCNTPdtTboHD WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "' AND FTXthDocNo = '" + poDocApv.ptDocNo + "'");
                    tBchTo = oDB.C_DAToExecuteQuery<string>(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);
                    if (!string.IsNullOrEmpty(tBchTo) && string.IsNullOrEmpty(tBchTo) != string.IsNullOrEmpty(cVB.tVB_BchCode))     //*Em 63-03-26  Moshi
                    {
                        oMQ = new cSP().SP_GEToMQBranch(poDocApv.ptBchCode, "MQDocument", poDocApv.ptConnStr, (int)poShopDB.nCommandTimeOut);
                        if (oMQ != null && oMQ.tMQHostName != null) //*Em 63-03-26  Moshi
                        {
                            oFactory = new ConnectionFactory();
                            oFactory.HostName = oMQ.tMQHostName;
                            oFactory.UserName = oMQ.tMQUserName;
                            oFactory.Password = oMQ.tMQPassword;
                            oFactory.VirtualHost = oMQ.tMQVirtualHost;
                            using (var oConn = oFactory.CreateConnection())
                            {
                                using (var oChannel = oConn.CreateModel())
                                {
                                    cmlCallSendBch oCallSendBch = new cmlCallSendBch();
                                    oCallSendBch.ptBchFrm = poDocApv.ptBchCode;
                                    oCallSendBch.ptBchTo = tBchTo;
                                    oCallSendBch.ptConnStr = poDocApv.ptConnStr;
                                    oCallSendBch.ptDocNo = poDocApv.ptDocNo;
                                    oCallSendBch.ptDocType = "1";
                                    tMsgMQ = JsonConvert.SerializeObject(oCallSendBch);
                                    var body = Encoding.UTF8.GetBytes(tMsgMQ);
                                    oChannel.QueueDeclare("CALLSENDBCH", false, false, false, null);
                                    oChannel.BasicPublish("", "CALLSENDBCH", false, null, body);
                                    ptErrMsg = "";
                                }
                            }
                        }
                    }
                    //++++++++++++++++++

                    cFunction.C_PRCxMQResponsce("RESTBO", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);

                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {

                        //*Arm 62-11-20  [UPLOADSTKBAL / UPLOADSTKCRD]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "TNFBRANCHOUT";
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
                    cFunction.C_PRCxMQResponsce("RESTBO", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbTnfBchOut");
                cFunction.C_PRCxMQResponsce("RESTBO", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }

        public bool C_PRCbTnfBchIn(cmlRcvDocApv poDocApv, cmlShopDB poShopDB, out string ptErrMsg)
        {
            DataTable odtTmp = new DataTable();
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            string tBchTo = "";
            string tMsgMQ = "";
            cmlRabbitMQ oMQ = new cmlRabbitMQ();
            ConnectionFactory oFactory;

            try
            {
                ptErrMsg = "";

                //1.ตรวจสอบข้อมูล
                if (poDocApv == null) return false;
                cFunction.C_PRCxMQResponsce("RESTBI", poDocApv.ptDocNo, poDocApv.ptUser, "10", out ptErrMsg);
                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poDocApv.ptBchCode},
                    new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poDocApv.ptDocNo},
                    new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = poDocApv.ptUser},
                    new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                cFunction.C_PRCxMQResponsce("RESTBI", poDocApv.ptDocNo, poDocApv.ptUser, "50", out ptErrMsg);
                if (oDB.C_DATbExecuteStoreProcedure(poDocApv.ptConnStr, "STP_DOCxBchPdtTnfIn", ref oPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNTPdtTbiHD with(rowlock)");
                    oSql.AppendLine("SET FTXthStaApv = '1'");
                    oSql.AppendLine(",FTXthStaPrcStk = '1' ");
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + poDocApv.ptUser + "' ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poDocApv.ptBchCode + "'");
                    oSql.AppendLine("AND FTXthDocNo = '" + poDocApv.ptDocNo + "'");
                    oDB.C_DATbExecuteNonQuery(poDocApv.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                    //if (nRowAffect == 0)
                    //{
                    //    cFunction.C_PRCxMQResponsce("RESTBI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    //    return false;
                    //}

                    cFunction.C_PRCxMQResponsce("RESTBI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);


                    //*Arm 63-03-31 Check StaUseCentralized
                    if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                    {

                        //*Arm 62-11-20  [UPLOADSTKBAL / UPLOADSTKCRD]
                        //*********************************
                        string tErrMsg = "";

                        cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                        oDocApv.ptBchCode = poDocApv.ptBchCode;
                        oDocApv.ptDocNo = poDocApv.ptDocNo;
                        oDocApv.ptDocType = "TNFBRANCHIN";
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
                    cFunction.C_PRCxMQResponsce("RESTBI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbTnfBchIn");
                cFunction.C_PRCxMQResponsce("RESTBI", poDocApv.ptDocNo, poDocApv.ptUser, "100", out ptErrMsg);
                return false;
            }
        }
    }
}
