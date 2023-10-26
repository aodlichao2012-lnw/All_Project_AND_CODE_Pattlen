using AdaPos.Class;
using AdaPos.Models.Webservice.Respond.KADS;
using AdaPos.Models.Webservice.Respond.KADS.Vehicle;
using AdaPos.Resources_String.Local;
using C1.Win.C1FlexGrid;
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

namespace AdaPos.Popup.wSale
{
    public partial class wVehicle : Form
    {
        private ResourceManager oW_Resource;
        private string tW_KubotaID;
        private DataTable oW_dtTmp;
        private int nW_TotalPage = 0;
        private int nW_PerPage = 0;
        private int nW_CurPage = 0;
        private int nW_Start = 0;
        private int nW_Stop = 0;
        private int nW_RowSal = 1;

        public wVehicle(string ptKubotaID)
        {
            InitializeComponent();
            try
            {
                tW_KubotaID = ptKubotaID;
                W_SETxDesign();
                W_SETxText();
                W_GETxLoadData();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "wVehicle : " + oEx.Message);
            }
        }
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridFormat(ogdVeh);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "W_SETxDesign : " + oEx.Message);
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
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                //Title
                olaTitleVehicle.Text = oW_Resource.GetString("tTitlVehicle");
                W_SETxColGrid(ogdVeh);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "W_SETxText : " + oEx.Message);
            }
        }

        private void W_GETxLoadData()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                W_CALxApiGetData();
                Cursor.Current = Cursors.Default;
                if (oW_dtTmp != null)
                {
                    if (oW_dtTmp.Rows.Count > 0)
                    {
                        nW_PerPage = 10;
                        decimal cTotalPage = Math.Ceiling(((decimal)oW_dtTmp.Rows.Count / (decimal)nW_PerPage));
                        nW_TotalPage = (int)cTotalPage;
                        nW_CurPage = 1;
                        nW_Start = ((nW_CurPage * nW_PerPage) - nW_PerPage) + 1;
                        nW_Stop = (nW_CurPage * nW_PerPage);

                        W_SHWxDataVehicle();
                    }
                    else
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDataNotFound"), 3);
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "W_GETxLoadData : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        private void W_SHWxDataVehicle()
        {
            int nCount = 1;
            try
            {
                ogdVeh.Rows.Count = ogdVeh.Rows.Fixed;
                foreach (DataRow oRow in oW_dtTmp.Rows)
                {
                    if (nCount >= nW_Start && nCount <= nW_Stop)
                    {
                        string tSaleDate = "";
                        string tYear = "";
                        string tMonth = "";
                        string tDay = "";

                        if (!string.IsNullOrEmpty(oRow.Field<string>("SaleDate")))
                        {
                            tYear = (Convert.ToInt32(oRow.Field<string>("SaleDate").Substring(0, 4)) + 543).ToString();
                            tMonth = oRow.Field<string>("SaleDate").Substring(4, 2);
                            tDay = oRow.Field<string>("SaleDate").Substring(6, 2);
                            tSaleDate = tDay + "/" + tMonth + "/" + tYear;
                        }
                        ogdVeh.Rows.Add();
                        ogdVeh.SetData(ogdVeh.Rows.Count - ogdVeh.Rows.Fixed, ogdVeh.Cols["otbColVIN"].Index, oRow.Field<string>("VhVin"));
                        ogdVeh.SetData(ogdVeh.Rows.Count - ogdVeh.Rows.Fixed, ogdVeh.Cols["otbColEngine"].Index, oRow.Field<string>("EngNo"));
                        ogdVeh.SetData(ogdVeh.Rows.Count - ogdVeh.Rows.Fixed, ogdVeh.Cols["otbColModel"].Index, oRow.Field<string>("Model"));
                        ogdVeh.SetData(ogdVeh.Rows.Count - ogdVeh.Rows.Fixed, ogdVeh.Cols["otbColSaleDate"].Index, tSaleDate);
                    }
                    nCount++;
                }
                olaPage.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), oW_dtTmp.Rows.Count, nW_CurPage, nW_TotalPage);

                // เช็คสถาณะปุ่มเลือกหน้า
                if(nW_CurPage <= 1)
                {
                    ocmSchPrevious.Enabled = false;
                }
                else
                {
                    ocmSchPrevious.Enabled = true;
                }

                if(nW_CurPage >= nW_TotalPage)
                {
                    ocmSchNextPage.Enabled = false;
                }
                else
                {
                    ocmSchNextPage.Enabled = true;
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "W_GETxLoadData : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void  W_CALxApiGetData()
        {
            cClientService oCall;
            HttpResponseMessage oRep;
            cmlResKADS<cmlResKADSResult<cmlResInfoResultVehicle>> oResult = new cmlResKADS<cmlResKADSResult<cmlResInfoResultVehicle>>();
            string tUrl = "";
            string Func = "";
            try
            {
                Func = "?$filter=KubotaId eq '" + tW_KubotaID + "'";
                tUrl = cVB.tVB_ApiVehicle + Func; //*Arm 63-08-09
                new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData : Call API Vehicle/ url : " + tUrl, cVB.bVB_AlwPrnLog);
                oCall = new cClientService();
                oCall = new cClientService("Authorization", cVB.tVB_ApiVehicle_Auth); //*Arm 63-08-09

                oRep = new HttpResponseMessage();
                try
                {
                    new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData : Call API C_GEToInvoke Start. ", cVB.bVB_AlwPrnLog);
                    oRep = oCall.C_GEToInvoke(tUrl);
                    oRep.EnsureSuccessStatusCode(); //*Arm 63-08-19
                    new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData : Call API C_GEToInvoke End. ", cVB.bVB_AlwPrnLog);
                }
                catch(HttpRequestException oEx)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + Environment.NewLine + "[" + oEx.Message + "]", 2); //*Arm 63-08-19
                    new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData : " + oEx.Message.ToString());
                    oW_dtTmp = null;
                    return;
                }
                catch (Exception oEx)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"), 2); //*Arm 63-08-19
                    new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData : " + oEx.Message.ToString());
                    oW_dtTmp = null;
                    return;
                }

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData : Call API Vehicle/ Response : " + tJSonRes, cVB.bVB_AlwPrnLog);
                    oResult = JsonConvert.DeserializeObject<cmlResKADS<cmlResKADSResult<cmlResInfoResultVehicle>>>(tJSonRes);

                    string tResults = JsonConvert.SerializeObject(oResult.d.results);
                    oW_dtTmp = new DataTable(); //*Arm 63-08-19
                    oW_dtTmp = JsonConvert.DeserializeObject<DataTable>(tResults);
                    
                }
                //else
                //{
                //    //if (oRep.StatusCode == System.Net.HttpStatusCode.NotFound)
                //    //{
                //    //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDataNotFound"), 2);
                //    //    new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData/Error : " + oRep.StatusCode);
                //    //    this.Close();
                //    //}
                //    //else
                //    //{
                //    //    //new cSP().SP_SHWxMsg("Service Error : " + oRep.StatusCode, 2);
                //    //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + " (" + oRep.StatusCode +")", 2); //*Arm 63-08-19
                //    //    new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData/Error : " + oRep.StatusCode);
                //    //    this.Close();
                //    //}
                //    //new cSP().SP_SHWxMsg("Service Error : " + oRep.StatusCode, 2);
                //    oW_dtTmp = null;
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + " (" + oRep.StatusCode + ")", 2); //*Arm 63-08-19
                //    new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData/Error : " + oRep.StatusCode);
                //    this.Close();
                //}
                oCall.C_PRCxCloseConn(); //*Arm 63-07-20
            }
            catch (Exception oEx)
            {
                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"), 2); //*Arm 63-08-19
                new cLog().C_WRTxLog("wVehicle", "W_CALxApiGetData : " + oEx.Message);
                oW_dtTmp = null;
            }
            finally
            {
                oCall = null;
                oRep = null;
                oResult = null;
                //new cSP().SP_CLExMemory();
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
                new cLog().C_WRTxLog("wVehicle", "ocmBack_Click : " + oEx.Message);
            }
        }

        private void ocmSchPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (oW_dtTmp != null && oW_dtTmp.Rows.Count > 0)
                {
                    if (nW_CurPage > 1)
                    {
                        nW_CurPage = nW_CurPage - 1;
                        nW_Start = ((nW_CurPage * nW_PerPage) - nW_PerPage) + 1;
                        nW_Stop = (nW_CurPage * nW_PerPage);

                        W_SHWxDataVehicle();
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "ocmSchPrevious_Click : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmSchNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (oW_dtTmp != null && oW_dtTmp.Rows.Count > 0)
                {
                    if (nW_CurPage < nW_TotalPage)
                    {
                        nW_CurPage = nW_CurPage + 1;
                        nW_Start = ((nW_CurPage * nW_PerPage) - nW_PerPage) + 1;
                        nW_Stop = (nW_CurPage * nW_PerPage);

                        W_SHWxDataVehicle();
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "ocmSchNextPage_Click : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                if(nW_RowSal > 0)
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();
                    oSql.AppendLine("UPDATE " + cSale.tC_TblSalHDCst + " WITH(ROWLOCK) SET ");
                    oSql.AppendLine("FTXshCtrName = '"+ ogdVeh.GetData(nW_RowSal, "otbColVIN").ToString() +"'");
                    oSql.AppendLine("WHERE FTBchCode ='"+ cVB.tVB_BchCode +"' AND FTXshDocNo = '"+ cVB.tVB_DocNo +"' ");
                    oDB.C_SETxDataQuery(oSql.ToString());
                }
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "ocmAccept_Click : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                switch (poGD.Name)
                {
                    case "ogdVeh":
                        nWidth = poGD.Width;
                        poGD.Cols["otbColVIN"].Width = nWidth * 25 / 100;
                        poGD.Cols["otbColEngine"].Width = nWidth * 25 / 100;
                        poGD.Cols["otbColModel"].Width = nWidth * 25 / 100;
                        poGD.Cols["otbColSaleDate"].Width = nWidth * 25 / 100;

                        poGD.Cols["otbColVIN"].Caption = oW_Resource.GetString("tTitleVehVIN");
                        poGD.Cols["otbColEngine"].Caption = oW_Resource.GetString("tTitleVehEngine");
                        poGD.Cols["otbColModel"].Caption = oW_Resource.GetString("tTitleVehModel");
                        poGD.Cols["otbColSaleDate"].Caption = oW_Resource.GetString("tTitleVehSaledate");

                        poGD.Cols["otbColVIN"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColEngine"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColModel"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColSaleDate"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["otbColVIN"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["otbColEngine"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["otbColModel"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["otbColSaleDate"].TextAlign = TextAlignEnum.LeftCenter;

                        poGD.Editor.BackColor = Color.White;

                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "W_SETxColGrid : " + oEx.Message);
            }
            finally
            {
                poGD = null;
                //new cSP().SP_CLExMemory();
            }
        }

        private void ogdVeh_Click(object sender, EventArgs e)
        {
            try
            {
                nW_RowSal = Convert.ToInt32(ogdVeh.RowSel.ToString());
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "ogdVeh_Click : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ogdVeh_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                nW_RowSal = Convert.ToInt32(ogdVeh.RowSel.ToString());
                ocmAccept_Click(ocmAccept, null);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wVehicle", "ogdVeh_DoubleClick : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
    }
}
