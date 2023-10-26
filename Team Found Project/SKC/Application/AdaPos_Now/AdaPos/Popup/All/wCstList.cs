using AdaPos.Class;
using AdaPos.Models.Webservice.Required.Customer;
using AdaPos.Models.Webservice.Required.KADS.Customer;
using AdaPos.Models.Webservice.Respond.Customer;
using AdaPos.Models.Webservice.Respond.KADS.Customer;
using AdaPos.Resources_String.Local;
using C1.Win.C1FlexGrid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.All
{
    public partial class wCstList : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        public cmlResCstKADRef oW_ResCstKad;
        //public wCstDetail oW_CstDetail = new wCstDetail();
        private int nW_SelRow = 1; //*Arm 63-07-31
        private string tW_KbdScreen;
        public wCstList(string ptKbdScreen ="")
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                tW_KbdScreen = ptKbdScreen;
                W_SETxDesign();
                W_SETxText();
                oW_ResCstKad = new cmlResCstKADRef();
                //W_DATxLoadCst();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "wCstList : " + oEx.Message);
            }
        }

        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                
                new cSP().SP_SETxSetGridFormat(ogdCstList);
                ogdCstList.Rows.Count = ogdCstList.Rows.Fixed;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_SETxDesign : " + oEx.Message);
            }
            finally
            {
                //oW_SP.SP_CLExMemory();
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
                olaTitleCstSearch.Text = oW_Resource.GetString("tTitleChooseCst");
                W_SETxColGrid(ogdCstList);

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_SETxText : " + oEx.Message);
            }
        }
        
        public void W_DATxLoadCst(cmlResCstKAD paCstKAD)
        {
            DataTable oDbTbl = new DataTable();
            try
            {
                foreach (cmlResKunnr oResKunnr in paCstKAD.d.results)
                {
                    string tJsonPrivil = Newtonsoft.Json.JsonConvert.SerializeObject(oResKunnr);
                    ogdCstList.Rows.Add();
                    ogdCstList.SetData(ogdCstList.Rows.Count - ogdCstList.Rows.Fixed, ogdCstList.Cols["FTCstCode"].Index,oResKunnr.CustomerCode.ToString());
                    ogdCstList.SetData(ogdCstList.Rows.Count - ogdCstList.Rows.Fixed, ogdCstList.Cols["FTCstName"].Index, oResKunnr.Title.ToString() + " " + oResKunnr.FirstName.ToString() + " " + oResKunnr.LastName.ToString());
                    ogdCstList.SetData(ogdCstList.Rows.Count - ogdCstList.Rows.Fixed, ogdCstList.Cols["FTMobile"].Index,oResKunnr.PhoneNo.ToString());
                    ogdCstList.SetData(ogdCstList.Rows.Count - ogdCstList.Rows.Fixed, ogdCstList.Cols["FTTaxNo"].Index,oResKunnr.TaxID.ToString());
                    ogdCstList.SetData(ogdCstList.Rows.Count - ogdCstList.Rows.Fixed, ogdCstList.Cols["JsonData"].Index, tJsonPrivil);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_DATxLoadCst : " + oEx.Message);
            }
            finally
            {
                //oW_SP.SP_CLExMemory();
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
                new cLog().C_WRTxLog("wCstList", "ocmBack_Click : " + oEx.Message);
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
                new cLog().C_WRTxLog("wCstList", "ocmShwKb_Click : " + oEx.Message);
            }
        }

        public void W_PRCxKADSCustomerSearch(string ptKubotaID)
        {
            try
            {
                
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_PRCxKADSCustomerSearch : " + oEx.Message.ToString());
            }
            finally
            {
                
            }
        }

        public bool W_VerifyReq(int pnReq, string ptKubotaID = "")
        {
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_SaleOrg))
                {
                    new cLog().C_WRTxLog("wCstList", "W_VerifyReq/Parameter SaleOrg is Empty ...");
                    return false;
                }
                if (cVB.tVB_SaleOrg.Length > 4)
                {
                    new cLog().C_WRTxLog("wCstList", "W_VerifyReq/Parameter SaleOrg is Length More than 4 character ...");
                    return false;
                }
                if (string.IsNullOrEmpty(ptKubotaID))
                {
                    new cLog().C_WRTxLog("wCstList", "VerifyReq/Parameter PhoneNo is Length More than 40 character ...");
                    return false;
                }

                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_VerifyReq : " + oEx.Message.ToString());
                return false;
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            //cmlResCstKADRef aoCstKAD;
            //cmlReqCustomerCreate oReqCst;
            //DataTable oDbTbl;
            wCstDetail oCstDetail;
            //*Arm 63-08-15
            cmlResKunnr oResKunnr;
            DataTable odtPrivil;
            cmlResCreateCustomerCode oCst;
            try
            {
                if (nW_SelRow > 0)
                {
                    //oCstDetail = new wCstDetail();

                    //*Arm 63-08-15
                    string tJsonCst = ogdCstList.GetData(nW_SelRow, "JsonData").ToString();
                    oResKunnr = new cmlResKunnr();
                    oResKunnr = JsonConvert.DeserializeObject<cmlResKunnr>(tJsonCst);

                    if (oResKunnr != null)
                    {
                        switch (oResKunnr.BUGroup)
                        {
                            case "ZAR1":
                                new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : BUGroup : " + oResKunnr.BUGroup, cVB.bVB_AlwPrnLog);
                                if (oResKunnr.PrivilegePointSet != null && oResKunnr.PrivilegePointSet.results != null && oResKunnr.PrivilegePointSet.results.Count > 0)
                                {
                                    //กรณีมีโควต้า บันทึกลง Table Temp
                                    string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oResKunnr.PrivilegePointSet.results);
                                    odtPrivil = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);

                                    new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : C_INTxInsertPrivilege2Tmp start...", cVB.bVB_AlwPrnLog);
                                    cServiceKADS.C_INTxInsertPrivilege2Tmp(odtPrivil);
                                    new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : C_INTxInsertPrivilege2Tmp end...", cVB.bVB_AlwPrnLog);
                                }

                                oCstDetail = new wCstDetail();
                                oCstDetail.tW_CstName = oResKunnr.Title + " " + oResKunnr.FirstName + " " + oResKunnr.LastName;
                                oCstDetail.tW_KubotaID = oResKunnr.KubotaID;
                                oCstDetail.nW_CstPoint = oResKunnr.Point;
                                oCstDetail.tW_Kunnr = oResKunnr.CustomerCode;
                                oCstDetail.W_DATxLoadCst(oResKunnr.KubotaID);
                                oCstDetail.tW_Membership = oResKunnr.Membership;
                                oCstDetail.tW_PhoneNo = oResKunnr.PhoneNo;
                                oCstDetail.tW_TaxID = oResKunnr.TaxID;

                                new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : Open wCstDetail ", cVB.bVB_AlwPrnLog);
                                oCstDetail.ShowDialog();
                                if (oCstDetail.DialogResult == DialogResult.OK)
                                {
                                    this.Close();
                                    this.DialogResult = DialogResult.OK;
                                }

                                break;

                            case "ZAR6":
                                new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : BUGroup : " + oResKunnr.BUGroup, cVB.bVB_AlwPrnLog);

                                oCstDetail = new wCstDetail();  //*Arm 63-08-31
                                oCstDetail.tW_KubotaID = oResKunnr.CustomerCode;    //*Arm 63-08-31

                                oCst = new cmlResCreateCustomerCode();

                                new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : C_PRCoBUGroup6 start...", cVB.bVB_AlwPrnLog);
                                Cursor.Current = Cursors.WaitCursor;
                                oCst = cServiceKADS.C_PRCoBUGroup6(oResKunnr); // Process Create Customer กรณี BUGroup != "ZAR1"
                                Cursor.Current = Cursors.Default;
                                new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : C_PRCoBUGroup6 end...", cVB.bVB_AlwPrnLog);

                                if (oCst != null) // Retrun BUGroup มาเป็น "ZAR6"  (สำร็จ)
                                {
                                    //if (oCst.d.PrivilegePointSet != null && oCst.d.PrivilegePointSet.results != null && oCst.d.PrivilegePointSet.results.Count > 0)
                                    //{
                                    //    //กรณีมีโควต้า บันทึกลง Table Temp
                                    //    string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oCst.d.PrivilegePointSet.results);
                                    //    odtPrivil = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);

                                    //    new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : C_INTxInsertPrivilege2Tmp start...", cVB.bVB_AlwPrnLog);
                                    //    cServiceKADS.C_INTxInsertPrivilege2Tmp(odtPrivil);
                                    //    new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : C_INTxInsertPrivilege2Tmp end...", cVB.bVB_AlwPrnLog);
                                    //}

                                    //oCstDetail = new wCstDetail();
                                    oCstDetail.tW_CstName = oCst.d.Title + " " + oCst.d.FirstName + " " + oCst.d.LastName;
                                    //oCstDetail.tW_KubotaID = oCst.d.KubotaID;
                                    oCstDetail.nW_CstPoint = oCst.d.Point;
                                    oCstDetail.tW_Kunnr = oCst.d.CustomerCode;
                                    //oCstDetail.W_DATxLoadCst(oCst.d.KubotaID);
                                    oCstDetail.W_DATxLoadCst(oCstDetail.tW_KubotaID); //*Arm 63-08-31
                                    oCstDetail.tW_Membership = oCst.d.Membership;
                                    oCstDetail.tW_PhoneNo = oCst.d.PhoneNo;
                                    oCstDetail.tW_TaxID = oCst.d.TaxID;

                                    new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : Open wCstDetail ", cVB.bVB_AlwPrnLog);
                                    oCstDetail.ShowDialog();
                                    if (oCstDetail.DialogResult == DialogResult.OK)
                                    {
                                        new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : Close. ", cVB.bVB_AlwPrnLog);
                                        this.Close();
                                        this.DialogResult = DialogResult.OK;
                                    }
                                }
                                //else oCst = null = สร้างไม่สำเร็จ
                                break;

                            default:
                                new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgPrcCreateCstCode") + " (BUGroup : " + oResKunnr.BUGroup + ")", 3);
                                new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : BUGroup : " + oResKunnr.BUGroup, cVB.bVB_AlwPrnLog);
                                break;
                        }
                    }
                    //++++++++++++

                    //oCstDetail = new wCstDetail();
                    //string tJsonCst = ogdCstList.GetData(nW_SelRow, "JsonData").ToString();
                    //aoCstKAD = new cmlResCstKADRef();
                    //aoCstKAD = JsonConvert.DeserializeObject<cmlResCstKADRef>(tJsonCst);

                    //if (aoCstKAD.BUGroup == "ZAR6")
                    //{
                    //    new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : BUGroup == ZAR6 (Customer No CstCode)", cVB.bVB_AlwPrnLog); //*Arm 63-08-03

                    //    //ส่งสร้าง CstCode ใหม่
                    //    oReqCst = new cmlReqCustomerCreate();
                    //    oReqCst.CustomerCode = aoCstKAD.CustomerCode;
                    //    oReqCst.Title = aoCstKAD.Title;
                    //    oReqCst.FirstName = aoCstKAD.FirstName;
                    //    oReqCst.LastName = aoCstKAD.LastName;
                    //    oReqCst.Titlee = aoCstKAD.Titlee;
                    //    oReqCst.Namee = aoCstKAD.Namee;
                    //    oReqCst.Surnamee = aoCstKAD.Surnamee;
                    //    oReqCst.Addr = aoCstKAD.Addr;
                    //    oReqCst.Soi = aoCstKAD.Soi;
                    //    oReqCst.Street = aoCstKAD.Street;
                    //    oReqCst.District = aoCstKAD.District;
                    //    oReqCst.City = aoCstKAD.City;
                    //    oReqCst.Province = aoCstKAD.Province;
                    //    oReqCst.Gender = aoCstKAD.Gender;
                    //    oReqCst.Birth = aoCstKAD.Birth;
                    //    oReqCst.Email = aoCstKAD.Email;
                    //    oReqCst.Mobile = aoCstKAD.Mobile;
                    //    oReqCst.Remark = aoCstKAD.Remark;
                    //    string tJsonCall = JsonConvert.SerializeObject(oReqCst);
                    //    string tJSonRes = "";
                    //    //tJSonRes = new wCstSearch(tW_KbdScreen).C_PRCxCreateCustomer(tJsonCall);
                    //    Cursor.Current = Cursors.WaitCursor; 
                    //    tJSonRes = wCstSearch.C_PRCxCreateCustomer(tJsonCall);
                    //    Cursor.Current = Cursors.Default;

                    //    if (!string.IsNullOrEmpty(tJSonRes))
                    //    {
                    //        cmlResCreateCustomerCode oCst = new cmlResCreateCustomerCode();
                    //        oCst = JsonConvert.DeserializeObject<cmlResCreateCustomerCode>(tJSonRes);
                    //        DataTable oDbTblKubId = new DataTable();
                    //        //if (oCst != null && oCst.d != null)
                    //        if(oCst != null && oCst.d != null && oCst.d.BUGroup == "ZAR1")
                    //        {
                    //            if (oCst.d.PrivilegePointSet != null && oCst.d.PrivilegePointSet.results != null && oCst.d.PrivilegePointSet.results.Count > 0)
                    //            {
                    //                string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oCst.d.PrivilegePointSet.results);
                    //                oDbTblKubId = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);
                    //                W_INSxPrivilege(oDbTblKubId);
                    //            }

                    //            oCstDetail.tW_CstName = oCst.d.Title + " " + oCst.d.FirstName + " " + oCst.d.LastName;
                    //            oCstDetail.tW_KubotaID = oCst.d.KubotaID;
                    //            oCstDetail.nW_CstPoint = oCst.d.Point;
                    //            oCstDetail.tW_Kunnr = oCst.d.CustomerCode;     //*Arm 63-06-26
                    //            oCstDetail.W_DATxLoadCst(oCst.d.KubotaID);
                    //            oCstDetail.tW_Membership = oCst.d.Membership;   //*Arm 63-08-03
                    //            oCstDetail.tW_PhoneNo = oCst.d.PhoneNo;         //*Arm 63-08-11
                    //            oCstDetail.tW_TaxID = oCst.d.TaxID;             //*Arm 63-08-11
                    //            oCstDetail.ShowDialog();
                    //            if (oCstDetail.DialogResult == DialogResult.OK)
                    //            {
                    //                this.Close();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgPrcCreateCstCode") + " (BUGroup : " + oCst.d.BUGroup + ")", 3); //*Arm 63-08-09
                    //            new cLog().C_WRTxLog("wCstList", "ocmAccept_Click :Create Customer retrun BUGroup == ZAR6 (Customer No CstCode)", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    oDbTbl = new DataTable();
                    //    string tJsonPrivil = Newtonsoft.Json.JsonConvert.SerializeObject(aoCstKAD.PrivilegePointSet.results);
                    //    oDbTbl = JsonConvert.DeserializeObject<DataTable>(tJsonPrivil);
                    //    W_INSxPrivilege(oDbTbl);

                    //    oCstDetail = new wCstDetail();
                    //    oCstDetail.tW_CstName = aoCstKAD.Title + " " + aoCstKAD.FirstName + " " + aoCstKAD.LastName;
                    //    oCstDetail.tW_KubotaID = aoCstKAD.KubotaID;
                    //    oCstDetail.nW_CstPoint = aoCstKAD.Point;
                    //    oCstDetail.tW_Kunnr = aoCstKAD.CustomerCode;
                    //    oCstDetail.tW_Membership = aoCstKAD.Membership;
                    //    oCstDetail.tW_PhoneNo = aoCstKAD.PhoneNo;         //*Arm 63-08-11
                    //    oCstDetail.tW_TaxID = aoCstKAD.TaxID;             //*Arm 63-08-11
                    //    oCstDetail.W_DATxLoadCst(aoCstKAD.KubotaID);

                    //    oCstDetail.ShowDialog();
                    //    if (oCstDetail.DialogResult == DialogResult.OK)
                    //    {
                    //        this.Close();
                    //        this.DialogResult = DialogResult.OK;
                    //    }
                    //}
                }
                else
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCstSchChoose"), 3); //*Arm 63-08-28
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : " + oEx.Message);
            }
            finally
            {
                oCstDetail = null;
                //aoCstKAD = null;
                //oDbTbl = null;
                //oReqCst = null;

                //*Arm 63-08-15
                oResKunnr = null;
                odtPrivil = null;
                oCst = null;
                //+++++++++++++
            }
        }

        //private void W_INSxPrivilege(DataTable poDbTbl)
        //{
        //    StringBuilder oSql = new StringBuilder();
        //    cDatabase oDb = new cDatabase();
        //    try
        //    {
        //        //*Arm 63-08-09
        //        oSql.Clear();
        //        oSql.AppendLine("IF EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TTmpCstSearch'))");
        //        oSql.AppendLine("BEGIN ");
        //        oSql.AppendLine("   DROP TABLE TTmpCstSearch");
        //        oSql.AppendLine("END");
        //        oDb.C_SETxDataQuery(oSql.ToString());
        //        //++++++++++++

        //        oSql.Clear();
        //        //oSql.AppendLine("DROP TABLE TTmpCstSearch");

        //        oSql.AppendLine("IF OBJECT_ID(N'TTmpCstSearch') IS NULL BEGIN");
        //        oSql.AppendLine("    CREATE TABLE[dbo].[TTmpCstSearch](");
        //        oSql.AppendLine("        [KubotaId][varchar](50) NULL,");
        //        oSql.AppendLine("        [MatNo] [varchar] (50) NULL,");
        //        oSql.AppendLine("        [QtyPrt] [varchar] (50) NULL,");
        //        oSql.AppendLine("        [QtyUse] [varchar] (50) NULL,");
        //        oSql.AppendLine("        [QtyBal] [varchar] (50) NULL,");
        //        oSql.AppendLine("        [MatUnit] [varchar] (50) NULL");
        //        oSql.AppendLine("    ) ON[PRIMARY]");
        //        oSql.AppendLine("END");

        //        oSql.AppendLine("TRUNCATE TABLE TTmpCstSearch");
        //        oDb.C_SETxDataQuery(oSql.ToString());


        //        using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_ConStr, SqlBulkCopyOptions.Default))
        //        {

        //            foreach (DataColumn oColName in poDbTbl.Columns)
        //            {
        //                oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
        //            }
        //            //+++++++++++++++
        //            oBulkCopy.BatchSize = 100;
        //            oBulkCopy.DestinationTableName = "dbo.TTmpCstSearch";

        //            try
        //            {
        //                oBulkCopy.WriteToServer(poDbTbl);
        //                //bPrc = true;
        //            }
        //            catch (Exception oEx)
        //            {
        //                //new cLog().C_PRCxLog("C_INSxDatabase", oEx.Message.ToString());
        //                //bPrc = false;
        //            }
        //        }
        //    }
        //    catch (Exception oEx)
        //    {
        //        new cLog().C_WRTxLog("wCstSearch", "W_INSxPrivilege : " + oEx.Message);
        //    }
        //}
        

        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                switch (poGD.Name)
                {
                    case "ogdCstList":
                        nWidth = poGD.Width;
                        poGD.Cols["FTCstCode"].Width = nWidth * 16 / 100;
                        poGD.Cols["FTCstName"].Width = nWidth * 40 / 100;
                        poGD.Cols["FTMobile"].Width = nWidth * 20 / 100;
                        poGD.Cols["FTTaxNo"].Width = nWidth * 24 / 100;

                        poGD.Cols["FTCstCode"].Caption = oW_Resource.GetString("tColCstCode");
                        poGD.Cols["FTCstName"].Caption = oW_Resource.GetString("tColCstName");
                        poGD.Cols["FTMobile"].Caption = oW_Resource.GetString("tColCstTel");
                        poGD.Cols["FTTaxNo"].Caption = oW_Resource.GetString("tColCstTaxNo");

                        poGD.Cols["FTCstCode"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTCstName"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTMobile"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTTaxNo"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["FTCstCode"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FTCstName"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FTMobile"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FTTaxNo"].TextAlign = TextAlignEnum.LeftCenter;

                        poGD.Editor.BackColor = Color.White;

                        break;
                }
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_SETxColGrid : " + oEx.Message);
            }
        }
        
        private void ogdCstList_Click(object sender, EventArgs e)
        {
            try
            {
                nW_SelRow = Convert.ToInt32(ogdCstList.RowSel); //*Arm 63-07-31
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "ogdCstList_Click : " + oEx.Message);
            }
            
        }

        private void ogdCstList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                nW_SelRow = Convert.ToInt32(ogdCstList.RowSel); //*Arm 63-07-31
                ocmAccept_Click(ocmAccept, null);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "ogdCstList_DoubleClick : " + oEx.Message);
            }
        }
    }
}
