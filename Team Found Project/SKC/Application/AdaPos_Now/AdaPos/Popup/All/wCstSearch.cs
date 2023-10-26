using AdaPos.Class;
using AdaPos.Models.Webservice.Required.Customer;
using AdaPos.Models.Webservice.Required.KADS.Customer;
using AdaPos.Models.Webservice.Respond.Customer;
using AdaPos.Models.Webservice.Respond.KADS.Customer;
using AdaPos.Models.Webservice.Respond.SaleOrder;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
                //opnHDCstSch.BackColor = cVB.oVB_ColNormal; //*Arm 63-08-11 Comment Code

                if (cVB.tVB_ApiCstSch_Fmt == "SKC") //*Arm 63-09-03
                {
                    otbSchCstName.Enabled = false;
                    otbSchCstCode.Enabled = false;
                    otbSchCstTel.Enabled = true;
                    otbSchCardID.Enabled = true;
                    otbSchMemCrdNo.Enabled = false;
                    otbSchTaxNo.Enabled = false;
                }
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
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCstSchChoose"), 3); //*Arm 63-08-28
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
                            //if(cVB.oVB_2ndScreen != null) cVB.oVB_2ndScreen.olaGrpCst.Text = ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value.ToString(); //*Arm 63-08-03 Comment Code ปรับตาม Moshi
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
            cClientService oCall = new cClientService(); //*Arm 63-07-20
            HttpResponseMessage oRep; //*Arm 63-07-20
            cmlResCst aoCstSch;
            cmlResCstKAD aoCstKAD;
            string tShowDateExpird = "";
            string tFunc = "";
            List<KeyValuePair<string, string>> aHeader;
            try
            {
                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Start Click", cVB.bVB_AlwPrnLog);

                if (string.IsNullOrEmpty(otbSchCstName.Text) &&
                  string.IsNullOrEmpty(otbSchCstCode.Text) &&
                  string.IsNullOrEmpty(otbSchCstTel.Text) &&
                  string.IsNullOrEmpty(otbSchCardID.Text) &&
                  string.IsNullOrEmpty(otbSchMemCrdNo.Text) &&
                  string.IsNullOrEmpty(otbSchTaxNo.Text)
                  )
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCstSchReq"), 3); //*Arm 63-08-28
                    return;
                }

                Cursor.Current = Cursors.WaitCursor; //*Arm 63-07-17
                
                // กำหนดค่าเพื่อรอการเช็คจาก Config จึงกำหนดค่าเป็น 1 ก่อนเพื่อให้ Dev แบบ Full Loop
                int nOption = 1;
                #region STD KAD
                //if (!string.IsNullOrEmpty(cVB.tVB_APIKADS))
                if (!string.IsNullOrEmpty(cVB.tVB_ApiCstSch) && cVB.tVB_ApiCstSch_Fmt == "SKC") //*Arm 63-08-09
                {
                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Search By Service KADS(SKC).", cVB.bVB_AlwPrnLog); 
                    //*Arm 63-08-14 - ปรับใหม่
                    string tReqParam = "";

                    cmlResCstKAD oCstSch = new cmlResCstKAD();
                    //if (!string.IsNullOrEmpty(otbSchCstTel.Text))
                    //{
                    //    //ถ้าระบุเบอร์โทร
                    //    tReqParam = otbSchCstTel.Text;
                    //    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Search by Phone number=" + otbSchCstTel.Text, cVB.bVB_AlwPrnLog);
                    //}
                    //else
                    //{
                    //    //ถ้าไม่ระบุเบอร์โทร เช็คเลขบัตรประชาชน
                    //    if (!string.IsNullOrEmpty(otbSchCardID.Text)) tReqParam = otbSchCardID.Text;
                    //    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Search by CardID=" + otbSchCardID.Text, cVB.bVB_AlwPrnLog);
                    //}
                    
                    //if (string.IsNullOrEmpty(tReqParam))
                    //{
                    //    //ไม่ได้กรอกข้อมูล
                    //    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : No enter data for search.", cVB.bVB_AlwPrnLog);
                    //    return;
                    //}
                    if(string.IsNullOrEmpty(otbSchCstTel.Text) && string.IsNullOrEmpty(otbSchCardID.Text))
                    {
                        //ไม่ได้กรอกข้อมูล
                        new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : No enter data for search.", cVB.bVB_AlwPrnLog);
                        return;
                    }
                    else
                    {
                        new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_PRCtServiceCstSearch start... ", cVB.bVB_AlwPrnLog);
                        //oCstSch = cServiceKADS.C_PRCtServiceCstSearch(tReqParam);   //ค้นหาลูกค้า
                        oCstSch = cServiceKADS.C_PRCtServiceCstSearch(otbSchCstTel.Text, otbSchCardID.Text);   //ค้นหาลูกค้า
                        new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_PRCtServiceCstSearch end... ", cVB.bVB_AlwPrnLog);

                        if (oCstSch != null )
                        {
                            if (oCstSch.d != null && oCstSch.d.results.Count > 0)
                            {
                                if (oCstSch.d.results.Count == 1) // Response 1 รายการ
                                {
                                    foreach (cmlResKunnr oResKunnr in oCstSch.d.results)
                                    {
                                        if (oResKunnr.BUGroup == "ZAR1")
                                        {
                                            new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : BUGroup = " + oResKunnr.BUGroup, cVB.bVB_AlwPrnLog);
                                            //  BUGroup เป็น "ZAR1"
                                            if (oResKunnr.PrivilegePointSet != null && oResKunnr.PrivilegePointSet.results != null && oResKunnr.PrivilegePointSet.results.Count > 0)
                                            {
                                                //กรณีมีโควต้า บันทึกลง Table Temp
                                                string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oResKunnr.PrivilegePointSet.results);
                                                DataTable odtPrivil = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);

                                                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_INTxInsertPrivilege2Tmp start...", cVB.bVB_AlwPrnLog);
                                                cServiceKADS.C_INTxInsertPrivilege2Tmp(odtPrivil);
                                                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_INTxInsertPrivilege2Tmp end...", cVB.bVB_AlwPrnLog);
                                            }

                                            wCstDetail oCstDetail = new wCstDetail();
                                            oCstDetail.tW_CstName = oResKunnr.Title + " " + oResKunnr.FirstName + " " + oResKunnr.LastName;
                                            oCstDetail.tW_KubotaID = oResKunnr.KubotaID;
                                            oCstDetail.nW_CstPoint = oResKunnr.Point;
                                            oCstDetail.tW_Kunnr = oResKunnr.CustomerCode;
                                            oCstDetail.W_DATxLoadCst(oResKunnr.KubotaID);
                                            oCstDetail.tW_Membership = oResKunnr.Membership;
                                            oCstDetail.tW_PhoneNo = oResKunnr.PhoneNo;
                                            oCstDetail.tW_TaxID = oResKunnr.TaxID;

                                            new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Open wCstDetail", cVB.bVB_AlwPrnLog);
                                            oCstDetail.ShowDialog();
                                            if (oCstDetail.DialogResult == DialogResult.OK)
                                            {
                                                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Close.", cVB.bVB_AlwPrnLog);
                                                this.Close();
                                            }
                                        }
                                        else
                                        {
                                            //BUGroup เป็น "ZAR6"
                                            wCstDetail oCstDetail = new wCstDetail();   //*Arm 63-08-31
                                            oCstDetail.tW_KubotaID = oResKunnr.CustomerCode;   //*Arm 63-08-31
                                            
                                            cmlResCreateCustomerCode oCst = new cmlResCreateCustomerCode();

                                            new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_PRCoBUGroup6 start...", cVB.bVB_AlwPrnLog);
                                            oCst = cServiceKADS.C_PRCoBUGroup6(oResKunnr); // สร้างลูกค้า
                                            new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_PRCoBUGroup6 end...", cVB.bVB_AlwPrnLog);

                                            if (oCst != null) // Retrun BUGroup มาเป็น "ZAR1"  (สำร็จ)
                                            {
                                                //Arm 63-08-20 Comment Code
                                                //if (oCst.d.PrivilegePointSet != null && oCst.d.PrivilegePointSet.results != null && oCst.d.PrivilegePointSet.results.Count > 0)
                                                //{
                                                //    //กรณีมีโควต้า บันทึกลง Table Temp
                                                //    string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oCst.d.PrivilegePointSet.results);
                                                //    DataTable odtPrivil = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);

                                                //    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_INTxInsertPrivilege2Tmp start...", cVB.bVB_AlwPrnLog);
                                                //    cServiceKADS.C_INTxInsertPrivilege2Tmp(odtPrivil);
                                                //    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_INTxInsertPrivilege2Tmp end...", cVB.bVB_AlwPrnLog);
                                                //}

                                                //wCstDetail oCstDetail = new wCstDetail();
                                                oCstDetail.tW_CstName = oCst.d.Title + " " + oCst.d.FirstName + " " + oCst.d.LastName;
                                                //oCstDetail.tW_KubotaID = oCst.d.KubotaID;
                                                oCstDetail.nW_CstPoint = oCst.d.Point;
                                                oCstDetail.tW_Kunnr = oCst.d.CustomerCode;
                                                //oCstDetail.W_DATxLoadCst(oCst.d.KubotaID);
                                                oCstDetail.W_DATxLoadCst(oCstDetail.tW_KubotaID); //*Arm 63-08-31
                                                oCstDetail.tW_Membership = oCst.d.Membership;
                                                oCstDetail.tW_PhoneNo = oCst.d.PhoneNo;
                                                oCstDetail.tW_TaxID = oCst.d.TaxID;

                                                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Open wCstDetail", cVB.bVB_AlwPrnLog);
                                                oCstDetail.ShowDialog();
                                                if (oCstDetail.DialogResult == DialogResult.OK)
                                                {
                                                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Close.", cVB.bVB_AlwPrnLog);
                                                    this.Close();
                                                }
                                            }
                                            //else oCst = null = สร้างไม่สำเร็จ
                                        }
                                    }
                                }
                                else
                                {
                                    //Return มากกว่า 1 รายการ
                                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Response more then 1 ", cVB.bVB_AlwPrnLog);
                                    wCstList oCstList = new wCstList(tW_KbdScreen);
                                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : wCstList Load data.", cVB.bVB_AlwPrnLog);
                                    oCstList.W_DATxLoadCst(oCstSch);
                                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Open wCstList", cVB.bVB_AlwPrnLog);
                                    oCstList.ShowDialog();
                                    if (oCstList.DialogResult == DialogResult.OK)
                                    {
                                        new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Close.", cVB.bVB_AlwPrnLog);
                                        this.Close();
                                    }
                                }
                            }
                            else
                            {
                                // ไม่พบข้อมูล 
                                oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDataNotFound"), 1);
                                return;
                            }
                        }
                        else
                        {
                            // ไม่พบข้อมูล 
                            //oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDataNotFound"), 1);
                            return;
                        }
                    }
                    //+++++++++++++

                    //if (!string.IsNullOrEmpty(otbSchCstCode.Text) || !string.IsNullOrEmpty(otbSchCstTel.Text) || !string.IsNullOrEmpty(otbSchCardID.Text))
                    //{
                    //    if (!string.IsNullOrEmpty(otbSchCstCode.Text))
                    //    {
                    //        // /sap/opu/odata/SAP/ZDP_GWSRV008_SRV/CustomerSet 
                    //        tFunc = String.Format("('{0}')", otbSchCstCode.Text);
                    //    }
                    //    else if (!string.IsNullOrEmpty(otbSchCstTel.Text))
                    //    {
                    //        // /sap/opu/odata/SAP/ZDP_GWSRV008_SRV/CustomerSet
                    //        //tFunc = String.Format("/sap/opu/odata/SAP/ZDP_GWSRV008_SRV/CustomerSet?$filter=PhoneSearch eq '{0}'&$expand=PrivilegePointSet", otbSchCstTel.Text);
                    //        tFunc = String.Format("?$filter=PhoneSearch eq '{0}'&$expand=PrivilegePointSet", otbSchCstTel.Text); //*Arm 63-06-26
                    //    }
                    //    else if (!string.IsNullOrEmpty(otbSchCardID.Text))
                    //    {
                    //        // /sap/opu/odata/SAP/ZDP_GWSRV008_SRV/CustomerSet
                    //        //tFunc = String.Format("/sap/opu/odata/SAP/ZDP_GWSRV008_SRV/CustomerSet?$filter=TxId eq '{0}'&$expand=PrivilegePointSet", otbSchCardID.Text);
                    //        tFunc = String.Format("?$filter=TaxID eq '{0}'&$expand=PrivilegePointSet", otbSchCardID.Text);
                    //    }
                    //    else
                    //    {
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    return;
                    //}

                    //ogdCst.Rows.Clear();
                    //nW_SelectRow = -1;
                                      
                    ////string tUrl = cVB.tVB_APIKADS;
                    //string tUrl = cVB.tVB_ApiCstSch; //*Arm 63-08-09

                    //new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call APIKads ("+ tFunc + ") Start ", cVB.bVB_AlwPrnLog);
                    //oCall = new cClientService();
                    ////oCall = new cClientService("Authorization", cVB.tVB_KADSAuth);
                    //oCall = new cClientService("Authorization", cVB.tVB_ApiCstSch_Auth);//*Arm 63-08-09

                    ////HttpResponseMessage oRep = new HttpResponseMessage();
                    //new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : URL :" + tUrl, cVB.bVB_AlwPrnLog); //*Arm 63-08-03

                    //new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call API(3) Search CstCode Start", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //oRep = new HttpResponseMessage(); //*Arm 63-07-20
                    //try
                    //{
                    //    oRep = oCall.C_GEToInvoke(tUrl + tFunc);
                    //    oRep.EnsureSuccessStatusCode();
                    //}
                    //catch (HttpRequestException oEx)
                    //{
                    //    new cSP().SP_SHWxMsg("Error : " + oEx.Message.ToString(), 2);
                    //    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call API(3) Search CstCode Error /" + oEx.Message.ToString()); //*Arm 63-08-03
                    //    return;
                    //}
                    //catch (Exception oEx)
                    //{
                    //    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call API(3) Search CstCode Error /" + oEx.Message.ToString()); //*Arm 63-08-03
                    //    return;
                    //}
                    //new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call API(3) Search CstCode End.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03



                    //new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call API2PSMaster (/Customer/CstSearch) End ", cVB.bVB_AlwPrnLog);

                    //if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    //{

                    //    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    //    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call API(3) Search CstCode Response :" + tJSonRes, cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //    aoCstKAD = new cmlResCstKAD();
                    //    aoCstKAD = JsonConvert.DeserializeObject<cmlResCstKAD>(tJSonRes);
                    //    DataTable oDbTbl = new DataTable();
                        
                    //    // Check Kunnr มีแค่ 1 รายการ
                    //    if (aoCstKAD.d.results.Count == 1)
                    //    {
                    //        wCstDetail oCstDetail = new wCstDetail();
                    //        foreach (cmlResKunnr oResKunnr in aoCstKAD.d.results)
                    //        {
                    //            if (oResKunnr.BUGroup == "ZAR6") //*Arm 63-07-20
                    //            // Check กรณี Kunnr ว่างต้องส่ง KubotaID ไป Url เพื่อขอข้อมูลอีกครั้ง
                    //            //if (string.IsNullOrEmpty(oResKunnr.KubotaID))
                    //            {
                    //                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : BUGroup == ZAR6 (Customer No CstCode)", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : API(3) Create CstCode Start", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                oRep = null;

                    //                //*Arm 63-07-20
                    //                cmlReqCustomerCreate oReqCst = new cmlReqCustomerCreate();
                    //                oReqCst.CustomerCode = oResKunnr.CustomerCode;
                    //                oReqCst.Title = oResKunnr.Title;
                    //                oReqCst.FirstName = oResKunnr.FirstName;
                    //                oReqCst.LastName = oResKunnr.LastName;
                    //                oReqCst.Titlee = oResKunnr.Titlee;
                    //                oReqCst.Namee = oResKunnr.Namee;
                    //                oReqCst.Surnamee = oResKunnr.Surnamee;
                    //                oReqCst.Addr = oResKunnr.Addr;
                    //                oReqCst.Soi = oResKunnr.Soi;
                    //                oReqCst.Street = oResKunnr.Street;
                    //                oReqCst.District = oResKunnr.District;
                    //                oReqCst.City = oResKunnr.City;
                    //                oReqCst.Province = oResKunnr.Province;
                    //                oReqCst.Gender = oResKunnr.Gender;
                    //                oReqCst.Birth = oResKunnr.Birth;
                    //                oReqCst.Email = oResKunnr.Email;
                    //                oReqCst.Mobile = oResKunnr.Mobile;
                    //                oReqCst.Remark = oResKunnr.Remark;
                    //                string tJsonCall = JsonConvert.SerializeObject(oReqCst);
                                    
                    //                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_PRCxCreateCustomer Start.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_PRCxCreateCustomer API(3) Create CstCode/Request : " + tJsonCall, cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                tJSonRes = C_PRCxCreateCustomer(tJsonCall);
                    //                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_PRCxCreateCustomer API(3) Create CstCode/Response : " + tJSonRes, cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : C_PRCxCreateCustomer End.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                                    
                    //                if (!string.IsNullOrEmpty(tJSonRes))
                    //                {
                    //                    cmlResCreateCustomerCode oCst = new cmlResCreateCustomerCode();
                    //                    oCst = JsonConvert.DeserializeObject<cmlResCreateCustomerCode>(tJSonRes);
                    //                    DataTable oDbTblKubId = new DataTable();

                    //                    if(oCst != null && oCst.d != null && oCst.d.BUGroup == "ZAR1")
                    //                    //if(oCst != null && oCst.d != null )
                    //                    {
                    //                        if (oCst.d.PrivilegePointSet != null &&  oCst.d.PrivilegePointSet.results != null && oCst.d.PrivilegePointSet.results.Count > 0)
                    //                        {
                    //                            string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oCst.d.PrivilegePointSet.results);
                    //                            oDbTblKubId = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);
                    //                            W_INSxPrivilege(oDbTblKubId);
                    //                        }
                    //                        oCstDetail.tW_CstName = oCst.d.Title + " " + oCst.d.FirstName + " " + oCst.d.LastName;
                    //                        oCstDetail.tW_KubotaID = oCst.d.KubotaID;
                    //                        oCstDetail.nW_CstPoint = oCst.d.Point;
                    //                        oCstDetail.tW_Kunnr = oCst.d.CustomerCode;     //*Arm 63-06-26
                    //                        oCstDetail.W_DATxLoadCst(oCst.d.KubotaID);
                    //                        oCstDetail.tW_Membership = oCst.d.Membership; //*Arm 63-08-03
                    //                        oCstDetail.tW_PhoneNo = oCst.d.PhoneNo;         //*Arm 63-08-11
                    //                        oCstDetail.tW_TaxID = oCst.d.TaxID;             //*Arm 63-08-11
                    //                        oCstDetail.ShowDialog();
                    //                        if (oCstDetail.DialogResult == DialogResult.OK)
                    //                        {
                    //                            this.Close();
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgPrcCreateCstCode")+ " (BUGroup : " + oCst.d.BUGroup +")", 3); //*Arm 63-08-09
                    //                        new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click :Create Customer  Retrun BUGroup == ZAR6 (Customer No CstCode)", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                    }
                    //                }
                                    

                    //            }
                    //            else
                    //            {
                    //                string tJsonPrivil = Newtonsoft.Json.JsonConvert.SerializeObject(oResKunnr.PrivilegePointSet.results);
                    //                oDbTbl = JsonConvert.DeserializeObject<DataTable>(tJsonPrivil);
                    //                W_INSxPrivilege(oDbTbl);
                                    
                    //                oCstDetail.tW_CstName = oResKunnr.Title + " " + oResKunnr.FirstName + " " + oResKunnr.LastName;
                    //                oCstDetail.tW_KubotaID = oResKunnr.KubotaID;
                    //                oCstDetail.nW_CstPoint = oResKunnr.Point;
                    //                oCstDetail.tW_Kunnr = oResKunnr.CustomerCode;     //*Arm 63-06-26
                    //                oCstDetail.W_DATxLoadCst(oResKunnr.KubotaID);
                    //                oCstDetail.tW_Membership = oResKunnr.Membership; //*Arm 63-08-03
                    //                oCstDetail.tW_PhoneNo = oResKunnr.PhoneNo;         //*Arm 63-08-11
                    //                oCstDetail.tW_TaxID = oResKunnr.TaxID;             //*Arm 63-08-11
                    //                oCstDetail.ShowDialog();
                    //                if (oCstDetail.DialogResult == DialogResult.OK)
                    //                {
                    //                    this.Close();
                    //                }
                    //            }
                    //        }
                            
                    //    }
                    //    else if (aoCstKAD.d.results.Count > 1)
                    //    {
                    //        wCstList oCstList = new wCstList(tW_KbdScreen);
                    //        oCstList.W_DATxLoadCst(aoCstKAD);
                    //        oCstList.ShowDialog();
                    //        if (oCstList.DialogResult == DialogResult.OK)
                    //        {
                    //            this.Close();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDataNotFound"), 1); //*Arm 63-08-08
                    //    }
                    //}
                }
                #endregion

                #region STD Ada
                else
                {
                   // if (string.IsNullOrEmpty(otbSchCstName.Text) &&
                   //string.IsNullOrEmpty(otbSchCstCode.Text) &&
                   //string.IsNullOrEmpty(otbSchCstTel.Text) &&
                   //string.IsNullOrEmpty(otbSchCardID.Text) &&
                   //string.IsNullOrEmpty(otbSchMemCrdNo.Text) &&
                   //string.IsNullOrEmpty(otbSchTaxNo.Text)
                   //)
                   // {
                   //     oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCstSchReq"), 3); //*Arm 63-08-08
                   //     return;
                   // }

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

                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call API2PSMaster (/Customer/CstSearch) Start,", cVB.bVB_AlwPrnLog);
                    oCall = new cClientService();
                    oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                    //HttpResponseMessage oRep = new HttpResponseMessage();
                    oRep = new HttpResponseMessage(); //*Arm 63-07-20
                    try
                    {
                        oRep = oCall.C_POSToInvoke(tUrl + tFunc, tJSonCall);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click/Call API Error : " + oEx.Message);
                        return;
                    }
                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Call API2PSMaster (/Customer/CstSearch) End ", cVB.bVB_AlwPrnLog);

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
                                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : Show Customer List ", cVB.bVB_AlwPrnLog);
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
                                                
                                                //if (cVB.oVB_2ndScreen != null)
                                                //{
                                                //    cVB.oVB_2ndScreen.olaGrpCst.Text = ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColPplCodeRet.Name].Value.ToString();
                                                //}

                                                cVB.tVB_QMemMsgID = "";
                                                cVB.bVB_ScanQR = false;

                                                cVB.tVB_CstStaAlwPosCalSo = ogdCst.Rows[nW_SelectRow].Cells[otbColCstStaAlwPosCalSo.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstStaAlwPosCalSo.Name].Value.ToString(); //*Arm 63-03-03 อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต(default)
                                                cVB.nVB_CstPoint = Convert.ToInt32(ogdCst.Rows[nW_SelectRow].Cells[otbColTxnPntQty.Name].Value); //*Arm 63-03-13 Point
                                                cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                                                cVB.tVB_MemberCard = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdNo.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdNo.Name].Value.ToString();
                                                cVB.tVB_ExpiredDate = ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdExpire.Name].Value == null ? string.Empty : ogdCst.Rows[nW_SelectRow].Cells[otbColCstCrdExpire.Name].Value.ToString();
                                                cVB.tVB_MemCode = cVB.tVB_CstCode; //*Arm 63-08-11
                                                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : W_SETxTextCst", cVB.bVB_AlwPrnLog); //*Arm 63-08-03 ยกมาจาก Moshi
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

                #endregion

                Cursor.Current = Cursors.Default; //*Arm 63-07-17
                oCall.C_PRCxCloseConn(); //*Arm 63-07-20
                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : End Click", cVB.bVB_AlwPrnLog);

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : " + oEx.Message);
            }
            finally
            {
                oCall = null;
                oRep = null;
                aoCstSch = null;
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

        public static void W_INSxPrivilege(DataTable poDbTbl)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            try
            {
                oSql.Clear();
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TTmpCstSearch'))");
                oSql.AppendLine("BEGIN ");
                oSql.AppendLine("   DROP TABLE TTmpCstSearch");
                oSql.AppendLine("END");
                oDb.C_SETxDataQuery(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("IF OBJECT_ID(N'TTmpCstSearch') IS NULL BEGIN");
                oSql.AppendLine("    CREATE TABLE[dbo].[TTmpCstSearch](");
                oSql.AppendLine("        [KubotaId][varchar](50) NULL,");
                oSql.AppendLine("        [MatNo] [varchar] (50) NULL,");
                oSql.AppendLine("        [QtyPrt] [varchar] (50) NULL,");
                oSql.AppendLine("        [QtyUse] [varchar] (50) NULL,");
                oSql.AppendLine("        [QtyBal] [varchar] (50) NULL,");
                oSql.AppendLine("        [MatUnit] [varchar] (50) NULL");
                oSql.AppendLine("    ) ON[PRIMARY]");
                oSql.AppendLine("END");
                oSql.AppendLine("TRUNCATE TABLE TTmpCstSearch");
                oDb.C_SETxDataQuery(oSql.ToString());
                
                
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_ConStr, SqlBulkCopyOptions.Default))
                {

                    foreach (DataColumn oColName in poDbTbl.Columns)
                    {
                        oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
                    }
                    //+++++++++++++++
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TTmpCstSearch";

                    try
                    {
                        oBulkCopy.WriteToServer(poDbTbl);
                        //bPrc = true;
                    }
                    catch (Exception oEx)
                    {
                        //new cLog().C_PRCxLog("C_INSxDatabase", oEx.Message.ToString());
                        //bPrc = false;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "W_INSxPrivilege : " + oEx.Message);
            }
        }

        private void wCstSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "wCstSearch_FormClosing : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }


        /// <summary>
        /// Create Customer Code
        /// </summary>
        /// <param name="ptMessage"></param>
        /// <returns></returns>
        public static string C_PRCxCreateCustomer(string ptMessage)
        {
            //CookieContainer oCookieJar;
            NetworkCredential oCredentials;
            HttpWebRequest oReq;
            HttpWebResponse oResp;
            string tResult = "";
            string tResToken;
            try
            {
                new cLog().C_WRTxLog("wCstSearch", "oC_PRCxCreateCustomer : start ", cVB.bVB_AlwPrnLog);

                if (string.IsNullOrEmpty(cVB.tVB_Token_CreateCstCode) || cVB.oVB_Cookie_CreateCstCode == null)
                {
                    //string tReplace = cVB.tVB_KADSAuth_ReqToken.Replace("Basic", "");
                    string tReplace = cVB.tVB_ApiGetToken_Auth.Replace("Basic", ""); //*Arm 63-08-09
                    byte[] data = System.Convert.FromBase64String(tReplace);
                    string tAuthen = System.Text.ASCIIEncoding.ASCII.GetString(data);
                    string[] aAuth = tAuthen.Split(':');
                    string tUsername = aAuth[0].ToString();
                    string tPassword = aAuth[1].ToString();
                    // Setup network credentials object to be used for requests to Gateway server
                    oCredentials = new System.Net.NetworkCredential(tUsername, tPassword);

                    // Create Gateway objects

                    //oReq = (HttpWebRequest)HttpWebRequest.Create(cVB.tVB_APIKADS_ReqToken_Url);
                    oReq = (HttpWebRequest)HttpWebRequest.Create(cVB.tVB_ApiGetToken); //*Arm 63-08-09
                    // Add custom header request to fetch the CSRF token
                    oReq.Credentials = oCredentials;
                    oReq.Method = "GET";
                    //if (!string.IsNullOrEmpty(cVB.tVB_APIKADS_ReqToken_Url))
                    if (!string.IsNullOrEmpty(cVB.tVB_ApiGetToken)) //*Arm 63-08-09
                    {
                        oReq.Headers.Add("X-CSRF-Token", "Fetch");
                    }
                    else
                    {
                        return tResult;
                    }

                    // Setup cookie jar to capture cookies coming back from Gateway server. These cookies are needed along with the CSRF token for modifying requests.
                    //oCookieJar = new CookieContainer();
                    //oReq.CookieContainer = oCookieJar;

                    cVB.oVB_Cookie_CreateCstCode = new CookieContainer();
                    oReq.CookieContainer = cVB.oVB_Cookie_CreateCstCode;

                    new cLog().C_WRTxLog("wCstSearch", "oC_PRCxCreateCustomer : call api Get Token start...", cVB.bVB_AlwPrnLog);
                    try
                    {
                        oResp = (HttpWebResponse)oReq.GetResponse();
                    }
                    catch (System.Net.WebException oEx)
                    {
                        // Add your error handling here
                        new cLog().C_WRTxLog("wCstSearch", "C_PRCxCreateCustomer : Call api (Get Token) Error : " + oEx.Message);
                        return tResult;
                    }
                    catch (Exception oEx)
                    {
                        // Add your error handling here
                        new cLog().C_WRTxLog("wCstSearch", "C_PRCxCreateCustomer : Call api (Get Token) Error  : " + oEx.Message);
                        return tResult;
                    }

                    // Assign values from response to class variables.
                    //tResToken = oResp.Headers.Get("X-CSRF-Token");
                    cVB.tVB_Token_CreateCstCode = oResp.Headers.Get("X-CSRF-Token");
                    new cLog().C_WRTxLog("wCstSearch", "oC_PRCxCreateCustomer : Response X-CSRF-Token : " + cVB.tVB_Token_CreateCstCode, cVB.bVB_AlwPrnLog);
                    new cLog().C_WRTxLog("wCstSearch", "oC_PRCxCreateCustomer : call api Get Token End...", cVB.bVB_AlwPrnLog);
                }

                if (!string.IsNullOrEmpty(cVB.tVB_Token_CreateCstCode))
                {
                    string tError = "";
                    new cLog().C_WRTxLog("wCstSearch", "oC_PRCxCreateCustomer : Process send create customer Code ", cVB.bVB_AlwPrnLog);
                    //tResult = C_PRCtcCallApiCreateCstCode(oCookieJar, cVB.tVB_Token_CreateCstCode, ptMessage, out tError);
                    tResult = C_PRCtcCallApiCreateCstCode(cVB.oVB_Cookie_CreateCstCode, cVB.tVB_Token_CreateCstCode, ptMessage, out tError);
                    if (tError != "")
                    {
                        return tResult="";
                    }
                }
                else
                {
                    new cLog().C_WRTxLog("wCstSearch", "C_PRCxCreateCustomer : Get token is null ");
                }
                new cLog().C_WRTxLog("wCstSearch", "oC_PRCxCreateCustomer : start ", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstSearch", "C_PRCxCreateCustomer :" + oEx.Message);
                return tResult;
            }
            finally
            {
                //oCookieJar = null;
                oCredentials = null;
                oResp = null;
                oReq = null;
                ptMessage = null;
            }
            return tResult;
        }

        public static string C_PRCtcCallApiCreateCstCode(CookieContainer poCookieJar, string ptToken, string ptBody, out string ptError)
        {
            string tMessage = "";
            string tCookie = "";

            HttpWebRequest oReq;
            NetworkCredential oCredentials;
            CookieCollection oCookies;

            try
            {
                new cLog().C_WRTxLog("wCstSearch", "C_PRCtcCallApiCreateCstCode : start ", cVB.bVB_AlwPrnLog);

                //string tReplace = cVB.tVB_KADSAuth.Replace("Basic", "");
                string tReplace = cVB.tVB_ApiCstSch_Auth.Replace("Basic", ""); //*Arm 63-08-09
                byte[] data = System.Convert.FromBase64String(tReplace);
                string tAuthen = System.Text.ASCIIEncoding.ASCII.GetString(data);
                string[] aAuth = tAuthen.Split(':');
                string tUsername = aAuth[0].ToString();
                string tPassword = aAuth[1].ToString();

                // Setup network credentials object to be used for requests to Gateway server
                oCredentials = new System.Net.NetworkCredential(tUsername, tPassword);

                // Create Gateway objects
                //oReq = (HttpWebRequest)HttpWebRequest.Create(cVB.tVB_APIKADS);
                oReq = (HttpWebRequest)HttpWebRequest.Create(cVB.tVB_ApiCstSch); //*Arm 63-08-09
                oReq.Credentials = oCredentials;
                oReq.Method = "POST";
                oReq.ContentType = "application/json";
                oReq.Headers.Add("X-CSRF-Token", ptToken);
                oReq.Accept = "application/json";
                
                //Uri oUri = new Uri(cVB.tVB_APIKADS_ReqToken_Url);
                Uri oUri = new Uri(cVB.tVB_ApiGetToken); //*Arm 63-08-09

                oCookies = poCookieJar.GetCookies(oUri);
                foreach (Cookie cookie in oCookies)
                {
                    tCookie = tCookie + ";" + cookie.ToString();
                }

                oReq.Headers.Add("Cookie", tCookie.Substring(1));

                new cLog().C_WRTxLog("wCstSearch", "oC_PRCxCreateCustomer : Request Header X-CSRF-Token : " + ptToken, cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("wCstSearch", "oC_PRCxCreateCustomer : Request Header Cookie : " + tCookie, cVB.bVB_AlwPrnLog);

                var oData = Encoding.UTF8.GetBytes(ptBody);
                using (var oStream = oReq.GetRequestStream())
                {
                    oStream.Write(oData, 0, oData.Length);
                }

                new cLog().C_WRTxLog("wCstSearch", "C_PRCtcCallApiCreateCstCode : Call api (create customer code) start... ", cVB.bVB_AlwPrnLog);
                try
                {
                    using (HttpWebResponse oResp = (HttpWebResponse)oReq.GetResponse())
                    {
                        using (StreamReader oRd = new StreamReader(oResp.GetResponseStream()))
                        {
                            tMessage = oRd.ReadToEnd();
                            oRd.Close();
                        }
                        oResp.Close();
                        new cLog().C_WRTxLog("wCstSearch", "C_PRCtcCallApiCreateCstCode : Call api create customer code Response : " + tMessage, cVB.bVB_AlwPrnLog);
                    }
                }
                catch (System.Net.WebException oEx)
                {
                    // Add your error handling here
                    ptError = oEx.Message.ToString();
                    new cSP().SP_SHWxMsg("Process fail : " + oEx.ToString(),2);
                    new cLog().C_WRTxLog("wCstSearch", "C_PRCtcCallApiCreateCstCode : Call api create customer code Error : " + oEx.Message);
                    return tMessage;
                }
                catch (Exception oEx)
                {
                    ptError = oEx.Message.ToString();
                    new cSP().SP_SHWxMsg("Process fail ", 2);
                    new cLog().C_WRTxLog("wCstSearch", "C_PRCtcCallApiCreateCstCode : Call api create customer code Error : " + oEx.Message);
                    return tMessage;
                }
                new cLog().C_WRTxLog("wCstSearch", "C_PRCtcCallApiCreateCstCode : Call api (create customer code) end... ", cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("wCstSearch", "C_PRCtcCallApiCreateCstCode : end ", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                ptError = oEx.Message.ToString();
                new cSP().SP_SHWxMsg("Process fail ", 2);
                new cLog().C_WRTxLog("wCstSearch", "C_PRCtcCallApiCreateCstCode : Error " + oEx.Message);
                return tMessage;
            }
            finally
            {
                oReq = null;
                oCredentials = null;
                oCookies = null;
                poCookieJar = null;
            }
            ptError = "";
            return tMessage;
        }
    }
}
