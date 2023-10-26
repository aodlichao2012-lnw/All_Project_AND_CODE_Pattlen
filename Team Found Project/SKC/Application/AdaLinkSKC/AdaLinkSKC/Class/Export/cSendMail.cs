using AdaLinkSKC.Model.Receive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class.Export
{
    class cSendMail
    {
        public bool C_PRCbSendMail(ref string ptErrMsg)
        {
            StringBuilder oBody;
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            string tListDocNo = "";
            bool bStaSnd = false;
            try
            {
                //log monitor
                new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Send Email Start...");

                oSql = new StringBuilder();
                oDB = new cDatabase();
                oBody = new StringBuilder();
                
                //log monitor
                new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Start Created Mail Content.");

                oSql.Clear();
                oSql.AppendLine("SELECT DISTINCT API.FDCreateOn AS dDate, HIS.FTLogTaskRef AS tDocNo, API.FNLogSndRound AS nRnd, ERR.FTErrCode AS tErrCode, ERR.FTErrDesc AS tErrMsg");
                oSql.AppendLine("FROM TLKTLogHis HIS WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TLKTLogError ERR WITH(NOLOCK) ON HIS.FTLogTaskRef = ERR.FTErrRef");
                oSql.AppendLine("LEFT JOIN TLKTLogAPI6 API WITH(NOLOCK) ON API.FTXshDocNo = ERR.FTErrRef");
                oSql.AppendLine("WHERE HIS.FTLogStaPrc = '2' AND HIS.FTLogStaSend = '2'");
                oSql.AppendLine("ORDER BY FTLogTaskRef,API.FDCreateOn");

                //log monitor
                new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Find List Error.");

                odtTmp = new DataTable();
                odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());

                //log monitor
                new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Result List Error Total " + odtTmp.Rows.Count);
                new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Cleated Content fromat Html.");

                int nRow = 0;
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {

                    oBody.AppendLine("<html>");
                    oBody.AppendLine("<style type='text/css'>");
                    oBody.AppendLine("  body{color:#0000;}");
                    oBody.AppendLine("</style>");
                    oBody.AppendLine("<body>");
                    oBody.AppendLine("  <p style='font-weight:bold;'>แจ้งเตือนรายการเอกสารที่ส่งไม่สำเร็จ..</p>");
                    oBody.AppendLine("  <table cellpadding='0' style='width:1200px;'>");
                    oBody.AppendLine("      <tr>");
                    oBody.AppendLine("          <td style='width:50px; font-weight:bold; border-bottom: 1px solid black;'>ลำดับ</td>");
                    oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>วันที่</td>");
                    oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>เวลา</td>");
                    oBody.AppendLine("          <td style='width:200px; font-weight:bold; border-bottom: 1px solid black;'>เลขที่เอกสาร</td>");
                    oBody.AppendLine("          <td style='width:80px; font-weight:bold; border-bottom: 1px solid black;'>จำนวนครั้ง</td>");
                    //oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'> Error Code</td>");
                    oBody.AppendLine("          <td style='font-weight:bold; border-bottom: 1px solid black;'>Erroe Message</td>");
                    oBody.AppendLine("      </tr>");

                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        if (nRow == 0) tListDocNo = oRow.Field<string>("tDocNo");
                        else tListDocNo += "," + oRow.Field<string>("tDocNo");
                        nRow++;
                        oBody.AppendLine("      <tr>");
                        oBody.AppendLine("          <td>" + nRow + "</td>");
                        oBody.AppendLine("          <td>" + string.Format("{0: dd/MM/yyyy}", oRow.Field<DateTime>("dDate"))+"</td>");
                        oBody.AppendLine("          <td>" + string.Format("{0: hh:mm:ss}", oRow.Field<DateTime>("dDate"))+"</td>");
                        oBody.AppendLine("          <td>" + oRow.Field<string>("tDocNo") + "</td>");
                        oBody.AppendLine("          <td>" + oRow.Field<int>("nRnd").ToString() + "</td>");
                        //oBody.AppendLine("          <td>" + oRow.Field<string>("tErrCode") + "</td>");
                        oBody.AppendLine("          <td>" + oRow.Field<string>("tErrMsg") + "</td>");
                        oBody.AppendLine("      </tr>");
                    }
                    oBody.AppendLine("  </table>");
                    oBody.AppendLine("</html>");
                    oBody.AppendLine("</body>");
                    //log monitor
                    new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "End Created Mail Content.");
                    
                    //log monitor
                    new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Start C_PRCbFuncSendMail");
                    // Send Mail
                    bStaSnd = cFunction.C_PRCbFuncSendMail(cVB.tVB_MailSnd, cVB.tVB_MailRcv, cVB.tVB_MailCC, cVB.tVB_MailBCC, cVB.tVB_MailSubj, oBody.ToString(), cVB.tVB_MailPwd, cVB.tVB_MailSMTP, cVB.nVB_MailPORT);
                    
                    //log monitor
                    new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "End C_PRCbFuncSendMail");
                    new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Status Send E-mail : " + bStaSnd);

                    if (bStaSnd == true) // Send Mail สำเร็จ
                    {
                        new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Update TLKTLogHis FTLogStaSend = 1");

                        // Update Status Send Mail Success
                        string[] aDoc = tListDocNo.Split(',');
                        if (aDoc != null && aDoc.Length > 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("UPDATE TLKTLogHis WITH(ROWLOCK) SET");
                            oSql.AppendLine("FTLogStaSend = '1',");
                            oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                            oSql.AppendLine("FTLastUpdBy = 'MQAdaLink'");
                            oSql.AppendLine("WHERE FTLogTaskRef in (");
                            foreach (string tDoc in aDoc)
                            {
                                oSql.AppendLine("'" + tDoc + "',");
                            }
                            oSql.AppendLine("'')");
                        }
                        oDB.C_SETxDataQuery(oSql.ToString());
                    }
                }
                else
                {
                    return false;
                }

                //log monitor
                new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Send Email End...");
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_PRCxLog("C_PRCbSendMail", ptErrMsg);
                return false;
            }
            finally
            {
                odtTmp = null;
                oSql = null;
                oDB = null;
                oBody = null;
                new cSP().SP_CLExMemory();
            }
        }
    }
}
