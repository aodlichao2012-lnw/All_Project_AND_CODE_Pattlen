using MQReceivePrc.Models.Receive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MQReceivePrc.Models.Config;
using System.Data;
using MQReceivePrc.Models.WebService.Request;
using System.Net.Http;
using MQReceivePrc.Models.WebService.Response;
using RabbitMQ.Client;

namespace MQReceivePrc.Class
{
    public class cPrcWallet
    {
        private cConfig oC_Config;
         
        /// <summary>
        /// ประมวลการอนุมัติเอกสาร - ใบเบิกบัตร
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <param name="poShopDB"></param>
        /// <returns>
        /// true : Success
        /// false : Failed 
        /// </returns>
        public bool C_PRCbCrdRequest(string ptMessage,cmlShopDB poShopDB,ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvPrcWallet oRcvWallet;
            string tConnStr = "";
            string tUrlCrdOpen = poShopDB.tUrlAPI2FNWallet + "";
            DataTable odtTmp;
            List<cmlReqApvOpenRetCard> aCardApv;
            int nRowEffect = 0;
            int nRowCnt = 0,nCnt =0;    //*Em 62-01-29  Pandora
            string tDocNo ="";  //*Em 62-01-29  Pandora
            int nRowProg = 0;    // [Pong][2019-02-20][จำนวน Row Progress]
            double cProg = 0.00;
            int nRecordPerRound = poShopDB.nRecordPerRound;  //*Em 62-03-21  Pandora
            try
            {
                oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
                
                if (oRcvWallet == null) return false;
                if (poShopDB.tUrlAPI2FNWallet == null | poShopDB.tUrlAPI2FNWallet == "")
                {
                    ptErrMsg = "Url not found.";      //*Em 62-01-30  Pandora
                    return false;
                }
                //cMQReceiver.tC_BchData = oRcvWallet.ptBchCode;
                    
                tDocNo = oRcvWallet.ptDocNo;    //*Em 62-01-29  Pandora
                //tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oRcvWallet.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
                tConnStr = oRcvWallet.ptConnStr;

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCrdCode FROM TFNTCrdShiftDT WITH(NOLOCK) WHERE FTBchCode = '"+ oRcvWallet.ptBchCode +"' AND FTCshDocNo = '"+ oRcvWallet.ptDocNo +"'");
                oSql.AppendLine("AND FTCrdStaCrd = '1'");   //*Em 62-01-11  Pandora
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                if (odtTmp == null)
                {
                    ptErrMsg = "Data not found.";     //*Em 62-01-30  Pandora
                    return false;
                }
                    
                if (odtTmp.Rows.Count > 0)
                {
                    nRowCnt = 0;
                    nCnt = 0;
                    aCardApv = new List<cmlReqApvOpenRetCard>();
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        cmlReqApvOpenRetCard oCard = new cmlReqApvOpenRetCard();
                        oCard.ptBchCode = oRcvWallet.ptBchCode;
                        oCard.ptCrdCode = oRow.Field<string>("FTCrdCode");
                        oCard.ptDocNoRef = oRcvWallet.ptDocNo;  //*Em 61-12-12  Pandora
                        aCardApv.Add(oCard);
                        nRowCnt = nRowCnt + 1;
                        nCnt = nCnt + 1;
                        nRowProg = nRowProg + 1;  // [Pong][2019-02-20][Set Row Progress]
                        //*Em 62-01-29  Pandora
                        //if (nCnt == 100 || nRowCnt == odtTmp.Rows.Count)
                        if (nCnt == nRecordPerRound || nRowCnt == odtTmp.Rows.Count)    //*Em 62-03-21  Pandora
                        {
                            string tJson = JsonConvert.SerializeObject(aCardApv);
                            string tURLFunc = "/Topup/ApvOpenCard";

                            cClientService oCall = new cClientService();
                            HttpResponseMessage oResponse = new HttpResponseMessage();
                            try
                            {
                                oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2FNWallet + tURLFunc, tJson);
                            }
                            catch (Exception oEx)
                            {
                                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(tJsonResult))
                                {
                                    cmlResaoApvOpenRetCard oResult = JsonConvert.DeserializeObject<cmlResaoApvOpenRetCard>(tJsonResult);
                                    if (oResult.rtCode == "1")
                                    {
                                        foreach (cmlResApvOpenRetCard oRes in oResult.roApvOpenCard)
                                        {
                                            nRowEffect = 0;
                                            oSql = new StringBuilder();
                                            oSql.AppendLine("UPDATE TFNTCrdShiftDT WITH(ROWLOCK)");
                                            oSql.AppendLine("SET FTCrdStaPrc = '" + (string.Equals(oRes.rtStatus, "1") ? "1" : "2") + "' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                            oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                                            oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                                            oSql.AppendLine("AND FTCrdCode = '" + oRes.rtCrdCode + "'");
                                            oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                                            if (nRowEffect == 0)
                                            {
                                                ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ptErrMsg = oResult.rtDesc;  //*Em 62-01-30  Pandora
                                        return false;
                                    }
                                }
                                else
                                {
                                    ptErrMsg = "Data result not found.";     //*Em 62-01-30  Pandora
                                    return false;
                                }
                            }
                            else
                            {
                                ptErrMsg = oResponse.StatusCode.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            aCardApv = new List<cmlReqApvOpenRetCard>();
                            nCnt = 0;

                            //[Pong][2019-02-21][คำนวณ %]
                            cProg = Convert.ToDouble((nRowProg * 100) / odtTmp.Rows.Count);

                            //[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
                            if (!C_SETbResQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg, "RESREQUEST")) { return false; }
                        }
                        //++++++++++++++++++++++
                    }

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdShiftDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCrdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCrdStaCrd = '2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdShiftHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCshStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    } 
                    return true;
                }
                else
                {
                    //*Em 62-01-11  Pandora
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdShiftDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCrdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCrdStaCrd = '2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdShiftHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCshStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
            }
            catch(Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), tDocNo, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRcvWallet = null;
                odtTmp = null;
            }
        }
        
