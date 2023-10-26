using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.Shift
{
    public partial class wProgressUldCloseShif : Form
    {
        private ResourceManager oW_Resource;
        private bool bW_ChkConn;

        Thread oW_ThreadSal, oW_ThreadShf, oW_ThreadVoid;
        public wProgressUldCloseShif()
        {
            InitializeComponent();
            try
            {
                W_SETxDesign();
                W_SETxText();
                oW_ThreadSal = new Thread(() => { });
                oW_ThreadShf = new Thread(() => { });
                oW_ThreadVoid = new Thread(() => { });
                //otmTimer.Start(); //*Net 63-06-05 Show แล้วค่อย Start
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif", "wProgressUldCloseShif : " + oEx.Message);
            }
        }

        public void W_PRCxUpload()
        {
            StringBuilder oSql;
            cDatabase oDB;
            //*Net 63-06-05 ย้ายไปใส่ใน Fn
            //List<cmlTCNTTmpLogChg> aoLogChgShf;
            //List<cmlTCNTTmpLogChg> aoLogChgSal;
            //List<cmlTCNTTmpLogChg> aoLogChgVod;
            List<cmlTSysSyncData> aoUpload;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                //*Net 63-06-05 ย้ายไปใส่ใน Fn
                //aoLogChgShf = new List<cmlTCNTTmpLogChg>();
                //aoLogChgSal = new List<cmlTCNTTmpLogChg>();
                //aoLogChgVod = new List<cmlTCNTTmpLogChg>();
                aoUpload = new cSyncData().C_GETaTableSyncDB();
                aoUpload = new List<cmlTSysSyncData>(aoUpload.Where(c => c.FTSynTable_L == "API2PSSale").ToList());

                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : Checking Connect API2PSSale...");
                if (new cSP().SP_CHKbConnection(cVB.tVB_API2PSSale + "/CheckOnline/IsOnline"))
                {
                    olaErrNotConn.ForeColor = Color.Green;
                    olaErrNotConn.Text = oW_Resource.GetString("tMsgUldConnSucc");
                    bW_ChkConn = true;
                    new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : API2PSSale Connected...");
                    foreach (cmlTSysSyncData oSync in aoUpload)
                    {
                        switch (oSync.FNSynSeqNo)
                        {
                            case 80:    //Sale-Return
                                //*Net 63-06-05 ย้ายไปเป็น Fn
                                #region Comment
                                //new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : ตรวจสอบเอกสารการขาย-คืนทั้งหมดของรอบการขาย " + cVB.tVB_ShfCode + "ที่ยังไม่มีใน TCNTTmpLogChg");
                                ////Check เอกที่ยังไม่มีการส่ง
                                //oSql.Clear();
                                //oSql.AppendLine("SELECT DISTINCT HD.FTShfCode,TmpLog.FTLogDocNo,HD.FTXshDocNo From TPSTSalHD HD with(nolock)");
                                //oSql.AppendLine("LEFT JOIN TCNTTmpLogChg TmpLog with(nolock) ON HD.FTXshDocNo = TmpLog.FTLogDocNo AND HD.FTShfCode = TmpLog.FTLogCode");
                                //oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "' AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' AND TmpLog.FTLogDocNo IS NULL");
                                //DataTable oDTbl = new DataTable();
                                //oDTbl = oDB.C_GEToDataQuery(oSql.ToString());
                                //if (oDTbl != null && oDTbl.Rows.Count > 0)
                                //{
                                //    new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : บันทึกเอกสารที่ยังไม่มีใน TCNTTmpLogChg ลง TCNTTmpLogChg");
                                //    foreach (DataRow oRow in oDTbl.Rows)
                                //    {
                                //        cSale.C_PRCxAdd2TmpLogChg(80, oRow.Field<string>("FTXshDocNo"), true);
                                //    }
                                //}
                                //else
                                //{
                                //    new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : เอกสารการขาย-คืนทั้งหมดของรอบการขาย " + cVB.tVB_ShfCode + "บันทึกลง TCNTTmpLogChg แล้ว");
                                //}
                                //oDTbl = null;

                                ////Sale
                                //new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : Check Documents (Sale-retrun) that have not been uploaded..");

                                //oSql.Clear();
                                //oSql.AppendLine("SELECT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc, FDCreateOn, FDLastUpdOn FROM TCNTTmpLogChg with(nolock)");
                                //oSql.AppendLine("WHERE FNLogType ='80' AND ISNULL(FTLogStaPrc,'') != '2' AND FTLogCode = '" + cVB.tVB_ShfCode + "' ");
                                //aoLogChgSal = oDB.C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());

                                //new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : Start Uploaded documents (Sale-retrun)..");
                                //W_SYNxUpload(aoLogChgSal, oSync, 80, olaSalTbl, olaStaSal, opgStaSal);
                                //new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : End Uploaded documents (Sale-retrun)..");
                                //aoLogChgSal = null;
                                #endregion
                                oW_ThreadSal = new Thread(() => W_PRCxSale(oSync))
                                {
                                    Priority = ThreadPriority.Highest,
                                    IsBackground = true
                                };
                                break;
                            case 81:    //Shift
                                //*Net 63-06-05 ย้ายไปเป็น Fn
                                #region Comment
                                //oSql.Clear();
                                //oSql.AppendLine("SELECT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc, FDCreateOn, FDLastUpdOn FROM TCNTTmpLogChg with(nolock)");
                                //oSql.AppendLine("WHERE FNLogType ='81' AND ISNULL(FTLogStaPrc,'') != '2' AND FTLogDocNo = '" + cVB.tVB_PosCode + "' ");
                                //aoLogChgShf = oDB.C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());

                                //new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : Start Uploaded documents (Shift)..");
                                //W_SYNxUpload(aoLogChgShf, oSync, 81, olaShfTbl, olaStaShf, opgStaShf);
                                //new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : End Uploaded documents (Shift)..");
                                //aoLogChgShf = null;
                                #endregion
                                oW_ThreadShf = new Thread(() => W_PRCxShift(oSync))
                                {
                                    Priority = ThreadPriority.Highest,
                                    IsBackground = true
                                };
                                break;

                            case 82:    //Void
                                //*Net 63-06-05 ย้ายไปเป็น Fn
                                #region Comment
                                //oSql.Clear();
                                //oSql.AppendLine("SELECT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc, FDCreateOn, FDLastUpdOn FROM TCNTTmpLogChg with(nolock)");
                                //oSql.AppendLine("WHERE FNLogType ='82' AND ISNULL(FTLogStaPrc,'') != '2' ");
                                //aoLogChgVod = oDB.C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());

                                //new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : Start Uploaded documents (VoidDT)..");
                                //W_SYNxUpload(aoLogChgVod, oSync, 82, olaVodTbl, olaStaVod, opgStaVod);
                                //new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : End Uploaded documents (End)..");
                                //aoLogChgVod = null;
                                #endregion
                                oW_ThreadVoid = new Thread(() => W_PRCxVoid(oSync))
                                {
                                    Priority = ThreadPriority.Highest,
                                    IsBackground = true
                                };
                                break;
                            case 90:

                                break;
                        }
                    }
                    oW_ThreadSal.Start();
                    oW_ThreadShf.Start();
                    oW_ThreadVoid.Start();
                }
                else
                {
                    olaErrNotConn.ForeColor = Color.Red;
                    olaErrNotConn.Text = oW_Resource.GetString("tMsgUldConnError");
                    bW_ChkConn = false;
                    new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : API2PSSale Connect false...");
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_SYNxUpload(List<cmlTCNTTmpLogChg> paoTmpLogChg, cmlTSysSyncData poSync, int pnType, Label polaStaTbl, Label polaStatus, ProgressBar popgStatus)
        {
            string tDocNo = "";
            string tType = "";
            StringBuilder oSql;
            cDatabase oDB;
            cSyncData oSync;
            string tResult = "";
            try
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload Start ...");

                popgStatus.Invoke((MethodInvoker)delegate
                {
                    popgStatus.Step = 0;
                    popgStatus.Maximum = paoTmpLogChg.Count;
                    popgStatus.Minimum = 0;
                });
                int nCount = 0;
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSync = new cSyncData();

                if (bW_ChkConn == true)
                {
                    if (paoTmpLogChg.Count == 0)
                    {
                        polaStaTbl.Invoke((MethodInvoker)delegate
                        {
                            switch (pnType)
                            {
                                case 80:
                                    polaStaTbl.Text = oW_Resource.GetString("tStaUploadSale") + " " + oW_Resource.GetString("tUploadSuccess");
                                    break;
                                case 81:
                                    polaStaTbl.Text = oW_Resource.GetString("tStaUploadShift") + " " + oW_Resource.GetString("tUploadSuccess");
                                    break;
                                case 82:
                                    polaStaTbl.Text = oW_Resource.GetString("tStaUploadVoid") + " " + oW_Resource.GetString("tUploadSuccess");
                                    break;
                            }
                            polaStaTbl.Update();

                        });
                        popgStatus.Invoke((MethodInvoker)delegate
                        {
                            popgStatus.Maximum = 100;
                            popgStatus.Value = 100;
                            popgStatus.PerformStep();
                            popgStatus.Update();
                            //this.Refresh();
                        });
                    }
                    else
                    {
                        foreach (cmlTCNTTmpLogChg oRw in paoTmpLogChg)
                        {
                            nCount += 1;
                            polaStatus.Invoke((MethodInvoker)delegate
                            {
                                //polaStatus.Text = string.Format("({0}/{1})", nCount, popgStatus.Maximum);
                                polaStatus.Text = string.Format("({0}/{1})", nCount, popgStatus.Maximum + 1); //*Net 63-06-05 บวก Progress ไป 1 เพื่อ Upload
                                polaStatus.Update();
                            });

                            switch (pnType)
                            {
                                case 80:
                                    tType = oW_Resource.GetString("tStaUploadSale");
                                    tDocNo = oRw.FTLogDocNo;

                                    oSql.Clear();
                                    oSql.AppendLine("UPDATE TCNTTmpLogChg with(rowlock)");
                                    oSql.AppendLine("SET FTLogStaPrc = '1'");
                                    oSql.AppendLine("WHERE FNLogType = 80 AND ISNULL(FTLogStaPrc,'') = '' AND FTLogDocNo = '" + tDocNo + "' ");
                                    oDB.C_SETxDataQuery(oSql.ToString());

                                    if (oSync.C_SYNbSale(poSync))
                                    {
                                        tResult = oW_Resource.GetString("tUploadSuccess");
                                    }
                                    else
                                    {
                                        tResult = oW_Resource.GetString("tUnSuccess");
                                    }
                                    break;

                                case 81:
                                    tType = oW_Resource.GetString("tStaUploadShift");
                                    tDocNo = oRw.FTLogCode;
                                    oSql.Clear();
                                    oSql.AppendLine("UPDATE TCNTTmpLogChg with(rowlock)");
                                    oSql.AppendLine("SET FTLogStaPrc = '1'");
                                    oSql.AppendLine("WHERE FNLogType = 81 AND ISNULL(FTLogStaPrc,'') = '' AND FTLogDocNo = '" + cVB.tVB_PosCode + "' ");
                                    oSql.AppendLine("AND FTLogCode='" + tDocNo + "'");
                                    oDB.C_SETxDataQuery(oSql.ToString());

                                    if (oSync.C_SYNbShift(poSync))
                                    {
                                        tResult = oW_Resource.GetString("tUploadSuccess");
                                    }
                                    else
                                    {
                                        tResult = oW_Resource.GetString("tUnSuccess");
                                    }
                                    break;

                                case 82:
                                    tType = oW_Resource.GetString("tStaUploadVoid");
                                    oSql.Clear();
                                    oSql.AppendLine("UPDATE TOP(1) TCNTTmpLogChg with(rowlock)");
                                    oSql.AppendLine("SET FTLogStaPrc = '1'");
                                    oSql.AppendLine("WHERE FNLogType = 82 AND ISNULL(FTLogStaPrc,'') = '' ");
                                    oDB.C_SETxDataQuery(oSql.ToString());

                                    if (oSync.C_SYNbVoid(poSync))
                                    {
                                        tResult = oW_Resource.GetString("tUploadSuccess");
                                    }
                                    else
                                    {
                                        tResult = oW_Resource.GetString("tUnSuccess");
                                    }
                                    break;

                            }
                            polaStaTbl.Invoke((MethodInvoker)delegate
                            {
                                polaStaTbl.Text = tType + " " + string.Format("{0} {1}", tDocNo, tResult);
                                polaStaTbl.Update();
                            });
                            popgStatus.Invoke((MethodInvoker)delegate
                            {
                                popgStatus.Value = nCount;
                                popgStatus.PerformStep();
                                popgStatus.Update();
                            });
                            //this.Refresh();
                            if (pnType != 82)
                            {
                                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload : " + tType + " " + string.Format("{0} {1}", tDocNo, tResult));
                            }
                        }

                        if (popgStatus.Value != popgStatus.Maximum)
                        {
                            polaStatus.Invoke((MethodInvoker)delegate
                            {
                                polaStatus.Text = string.Format("({0}/{1})", popgStatus.Maximum + 1, popgStatus.Maximum + 1);
                                polaStatus.Update();
                            });
                            popgStatus.Invoke((MethodInvoker)delegate
                            {
                                popgStatus.Value = popgStatus.Maximum;
                                popgStatus.PerformStep();
                                popgStatus.Update();
                            });
                        }
                        new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxUpload Success...");
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif:", "W_SYNxUpload : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                //paoTmpLogChg = null;
                //poSync = null;
                //popgStatus = null;
                //polaStaTbl = null;
                //polaStatus = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }

        }

        private void W_PRCxSale(cmlTSysSyncData poSync)
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlTCNTTmpLogChg> aoLogChgSal;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                aoLogChgSal = new List<cmlTCNTTmpLogChg>();
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxSale : ตรวจสอบเอกสารการขาย-คืนทั้งหมดของรอบการขาย " + cVB.tVB_ShfCode + "ที่ยังไม่มีใน TCNTTmpLogChg");
                //Check เอกที่ยังไม่มีการส่ง
                oSql.Clear();
                oSql.AppendLine("SELECT DISTINCT HD.FTShfCode,TmpLog.FTLogDocNo,HD.FTXshDocNo From TPSTSalHD HD with(nolock)");
                oSql.AppendLine("LEFT JOIN TCNTTmpLogChg TmpLog with(nolock) ON HD.FTXshDocNo = TmpLog.FTLogDocNo AND HD.FTShfCode = TmpLog.FTLogCode");
                oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "' AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' AND TmpLog.FTLogDocNo IS NULL");
                DataTable oDTbl = new DataTable();
                oDTbl = oDB.C_GEToDataQuery(oSql.ToString());
                if (oDTbl != null && oDTbl.Rows.Count > 0)
                {
                    new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxSale : บันทึกเอกสารที่ยังไม่มีใน TCNTTmpLogChg ลง TCNTTmpLogChg");
                    foreach (DataRow oRow in oDTbl.Rows)
                    {
                        cSale.C_PRCxAdd2TmpLogChg(80, oRow.Field<string>("FTXshDocNo"), true);
                    }
                }
                else
                {
                    new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxSale : เอกสารการขาย-คืนทั้งหมดของรอบการขาย " + cVB.tVB_ShfCode + "บันทึกลง TCNTTmpLogChg แล้ว");
                }
                oDTbl = null;

                //Sale
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxSale : Check Documents (Sale-retrun) that have not been uploaded..");

                oSql.Clear();
                //*Net 63-06-05 เอาตัวซ้ำออก
                //oSql.AppendLine("SELECT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc, FDCreateOn, FDLastUpdOn FROM TCNTTmpLogChg with(nolock)");
                oSql.AppendLine("SELECT DISTINCT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc FROM TCNTTmpLogChg with(nolock)");
                //oSql.AppendLine("WHERE FNLogType ='80' AND ISNULL(FTLogStaPrc,'') != '2' AND FTLogCode = '" + cVB.tVB_ShfCode + "' ");
                oSql.AppendLine("WHERE FNLogType ='80' AND ISNULL(FTLogStaPrc,'') != '2'  "); //*Net 63-06-05 เอารายการทั้งหมดที่ไม่ได้อัพโหลด
                aoLogChgSal = oDB.C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());

                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxSale : Start Uploaded documents (Sale-retrun)..");
                this.Invoke((MethodInvoker)delegate
                {
                    W_SYNxUpload(aoLogChgSal, poSync, 80, olaSalTbl, olaStaSal, opgStaSal);
                });
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxSale : End Uploaded documents (Sale-retrun)..");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxSale : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_PRCxVoid(cmlTSysSyncData poSync)
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlTCNTTmpLogChg> aoLogChgVod;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                aoLogChgVod = new List<cmlTCNTTmpLogChg>();
                oSql.Clear();
                //*Net 63-06-05 เอาตัวซ้ำออก
                //oSql.AppendLine("SELECT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc, FDCreateOn, FDLastUpdOn FROM TCNTTmpLogChg with(nolock)");
                oSql.AppendLine("SELECT DISTINCT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc FROM TCNTTmpLogChg with(nolock)");
                oSql.AppendLine("WHERE FNLogType ='82' AND ISNULL(FTLogStaPrc,'') != '2' ");
                aoLogChgVod = oDB.C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());

                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxVoid : Start Uploaded documents (VoidDT)..");
                this.Invoke((MethodInvoker)delegate
                {
                    W_SYNxUpload(aoLogChgVod, poSync, 82, olaVodTbl, olaStaVod, opgStaVod);
                });
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxVoid : End Uploaded documents (End)..");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxVoid : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_PRCxShift(cmlTSysSyncData poSync)
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlTCNTTmpLogChg> aoLogChgShf;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                aoLogChgShf = new List<cmlTCNTTmpLogChg>();
                oSql.Clear();
                //*Net 63-06-05 เอาตัวซ้ำออก
                //oSql.AppendLine("SELECT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc, FDCreateOn, FDLastUpdOn FROM TCNTTmpLogChg with(nolock)");
                oSql.AppendLine("SELECT DISTINCT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc FROM TCNTTmpLogChg with(nolock)");
                oSql.AppendLine("WHERE FNLogType ='81' AND ISNULL(FTLogStaPrc,'') != '2' AND FTLogDocNo = '" + cVB.tVB_PosCode + "' ");
                aoLogChgShf = oDB.C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());

                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxShift : Start Uploaded documents (Shift)..");
                this.Invoke((MethodInvoker)delegate
                {
                    W_SYNxUpload(aoLogChgShf, poSync, 81, olaShfTbl, olaStaShf, opgStaShf);
                });
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxShift : End Uploaded documents (Shift)..");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif", "W_PRCxShift : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmReSync.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif:", "W_SETxDesign : " + oEx.Message);
            }

        }
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resSync_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSync_EN));
                        break;
                }
                olaTitle.Text = oW_Resource.GetString("tUpload");
                olaSalTbl.Text = oW_Resource.GetString("tStaUploadSale");
                olaShfTbl.Text = oW_Resource.GetString("tStaUploadShift");
                olaVodTbl.Text = oW_Resource.GetString("tStaUploadVoid");
                olaErrNotConn.Text = "";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProgressUldCloseShif", "W_SETxText : " + oEx.Message); }

        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {

                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif", "ocmAccept_Click : " + oEx.Message);
            }
        }


        private void otmTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                otmTimer.Stop();

                W_PRCxUpload();

                otmClose.Start();

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgressUldCloseShif", "otmTimer_Tick : " + oEx.Message);
            }

        }

        private void otmClose_Tick(object sender, EventArgs e)
        {
            ocmAccept_Click(ocmAccept, null);
        }

        private void wProgressUldCloseShif_Shown(object sender, EventArgs e)
        {
            otmTimer.Start();
        }
    }
}
