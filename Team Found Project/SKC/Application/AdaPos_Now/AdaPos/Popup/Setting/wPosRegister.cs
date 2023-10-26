using AdaPos.Class;
using AdaPos.Models.Webservice.Required.Pos;
using AdaPos.Models.Webservice.Respond.Base;
using AdaPos.Models.Webservice.Respond.Pos;
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

namespace AdaPos.Popup.Setting
{
    public partial class wPosRegister : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        public wPosRegister()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                W_SETxDesign();
                W_SETxText();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wPosRegister", "wPosRegister : " + oEx.Message);
            }
        }
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPosRegister", "wPosRegister : " + oEx.Message);
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
                olaTitlePosRegister.Text = oW_Resource.GetString("tTitlePosRegister");
                otbMacAddr.Text = oW_SP.SP_GETtMacAddress();
                otbComName.Text = Environment.MachineName.ToString();
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPosRegister", "W_SETxText : " + oEx.Message);
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
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPosRegister", "ocmBack_Click : " + oEx.Message);
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            cClientService oCall;
            HttpResponseMessage oRep;
            cmlResPosRegister oRes;
            cmlReqPosRegister oReq;
            string tFunc = "";
            string tUrl = "";
            string tJSonCall = "";
            string tMsgErr = "";
            try
            {
                if(string.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlMasterNotDefine"), 3);
                    return;
                }

                tFunc = "/Pos/PosRegister";
                tUrl = cVB.tVB_API2PSMaster + tFunc;

                oReq = new cmlReqPosRegister();
                oReq.ptBchCode = cVB.tVB_BchCode;
                oReq.ptMacAddress = otbMacAddr.Text;
                oReq.ptPosCode = cVB.tVB_PosCode;
                oReq.ptCompName = otbComName.Text;

                //Check Validate parameter.
                string tValidate = oW_Resource.GetString("tMsgReqPram");
                bool bValidate = true;
                if (string.IsNullOrEmpty(oReq.ptBchCode))
                {
                    bValidate = false;
                    if(cVB.nVB_Language == 1)
                    tValidate = Environment.NewLine + oW_Resource.GetString("tBchCode");
                }
                if (string.IsNullOrEmpty(oReq.ptMacAddress))
                {
                    bValidate = false;
                    tValidate = Environment.NewLine + "Mac Address No.";
                }
                if (string.IsNullOrEmpty(oReq.ptPosCode))
                {
                    bValidate = false;
                    tValidate = Environment.NewLine + oW_Resource.GetString("tPosCode");
                }
                if (string.IsNullOrEmpty(oReq.ptCompName))
                {
                    bValidate = false;
                    tValidate = Environment.NewLine + "Computer Name";
                }

                if(bValidate == true)
                {
                    tJSonCall = JsonConvert.SerializeObject(oReq);
                }
                else
                {
                    oW_SP.SP_SHWxMsg(tValidate, 3);
                    return;
                }
                
                oCall = new cClientService();
                oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);
                oRep = new HttpResponseMessage();
                try
                {
                    oRep = oCall.C_POSToInvoke(tUrl, tJSonCall);
                }
                catch (Exception oEx)
                {
                    new cLog().C_WRTxLog("wPosRegister", "ocmAccept_Click/Call API Error : " + oEx.Message);
                    return;
                }

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    oRes = new cmlResPosRegister();
                    oRes = JsonConvert.DeserializeObject<cmlResPosRegister>(tJSonRes);

                    switch (oRes.rtCode)
                    {
                        case "1":
                            //ส่ง Register สำเร็จ

                            // เข้ารหัส MacAddr 
                            string tMacAddr = new cEncryptDecrypt("2").C_CALtEncrypt(otbMacAddr.Text);

                            //บันทึกลง Field FTPrgRegToken ในตาราง TCNMPos
                            StringBuilder oSql = new StringBuilder();
                            oSql.AppendLine("UPDATE TCNMPos WITH(ROWLOCK) SET FTPrgRegToken ='" + tMacAddr + "' ");
                            oSql.AppendLine("WHERE FTBchCode ='" + cVB.tVB_BchCode + "' AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                            new cDatabase().C_SETxDataQuery(oSql.ToString());

                            // Message
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgPosRegSuccess"), 1);
                            this.Close();
                            break;

                        case "709":
                            tMsgErr = Environment.NewLine + oW_Resource.GetString("tBchCode") + " : " + oRes.rtBchCode;
                            tMsgErr += Environment.NewLine + oW_Resource.GetString("tPosCode") + " : " + oRes.rtPosCode;
                            oW_SP.SP_SHWxMsg(string.Format(oW_Resource.GetString("tMsgPosRegFalse"), "Mac Address No. ", tMsgErr), 2);
                            break;

                        case "710":
                            tMsgErr = Environment.NewLine + oW_Resource.GetString("tBchCode") + " : " + oRes.rtBchCode;
                            tMsgErr += Environment.NewLine + oW_Resource.GetString("tPosCode") + " : " + oRes.rtPosCode;
                            tMsgErr += Environment.NewLine + "Mac Address No. : " + oRes.rtMacAddr;
                            tMsgErr += Environment.NewLine + "Computer Name : " + oRes.rtCompName;
                            oW_SP.SP_SHWxMsg(string.Format(oW_Resource.GetString("tMsgPosRegFalse"), cVB.oVB_GBResource.GetString("tMsg_Pos"), tMsgErr), 2);
                            break;

                        default:
                            oW_SP.SP_SHWxMsg("Error : " + oRes.rtCode + " : " + oRes.rtDesc, 2);
                            new cLog().C_WRTxLog("wPosRegister", "ocmAccept_Click : Response Code" + oRes.rtCode + " : " + oRes.rtDesc);
                            break;
                    }
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wPosRegister", "ocmAccept_Click : " + oEx.Message);
            }
        }
    }
}
