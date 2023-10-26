using AdaPos.Class;
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
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using AdaPos.Models.Webservice.Required.SaleOrder;
using System.Net.Http;
using AdaPos.Models.Webservice.Respond.SaleOrder;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Respond;

namespace AdaPos.Popup.wSale
{
    public partial class wReferSO : Form
    {
        private ResourceManager oW_Resource;
        private string tW_DocNo;
        private int nW_SelectRow;
        cSP oW_SP;
        public wReferSO(string ptCstCode)
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                W_SETxDesign();
                W_SETxText();

                new cLog().C_WRTxLog("wReferSO", "Call API2ARDoc ==> OrdersByList Start..."); //*Arm 63-05-11
                W_GETxOrdersByList(ptCstCode);  // Get SO
                new cLog().C_WRTxLog("wReferSO", "Call API2ARDoc ==> OrdersByList End..."); //*Arm 63-05-11

                this.KeyPreview = true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "wReferSO : " + oEx.Message);
            }
        }

        #region Function/Method

        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;

                ogdSo.EnableHeadersVisualStyles = false;
                //ogdSo.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdSo); //*Net 63-03-03 Set Design Gridview


            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "W_SETxDesign : " + oEx.Message);
            }
        }

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

                olaTitle.Text = oW_Resource.GetString("tTitleReferSO");
                otbBchCode.HeaderText = oW_Resource.GetString("tReferSOBchCode");
                otbDocNo.HeaderText = oW_Resource.GetString("tReferSODocNo"); ;
                otbDocDate.HeaderText = oW_Resource.GetString("tReferSODate");
                otbGrand.HeaderText = oW_Resource.GetString("tAmt");
                otbUsrKey.HeaderText = oW_Resource.GetString("tReferSOUsrKey");
                otbUsrApv.HeaderText = oW_Resource.GetString("tReferSOUsrApv");
                otbCshOrCrd.HeaderText = oW_Resource.GetString("tReferSOCshOrCrd");

                olaCstName.Text = cVB.tVB_CstName;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "W_SETxText : " + oEx.Message);
            }
        }

        public void W_GETxOrdersByList(string ptCstCode)
        {
            string tCshOrCrd = ""; //*Arm 63-04-21
            try
            {
                Cursor.Current = Cursors.WaitCursor;  //*Arm 63-05-11

                // Check API2ARDoc
                if (string.IsNullOrEmpty(cVB.tVB_API2ARDoc)) 
                {
                    // ไม่ได้ Config API2ARDoc
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlARDocNotDefine"), 3);
                }
                else
                {
                    if (W_CHKxUrl() == false) { this.Close(); }

                    cmlReqOrdersByList oReqByList = new cmlReqOrdersByList();
                    oReqByList.ptBchCode = cVB.tVB_BchCode;
                    oReqByList.ptCstCode = ptCstCode;
                    string tJSonCall = JsonConvert.SerializeObject(oReqByList);

                    string tUrl = cVB.tVB_API2ARDoc;

                    cClientService oCall = new cClientService();
                    oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                    HttpResponseMessage oRep = new HttpResponseMessage();
                    try
                    {
                        oRep = oCall.C_POSToInvoke(tUrl + "/SaleOrder/OrdersByList", tJSonCall);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("wReferSO", "W_GETxOrdersByList : " + oEx.Message);
                        return;
                    }


                    if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                        cmlResOrdersByList oRes = JsonConvert.DeserializeObject<cmlResOrdersByList>(tJSonRes);
                        
                        switch (oRes.rtCode)
                        {
                            case "001":

                                if (oRes.raItems.Count > 0)
                                {
                                    foreach (cmlDataOrdersByList oData in oRes.raItems)
                                    {
                                        //*Arm 63-04-21
                                        if(!string.IsNullOrEmpty(oData.rtCshOrCrd))
                                        {
                                            if (oData.rtCshOrCrd == "2")
                                            {
                                                tCshOrCrd = oW_Resource.GetString("tPayCrd");
                                            }
                                            else
                                            {
                                                tCshOrCrd = oW_Resource.GetString("tPayCsh");
                                            }
                                        }
                                        
                                        //+++++++++++++++++

                                        //Add So ลง DataGridview
                                        ogdSo.Rows.Add(oData.rtBchCode, oData.rtDocNo, string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(oData.rdDocDate)), oData.rcGrand, oData.rtUsrKey, oData.rtUsrApv, tCshOrCrd);
                                    }

                                }
                                break;

                            case "800":
                                // ไม่พบข้อมูล SO
                                oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoData"), 1);
                                this.Close();
                                break;

                            default:
                                //ERROR
                                new cLog().C_WRTxLog("wReferSO", "W_GETxOrdersByList : " + oRes.rtDesc);
                                break;
                        }

                        Cursor.Current = Cursors.Default;  //*Arm 63-05-11
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "W_GETxOrdersByList : " + oEx.Message);
            }
        }

        /// <summary>
        /// *Arm 63-03-22
        /// Check Online ผ่าน URL 
        /// </summary>
        /// <returns></returns>
        private bool W_CHKxUrl()
        {
            cClientService oCall = new cClientService();
            HttpResponseMessage oResponse = new HttpResponseMessage();
            string tResponse = "";
            cmlResIsOnline oResIsOnline = new cmlResIsOnline();
            try
            {
                
                //*Arm 63-01-31 - Check Online ผ่าน URL 
                tResponse = oCall.C_GETtInvoke(cVB.tVB_API2ARDoc + @"/CheckOnline/IsOnline");
                oResIsOnline = JsonConvert.DeserializeObject<cmlResIsOnline>(tResponse);

                if (oResIsOnline.rtCode == "001" ) 
                {
                    //new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgConnUrlSuccess"), 1);
                    return true;
                }
                else
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotConnUrl"), 1);
                    return false;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "W_CHKxUrl : " + oEx.Message);
                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotConnUrl"), 1);
                return false;
            }
        }

        public void W_GETxOrdersByDoc(string ptDocNo)
        {
            cmlPdtOrder oPdt;
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_API2ARDoc))
                {
                    // ไม่ได้ Config API2ARDoc
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlARDocNotDefine"), 3);
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;

                    cmlReqOrdersByDoc oReqByDoc = new cmlReqOrdersByDoc();
                    oReqByDoc.ptBchCode = cVB.tVB_BchCode;
                    oReqByDoc.ptDocNo = ptDocNo;
                    string tJSonCall = JsonConvert.SerializeObject(oReqByDoc);

                    string tUrl = cVB.tVB_API2ARDoc;

                    cClientService oCall = new cClientService();
                    oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                    HttpResponseMessage oRep = new HttpResponseMessage();
                    try
                    {
                        oRep = oCall.C_POSToInvoke(tUrl + "/SaleOrder/OrdersByDoc", tJSonCall);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("wReferSO", "W_GETxOrdersByDoc : " + oEx.Message);
                        return;
                    }


                    if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                        cmlResOrdersByDoc oRes = JsonConvert.DeserializeObject<cmlResOrdersByDoc>(tJSonRes);

                        switch (oRes.rtCode)
                        {
                            case "001":

                                //*Arm 63-03-05
                                cVB.oVB_ReferSO = new cmlDataOrdersByDoc();
                                cVB.oVB_ReferSO = oRes.roItem;
                                //+++++++++++++++++

                                cVB.tVB_RefDocNo = oRes.roItem.aoTARTSoHD[0].rtXshDocNo;

                                //*Arm 63-05-11 Comment Code
                                //if (oRes.roItem.aoTARTSoDT.Count > 0)
                                //{
                                //    cVB.aoVB_PdtReferSO = new List<cmlPdtOrder>();
                                //    foreach (cmlResInfoTARTSoDT oData in oRes.roItem.aoTARTSoDT)
                                //    {
                                //        oPdt = new cmlPdtOrder();
                                //        oPdt.tPdtCode = oData.rtPdtCode;
                                //        oPdt.tPdtName = oData.rtXsdPdtName;
                                //        oPdt.tBarcode = oData.rtXsdBarCode;
                                //        oPdt.tUnit = oData.rtPunName;
                                //        oPdt.cSetPrice = Convert.ToDecimal(oData.rcXsdSetPrice);
                                //        oPdt.cQty = Convert.ToDecimal(oData.rcXsdQty);
                                //        oPdt.cFactor = Convert.ToDecimal(oData.rcXsdFactor);
                                //        oPdt.tStaPdt = oData.rtXsdStaPdt;
                                        
                                //        cVB.aoVB_PdtReferSO.Add(oPdt);
                                //    }
                                //}
                                break;

                            case "800":
                                // ไม่พบข้อมูล SO
                                oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoData"), 1);
                                this.Close();
                                break;

                            default:
                                //ERROR
                                new cLog().C_WRTxLog("wReferSO", "W_GETxOrdersByDoc : " + oRes.rtDesc);
                                break;
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "W_GETxOrdersByDoc : " + oEx.Message);
            }
        }
        
        #endregion  End Function/Method

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                new cLog().C_WRTxLog("wReferSO", "Call API2ARDoc ==> W_GETxOrdersByDoc  Start..."); //*Arm 63-05-11
                tW_DocNo = ogdSo.CurrentRow.Cells["otbDocNo"].Value.ToString();
                W_GETxOrdersByDoc(tW_DocNo);
                this.DialogResult = DialogResult.OK;
                this.Close();
                new cLog().C_WRTxLog("wReferSO", "Call API2ARDoc ==> W_GETxOrdersByDoc  End..."); //*Arm 63-05-11
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "ocmAccept_Click : " + oEx.Message);
            }
        }

        private void ogdSo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;
                if (e.RowIndex < 0) return;
                
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "ogdSo_CellClick : " + oEx.Message);
            }
            
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "ocmBack_Click : " + oEx.Message);
            }
        }

        private void wReferSO_KeyDown(object sender, KeyEventArgs e)
        {
            object oSender = new object();
            EventArgs oEvent = new EventArgs();
            
            switch (e.KeyCode)
            {
                case Keys.F10:
                    ocmAccept_Click(oSender, oEvent);
                    
                    break;

                case Keys.Up:
                   
                    if (ogdSo.SelectedRows[0].Index > 0)
                        ogdSo.Rows[ogdSo.SelectedRows[0].Index - 1].Selected = true;
                    break;

                case Keys.Down:
                    if (ogdSo.Rows.Count - 1 > ogdSo.SelectedRows[0].Index)
                    {
                        ogdSo.Rows[ogdSo.SelectedRows[0].Index + 1].Selected = true;
                    }

                    break;

                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void ogdSo_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            object oSender = new object();
            EventArgs oEvent = new EventArgs();

            try
            {
                if (e.ColumnIndex < 0) return;
                if (e.RowIndex < 0) return;
                ocmAccept_Click(oSender, oEvent);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferSO", "ogdSo_CellMouseDoubleClick : " + oEx.Message);
            }
        }
    }
}
