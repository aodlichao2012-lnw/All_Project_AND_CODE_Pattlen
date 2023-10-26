using AdaPos.Class;
using AdaPos.Models.Webservice.Required.Customer;
using AdaPos.Models.Webservice.Respond.Customer;
using AdaPos.Models.Webservice.Respond.SaleOrder;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.All
{
    public partial class wCstSearch : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_SchMode = 1;
        private int nW_SelectRow = -1;
        private string tW_KbdScreen;
        public wCstSearch(string ptKbdScreen)
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                tW_KbdScreen = ptKbdScreen;
                W_SETxDesign();
                W_SETxText();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "wCstSearch : " + oEx.Message);
            }
        }
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdCst);

                ocmSearch.BackColor = cVB.oVB_ColDark;
                opnHDCstSch.BackColor = cVB.oVB_ColNormal;

                //*Arm 63-05-18 - ปิดช่องรับข้อมูล
                //otbSchCstCode.Enabled = false;
                //otbSchCstName.Enabled = false;
                //otbSchMemCrdNo.Enabled = false;
                //otbSchTaxNo.Enabled = false;
                //+++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "W_SETxDesign : " + oEx.Message);
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

                //Title
                olaTitleCstSearch.Text = oW_Resource.GetString("tTitleCstSearch");

                //Req
                olaSchByCstName.Text = oW_Resource.GetString("tTitleCstSchByCstName");
                olaSchByCstCode.Text = oW_Resource.GetString("tTitleCstSchByCstCode");
                olaSchByTaxNo.Text = oW_Resource.GetString("tTitleCstSchByCstTaxNo");
                olaSchByTel.Text = oW_Resource.GetString("tTitleCstSchByCstTel");
                olaSchByMemCrdNo.Text = oW_Resource.GetString("tTitleCstSchByCstMemCrdNo");
                olaSchByCardID.Text = oW_Resource.GetString("tTitleCstSchByCstCardID");
                
                //Select Mode
                olaCstSearch.Text = oW_Resource.GetString("tTitleCstSchBtnCstSch");
                olaRFIDSesrch.Text = oW_Resource.GetString("tTitleCstSchBtnRFIDSch");

                //Button
                ocmSearch.Text = oW_Resource.GetString("tTitleCstSchBtnSearch");

                // DataGrid
                otbColCstCode.HeaderText = oW_Resource.GetString("tColCstCode");
                otbColCstName.HeaderText = oW_Resource.GetString("tColCstName");
                otbColCstTel.HeaderText = oW_Resource.GetString("tColCstTel");
                otbColCstCrdNo.HeaderText = oW_Resource.GetString("tColCstMemCrdNo");
                otbColCstTaxNo.HeaderText = oW_Resource.GetString("tColCstTaxNo");
                otbColCstCardID.HeaderText = oW_Resource.GetString("tColCstCardID");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "W_SETxText : " + oEx.Message);
            }
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "ocmBack_Click : " + oEx.Message);
            }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_PRCxCallByName("C_KBDxKeyboard");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "ocmShwKb_Click : " + oEx.Message);
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            
            try
            {
                if(nW_SelectRow <0)
                {
                    return;
                }
                else
                {
                    switch (tW_KbdScreen)
                    {
                        case "SALESTD":

                            if (cSale.nC_CntItem > 0) //ถ้ามีการทำรายการอยู่
                            {
                                
                                if (!string.IsNullOrEmpty(cVB.tVB_CstCode)) //เช็คถ้ามีการเลือกลูฏค้าแล้ว
                                {
                                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantSelectCst"), 3);
                                    this.Close();
                                    this.Dispose();
                                    return;
                                }
                                
                                if (!string.Equals(cVB.tVB_PriceGroup, ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value.ToString()))
                                {
                                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgDiffPriLev"), 3);
                                    this.Close();
                                    this.Dispose();
                                    return;
                                }

                            }
                           
                            cVB.tVB_CstCode = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCode.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCode.Name].Value.ToString();
                            cVB.tVB_CstName = ogdCst.Rows[nW_SelectRow].Cells[otbColCstName.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstName.Name].Value.ToString();
                            cVB.tVB_CstTel = ogdCst.Rows[nW_SelectRow].Cells[otbColCstTel.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstTel.Name].Value.ToString();
                            cVB.tVB_PriceGroup = ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value.ToString();
                            if(cVB.oVB_2ndScreen != null) cVB.oVB_2ndScreen.olaGrpCst.Text = ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value.ToString();
                            cVB.tVB_QMemMsgID = "";
                            cVB.bVB_ScanQR = false;

                            cVB.tVB_CstStaAlwPosCalSo = ogdCst.Rows[nW_SelectRow].Cells[otbColCstStaAlwPosCalSo.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstStaAlwPosCalSo.Name].Value.ToString(); //*Arm 63-03-03 อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต(default)
                            cVB.nVB_CstPoint = Convert.ToInt32(ogdCst.Rows[nW_SelectRow].Cells[otbColTxnPntQty.Name].Value); //*Arm 63-03-13 Point
                            cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                            cVB.tVB_MemberCard = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdNo.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdNo.Name].Value.ToString();
                            cVB.tVB_ExpiredDate = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdExpire.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdExpire.Name].Value.ToString();

                            cVB.oVB_Sale.W_SETxTextCst();
                            
                            this.Close();
                            //this.Dispose();
                            break;
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "ocmAccept_Click : " + oEx.Message);
            }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            cmlResCst aoCstSch;
            string tShowDateExpird = "";
            string tFunc = "";
            try
            {
                //cmlResSKCCstByList oResCstByList = new cmlResSKCCstByList();
                //wCstList oCstList = new wCstList(oResCstByList);
                //oCstList.ShowDialog();
                //this.Close();
                if (string.IsNullOrEmpty(otbSchCstName.Text) &&
                   string.IsNullOrEmpty(otbSchCstCode.Text) &&
                   string.IsNullOrEmpty(otbSchCstTel.Text) &&
                   string.IsNullOrEmpty(otbSchCardID.Text) &&
                   string.IsNullOrEmpty(otbSchMemCrdNo.Text) &&
                   string.IsNullOrEmpty(otbSchTaxNo.Text)
                   ) { return; }

                ogdCst.Rows.Clear();
                nW_SelectRow = -1;

                if (nW_SchMode == 1)
                {
                    tFunc = "/Customer/CstSearch";
                }
                else
                {
                    //tFunc = "/Customer/CstSearchRFID";
                }

                cmlReqCstSch oReq = new cmlReqCstSch();
                oReq.ptCstName = otbSchCstName.Text;
                oReq.ptCstCode = otbSchCstCode.Text;
                oReq.ptCstTel = otbSchCstTel.Text;
                oReq.ptCstCardID = otbSchCardID.Text;
                oReq.ptCstCrdNo = otbSchMemCrdNo.Text;
                oReq.ptCstTaxNo = otbSchTaxNo.Text;

                string tJSonCall = JsonConvert.SerializeObject(oReq);

                string tUrl = cVB.tVB_API2PSMaster;

                cClientService oCall = new cClientService();
                oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                HttpResponseMessage oRep = new HttpResponseMessage();
                try
                {
                    oRep = oCall.C_POSToInvoke(tUrl + tFunc, tJSonCall);
                }
                catch (Exception oEx)
                {
                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click/Call API Error : " + oEx.Message);
                    return;
                }


                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    aoCstSch = new cmlResCst();
                    aoCstSch = JsonConvert.DeserializeObject<cmlResCst>(tJSonRes);

                    switch (aoCstSch.rtCode)
                    {
                        case "1":

                            if (aoCstSch.raItems.Count > 0)
                            {

                                foreach (cmlResCstSch oData in aoCstSch.raItems)
                                {
                                    ogdCst.Rows.Add(oData.rtCstCode, oData.rtCstName, oData.rtCstTel, oData.rtCstCrdNo, oData.rtCstTaxNo, oData.rtCstCardID, oData.rtCstEmail, oData.rtCstSex, oData.rdCstDob,
                                        oData.rtPplCodeRet, oData.rtCstDiscRet, oData.rdCstCrdExpire == null ? string.Empty : string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(oData.rdCstCrdExpire)), oData.rtTxnPntQty, oData.rtTxnBuyTotal, oData.rtCstStaAlwPosCalSo);
                                }

                                if (aoCstSch.raItems.Count == 1)
                                {
                                    switch (tW_KbdScreen)
                                    {
                                        case "SALESTD":

                                            nW_SelectRow = 0;

                                            if (cSale.nC_CntItem > 0) //ถ้ามีการทำรายการอยู่
                                            {

                                                if (!string.IsNullOrEmpty(cVB.tVB_CstCode)) //เช็คถ้ามีการเลือกลูฏค้าแล้ว
                                                {
                                                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantSelectCst"), 3);
                                                    this.Close();
                                                    this.Dispose();
                                                    return;
                                                }

                                                if (!string.Equals(cVB.tVB_PriceGroup, ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value.ToString()))
                                                {
                                                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgDiffPriLev"), 3);
                                                    this.Close();
                                                    this.Dispose();
                                                    return;
                                                }

                                            }

                                            cVB.tVB_CstCode = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCode.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCode.Name].Value.ToString();
                                            cVB.tVB_CstName = ogdCst.Rows[nW_SelectRow].Cells[otbColCstName.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstName.Name].Value.ToString();
                                            cVB.tVB_CstCardID = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCardID.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCardID.Name].Value.ToString();
                                            cVB.tVB_CstTel = ogdCst.Rows[nW_SelectRow].Cells[otbColCstTel.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstTel.Name].Value.ToString();
                                            cVB.tVB_PriceGroup = ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value.ToString();
                                            if (cVB.oVB_2ndScreen != null)
                                            {
                                                cVB.oVB_2ndScreen.olaGrpCst.Text = ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value.ToString();
                                            }
                                            cVB.tVB_QMemMsgID = "";
                                            cVB.bVB_ScanQR = false;

                                            cVB.tVB_CstStaAlwPosCalSo = ogdCst.Rows[nW_SelectRow].Cells[otbColCstStaAlwPosCalSo.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstStaAlwPosCalSo.Name].Value.ToString(); //*Arm 63-03-03 อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต(default)
                                            cVB.nVB_CstPoint = Convert.ToInt32(ogdCst.Rows[nW_SelectRow].Cells[otbColTxnPntQty.Name].Value); //*Arm 63-03-13 Point
                                            cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                                            cVB.tVB_MemberCard = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdNo.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdNo.Name].Value.ToString();
                                            cVB.tVB_ExpiredDate = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdExpire.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdExpire.Name].Value.ToString();

                                            cVB.oVB_Sale.W_SETxTextCst();


                                            this.Close();
                                            this.Dispose();

                                            break;
                                    }

                                }

                            }
                            break;

                        case "800":
                            // ไม่พบข้อมูล 
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDataNotFound"), 1);

                            break;

                        default:
                            //ERROR
                            new cLog().C_WRTxLog("wReferSO", "ocmSearch_Click/API Response Error : " + aoCstSch.rtDesc);
                            break;
                    }

                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : " + oEx.Message);
            }
            finally
            {
                aoCstSch = null;
            }
        }

        /// <summary>
        /// *Arm 63-05-18
        /// </summary>
        public void W_PRCxKADSCustomerSearch()
        {
            cmlReqCstSchSKC oReqCst;
            cmlResSKCCstByList oResCstByList;
            cmlResSKCCstByCst oResCstByCst;
            string[] aErrorCode = { "001", "700", "701", "706", "800", "905" };
            string[] aErrorDesc = { "success.", "all parameter is null.", "validate parameter model false.", "product retail price not allow less than 0.", "data not found.", "cannot connect database." };

            cClientService oCall;
            HttpResponseMessage oRep;
            string tJSonCall = "";
            string tUrl = "";
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_APIKADS))
                {
                    //W_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlKADSNotDefine"), 3);
                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click/URL API is Empty ...");
                }
                else
                {
                    if (W_VerifyReq(1) == false) return;

                    oReqCst = new cmlReqCstSchSKC();
                    oReqCst.PhoneNo = otbSchCstTel.Text;
                    oReqCst.TaxID = otbSchCardID.Text;
                    oReqCst.SaleOrg = cVB.tVB_SaleOrg;

                    tJSonCall = JsonConvert.SerializeObject(oReqCst);

                    oCall = new cClientService();
                    oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                    tUrl = cVB.tVB_APIKADS + "";

                    oRep = new HttpResponseMessage();
                    try
                    {
                        oRep = oCall.C_POSToInvoke(tUrl, tJSonCall);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("wCstSearch", "W_PRCxKADSCustomerSearch/Call API Error : " + oEx.Message);
                        return;
                    }

                    if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                        oResCstByList = new cmlResSKCCstByList();
                        oResCstByCst = new cmlResSKCCstByCst();

                        try
                        {
                            oResCstByCst = JsonConvert.DeserializeObject<cmlResSKCCstByCst>(tJSonRes);
                            if (oResCstByCst.ErrorCode == "001")
                            {
                                wCstDetail oCstDetail = new wCstDetail(oResCstByCst);
                                oCstDetail.ShowDialog();
                            }
                            else
                            {
                                string tDesc = "";
                                for (int n = 0; n <= 5; n++)
                                {
                                    if (aErrorCode[n].ToString() == oResCstByCst.ErrorCode)
                                    {
                                        tDesc = aErrorDesc[n].ToString();
                                        break;
                                    }
                                }
                                new cLog().C_WRTxLog("wCstList", "W_PRCxKADSCustomerSearch/API Response Error " + oResCstByCst.ErrorCode + " : " + tDesc);
                            }
                        }
                        catch(Exception oEx)
                        {
                            oResCstByList = JsonConvert.DeserializeObject<cmlResSKCCstByList>(tJSonRes);
                            if (oResCstByCst.ErrorCode == "001")
                            {
                                wCstList oCstList = new wCstList(oResCstByList);
                                oCstList.ShowDialog();
                            }
                            else
                            {
                                string tDesc = "";
                                for (int n = 0; n <= 5; n++)
                                {
                                    if (aErrorCode[n].ToString() == oResCstByCst.ErrorCode)
                                    {
                                        tDesc = aErrorDesc[n].ToString();
                                        break;
                                    }
                                }
                                new cLog().C_WRTxLog("wCstList", "W_PRCxKADSCustomerSearch/API Response Error " + oResCstByCst.ErrorCode + " : " + tDesc);
                            }
                        }
                        
                    }
                }
                this.Close();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "W_PRCxKADSCustomerSearch : " + oEx.Message.ToString());
            }
            finally
            {
                oReqCst = null;
                oResCstByList = null;
                oResCstByCst = null;
                oCall = null;
                oRep = null; 
            }
        }

        /// <summary>
        /// *Arm 63-05-18
        /// ตรวจสอบ Requst 1:CstSch ,2:ChooseCst
        /// </summary>
        /// <param name="pnReq"></param>
        /// <returns></returns>
        public bool W_VerifyReq(int pnReq, string ptKubotaID ="")
        {
            try
            {
                switch(pnReq)
                {
                    case 1:
                        if (string.IsNullOrEmpty(cVB.tVB_SaleOrg))
                        {
                            new cLog().C_WRTxLog("wCstSearch", "W_VerifyReq/Parameter SaleOrg is Empty ...");
                            return false;
                        }

                        if (cVB.tVB_SaleOrg.Length > 4)
                        {
                            new cLog().C_WRTxLog("wCstSearch", "W_VerifyReq/Parameter SaleOrg is Length More than 4 character ...");
                            return false;
                        }

                        if (otbSchCardID.TextLength > 40)
                        {
                            new cLog().C_WRTxLog("wCstSearch", "W_VerifyReq/Parameter TaxID is Length More than 40 character ...");
                            return false;
                        }

                        if (otbSchCstTel.TextLength > 40)
                        {
                            new cLog().C_WRTxLog("wCstSearch", "W_VerifyReq/Parameter PhoneNo is Length More than 40 character ...");
                            return false;
                        }
                        break;

                    case 2:
                        if (string.IsNullOrEmpty(cVB.tVB_SaleOrg))
                        {
                            new cLog().C_WRTxLog("wCstSearch", "W_VerifyReq/Parameter SaleOrg is Empty ...");
                            return false;
                        }
                        if (cVB.tVB_SaleOrg.Length > 4)
                        {
                            new cLog().C_WRTxLog("wCstSearch", "W_VerifyReq/Parameter SaleOrg is Length More than 4 character ...");
                            return false;
                        }
                        if (string.IsNullOrEmpty(ptKubotaID))
                        {
                            new cLog().C_WRTxLog("wCstSearch", "VerifyReq/Parameter PhoneNo is Length More than 40 character ...");
                            return false;
                        }
                        break;
                }
                
                return true;
            }
            catch(Exception oEx)
            {
                return false;
            }
        }

        private void olaCstSearch_Click(object sender, EventArgs e)
        {
            try
            {
                nW_SchMode = 1;
                olaCstSearch.Enabled = true;
                //olaRFIDSesrch.Enabled = true;
                //olaRFIDSesrch.ForeColor = Color.White;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "olaCstSearch_Click : " + oEx.Message);
            }
        }

        private void olaRFIDSesrch_Click(object sender, EventArgs e)
        {
            try
            {
                //nW_SchMode = 2;
                //olaRFIDSesrch.Enabled = false;
                //olaCstSearch.Enabled = true;
                //olaCstSearch.ForeColor = Color.White;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "olaRFIDSesrch_Click : " + oEx.Message);
            }
        }

        private void wCstSearch_Shown(object sender, EventArgs e)
        {
            try
            {
                olaCstSearch_Click(olaCstSearch, null);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "wCstSearch_Shown : " + oEx.Message);
            }
        }

        private void ogdCst_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;
                if (e.RowIndex < 0) return;

                nW_SelectRow = e.RowIndex;
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "wCstSearch_Shown : " + oEx.Message);
            }
        }
    }
}
