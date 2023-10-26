using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Popup.All;
using AdaPos.Properties;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.Shift
{
    public partial class wWaitUplCloseShf : Form
    {
        private List<cmlTSysSyncData> aoW_SyncShf;
        private List<cmlTSysSyncData> aoW_SyncVoid;
        private List<cmlTSysSyncData> aoW_SyncSale;

        private ResourceManager oW_Resource;
        private cSyncData oW_Sync;

        private int nW_FixShf;
        public wWaitUplCloseShf()
        {
            InitializeComponent();
            try
            {
                W_SETxDesign();
                W_SETxText();
                opnUpdSaleContent.Visible = false;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void wWaitUplCloseShf_Shown(object sender, EventArgs e)
        {
            otmStart.Start();
        }

        private async void otmStart_Tick(object sender, EventArgs e)
        {
            otmStart.Stop();

            if (W_CHKbConnection())
            {
                oW_Sync = new cSyncData();
                W_GENbTblSync();
                await W_PRCxUploadAsync();
            }
            await Task.Delay(2000);
            this.DialogResult = DialogResult.OK;
        }



        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
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
                olaTitle.Text = oW_Resource.GetString("tUploadCloseShf");
                olaTitleMsg.Text = oW_Resource.GetString("tMsgUldConnSucc");

                olaShift.Text = oW_Resource.GetString("tStaUploadShift");
                olaVoid.Text = oW_Resource.GetString("tStaUploadVoid");
                olaSendFixDoc.Text = oW_Resource.GetString("tStaUploadFixDoc");
                olaGetFixDoc.Text = oW_Resource.GetString("tStaGetFixDoc");


                olaTitleMsg.ForeColor = Color.Black;
                olaTitleMsg.Text = oW_Resource.GetString("tTitleAutoSync");

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProgressUldCloseShif", "W_SETxText : " + oEx.Message); }

        }
        private string W_GETtTitleFixDoc(int nQty)
        {
            return oW_Resource.GetString("tTitleItemFixDoc") +
                   (nQty).ToString().PadLeft(10) +
                   " " + cVB.oVB_GBResource.GetString("tDoc");
        }
        private string W_GETtMsgFixDoc(int nCount, int nMax, string tText)
        {
            return nCount.ToString("N0") + "/" +
                   nMax.ToString("N0") + " : " + tText;
        }

        private bool W_CHKbConnection()
        {
            try
            {
                if (new cSP().SP_CHKbConnection(cVB.tVB_API2PSSale + "/CheckOnline/IsOnline"))
                {
                    olaTitleMsg.ForeColor = Color.Green;
                    olaTitleMsg.Text = oW_Resource.GetString("tMsgUldConnSucc");
                    return true;
                }
                else
                {
                    olaTitleMsg.ForeColor = Color.Red;
                    olaTitleMsg.Text = oW_Resource.GetString("tMsgUldConnError");
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {

            }
            return false;
        }
        private bool W_GENbTblSync()
        {
            List<cmlTSysSyncData> aoTblUpload;
            try
            {
                aoW_SyncSale = new List<cmlTSysSyncData>();
                aoW_SyncShf = new List<cmlTSysSyncData>();
                aoW_SyncVoid = new List<cmlTSysSyncData>();

                aoTblUpload = new cSyncData().C_GETaTableSyncDB();
                aoTblUpload = new List<cmlTSysSyncData>(aoTblUpload.Where(c => c.FTSynTable_L == "API2PSSale").ToList());
                foreach (cmlTSysSyncData oSync in aoTblUpload)
                {
                    switch (oSync.FNSynSeqNo)
                    {
                        case 80: // Sale
                            aoW_SyncSale.Add(oSync);
                            break;
                        case 81: // Shift
                            aoW_SyncShf.Add(oSync);
                            break;
                        case 82: // Void
                            aoW_SyncVoid.Add(oSync);
                            break;
                    }
                }

                aoTblUpload.Clear();
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                aoTblUpload = null;
            }
            return false;
        }
        private List<cmlTCNTTmpLogChg> W_GEToDocTmpLogChg(int pnLogType, string ptDocNo)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine("SELECT DISTINCT FTLogCode, FTLogDocNo,FNLogType, FTWahCode, FTLogStaPrc FROM TCNTTmpLogChg with(nolock)");
                oSql.AppendLine($"WHERE FNLogType ='{pnLogType}' AND ISNULL(FTLogStaPrc,'') != '2'  ");
                if (!String.IsNullOrEmpty(ptDocNo))
                {
                    oSql.AppendLine($"AND FTLogDocNo = '{ptDocNo}'");
                }

                return oDB.C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return new List<cmlTCNTTmpLogChg>();
        }
        private List<cmlTCNTTmpLogChg> W_GEToSaleDocFrmTmpLogChg(bool pbTrans)
        {
            StringBuilder oSql;
            cDatabase oDB;
            string tTblSalHD;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                if (pbTrans) tTblSalHD = "TPSTSalHD";
                else tTblSalHD = "TSHD" + cVB.tVB_PosCode;

                oSql.AppendLine($"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tTblSalHD}')) BEGIN");
                oSql.AppendLine("SELECT DISTINCT FTLogCode, FTLogDocNo,FNLogType, HD.FTWahCode, FTLogStaPrc FROM TCNTTmpLogChg TMP with(nolock)");
                oSql.AppendLine($"INNER JOIN {tTblSalHD} HD ON HD.FTBchCode='{cVB.tVB_BchCode}' AND HD.FTPosCode='{cVB.tVB_PosCode}'");
                oSql.AppendLine($" AND TMP.FTLogCode=HD.FTShfCode AND TMP.FTLogDocNo=HD.FTXshDocNo");
                oSql.AppendLine($"WHERE FNLogType ='80' AND ISNULL(FTLogStaPrc,'') != '2'  ");
                oSql.AppendLine($"END");

                return oDB.C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return new List<cmlTCNTTmpLogChg>();
        }
        private void W_SETxClearStaPrc(List<cmlTCNTTmpLogChg> poTmpLogChg)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                foreach (cmlTCNTTmpLogChg oTmpLog in poTmpLogChg)
                {
                    oSql.AppendLine("UPDATE TCNTTmpLogChg with(rowlock)");
                    oSql.AppendLine("SET FTLogStaPrc = ''");
                    oSql.AppendLine($"WHERE FNLogType = {oTmpLog.FNLogType} ");
                    if (!String.IsNullOrEmpty(oTmpLog.FTLogDocNo))
                    {
                        oSql.AppendLine($" AND FTLogDocNo = '{oTmpLog.FTLogDocNo}' ");
                    }
                    oSql.AppendLine($" AND FTLogCode='{oTmpLog.FTLogCode}' ");
                }
                //oDB.C_SETxDataQuery(oSql.ToString());
                if (!string.IsNullOrEmpty(oSql.ToString())) oDB.C_SETxDataQuery(oSql.ToString());   //*Em 63-08-09

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
            }
        }
        private void W_SETxLogStaPrcUpd(cmlTCNTTmpLogChg poTmpLogChg)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                oSql.AppendLine("UPDATE TCNTTmpLogChg with(rowlock)");
                oSql.AppendLine("SET FTLogStaPrc = '1'");
                oSql.AppendLine($"WHERE FNLogType = {poTmpLogChg.FNLogType} ");
                oSql.AppendLine($" AND FTLogDocNo = '{poTmpLogChg.FTLogDocNo}' ");
                oSql.AppendLine($" AND FTLogCode='{poTmpLogChg.FTLogCode}' ");
                oDB.C_SETxDataQuery(oSql.ToString());

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
            }
        }
        private void W_PRCxNoSaleDoc2TmpLogChg()
        {
            DataTable oTblDocNo;
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.Clear();
                oSql.AppendLine("IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TSHD" + cVB.tVB_PosCode + "')) BEGIN");
                oSql.AppendLine("   SELECT DISTINCT '1' AS FTTable,HD.FTXshDocNo From TPSTSalHD HD with(nolock)");
                oSql.AppendLine("   LEFT JOIN TCNTTmpLogChg TmpLog with(nolock) ON HD.FTXshDocNo = TmpLog.FTLogDocNo AND HD.FTShfCode = TmpLog.FTLogCode");
                oSql.AppendLine("   WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "' AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' AND TmpLog.FTLogDocNo IS NULL AND HD.FTXshStaDoc='1'");
                oSql.AppendLine("UNION ALL");
                oSql.AppendLine("   SELECT DISTINCT '2' AS FTTable,HD.FTXshDocNo From TSHD" + cVB.tVB_PosCode + " HD with(nolock)");
                oSql.AppendLine("   LEFT JOIN TCNTTmpLogChg TmpLog with(nolock) ON HD.FTXshDocNo = TmpLog.FTLogDocNo AND HD.FTShfCode = TmpLog.FTLogCode");
                oSql.AppendLine("   WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "' AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' AND TmpLog.FTLogDocNo IS NULL AND HD.FTXshStaDoc='1'");
                oSql.AppendLine("END");
                oSql.AppendLine("ELSE BEGIN");
                oSql.AppendLine("   SELECT DISTINCT '2' AS FTTable,HD.FTXshDocNo From TPSTSalHD HD with(nolock)");
                oSql.AppendLine("   LEFT JOIN TCNTTmpLogChg TmpLog with(nolock) ON HD.FTXshDocNo = TmpLog.FTLogDocNo AND HD.FTShfCode = TmpLog.FTLogCode");
                oSql.AppendLine("   WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "' AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' AND TmpLog.FTLogDocNo IS NULL AND HD.FTXshStaDoc='1'");
                oSql.AppendLine("END");
                oTblDocNo = oDB.C_GEToDataQuery(oSql.ToString());

                foreach (DataRow oDocNo in oTblDocNo.Rows)
                {
                    if (oDocNo.Field<string>("FTTable") == "1")
                    {
                        cSale.C_PRCxAdd2TmpLogChg(80, oDocNo.Field<string>("FTXshDocNo"), false);
                    }
                    else
                    {
                        cSale.C_PRCxAdd2TmpLogChg(80, oDocNo.Field<string>("FTXshDocNo"), true);
                    }
                }
                oDB.C_SETxDataQuery(oSql.ToString());

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oTblDocNo = null;
                oSql = null;
                oDB = null;
            }
        }

        private async Task W_PRCxUploadAsync()
        {
            List<Task<string>> aoTask = new List<Task<string>>();
            aoTask.Add(W_SYNbUpdShf());
            aoTask.Add(W_SYNbUpdVoid());
            aoTask.Add(W_PRCbUpdFixDoc());


            while (aoTask.Count > 0)
            {
                Task<string> oFinnished = await Task.WhenAny(aoTask);
                switch (await oFinnished)
                {
                    case "W_PRCbUpdShf":
                        break;
                    case "W_PRCbUpdVoid":
                        break;
                    case "W_PRCbUpdFixDoc":
                        aoTask.Add(W_PRCbGetFixDoc());
                        break;
                    case "W_PRCbGetFixDoc":
                        if(nW_FixShf>0)
                        {
                            //using (wCountDown wCount = new wCountDown(3, oW_Resource.GetString("tTitleStopUpdSale")))
                            //{
                            //    if (wCount.ShowDialog() == DialogResult.OK)
                            //    {
                            //        aoTask.Add(W_PRCbUpdSale());
                            //    }
                            //}
                            aoTask.Add(W_PRCbUpdSale());    //*Em 63-07-29

                        }
                        break;
                    case "W_PRCbUpdSale":
                        break;
                }
                aoTask.Remove(oFinnished);
            }

        }
        private async Task<string> W_SYNbUpdShf()
        {
            List<cmlTCNTTmpLogChg> aoDocShf;
            bool bSuccess;
            try
            {
                bSuccess = true;
                opbProgressShift.Image = Resources.Time_32;

                aoDocShf = W_GEToDocTmpLogChg(81, cVB.tVB_PosCode);
                W_SETxClearStaPrc(aoDocShf);

                opgShift.Value = 0;
                opgShift.Maximum = aoW_SyncShf.Count + aoDocShf.Count;

                foreach (cmlTSysSyncData oSyncShf in aoW_SyncShf)
                {
                    foreach (cmlTCNTTmpLogChg oLogShf in aoDocShf)
                    {
                        W_SETxLogStaPrcUpd(oLogShf);
                        bSuccess &= oW_Sync.C_SYNbShift(oSyncShf);
                        opgShift.Value++;
                        await Task.Delay(1);
                    }
                }
                opgShift.Value = opgShift.Maximum;

                if (bSuccess == true)
                {
                    opbProgressShift.Image = Resources.AcceptG_32;
                }
                else
                {
                    opbProgressShift.Image = Resources.ClearR_32;
                }
            }
            catch (Exception oEx)
            {
                opbProgressShift.Image = Resources.ClearR_32;
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return "W_PRCbUpdShf";
        }
        private async Task<string> W_SYNbUpdVoid()
        {
            List<cmlTCNTTmpLogChg> aoDocVoid;
            bool bSuccess;
            try
            {
                bSuccess = true;

                opgVoid.Value = 0;
                opbProgressVoid.Image = Resources.Time_32;

                aoDocVoid = W_GEToDocTmpLogChg(82, "");
                W_SETxClearStaPrc(aoDocVoid);

                opgVoid.Value = 0;
                opgVoid.Maximum = aoW_SyncVoid.Count + aoDocVoid.Count;
                foreach (cmlTSysSyncData oSyncVoid in aoW_SyncVoid)
                {
                    foreach (cmlTCNTTmpLogChg oLogVoid in aoDocVoid)
                    {
                        W_SETxLogStaPrcUpd(oLogVoid);
                        bSuccess &= oW_Sync.C_SYNbVoid(oSyncVoid);
                        opgVoid.Value++;
                        await Task.Delay(1);
                    }
                }
                opgVoid.Value = opgVoid.Maximum;

                if (bSuccess == true)
                {
                    opbProgressVoid.Image = Resources.AcceptG_32;
                }
                else
                {
                    opbProgressVoid.Image = Resources.ClearR_32;
                }
            }
            catch (Exception oEx)
            {
                opbProgressVoid.Image = Resources.ClearR_32;
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return "W_PRCbUpdVoid";
        }
        private async Task<string> W_PRCbUpdFixDoc()
        {
            List<string> atShfNotFix;
            bool bSuccess;
            int nResult;
            try
            {
                bSuccess = true;
                nResult = 0;
                nW_FixShf = 0;
                cVB.nVB_FixDoc = 0;

                opgUpdFixDoc.Value = 0;
                opbProgressSendFixDoc.Image = Resources.Time_32;

                atShfNotFix = new cShiftEvent().C_GETaShiftbyEvent("007", "N");

                opgUpdFixDoc.Value = 0;
                opgUpdFixDoc.Maximum = atShfNotFix.Count + 1;
                foreach (string tShfCode in atShfNotFix)
                {
                    nResult = cShift.C_CHKbShift2BackOffice(tShfCode);
                    if (nResult >= 0)
                    {
                        nW_FixShf += nResult;
                        bSuccess &= true;
                    }
                    else
                    {
                        bSuccess &= false;
                    }

                    opgUpdFixDoc.Value++;
                    await Task.Delay(1);
                }
                nResult = cShift.C_CHKbShift2BackOffice(cVB.tVB_ShfCode);
                if (nResult >= 0)
                {
                    nW_FixShf += nResult;
                    bSuccess &= true;
                }
                else
                {
                    bSuccess &= false;
                }
                opgUpdFixDoc.Value = opgUpdFixDoc.Maximum;
                await Task.Delay(1);
                await Task.Run(() =>
                {
                    cShift.C_GENxListenFixDoc(true);
                });
                if (bSuccess == true)
                {
                    opbProgressSendFixDoc.Image = Resources.AcceptG_32;
                }
                else
                {
                    opbProgressSendFixDoc.Image = Resources.ClearR_32;
                }
            }
            catch (Exception oEx)
            {
                opbProgressSendFixDoc.Image = Resources.ClearR_32;
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return "W_PRCbUpdFixDoc";
        }
        private async Task<string> W_PRCbGetFixDoc()
        {
            int nTimeout = 30000;
            Task oChkFixDoc;
            try
            {
                opgGetFixDoc.Value = 0;
                if (nW_FixShf == 0)
                {
                    opgGetFixDoc.Maximum = 100;
                }
                else
                {
                    opgGetFixDoc.Maximum = nW_FixShf;
                }
                opbPgrsGetFixDoc.Image = Resources.Time_32;

                W_PRCxNoSaleDoc2TmpLogChg();
                opgGetFixDoc.Value++;

                oChkFixDoc = Task.Run(async () =>
                {
                    while (cVB.nVB_FixDoc < nW_FixShf)
                    {
                        opgGetFixDoc.Value = cVB.nVB_FixDoc;
                        await Task.Delay(100);
                    }
                });

                if (await Task.WhenAny(oChkFixDoc, Task.Delay(nTimeout)) == oChkFixDoc)
                {
                    // task completed
                    opbPgrsGetFixDoc.Image = Resources.AcceptG_32;
                }
                else
                {
                    // timeout logic
                    opbPgrsGetFixDoc.Image = Resources.ClearR_32;
                }

                opgGetFixDoc.Value = opgGetFixDoc.Maximum;
            }
            catch (Exception oEx)
            {
                opbPgrsGetFixDoc.Image = Resources.ClearR_32;
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return "W_PRCbGetFixDoc";
        }
        private async Task<string> W_PRCbUpdSale()
        {
            bool bSuccess;
            List<cmlTCNTTmpLogChg> aoDocSaleTrans, aoDocSaleTmp;
            try
            {
                bSuccess = true;
                opgUpdSale.Value = 0;
                opbUpdSale.Image = Resources.Time_32;

                aoDocSaleTrans = W_GEToSaleDocFrmTmpLogChg(true);
                aoDocSaleTmp = W_GEToSaleDocFrmTmpLogChg(false);

                if (aoDocSaleTrans.Count + aoDocSaleTmp.Count > 0)
                {
                    opgUpdSale.Maximum = aoW_SyncSale.Count * (aoDocSaleTrans.Count + aoDocSaleTmp.Count);

                    W_SETxClearStaPrc(aoDocSaleTrans);
                    W_SETxClearStaPrc(aoDocSaleTmp);
                    olaListFixDoc.Text = W_GETtTitleFixDoc(opgUpdSale.Maximum);
                    opnUpdSaleContent.Visible = true;
                    opnUpdSaleContent.Refresh();

                    //*Em 63-07-29
                    if (aoDocSaleTrans.Count > 0)
                    {
                        olaUpdStaFixDoc.Text = W_GETtMsgFixDoc(opgUpdSale.Value, opgUpdSale.Maximum, aoDocSaleTrans[0].FTLogDocNo);
                    }
                    else
                    {
                        if (aoDocSaleTmp.Count > 0)
                        {
                            olaUpdStaFixDoc.Text = W_GETtMsgFixDoc(opgUpdSale.Value, opgUpdSale.Maximum, aoDocSaleTmp[0].FTLogDocNo);
                        }
                    }
                    using (wCountDown wCount = new wCountDown(3, oW_Resource.GetString("tTitleStopUpdSale")))
                    {
                        if (wCount.ShowDialog() == DialogResult.Cancel)
                        {
                            return "W_PRCbUpdSale";
                        }
                    }
                    //++++++++++++++++++

                    foreach (cmlTSysSyncData oSyncSale in aoW_SyncSale)
                    {
                        foreach (cmlTCNTTmpLogChg oLogSale in aoDocSaleTrans)
                        {
                            olaUpdStaFixDoc.Text = W_GETtMsgFixDoc(opgUpdSale.Value, opgUpdSale.Maximum, oLogSale.FTLogDocNo);
                            W_SETxLogStaPrcUpd(oLogSale);
                            bSuccess &= oW_Sync.C_SYNbSale(oSyncSale);
                            opgUpdSale.Value++;
                            await Task.Delay(1);
                        }
                        foreach (cmlTCNTTmpLogChg oLogSale in aoDocSaleTmp)
                        {
                            olaUpdStaFixDoc.Text = W_GETtMsgFixDoc(opgUpdSale.Value, opgUpdSale.Maximum, oLogSale.FTLogDocNo);
                            W_SETxLogStaPrcUpd(oLogSale);
                            bSuccess &= oW_Sync.C_SYNbSale(oSyncSale, true);
                            opgUpdSale.Value++;
                            await Task.Delay(1);
                        }
                    }
                }
                else
                {
                    opgUpdSale.Maximum = 100;
                }


                opgUpdSale.Value = opgUpdSale.Maximum;
                olaUpdStaFixDoc.Text = W_GETtMsgFixDoc(opgUpdSale.Value, opgUpdSale.Maximum, "");
                if (bSuccess == true)
                {
                    opbUpdSale.Image = Resources.AcceptG_32;
                }
                else
                {
                    opbUpdSale.Image = Resources.ClearR_32;
                }
            }
            catch (Exception oEx)
            {
                opbUpdSale.Image = Resources.ClearR_32;
                new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return "W_PRCbUpdSale";
        }
    }
}
