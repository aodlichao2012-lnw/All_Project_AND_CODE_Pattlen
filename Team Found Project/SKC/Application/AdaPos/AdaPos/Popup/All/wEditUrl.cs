using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Webservice.Respond;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.All
{
    public partial class wEditUrl : Form
    {
        private cmlTSysConfig oW_Cfg;
        public string tW_Url;
        public wEditUrl(cmlTSysConfig poConfig)
        {
            InitializeComponent();
            try
            {
                oW_Cfg = new cmlTSysConfig();
                oW_Cfg = poConfig;
                W_SETxDesign();
                W_SETxText();

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditUrl", "wEditUrl : " + oEx.Message); }
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
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmChkUrl.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditUrl", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                olaTitle.Text = cVB.oVB_GBResource.GetString("tTitleEdit");
                olaTitleEdit.Text = cVB.oVB_GBResource.GetString("tTitleURL") + " : " + oW_Cfg.FTSysName;
                otbUrl.Text = oW_Cfg.FTSysStaUsrValue;
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditUrl", "W_SETxText : " + oEx.Message); }
        }
        #endregion End Function

        #region Method/Events
        /// <summary>
        /// Paint Background
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                using (SolidBrush oBrush = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(oBrush, e.ClipRectangle);
                    oBrush.Dispose();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditUrl", "OnPaintBackground : " + oEx.Message); }
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditUrl", "ocmBack_Click : " + oEx.Message); }
        }

        private void wEditUrl_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditUrl", "wEditUrl_FormClosing : " + oEx.Message); }
        }

        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditUrl", "ocmKB_Click : " + oEx.Message); }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("UPDATE TSysConfig WITH(ROWLOCK)");
                oSql.AppendLine("SET FTSysStaUsrValue = '"+ otbUrl.Text.Trim() +"'");
                oSql.AppendLine("WHERE FTSysCode = '"+ oW_Cfg.FTSysCode +"'");
                oDB.C_SETxDataQuery(oSql.ToString());
                tW_Url = otbUrl.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditUrl", "ocmKB_Click : " + oEx.Message); }
        }
        private void ocmChkUrl_Click(object sender, EventArgs e)
        {
            cClientService oCall = new cClientService();
            HttpResponseMessage oResponse = new HttpResponseMessage();
            string tResponse = "";
            cmlResIsOnline oResIsOnline = new cmlResIsOnline();
            try
            {
                //oResponse = oCall.C_POSToInvoke(otbUrl.Text + @"/CheckOnline/IsOnline");
                //if (oResponse.StatusCode == HttpStatusCode.OK)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgConnUrlSuccess"), 1);
                //}
                //else
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotConnUrl"), 1);
                //}


                //*Arm 63-01-31 - Check Online ผ่าน URL 
                tResponse = oCall.C_GETtInvoke(otbUrl.Text + @"/CheckOnline/IsOnline");
                oResIsOnline = JsonConvert.DeserializeObject<cmlResIsOnline>(tResponse);
                
                if (oResIsOnline.rtCode == "001" || oResIsOnline.rtCode == "1")     //API2PSMaster : Code Success = 001, API2PSSale : Code Success = 1
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgConnUrlSuccess"), 1);
                }
                else
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotConnUrl"), 1);
                }
            }
            catch (Exception oEx)
            {
                new cSP().SP_SHWxMsg(oEx.Message.ToString(), 2);
            }
        }
        #endregion End Method/Events

        
    }
}
