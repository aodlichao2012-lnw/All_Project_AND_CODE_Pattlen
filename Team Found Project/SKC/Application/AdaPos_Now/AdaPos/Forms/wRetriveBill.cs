using AdaPos.Class;
using AdaPos.Resources_String.Local;
using C1.Win.C1FlexGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wRetriveBill : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        #endregion End Variable

        #region Function
        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;

                ocmSearch.BackColor = cVB.oVB_ColNormal;
                //ogdVoid.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                //oW_SP.SP_SETxSetGridviewFormat(ogdVoid); //*Net 63-03-03 Set Design Gridview
                oW_SP.SP_SETxSetGridFormat(ogdVoid);    //*Em 63-08-11

                ocmAccept.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDeposit", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                //*Em 62-09-09
                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                olaRetrive.Text = oW_Resource.GetString("tRetriveBill");    //*Em 63-08-11
                olaTitleSearch.Text = cVB.oVB_GBResource.GetString("tSearch"); //*Em 63-08-11

                olaUsrName.Text = new cUser().C_GETtUsername();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetriveBill", "W_SETxText : " + oEx.Message); }
        }

        private void W_GETxData2Grid()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            try
            {
                //oSql.AppendLine("SELECT FNHldNo, FTXshDocNo, FDXshDocDate, FTCstCode, FCXshGrand");
                oSql.AppendLine("SELECT FNHldNo, FTXshDocNo, FDXshDocDate, ISNULL(FTXshDisChgTxt,'') AS tRetriveCode, FCXshGrand, FTCstCode"); //*Arm 63-09-13 รหัสค้นคืน
                oSql.AppendLine("FROM TPSTHoldHD WITH(NOLOCK)");
                if (!string.IsNullOrEmpty(otbSearch.Text))
                {
                    oSql.AppendLine("WHERE FTXshDocNo LIKE '%" + otbSearch.Text.Trim() + "%'");
                    oSql.AppendLine("OR FTCstCode LIKE '%" + otbSearch.Text.Trim() + "%'");
                    oSql.AppendLine("OR FTXshDisChgTxt LIKE '%" + otbSearch.Text.Trim() + "%'"); //*Arm 63-09-13
                }
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                //*Em 63-08-11
                //ogdVoid.Rows.Count = ogdVoid.Rows.Fixed;
                ogdVoid.DataSource = odtTmp;
                W_SETxColGrid(ogdVoid);
                //+++++++++++++++

                //ogdVoid.Rows.Clear();
                //foreach (DataRow oRow in odtTmp.Rows)
                //{
                //    //ogdVoid.Rows.Add(oRow["FNHldNo"].ToString(), oRow["FTXshDocNo"], oRow["FDXshDocDate"], oRow["FTCstCode"],  oRow["FCXshGrand"]);
                //    ogdVoid.Rows.Add(oRow["FNHldNo"].ToString(), oRow["FTXshDocNo"], oRow["FDXshDocDate"], oRow["FTCstCode"], new cSP().SP_SETtDecShwSve(1, (decimal)oRow["FCXshGrand"], cVB.nVB_DecShow)); //*Em 63-06-07
                //}

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetriveBill", "W_GETxData2Grid : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
            }
        }

        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                switch (poGD.Name)
                {
                    case "ogdVoid":
                        nWidth = poGD.Width - 80;
                        poGD.Cols["FNHldNo"].Width = 80;
                        poGD.Cols["FTXshDocNo"].Width = nWidth * 30 / 100;
                        poGD.Cols["FDXshDocDate"].Width = nWidth * 20 / 100;
                        //poGD.Cols["FTCstCode"].Width = nWidth * 25 / 100;
                        poGD.Cols["tRetriveCode"].Width = nWidth * 25 / 100; //*Arm 63-09-13
                        poGD.Cols["FCXshGrand"].Width = nWidth * 20 / 100;

                        poGD.Cols["FNHldNo"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                        poGD.Cols["FTXshDocNo"].Caption = cVB.oVB_GBResource.GetString("tDocNo");
                        poGD.Cols["FDXshDocDate"].Caption = cVB.oVB_GBResource.GetString("tDatetime");
                        //poGD.Cols["FTCstCode"].Caption = cVB.oVB_GBResource.GetString("tCstCode");
                        poGD.Cols["tRetriveCode"].Caption = cVB.oVB_GBResource.GetString("tReturnBillCode"); //*Arm 63-09-13 รหัสค้นคืน
                        poGD.Cols["FCXshGrand"].Caption = cVB.oVB_GBResource.GetString("tAmount");

                        poGD.ExtendLastCol = true; //*Net 63-07-31 ปรับตาม Moshi

                        poGD.Cols["FNHldNo"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTXshDocNo"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FDXshDocDate"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        //poGD.Cols["FTCstCode"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["tRetriveCode"].TextAlignFixed = TextAlignEnum.CenterCenter; //*Arm 63-09-13
                        poGD.Cols["FCXshGrand"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["FNHldNo"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTXshDocNo"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FDXshDocDate"].TextAlign = TextAlignEnum.CenterCenter;
                        //poGD.Cols["FTCstCode"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["tRetriveCode"].TextAlign = TextAlignEnum.LeftCenter; //*Arm 63-09-13
                        poGD.Cols["FCXshGrand"].TextAlign = TextAlignEnum.RightCenter;

                        poGD.Cols["FCXshGrand"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);

                        poGD.Cols["FTCstCode"].Visible = false; //*Arm 63-09-13
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_SETxColGrid : " + oEx.Message);
            }
        }
        #endregion Function
        public wRetriveBill()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                //oW_SP.SP_PRCxFlickering(this.Handle);
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);

                W_SETxDesign();
                W_SETxText();
                W_GETxData2Grid();
                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in opnMenu.Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetriveBill", "wRetriveBill : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }

        /// <summary>
        /// GET Notification
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void W_Notification(object s, EventArgs e)
        {
            try
            {
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wHome", "W_Notification : " + oEx.Message);
            }
        }

        /// <summary>
        /// Timing to Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmClose_Tick(object sender, EventArgs e)
        {
            try
            {
                if (nW_Time == 5)
                    this.Close();

                nW_Time++;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetriveBill", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wRetriveBill_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetriveBill", "wRetriveBill_FormClosing : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "ocmNotify_Click : " + oEx.Message); }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                //cSale.nC_HoldNo = Convert.ToInt32( ogdVoid.Rows[ogdVoid.CurrentRow.Index].Cells[0].Value);
                //cVB.tVB_CstCode = ogdVoid.Rows[ogdVoid.CurrentRow.Index].Cells[3].Value.ToString(); //*Em 62-12-18

                //*Em 63-08-11
                cSale.nC_HoldNo = Convert.ToInt32(ogdVoid.GetData(ogdVoid.RowSel, ogdVoid.Cols["FNHldNo"].Index).ToString());
                cVB.tVB_CstCode = ogdVoid.GetData(ogdVoid.RowSel, ogdVoid.Cols["FTCstCode"].Index).ToString();
                //+++++++++++++
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "ocmMenu_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open Popup about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAbout_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAbout();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open popup help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHelp_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxHelp();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "ocmHelp_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Show function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDoShowKB();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "ocmShwKb_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open Calculate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxCalculator();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "ocmCalculate_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Call Keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wRetrive", "ocmKB_Click : " + ex.Message); }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //*Arm 63-09-13 Comment Code
                //if (!string.IsNullOrEmpty(otbSearch.Text))
                //{
                //    W_GETxData2Grid();
                //}

                W_GETxData2Grid(); //*Arm 63-09-13
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wRetrive", "ocmSearch_Click : " + ex.Message); }
        }

        /// <summary>
        /// Keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyData)
                {
                    case Keys.Enter:
                        W_GETxData2Grid();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRetrive", "otbSearch_KeyDown : " + oEx.Message); }
        }
        //*Net 63-07-31 ปรับตาม Moshi
        private void wRetriveBill_Shown(object sender, EventArgs e)
        {
            if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
            cSP.SP_GETxCountNotify(olaMsgCount, opnNotify); //*Net 63-07-06 ย้ายมาจาก ctor
            W_SETxColGrid(ogdVoid); //*Em 63-08-11
        }

        private void ogdVoid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ocmAccept_Click(ocmAccept, null);
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wRetrive", "ogdVoid_DoubleClick : " + oEx.Message);
            }
        }
    }
}
