using AdaPos.Class;
using AdaPos.Models.Webservice.Required.User;
using AdaPos.Models.Webservice.Respond.Base;
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
    public partial class wChangePassword : Form
    {
        private ResourceManager oW_Resource;
        private cSP oW_SP;
        private string tW_OldPwdEnc;
        private string tW_UsrPwd;
        private string tW_UsrLogin;
        private string tW_UsrCode;
        private bool bW_Result = false;
        private string tW_SinginType;

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="ptOldPwdEnc"></param>
        /// <param name="ptUsrPwd"></param>
        /// <param name="ptUsrLogin"></param>
        /// <param name="ptUsrCode"></param>
        public wChangePassword(string ptOldPwdEnc,string ptUsrPwd, string ptUsrLogin, string ptUsrCode, string ptSinginType)
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                tW_UsrPwd = ptUsrPwd;
                tW_UsrLogin = ptUsrLogin;
                tW_UsrCode = ptUsrCode;
                tW_OldPwdEnc = ptOldPwdEnc;
                tW_SinginType = ptSinginType; //*Arm 63-08-12
                W_SETxDesign();
                W_SETxText();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "wChangePassword : " + oEx.Message.ToString());
            }
        }

        #region Function / Method

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
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                if(cVB.nVB_Language == 1)
                {

                }
                else
                {
                    olaTitleChangePassword.Font = new Font(olaTitleChangePassword.Font.FontFamily, 12, FontStyle.Bold);
                }

                switch (tW_SinginType)
                {
                    case "1": //Password
                        otbOldPwd.MaxLength = 20;
                        otbNewPwd.MaxLength = 20;
                        otbConfirmPwd.MaxLength = 20;
                        break;
                    case "2": //PinCode
                        otbOldPwd.MaxLength = 6;
                        otbNewPwd.MaxLength = 6;
                        otbConfirmPwd.MaxLength = 6;
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "W_SETxDesign : " + oEx.Message.ToString());
            }
        }


        /// <summary>
        /// Set text form
        /// </summary>
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

                switch (tW_SinginType)
                {
                    case "1": //Password
                        olaTitleChangePassword.Text = oW_Resource.GetString("tTitleChangePassword");
                        olaOldPwd.Text = oW_Resource.GetString("tChgPwdOld");
                        olaNewPwd.Text = oW_Resource.GetString("tChgPwdNew");
                        olaConfirmPwd.Text = oW_Resource.GetString("tChgPwdConfirm");
                        break;
                    case "2": //PinCode
                        olaTitleChangePassword.Text = oW_Resource.GetString("tTitleChangePinCode");
                        olaOldPwd.Text = oW_Resource.GetString("tChgPINOld");
                        olaNewPwd.Text = oW_Resource.GetString("tChgPINNew");
                        olaConfirmPwd.Text = oW_Resource.GetString("tChgPINConfirm");
                        break;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "W_SETxText : " + oEx.Message.ToString());
            }
        }

        /// <summary>
        /// ตรวจสอบข้อมูล
        /// </summary>
        /// <returns></returns>
        private bool W_CHKbVerifyPwd()
        {
            string tMsgOldPwd = "";
            string tMsgNewPwd = "";
            string tMsgComfPwd = "";

            try
            {
                switch (tW_SinginType)
                {
                    case "1":
                        tMsgOldPwd = oW_Resource.GetString("tChgPwdOld");
                        tMsgNewPwd = oW_Resource.GetString("tChgPwdNew");
                        tMsgComfPwd = oW_Resource.GetString("tChgPwdConfirm");
                        break;

                    case "2":
                        tMsgOldPwd = oW_Resource.GetString("tChgPINOld");
                        tMsgNewPwd = oW_Resource.GetString("tChgPINNew");
                        tMsgComfPwd = oW_Resource.GetString("tChgPINConfirm");
                        break;
                }

                if (string.IsNullOrEmpty(otbOldPwd.Text)) 
                {
                    //Error Old Password is null
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgChgPwdReqPram")+" "+ tMsgOldPwd, 3);
                    otbOldPwd_Click(otbOldPwd, null);
                    return false;
                }
                if (string.IsNullOrEmpty(otbNewPwd.Text))
                {
                    //Error new Password is null
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgChgPwdReqPram") + " " + tMsgNewPwd, 3);
                    otbNewPwd_Click(otbNewPwd, null);
                    return false;
                }
                if (string.IsNullOrEmpty(otbConfirmPwd.Text))
                {
                    //Error Cnofrim Password is null
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgChgPwdReqPram") + " " + tMsgComfPwd, 3);
                    otbConfirmPwd_Click(otbConfirmPwd, null);
                    return false;
                }
                
                if (otbOldPwd.Text == tW_UsrPwd)    //ตรวจสอบ Password เดิม
                {
                    switch (tW_SinginType)
                    {
                        case "1":
                            //น้อยกว่า 8
                            if (!string.IsNullOrEmpty(otbNewPwd.Text))
                            {
                                if (otbNewPwd.TextLength < 8)
                                {
                                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgReqLengthPwd"), 3);
                                    otbNewPwd.Clear();
                                    otbConfirmPwd.Clear();
                                    otbNewPwd_Click(otbNewPwd, null);
                                    return false;
                                }
                            }

                            break;
                        case "2":
                            //น้อยกว่า 6
                            if (!string.IsNullOrEmpty(otbNewPwd.Text))
                            {
                                if (otbNewPwd.TextLength < 6)
                                {
                                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgReqLengthPIN"), 3);
                                    otbNewPwd.Clear();
                                    otbConfirmPwd.Clear();
                                    otbNewPwd_Click(otbNewPwd, null);
                                    return false;
                                }
                            }
                            break;
                    }

                    if (otbNewPwd.Text == otbConfirmPwd.Text) //ตรวจสอบ Confirm Password ใหม่
                    {
                        //*Arm 63-08-31 ตรวจสอบ Password ใหม่ห้ามตรงกับ Password เก่า 
                        if(otbNewPwd.Text == otbOldPwd.Text) 
                        {
                            // กรณี Password ใหม่ เหมือนกับ Password เก่า 
                            switch (tW_SinginType)
                            {
                                case "1":
                                    tMsgOldPwd = oW_Resource.GetString("tChgPwdOld");
                                    tMsgNewPwd = oW_Resource.GetString("tChgPwdNew");
                                    tMsgComfPwd = oW_Resource.GetString("tChgPwdConfirm");
                                    oW_SP.SP_SHWxMsg(string.Format(oW_Resource.GetString("tMsgChgPwdNewSameOld"), oW_Resource.GetString("tChgPwdNew"), oW_Resource.GetString("tChgPwdOld"))+ "!!!", 3);
                                    break;

                                case "2":
                                    tMsgOldPwd = oW_Resource.GetString("tChgPINOld");
                                    tMsgNewPwd = oW_Resource.GetString("tChgPINNew");
                                    tMsgComfPwd = oW_Resource.GetString("tChgPINConfirm");
                                    oW_SP.SP_SHWxMsg(string.Format(oW_Resource.GetString("tMsgChgPwdNewSameOld"), oW_Resource.GetString("tChgPINNew"), oW_Resource.GetString("tChgPINOld")) + "!!!", 3);
                                    break;
                            }
                            
                            return false;
                        }
                        else
                        {
                            // ตรวจสอบผ่าน
                            return true;
                        }
                        //++++++++++++++
                    }
                    else
                    {

                        //Error Confrim Password incorrect 
                        oW_SP.SP_SHWxMsg(tMsgComfPwd + " " + oW_Resource.GetString("tMsgChgPwdIncorrect")+ "!!!", 3);
                        otbConfirmPwd.Clear();
                        otbConfirmPwd_Click(otbConfirmPwd, null);
                        return false;
                    }
                }
                else
                {
                    //Error Old Password incorrect 
                    oW_SP.SP_SHWxMsg(tMsgOldPwd + " " + oW_Resource.GetString("tMsgChgPwdIncorrect") + " !!!", 2);
                    otbOldPwd.Clear();
                    otbOldPwd_Click(otbOldPwd, null);
                    return false;
                }
                
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "W_CHKbVerifyPwd : " + oEx.Message.ToString());
                return false;
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Call API Change Password
        /// </summary>
        private void W_PRCbChangePassword()
        {
            StringBuilder oSql;
            cDatabase oDB;
            cClientService oCall;
            HttpResponseMessage oRep;
            cmlReqPwdChange oReq;
            cmlResBase oRes;
            string tUrl = "";
            string tFunc = "";
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsrname"), 3);
                    return;
                }
                else
                {
                    tUrl = cVB.tVB_API2PSMaster;
                }

                tFunc = "/USER/ChangePassword";
                oSql = new StringBuilder();
                oDB = new cDatabase();

                //Set Requst Parameter
                oReq = new cmlReqPwdChange();
                oReq.ptUsrCode = tW_UsrCode;
                oReq.ptUsrLogin = tW_UsrLogin;
                oReq.ptOldPwd = otbOldPwd.Text;
                oReq.ptNewPwd = otbNewPwd.Text;
                oReq.ptLoginType = tW_SinginType; //*Arm 63-08-12
                string tJSonCall = JsonConvert.SerializeObject(oReq);

                oCall = new cClientService();
                oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                oRep = new HttpResponseMessage();
                try
                {
                    oRep = oCall.C_POSToInvoke(tUrl + tFunc, tJSonCall);
                }
                catch (Exception oEx)
                {
                    new cLog().C_WRTxLog("wChangePassword", "W_PRCbChangePassword/Call API Error : " + oEx.Message);
                    return;
                }

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    oRes = new cmlResBase();
                    oRes = JsonConvert.DeserializeObject<cmlResBase>(tJSonRes);

                    switch (oRes.rtCode)
                    {
                        case "1":

                            string tPwdEnc = new cEncryptDecrypt("2").C_CALtEncrypt(otbNewPwd.Text);

                            //Update TCNMUsrLogin
                            oSql.Clear();
                            oSql.AppendLine("UPDATE TCNMUsrLogin WITH(ROWLOCK) SET ");
                            oSql.AppendLine("FTUsrLoginPwd = '" + tPwdEnc + "',");
                            oSql.AppendLine("FTUsrStaActive = '1' ");
                            oSql.AppendLine("WHERE FTUsrCode = '" + tW_UsrCode + "' ");
                            oSql.AppendLine("AND FTUsrLogin ='" + tW_UsrLogin + "' ");
                            oSql.AppendLine("AND FTUsrLoginPwd = '" + tW_OldPwdEnc + "' ");
                            oSql.AppendLine("AND FTUsrLogType = '" + tW_SinginType + "' "); //*Arm 63-08-12

                            oDB.C_SETxDataQuery(oSql.ToString());
                            bW_Result = true;
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgChgPwdSuccess"), 1);
                            break;
                            
                        default:
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgErrChgPwdUnSuccess"), 3);
                            break;
                    }
                }
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "W_PRCbChangePassword : " + oEx.Message.ToString());
            }
            finally
            {
                oSql = null;
                oDB = null;
                oReq = null;
                oRep = null;
                oRes = null;
                oCall = null;
                //new cSP().SP_CLExMemory();
            }
        }
        #endregion End Function / Method


        /// <summary>
        /// Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "ocmBack_Click : " + oEx.Message.ToString());
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if(W_CHKbVerifyPwd() == true)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    W_PRCbChangePassword();
                    Cursor.Current = Cursors.Default;

                    if (bW_Result == true)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "ocmAccept_Click : " + oEx.Message.ToString());
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        
        /// <summary>
        /// Show Keboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_PRCxCallByName("C_KBDxKeyboard");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "ocmShwKb_Click : " + oEx.Message.ToString());
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void otbOldPwd_Click(object sender, EventArgs e)
        {
            try
            {
                uNumpadPwd.oU_TextValue = otbOldPwd;
                otbOldPwd.Focus();
                otbOldPwd.SelectionStart = 0;
                otbOldPwd.SelectAll();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "otbOldPwd_Click : " + oEx.Message.ToString());
            }
        }

        private void otbNewPwd_Click(object sender, EventArgs e)
        {
            try
            {
                uNumpadPwd.oU_TextValue = otbNewPwd;
                otbNewPwd.Focus();
                otbNewPwd.SelectionStart = 0;
                otbNewPwd.SelectAll();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "otbNewPwd_Click : " + oEx.Message.ToString());
            }
        }

        private void otbConfirmPwd_Click(object sender, EventArgs e)
        {
            try
            {
                uNumpadPwd.oU_TextValue = otbConfirmPwd;
                otbConfirmPwd.Focus();
                otbConfirmPwd.SelectionStart = 0;
                otbConfirmPwd.SelectAll();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "otbConfirmPwd_Click : " + oEx.Message.ToString());
            }
        }
        
        private void wChangePassword_Shown(object sender, EventArgs e)
        {
            try
            {
                otbOldPwd_Click(otbOldPwd, null);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChangePassword", "wChangePassword_Shown : " + oEx.Message.ToString());
            }
        }
    }
}
