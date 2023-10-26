using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Resources_String.Global;
using AdaPos.Resources_String.Local;
using RabbitMQ.Client.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MethodBase = System.Reflection.MethodBase;

namespace AdaPos.Forms
{
    public partial class wProgress : Form
    {
        private ResourceManager oW_Resource;
        private cSyncData oC_oSyncData;
        private List<cmlTSysSyncData> aC_TSysSyncData;
        private int nW_Mode;  // Mode 1 AutoSync , 2 Load Data
        private wBlank oW_BlankPage;
        public wProgress(int pnMode,bool pbBGPage)
        {
            if(pbBGPage)
            {
                oW_BlankPage = new wBlank();
                oW_BlankPage.Show();
            }
            InitializeComponent();
            try
            {
                nW_Mode = pnMode;
                W_SETxText();
                W_SETxDesign();
                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgress", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void wAutoSync_Load(object sender, EventArgs e)
        {
            try
            {
                if (nW_Mode == 1)
                {
                    oC_oSyncData = new cSyncData();
                    aC_TSysSyncData = oC_oSyncData.C_GETaTableSyncDB();
                    if (aC_TSysSyncData != null && aC_TSysSyncData.Count > 0)
                    {
                        opgStatus.Maximum = aC_TSysSyncData.LastOrDefault().FNSynSeqNo;
                        oC_oSyncData.C_PRCxSyncDwn(aC_TSysSyncData, "", OBGWorker_ProgressChanged, OBGWorker_RunWorkerCompleted);
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgress", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void OBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int nIndex = 0;
            try
            {
                this.Invoke((MethodInvoker)delegate //*Arm 63-07-31 ¡�Ҩҡ Moshi
                {
                    //nIndex = Convert.ToInt32(Math.Ceiling((e.ProgressPercentage * aC_TSysSyncData.LastOrDefault().FNSynSeqNo) / 100f));
                    nIndex = aC_TSysSyncData.IndexOf(aC_TSysSyncData.Where(oData => oData.FNSynSeqNo == e.ProgressPercentage).FirstOrDefault());
                    opgStatus.Value = e.ProgressPercentage;
                    if (nIndex < aC_TSysSyncData.Count && nIndex >= 0) //*Arm 63-07-31 ¡�Ҩҡ Moshi (nIndex+1)
                    {
                        olaStaTblName.Text = aC_TSysSyncData[nIndex].FTSynName; //*Arm 63-07-31 ¡�Ҩҡ Moshi (nIndex+1)
                    }
                    else
                    {
                        olaStaTblName.Text = "";
                    }
                    olaStatusTable.Text = $"({e.ProgressPercentage}/{aC_TSysSyncData.LastOrDefault().FNSynSeqNo})";
                    this.Refresh();
                });
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgress", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void OBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgress", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
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
                if (opnHD.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        opnHD.BackColor = cVB.oVB_ColDark;
                        opgStatus.ForeColor = cVB.oVB_ColNormal;
                    });
                }
                else
                {
                    opnHD.BackColor = cVB.oVB_ColDark;
                    opgStatus.ForeColor = cVB.oVB_ColNormal; //*Arm 63-07-31 ¡�Ҩҡ Moshi
                }

                if (opgStatus.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        opgStatus.ForeColor = cVB.oVB_ColNormal;
                    });
                }
                else
                {
                    opgStatus.ForeColor = cVB.oVB_ColNormal;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgress", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
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

                cVB.tVB_KbdScreen = "SPLASHSCREEN";

                if (olaTitle.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        switch (nW_Mode)
                        {
                            case 1:
                                olaTitle.Text = oW_Resource.GetString("tTitleAutoSync") + "...";
                                break;
                            case 2:
                                olaTitle.Text = oW_Resource.GetString("tSetupPos") + "...";
                                break;
                            case 3:
                                olaTitle.Text = oW_Resource.GetString("tCalPmt") + "...";
                                break;
                        }
                    });
                }
                else
                {
                    switch (nW_Mode)
                    {
                        case 1:
                            olaTitle.Text = oW_Resource.GetString("tTitleAutoSync") + "...";
                            break;
                        case 2:
                            olaTitle.Text = oW_Resource.GetString("tSetupPos") + "...";
                            break;
                        case 3:
                            olaTitle.Text = oW_Resource.GetString("tCalPmt") + "...";
                            break;
                    }
                }

                if (olaStatus.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        switch (nW_Mode)
                        {
                            case 1:
                                //olaStatus.Text = oW_Resource.GetString("tTitleNameDwn")+" : ";
                                olaStatus.Text = oW_Resource.GetString("tTitleChkUpd") + " : "; //*Arm 63-08-18
                                break;
                            case 2:
                                olaStatus.Text = oW_Resource.GetString("");
                                break;
                        }
                    });
                }
                else
                {
                    switch (nW_Mode)
                    {
                        case 1:
                            //olaStatus.Text = oW_Resource.GetString("tTitleNameDwn")+" : ";
                            olaStatus.Text = oW_Resource.GetString("tTitleChkUpd") + " : "; //*Arm 63-08-18
                            break;
                        case 2:
                            olaStatus.Text = oW_Resource.GetString("");
                            break;
                    }
                }

                if (olaStaTblName.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        olaStaTblName.Text = "";
                    });
                }
                else
                {
                    olaStaTblName.Text = "";
                }

                if (olaStatusTable.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        olaStatusTable.Text = $"(0/0)";
                    });
                }
                else
                {
                    olaStatusTable.Text = $"(0/0)";
                }
                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgress", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        public void W_SETxProgress(int pnPercent)
        {
            try
            {
                if (pnPercent < 0) pnPercent = 0;
                if (pnPercent > 100) pnPercent = 100;
                this.Invoke((MethodInvoker)delegate
                {
                    opgStatus.Value = pnPercent;
                    olaStatusTable.Text = opgStatus.Value.ToString() + "%";
                    this.Refresh();
                });
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgress", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void wProgress_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (oW_BlankPage != null) oW_BlankPage.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wProgress", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void wProgress_Shown(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