        /// <summary>
        /// ประมวลการอนุมัติเอกสาร - ใบคืนบัตร
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <param name="poShopDB"></param>
        /// <returns>
        /// true : Success
        /// false : Failed 
        /// </returns>
        public bool C_PRCbCrdReturn(string ptMessage, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvPrcWallet oRcvWallet;
            string tConnStr = "";
            string tUrlCrdOpen = poShopDB.tUrlAPI2FNWallet + "";
            DataTable odtTmp;
            List<cmlReqApvOpenRetCard> aCardApv;
            int nRowEffect = 0;
            int nRowCnt = 0, nCnt = 0;    //*Em 62-01-30  Pandora
            string tDocNo = "";  //*Em 62-01-30  Pandora
            int nRowProg = 0;    // [Pong][2019-02-20][จำนวน Row Progress]
            double cProg = 0.00;
            int nRecordPerRound = poShopDB.nRecordPerRound;  //*Em 62-03-21  Pandora

            try
            {
                oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
                
                if (oRcvWallet == null) return false;
                if (poShopDB.tUrlAPI2FNWallet == null | poShopDB.tUrlAPI2FNWallet == "")
                {
                    ptErrMsg = "Url not found.";      //*Em 62-01-30  Pandora
                    return false;
                }
                //cMQReceiver.tC_BchData = oRcvWallet.ptBchCode;
                
                tDocNo = oRcvWallet.ptDocNo;    //*Em 62-01-30  Pandora
                //tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oRcvWallet.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
                tConnStr = oRcvWallet.ptConnStr;

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCrdCode FROM TFNTCrdShiftDT WITH(NOLOCK) WHERE FTBchCode = '" + oRcvWallet.ptBchCode + "' AND FTCshDocNo = '" + oRcvWallet.ptDocNo + "'");
                oSql.AppendLine("AND FTCrdStaCrd = '1'");   //*Em 62-01-11  Pandora
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                if (odtTmp == null)
                {
                    ptErrMsg = "Data not found.";     //*Em 62-01-30  Pandora
                    return false;
                }
                if (odtTmp.Rows.Count > 0)
                {
                    nRowCnt = 0;    //*Em 62-01-30  Pandora
                    nCnt = 0;   //*Em 62-01-30  Pandora
                    aCardApv = new List<cmlReqApvOpenRetCard>();
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        cmlReqApvOpenRetCard oCard = new cmlReqApvOpenRetCard();
                        oCard.ptBchCode = oRcvWallet.ptBchCode;
                        oCard.ptCrdCode = oRow.Field<string>("FTCrdCode");
                        oCard.ptDocNoRef = oRcvWallet.ptDocNo;  //*Em 61-12-12  Pandora
                        aCardApv.Add(oCard);
                        nRowCnt = nRowCnt + 1;
                        nCnt = nCnt + 1;
                        nRowProg = nRowProg + 1;  // [Pong][2019-02-20][Set Row Progress]]
                        //*Em 62-01-29  Pandora
                        //if (nCnt == 100 || nRowCnt == odtTmp.Rows.Count)
                        if (nCnt == nRecordPerRound || nRowCnt == odtTmp.Rows.Count)    //*Em 62-03-21  Pandora
                        {
                            string tJson = JsonConvert.SerializeObject(aCardApv);
                            string tURLFunc = "/Topup/ApvReturnCard";

                            cClientService oCall = new cClientService();
                            HttpResponseMessage oResponse = new HttpResponseMessage();
                            try
                            {
                                oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2FNWallet + tURLFunc, tJson);
                            }
                            catch (Exception oEx)
                            {
                                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(tJsonResult))
                                {
                                    cmlResaoApvOpenRetCard oResult = JsonConvert.DeserializeObject<cmlResaoApvOpenRetCard>(tJsonResult);
                                    if (oResult.rtCode == "1")
                                    {
                                        foreach (cmlResApvOpenRetCard oRes in oResult.roApvOpenCard)
                                        {
                                            oSql = new StringBuilder();
                                            oSql.AppendLine("UPDATE TFNTCrdShiftDT WITH(ROWLOCK)");
                                            oSql.AppendLine("SET FTCrdStaPrc = '" + (string.Equals(oRes.rtStatus, "1") ? "1" : "2") + "' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                            oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                                            oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                                            oSql.AppendLine("AND FTCrdCode = '" + oRes.rtCrdCode + "'");
                                            oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                                            if (nRowEffect == 0)
                                            {
                                                ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ptErrMsg = oResult.rtDesc;  //*Em 62-01-30  Pandora
                                        return false;
                                    }
                                }
                                else
                                {
                                    ptErrMsg = "Data result not found.";     //*Em 62-01-30  Pandora
                                    return false;
                                }
                            }
                            else
                            {
                                ptErrMsg = oResponse.StatusCode.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }
                            aCardApv = new List<cmlReqApvOpenRetCard>();
                            nCnt = 0;

                            //[Pong][2019-02-21][คำนวณ %]
                            cProg = Convert.ToDouble((nRowProg * 100) / odtTmp.Rows.Count);

                            ////[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
                            if (!C_SETbResQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg, "RESRETURN")) { return false; }
                        }
                        //++++++++++++++++++++++
                    }
                    //*Em 62-01-11  Pandora
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdShiftDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCrdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCrdStaCrd = '2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //+++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdShiftHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCshStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
                else
                {
                    //*Em 62-01-11  Pandora
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdShiftDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCrdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCrdStaCrd = '2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdShiftHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCshStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCshDocNo='" + oRcvWallet.ptDocNo + "')");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
            }
            catch(Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), tDocNo, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRcvWallet = null;
                odtTmp = null;
            }
        }

