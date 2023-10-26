using AdaPos.Class;
using AdaPos.Models.Webservice.Required;
using AdaPos.Models.Webservice.Respond;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wSpotCheck
{
    public partial class wSpotCheckHistory : Form
    {
        #region Variable

        private ResourceManager oW_Resource;
        private cSP oW_SP;

        #endregion End Variable

        public wSpotCheckHistory()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
                W_GETxHistory();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheckHistory", "wSpotCheckHistory : " + oEx.Message); }
        }

        /// <summary>
        /// Close popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheckHistory", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSpotCheckHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheckHistory", "wSpotCheckHistory_FormClosing : " + oEx.Message); }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;

                //ogdHistory.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdHistory); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheckHistory", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set tex form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resSpotCheck_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSpotCheck_EN));
                        break;
                }

                olaTitleHistory.Text = oW_Resource.GetString("tHistory");
                otbTitleAmt.HeaderText = cVB.oVB_GBResource.GetString("tAmount");
                otbTitleDatetime.HeaderText = cVB.oVB_GBResource.GetString("tDatetime");
                otbTitleDocNo.HeaderText = cVB.oVB_GBResource.GetString("tDocNo");
                otbTitlePos.HeaderText = cVB.oVB_GBResource.GetString("tPos");
                otbTitleSeq.HeaderText = cVB.oVB_GBResource.GetString("tSeq");
                otbTitleType.HeaderText = cVB.oVB_GBResource.GetString("tType");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheckHistory", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSpotCheckHistory_Shown(object sender, EventArgs e)
        {
            try
            {
                ogdHistory.Focus();
                ogdHistory.ClearSelection();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheckHistory", "wSpotCheckHistory_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Get history
        /// </summary>
        private void W_GETxHistory()
        {
            bool bChkConn;
            string tJson;
            cEncryptDecrypt oEncryptDecrypt;
            cmlReqCardHistory oReqHistory;
            cClientService oCall;
            HttpResponseMessage oResponse = null;
            cmlResCardHistory oResHistory;
            int nCount = 1;

            try
            {
                //bChkConn = oW_SP.SP_CHKbConnection();
                if (String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                {
                    return;
                }
                bChkConn = oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline");   // Connect internet  //*Em 63-03-05


                if (bChkConn)
                {
                    //if (!string.IsNullOrEmpty(cVB.tVB_URLStore))
                    //{
                    //    oEncryptDecrypt = new cEncryptDecrypt();

                    //    // Set parameter.
                    //    oReqHistory = new cmlReqCardHistory();
                    //    oReqHistory.ptBchCode = cVB.tVB_BchCode;
                    //    oReqHistory.pnQty = 0;
                    //    oReqHistory.ptCrdNo = cVB.tVB_CardNo;
                    //    oReqHistory.ptSort = "0";

                    //    // 2. เอา ตัวแปรในข้อที่ 1 มาทำเป็น JSON เก็บไว้ในตัวแปร string
                    //    tJson = JsonConvert.SerializeObject(oReqHistory);

                    //    // Call API service.
                    //    oCall = new cClientService();
                    //    oResponse = new HttpResponseMessage();

                    //    try
                    //    {
                    //        // 4. call web service พร้อมส่ง parameter string JSON ในข้อ 2
                    //        oResponse = oCall.C_POSToInvoke(cVB.tVB_URLStore + "/V1/SpotCheck/Check", tJson);
                    //    }
                    //    catch
                    //    {
                    //        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 2);   // ไม่สามารถเชื่อมต่อ Web Service ได้

                    //        new cLog().C_WRTxLog("wSpotCheck", "W_GETxSpotCheck : " + oResponse.StatusCode.ToString());
                    //        return;
                    //    }

                    //    // Check response.
                    //    if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    //    {
                    //        string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;

                    //        if (!string.IsNullOrEmpty(tJsonResult))
                    //        {
                    //            // 7. แปลง JSON เป็น class ตามเอกสาร
                    //            oResHistory = JsonConvert.DeserializeObject<cmlResCardHistory>(tJsonResult);

                    //            switch (oResHistory.rtCode)
                    //            {
                    //                case "1":   // 1 : Success
                    //                    foreach (cmlResCardHistoryList oHisty in oResHistory.roCrdHistory)
                    //                    {
                    //                        ogdHistory.Rows.Add(nCount, oHisty.ptDocRef, oHisty.ptDocDate, oHisty.ptType, oHisty.ptPos, oHisty.ptAmount);
                    //                        nCount++;
                    //                    }

                    //                    break;

                    //                case "701": // 701 : Validate parameter model false
                    //                    Debug.WriteLine("Error : Parameter is null !!");
                    //                    break;

                    //                case "712": // 712 : Not found card
                    //                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsg712"), 3);
                    //                    break;

                    //                case "802": // 802 : format data in correct
                    //                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsg802"), 3);
                    //                    break;

                    //                case "900": // 900 : service process false
                    //                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsg900"), 3);
                    //                    break;

                    //                case "904": // 904 : Key not allowed to use method
                    //                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsg904"), 3);
                    //                    break;

                    //                case "905": // 905 : Cannot connect database
                    //                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsg905"), 3);
                    //                    break;

                    //                case "906": // 906 : This time not allowed to use method
                    //                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsg906"), 3);
                    //                    break;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheckHistory", "W_GETxHistory : " + oEx.Message); }
            finally
            {
                if (oResponse != null)
                    oResponse.Dispose();

                tJson = null;
                oEncryptDecrypt = null;
                oReqHistory = null;
                oCall = null;
                oResponse = null;
                oResHistory = null;
                oW_SP.SP_CLExMemory();
            }
        }
    }
}
