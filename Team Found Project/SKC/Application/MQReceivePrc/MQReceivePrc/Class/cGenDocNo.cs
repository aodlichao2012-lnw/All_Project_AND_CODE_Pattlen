using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cGenDocNo
    {
        public bool C_PRCbGENTexNo(cRcvGenTexNo poParam, cmlShopDB poShopDB, out string ptErrMsg)
        {
            cMS oMsg;
            cResGenTexNo oRes;
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            
            //int nMax = 0;
            string tDocNo = "";
            //string tDocFmt = "";
            string tDocFmtChr = "";
            //string tDocFmtBch = "";
            //string tDocFmtPosShp = "";
            string tDocFmtYear = "";
            //string tDocFmtMonth = "";
            //string tDocFmtDay = "";
            //string tDocFmtSep = "";
            //string tDocFmtNum = "";
            string tDocFmtLeft = "";
            int nDocRuningLength = 0;

            string tMsgJson = "";
            int nRowAffect; //*Arm 63-06-07
            string tQueueName="";
            try
            {

                ptErrMsg = "";
                if (poParam == null) return false;
                oMsg = new cMS();
                oRes = new cResGenTexNo();
                oDB = new cDatabase();
                oSql = new StringBuilder();

                //if (poParam == null || string.IsNullOrEmpty(poParam.ptBchCode) || poParam.pnSaleType == null)
                //{
                //    // Validate parameter model false.
                //    oRes.rtCode = oMsg.tMS_RespCode701;
                //    oRes.rtDesc = oMsg.tMS_RespDesc701;
                //    tMsgJson = JsonConvert.SerializeObject(oRes);
                //    //C_PRCxMQResponse("CN_QRetGenTaxNo" + poParam.ptBchCode, tMsgJson, out ptErrMsg); //*Arm 63-05-23
                //    return true;
                //}


                tQueueName = "CN_QRetGenTaxNo_"+ poParam.ptDocNo +"_"+ poParam.ptUser; //*Arm 63-06-07
                string t_ConnStr = oDB.C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                
                //string tTblName = "TPSTTaxHD";
                //string tFedDocNo = "FTXshDocNo";
                string tDocLeft = "";
                int nDocType = (poParam.pnSaleType == 9 ? 5 : 4);
                

                //// # GEN Fmt
                //// ================================================================
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT * FROM TCNTAuto WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTSatTblName = '" + tTblName + "'");
                //oSql.AppendLine("AND FTSatStaDocType = '" + nDocType + "'");
                //odtTmp = oDB.C_DAToExecuteQuery(t_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                //if (odtTmp != null)
                //{
                //    if (odtTmp.Rows.Count > 0)
                //    {
                //        if (odtTmp.Rows[0].Field<string>("FTSatStaDefUsage").ToString() == "1")
                //        {
                //            //char
                //            tDocFmtChr = odtTmp.Rows[0].Field<string>("FTSatDefChar").ToString();
                //            //Branch
                //            tDocFmtBch = odtTmp.Rows[0].Field<string>("FTSatDefBch").ToString() == "1" ? poParam.ptBchCode : "";

                //            //Year
                //            tDocFmtYear = odtTmp.Rows[0].Field<string>("FTSatDefYear").ToString() == "1" ? string.Format("{0:yy}", DateTime.Now.Date) : "";
                //            //Month
                //            tDocFmtMonth = odtTmp.Rows[0].Field<string>("FTSatDefMonth").ToString() == "1" ? string.Format("{0:MM}", DateTime.Now.Date) : "";
                //            //Day
                //            tDocFmtDay = odtTmp.Rows[0].Field<string>("FTSatDefDay").ToString() == "1" ? string.Format("{0:dd}", DateTime.Now.Date) : "";
                //            //Sep
                //            tDocFmtSep = odtTmp.Rows[0].Field<string>("FTSatDefSep").ToString() == "1" ? "-" : "";
                //            //Num
                //            tDocFmtNum = odtTmp.Rows[0].Field<string>("FTSatDefNum").ToString();
                //        }
                //        else
                //        {
                //            //char
                //            tDocFmtChr = odtTmp.Rows[0].Field<string>("FTSatUsrChar").ToString();
                //            //Branch
                //            tDocFmtBch = odtTmp.Rows[0].Field<string>("FTSatUsrBch").ToString() == "1" ? poParam.ptBchCode : "";

                //            //Year
                //            tDocFmtYear = odtTmp.Rows[0].Field<string>("FTSatUsrYear").ToString() == "1" ? string.Format("{0:yy}", DateTime.Now.Date) : "";
                //            //Month
                //            tDocFmtMonth = odtTmp.Rows[0].Field<string>("FTSatUsrMonth").ToString() == "1" ? string.Format("{0:MM}", DateTime.Now.Date) : "";
                //            //Day
                //            tDocFmtDay = odtTmp.Rows[0].Field<string>("FTSatUsrDay").ToString() == "1" ? string.Format("{0:dd}", DateTime.Now.Date) : "";
                //            //Sep
                //            tDocFmtSep = odtTmp.Rows[0].Field<string>("FTSatUsrSep").ToString() == "1" ? "-" : "";
                //            //Num
                //            tDocFmtNum = odtTmp.Rows[0].Field<string>("FTSatUsrNum").ToString();
                //        }

                //        //tC_DocFmtLeft = tC_DocFmtChr + tC_DocFmtBch + tC_DocFmtPosShp + tC_DocFmtYear + tC_DocFmtMonth + tC_DocFmtDay + tC_DocFmtSep;
                //        tDocFmtLeft = tDocFmtChr + tDocFmtYear + tDocFmtMonth + tDocFmtDay + tDocFmtBch + tDocFmtPosShp + tDocFmtSep;   //*Arm 63-02-17 - ปรับ Fomat DocNo
                //        nDocRuningLength = tDocFmtNum.Length;

                //        tDocFmt = tDocFmtLeft + new string('#', nDocRuningLength); ; //*Em 63-02-28
                //    }
                //}
                //// # End GEN Fmt
                //// ================================================================


                // # GEN DocNo
                // ================================================================
                //string tFmt = new string('0', nDocRuningLength);
                //tDocLeft = tDocFmtLeft;

                //oSql.Clear();
                //oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + nDocRuningLength + ") AS FTMax");
                ////oSql.AppendLine("FROM " + tTblName + " WITH(NOLOCK)");//*Arm 63-06-07 Comment Code
                //oSql.AppendLine("FROM TPSTTaxNo WITH(NOLOCK)"); //*Arm 63-06-07
                //oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + poParam.ptBchCode + "' ");
                //oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + nDocRuningLength));
                //nMax = oDB.C_DAToExecuteQuery<int>(t_ConnStr,oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                ////tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", nMax + 1);//*Arm 63-06-07 Comment Code

                ////*Arm 63-06-07
                //if (nMax == 0)
                //{
                //    tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", nMax + 1);

                //    oSql.Clear();
                //    oSql.AppendLine("INSERT INTO TPSTTaxNo (FTBchCode,FTXshDocNo) VALUES ('" + poParam.ptBchCode + "', '" + tDocNo + "')");
                //    oDB.C_DATbExecuteNonQuery(t_ConnStr,oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //}
                //else
                //{
                //    tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", nMax + 1);

                //    oSql.Clear();
                //    oSql.AppendLine("UPDATE TPSTTaxNo with(rowlock) SET FTXshDocNo = '" + tDocNo + "' WHERE FTBchCode = '" + poParam.ptBchCode + "'");
                //    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //}
                ////+++++++++++++++


                //*Arm 63-06-07 
                tDocFmtYear = string.Format("{0:yy}", DateTime.Now.Date);
                if (nDocType == 5)
                {
                    tDocFmtChr = "R";
                }
                else
                {
                    tDocFmtChr = "S";
                }
                tDocFmtLeft = tDocFmtChr + tDocFmtYear + poParam.ptBchCode;   //*Arm 63-02-17 - ปรับ Fomat DocNo
                nDocRuningLength = 7;
                string tFmt = new string('0', nDocRuningLength);
                tDocLeft = tDocFmtLeft;

                oSql.Clear();
                oSql.AppendLine("SELECT FTXshDocNo ");
                oSql.AppendLine("FROM TPSTTaxNo WITH(NOLOCK)"); //*Arm 63-06-07
                oSql.AppendLine("WHERE FTBchCode = '" + poParam.ptBchCode + "' AND FNXshDocType = " + nDocType + " ");

                odtTmp = new DataTable();
                odtTmp = oDB.C_DAToExecuteQuery(t_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                //*Arm 63-06-07
                if (odtTmp == null || odtTmp.Rows.Count == 0)
                {
                    tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", 1);
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TPSTTaxNo (FTBchCode,FTXshDocNo,FNXshDocType) VALUES ('" + poParam.ptBchCode + "', '" + tDocNo + "', " + nDocType + " )");
                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                }
                else
                {
                    string tDoc = odtTmp.Rows[0].Field<string>("FTXshDocNo");
                    if (string.IsNullOrEmpty(tDoc))
                    {
                        tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", 1);
                    }
                    else
                    {
                        tDocNo = tDocLeft + string.Format("{0:" + tFmt + "}", Convert.ToInt32(tDoc.Substring(8, 7)) + 1);
                    }

                    oSql.Clear();
                    oSql.AppendLine("UPDATE TPSTTaxNo with(rowlock) SET FTXshDocNo = '" + tDocNo + "' WHERE FTBchCode = '" + poParam.ptBchCode + "' AND FNXshDocType = " + nDocType + " ");
                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                }
                //++++++++++++
                
                oRes.rtDocNo = tDocNo;
                oRes.rtCode = new cMS().tMS_RespCode001;
                oRes.rtDesc = new cMS().tMS_RespDesc001;

                tMsgJson = JsonConvert.SerializeObject(oRes);
                C_PRCxMQResponse(tQueueName, tMsgJson, out ptErrMsg); //*Arm 63-05-23
                
                return true;
            }
            catch(Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                oRes = new cResGenTexNo();
                oRes.rtCode = new cMS().tMS_RespCode900;
                oRes.rtDesc = new cMS().tMS_RespDesc900 + "/" + ptErrMsg;
                tMsgJson = JsonConvert.SerializeObject(oRes);

                C_PRCxMQResponse(tQueueName, tMsgJson, out ptErrMsg); //*Arm 63-05-23
               
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbGENTexNo");
                return true;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRes = null;
                odtTmp = null;
            }
        }

        /// <summary>
        /// *Arm 63-05-23
        /// Response
        /// </summary>
        /// <param name="ptQueueName"></param>
        /// <param name="ptMessage"></param>
        /// <param name="ptErrMsg"></param>
        public static void C_PRCxMQResponse(string ptQueueName, string ptMessage, out string ptErrMsg)
        {
            ConnectionFactory oFactory;
            cFunction oFunc = new cFunction();
            string tQueueName = ptQueueName;

            try
            {
                oFactory = new ConnectionFactory();
                oFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                oFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                oFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                oFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                using (var oConn = oFactory.CreateConnection())
                {
                    using (var oChannel = oConn.CreateModel())
                    {
                        var body = Encoding.UTF8.GetBytes(ptMessage);
                        oChannel.QueueDeclare(tQueueName, true, false, false, null);
                        oChannel.BasicPublish("", tQueueName, false, null, body);
                        ptErrMsg = "";
                    }
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
            }
            finally
            {
                oFactory = null;
            }
        }
    }
}