        /// <summary>
        /// ประมวลการอนุมัติเอกสาร - ใบเติมเงิน
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <param name="poShopDB"></param>
        /// <returns>
        /// true : Success
        /// false : Failed 
        /// </returns>
        public bool C_PRCbCrdTopUp(string ptMessage, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvPrcWallet oRcvWallet;
            string tConnStr = "";
            string tUrlCrdOpen = poShopDB.tUrlAPI2FNWallet + "";
            DataTable odtTmp;
            List<cmlReqTopupList> aCardTopup;
            int nRowEffect = 0;
            int nRowCnt = 0, nCnt = 0;    //*Em 62-01-30  Pandora
            string tDocNo = "";  //*Em 62-01-30  Pandora
            int nRowProg = 0;    // [Pong][2019-02-20][จำนวน Row Progress]
            double cProg = 0.00;
            int nRecordPerRound = poShopDB.nRecordPerRound;  //*Em 62-03-21  Pandora

            oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
            try
            {
                oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
                if (oRcvWallet == null) return false;
                if (poShopDB.tUrlAPI2FNWallet == null | poShopDB.tUrlAPI2FNWallet == "")
                {
                    ptErrMsg = "Url not found.";      //*Em 62-01-30  Pandora
                    return false;
                }
                //cMQReceiver.tC_BchData = oRcvWallet.ptBchCode;
                tDocNo = oRcvWallet.ptDocNo;    //*Em 62-01-30  Pandora
                //tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oRcvWallet.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
                tConnStr = oRcvWallet.ptConnStr;

                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT DT.FTCrdCode ,HD.FCCthAmtTP");
                oSql.AppendLine("SELECT DT.FTCrdCode ,DT.FCCtdCrdTP");  //*Em 62-01-11  Pandora
                oSql.AppendLine("FROM TFNTCrdTopUpDT DT WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TFNTCrdTopUpHD HD WITH(NOLOCK) ON DT.FTCthDocNo = HD.FTCthDocNo");
                oSql.AppendLine("WHERE HD.FTBchCode = '" + oRcvWallet.ptBchCode + "' AND HD.FTCthDocNo = '" + oRcvWallet.ptDocNo + "'");
                oSql.AppendLine("AND HD.FTCthDocType='1' AND HD.FTCthDocFunc='1'");
                oSql.AppendLine("AND DT.FTCtdStaCrd='1'");
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                if (odtTmp == null)
                {
                    ptErrMsg = "Data not found.";     //*Em 62-01-30  Pandora
                    return false;
                }
                if (odtTmp.Rows.Count > 0)
                {
                    nRowCnt = 0;    //*Em 62-01-30  Pandora
                    nCnt = 0;   //*Em 62-01-30  Pandora
                    aCardTopup = new List<cmlReqTopupList>();
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        cmlReqTopupList oCard = new cmlReqTopupList();
                        oCard.ptBchCode = oRcvWallet.ptBchCode;
                        oCard.ptCrdCode = oRow.Field<string>("FTCrdCode");
                        //oCard.pcTxnValue = oRow.Field<decimal>("FCCthAmtTP");
                        oCard.pcTxnValue = oRow.Field<decimal>("FCCtdCrdTP");   //*Em 62-01-11  Pandora
                        oCard.ptAuto = "0";
                        oCard.ptTxnPosCode = "";
                        oCard.ptShpCode = "";
                        oCard.ptDocNoRef = oRcvWallet.ptDocNo;  //*Em 61-12-10  Pandora
                        aCardTopup.Add(oCard);

                        nRowCnt = nRowCnt + 1;
                        nCnt = nCnt + 1;
                        nRowProg = nRowProg + 1;  // [Pong][2019-02-20][Set Row Progress]
                        //*Em 62-01-29  Pandora
                        //if (nCnt == 100 || nRowCnt == odtTmp.Rows.Count)
                        if (nCnt == nRecordPerRound || nRowCnt == odtTmp.Rows.Count)    //*Em 62-03-21  Pandora
                        {
                            string tJson = JsonConvert.SerializeObject(aCardTopup);
                            string tURLFunc = "/Topup/TopupList";

                            cClientService oCall = new cClientService();
                            HttpResponseMessage oResponse = new HttpResponseMessage();
                            try
                            {
                                oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2FNWallet + tURLFunc, tJson);
                            }
                            catch (Exception oEx)
                            {
                                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(tJsonResult))
                                {
                                    cmlResaoTopupList oResult = JsonConvert.DeserializeObject<cmlResaoTopupList>(tJsonResult);
                                    if (oResult.rtCode == "1")
                                    {
                                        foreach (cmlResTopupList oRes in oResult.roTopupList)
                                        {
                                            oSql = new StringBuilder();
                                            oSql.AppendLine("UPDATE TFNTCrdTopUpDT WITH(ROWLOCK)");
                                            oSql.AppendLine("SET FTCtdStaPrc = '" + (string.Equals(oRes.rtStatus, "1") ? "1" : "2") + "' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                            oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                                            oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "')");
                                            oSql.AppendLine("AND FTCrdCode = '" + oRes.rtCrdCode + "'");
                                            oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                                            if (nRowEffect == 0)
                                            {
                                                ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ptErrMsg = oResult.rtDesc;  //*Em 62-01-30  Pandora
                                        return false;
                                    }
                                }
                                else
                                {
                                    ptErrMsg = "Data result not found.";     //*Em 62-01-30  Pandora
                                    return false;
                                }
                            }
                            else
                            {
                                ptErrMsg = oResponse.StatusCode.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }
                            nCnt = 0;   //*Em 62-01-30  Pandora
                            aCardTopup = new List<cmlReqTopupList>();

                            //[Pong][2019-02-21][คำนวณ %]
                            cProg = Convert.ToDouble((nRowProg * 100) / odtTmp.Rows.Count);

                            //[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
                            // if (!C_SETbResTopupQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg)) { return false; }
                            if (!C_SETbResQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg, "RESTOPUP")) { return false; }
                        }
                        //++++++++++++++++++++++
                    }

                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdTopUpDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCtdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "') AND FTCtdStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //+++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdTopUpHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCthStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCthDocType='1' AND FTCthDocFunc='1'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0){
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }

                    return true;
                }
                else
                {
                    //*Em 62-01-11  Pandora
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdTopUpDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCtdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "') AND FTCtdStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //+++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdTopUpHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCthStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCthDocType='1' AND FTCthDocFunc='1'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
                
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), tDocNo, "", "");  //*Em 62-01-30  Pandora

