using AdaPos.Class;
using AdaPos.Models.Webservice.Required.Customer;
using AdaPos.Models.Webservice.Respond.Customer;
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
    public partial class wCstList : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;

        public wCstList(cmlResSKCCstByList poCstList)
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                
                W_SETxDesign();
                W_SETxText();
                W_DATxLoadCst(poCstList);
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
                new cSP().SP_SETxSetGridviewFormat(ogdCst);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_SETxDesign : " + oEx.Message);
            }
            finally
            {
                oW_SP.SP_CLExMemory();
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
                new cLog().C_WRTxLog("wCstList", "W_SETxText : " + oEx.Message);
            }
        }
        
        public void W_DATxLoadCst(cmlResSKCCstByList poCstList)
        {
            List<cmlDatas> oData;
            try
            {
                oData = new List<cmlDatas>();
                oData = poCstList.Datas;

                foreach (cmlDatas oRow in oData)
                {
                    ogdCst.Rows.Add(oRow.KubotaID, oRow.FirstName+" "+oRow.LastName);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_DATxLoadCst : " + oEx.Message);
            }
            finally
            {
                oW_SP.SP_CLExMemory();
            }
        }

        private void ogdCst_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "ogdCst_CellContentClick : " + oEx.Message);
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
            cmlReqChooseCst oReqCst;
            cmlResSKCCstByCst oResCstByCst;
            string[] aErrorCode = { "001", "700", "701", "706", "800", "905" };
            string[] aErrorDesc = {"success.", "all parameter is null.", "validate parameter model false.", "product retail price not allow less than 0.", "data not found.", "cannot connect database." };
            
            cClientService oCall;
            HttpResponseMessage oRep;
            string tJSonCall = "";
            string tUrl = "";
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_APIKADS))
                {
                    //W_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlKADSNotDefine"), 3);
                    new cLog().C_WRTxLog("wCstList", "ocmSearch_Click/URL API is Empty ...");
                }
                else
                {
                    if (W_VerifyReq(1) == false) return;

                    oReqCst = new cmlReqChooseCst();
                    oReqCst.KubotaID = ptKubotaID;
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
                        new cLog().C_WRTxLog("wCstList", "W_PRCxKADSCustomerSearch/Call API Error : " + oEx.Message);
                        return;
                    }

                    if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                        oResCstByCst = new cmlResSKCCstByCst();
                        oResCstByCst = JsonConvert.DeserializeObject<cmlResSKCCstByCst>(tJSonRes);
                        
                        if(oResCstByCst.ErrorCode == "001")
                        {
                            wCstDetail oCstDetail = new wCstDetail(oResCstByCst);
                            oCstDetail.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            string tDesc = "";
                            for(int n=0; n<=5;n++)
                            {
                                if(aErrorCode[n].ToString() == oResCstByCst.ErrorCode)
                                {
                                    tDesc = aErrorDesc[n].ToString();
                                    break;
                                }
                            }
                            new cLog().C_WRTxLog("wCstList", "W_PRCxKADSCustomerSearch/API Response Error " + oResCstByCst.ErrorCode + " : "+ tDesc);
                        }
                    }
                }
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_PRCxKADSCustomerSearch : " + oEx.Message.ToString());
            }
            finally
            {
                oReqCst = null;
                oResCstByCst = null;
                oCall = null;
                oRep = null;
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
            try
            {
                cmlResSKCCstByCst oCst = new cmlResSKCCstByCst();
                wCstDetail oCstDetail = new wCstDetail(oCst);
                oCstDetail.ShowDialog();

                //W_PRCxKADSCustomerSearch(ogdCst.CurrentRow.Cells[otbColCstCode.Name].ToString());
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "ocmAccept_Click : " + oEx.Message);
            }
        }
    }
}
