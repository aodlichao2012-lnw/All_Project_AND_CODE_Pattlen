using MQAdaLink.Model.Receive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Class.Export
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
            string tTask = "SendMail"; //*Arm 63-08-27
            try
            {
                //log monitor
                new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Send Email Start...", tTask);
                string tFmtDateTime = "yyyy-MM-dd HH:mm:ss";
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oBody = new StringBuilder();
                
                //log monitor
                new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Start Created Mail Content.", tTask);
                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Preparing data ...");

                oSql.Clear();
                //oSql.AppendLine("SELECT DISTINCT API.FDCreateOn AS dDate, CAST(CONVERT(time,API.FDCreateOn,121) AS VARCHAR(8)) AS tTime, HIS.FTLogTaskRef AS tDocNo, API.FNLogSndRound AS nRnd, ERR.FTErrCode AS tErrCode, ERR.FTErrDesc AS tErrMsg");
                //oSql.AppendLine("FROM TLKTLogHis HIS WITH(NOLOCK)");
                //oSql.AppendLine("LEFT JOIN TLKTLogError ERR WITH(NOLOCK) ON HIS.FTLogTaskRef = ERR.FTErrRef");
                //oSql.AppendLine("LEFT JOIN TLKTLogAPI6 API WITH(NOLOCK) ON API.FTXshDocNo = ERR.FTErrRef");
                //oSql.AppendLine("WHERE HIS.FTLogStaPrc = '2' AND HIS.FTLogStaSend = '2' AND HIS.FTLogType ='2'");
                //oSql.AppendLine("ORDER BY FTLogTaskRef,API.FDCreateOn");

                //*Arm 63-08-20
                oSql.AppendLine("SELECT (SELECT TOP 1 FDErrDate FROM TLKTLogError WITH(NOLOCK) WHERE FTErrRef = ERR.FTErrRef ORDER BY FDErrDate DESC) AS dDate, ");
                oSql.AppendLine("CAST(CONVERT(time, (SELECT TOP 1 FDErrDate FROM TLKTLogError WITH(NOLOCK) WHERE FTErrRef = ERR.FTErrRef ORDER BY FDErrDate DESC), 121) AS VARCHAR(8)) AS tTime, ");
                oSql.AppendLine("ERR.FTErrRef AS tDocNo, ");
                oSql.AppendLine("(SELECT COUNT(FTErrRef)FROM TLKTLogError WITH(NOLOCK) WHERE FTErrRef = ERR.FTErrRef ) AS nRnd, ");
                oSql.AppendLine("'' AS tErrCode, ");
                oSql.AppendLine("(SELECT TOP 1 FTErrDesc FROM TLKTLogError WITH(NOLOCK) WHERE FTErrRef = ERR.FTErrRef ORDER BY FDErrDate) AS tErrMsg ");
                oSql.AppendLine("FROM( SELECT FTErrRef FROM TLKTLogError WITH(NOLOCK) ");
                oSql.AppendLine("    GROUP BY FTErrRef ");
                oSql.AppendLine(")ERR ");
                oSql.AppendLine("INNER JOIN TLKTLogHis HIS WITH(NOLOCK) ON ERR.FTErrRef = HIS.FTLogTaskRef ");
                oSql.AppendLine("WHERE HIS.FTLogStaPrc = '2' AND HIS.FTLogStaSend = '2'  AND HIS.FTLogType = '2' ");
                oSql.AppendLine("ORDER BY dDate"); //*Arm 63-08-20
                //+++++++++++++

                new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Find Data/SQL : " + oSql.ToString(), tTask);
                //log monitor
                new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Find Data.", tTask);

                odtTmp = new DataTable();
                odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());

                //log monitor
                new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Result List Data Total " + odtTmp.Rows.Count, tTask);
                new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Cleated Content fromat Html.", tTask);

                int nRow = 0;
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {

                    oBody.AppendLine("<html>");
                    oBody.AppendLine("<style type='text/css'>");
                    oBody.AppendLine("  body{color:#0000;}");
                    oBody.AppendLine("</style>");
                    oBody.AppendLine("<body>");
                    oBody.AppendLine("  <p style='font-weight:bold;'>แจ้งเตือนรายการเอกสารที่ส่งไม่สำเร็จ...</p>");
                    oBody.AppendLine("  <table cellpadding='0' style='width:1200px;'>");
                    oBody.AppendLine("      <tr>");
                    oBody.AppendLine("          <td style='width:50px; font-weight:bold; border-bottom: 1px solid black;'>ลำดับ</td>");
                    oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>วันที่</td>");
                    oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>เวลา</td>");
                    oBody.AppendLine("          <td style='width:200px; font-weight:bold; border-bottom: 1px solid black;'>เลขที่เอกสาร</td>");
                    oBody.AppendLine("          <td style='width:80px; font-weight:bold; border-bottom: 1px solid black;'>รอบที่ส่ง</td>");
                    oBody.AppendLine("          <td style='font-weight:bold; border-bottom: 1px solid black;'>Error Message</td>");
                    oBody.AppendLine("      </tr>");

                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        if (nRow == 0) tListDocNo = oRow.Field<string>("tDocNo");
                        else tListDocNo += "," + oRow.Field<string>("tDocNo");
                        nRow++;
                        oBody.AppendLine("      <tr>");
                        oBody.AppendLine("          <td>" + nRow + "</td>");
                        if (oRow.Field<DateTime>("dDate") != null)
                        {
                            oBody.AppendLine("          <td>" + string.Format("{0: dd/MM/yyyy}", oRow.Field<DateTime>("dDate")) + "</td>");
                        }
                        oBody.AppendLine("          <td>" + oRow.Field<string>("tTime") + "</td>");
                        oBody.AppendLine("          <td>" + oRow.Field<string>("tDocNo") + "</td>");
                        oBody.AppendLine("          <td>" + oRow.Field<int>("nRnd").ToString() + "</td>");
                        oBody.AppendLine("          <td>" + oRow.Field<string>("tErrMsg") + "</td>");
                        oBody.AppendLine("      </tr>");
                    }
                    oBody.AppendLine("  </table>");
                    oBody.AppendLine("</html>");
                    oBody.AppendLine("</body>");
                    new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : End Created Mail Content.", tTask);
                    
                    new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Start C_PRCbFuncSendMail", tTask);
                    Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Sending mail...");
                    // Send Mail
                    bStaSnd = cFunction.C_PRCbFuncSendMail(cVB.tVB_MailSnd, cVB.tVB_MailRcv, cVB.tVB_MailCC, cVB.tVB_MailBCC, "(ไม่สำเร็จ) "+ cVB.tVB_MailSubj, oBody.ToString(), cVB.tVB_MailPwd, cVB.tVB_MailSMTP, cVB.nVB_MailPORT);
                    
                    new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : End C_PRCbFuncSendMail", tTask);
                    new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Status Send E-mail : " + bStaSnd, tTask);
                    Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Send mail " + bStaSnd);

                    if (bStaSnd == true) // Send Mail สำเร็จ
                    {
                        new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Update TLKTLogHis FTLogStaSend = 1", tTask);

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
                    new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : No Data Error for Send Mail...", tTask);
                    Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " No data for send Mail..." );
                }
                
                new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Send Email End...", tTask);
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_PRCxLog("cSendMail", "C_PRCbSendMail : Error/"+ ptErrMsg);
                new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCbSendMail : Error/" + ptErrMsg, tTask); //*Arm 63-08-27
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

        //AdaLink
        public void C_PRCxSendMail(string nLogId,string tType)
        {
            StringBuilder oBody;
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            bool bStaSnd = false;
            string tSubject = "";
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oBody = new StringBuilder();

                oSql.Clear();
                oSql.AppendLine("SELECT FDLogCreate,CASE WHEN FTLogType = 1 THEN 'นำเข้าข้อมูล' ELSE '' END As FTLogType,FTLogTaskRef, ");
                oSql.AppendLine("FTLogTask,FNLogQtyAll,FNLogQtyDone,FNLogQtyAll - FNLogQtyDone As FNLogError,CASE WHEN FTLogStaPrc = 1 THEN 'สำเร็จ' WHEN FTLogStaPrc = 3 THEN 'ไฟล์ซ้ำ' ELSE 'ล้มเหลว' END As FTLogStaPrc, ");
                oSql.AppendLine("CASE WHEN FTLogStaPrc = 1 THEN 'สำเร็จ' ELSE 'ไม่สำเร็จ' END As MailSubject");
                oSql.AppendLine("FROM TLKTLogHis HIS WITH(NOLOCK)");
                oSql.AppendLine("WHERE HIS.FTLogStaSend = '2' AND HIS.FTLogTaskRef = '" + nLogId+"'");
                
                odtTmp = new DataTable();
                odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());

                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    oBody.AppendLine("<html>");
                    oBody.AppendLine("<style type='text/css'>");
                    oBody.AppendLine("  body{color:#0000;}");
                    oBody.AppendLine("</style>");
                    oBody.AppendLine("<body>");
                    oBody.AppendLine("  <p style='font-weight:bold;'>รายการนำเข้า</p>");
                    oBody.AppendLine("  <table cellpadding='0' style='width:1200px;'>");
                    oBody.AppendLine("      <tr>");
                    //oBody.AppendLine("          <td style='width:50px; font-weight:bold; border-bottom: 1px solid black;'>ลำดับ</td>");
                    oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>วันที่</td>");
                    oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>เวลา</td>");
                    //oBody.AppendLine("          <td style='width:150px; font-weight:bold; border-bottom: 1px solid black;'>ประเภท</td>");
                    oBody.AppendLine("          <td style='width:250px; font-weight:bold; border-bottom: 1px solid black;'>รายการ</td>");
                    oBody.AppendLine("          <td style='width:150px; font-weight:bold; border-bottom: 1px solid black;'>จำนวนรายการ</td>");
                    oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>สำเร็จ</td>");
                    oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>ไม่สำเร็จ</td>");
                    //oBody.AppendLine("          <td style='width:100px; font-weight:bold; border-bottom: 1px solid black;'>สถานะ</td>");
                    //oBody.AppendLine("          <td style='font-weight:bold; border-bottom: 1px solid black;'>Erroe Message</td>");
                    oBody.AppendLine("      </tr>");

                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        //if (nRow == 0) tListDocNo = oRow.Field<string>("tDocNo");
                        //else tListDocNo += "," + oRow.Field<string>("tDocNo");
                        //nRow++;
                        oBody.AppendLine("      <tr>");
                        oBody.AppendLine("          <td>" + string.Format("{0: dd/MM/yyyy}", oRow.Field<DateTime>("FDLogCreate")) + "</td>");
                        oBody.AppendLine("          <td>" + string.Format("{0: HH:mm:ss}", oRow.Field<DateTime>("FDLogCreate")) + "</td>");
                        //oBody.AppendLine("          <td>" + oRow.Field<string>("FTLogTask") + "</td>");
                        if (tType == "1")
                        {
                            oBody.AppendLine("          <td> ข้อมูลสินค้า </td>");
                        }
                        else if(tType == "2")
                        {
                            oBody.AppendLine("          <td> ข้อมูลใบปรับราคา </td>");
                        }
                        else
                        {
                            oBody.AppendLine("          <td> ข้อมูลพนักงาน </td>");
                        }
                        
                        oBody.AppendLine("          <td>" + Convert.ToDecimal(oRow.Field<int>("FNLogQtyAll").ToString()).ToString("#,##0") + "</td>");
                        oBody.AppendLine("          <td>" + Convert.ToDecimal(oRow.Field<int>("FNLogQtyDone").ToString()).ToString("#,##0") + "</td>");
                        oBody.AppendLine("          <td>" + Convert.ToDecimal(oRow.Field<int>("FNLogError").ToString()).ToString("#,##0") + "</td>");
                        //oBody.AppendLine("          <td>" + oRow.Field<string>("FTLogStaPrc") + "</td>");
                        oBody.AppendLine("      </tr>");
                       
                    }
                    
                    oBody.AppendLine("  </table>");
                    oBody.AppendLine("  <br/>");
                    oBody.AppendLine("  <p style='font-weight:bold;'>====== ส่งจากระบบ AdaLink ======</p>");
                    oBody.AppendLine("</html>");
                    oBody.AppendLine("</body>");
                    tSubject = "(" + odtTmp.Rows[0]["MailSubject"].ToString() + ") " + cVB.tVB_MailSubj;
                    new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "End Created Mail Content.");

                    new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Start C_PRCxSendMail");
                    // Send Mail
                    bStaSnd = cFunction.C_PRCbFuncSendMail(cVB.tVB_MailSnd, cVB.tVB_MailRcv, cVB.tVB_MailCC, cVB.tVB_MailBCC, tSubject, oBody.ToString(), cVB.tVB_MailPwd, cVB.tVB_MailSMTP, cVB.nVB_MailPORT);

                    new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "End C_PRCbFuncSendMail");

                    new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Status Send E-mail : " + bStaSnd);

                    if (bStaSnd == true) // Send Mail สำเร็จ
                    {
                        new cLog().C_PRCxLogMonitor("C_PRCbSendMail", "Update TLKTLogHis FTLogStaSend = 1");

                        // Update Status Send Mail Success
                       
                        oSql.Clear();
                        oSql.AppendLine("UPDATE TLKTLogHis WITH(ROWLOCK) SET");
                        oSql.AppendLine("FTLogStaSend = '1',");
                        oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                        oSql.AppendLine("FTLastUpdBy = 'AdaLink'");
                        oSql.AppendLine("WHERE FTLogTaskRef = '" + nLogId + "'");
                        
                        oDB.C_SETxDataQuery(oSql.ToString());
                    }
                }
                else
                {

                }

            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cSendMail","C_PRCxSendMail : Error/" + oEx.Message); //*Arm 63-08-27
                new cLog().C_PRCxLogMonitor("cSendMail", "C_PRCxSendMail : Error/" + oEx.Message); //*Arm 63-08-27
            }
        }
    }
}