                //[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
            //    if (!C_SETbResTopupQueuse("0|" + oEx.Message, oRcvWallet, ref ptErrMsg)) { return false; }
                //  C_DELbQueuseRes(oRcvWallet, ref ptErrMsg);
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRcvWallet = null;
                odtTmp = null;
                nRowProg = 0;
                cProg = 0.00;
            }
        }

        /// <summary>
        /// ประมวลการอนุมัติเอกสาร - ใบคืนเงิน
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <param name="poShopDB"></param>
        /// <returns>
        /// true : Success
        /// false : Failed 
        /// </returns>
        public bool C_PRCbCrdReturnTopUp(string ptMessage, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvPrcWallet oRcvWallet;
            string tConnStr = "";
            string tUrlCrdOpen = poShopDB.tUrlAPI2FNWallet + "";
            DataTable odtTmp;
            List<cmlReqReturnTopupList> aCardVoid;
            int nRowEffect = 0;
            int nRowCnt = 0, nCnt = 0;    //*Em 62-01-30  Pandora
            string tDocNo = "";  //*Em 62-01-30  Pandora

            int nRowProg = 0;    // [Pong][2019-02-20][จำนวน Row Progress]
            double cProg = 0.00;
            int nRecordPerRound = poShopDB.nRecordPerRound;  //*Em 62-03-21  Pandora

            try
            {
                oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
                if (oRcvWallet == null) return false;
                if (poShopDB.tUrlAPI2FNWallet == null | poShopDB.tUrlAPI2FNWallet == "")
                {
                    ptErrMsg = "Url not found.";      //*Em 62-01-30  Pandora
                    return false;
                }
                //cMQReceiver.tC_BchData = oRcvWallet.ptBchCode;
                tDocNo = oRcvWallet.ptDocNo;    //*Em 62-01-30  Pandora
                //tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oRcvWallet.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
                tConnStr = oRcvWallet.ptConnStr;

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCrdCode,FCCtdCrdTP FROM TFNTCrdTopUpDT WITH(NOLOCK) ");    //*Em 62-01-16  Pandora
                oSql.AppendLine("WHERE FTBchCode = '" + oRcvWallet.ptBchCode + "' AND FTCthDocNo = '" + oRcvWallet.ptDocNo + "' ");
                oSql.AppendLine("AND FTCtdStaCrd='1'");
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                if (odtTmp == null)
                {
                    ptErrMsg = "Data not found.";     //*Em 62-01-30  Pandora
                    return false;
                }
                if (odtTmp.Rows.Count > 0)
                {
                    nRowCnt = 0;    //*Em 62-01-30  Pandora
                    nCnt = 0;   //*Em 62-01-30  Pandora
                    nRowProg = nRowProg + 1;  // [Pong][2019-02-20][Set Row Progress]
                    aCardVoid = new List<cmlReqReturnTopupList>();
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        cmlReqReturnTopupList oCard = new cmlReqReturnTopupList();
                        oCard.ptBchCode = oRcvWallet.ptBchCode;
                        oCard.ptCrdCode = oRow.Field<string>("FTCrdCode");
                        oCard.pcTxnValue = oRow.Field<decimal>("FCCtdCrdTP");   //*Em 62-01-16  Pandora
                        oCard.ptAuto = "0";  //*Em 62-01-16  Pandora
                        oCard.ptTxnPosCode = "";     //*Em 62-01-16  Pandora
                        oCard.ptShpCode = "";    //*Em 62-01-16  Pandora
                        oCard.ptDocNoRef = oRcvWallet.ptDocNo;  //*Em 61-12-11  Pandora
                        aCardVoid.Add(oCard);

                        nRowCnt = nRowCnt + 1;
                        nCnt = nCnt + 1;
                        nRowProg = nRowProg + 1;
                        //*Em 62-01-29  Pandora
                        //if (nCnt == 100 || nRowCnt == odtTmp.Rows.Count)
                        if (nCnt == nRecordPerRound || nRowCnt == odtTmp.Rows.Count)    //*Em 62-03-21  Pandora
                        {
                            string tJson = JsonConvert.SerializeObject(aCardVoid);
                            string tURLFunc = "/Topup/ReturnTopupList";

                            cClientService oCall = new cClientService();
                            HttpResponseMessage oResponse = new HttpResponseMessage();
                            try
                            {
                                oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2FNWallet + tURLFunc, tJson);
                            }
                            catch (Exception oEx)
                            {
                                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(tJsonResult))
                                {
                                    cmlResaoReturnTopupList oResult = JsonConvert.DeserializeObject<cmlResaoReturnTopupList>(tJsonResult);
                                    if (oResult.rtCode == "1")
                                    {
                                        foreach (cmlResReturnTopupList oRes in oResult.roReturnTopupList)
                                        {
                                            nRowEffect = 0;
                                            oSql = new StringBuilder();
                                            oSql.AppendLine("UPDATE TFNTCrdTopUpDT WITH(ROWLOCK)");
                                            oSql.AppendLine("SET FTCtdStaPrc = '" + (string.Equals(oRes.rtStatus, "1") ? "1" : "2") + "' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                            oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                                            oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "')");
                                            oSql.AppendLine("AND FTCrdCode = '" + oRes.rtCrdCode + "'");
                                            oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                                            if (nRowEffect == 0)
                                            {
                                                ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ptErrMsg = oResult.rtDesc;  //*Em 62-01-30  Pandora
                                        return false;
                                    }
                                }
                                else
                                {
                                    ptErrMsg = "Data result not found.";     //*Em 62-01-30  Pandora
                                    return false;
                                }
                            }
                            else
                            {
                                ptErrMsg = oResponse.StatusCode.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }
                            nCnt = 0;   //*Em 62-01-30  Pandora
                            aCardVoid = new List<cmlReqReturnTopupList>();

                            //[Pong][2019-02-21][คำนวณ %]
                            cProg = Convert.ToDouble((nRowProg * 100) / odtTmp.Rows.Count);

                            //[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
                            if (!C_SETbResQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg, "RESVOIDTOPUP")) { return false; }
                        }
                        //++++++++++++++++++++++
                    }

                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdTopUpDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCtdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "') AND FTCtdStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //+++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdTopUpHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCthStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCthDocType='5' AND FTCthDocFunc='5'");   //ที่มาจาก Function เรียกใช้งาน 1:เติมเงิน 2:แลกบัตร 5:คืนเงิน
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
                else
                {
                    //*Em 62-01-11  Pandora
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdTopUpDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCtdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "') AND FTCtdStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //+++++++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdTopUpHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCthStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCthDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCthDocType='5' AND FTCthDocFunc='5'");   //ที่มาจาก Function เรียกใช้งาน 1:เติมเงิน 2:แลกบัตร 5:คืนเงิน
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), tDocNo, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRcvWallet = null;
                odtTmp = null;
            }
        }

        /// <summary>
        /// ประมวลการอนุมัติเอกสาร - ใบปรับสถานะบัตร
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <param name="poShopDB"></param>
        /// <returns>
        /// true : Success
        /// false : Failed 
        /// </returns>
        public bool C_PRCbCrdAdjStatus(string ptMessage, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvPrcWallet oRcvWallet;
            string tConnStr = "";
            string tUrlCrdOpen = poShopDB.tUrlAPI2FNWallet + "";
            DataTable odtTmp;
            List<cmlReqChangeSta> aCardApv;
            int nRowEffect = 0;
            int nRowCnt = 0, nCnt = 0;    //*Em 62-01-30  Pandora
            string tDocNo = "";  //*Em 62-01-30  Pandora

            int nRowProg = 0;    // [Pong][2019-02-20][จำนวน Row Progress]
            double cProg = 0.00;
            int nRecordPerRound = poShopDB.nRecordPerRound;  //*Em 62-03-21  Pandora

            try
            {
                oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
                if (oRcvWallet == null) return false;
                if (poShopDB.tUrlAPI2FNWallet == null | poShopDB.tUrlAPI2FNWallet == "")
                {
                    ptErrMsg = "Url not found.";      //*Em 62-01-30  Pandora
                    return false;
                }
                //cMQReceiver.tC_BchData = oRcvWallet.ptBchCode;
                tDocNo = oRcvWallet.ptDocNo;    //*Em 62-01-30  Pandora
                //tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oRcvWallet.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
                tConnStr = oRcvWallet.ptConnStr;

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT DT.FTCvdOldCode,HD.FTCvhStaCrdActive ");
                oSql.AppendLine("FROM TFNTCrdVoidDT DT WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TFNTCrdVoidHD HD WITH(NOLOCK) ON DT.FTCvhDocNo = HD.FTCvhDocNo");
                oSql.AppendLine("WHERE HD.FTBchCode = '" + oRcvWallet.ptBchCode + "' AND HD.FTCvhDocNo = '" + oRcvWallet.ptDocNo + "'");
                oSql.AppendLine("AND DT.FTCvdStaCrd = '1'");
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                if (odtTmp == null)
                {
                    ptErrMsg = "Data not found.";     //*Em 62-01-30  Pandora
                    return false;
                }
                if (odtTmp.Rows.Count > 0)
                {
                    nRowCnt = 0;    //*Em 62-01-30  Pandora
                    nCnt = 0;   //*Em 62-01-30  Pandora
                    aCardApv = new List<cmlReqChangeSta>();
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        cmlReqChangeSta oCard = new cmlReqChangeSta();
                        oCard.ptBchCode = oRcvWallet.ptBchCode;
                        oCard.ptCrdCode = oRow.Field<string>("FTCvdOldCode");
                        oCard.ptCrdSta = oRow.Field<string>("FTCvhStaCrdActive");
                        oCard.ptDocNoRef = oRcvWallet.ptDocNo;  //*Em 61-12-12  Pandora
                        aCardApv.Add(oCard);

                        nRowCnt = nRowCnt + 1;
                        nCnt = nCnt + 1;
                        nRowProg = nRowProg + 1;  // [Pong][2019-02-20][Set Row Progress]]

                        //*Em 62-01-29  Pandora
                        //if (nCnt == 100 || nRowCnt == odtTmp.Rows.Count)
                        if (nCnt == nRecordPerRound  || nRowCnt == odtTmp.Rows.Count)   //*Em 62-03-21  Pandora
                        {
                            string tJson = JsonConvert.SerializeObject(aCardApv);
                            string tURLFunc = "/Card/ChangSta";

                            cClientService oCall = new cClientService();
                            HttpResponseMessage oResponse = new HttpResponseMessage();
                            try
                            {
                                oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2FNWallet + tURLFunc, tJson);
                            }
                            catch (Exception oEx)
                            {
                                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(tJsonResult))
                                {
                                    cmlResChangeSta oResult = JsonConvert.DeserializeObject<cmlResChangeSta>(tJsonResult);
                                    if (oResult.rtCode == "1")
                                    {
                                        foreach (cmlResaoChangeSta oRes in oResult.raoCancelCard)
                                        {
                                            nRowEffect = 0;
                                            oSql = new StringBuilder();
                                            oSql.AppendLine("UPDATE TFNTCrdVoidDT WITH(ROWLOCK)");
                                            oSql.AppendLine("SET FTCvdStaPrc = '" + (string.Equals(oRes.rtStatus, "1") ? "1" : "2") + "' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                            oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                                            oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "')");
                                            oSql.AppendLine("AND FTCvdOldCode = '" + oRes.rtCrdCode + "'");
                                            oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                                            if (nRowEffect == 0)
                                            {
                                                ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ptErrMsg = oResult.rtDesc;  //*Em 62-01-30  Pandora
                                        return false;
                                    }
                                }
                                else
                                {
                                    ptErrMsg = "Data result not found.";     //*Em 62-01-30  Pandora
                                    return false;
                                }
                            }
                            else
                            {
                                ptErrMsg = oResponse.StatusCode.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }
                            nCnt = 0;   //*Em 62-01-30  Pandora
                            aCardApv = new List<cmlReqChangeSta>();

                            //[Pong][2019-02-21][คำนวณ %]
                            cProg = Convert.ToDouble((nRowProg * 100) / odtTmp.Rows.Count);

                            //[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
                            if (!C_SETbResQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg, "RESADJSTATUS")) { return false; }
                        }
                        //+++++++++++++++++++++++
                    }
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdVoidDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCvdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "') AND FTCvdStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdVoidHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCvhStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCvhDocType='1'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
                else
                {
                    //*Em 62-01-11  Pandora
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdVoidDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCvdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "') AND FTCvdStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdVoidHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCvhStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCvhDocType='1'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), tDocNo, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRcvWallet = null;
                odtTmp = null;
            }
        }

        /// <summary>
        /// ประมวลการอนุมัติเอกสาร - ใบเปลี่ยนบัตร
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <param name="poShopDB"></param>
        /// <returns>
        /// true : Success
        /// false : Failed 
        /// </returns>
        public bool C_PRCbCrdSwap(string ptMessage, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvPrcWallet oRcvWallet;
            string tConnStr = "";
            string tUrlCrdOpen = poShopDB.tUrlAPI2FNWallet + "";
            DataTable odtTmp;
            List<cmlReqChangeCard> aCardSwap;
            int nRowEffect = 0;
            int nRowCnt = 0, nCnt = 0;    //*Em 62-01-30  Pandora
            string tDocNo = "";  //*Em 62-01-30  Pandora
            int nRowProg = 0;    // [Pong][2019-02-20][จำนวน Row Progress]
            double cProg = 0.00;
            int nRecordPerRound = poShopDB.nRecordPerRound;  //*Em 62-03-21  Pandora
            try
            {
                oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
                if (oRcvWallet == null) return false;
                if (poShopDB.tUrlAPI2FNWallet == null | poShopDB.tUrlAPI2FNWallet == "")
                {
                    ptErrMsg = "Url not found.";      //*Em 62-01-30  Pandora
                    return false;
                }
                //cMQReceiver.tC_BchData = oRcvWallet.ptBchCode;
                tDocNo = oRcvWallet.ptDocNo;    //*Em 62-01-30  Pandora
                //tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oRcvWallet.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
                tConnStr = oRcvWallet.ptConnStr;

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCvdOldCode,FTCvdNewCode FROM TFNTCrdVoidDT WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTBchCode = '" + oRcvWallet.ptBchCode + "' AND FTCvhDocNo = '" + oRcvWallet.ptDocNo + "'");
                oSql.AppendLine("AND FTCvdStaCrd='1'");
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                if (odtTmp == null)
                {
                    ptErrMsg = "Data not found.";     //*Em 62-01-30  Pandora
                    return false;
                }
                if (odtTmp.Rows.Count > 0)
                {
                    nRowCnt = 0;    //*Em 62-01-30  Pandora
                    nCnt = 0;   //*Em 62-01-30  Pandora
                    nRowProg = nRowProg + 1;  // [Pong][2019-02-20][Set Row Progress]
                    aCardSwap = new List<cmlReqChangeCard>();
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        cmlReqChangeCard oCard = new cmlReqChangeCard();
                        oCard.ptBchCode = oRcvWallet.ptBchCode;
                        oCard.ptFrmCrdCode = oRow.Field<string>("FTCvdOldCode");
                        oCard.ptToCrdCode = oRow.Field<string>("FTCvdNewCode");
                        oCard.ptDocNoRef = oRcvWallet.ptDocNo;  //*Em 61-12-12  Pandora
                        aCardSwap.Add(oCard);

                        nRowCnt = nRowCnt + 1;
                        nCnt = nCnt + 1;
                        //*Em 62-01-29  Pandora
                        //if (nCnt == 100 || nRowCnt == odtTmp.Rows.Count)
                        if (nCnt == nRecordPerRound || nRowCnt == odtTmp.Rows.Count)    //*Em 62-03-21  Pandora
                        {
                            string tJson = JsonConvert.SerializeObject(aCardSwap);
                            string tURLFunc = "/Card/ChangeCardLis";

                            cClientService oCall = new cClientService();
                            HttpResponseMessage oResponse = new HttpResponseMessage();
                            try
                            {
                                oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2FNWallet + tURLFunc, tJson);
                            }
                            catch (Exception oEx)
                            {
                                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(tJsonResult))
                                {
                                    cmlResChangeCard oResult = JsonConvert.DeserializeObject<cmlResChangeCard>(tJsonResult);
                                    if (oResult.rtCode == "1")
                                    {
                                        foreach (cmlResaoChangeCard oRes in oResult.raoChangeCard)
                                        {
                                            nRowEffect = 0;
                                            oSql = new StringBuilder();
                                            oSql.AppendLine("UPDATE TFNTCrdVoidDT WITH(ROWLOCK)");
                                            oSql.AppendLine("SET FTCvdStaPrc = '" + (string.Equals(oRes.rtStatus, "1") ? "1" : "2") + "' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                            oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                                            oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "')");
                                            oSql.AppendLine("AND FTCvdOldCode = '" + oRes.rtFrmCrdCode + "'");
                                            oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                                            if (nRowEffect == 0)
                                            {
                                                ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                                                return false;
                                            }
                                        }  
                                    }
                                    else
                                    {
                                        ptErrMsg = oResult.rtDesc;  //*Em 62-01-30  Pandora
                                        return false;
                                    }
                                }
                                else
                                {
                                    ptErrMsg = "Data result not found.";     //*Em 62-01-30  Pandora
                                    return false;
                                }
                            }
                            else
                            {
                                ptErrMsg = oResponse.StatusCode.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }
                            nCnt = 0;   //*Em 62-01-30  Pandora
                            aCardSwap = new List<cmlReqChangeCard>();

                            //[Pong][2019-02-21][คำนวณ %]
                            //cProg = Convert.ToDouble((nRowProg * 100) / odtTmp.Rows.Count);

                            //*Arm 62-10-28 ขแก้ไข [คำนวณ %]
                            cProg = Convert.ToDouble((nRowCnt * 100) / odtTmp.Rows.Count);

                            //[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
                            if (!C_SETbResQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg, "RESSWAP")) { return false; }

                        }
                        //+++++++++++++++++++++++
                    }
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdVoidDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCvdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "') AND FTCvdStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdVoidHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCvhStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCvhDocType='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
                else
                {
                    //*Em 62-01-11  Pandora
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdVoidDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCvdStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "') AND FTCvdStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdVoidHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCvhStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCvhDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCvhDocType='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), tDocNo, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRcvWallet = null;
                odtTmp = null;
            }
        }

        /// <summary>
        /// ประมวลการอนุมัติเอกสาร - นำเข้าบัตรใหม่
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <param name="poShopDB"></param>
        /// <returns>
        /// true : Success
        /// false : Failed 
        /// </returns>
        public bool C_PRCbCrdNew(string ptMessage, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvPrcWallet oRcvWallet;
            string tConnStr = "";
            string tUrlCrdOpen = poShopDB.tUrlAPI2FNWallet + "";
            DataTable odtTmp;
            List<cmlReqCardNewList> aCard;
            int nRowEffect = 0;
            int nRowCnt = 0, nCnt = 0;    //*Em 62-01-30  Pandora
            string tDocNo = "";  //*Em 62-01-30  Pandora

            int nRowProg = 0;    // [Pong][2019-02-20][จำนวน Row Progress]
            double cProg = 0.00;
            int nRecordPerRound = poShopDB.nRecordPerRound;  //*Em 62-03-21  Pandora

            try
            {
                oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
                if (oRcvWallet == null) return false;
                if (poShopDB.tUrlAPI2FNWallet == null | poShopDB.tUrlAPI2FNWallet == "")
                {
                    ptErrMsg = "Url not found.";      //*Em 62-01-30  Pandora
                    return false;
                }
                tDocNo = oRcvWallet.ptDocNo;    //*Em 62-01-30  Pandora
                //tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oRcvWallet.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
                tConnStr = oRcvWallet.ptConnStr;

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCidCrdCode,FTCtyCode,FTCidCrdHolderID,FTCidCrdDepart,FTCidCrdName ");
                oSql.AppendLine("FROM TFNTCrdImpDT WITH(NOLOCK) WHERE FTBchCode = '" + oRcvWallet.ptBchCode + "' AND FTCihDocNo = '" + oRcvWallet.ptDocNo + "'");
                oSql.AppendLine("AND FTCidStaCrd = '1' ");
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                if (odtTmp == null)
                {
                    ptErrMsg = "Data not found.";     //*Em 62-01-30  Pandora
                    return false;
                }
                if (odtTmp.Rows.Count > 0)
                {
                    nRowCnt = 0;    //*Em 62-01-30  Pandora
                    nCnt = 0;   //*Em 62-01-30  Pandora
                    aCard = new List<cmlReqCardNewList>();
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        cmlReqCardNewList oCard = new cmlReqCardNewList();
                        oCard.pnLngID = 1;
                        oCard.ptBchCode = oRcvWallet.ptBchCode;
                        oCard.ptCrdCode = oRow.Field<string>("FTCidCrdCode");
                        oCard.ptCtyCode = oRow.Field<string>("FTCtyCode");
                        oCard.ptCrdHolderID = oRow.Field<string>("FTCidCrdHolderID");
                        oCard.ptDptCode = oRow.Field<string>("FTCidCrdDepart");
                        oCard.ptCrdName = oRow.Field<string>("FTCidCrdName");
                        aCard.Add(oCard);

                        nRowCnt = nRowCnt + 1;
                        nCnt = nCnt + 1;
                        nRowProg = nRowProg + 1;  // [Pong][2019-02-20][Set Row Progress]]
                        //*Em 62-01-29  Pandora
                        //if (nCnt == 100 || nRowCnt == odtTmp.Rows.Count)
                        if (nCnt == nRecordPerRound || nRowCnt == odtTmp.Rows.Count)    //*Em 62-03-21  Pandora
                        {
                            string tJson = JsonConvert.SerializeObject(aCard);
                            string tURLFunc = "/Card/CardNewList";

                            cClientService oCall = new cClientService();
                            HttpResponseMessage oResponse = new HttpResponseMessage();
                            try
                            {
                                oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2FNWallet + tURLFunc, tJson);
                            }
                            catch (Exception oEx)
                            {
                                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(tJsonResult))
                                {
                                    cmlResCardNewList oResult = JsonConvert.DeserializeObject<cmlResCardNewList>(tJsonResult);
                                    if (oResult.rtCode == "1")
                                    {
                                        foreach (cmlResaoCardNewList oRes in oResult.raoCardNewList)
                                        {
                                            nRowEffect = 0;
                                            oSql = new StringBuilder();
                                            oSql.AppendLine("UPDATE TFNTCrdImpDT WITH(ROWLOCK)");
                                            oSql.AppendLine("SET FTCidStaPrc = '" + (string.Equals(oRes.rtStatus, "1") ? "1" : "2") + "' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                            oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "')");
                                            oSql.AppendLine("AND FTCidCrdCode = '" + oRes.rtCrdCode + "'");
                                            oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                                            if (nRowEffect == 0)
                                            {
                                                ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ptErrMsg = oResult.rtDesc;  //*Em 62-01-30  Pandora
                                        return false;
                                    }
                                }
                                else
                                {
                                    ptErrMsg = "Data result not found.";     //*Em 62-01-30  Pandora
                                    return false;
                                }
                            }
                            else
                            {
                                ptErrMsg = oResponse.StatusCode.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }
                            nCnt = 0;   //*Em 62-01-30  Pandora
                            aCard = new List<cmlReqCardNewList>();

                            //[Pong][2019-02-21][คำนวณ %]
                            cProg = Convert.ToDouble((nRowProg * 100) / odtTmp.Rows.Count);

                            //[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
                            if (!C_SETbResQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg, "RESNEW")) { return false; }
                        }
                        //+++++++++++++++++++++++
                    }
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdImpDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCidStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "') AND FTCidStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //+++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdImpHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCihStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                                                   //oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                                                   //oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCihDocType='1'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
                else
                {
                    //*Em 62-01-11  Pandora
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdImpDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCidStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "') AND FTCidStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdImpHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCihStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                                                   //oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                                                   //oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCihDocType='1'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), tDocNo, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRcvWallet = null;
                odtTmp = null;
            }
        }

        /// <summary>
        /// ประมวลการอนุมัติเอกสาร - ล้างบัตร
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <param name="poShopDB"></param>
        /// <returns>
        /// true : Success
        /// false : Failed 
        /// </returns>
        public bool C_PRCbCrdClear(string ptMessage, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvPrcWallet oRcvWallet;
            string tConnStr = "";
            string tUrlCrdOpen = poShopDB.tUrlAPI2FNWallet + "";
            DataTable odtTmp;
            List<cmlReqCardClearList> aCardClear;
            int nRowEffect = 0;
            int nRowCnt = 0, nCnt = 0;    //*Em 62-01-30  Pandora
            string tDocNo = "";  //*Em 62-01-30  Pandora

            int nRowProg = 0;    // [Pong][2019-02-20][จำนวน Row Progress]
            double cProg = 0.00;
            int nRecordPerRound = poShopDB.nRecordPerRound;  //*Em 62-03-21  Pandora

            try
            {
                oRcvWallet = JsonConvert.DeserializeObject<cmlRcvPrcWallet>(ptMessage);
                if (oRcvWallet == null) return false;
                if (poShopDB.tUrlAPI2FNWallet == null | poShopDB.tUrlAPI2FNWallet == "")
                {
                    ptErrMsg = "Url not found.";      //*Em 62-01-30  Pandora
                    return false;
                }
                tDocNo = oRcvWallet.ptDocNo;    //*Em 62-01-30  Pandora
                //tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oRcvWallet.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
                tConnStr = oRcvWallet.ptConnStr;

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCidCrdCode FROM TFNTCrdImpDT WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTBchCode = '" + oRcvWallet.ptBchCode + "' AND FTCihDocNo = '" + oRcvWallet.ptDocNo + "'");
                oSql.AppendLine("AND FTCidStaCrd = '1' ");
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                if (odtTmp == null)
                {
                    ptErrMsg = "Data not found.";     //*Em 62-01-30  Pandora
                    return false;
                }
                if (odtTmp.Rows.Count > 0)
                {
                    nRowCnt = 0;    //*Em 62-01-30  Pandora
                    nCnt = 0;   //*Em 62-01-30  Pandora
                    aCardClear = new List<cmlReqCardClearList>();
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        cmlReqCardClearList oCard = new cmlReqCardClearList();
                        oCard.ptBchCode = oRcvWallet.ptBchCode;
                        oCard.ptCrdCode = oRow.Field<string>("FTCidCrdCode");
                        oCard.ptDocNoRef = oRcvWallet.ptDocNo;  //*Em 61-12-12  Pandora
                        aCardClear.Add(oCard);

                        nRowCnt = nRowCnt + 1;
                        nCnt = nCnt + 1;
                        nRowProg = nRowProg + 1;  // [Pong][2019-02-20][Set Row Progress]

                        //*Em 62-01-29  Pandora
                        //if (nCnt == 100 || nRowCnt == odtTmp.Rows.Count)
                        if (nCnt == nRecordPerRound || nRowCnt == odtTmp.Rows.Count)    //*Em 62-03-21  Pandora
                        {
                            string tJson = JsonConvert.SerializeObject(aCardClear);
                            string tURLFunc = "/Card/CardClearList";

                            cClientService oCall = new cClientService();
                            HttpResponseMessage oResponse = new HttpResponseMessage();
                            try
                            {
                                oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2FNWallet + tURLFunc, tJson);
                            }
                            catch (Exception oEx)
                            {
                                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(tJsonResult))
                                {
                                    cmlResCardClearList oResult = JsonConvert.DeserializeObject<cmlResCardClearList>(tJsonResult);
                                    if (oResult.rtCode == "1")
                                    {
                                        foreach (cmlResaoCardClearList oRes in oResult.raoCardClearList)
                                        {
                                            nRowEffect = 0;
                                            oSql = new StringBuilder();
                                            oSql.AppendLine("UPDATE TFNTCrdImpDT WITH(ROWLOCK)");
                                            oSql.AppendLine("SET FTCidStaPrc = '" + (string.Equals(oRes.rtStatus, "1") ? "1" : "2") + "' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                            oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                                            oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "')");
                                            oSql.AppendLine("AND FTCidCrdCode = '" + oRes.rtCrdCode + "' ");
                                            oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                                            if (nRowEffect == 0)
                                            {
                                                ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ptErrMsg = oResult.rtDesc;  //*Em 62-01-30  Pandora
                                        return false;
                                    }
                                }
                                else
                                {
                                    ptErrMsg = "Data result not found.";     //*Em 62-01-30  Pandora
                                    return false;
                                }
                            }
                            else
                            {
                                ptErrMsg = oResponse.StatusCode.ToString();     //*Em 62-01-30  Pandora
                                return false;
                            }
                            nCnt = 0;   //*Em 62-01-30  Pandora
                            aCardClear = new List<cmlReqCardClearList>();

                            //[Pong][2019-02-21][คำนวณ %]
                            cProg = Convert.ToDouble((nRowProg * 100) / odtTmp.Rows.Count);

                            //[Pong][2019-02-21][ส่งสถานะการทำงานความคืบหน้า % เข้า Queuse]
                            if (!C_SETbResQueuse(Convert.ToString(cProg), oRcvWallet, ref ptErrMsg, "RESCLEAR")) { return false; }

                        }
                        //++++++++++++++++++++++
                    }
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdImpDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCidStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "') AND FTCidStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdImpHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCihStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCihDocType='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
                else
                {
                    //*Em 62-01-11  Pandora
                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdImpDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCidStaPrc = '2' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "') AND FTCidStaCrd='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    //+++++++++++++++++++++++

                    nRowEffect = 0;
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TFNTCrdImpHD WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTCihStaPrcDoc = '1' ");  //สถานะประมวลผล ว่าง Null : ยังไม่ประมวลผล 1:ประมวลผลสำเร็จ 2:ประมวลผลไม่สำเร็จ
                    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(",FTLastUpdBy = '" + oRcvWallet.ptUsrCode + "'");
                    oSql.AppendLine("WHERE FTBchCode='" + oRcvWallet.ptBchCode + "' AND (FTCihDocNo='" + oRcvWallet.ptDocNo + "')");
                    oSql.AppendLine("AND FTCihDocType='2'");
                    oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEffect);
                    if (nRowEffect == 0)
                    {
                        ptErrMsg = "Can not update data.";     //*Em 62-01-30  Pandora
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), tDocNo, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oRcvWallet = null;
                odtTmp = null;
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="ptProg"></param>
       /// <param name="oPrcWallet"></param>
       /// <param name="ptErrMsg"></param>
       /// <returns></returns>
        public bool C_SETbResTopupQueuse(string ptProg, cmlRcvPrcWallet oPrcWallet, ref string ptErrMsg)
        {
            ConnectionFactory oFactory;
            oC_Config = new cConfig();
            string tMsgErr;
            try {
                if (oC_Config.C_CFGbLoadConfig(out tMsgErr))
                {
                    oFactory = new ConnectionFactory();
                    oFactory.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                    oFactory.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                    oFactory.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                    oFactory.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    using (var connection = oFactory.CreateConnection())
                    using (var oChannel = connection.CreateModel())
                    {
                        //string tJson = JsonConvert.SerializeObject(ptProg);
                        var body = Encoding.UTF8.GetBytes(ptProg);
                        oChannel.QueueDeclare("RESTOPUP_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode, true, false, false, null);
                        oChannel.BasicPublish("", "RESTOPUP_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode, false, null, body);
                        Console.WriteLine("Response Topup Queues : RESTOPUP_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode + " Progress " + ptProg + " %");

                        ptErrMsg = "";
                        return true;
                    }
                }
                else {
                    ptErrMsg = "Config RabbitMQ is null";
                    return false;
                }
             
            } catch (Exception oEx) {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), "Q_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode, "", "");  //*Em 62-01-30  Pandora
                return false;
            } finally {
                oFactory = null;
                tMsgErr = null;
            }

        }

        public bool C_SETbResQueuse(string ptProg, cmlRcvPrcWallet oPrcWallet, ref string ptErrMsg,string ptNameQueuse)
        {
            ConnectionFactory oFactory;
            oC_Config = new cConfig();
            string tMsgErr;
            try
            {
                if (oC_Config.C_CFGbLoadConfig(out tMsgErr))
                {
                    oFactory = new ConnectionFactory();
                    oFactory.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                    oFactory.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                    oFactory.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                    oFactory.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    using (var connection = oFactory.CreateConnection())
                    using (var oChannel = connection.CreateModel())
                    {
                        //string tJson = JsonConvert.SerializeObject(ptProg);
                        var body = Encoding.UTF8.GetBytes(ptProg);
                        oChannel.QueueDeclare(ptNameQueuse + "_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode, true, false, false, null);
                        oChannel.BasicPublish("", ptNameQueuse + "_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode, false, null, body);
                        Console.WriteLine("Response Queues : " + ptNameQueuse + "_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode + " Progress " + ptProg + " %");

                        ptErrMsg = "";
                        return true;
                    }
                }
                else
                {
                    ptErrMsg = "Config RabbitMQ is null";
                    return false;
                }

            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), "Q_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oFactory = null;
                tMsgErr = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPrcWallet"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_DELbQueuseRes(cmlRcvPrcWallet oPrcWallet, ref string ptErrMsg)
        {
            ConnectionFactory oFactory;
            oC_Config = new cConfig();
            string tMsgErr;
            try
            {
                if (oC_Config.C_CFGbLoadConfig(out tMsgErr))
                {
                    oFactory = new ConnectionFactory();
                    oFactory.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                    oFactory.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                    oFactory.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                    oFactory.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    using (var connection = oFactory.CreateConnection())
                    using (var oChannel = connection.CreateModel())
                    {
                        oChannel.QueueDelete("RESTOPUP_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode, false, false);
                        Console.WriteLine("Delete RESTOPUP_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode + "  Succuss");
                    }
                    ptErrMsg = "";
                    return true;
                }
                else
                {
                    ptErrMsg = "Config RabbitMQ is null";
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                new cMQReceiver().C_PRCxReponseStatus(oEx.Message.ToString(), "ResTopup_" + oPrcWallet.ptDocNo + "_" + oPrcWallet.ptUsrCode, "", "");  //*Em 62-01-30  Pandora
                return false;
            }
            finally
            {
                oFactory = null;
            }

        }
    }
}
