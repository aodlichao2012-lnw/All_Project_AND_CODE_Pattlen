using AdaPos.Class;
using AdaPos.Models.Webservice.Respond.Customer;
using AdaPos.Models.Webservice.Respond.KADS.Customer;
using AdaPos.Resources_String.Local;
using C1.Win.C1FlexGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.All
{
    public partial class wCstDetail : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        public string tW_CstName;
        public int nW_CstPoint;
        public string tW_KubotaID;
        public string tW_Kunnr;
        public string tW_Membership; //*Arm 63-08-03
        public string tW_PhoneNo;    //*Arm 63-08-11
        public string tW_TaxID;      //*Arm 63-08-11
        public wCstDetail()
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                
                W_SETxDesign();
                W_SETxText();
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "wCstDetail : " + oEx.Message);
            }
        }

        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                //opnTop.BackColor = cVB.oVB_ColLight; //*Arm 63-08-11 Comment Code

                //new cSP().SP_SETxSetGridviewFormat(ogdCst);
                new cSP().SP_SETxSetGridFormat(ogdCstDetail);
                ogdCstDetail.Rows.Count = ogdCstDetail.Rows.Fixed;
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "W_SETxDesign : " + oEx.Message);
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
                olaTitleCstSearch.Text = oW_Resource.GetString("tTitleCstDetail");
                olaCstCode.Text = oW_Resource.GetString("tColCstCode");
                olaPntAval.Text = oW_Resource.GetString("tTitleCstPntAvailable");
                olaTableTitlt.Text = oW_Resource.GetString("tTitleCstBenefit");
                W_SETxColGrid(ogdCstDetail);
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "W_SETxText : " + oEx.Message);
            }
        }

        public void W_DATxLoadCst(string ptKubotaId)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            DataTable oDbTbl = new DataTable();
            try
            {
                olaCstCode.Text = oW_Resource.GetString("tColCstCode")+ " : " + tW_Kunnr;
                olaName.Text = tW_CstName;
                //olaPoint.Text = string.Format(CultureInfo.InvariantCulture,"{0:0,0.00}", nW_CstPoint);
                olaPoint.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(nW_CstPoint), cVB.nVB_DecShow);

                if (!string.IsNullOrEmpty(ptKubotaId)) //*Arm 63-08-20 มีค่า KubotaID
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT TmpCst.KubotaId,TmpCst.MatNo As FTPdtCode,Pdt_L.FTPdtName, CAST(TmpCst.QtyBal AS NUMERIC(18,4)) As FNQuotas FROM TTmpCstSearch TmpCst");
                    oSql.AppendLine("LEFT JOIN TCNMPdt_L Pdt_L ON TmpCst.MatNo = Pdt_L.FTPdtCode");
                    //ตอนนี้ใส่ KubotaId ไปก่อนเพื่อกรองข้อมูล
                    oSql.AppendLine("WHERE KubotaId = '" + ptKubotaId + "' ");
                    oSql.AppendLine("ORDER BY TmpCst.MatNo ASC"); //*Arm 63-08-18 การแสดงรายการข้อมูล Quota .เรียงตาม รหัสสินค้า
                    oDbTbl = oDb.C_GEToDataQuery(oSql.ToString());

                    if (oDbTbl.Rows.Count > 0)
                    {
                        foreach (DataRow oDataPri in oDbTbl.Rows)
                        {
                            //ogdCst.Rows.Add(oDataPri.Field<string>("FTPdtCode"), oDataPri.Field<string>("FTPdtName"), oDataPri.Field<string>("FNQuotas"));
                            ogdCstDetail.Rows.Add();
                            ogdCstDetail.SetData(ogdCstDetail.Rows.Count - ogdCstDetail.Rows.Fixed, ogdCstDetail.Cols["FTPdtCode"].Index, oDataPri.Field<string>("FTPdtCode"));
                            ogdCstDetail.SetData(ogdCstDetail.Rows.Count - ogdCstDetail.Rows.Fixed, ogdCstDetail.Cols["FTPdtName"].Index, oDataPri.Field<string>("FTPdtName"));
                            ogdCstDetail.SetData(ogdCstDetail.Rows.Count - ogdCstDetail.Rows.Fixed, ogdCstDetail.Cols["FNQuotas"].Index, oDataPri.Field<decimal>("FNQuotas"));
                        }
                        cVB.oVB_CstPrivilege = oDbTbl;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "W_DATxLoadCst : " + oEx.Message);
            }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {

                cVB.nVB_CstPoint = nW_CstPoint;
                cVB.nVB_CstPiontB4Used = nW_CstPoint;
                cVB.tVB_CstName = tW_CstName;
                cVB.tVB_KubotaID = tW_KubotaID;     //*Arm 63-06-26
                cVB.tVB_CstCode = tW_Kunnr;         //*Arm 63-06-26
                cVB.tVB_CstTel = tW_PhoneNo;
                cVB.tVB_CstCardID = tW_TaxID;

                //*Arm 63-08-03
                if (string.IsNullOrEmpty(tW_Membership))
                {
                    cVB.tVB_MemCode = "";   //*Arm 63-08-11
                    cVB.bVB_Flag = false;   //*Arm 63-08-11
                }
                else
                {
                    cVB.tVB_MemCode = tW_Membership;
                    cVB.bVB_Flag = true;
                }
                //+++++++++++++

                cVB.oVB_Sale.W_SETxTextCst();

                //oSql.Clear();
                //oSql.AppendLine("UPDATE Pmt");
                //oSql.AppendLine("SET Pmt.FNPmhLimitQty = Tmp.QtyBal");
                //oSql.AppendLine("FROM TPMTPmt Pmt");
                //oSql.AppendLine("INNER JOIN  TTmpCstSearch Tmp ON Pmt.FTPmdRefCode = Tmp.MatNo");               
                //oDB.C_SETxDataQuery(oSql.ToString());

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "ocmAccept_Click : " + oEx.Message);
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
                //*Arm 63-08-03 ClearParameter
                tW_CstName = "";
                tW_KubotaID = "";
                tW_Membership = "";
                tW_Kunnr = "";
                nW_CstPoint = 0;
                //+++++++++++++
                
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "ocmBack_Click : " + oEx.Message);
            }
        }

        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                switch (poGD.Name)
                {
                    case "ogdCstDetail":

                        nWidth = poGD.Width;
                        poGD.Cols["FTPdtCode"].Width = nWidth * 30 / 100;
                        poGD.Cols["FTPdtName"].Width = nWidth * 50 / 100;
                        poGD.Cols["FNQuotas"].Width = nWidth * 20 / 100;

                        poGD.Cols["FTPdtCode"].Caption = oW_Resource.GetString("tTitlePdtCode");
                        poGD.Cols["FTPdtName"].Caption = oW_Resource.GetString("tTitlePdtName");
                        poGD.Cols["FNQuotas"].Caption = oW_Resource.GetString("tTitleQuotas");

                        poGD.Cols["FTPdtCode"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPdtName"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FNQuotas"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["FTPdtCode"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FTPdtName"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FNQuotas"].TextAlign = TextAlignEnum.RightCenter; //*Arm 63-09-13 ปรับชิดขวา

                        poGD.Cols["FNQuotas"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow); //Arm 63-08-11

                        break;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "W_SETxColGrid : " + oEx.Message);
            }
        }
    }
}
