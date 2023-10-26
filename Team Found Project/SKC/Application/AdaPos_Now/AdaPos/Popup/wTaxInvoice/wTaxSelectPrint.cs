using AdaPos.Class;
using AdaPos.Resources_String.Local;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Viewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wTaxInvoice
{
    public partial class wTaxSelectPrint : Form
    {
        #region Variable
        private ResourceManager oW_Resource;
        #endregion End Variable

        public wTaxSelectPrint()
        {
            InitializeComponent();
            try
            {
                //Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHlDr8/6PIqNKBuLMEkN8xMUPugEQPeiwAHVm+OV" +
                //"bCQVabeN3of7ZbnsixRRu+7irZqJ8c0f4LGB9+5sPaMJomcsE37V4Zf1NuPeQ8n+CDF+5Cp4IOyIAra8" +
                //"o4iA3x/nD4ktTT7e/BzGEHvbCZvNgR9i00xpzfC/5xrrzGqNC0AF8PWDnOCg0MPNodj9soA4ZH0NPRLj" +
                //"jwNPBOxmmG1pLoKBG3Bh7ALEQ2moT93cIEj124GvRIPnChAkiyLRMZkIlTdPYuBHEa7CPM9knzuGqaiz" +
                //"ZrN9eWQ+iGiV/grvhEJU3foCQaGJgwnsRHbMPCSZdHtT/4yxoO42SWgZFayM/pDuOXkVhKytawLWnrrQ" +
                //"oNUQpmvSarHOUVDLRe70HbyRswH0AXraboEed4qTfn+CUBtMdSEwQLqj237m6N8OTvsROjcXLi4QfXlP" +
                //"A28SpfXbQBvEN2TrGqBr5dyKpgbkG+58x85lFO9s1XcQoKXfml8elYzFhMlcae97o5u4dTE/VIseSJ7W" +
                //"/scPHOg5gM3Tn72U32bW53UF8/kcNl4+T0WHpg==";

                //Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHmqPmkgcX/zd1ltNIeNhk9sfWeDJxxDQwVKrD+nljWuRzePYk" +
                //"RjL5nQ74+OnxhfQlN22oqJHsZS6p6zUzMLNCNc2yb9HEFtwhlzZ65/0DGLaJWFEWEEea1/OaH9Vt1TFWNCDGw0nULW" +
                //"ie526WvmZ6SdY0eqbd+vW18+TNaizcOtKgFLeSYdqdxuyl8uRRmKVdNUpGodrQRB5ph+pBeaYASlWgF5dLjbUXBT54" +
                //"eTgM8sGsvxQ1+vSczSYRICRhm8dme9pgunvVk4A9SnhEsiGYdkbTKmY+g/kot6jaKqR2fyuG3ulGBhDxd36g3CTe1T" +
                //"mD0z+jc2y9lklJzPyF+lNeOx0+itlwPNBqsiGQITL5DMomSdFZ0/uYPh2rlWuK38wOUPwhDWJ/ladJTyoevjf8+Taz" +
                //"7J2vopL/zc7nT27zr9lsf6GfS7QYdhKcZcWQ4pVXBG7Z4YO2F9pV4mCShseurNVVbwfQ2xexMKC+aH7naDjvPrQukD" +
                //"slHFrEf7Bfdh10Ns3/ecsxWTg5vAl9z45nNouLYcOgv7HrHb4CkimuOXEA==";

                Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHk1VTl4VbiYSNBazINvEmP+u37hUXswxFJSjuF0f6pbNFNGqW" +
                "oEovpmKYyeK/3F42BkBqR0kRbc+mxusAVmHck6Cdjuf9qGm2+qoo8rFrA1+osiNq/AUpk0SxPv/pwHz1BWBcLkCp3F" +
                "Em0w7HHlvFo1czBWhbEBTEDLGTwcCQT8iQ0NnTbj51HkqA2U3J+z4uBoJfmVBeAWaXBvK0rGPxWdz088qhCqaRrDZW" +
                "NrXqqVtnzKKkb77v+8D7/DnWT90gJPRI8mylj6fbXj2YrMm6wHIX49JL3wwOdL9A+up7x+U0DZsLn/fk6/LQf5bHdv" +
                "eJeRWqmm1wBw4DcFaiE4F33RlQtzCiUXYRTnKfcJnVrrgtiB8gDB7s3zLYrIWO1Wm+UEWwU2Yci+s8y/dQV5lQeI5g" +
                "vzNkdupw1BuYLxxcpqktdq9ptZf/9hy5wLsbgETK47kFB6rUxaOWh7LfMSCtDL6Yo3JtDveILl9NDiRGVFPaGg7/6/" +
                "tuM2pDielSLPyu3Rwy52QUyx1t3Mhq79cfm0+hZe6pWKSl/ehS9Pjbzeam6jrwme9nMmWsbL6mTTcK1gxRVJHZ5sRT" +
                "lqIzcJNtFpIRfcSA5kQHkYA0Te604CL9oEM84ohM7OSaVKMeRBXbS0/hSiz9JW4GcPQ21W3bB1";

                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "wTaxSelectPrint : " + oEx.Message); }
        }

        #region Function
        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmPreview.BackColor = cVB.oVB_ColDark;
                ocmPrint.BackColor = cVB.oVB_ColDark;
                ocmPrnSelect.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "W_SETxDesign : " + oEx.Message); }
        }
        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;
                }

                olaTitleSelectPnt.Text = oW_Resource.GetString("tTitlePrnSelect");
                orbPrnThermal.Text = oW_Resource.GetString("tPaperThermal");
                orbPrnA4.Text = oW_Resource.GetString("tPaperA4");
                orbPrnThermal.Checked = true;

                if (cVB.tVB_CNStaPrnTax == "1")
                {
                    orbPrnThermal.Checked = true;
                }
                else
                {
                    orbPrnA4.Checked = true;
                }
                otbPrnDriver.Text = cVB.tVB_CNTaxPrnDriver;
                if (!string.IsNullOrEmpty(cVB.tVB_CNTaxPrnDriver))
                {
                    ocdPrn.PrinterSettings.PrinterName = cVB.tVB_CNTaxPrnDriver;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "W_SETxText : " + oEx.Message); }
        }
        private string W_GETtReport(string ptRptName)
        {
            string tPathRpt = "";
            try
            {
                tPathRpt = Directory.GetParent(Application.StartupPath).ToString() + @"\Reports";
                if (!Directory.Exists(tPathRpt)) Directory.CreateDirectory(tPathRpt);

                if (File.Exists(tPathRpt + @"\" + ptRptName))
                {
                    tPathRpt += @"\" + ptRptName;
                }
                else
                {
                    tPathRpt = "";
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "W_CHKbReport : " + oEx.Message); }
            return tPathRpt;
        }

        private void W_DATxUpdSetting()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("IF EXISTS(SELECT FTSysCode FROM TSysSetting WITH(NOLOCK) WHERE FTSysCode = 'tPS_TaxPrint') BEGIN");
                oSql.AppendLine("	UPDATE TSysSetting");
                oSql.AppendLine("	SET FTSysStaUsrValue = '" + (orbPrnThermal.Checked == true ? "1" : "2") + "',");
                oSql.AppendLine("	FTSysStaUsrRef = '" + otbPrnDriver.Text + "'");
                oSql.AppendLine("	WHERE FTSysCode = 'tPS_TaxPrint'");
                oSql.AppendLine("END");
                oSql.AppendLine("ELSE BEGIN");
                oSql.AppendLine("	INSERT INTO TSysSetting");
                oSql.AppendLine("	(FTSysCode,FTSysApp,FTSysKey,FTSysSeq,FTGmnCode,");
                oSql.AppendLine("	FTSysStaAlwEdit,FTSysStaDataType,FNSysMaxLength,FTSysStaDefValue,FTSysStaDefRef,FTSysStaUsrValue,FTSysStaUsrRef,");
                oSql.AppendLine("	FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                oSql.AppendLine("	VALUES");
                oSql.AppendLine("	('tPS_TaxPrint','AdaPos','PrnTax','1','MPOS',");
                oSql.AppendLine("	'1','0',50,'1','','" + (orbPrnThermal.Checked == true ? "1" : "2") + "','" + otbPrnDriver.Text + "',");
                oSql.AppendLine("	GETDATE(),'',GETDATE(),'')");
                oSql.AppendLine("END");
                oDB.C_SETxDataQuery(oSql.ToString());

                cVB.tVB_CNStaPrnTax = (orbPrnThermal.Checked == true ? "1" : "2");
                cVB.tVB_CNTaxPrnDriver = otbPrnDriver.Text;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "W_DATxUpdSetting : " + oEx.Message); }
        }
        #endregion End Function

        #region Method/Events
        private void oDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                cVB.oVB_TaxInvoice.W_PRNxDrawTax(e.Graphics);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "oDoc_PrintPage : " + oEx.Message); }
        }
        private void ocmPrnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocdPrn.ShowDialog() == DialogResult.OK)
                {
                    otbPrnDriver.Text = ocdPrn.PrinterSettings.PrinterName;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "ocmPrnSelect_Click : " + oEx.Message); }
        }
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "ocmBack_Click : " + oEx.Message); }
        }

        private void wTaxSelectPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                W_DATxUpdSetting();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "wTaxSelectPrint_FormClosing : " + oEx.Message); }
        }
        private void orbPrnThermal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                otbPrnDriver.Enabled = false;
                ocmPrnSelect.Enabled = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "orbPrnThermal_CheckedChanged : " + oEx.Message); }
        }
        private void orbPrnA4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                otbPrnDriver.Enabled = true;
                ocmPrnSelect.Enabled = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "orbPrnA4_CheckedChanged : " + oEx.Message); }
        }

        private void ocmPreview_Click(object sender, EventArgs e)
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            StiReport oReport = new StiReport();
            StiDictionary oParam = new StiDictionary();
            string tPathRpt = "";
            try
            {
                if (orbPrnThermal.Checked == true)
                {
                    new wTaxInvoicePreview().ShowDialog();
                }

                if (orbPrnA4.Checked == true)
                {
                    if (string.IsNullOrEmpty(otbPrnDriver.Text))
                    {
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgSelDriverPrn"), 3);
                        return;
                    }

                    //if (cVB.oVB_TaxInvoice.oW_TaxHD.FNXshDocType == 9)
                    if (cVB.oVB_TaxInvoice.oW_TaxHD.FNXshDocType == 5)  //*Em 63-04-21
                    {
                        tPathRpt = W_GETtReport("Frm_PSInvoiceRefund.mrt");
                    }
                    else
                    {
                        tPathRpt = W_GETtReport("Frm_PSInvoiceSale.mrt");
                    }

                    if (tPathRpt != "")
                    {
                        oSettings = new PrinterSettings();
                        oSettings = ocdPrn.PrinterSettings;

                        oReport = new StiReport();
                        oReport.Load(tPathRpt);

                        StiSqlDatabase oDBSale = (StiSqlDatabase)oReport.Dictionary.Databases["SaleInv"];
                        oDBSale.ConnectionString = new cDatabase().C_CONoDatabase(cVB.oVB_Config).ConnectionString;

                        oReport.Dictionary.Variables["SP_nLang"].ValueObject = cVB.nVB_Language;
                        oReport.Dictionary.Variables["nLanguage"].ValueObject = cVB.nVB_Language;
                        oReport.Dictionary.Variables["SP_tCompCode"].ValueObject = cVB.tVB_CmpCode;
                        oReport.Dictionary.Variables["SP_tCmpBch"].ValueObject = cVB.tVB_BchCode;
                        oReport.Dictionary.Variables["SP_nAddSeq"].ValueObject = "1";
                        oReport.Dictionary.Variables["SP_tDocNo"].ValueObject = cVB.oVB_TaxInvoice.oW_TaxHD.FTXshDocNo;
                        oReport.Dictionary.Variables["SP_tStaPrn"].ValueObject = "1";

                        oReport.PreviewSettings = 10;
                        oReport.Render(false);
                        oReport.Show();
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgChkReport"), 3);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxSelectPrint", "ocmPreview_Click : " + oEx.Message); }
        }

        private void ocmPrint_Click(object sender, EventArgs e)
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            StiReport oReport = new StiReport();
            StiDictionary oParam = new StiDictionary();
            string tPathRpt = "";
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (orbPrnThermal.Checked == true)
                {
                    cVB.bVB_PrnTaxInvoiceCopy = false;
                    for (int nOri = 0; nOri < cVB.nVB_PrnTaxMaster; nOri++)
                    {
                        oDoc = new PrintDocument();
                        oSettings = new PrinterSettings();

                        oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                        oDoc.PrintController = new StandardPrintController();
                        oDoc.PrintPage += oDoc_PrintPage;
                        oDoc.Print();
                    }
                    cVB.bVB_PrnTaxInvoiceCopy = true;
                    for (int nCopy = 0; nCopy < cVB.nVB_PrnTaxCopy; nCopy++)
                    {
                        oDoc = new PrintDocument();
                        oSettings = new PrinterSettings();

                        oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                        oDoc.PrintController = new StandardPrintController();
                        oDoc.PrintPage += oDoc_PrintPage;
                        oDoc.Print();
                    }
                    cVB.oVB_TaxInvoice.W_PRCxPrintUpd();
                    Cursor.Current = Cursors.Default;
                }

                if (orbPrnA4.Checked == true)
                {
                    if (string.IsNullOrEmpty(otbPrnDriver.Text))
                    {
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgSelDriverPrn"), 3);
                        return;
                    }

                    //if (cVB.oVB_TaxInvoice.oW_TaxHD.FNXshDocType == 9)
                    if (cVB.oVB_TaxInvoice.oW_TaxHD.FNXshDocType == 5)  //*Em 63-04-21
                    {
                        tPathRpt = W_GETtReport("Frm_PSInvoiceRefund.mrt");
                    }
                    else
                    {
                        tPathRpt = W_GETtReport("Frm_PSInvoiceSale.mrt");
                    }
                    if (tPathRpt != "")
                    {
                        oSettings = new PrinterSettings();
                        oSettings = ocdPrn.PrinterSettings;
                        oReport.Load(tPathRpt);

                        //cVB.bVB_PrnTaxInvoiceCopy = false;
                        for (int nOri = 0; nOri < cVB.nVB_PrnTaxMaster; nOri++)
                        {
                            StiSqlDatabase oDBSale = (StiSqlDatabase)oReport.Dictionary.Databases["SaleInv"];
                            oDBSale.ConnectionString = new cDatabase().C_CONoDatabase(cVB.oVB_Config).ConnectionString;

                            oReport.Dictionary.Variables["SP_nLang"].ValueObject = cVB.nVB_Language;
                            oReport.Dictionary.Variables["nLanguage"].ValueObject = cVB.nVB_Language;
                            oReport.Dictionary.Variables["SP_tCompCode"].ValueObject = cVB.tVB_CmpCode;
                            oReport.Dictionary.Variables["SP_tCmpBch"].ValueObject = cVB.tVB_BchCode;
                            oReport.Dictionary.Variables["SP_nAddSeq"].ValueObject = "1";
                            oReport.Dictionary.Variables["SP_tDocNo"].ValueObject = cVB.oVB_TaxInvoice.oW_TaxHD.FTXshDocNo;
                            oReport.Dictionary.Variables["SP_tStaPrn"].ValueObject = "1";

                            oReport.Render();
                            oReport.Print(false, oSettings);
                            Cursor.Current = Cursors.Default;
                        }
                        //cVB.bVB_PrnTaxInvoiceCopy = true; cVB.nVB_PrnTaxRef
                        StiReport oReportCopy = new StiReport();
                        oReportCopy.Load(tPathRpt);

                        for (int nCopy = 0; nCopy < cVB.nVB_PrnTaxCopy; nCopy++)
                        {
                            //oDoc.Print();
                            StiSqlDatabase oDBSaleCopy = (StiSqlDatabase)oReportCopy.Dictionary.Databases["SaleInv"];
                            oDBSaleCopy.ConnectionString = new cDatabase().C_CONoDatabase(cVB.oVB_Config).ConnectionString;

                            oReportCopy.Dictionary.Variables["SP_nLang"].ValueObject = cVB.nVB_Language;
                            oReportCopy.Dictionary.Variables["nLanguage"].ValueObject = cVB.nVB_Language;
                            oReportCopy.Dictionary.Variables["SP_tCompCode"].ValueObject = cVB.tVB_CmpCode;
                            oReportCopy.Dictionary.Variables["SP_tCmpBch"].ValueObject = cVB.tVB_BchCode;
                            oReportCopy.Dictionary.Variables["SP_nAddSeq"].ValueObject = "1";
                            oReportCopy.Dictionary.Variables["SP_tDocNo"].ValueObject = cVB.oVB_TaxInvoice.oW_TaxHD.FTXshDocNo;

                            oReportCopy.Dictionary.Variables["SP_tStaPrn"].ValueObject = "2";

                            oReportCopy.Render();
                            oReportCopy.Print(false, oSettings);
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgChkReport"), 3);
                        return;
                    }
                }
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wTaxSelectPrint", "ocmPrint_Click : " + oEx.Message);
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion End Method/Events
    }
}
