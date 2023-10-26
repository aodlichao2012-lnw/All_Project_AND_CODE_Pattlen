using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.DatabaseTmp;
using AdaPos.Models.Webservice.Required.SaleDocRefer;
using AdaPos.Models.Webservice.Respond.SaleDocRefer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wSale
{
    public partial class wSearchDoc : Form
    {
        private string tW_DocNo;
        private DateTime dW_Date;   //*Arm 63-06-01
        private int nW_RefMode;     //*Arm 63-06-01
        public wSearchDoc(DateTime pdDate, int pnRefMode =2)
        {
            InitializeComponent();
            try
            {
                nW_RefMode = pnRefMode; //*Arm 63-06-01
                dW_Date = pdDate;       //*Arm 63-06-01
                W_SETxDesign();
                W_SETxText();

                W_SCHnSearchByAPI();    //* ค้นหาเอกสารทั้งหมดจากหลังบ้านภายใต้สาขา และภายในวันที่ที่เลือก ผ่าน API2ARDoc (*Arm 63-06-02)
                W_DATxLoadData();       //* เตรียมข้อมูลแสดงใน GridView  (*Arm 63-06-02)
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "wSearchDoc : " + oEx.Message); }
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

                ocmSearch.BackColor = cVB.oVB_ColNormal;
                //ogdDoc.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdDoc); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                olaTitleAbout.Text = cVB.oVB_GBResource.GetString("tSchDoc"); //*Arm 62-12-20
                //olaTitleSearch.Text = cVB.oVB_GBResource.GetString("tSchDoc");
                olaTitleSearch.Text = cVB.oVB_GBResource.GetString("tSearch");
                //otbTitleAmount.HeaderText = cVB.oVB_GBResource.GetString("tAmount");
                //otbTitleDatetime.HeaderText = cVB.oVB_GBResource.GetString("tDatetime");
                //otbTitleDocNo.HeaderText = cVB.oVB_GBResource.GetString("tDocNo");
                //otbTitlePos.HeaderText = cVB.oVB_GBResource.GetString("tPos");
                W_SETxGridColumns();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "W_SETxText : " + oEx.Message); }
        }
        private void W_DATxLoadData()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalHD> aHD = new List<cmlTPSTSalHD>();
            DataTable odtTmp = new DataTable();
            try
            {
                //*ARM 62-12-19  [Comment Code]
                //oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand");
                //oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FNXshDocType = 1");
                //if (!string.IsNullOrEmpty(otbTicketNo.Text))
                //{
                //    oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                //}
                //oSql.AppendLine("ORDER BY FDCreateOn DESC");
                //aHD = oDB.C_GETaDataQuery<cmlTPSTSalHD>(oSql.ToString());
                //ogdDoc.Rows.Clear();
                //foreach (cmlTPSTSalHD oHD in aHD)
                //{
                //    ogdDoc.Rows.Add(oHD.FTXshDocNo, oHD.FDXshDocDate, oHD.FTPosCode, oHD.FCXshGrand);
                //}
                //*ARM 62-12-19  [Comment Code]


                //*Arm 62-12-19 Check Option การคืน 
                //================================
                //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                //2:คืนได้ครังเดียว บางรายการได้ 
                //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 
                //5:ห้ามคืน
                //switch (cVB.nVB_ReturnType)
                //{
                //    case 1:   //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                //        oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand");
                //        oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FNXshDocType = 1");
                //        oSql.AppendLine("AND FTXshStaRefund != '2' ");
                //        if (!string.IsNullOrEmpty(otbTicketNo.Text))
                //        {
                //            oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                //        }
                //        oSql.AppendLine("ORDER BY FDCreateOn DESC");
                //        break;

                //    case 2:   //2:คืนได้ครังเดียว บางรายการได้ 
                //        oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand");
                //        oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FNXshDocType = 1");
                //        oSql.AppendLine("AND FTXshStaRefund != '2' ");
                //        if (!string.IsNullOrEmpty(otbTicketNo.Text))
                //        {
                //            oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                //        }
                //        oSql.AppendLine("ORDER BY FDCreateOn DESC");
                //        break;

                //    case 3:   //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                //        oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand");
                //        oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FNXshDocType = 1");
                //        oSql.AppendLine("AND FNXshStaRef != '2' ");
                //        if (!string.IsNullOrEmpty(otbTicketNo.Text))
                //        {
                //            oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                //        }
                //        oSql.AppendLine("ORDER BY FDCreateOn DESC");
                //        break;

                //    case 4:   //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 
                //        oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand");
                //        oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FNXshDocType = 1");
                //        if (!string.IsNullOrEmpty(otbTicketNo.Text))
                //        {
                //            oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                //        }
                //        oSql.AppendLine("ORDER BY FDCreateOn DESC");
                //        break;

                //}
                //oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand");
                //oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FNXshDocType = 1");
                //switch (cVB.nVB_ReturnType)
                //{
                //    case 1:   //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                //        oSql.AppendLine("AND FTXshStaRefund != '2' ");
                //        break;

                //    case 2:   //2:คืนได้ครังเดียว บางรายการได้ 
                //        oSql.AppendLine("AND FTXshStaRefund != '2' ");
                //        break;

                //    case 3:   //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                //        oSql.AppendLine("AND FNXshStaRef != '2' ");
                //        break;

                //    case 4:   //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 
                       
                //        break;
                //}
                //oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                //oSql.AppendLine("ORDER BY FDCreateOn DESC");

                //aHD = oDB.C_GETaDataQuery<cmlTPSTSalHD>(oSql.ToString());
                //ogdDoc.Rows.Clear(); //*Net 63-03-02
                //foreach (cmlTPSTSalHD oHD in aHD)
                //{
                //    ogdDoc.Rows.Add(oHD.FTXshDocNo, oHD.FDXshDocDate, oHD.FTPosCode, oHD.FCXshGrand);
                //}


                //*Arm 63-06-01
                //***************************************
                oSql.Clear();
                oSql.AppendLine("SELECT FTXshDocNo AS otbTitleDocNo, FDXshDocDate AS otbTitleDatetime,FTPosCode AS otbTitlePos,FCXshGrand AS otbTitleAmount");
                oSql.AppendLine("FROM( ");
                //back office
                oSql.AppendLine("     SELECT FTBchCode,FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand,FNXshDocType,FTXshStaRefund,FNXshStaRef,FDCreateOn");
                oSql.AppendLine("     FROM TPSTSalHDTmp WITH(NOLOCK)");
                oSql.AppendLine("     WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTPosCode != '"+cVB.tVB_PosCode+"' ");
                oSql.AppendLine("     UNION");
                //Transaction
                oSql.AppendLine("     SELECT FTBchCode, FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand,FNXshDocType,FTXshStaRefund,FNXshStaRef,FDCreateOn");
                oSql.AppendLine("     FROM TPSTSalHD WITH(NOLOCK)");
                oSql.AppendLine("     WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("     UNION");
                //Temp
                oSql.AppendLine("     SELECT FTBchCode,FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand,FNXshDocType,FTXshStaRefund,FNXshStaRef,FDCreateOn");
                oSql.AppendLine("     FROM " + cSale.tC_TblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("     WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshStaDoc = '1' "); // FTXshStaDoc เช็คเอกสารที่สมบูรณ์
                oSql.AppendLine(")HD");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND CONVERT(Date,FDXshDocDate,121) = '" + string.Format("{0: yyyy-MM-dd}", dW_Date).ToString() + "' ");
                if (nW_RefMode == 1) // ขายอ้างอิง บิลคืน
                {
                    oSql.AppendLine("AND FNXshDocType = 9");
                    oSql.AppendLine("AND FNXshStaRef != '2' ");
                }
                else    // คืนอ้างอิง บิลขาย
                {
                    oSql.AppendLine("AND FNXshDocType = 1");
                    switch (cVB.nVB_ReturnType)
                    {
                        case 1:   //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                        case 2:   //2:คืนได้ครังเดียว บางรายการได้ 
                            oSql.AppendLine("AND FTXshStaRefund != '2' ");
                            break;
                        case 3:   //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                            oSql.AppendLine("AND FNXshStaRef != '2' ");
                            break;
                        default:   //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 
                            break;
                    }
                }
                oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                oSql.AppendLine("ORDER BY FDCreateOn DESC");
                
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                
                ogdDoc.Columns.Clear();
                ogdDoc.DataSource = odtTmp;
                ogdDoc.Refresh();
                W_SETxGridColumns();
                //this.ogdDoc.Sort(this.ogdDoc.Columns[otbTitleDatetime.Name], ListSortDirection.Descending);
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "W_DATxLoadData : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                aHD = null;
                new cSP().SP_CLExMemory();
            }
        }

        private void W_SETxGridColumns()
        {
            try
            {
                ogdDoc.Columns[otbTitleDocNo.Name].Visible = true;
                ogdDoc.Columns[otbTitleDatetime.Name].Visible = true;
                ogdDoc.Columns[otbTitlePos.Name].Visible = true;
                ogdDoc.Columns[otbTitleAmount.Name].Visible = true;

                ogdDoc.Columns[otbTitleDocNo.Name].FillWeight = 100;
                ogdDoc.Columns[otbTitleDatetime.Name].FillWeight = 80;
                ogdDoc.Columns[otbTitlePos.Name].FillWeight = 60;
                ogdDoc.Columns[otbTitleAmount.Name].FillWeight = 60;
               

                ogdDoc.Columns[otbTitleDocNo.Name].HeaderText = cVB.oVB_GBResource.GetString("tDocNo");
                ogdDoc.Columns[otbTitleDatetime.Name].HeaderText = cVB.oVB_GBResource.GetString("tDatetime");
                ogdDoc.Columns[otbTitlePos.Name].HeaderText = cVB.oVB_GBResource.GetString("tPos");
                ogdDoc.Columns[otbTitleAmount.Name].HeaderText = cVB.oVB_GBResource.GetString("tAmount");
                
                ogdDoc.Columns[otbTitleDocNo.Name].ReadOnly = true;
                ogdDoc.Columns[otbTitleDatetime.Name].ReadOnly = true;
                ogdDoc.Columns[otbTitlePos.Name].ReadOnly = true;
                ogdDoc.Columns[otbTitleAmount.Name].ReadOnly = true;


                ogdDoc.Columns[otbTitleDocNo.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ogdDoc.Columns[otbTitleDatetime.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ogdDoc.Columns[otbTitlePos.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                ogdDoc.Columns[otbTitleAmount.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "W_SETxGridColumns : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        #endregion End Function

        #region Method/Events
        /// <summary>
        /// Close Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Accept
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tW_DocNo))
                {
                    return;
                }
                //cVB.tVB_DocNo = tW_DocNo;
                cVB.tVB_RefDocNo = tW_DocNo; // *Arm 63-06-04
                this.DialogResult = DialogResult.OK;// *Arm 63-06-04
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "ocmAccept_Click : " + oEx.Message); }
        }

        private void wSearchDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "wSearchDoc_FormClosing : " + oEx.Message); }
        }

        private void ogdDoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;
                if (e.RowIndex < 0) return;
                tW_DocNo = ogdDoc.Rows[e.RowIndex].Cells["otbTitleDocNo"].Value.ToString();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "ogdDoc_CellClick : " + oEx.Message); }
        }

        #endregion End Method/Events

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                W_DATxLoadData();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "ocmSearch_Click : " + oEx.Message); }
        }

        private void ogdDoc_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ocmAccept_Click(ocmAccept, new EventArgs());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "ogdDoc_CellDoubleClick : " + oEx.Message); }
        }

        /// <summary>
        /// call API2ARDoc ค้นหาเอกสารการขายคืน (*Arm 63-06-02)
        /// </summary>
        public void W_SCHnSearchByAPI()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalHD> aHD = new List<cmlTPSTSalHD>();
            cmlReqSaleDwn oReq;
            cmlResItemSaleDwn oRes;
            cClientService oCall;
            HttpResponseMessage oRep;
            string tJSonCall;
            string tUrl;
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_API2ARDoc)) // Check Url API2ArDoc
                {
                    new cLog().C_WRTxLog("wSearchDoc", "W_SCHnSearchByAPI/URL API2ARDoc is null or empty ...");
                    return;
                }

                tUrl = cVB.tVB_API2ARDoc + "/SaleDocRefer/Data";
                oReq = new cmlReqSaleDwn();
                oRes = new cmlResItemSaleDwn();

                // 2. Set Request Parameter 
                oReq.ptBchCode = cVB.tVB_BchCode;
                oReq.pdSaleDate = dW_Date;
                oReq.ptDocNo = "";

                if (nW_RefMode == 1) oReq.pnDoctype = 9; //ขายอ้างอิงบิลคืน
                else oReq.pnDoctype = 1;              //คืนอ้างอิงบิลขาย

                tJSonCall = JsonConvert.SerializeObject(oReq);

                new cLog().C_WRTxLog("wSearchDoc", " W_SCHnSearchByAPI : Call API2ARDoc start...");
                oCall = new cClientService();
                oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);
                oRep = new HttpResponseMessage();
                try
                {
                    oRep = oCall.C_POSToInvoke(tUrl, tJSonCall);
                }
                catch (Exception oEx)
                {
                    new cLog().C_WRTxLog("wSearchDoc", "W_SCHnSearchByAPI/Call API2ARDoc Error : " + oEx.Message);
                    return;
                }
                new cLog().C_WRTxLog("wSearchDoc", " W_SCHnSearchByAPI : Call API2ARDoc End...");

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    oRes = JsonConvert.DeserializeObject<cmlResItemSaleDwn>(tJSonRes);

                    if (oRes.rtCode == "001")
                    {
                        if (oRes.roItem.aoTPSTSalHD.Count > 0)
                        {
                            new cLog().C_WRTxLog("wSearchDoc", " W_SCHnSearchByAPI : W_PRCbInsert2Temp Start...");
                            W_PRCbInsert2Temp(oRes);
                            new cLog().C_WRTxLog("wSearchDoc", " W_SCHnSearchByAPI : W_PRCbInsert2Temp End...");
                            
                        } // end Check aoTPSTSalHD.Count > 0
                    } // end check oRes.rtCode = 001
                    else
                    {
                        new cLog().C_WRTxLog("wSearchDoc", "W_SCHnSearchByAPI/API2ARDoc Response Code " + oRes.rtCode + " " + oRes.rtDesc);
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchDoc", "W_SCHnSearchByAPI : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                aHD = null;
                oReq = null;
                oCall = null;
                oRep = null;
                oRes = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Insert HD ที่ได้มาจาก API ลง ตาราง Temp (*Arm 63-06-02)
        /// </summary>
        /// <param name="poData"></param>
        /// <returns></returns>
        public bool W_PRCbInsert2Temp(cmlResItemSaleDwn poData)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            SqlTransaction oTranscation;

            List<cmlTPSTSalHDTmp> aoHD;
            cDataReaderAdapter<cmlTPSTSalHDTmp> oHD;

            try
            {
                new cLog().C_WRTxLog("wReferBill", " W_PRCbInsert2Temp : Insert Data to TableTemp Start...");

                //** PROCESS STEP (Arm 63-05-23)
                //=====================================================
                // ถ้า Event เกิดจากการกดปุ่ม Borws จะทำแค่ Insert HD อย่างเดียว
                // ถ้า Event เกิดจากการกดปุ่ม Accep จะทำทั้งหมด


                //1.Created Temp
                oSql = new StringBuilder();
                oDB = new cDatabase();

                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalHD", "TPSTSalHDTmp");
                
                oTranscation = cVB.oVB_ConnDB.BeginTransaction();

                //2.Bluk Copy

                // Bulk Copy : TPSTSalHD
                aoHD = C_PRCaListSalHD(poData.roItem.aoTPSTSalHD);
                oHD = new cDataReaderAdapter<cmlTPSTSalHDTmp>(aoHD);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oHD.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalHDTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oHD);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wSearchDoc", "W_PRCbInsert2Temp/TPSTSalHDTmp : " + oEx.Message);
                        return false;
                    }
                }
                
                oTranscation.Commit();
                new cLog().C_WRTxLog("wSearchDoc", " W_PRCbInsert2Temp : Insert Data to TableTemp End...");

                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchDoc", "W_PRCbInsert2Temp : " + oEx.Message.ToString());
                return false;
            }
            finally
            {
                oDB = null;
                oSql = null;
                oTranscation = null;
                aoHD = null;
                oHD = null;
                new cSP().SP_CLExMemory();
            }
        }

        private List<cmlTPSTSalHDTmp> C_PRCaListSalHD(List<cmlResInfoSalHD> paoSalHD)
        {
            List<cmlTPSTSalHDTmp> aoData = new List<cmlTPSTSalHDTmp>();
            try
            {
                aoData = paoSalHD.Select(oItem => new cmlTPSTSalHDTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FTShpCode = oItem.rtShpCode,
                    FNXshDocType = oItem.rnXshDocType,
                    FDXshDocDate = oItem.rdXshDocDate,
                    FTXshCshOrCrd = oItem.rtXshCshOrCrd,
                    FTXshVATInOrEx = oItem.rtXshVATInOrEx,
                    FTDptCode = oItem.rtDptCode,
                    FTWahCode = oItem.rtWahCode,
                    FTPosCode = oItem.rtPosCode,
                    FTShfCode = oItem.rtShfCode,
                    FNSdtSeqNo = oItem.rnSdtSeqNo,
                    FTUsrCode = oItem.rtUsrCode,
                    FTSpnCode = oItem.rtSpnCode,
                    FTXshApvCode = oItem.rtXshApvCode,
                    FTCstCode = oItem.rtCstCode,
                    FTXshDocVatFull = oItem.rtXshDocVatFull,
                    FTXshRefExt = oItem.rtXshRefExt,
                    FDXshRefExtDate = oItem.rdXshRefExtDate,
                    FTXshRefInt = oItem.rtXshRefInt,
                    FDXshRefIntDate = oItem.rdXshRefIntDate,
                    FTXshRefAE = oItem.rtXshRefAE,
                    FNXshDocPrint = oItem.rnXshDocPrint,
                    FTRteCode = oItem.rtRteCode,
                    FCXshRteFac = oItem.rcXshRteFac,
                    FCXshTotal = oItem.rcXshTotal,
                    FCXshTotalNV = oItem.rcXshTotalNV,
                    FCXshTotalNoDis = oItem.rcXshTotalNoDis,
                    FCXshTotalB4DisChgV = oItem.rcXshTotalB4DisChgV,
                    FCXshTotalB4DisChgNV = oItem.rcXshTotalB4DisChgNV,
                    FTXshDisChgTxt = oItem.rtXshDisChgTxt,
                    FCXshDis = oItem.rcXshDis,
                    FCXshChg = oItem.rcXshChg,
                    FCXshTotalAfDisChgV = oItem.rcXshTotalAfDisChgV,
                    FCXshTotalAfDisChgNV = oItem.rcXshTotalAfDisChgNV,
                    FCXshRefAEAmt = oItem.rcXshRefAEAmt,
                    FCXshAmtV = oItem.rcXshAmtV,
                    FCXshAmtNV = oItem.rcXshAmtNV,
                    FCXshVat = oItem.rcXshVat,
                    FCXshVatable = oItem.rcXshVatable,
                    FTXshWpCode = oItem.rtXshWpCode,
                    FCXshWpTax = oItem.rcXshWpTax,
                    FCXshGrand = oItem.rcXshGrand,
                    FCXshRnd = oItem.rcXshRnd,
                    FTXshGndText = oItem.rtXshGndText,
                    FCXshPaid = oItem.rcXshPaid,
                    FCXshLeft = oItem.rcXshLeft,
                    FTXshRmk = oItem.rtXshRmk,
                    FTXshStaRefund = oItem.rtXshStaRefund,
                    FTXshStaDoc = oItem.rtXshStaDoc,
                    FTXshStaApv = oItem.rtXshStaApv,
                    FTXshStaPrcStk = oItem.rtXshStaPrcStk,
                    FTXshStaPaid = oItem.rtXshStaPaid,
                    FNXshStaDocAct = oItem.rnXshStaDocAct,
                    FNXshStaRef = oItem.rnXshStaRef,
                    FDLastUpdOn = oItem.rdLastUpdOn,
                    FTLastUpdBy = oItem.rtLastUpdBy,
                    FDCreateOn = oItem.rdCreateOn,
                    FTCreateBy = oItem.rtCreateBy
                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchDoc", "C_PRCaListSalHD : " + oEx.Message); }
            finally
            {
                paoSalHD = null;
                new cSP().SP_CLExMemory();
            }

            return aoData;
        }
    }
}
