using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Popup.All;
using AdaPos.Popup.Setting;
using AdaPos.Popup.wHome;
using AdaPos.Popup.wLogin;
using AdaPos.Popup.wPayment;
using AdaPos.Popup.wProduct;
using AdaPos.Popup.wSale;
using AdaPos.Popup.wSpotCheck;
using AdaPos.Popup.wTaxInvoice;
using AdaPos.Resources_String.Global;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace AdaPos.Class
{
    public class cFunctionKeyboard
    {
        #region Variable
        private cSP oW_SP;
        private ResourceManager oW_Resource;

        #endregion

        #region Constructor

        public cFunctionKeyboard()
        {

        }

        #endregion End Constructor

        #region Function

        /// <summary>
        /// Discount Amont
        /// </summary>
        /// <param name="poKey"></param>
        public void C_KBDxDisAmt()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm = new wDiscount(1, 1);
            bool Role = false;
            //ResourceManager oResource;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {

                cVB.tVB_KbdCallByName = "C_KBDxDisAmt";
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                ////tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        Role = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            Role = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (Role)
                {
                    ////*Net 63-07-31 ย้ายมาจาก wSale
                    //wChooseItemRef oChooseItem = new wChooseItemRef(1, "C_KBDxChgAmt");
                    //oChooseItem.ShowDialog();
                    //if (cSale.nC_DTSeqNo == 0)
                    //{
                    //    return;
                    //}
                    ////+++++++++++++++++++++++++++++++

                    //*Arm 63-08-18
                    cSP.SP_GETxListItem("C_KBDxDisAmt");
                    if (cSale.nC_DTSeqNo == 0)
                    {
                        return;
                    }
                    //+++++++++++++

                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ลดจำนวน หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxDisAmt", 1);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลด
                            {
                                //มูลค่าที่สามารถลดได้
                                cVB.oVB_PdtOrder.cSetPrice = oDiscPolicy.C_GETcGetAmtAlwDisc(1, cVB.tVB_StaPrice, cSale.nC_DTSeqNo); // Get Amount ที่สามารถลดได้

                                if (cVB.oVB_PdtOrder.cSetPrice > 0)
                                {
                                    // มียอดสำหรับการทำราการส่วนลด
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดสำหรับการทำราการส่วนลด
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนลด
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"),3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //******************** End ****************************
                    //*Arm 63-07-13 Check Discount Policy


                    //if (cSale.C_PRCxCheckChg())
                    //{
                    //    oFrm.ShowDialog();
                    //}
                    //else
                    //{
                    //    switch (cVB.nVB_Language)
                    //    {
                    //        case 1:     // TH
                    //            oResource = new ResourceManager(typeof(resGlobal_TH));
                    //            break;

                    //        default:    // EN
                    //            oResource = new ResourceManager(typeof(resGlobal_EN));
                    //            break;
                    //    }
                    //    MessageBox.Show(oResource.GetString("tMsgNotAlwDisAftChg"));
                    //}
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDisAmt : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oFrm.Dispose();
                //oResource = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Discount Percen
        /// </summary>
        /// <param name="poKey"></param>
        public void C_KBDxDisPer()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm = new wDiscount(2, 1);
            bool Role = false;
            //ResourceManager oResource;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต
            try
            {
                cVB.tVB_KbdCallByName = "C_KBDxDisPer";

                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                ////tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        Role = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            Role = true;
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (Role)
                {
                    ////*Net 63-07-31 ย้ายมาจาก wSale
                    //wChooseItemRef oChooseItem = new wChooseItemRef(1, "C_KBDxDisPer");
                    //oChooseItem.ShowDialog();
                    //if (cSale.nC_DTSeqNo == 0)
                    //{
                    //    return;
                    //}
                    ////+++++++++++++++++++++++++++

                    //*Arm 63-08-18
                    cSP.SP_GETxListItem("C_KBDxDisPer");
                    if (cSale.nC_DTSeqNo == 0)
                    {
                        return;
                    }
                    //+++++++++++++

                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ลด % หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxDisPer", 1);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลด
                            {
                                //มูลค่าที่สามารถลดได้
                                cVB.oVB_PdtOrder.cSetPrice = oDiscPolicy.C_GETcGetAmtAlwDisc(1, cVB.tVB_StaPrice, cSale.nC_DTSeqNo); // Get Amount ที่สามารถลดได้

                                if (cVB.oVB_PdtOrder.cSetPrice > 0)
                                {
                                    // มียอดสำหรับการทำราการส่วนลด
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดสำหรับการทำราการส่วนลด
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนลด
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //******************** End ****************************
                    //*Arm 63-07-13 Check Discount Policy


                    //if (cSale.C_PRCxCheckChg())
                    //{
                    //    //   oFrm.nW_DisType = 2;
                    //    oFrm.ShowDialog();
                    //}
                    //else
                    //{
                    //    switch (cVB.nVB_Language)
                    //    {
                    //        case 1:     // TH
                    //            oResource = new ResourceManager(typeof(resGlobal_TH));
                    //            break;

                    //        default:    // EN
                    //            oResource = new ResourceManager(typeof(resGlobal_EN));
                    //            break;
                    //    }
                    //    MessageBox.Show(oResource.GetString("tMsgNotAlwDisAftChg"));
                    //}
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDisPer : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oFrm.Dispose();
                //oResource = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Chart %
        /// </summary>
        /// <param name="poKey"></param>
        public void C_KBDxChgPer()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm = new wDiscount(4, 1);
            bool Role = false;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {

                cVB.tVB_KbdCallByName = "C_KBDxChgPer";

                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                ////tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        Role = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            Role = true;
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (Role)
                { 
                    ////*Net 63-07-31 ย้ายมาจาก wSale
                    //wChooseItemRef oChooseItem = new wChooseItemRef(1, "C_KBDxChgPer");
                    //oChooseItem.ShowDialog();
                    //if (cSale.nC_DTSeqNo == 0)
                    //{
                    //    return;
                    //}
                    ////  oFrm.nW_DisType = 4;
                    //oFrm.ShowDialog();

                    //*Arm 63-08-18
                    cSP.SP_GETxListItem("C_KBDxChgPer");
                    if (cSale.nC_DTSeqNo == 0)
                    {
                        return;
                    }
                    //+++++++++++++

                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ชาจน์ % หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxChgPer", 1);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode)) //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice)) //ตรวจสอบสถานะ Price ก่อนชาจน์ %
                            {
                                //มูลค่าที่สามารถลดได้
                                cVB.oVB_PdtOrder.cSetPrice = oDiscPolicy.C_GETcGetAmtAlwDisc(1, cVB.tVB_StaPrice, cSale.nC_DTSeqNo);

                                if (cVB.oVB_PdtOrder.cSetPrice >= 0)
                                {
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดที่สามารถทำรายการชาจน์ ได้
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนชาจน์ %
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //********************** END **************************
                    //*Arm 63-07-13 Check Discount Policy
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxChgPer : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oFrm.Dispose();
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Chart Amount
        /// </summary>
        public void C_KBDxChgAmt()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm = new wDiscount(3, 1);
            bool Role = false;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {
                cVB.tVB_KbdCallByName = "C_KBDxChgAmt";

                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        Role = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            Role = true;
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (Role)
                {
                    ////*Net 63-07-31 ย้ายมาจาก wSale
                    //wChooseItemRef oChooseItem = new wChooseItemRef(1, "C_KBDxChgAmt");
                    //oChooseItem.ShowDialog();
                    //if (cSale.nC_DTSeqNo == 0)
                    //{
                    //    return;
                    //}
                    ////++++++++++++++++++++++++++++++

                    //*Arm 63-08-18
                    cSP.SP_GETxListItem("C_KBDxChgAmt");
                    if (cSale.nC_DTSeqNo == 0)
                    {
                        return;
                    }
                    //+++++++++++++

                    ////  oFrm.nW_DisType = 4;
                    //oFrm.ShowDialog();

                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ชาจน์หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxChgAmt", 1);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode)) //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice)) //ตรวจสอบสถานะ Price ก่อนชาจน์
                            {
                                //มูลค่าที่สามารถลดได้
                                cVB.oVB_PdtOrder.cSetPrice = oDiscPolicy.C_GETcGetAmtAlwDisc(1, cVB.tVB_StaPrice, cSale.nC_DTSeqNo);

                                if (cVB.oVB_PdtOrder.cSetPrice >= 0)
                                {
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดที่สามารถทำรายการชาจน์ ได้
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนชาจน์
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //********************** END **************************
                    //*Arm 63-07-13 Check Discount Policy
                    
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxChgAmt : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oFrm.Dispose();
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Keyboard function
        /// </summary>
        /// <returns></returns>
        public string C_KBDtFunction(KeyEventArgs poKey)
        {
            int nKeyShift = 0, nKeyFunc;
            string tFuncName = "";

            try
            {
                switch (poKey.KeyValue)
                {
                    case 13:
                    case 27:
                    case int nKeyMenu when (nKeyMenu >= 33 && nKeyMenu <= 36):
                    case 45:
                    case int nKeyF when (nKeyF >= 112 && nKeyF <= 123):
                        nKeyShift = C_CHKnKeyshift(poKey);
                        nKeyFunc = poKey.KeyValue;
                        tFuncName = C_GETtFuncName(nKeyShift, nKeyFunc);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDtFunction : " + oEx.Message); }
            finally
            {
                poKey = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return tFuncName;
        }

        /// <summary>
        /// Check key shift
        /// </summary>
        /// <param name="poKey"></param>
        /// <returns></returns>
        private int C_CHKnKeyshift(KeyEventArgs poKey)
        {
            int nKeyShift = 0;

            try
            {
                if (poKey.Control && poKey.Shift && poKey.Alt)
                    nKeyShift = 7;
                else if (poKey.Control && poKey.Alt)
                    nKeyShift = 6;
                else if (poKey.Shift && poKey.Alt)
                    nKeyShift = 5;
                else if (poKey.Alt)
                    nKeyShift = 4;
                else if (poKey.Shift && poKey.Control)
                    nKeyShift = 3;
                else if (poKey.Control)
                    nKeyShift = 2;
                else if (poKey.Shift)
                    nKeyShift = 1;
                else
                    nKeyShift = 0;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_CHKnKeyshift : " + oEx.Message); }
            finally
            {
                poKey = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return nKeyShift;
        }

        /// <summary>
        /// Check function name
        /// </summary>
        /// <returns></returns>
        public string C_GETtFuncName(int pnKeyShift, int pnKeyFunc)
        {
            StringBuilder oSql;
            string tFuncName = "";

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FDT.FTGdtCallByName");
                oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSMFuncDT_L FDTL WITH(NOLOCK) ON FDTL.FTGhdCode = FDT.FTGhdCode ");
                oSql.AppendLine("   AND FDTL.FTSysCode = FDT.FTSysCode ");
                oSql.AppendLine("   AND FDTL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                oSql.AppendLine("WHERE FDT.FTGdtStaUse = '1'");
                oSql.AppendLine("AND FTGdtSysUse = '1' "); //*Net 63-04-01 ยกมาจาก baseline

                switch (cVB.tVB_PosType)
                {
                    case "1": // Store
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;

                    case "2": // Cashier
                        oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                        break;
                    default:
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;
                }

                oSql.AppendLine("AND (FHD.FTKbdScreen = 'ALL' OR FHD.FTKbdScreen = '" + cVB.tVB_KbdScreen + "')");
                oSql.AppendLine("AND FKB.FNSysKeyAscii = " + pnKeyFunc);
                oSql.AppendLine("AND FKB.FNSysKeyShift = " + pnKeyShift);
                oSql.AppendLine("ORDER BY FDT.FNGdtUsrSeq");


                tFuncName = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_CHKtFuncName : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return tFuncName;
        }

        /// <summary>
        /// Get function code
        /// </summary>
        /// <param name="ptCallByName"></param>
        /// <returns></returns>
        public string C_GETtFuncCode(string ptScreen)
        {
            StringBuilder oSql;
            string tFuncCode = "";

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FDT.FTSysCode");
                oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode ");
                oSql.AppendLine("WHERE FTGdtStaUse = '1'");

                switch (cVB.tVB_PosType)
                {
                    case "1": // Store
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;

                    case "2": // Cashier
                        oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                        break;
                    default:
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;
                }

                oSql.AppendLine("AND (FHD.FTKbdScreen = 'ALL' OR FHD.FTKbdScreen = '" + ptScreen + "')");
                oSql.AppendLine("AND FDT.FTGdtCallByName = '" + cVB.tVB_KbdCallByName + "'");

                tFuncCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_GETtFuncCode : " + oEx.Message); }
            finally
            {
                oSql = null;
                ptScreen = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return tFuncCode;
        }

        /// <summary>
        ///  Get function keyboard
        /// </summary>
        /// <param name="pbForm">True : Form, False : Popup</param>
        /// <returns></returns>
        public List<cmlTPSMFunc> C_GETaFuncKb()
        {
            StringBuilder oSql;
            List<cmlTPSMFunc> aoKb = new List<cmlTPSMFunc>();

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FDT.FTSysCode, FDT.FNGdtPage, FDT.FNGdtBtnSizeX, FDT.FNGdtBtnSizeY, ");
                oSql.AppendLine("       FDT.FTGdtCallByName, FDTL.FTGdtName, FHD.FTKbdScreen, FKB.FTSysKeyFunc");
                oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TPSMFuncDT_L FDTL WITH(NOLOCK) ON FDTL.FTGhdCode = FDT.FTGhdCode ");
                oSql.AppendLine("   AND FDTL.FTSysCode = FDT.FTSysCode ");
                oSql.AppendLine("   AND FDTL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                oSql.AppendLine("WHERE FDT.FTGdtStaUse = '1'");

                switch (cVB.tVB_PosType)
                {
                    case "1": // Store
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;

                    case "2": // Cashier
                        oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                        break;
                    default:
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;
                }

                oSql.AppendLine("AND FHD.FTKbdScreen = '" + cVB.tVB_KbdScreen + "'");
                oSql.AppendLine("ORDER BY FDT.FTSysCode");

                aoKb = new cDatabase().C_GETaDataQuery<cmlTPSMFunc>(oSql.ToString()).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_GETaFuncKb : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoKb;
        }

        /// <summary>
        /// Get menu bar
        /// </summary>
        /// <returns></returns>
        public List<cmlTPSMFunc> C_GETaMenuBar(string ptScreen)
        {
            StringBuilder oSql;
            List<cmlTPSMFunc> aoKb = new List<cmlTPSMFunc>();

            try
            {
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FDTL.FTGdtName, FDT.FTSysCode, FDTL.FNLngID");
                oSql.AppendLine("SELECT FDTL.FTGdtName, FDT.FTSysCode, FDTL.FNLngID, FDT.FNGdtPage, FDT.FNGdtUsrSeq, FDT.FTGdtCallByName");  //*Em 62-01-21  WaterPark
                oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TPSMFuncDT_L FDTL WITH(NOLOCK) ON FDTL.FTGhdCode = FDT.FTGhdCode ");
                oSql.AppendLine("   AND FDTL.FTSysCode = FDT.FTSysCode");
                oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                oSql.AppendLine("WHERE (FDT.FTGdtStaUse = '1' OR FDT.FTGdtSysUse = '3')"); //*Arm 63-08-04
                //oSql.AppendLine("WHERE FDT.FTGdtStaUse = '1' AND FDT.FTGdtStaActive = '1'");     //*Em 62-01-21  WaterPark

                switch (cVB.tVB_PosType)
                {
                    case "":
                    case "1": // Store
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;

                    case "2": // Cashier
                        oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                        break;

                    default:
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;
                }

                oSql.AppendLine("AND FHD.FTKbdScreen = '" + ptScreen + "'");
                oSql.AppendLine("AND FHD.FTKbdGrpName = 'BAR' AND FDT.FNGdtUsrSeq > 0");   //*Em 62-01-24  WaterPark
                if (!string.Equals(ptScreen, "SPLASHSCREEN"))
                    oSql.AppendLine("AND FDTL.FNLngID = " + cVB.nVB_Language);

                aoKb = new cDatabase().C_GETaDataQuery<cmlTPSMFunc>(oSql.ToString()).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_GETaMenuBar : " + oEx.Message); }
            finally
            {
                oSql = null;
                ptScreen = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoKb;
        }

        /// <summary>
        /// Call by name
        /// </summary>
        public void C_PRCxCallByName(string ptFunc)
        {
            MethodInfo oMethod;

            try
            {
              

             if (!string.IsNullOrEmpty(ptFunc))
                {
                    cVB.tVB_KbdCallByName = ptFunc; //*Em 62-10-04
                    oMethod = typeof(cFunctionKeyboard).GetMethod(ptFunc);

                    if (oMethod != null)
                        oMethod.Invoke(new cFunctionKeyboard(), null);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_PRCxCallByName : " + oEx.Message); }
            finally
            {
                ptFunc = null;
                oMethod = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Get Page function home
        /// </summary>
        /// <returns></returns>
        public int C_GETnPageFuncHome()
        {
            StringBuilder oSql;
            int nPage = 0;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT MAX(FDT.FNGdtPage) AS FNGdtPage");
                oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TPSMFuncDT_L FDTL WITH(NOLOCK) ON FDTL.FTGhdCode = FDT.FTGhdCode ");
                oSql.AppendLine("   AND FDTL.FTSysCode = FDT.FTSysCode ");
                oSql.AppendLine("   AND FDTL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                oSql.AppendLine("WHERE FDT.FTGdtStaUse = '1'");

                switch (cVB.tVB_PosType)
                {
                    case "1": // Store
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;

                    case "2": // Cashier
                        oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                        break;
                    default:
                        oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                        break;
                }

                oSql.AppendLine("AND FHD.FTKbdScreen = 'HOME'");
                oSql.AppendLine("AND FHD.FTKbdGrpName = 'FUNC'");

                nPage = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_GETaMainMenuKb : " + oEx.Message); }
            finally
            {
                oSql = null;

                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return nPage;
        }

        /// <summary>
        /// Menu Bill Sale, Rental, Ticket
        /// </summary>
        public List<cmlTPSMFunc> C_GETaFuncList(string ptGhdCode)
        {
            StringBuilder oSql;
            List<cmlTPSMFunc> aoFunc = new List<cmlTPSMFunc>();

            try
            {
                switch (ptGhdCode)
                {
                    case "031":
                    case "032":
                        oSql = new StringBuilder();
                        //*Em 63-04-25
                        switch (cVB.tVB_PosType)
                        {
                            case "1": // Store
                                oSql.AppendLine("DECLARE @ptAppCode varchar(5) = 'PS';");
                                break;

                            case "2": // Cashier
                                oSql.AppendLine("DECLARE @ptAppCode varchar(5) = 'FC';");
                                break;
                        }
                        oSql.AppendLine($"DECLARE @ptBchCode varchar(5) = '{cVB.tVB_BchCode}';");
                        //*Arm 63-08-10 Comment Code
                        //oSql.AppendLine($"WITH RCV AS ");
                        //oSql.AppendLine($"(");
                        //oSql.AppendLine($"SELECT * FROM TFNMRcvSpc WITH(NOLOCK) WHERE FTAppCode=@ptAppCode AND FTBchCode=@ptBchCode");
                        //oSql.AppendLine($"),");
                        //oSql.AppendLine($"RCVA AS");
                        //oSql.AppendLine($"(");
                        //oSql.AppendLine($"SELECT * FROM RCV");
                        //oSql.AppendLine($"UNION");
                        //oSql.AppendLine($"SELECT * FROM TFNMRcvSpc WITH(NOLOCK)");
                        //oSql.AppendLine($"WHERE  FTAppCode=@ptAppCode AND FTRcvCode NOT IN (SELECT RCV.FTRcvCode FROM RCV) AND ISNULL(FTBchCode,'')=''");
                        //oSql.AppendLine($")");
                        //*Arm 63-08-10 Comment Code End
                        //++++++++++++++++++++++++++++++++++++++++++++++++++++
                        oSql.AppendLine("SELECT DISTINCT Count(*) OVER(PARTITION BY 1 ) AS nRowCount,* FROM (");
                        oSql.AppendLine("SELECT DISTINCT FDT.FNGdtUsrSeq,FDT.FTSysCode, FDT.FNGdtPage, FDT.FNGdtBtnSizeX, FDT.FNGdtBtnSizeY, ");
                        oSql.AppendLine("       FDT.FTGdtCallByName, FDTL.FTGdtName, FDT.FTGdtSysUse, FHD.FTKbdScreen, FKB.FTSysKeyFunc");
                        oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TPSMFuncDT_L FDTL  WITH(NOLOCK) ON FDTL.FTGhdCode = FDT.FTGhdCode ");
                        oSql.AppendLine("   AND FDTL.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("   AND FDTL.FNLngID = " + cVB.nVB_Language);
                        oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                        oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("WHERE (FDT.FTGdtStaUse = '1' OR FDT.FTGdtSysUse = '3')"); //*Arm 63-08-04 OR FDT.FTGdtSysUse = '3'

                        switch (cVB.tVB_PosType)
                        {
                            case "1": // Store
                                oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                                break;

                            case "2": // Cashier
                                oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                                break;
                        }

                        //oSql.AppendLine("AND FHD.FTGhdCode = '015'");
                        oSql.AppendLine("AND FHD.FTGhdCode IN ('015','050')"); //*Net 63-05-28 เพิ่มปุ่ม Print Pickinglist
                        oSql.AppendLine("UNION");
                        //+++++++++++++
                        //oSql.AppendLine("SELECT DISTINCT FDT.FNGdtUsrSeq,RCV.FTRcvCode + '-' + RCV.FTFmtCode + '-' + RCVA.FTAppStaAlwRet + '-' + RCVA.FTAppStaAlwCancel  + '-' + RCVA.FTAppStaPayLast AS FTSysCode, FDT.FNGdtPage, FDT.FNGdtBtnSizeX, FDT.FNGdtBtnSizeY, ");
                        oSql.AppendLine("SELECT DISTINCT FDT.FNGdtUsrSeq,RCV.FTRcvCode + '-' + RCV.FTFmtCode + '-' + RCV.FTAppStaAlwRet + '-' + RCV.FTAppStaAlwCancel  + '-' + RCV.FTAppStaPayLast AS FTSysCode, FDT.FNGdtPage, FDT.FNGdtBtnSizeX, FDT.FNGdtBtnSizeY, "); //*Arm 63-07-30
                        oSql.AppendLine("       FDT.FTGdtCallByName, ISNULL(RCVL.FTRcvName,(SELECT TOP 1 FTRcvName FROM TFNMRcv_L WITH(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode)) AS FTGdtName, FDT.FTGdtSysUse, FHD.FTKbdScreen, FKB.FTSysKeyFunc");
                        oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                        oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("INNER JOIN TSysRcvFmt RCVF WITH(NOLOCK) ON FDT.FTGdtCallByName = RCVF.FTFmtRef");
                        oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RCVF.FTFmtCode = RCV.FTFmtCode AND RCV.FTRcvStaUse = '1'");
                        oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);

                        switch (cVB.tVB_PosType)
                        {
                            case "1": // Store
                                //oSql.AppendLine("INNER JOIN TSysRcvApp RCVA WITH(NOLOCK) ON RCVF.FTFmtCode = RCVA.FTFmtCode AND RCVA.FTAppCode = 'PS'");
                                //*Em 63-01-10
                                oSql.AppendLine("LEFT JOIN TFNMRcvSpc RCVA WITH(NOLOCK) ON RCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'PS'"); //*Arm 63-08-10
                                //oSql.AppendLine("INNER JOIN RCVA WITH(NOLOCK) ON RCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'PS'");      //*Arm 63-08-10 Coment Code
                                //oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = '"+ cVB.tVB_BchCode +"')");
                                //oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = @ptBchCode)");                //*Arm 63-08-10 Coment Code
                                //oSql.AppendLine("AND (ISNULL(RCVA.FTMerCode,'') = '' OR ISNULL(RCVA.FTMerCode,'') = '"+ cVB.tVB_Merchart +"')");  //*Arm 63-08-10 Coment Code
                                //oSql.AppendLine("AND (ISNULL(RCVA.FTShpCode,'') = '' OR ISNULL(RCVA.FTShpCode,'') = '"+ cVB.tVB_ShpCode +"')");   //*Arm 63-08-10 Coment Code
                                //++++++++++++++++++
                                oSql.AppendLine("WHERE (FDT.FTGdtStaUse = '1' OR FDT.FTGdtSysUse = '3')"); //*Arm 63-08-04 OR FDT.FTGdtSysUse = '3'
                                oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                                
                                //*Arm 63-08-10
                                oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = @ptBchCode)");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTMerCode,'') = '' OR ISNULL(RCVA.FTMerCode,'') = '" + cVB.tVB_Merchart + "')");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTShpCode,'') = '' OR ISNULL(RCVA.FTShpCode,'') = '" + cVB.tVB_ShpCode + "')");
                                //+++++++++++++

                                if (cSale.nC_DocType == 9)
                                {
                                    //oSql.AppendLine("AND RCVA.FTAppStaAlwRet = '1'"); //*Arm 63-07-30 Comment Code
                                    oSql.AppendLine("AND RCV.FTAppStaAlwRet = '1'");    //*Arm 63-07-30
                                }
                                break;

                            case "2": // Cashier
                                //oSql.AppendLine("INNER JOIN TSysRcvApp RCVA WITH(NOLOCK) ON RCVF.FTFmtCode = RCVA.FTFmtCode AND RCVA.FTAppCode = 'FC'");
                                //*Em 63-01-10
                                //oSql.AppendLine("INNER JOIN TFNMRcvSpc RCVA WITH(NOLOCK) ON RCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'FC'");
                                oSql.AppendLine("INNER JOIN RCVA WITH(NOLOCK) ON RCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'FC'");
                                //oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = '" + cVB.tVB_BchCode + "')");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = @ptBchCode)");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTMerCode,'') = '' OR ISNULL(RCVA.FTMerCode,'') = '" + cVB.tVB_Merchart + "')");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTShpCode,'') = '' OR ISNULL(RCVA.FTShpCode,'') = '" + cVB.tVB_ShpCode + "')");
                                //++++++++++++++++++
                                oSql.AppendLine("WHERE (FDT.FTGdtStaUse = '1' OR FDT.FTGdtSysUse = '3')"); //*Arm 63-08-04 OR FDT.FTGdtSysUse = '3'
                                oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                                if (cSale.nC_DocType == 9)
                                {
                                    //oSql.AppendLine("AND RCVA.FTAppStaAlwRet = '1'"); //*Arm 63-07-30 Comment Code
                                    oSql.AppendLine("AND RCV.FTAppStaAlwRet = '1'");    //*Arm 63-07-30
                                }
                                break;
                        }

                        oSql.AppendLine("AND FHD.FTGhdCode = '" + ptGhdCode + "') TPAY");
                        oSql.AppendLine("ORDER BY TPAY.FNGdtUsrSeq");
                        break;
                    default:
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT  FDT.FTSysCode, FDT.FNGdtPage, FDT.FNGdtBtnSizeX, FDT.FNGdtBtnSizeY, ");
                        oSql.AppendLine("       FDT.FTGdtCallByName, FDTL.FTGdtName, FHD.FTKbdScreen, FKB.FTSysKeyFunc");
                        oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TPSMFuncDT_L FDTL  WITH(NOLOCK) ON FDTL.FTGhdCode = FDT.FTGhdCode ");
                        oSql.AppendLine("   AND FDTL.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("   AND FDTL.FNLngID = " + cVB.nVB_Language);
                        oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                        oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("WHERE (FDT.FTGdtStaUse = '1' OR FDT.FTGdtSysUse = '3') "); //*Arm 63-08-04 OR FDT.FTGdtSysUse = '3'

                        switch (cVB.tVB_PosType)
                        {
                            case "1": // Store
                                oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                                break;

                            case "2": // Cashier
                                oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                                break;
                        }

                        oSql.AppendLine("AND FHD.FTGhdCode = '" + ptGhdCode + "'");
                        oSql.AppendLine("ORDER BY FNGdtUsrSeq");
                        break;
                }
                aoFunc = new cDatabase().C_GETaDataQuery<cmlTPSMFunc>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_GETaMenuBill : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return aoFunc;
        }

        #endregion End Function

        #region Keyboard Function 

        /// <summary>
        /// Help
        /// </summary>
        public void C_KBDxHelp()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxHelp";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wHelp().ShowDialog();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            new wHelp().ShowDialog();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                cVB.tVB_KbdCallByName = "C_KBDxHelp";
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wHelp().ShowDialog();
                }
                else
                    new wHelp().ShowDialog();
                */

                oFormShow = Application.OpenForms[Application.OpenForms.OfType<Form>().Count() - 1];
                cVB.tVB_KbdScreen = oFormShow.Name.Substring(1).ToUpper();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxHelp : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                oFormShow = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Help
        /// </summary>
        public void C_KBDxLanguage()
        {
            List<cmlTSysLanguage> aoLang;
            cmlTSysLanguage oLang;

            try
            {
                aoLang = new cLanguage().C_GETaLanguage();

                if (aoLang.Count <= 2)  // ถ้ามี 1 - 2 ภาษา
                {
                    oLang = (from Lng in aoLang where Lng.FNLngID != cVB.nVB_Language select Lng).LastOrDefault();

                    if (oLang != null)
                        cVB.nVB_Language = Convert.ToInt32(oLang.FNLngID);
                }
                else    // ถ้ามี 3 ภาษาขึ้นไป แสดง Popup
                    new wLanguage().ShowDialog();

                new cCompany().C_GETxCompany();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxLanguage : " + oEx.Message); }
            finally
            {
                aoLang = null;
                oLang = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show function keyboard
        /// </summary>
        /// <returns>Dialog result</returns>
        public DialogResult C_KBDoShowKB()
        {
            DialogResult oResult = DialogResult.None;

            try
            {
                oResult = new wShowKb().ShowDialog();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDoShowKB : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return oResult;
        }

        /// <summary>
        /// Open Calculate
        /// </summary>
        public void C_KBDxCalculator()
        {
            try
            {
                new cSP().SP_OPNxCalculator();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "W_KBDxCalculator : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Open Keyboard
        /// </summary>
        public void C_KBDxKeyboard()
        {
            string[] atPath64;
            string tPath64 = "", tPath32, tPath;

            try
            {
                atPath64 = Directory.GetDirectories(@"C:\Windows\winsxs", "amd64_microsoft-windows-osk_31bf3856ad364e35_*");

                if (atPath64.Length > 0)
                    tPath64 = atPath64[0] + @"\osk.exe";

                tPath32 = @"C:\windows\system32\osk.exe";
                tPath = (Environment.Is64BitOperatingSystem) ? tPath64 : tPath32;

                Process.Start(tPath);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxKeyboard : " + oEx.Message); }
            finally
            {
                atPath64 = null;
                tPath64 = null;
                tPath32 = null;
                tPath = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show popup about
        /// </summary>
        public void C_KBDxAbout()
        {
            try
            {
                new wAbout().ShowDialog();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShowPopup", "C_SHWxPopupAbout : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Close Keyboard
        /// </summary>
        public void C_KBDxClose()
        {
            IEnumerable<Process> oProcesses;

            try
            {
                oProcesses = Process.GetProcesses().Where(pr => pr.ProcessName == "osk");

                foreach (Process oProcess in oProcesses)
                {
                    oProcess.Kill();
                    oProcess.Dispose();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxClose : " + oEx.Message); }
            finally
            {
                oProcesses = null;

                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Close Application
        /// </summary>
        public void C_KBDxExit()
        {
            DialogResult oResult;

            try
            {
                oResult = MessageBox.Show(cVB.oVB_GBResource.GetString("tMsgExit"), "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (oResult == DialogResult.Yes)
                {
                    cSale.C_DATxUsrLog(true);   //*Em 63-06-09
                    cShift.C_SETxDisconnectFixDoc(); //*Net 63-06-05 ตัด Sub MQ FixDoc ออก
                    //new Popup.Shift.wProgressUldCloseShif().ShowDialog(); //*Net 63-06-05 Upload อีกครั้งก่อนปิดเครื่อง
                    Environment.Exit(1);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxExit : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Open shift
        /// </summary>
        public void C_KBDxOpenShift()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            bool bRole = false; // *Arm 63-07-09
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                //*[AnUBiS][][2019-01-09] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed. 
                        //new wOpenShift().ShowDialog(); //*Arm 63-07-09 Comment Code
                        bRole = true; //*Arm 63-07-09
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bRole = true; //*Arm 63-07-09
                            //new wOpenShift().ShowDialog(); //*Arm 63-07-09 Comment Code

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if(bRole == true )
                {
                    if (cVB.bVB_ChkPosRegister == true) // ตรวจสอบ Option อญาตใให้ช้งานตรวจสอบการลงทะเบียนของ Pos หรือไม่
                    {
                        oSql = new StringBuilder();
                        oDB = new cDatabase();
                        string tToken = "";
                        //bool tChkReg = false;

                        oSql.AppendLine("SELECT FTPrgRegToken FROM TCNMPos WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                        tToken = oDB.C_GEToDataQuery<string>(oSql.ToString());

                        if (string.IsNullOrEmpty(tToken))
                        {
                            //ยังไม่ได้ลงทะเบียนเครื่องจุดขาย

                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosRegNotRegistered"), 2);
                        }
                        else
                        {
                            string tThisMac = new cSP().SP_GETtMacAddress(); //Get Mac. Address
                            string tTokenDecrypt = "";
                            tTokenDecrypt = new cEncryptDecrypt("2").C_CALtDecrypt(tToken); //ถอดรหัส Token
                            string[] atoken = tTokenDecrypt.Split(':');

                            if (atoken.Length == 1)
                            {
                                //มีการลงทะเบียนไปแล้วรอการอนุมัติ 
                                if (tThisMac == atoken[0].ToString())
                                {
                                    //กรณีเป็นค่า Mac Addr ของเครื่องตัวเอง  แจ้ง  "ลงทะเบียนแล้วรอการอนุมัติ"
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosRegWaitApv"), 1);
                                }
                                else
                                {
                                    //กรณีค่า Mac Addr เครื่องอื่น(เข้าหน้าจอลงทะเบียนปกติ)
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosRegistered"), 2);
                                }
                                
                                
                            }
                            else
                            {
                                //ลงทะเบียนเครื่องจุดขายแล้ว
                                //string tBchCode = atoken[0].ToString();
                                //string tPosCode = atoken[1].ToString();
                                //string tMac = atoken[2].ToString();
                                //DateTime dExpried = Convert.ToDateTime(atoken[3].ToString());

                                DateTime dExpried = DateTime.Today.AddDays(-1);
                                string tBchCode = "";
                                string tPosCode = "";
                                string tMac = "";
                                try
                                {
                                    tBchCode = atoken[0].ToString();
                                    tPosCode = atoken[1].ToString();
                                    tMac = atoken[2].ToString();
                                    dExpried = Convert.ToDateTime(atoken[3].ToString());
                                }
                                catch (Exception oEx)
                                {
                                    new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxOpenShift :" + oEx.Message);
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosRegTokenFmtInvalid"), 3);
                                }

                                //1.นำค่า BCH ,POS ,MacAddr ไปตรวจสอบกับ Token(Local)
                                if (tBchCode == cVB.tVB_BchCode && tPosCode == cVB.tVB_PosCode && tMac == tThisMac)
                                {
                                    // 1.1  ถ้าตรง 
                                    DateTime dToDay = Convert.ToDateTime(string.Format("{0: yyyy-MM-dd}", DateTime.Now));
                                    if (dToDay > dExpried)
                                    {
                                        //1.1.1  Check Expire   ถ้า Expire แล้วให้แจ้งว่า   "ทะเบียนใช้งานหมดอายุกรุณาติดต่อ ผู้ดูแลระบบ"
                                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosRegExprie"), 3);
                                    }
                                    else
                                    {
                                        //1.1.2 ถ้ายังไม่ Expire อนุญาตปิดรอบได้
                                        new wOpenShift().ShowDialog();
                                    }
                                }
                                else
                                {
                                    //1.2  ถ้าไม่ตรง
                                    //1.2.1  ไม่อนุญาตใช้งาน แจ้ง "เลขทะเบียนไม่ถูกต้อง กรุณาแจ้งผู้ดูแลระบบ" / (BCH + POS ตาม Token)
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosRegNotMatch") + Environment.NewLine+ "(BchCode:" + tBchCode + ", PosCode:"+ tPosCode + ", Mac Address No:"+ tMac +")", 3);
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        new wOpenShift().ShowDialog();
                    }
                }

                //-------
                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wOpenShift().ShowDialog();
                }
                else
                    new wOpenShift().ShowDialog();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxOpenShift : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tFuncCode = null;
                oDB = null; //*Arm 63-07-10
                oSql = null; //*Arm 63-07-10
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show passcode
        /// </summary>
        public void C_KBDxSetting()
        {
            Form oFormShow = null;

            try
            {
                new wPasscode().ShowDialog();

                if (Application.OpenForms.OfType<wSetting>().Count() > 0)
                {
                    if (Application.OpenForms.OfType<wHome>().Count() > 0)
                    {
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        if (oFormShow != null)
                            oFormShow.Close();
                    }
                    else
                    {
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSplashScreen);

                        if (Application.OpenForms.OfType<wSplashScreen>().Count() > 1)
                        {
                            if (oFormShow != null)
                                oFormShow.Close();
                        }
                        else
                        {
                            if (oFormShow != null)
                                oFormShow.Hide();
                        }
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxSetting : " + oEx.Message); }
            finally
            {
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show video
        /// </summary>
        public void C_KBDxVideo()
        {
            Form oFormShow = null;

            try
            {
                new wVideo().Show();

                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSplashScreen);

                if (Application.OpenForms.OfType<wSplashScreen>().Count() > 1)
                {
                    if (oFormShow != null)
                        oFormShow.Close();
                }
                else
                {
                    if (oFormShow != null)
                        oFormShow.Hide();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxVideo : " + oEx.Message); }
            finally
            {
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Register
        /// </summary>
        public void C_KBDxRegister()
        {
            Form oFormShow = null;

            try
            {
                new wRegister().Show();

                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSplashScreen);

                if (Application.OpenForms.OfType<wSplashScreen>().Count() > 1)
                {
                    if (oFormShow != null)
                        oFormShow.Close();
                }
                else
                {
                    if (oFormShow != null)
                        oFormShow.Hide();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRegister : " + oEx.Message); }
            finally
            {
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show sale
        /// </summary>
        public void C_KBDxSale()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wSale(3).Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wSale(3).Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wSale(3).Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wSale(3).Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxSale : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                    oFormShow.Close();

                if (oSignIn != null)
                    oSignIn.Close();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Return sale
        /// </summary>
        public void C_KBDxReturnSale()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wReferBill oReferBill = null;
            wSale oSale = null;
            Form oFormShow = null;
            bool bRole = false;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        //new wReferBill().ShowDialog(); //*Arm 62-12-20 Comment Code
                        bRole = true;                    //*Arm 62-12-20
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            //new wReferBill().ShowDialog(); //*Arm 62-12-20 Comment Code
                            bRole = true;
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                //*Arm 62-12-20
                if (bRole == true)
                {
                    if (cVB.nVB_ReturnType == 5)
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotOpen"), 2);
                    }
                    else
                    {
                        if (cSale.nC_CntItem > 0) //*Net 63-04-01 ยกมาจาก baseline
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantBack"), 3);

                        }
                        else
                        {
                            cVB.oVB_Reason = null;
                            oReferBill = new wReferBill();
                            oReferBill.ShowDialog();
                            ////if (wReferBill.tVB_StatusReBill == "1")
                            ////{
                            //    //new wChooseItemRef(2).ShowDialog();
                            //    //if (cVB.aoVB_PdtRefund.Count == 0) cVB.tVB_RefDocNo = "";

                            //if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                            //{
                            //    new wChooseItemRef(2).ShowDialog();
                            //    if (cVB.aoVB_PdtRefund.Count == 0) cVB.tVB_RefDocNo = "";
                            //    if (cVB.aoVB_PdtRefund.Count > 0)
                            //    {
                            //        wReason oReason;
                            //        oReason = new wReason("003");
                            //        //oReason.TopMost = true;
                            //        oReason.ShowDialog();

                            //        //if (cVB.oVB_Reason != null) cVB.oVB_Sale.ocmPayment_Click(null, null);
                            //    }
                            //}
                            ////}
                            
                            //if (cVB.oVB_Reason != null)
                            //{

                            //    /* cVB.oVB_Sale.Close();
                            //     oSale = new wSale(4);
                            //     oSale.Show();
                            //     oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);*/
                            //    //cVB.oVB_Sale.Close(); //*Net 63-04-01 ยกมาจาก baseline

                            //    //++++++++++++++++++++++++++++++++++++++++++
                            //    //new wSale(4).Show();
                            //    //cVB.oVB_Sale.nW_Mode = 4;
                            //    //cVB.oVB_Sale.Show();
                            //    //cVB.oVB_Sale.Update();
                            //    //cVB.oVB_Sale.Refresh();
                            //    //cVB.oVB_Sale.Activate();
                            //    //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                            //    //cVB.oVB_Sale.Close();
                            //    //oFormShow.Show();
                            //    //++++++++++++++++++++++++++++
                            //}
                        }
                    }
                }
                else
                {
                    return;
                }
                //---------------

                //if (cVB.oVB_Reason != null)
                //{
                //    new wSale(4).Show();
                //    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxReturnSale : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }


                if (oSignIn != null)
                    oSignIn.Close();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Sync data
        /// </summary>
        public void C_KBDxSyncData()
        {
            Form oFormShow = null;

            try
            {
                new wSyncData().Show();

                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                if (oFormShow != null)
                    oFormShow.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxSyncData : " + oEx.Message); }
            finally
            {
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Reprint
        /// </summary>
        public void C_KBDxReprint()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wReprint().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wReprint().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wReprint().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wReprint().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxReprint : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Switch User
        /// </summary>
        public void C_KBDxSwitchUser()
        {
            Form oFormShow = null;
            wSignin oSignin = null;

            try
            {
                oSignin = new wSignin(2, "");
                oSignin.ShowDialog();

                if (oSignin.DialogResult == DialogResult.OK)
                {
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    //*Em 62-01-28  AdaPos 5.0
                    if (oFormShow == null)
                    {
                        new wHome().Show();
                    }
                    //++++++++++++++++++++++++
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxSwitchUser : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignin != null)
                    oSignin.Dispose();

                oFormShow = null;
                oSignin = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Rental
        /// </summary>
        public void C_KBDxRental()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wRentalPdt().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wSale(5).Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wSale(5).Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wSale(5).Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRental : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Return Rental
        /// </summary>
        public void C_KBDxReturnRental()
        {
            try
            {

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxReturnRental : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Spot check
        /// </summary>
        public void C_KBDxSpotCheck()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdScreen = "HOME";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wSpotCheck().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wSpotCheck().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                cVB.tVB_KbdScreen = "HOME";
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wSpotCheck().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wSpotCheck().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxSpotCheck : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Customer
        /// </summary>
        public void C_KBDxCustomerM()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdScreen = "HOME";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wCustomerM().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wCustomerM().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                cVB.tVB_KbdScreen = "HOME";
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wCustomerM().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wCustomerM().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCustomerM : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Back office
        /// </summary>
        public void C_KBDxBackOffice()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdScreen = "HOME";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        Process.Start(cVB.tVB_StoreBack);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            Process.Start(cVB.tVB_StoreBack);

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                cVB.tVB_KbdScreen = "HOME";
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        Process.Start(cVB.tVB_StoreBack);
                }
                else
                    Process.Start(cVB.tVB_StoreBack);
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxBackOffice : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Deposit
        /// </summary>
        public void C_KBDxDeposit()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wDeposit().ShowDialog();

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            new wDeposit().ShowDialog();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wDeposit().ShowDialog();
                }
                else
                    new wDeposit().ShowDialog();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDeposit : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Withdraw
        /// </summary>
        public void C_KBDxWithdraw()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wWithdraw().ShowDialog();

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            new wWithdraw().ShowDialog();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wWithdraw().ShowDialog();
                }
                else
                    new wWithdraw().ShowDialog();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxWithdraw : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Trade
        /// </summary>
        public void C_KBDxTrade()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wTrade().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wTrade().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wTrade().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wTrade().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxTrade : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Change
        /// </summary>
        public void C_KBDxChangeWristband()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wChangeWristband().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wChangeWristband().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wChangeWristband().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wChangeWristband().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxChangeWristband : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Drawer
        /// </summary>
        public void C_KBDxDrawer()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                //cVB.tVB_KbdScreen = "HOME";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wReason("006").ShowDialog();

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wReason("006").ShowDialog();
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                //if (cVB.oVB_Reason != null)
                //{
                //    cSale.C_PRCxOpenDrawer();
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDrawer : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Top-up
        /// </summary>
        public void C_KBDxTopUp()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wTopUp().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wTopUp().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wTopUp().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wTopUp().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxTopUp : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Cancel Top-up
        /// </summary>
        public void C_KBDxCancelTopUp()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wCancelTopUp().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wCancelTopUp().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wCancelTopUp().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wCancelTopUp().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCancelTopUp : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Ticket
        /// </summary>
        public void C_KBDxTicket()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wTicket().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wTicket().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wTicket().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wTicket().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxTicket : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Tax Invoice
        /// </summary>
        public void C_KBDxTaxInvoice()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;
            //wTaxInvoiceMode oTax = null;
            wTax oTax = null; //*Net 63-03-26
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        //oTax = new wTaxInvoiceMode();
                        oTax = new wTax(); //*Net 63-03-26
                        oTax.ShowDialog();

                        if (oTax.DialogResult == DialogResult.OK)
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            //oTax = new wTaxInvoiceMode();
                            oTax = new wTax(); //*Net 63-03-26
                            oTax.ShowDialog();

                            if (oTax.DialogResult == DialogResult.OK)
                                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        oTax = new wTaxInvoiceMode();
                        oTax.ShowDialog();

                        if (oTax.DialogResult == DialogResult.OK)
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    oTax = new wTaxInvoiceMode();
                    oTax.ShowDialog();

                    if (oTax.DialogResult == DialogResult.OK)
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxTaxInvoice : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                if (oTax != null)
                    oTax.Dispose();

                oSignIn = null;
                oTax = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Close shift
        /// </summary>
        public void C_KBDxCloseShift()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;
            bool bRole = false;     //*Arm 63-04-28
            bool bChkHoldBill = false;  //*Arm 63-04-28
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.

                        //*Arm 63-04-28 Comment Code
                        //new wCloseShift().Show();
                        //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        bRole = true;   //*Arm 63-04-28

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            //*Arm 63-04-28 Comment Code
                            //new wCloseShift().Show();
                            //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                            bRole = true;   //*Arm 63-04-28
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                //*Arm 63-04-28 

                if(bRole == true)
                {
                    bChkHoldBill = cSale.C_PRCbCheckHoldBill(); //Check & Clear HoldBill

                    if(bChkHoldBill == true)
                    {
                        new wCloseShift().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                    
                }
                //+++++++++++++


                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wCloseShift().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wCloseShift().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCloseShift : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Return Wristband
        /// </summary>
        public void C_KBDxReturnWristband()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wReturnCard().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wReturnCard().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                //if (string.IsNullOrEmpty(tChkRole))
                //{
                //    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                //    oSignIn.ShowDialog();

                //    if (oSignIn.DialogResult == DialogResult.OK)
                //    {
                //        //new wCloseShift().Show();
                //        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                //    }
                //}
                //else
                //{
                //    //new wCloseShift().Show();
                //    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxReturnWristband : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show popup History
        /// </summary>
        public void C_KBDxHistorySpotChk()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxHistorySpotChk";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wSpotCheckHistory().ShowDialog();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            new wSpotCheckHistory().ShowDialog();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                cVB.tVB_KbdCallByName = "C_KBDxHistorySpotChk";
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wSpotCheckHistory().ShowDialog();
                }
                else
                    new wSpotCheckHistory().ShowDialog();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxHistorySpotChk : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Product master
        /// </summary>
        public void C_KBDxProduct()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wProductM().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wProductM().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wProductM().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wProductM().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxProduct : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Popup Add Unit
        /// </summary>
        public void C_KBDxAddUnit()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wPdtUnit().ShowDialog();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            new wPdtUnit().ShowDialog();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wPdtUnit().ShowDialog();
                }
                else
                    new wPdtUnit().ShowDialog();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxAddUnit : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Popup Add Unit
        /// </summary>
        public void C_KBDxAddBarcode()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wPdtBarcode().ShowDialog();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            new wPdtBarcode().ShowDialog();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wPdtBarcode().ShowDialog();
                }
                else
                    new wPdtBarcode().ShowDialog();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxAddBarcode : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Popup Add Unit
        /// </summary>
        public void C_KBDxAddSupplier()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wPdtSupplier().ShowDialog();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            new wPdtSupplier().ShowDialog();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wPdtSupplier().ShowDialog();
                }
                else
                    new wPdtSupplier().ShowDialog();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxAddSupplier : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Show Coupon Print
        /// </summary>
        public void C_KBDxCouponPrint()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            Form oFormShow = null;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wCouponPrint().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            new wCouponPrint().Show();
                            oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                        }

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                    {
                        new wCouponPrint().Show();
                        oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                    }
                }
                else
                {
                    new wCouponPrint().Show();
                    oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wHome);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCloseShift : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Accept Refer bill
        /// </summary>
        public void C_KBDxAcceptReferBill()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        new wReason("003").ShowDialog();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            new wReason("003").ShowDialog();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        new wReason("003").ShowDialog();
                }
                else
                    new wReason("003").ShowDialog();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxAddSupplier : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Credit Card
        /// </summary>
        public void C_KBDxCreditCard()
        {
            List<cmlTCNMPosHW> aoPosHW;
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            bool bRole = false;
            Form oForm = null;
            try
            {

                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        cVB.tVB_KbdScreen = "PAYMENT";
                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                if (bRole == false) return;

                if (cVB.oVB_Payment.W_CHKbVerify2Payment() == false) return;

                cPayment.tC_CrdNo = "";
                cPayment.tC_BnkCode = "";
                cPayment.tC_BnkName = "";
                cPayment.tC_CrdTrans = "";
                cPayment.tC_CrdApvCode = "";
                if (cSale.nC_DocType == 9) //*Arm 62-11-13 - คืน
                {
                    // ใช้งานแบบ Manual
                    oForm = new wCreditCard();
                    oForm.ShowDialog();
                }
                else
                {
                    cPayment.aoC_EDC = new cPayment().C_GETaEdc();

                    
                    switch (cPayment.aoC_EDC.Count)
                    {
                        case 0:     // ใช้งานแบบ Manual
                            oForm = new wCreditCard();
                            oForm.ShowDialog();
                            break;

                        case 1:     // ใช้งาน EDC
                            cPayment.oC_EDCSel = cPayment.aoC_EDC.First();
                            oForm = new wEDC();
                            oForm.ShowDialog();
                            break;

                        default:    // ใช้งาน EDC หลายตัว ให้เลือก Bank
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(cPayment.tC_CrdNo))
                {
                    cPayment.C_PRCxPaymentCreditCard();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCreditCard : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Void bill.
        /// </summary>
        public void C_KBDxVoidBill()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        cSale.C_PRCxVoidBill();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            cSale.C_PRCxVoidBill();
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                //cVB.tVB_KbdScreen = "SALE"; //*Arm 63-04-08 Comment Code
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxVoidBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Payment by cash.
        /// </summary>
        public void C_KBDxCash()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        cPayment.C_PRCxPaymentCash();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        cVB.tVB_KbdScreen = "PAYMENT";
                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            cPayment.C_PRCxPaymentCash();
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxAddSupplier : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public void C_KBDxConfirm()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        cPayment.C_PRCxPaymentConfirm();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        cVB.tVB_KbdScreen = "PAYMENT";
                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            cPayment.C_PRCxPaymentConfirm();
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxAddSupplier : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Void Item.
        /// </summary>
        public void C_KBDxVoidItem()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wChooseItemRef oChooseItem;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.

                        ////*Net 63-06-23 ย้ายมาจาก wSale
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxVoidItem");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        ////++++++++++++++++++++++++

                        //*Arm 63-08-26
                        cSP.SP_GETxListItem("C_KBDxVoidItem");
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        //+++++++++++++

                        cSale.C_PRCxVoidItem();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            ////*Net 63-06-23 ย้ายมาจาก wSale
                            //oChooseItem = new wChooseItemRef(1, "C_KBDxVoidItem");
                            //oChooseItem.ShowDialog();
                            //if (cSale.nC_DTSeqNo == 0)
                            //{
                            //    return;
                            //}
                            ////++++++++++++++++++++++++

                            //*Arm 63-08-18
                            cSP.SP_GETxListItem("C_KBDxVoidItem");
                            if (cSale.nC_DTSeqNo == 0)
                            {
                                return;
                            }
                            //+++++++++++++

                            cSale.C_PRCxVoidItem();
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                //cVB.tVB_KbdScreen = "SALE";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxVoidItem : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oChooseItem = null;
                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Hold bill.
        /// </summary>
        public void C_KBDxHoldBill()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        cSale.C_PRCxHoldBill();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            cSale.C_PRCxHoldBill();
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                //cVB.tVB_KbdScreen = "SALE";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxHoldBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Retrive bill.
        /// </summary>
        public void C_KBDxRetriveBill()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        cSale.C_PRCxRetriveBill();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            cSale.C_PRCxRetriveBill();
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                cVB.tVB_KbdScreen = "SALESTD";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRetriveBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Bill Remark.
        /// </summary>
        public void C_KBDxRmkBill()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wRemark oRmk = null;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        oRmk = new wRemark(1);   //*Arm 63-09-13 ส่งค่า 1
                        //oRmk.nW_RmkType = 1;    //*Arm 63-09-13 Comment Code
                        oRmk.ShowDialog();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            oRmk = new wRemark(1);  //*Arm 63-09-13 ส่งค่า 1
                            //oRmk.nW_RmkType = 1;  //*Arm 63-09-13 Comment Code
                            oRmk.ShowDialog();
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                //cVB.tVB_KbdScreen = "SALE";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRetriveBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Product Remark.
        /// </summary>
        public void C_KBDxPdtRmk()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wRemark oRmk = null;

            wChooseItemRef oChooseItem;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        ////*Net 63-06-23 ย้ายมาจาก wSale
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxPdtRmk");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        ////++++++++++++++++++++++++++++

                        //*Arm 63-08-18
                        cSP.SP_GETxListItem("C_KBDxPdtRmk");
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        //+++++++++++++

                        oRmk = new wRemark(2); //*Arm 63-09-13 ส่งค่า 2
                        //oRmk.nW_RmkType = 2; //*Arm 63-09-13 Comment Code
                        oRmk.ShowDialog();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            ////*Net 63-06-23 ย้ายมาจาก wSale
                            //oChooseItem = new wChooseItemRef(1, "C_KBDxPdtRmk");
                            //oChooseItem.ShowDialog();
                            //if (cSale.nC_DTSeqNo == 0)
                            //{
                            //    return;
                            //}
                            ////++++++++++++++++++++++++++++

                            //*Arm 63-08-18
                            cSP.SP_GETxListItem("C_KBDxPdtRmk");
                            if (cSale.nC_DTSeqNo == 0)
                            {
                                return;
                            }
                            //+++++++++++++

                            oRmk = new wRemark(2);  //*Arm 63-09-13 ส่งค่า 2
                            //oRmk.nW_RmkType = 2;    //*Arm 63-09-13 Comment Code
                            oRmk.ShowDialog();
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                //cVB.tVB_KbdScreen = "SALE";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRetriveBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oChooseItem = null;
                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Payment Alipay
        /// </summary>
        public void C_KBDxAlipay()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wAlipay oAlipay = null;
            bool bRole = false;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;

                if (cVB.oVB_Payment.W_CHKbVerify2Payment() == false) return;

                cPayment.tC_AliPaymentCode = "";
                cPayment.tC_AliTransID = "";
                cVB.cVB_Amount = cVB.cVB_Amount - cVB.cVB_RoundDiff;    //*Em 62-11-18

                if (cSale.nC_DocType == 9) //*Arm 62-11-14 - คืน
                {
                    oAlipay = new wAlipay();
                    oAlipay.Show();
                    oAlipay.W_PRCxRefund();
                }
                else
                {
                    oAlipay = new wAlipay();
                    oAlipay.ShowDialog();
                }

                if (!string.IsNullOrEmpty(cPayment.tC_AliTransID))
                {
                    cPayment.C_PRCxPaymentAlipay();
                }
                cVB.tVB_KbdScreen = "PAYMENT";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRetriveBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public void C_KBDxCheckPrice()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wPriceCheck oPriChk = null;
            bool bRole = false; //*Arm 63-09-13

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.

                        //oPriChk = new wPriceCheck();  //*Arm 63-09-13 Comment Code
                        //oPriChk.ShowDialog();         //*Arm 63-09-13 Comment Code
                        bRole = true; //*Arm 63-09-13
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen);//*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            //oPriChk = new wPriceCheck();    //*Arm 63-09-13 Comment Code
                            //oPriChk.ShowDialog();           //*Arm 63-09-13 Comment Code
                            bRole = true; //*Arm 63-09-13
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                //cVB.tVB_KbdScreen = "SALE";

                //*Arm 63-09-13
                if(bRole)
                {
                    oPriChk = new wPriceCheck();
                    if(oPriChk.ShowDialog() == DialogResult.OK)
                    {
                        cVB.oVB_Sale.otbScan.Text = oPriChk.rtBarcode;
                        cVB.oVB_Sale.W_PRCxScan();
                    }
                }
                //+++++++++++++

                //*Em 63-04-23
                cVB.oVB_Sale.bW_Activate = false;
                cVB.oVB_Sale.Show();
                //+++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRetriveBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public void C_KBDxCustomer()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            //wCustomerM oCst = null;   //*Arm 63-04-02 Comment Code
            wCstSearch oCst = null;     //*Arm 63-04-02
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.

                        //*Arm 63-04-02 Comment Code
                        //oCst = new wCustomerM();
                        //oCst.ShowDialog();

                        //*Arm 63-04-02
                        oCst = new wCstSearch(cVB.tVB_KbdScreen);
                        oCst.ShowDialog();
                        //+++++++++++++

                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            //*Arm 63-04-02 Comment Code
                            //oCst = new wCustomerM();
                            //oCst.ShowDialog();

                            //*Arm 63-04-02
                            oCst = new wCstSearch(cVB.tVB_KbdScreen);
                            oCst.ShowDialog();
                            //+++++++++++++
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                {
                    cSale.C_DATxInsHDCst(cVB.tVB_CstCode);
                }
                //cVB.tVB_KbdScreen = "SALE";

                //*Em 63-04-23
                cVB.oVB_Sale.bW_Activate = false;
                cVB.oVB_Sale.Show();
                //++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRetriveBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Payment PromptPay
        /// </summary>
        public void C_KBDxPromptPay()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wPromptPay oPromptPay = null;
            bool bRole = false;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;

                if (cVB.oVB_Payment.W_CHKbVerify2Payment() == false) return;

                cPayment.tC_XrcRef1 = "";
                cPayment.tC_XrcRef2 = "";
                cPayment.tC_BnkCode = "";
                cPayment.tC_BnkName = "";
                oPromptPay = new wPromptPay();
                oPromptPay.ShowDialog();

                if (!string.IsNullOrEmpty(cPayment.tC_XrcRef1))
                {
                    cPayment.C_PRCxPaymentPromptPay();
                }
                cVB.tVB_KbdScreen = "PAYMENT";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxPromptPay : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Lock SplashScreen
        /// </summary>
        public void C_KBDxLock()     //*Arm 62-09-26
        {
            try
            {
              
                wSplashScreen owSplashScreen = new wSplashScreen(4);
                owSplashScreen.ShowDialog();

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxLoc : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        ///<summary>
        /// Change Qty
        /// </summary>
        public void C_KBDxPdtQty() //*Arm 62-10-02
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            bool bRole = false;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;

                ////*Net 63-06-23 ย้ายมาจาก wSale
                //wChooseItemRef oChooseItem = new wChooseItemRef(1, "C_KBDxPdtQty");
                //oChooseItem.ShowDialog();

                //if (cSale.nC_DTSeqNo == 0)
                //{
                //    return;
                //}
                ////++++++++++++++++++++++++++

                //*Arm 63-08-18
                cSP.SP_GETxListItem("C_KBDxPdtQty");
                if (cSale.nC_DTSeqNo == 0)
                {
                    return;
                }
                //+++++++++++++

                if (new wChangePdtQty().ShowDialog() == DialogResult.OK)
                {

                    //cVB.oVB_Sale.W_CHAxChangePdtQty(); //*Arm 63-06-20 Comment Code

                    //*Arm 63-06-20 P1RFP-002 ตรวจสอบ Stock แบบ Realtime จากระบบ KADS 
                    if (cSale.nC_DocType != 9 && !string.IsNullOrEmpty(cVB.tVB_WahStaChkStk) && cVB.tVB_WahStaChkStk == "3") //(TCNMWaHouse.FTWahStaChkStk = 3 :ใช้ตรวจสอบในขั้นตอนการขาย 3: Check Online  )
                    {
                        if (new cStock().C_CHKbUpdQtyCheckStock() == false) // ตรวจสอบ Stock Online
                        {
                            //สินค้าคงเหลือไม่พอ
                            return;
                        }
                        else
                        {
                            //สินค้าคงเหลือพอ
                            cVB.oVB_Sale.W_CHAxChangePdtQty();
                        }
                    }
                    else
                    {
                        //ไม่เช็ค Stock
                        cVB.oVB_Sale.W_CHAxChangePdtQty();
                    }
                    //++++++++++++
                }
 
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxPdtQty : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        ///<summary>
        /// สินค้าฟรี
        /// </summary>
        public void C_KBDxItemFree() //*Arm 62-10-07
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            bool bRole = false;
            
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;

                //*Arm 62-10-08 แก้ไข
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resGlobal_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resGlobal_EN));

                        break;
                }

                oW_SP = new cSP();
                ////*Net 63-06-23 ย้ายมาจาก wSale
                //wChooseItemRef oChooseItem = new wChooseItemRef(1, "C_KBDxItemFree");
                //if (oChooseItem == null) //*Net 63-07-06
                //{
                //    return;
                //}
                //oChooseItem.ShowDialog();
                //if (cSale.nC_DTSeqNo == 0)
                //{
                //    return;
                //}
                ////++++++++++++++++++++++++++++++++

                //*Arm 63-08-18
                cSP.SP_GETxListItem("C_KBDxItemFree");
                if (cSale.nC_DTSeqNo == 0)
                {
                    return;
                }
                //+++++++++++++

                DialogResult odgResult = oW_SP.SP_SHWoMsg(cVB.oVB_GBResource.GetString("tPdtFree"), 1);
                if (odgResult == DialogResult.Yes)
                {
                    cVB.oVB_Sale.W_FRExItemFree();
                } //*Arm 62-10-08
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxItemFree : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();
                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ส่วนลดบาทท้ายบิล  KB016
        /// </summary>
        public void C_KBDxDisAmtBill()
        {
            bool bChkAlw;
            string tFuncCode, tChkRole;
            wAuthentication oSignIn; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm = new wDiscount(1, 2);
            StringBuilder oSql = new StringBuilder();
            //decimal cAmount = 0;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-14 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-14 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {
                // *[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxDisAmtBill";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์//*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                bChkAlw = false;
                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bChkAlw = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bChkAlw = true;

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                ////*Em 63-01-08
                ////ไม่อนุญาตให้ลดหลังชาร์จ
                //oSql.Clear();
                //oSql.AppendLine("SELECT ISNULL(SUM(FCXhdAmt),0.00) AS 'Amount'");
                //oSql.AppendLine("FROM " + cSale.tC_TblSalHDDis + " WITH (NOLOCK)");
                //oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "' AND FTXhdDisChgType IN ('3','4')");
                //cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //if (cAmount > 0)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwDisAftChg"), 3);
                //    return;
                //}
                ////+++++++++++++

                //*Em 62-10-10
                if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                    return;
                }
                //+++++++++++++++

                //// คำนวณส่วนลด
                //if (bChkAlw)
                //{
                //    oSql.Clear();
                //    //oSql.AppendLine("SELECT ISNULL(FCXshTotalB4DisChgNV + FCXshTotalB4DisChgV,0.00) AS 'Amount'");
                //    oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");  //*Em 63-01-08
                //    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                //    oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                //    cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //    if (cAmount > 0)
                //    {
                //        oFrm.cW_B4DisChg = cAmount;
                //        oFrm.ShowDialog();
                //    }
                //    else {
                //        // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                //        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                //    }
                //}

                if (bChkAlw)
                {
                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ลดจำนวนท้ายบิล หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxDisAmtBill", 2);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลดท้ายบิล
                            {

                                //oFrm.cW_B4DisChg = oDiscPolicy.C_GETcGetAmount(2); // Get Amount ก่อนลดท้ายบิล
                                oFrm.cW_B4DisChg = oDiscPolicy.C_GETcGetAmtAlwDisc(2, cVB.tVB_StaPrice);
                                if (oFrm.cW_B4DisChg > 0)
                                {
                                    // มียอดสำหรับการทำราการส่วนลดบิล
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดสำหรับการทำราการส่วนลดบิล
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนลดบิล
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //******************** End ****************************
                    //*Arm 63-07-13 Check Discount Policy
                }
            }
            catch (Exception oEx) {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDisAmtBill : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oSignIn = null;
                oFrm = null;
                oSql = null;

                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ส่วนลดเปอร์เซนต์ท้ายบิล
        /// </summary>
        public void C_KBDxDisPerBill()
        {
            bool bChkAlw;
            string tFuncCode, tChkRole;
            wAuthentication oSignIn; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm = new wDiscount(2, 2);
            StringBuilder oSql = new StringBuilder();
            //decimal cAmount = 0;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-14 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-14 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {
                // *[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxDisPerBill";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                bChkAlw = false;
                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bChkAlw = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bChkAlw = true;

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                ////*Em 63-01-08
                ////ไม่อนุญาตให้ลดหลังชาร์จ
                //oSql.Clear();
                //oSql.AppendLine("SELECT ISNULL(SUM(FCXhdAmt),0.00) AS 'Amount'");
                //oSql.AppendLine("FROM " + cSale.tC_TblSalHDDis + " WITH (NOLOCK)");
                //oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "' AND FTXhdDisChgType IN ('3','4')");
                //cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //if (cAmount > 0)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwDisAftChg"), 3);
                //    return;
                //}
                ////+++++++++++++

                //*Em 62-10-10
                if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                    return;
                }
                //+++++++++++++++

                //// คำนวณส่วนลด
                //if (bChkAlw)
                //{
                //    oSql.Clear();
                //    //oSql.AppendLine("SELECT ISNULL(FCXshTotalB4DisChgNV + FCXshTotalB4DisChgV,0.00) AS 'Amount'");
                //    oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");  //*Em 63-01-08
                //    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                //    oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                //    cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //    if (cAmount > 0)
                //    {
                //        oFrm.cW_B4DisChg = cAmount;
                //        oFrm.ShowDialog();
                //    }
                //    else
                //    {
                //        // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                //        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                //    }
                //}

                if (bChkAlw)
                {
                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ลดจำนวนท้ายบิล หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxDisPerBill", 2);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลดท้ายบิล
                            {

                                oFrm.cW_B4DisChg = oDiscPolicy.C_GETcGetAmtAlwDisc(2, cVB.tVB_StaPrice); // Get Amount ก่อนลดท้ายบิล
                                if (oFrm.cW_B4DisChg > 0)
                                {
                                    // มียอดสำหรับการทำราการส่วนลดบิล
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดสำหรับการทำราการส่วนลดบิล
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนลดบิล
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //******************** End ****************************
                    //*Arm 63-07-13 Check Discount Policy
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDisPerBill : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oSignIn = null;
                oFrm = null;
                oSql = null;

                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ชาร์ทบาทท้ายบิล
        /// </summary>
        public void C_KBDxChgAmtBill()
        {
            bool bChkAlw;
            string tFuncCode, tChkRole;
            wAuthentication oSignIn; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm = new wDiscount(3, 2);
            StringBuilder oSql = new StringBuilder();
            //decimal cAmount = 0;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {
                // *[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxDisPerBill";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                bChkAlw = false;
                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bChkAlw = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bChkAlw = true;

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                //*Em 62-10-10 //*Net 63-04-01 Comment from baseline
                //if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                //    return;
                //}
                //+++++++++++++++

                //// คำนวณส่วนลด
                //if (bChkAlw)
                //{
                //    oSql.Clear();
                //    //oSql.AppendLine("SELECT ISNULL(FCXshTotalB4DisChgNV + FCXshTotalB4DisChgV,0.00) AS 'Amount'");
                //    oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");  //*Em 63-01-08
                //    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                //    oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                //    cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //    if (cAmount >= 0) //*Net 63-04-01 >=0 จาก baseline
                //    {
                //        oFrm.cW_B4DisChg = cAmount;
                //        oFrm.ShowDialog();
                //    }
                //    else
                //    {
                //        // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                //        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                //    }
                //}

                if(bChkAlw)
                {
                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ชาจน์ หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxChgAmtBill", 2);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode)) //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice)) //ตรวจสอบสถานะ Price ก่อนชาจน์
                            {
                                oFrm.cW_B4DisChg = oDiscPolicy.C_GETcGetAmtAlwDisc(2, cVB.tVB_StaPrice); // Get มูลค่าที่สามารถลดได้

                                if (oFrm.cW_B4DisChg >= 0)
                                {
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดที่สามารถทำรายการชาจน์ ได้
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนชาจน์
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //********************** END **************************
                    //*Arm 63-07-13 Check Discount Policy
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDisPerBill : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oSignIn = null;
                oFrm = null;
                oSql = null;

                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ชาร์ทเปอร์เซนต์ท้ายบิล
        /// </summary>
        public void C_KBDxChgPerBill()
        {
            bool bChkAlw;
            string tFuncCode, tChkRole;
            wAuthentication oSignIn; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm = new wDiscount(4, 2);
            StringBuilder oSql = new StringBuilder();
            //decimal cAmount = 0;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต
            try
            {
                // *[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxDisPerBill";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                bChkAlw = false;
                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bChkAlw = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bChkAlw = true;

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                //*Em 62-10-10 //*Net 63-04-01 Comment from baseline
                //if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                //    return;
                //}
                //+++++++++++++++

                //// คำนวณส่วนลด
                //if (bChkAlw)
                //{
                //    oSql.Clear();
                //    //oSql.AppendLine("SELECT ISNULL(FCXshTotalB4DisChgNV + FCXshTotalB4DisChgV,0.00) AS 'Amount'");
                //    oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");  //*Em 63-01-08
                //    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                //    oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                //    cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //    if (cAmount >= 0) //*Net 63-04-01 >=0 จาก baseline
                //    {
                //        oFrm.cW_B4DisChg = cAmount;
                //        oFrm.ShowDialog();
                //    }
                //    else
                //    {
                //        // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                //        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                //    }
                //}

                if (bChkAlw)
                {
                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ชาจน์ % หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxChgPerBill", 2);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode)) //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice)) //ตรวจสอบสถานะ Price ก่อนชาจน์ %
                            {
                                oFrm.cW_B4DisChg = oDiscPolicy.C_GETcGetAmtAlwDisc(2, cVB.tVB_StaPrice); // Get มูลค่าที่สามารถลดได้
                                if (oFrm.cW_B4DisChg >= 0)
                                {
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดที่สามารถทำรายการชาจน์ % ได้
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนชาจน์ %
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //********************** END **************************
                    //*Arm 63-07-13 Check Discount Policy
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDisPerBill : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oSignIn = null;
                oFrm = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public void C_KBDxCashCoupon()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            //wCoupon oCoupon = null;
            wCouponDis oCoupon = null; //*Net 63-03-12
            bool bRole = false;
            string tMsg = "";
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;

                //if (Convert.ToDecimal(cVB.cVB_Amount) > Convert.ToDecimal(cVB.oVB_Payment.cW_AmtTotalCal))
                //{
                //    tMsg = cVB.oVB_GBResource.GetString("tMsgPayMost");
                //    tMsg += Environment.NewLine + cVB.oVB_GBResource.GetString("tMsgPayConfirm");
                //    if (new cSP().SP_SHWoMsg(tMsg,1) == DialogResult.No)
                //    {
                //        return;
                //    }
                //}

                cPayment.tC_XrcRef1 = "";
                cPayment.tC_XrcRef2 = "";
                //oCoupon = new wCoupon(1); //*Net 63-03-13
                oCoupon = new wCouponDis(1);
                oCoupon.ShowDialog();

                if (!string.IsNullOrEmpty(cPayment.tC_XrcRef1))
                {
                    cPayment.C_PRCxPaymentCoupon(1);
                }
                cVB.tVB_KbdScreen = "PAYMENT";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCashCoupon : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        ///<summary>
        /// ชำระเงินด้วยเช็ค
        /// </summary>
        public void C_KBDxCheque() //*Arm 62-10-11
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            bool bRole = false;
            string tMsg = "";
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;

                if (cVB.oVB_Payment.W_CHKbVerify2Payment() == false) return;
                if (Convert.ToDecimal(cVB.cVB_Amount) > Convert.ToDecimal(cVB.oVB_Payment.cW_AmtTotalCal))
                {
                    tMsg = cVB.oVB_GBResource.GetString("tMsgPayMost");
                    tMsg += Environment.NewLine + cVB.oVB_GBResource.GetString("tMsgPayConfirm");
                    if (new cSP().SP_SHWoMsg(tMsg, 1) == DialogResult.No)
                    {
                        return;
                    }
                }
                cPayment.tC_ChequeNo = "";
                cPayment.dC_ChequeDate = DateTime.Now.Date;
                cPayment.tC_BnkCode = "";

                wCheque owCheque = new wCheque();
                owCheque.ShowDialog();

                if (!string.IsNullOrEmpty(cPayment.tC_ChequeNo))
                {
                    cPayment.C_PRCxPaymentCheque();
                }
                cVB.tVB_KbdScreen = "PAYMENT";

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCheque : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();
                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public void C_KBDxDiscountCoupon()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            //wCoupon oCoupon = null; //*Net 63-03-13
            wCouponDis oCoupon = null; //*Net 63-03-13
            bool bRole = false;
            StringBuilder oSql = new StringBuilder();
            decimal cAmount = 0;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต
            
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;

                //*Arm 63-07-13 Check Discount Policy
                //******************** Start ****************************

                oDiscPolicy = new cDiscPolicy();
                //ตรวจสอบอนุญาตให้ลดจำนวนท้ายบิล หรือไม่
                bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxDiscountCoupon", 2);

                if (bStaDis)
                {
                    // อนุญาตให้ทำรายการ
                    if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                    {
                        if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลดท้ายบิล
                        {
                            if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                            {
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                                return;
                            }

                            //มูลค่าที่สามารถลดได้
                            cAmount = oDiscPolicy.C_GETcGetAmtAlwDisc(2, cVB.tVB_StaPrice);

                            if (cAmount > 0)
                            {
                                cPayment.tC_XrcRef1 = "";
                                cPayment.tC_XrcRef2 = "";
                                oCoupon = new wCouponDis(2);
                                oCoupon.cW_AlwDisChg = cAmount;
                                oCoupon.ShowDialog();
                                if (cVB.cVB_Amount == 0) return;

                                if (!string.IsNullOrEmpty(cPayment.tC_XrcRef1))
                                {
                                    if (cVB.tVB_DisChgTxt.IndexOf("%") > 0)
                                    {
                                        //*Net 63-03-16 ส่ง BarCpn ไปบันทึก RefCode ด้วย
                                        cPayment.C_ADDxDisChgBill(cVB.cVB_Amount, cVB.cVB_Amount, cVB.tVB_DisChgTxt, cAmount, 5, cPayment.tC_XrcRef1, oCoupon.aoW_ProrateDT);
                                    }
                                    else
                                    {
                                        //*Net 63-03-16 ส่ง BarCpn ไปบันทึก RefCode ด้วย
                                        cPayment.C_ADDxDisChgBill(cVB.cVB_Amount, cVB.cVB_Amount, cVB.tVB_DisChgTxt, cAmount, 5, cPayment.tC_XrcRef1, oCoupon.aoW_ProrateDT);
                                    }
                                }
                            }
                            else
                            {
                                // ไม่มียอดที่สามารถทำรายการลด ได้
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                        else
                        {
                            // ไม่ได้กำหนดสถานะ Price ก่อนลดบิล
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                        }
                    }
                }
                else
                {
                    // ไม่อนุญาตให้ทำรายการ
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                }

                //******************** End ****************************
                //*Arm 63-07-13 Check Discount Policy


                ////ไม่อนุญาตให้ลดหลังชาร์จ
                //oSql.Clear();
                //oSql.AppendLine("SELECT ISNULL(SUM(FCXhdAmt),0.00) AS 'Amount'");
                //oSql.AppendLine("FROM " + cSale.tC_TblSalHDDis + " WITH (NOLOCK)");
                //oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "' AND FTXhdDisChgType IN ('3','4')");
                //cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //if (cAmount > 0)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwDisAftChg"), 3);
                //    return;
                //}

                //if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                //    return;
                //}

                //oSql.Clear();
                //oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                //oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                //oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                //cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //if (cAmount > 0)
                //{
                //    cPayment.tC_XrcRef1 = "";
                //    cPayment.tC_XrcRef2 = "";
                //    //oCoupon = new wCoupon(2); //*Net 63-03-13
                //    oCoupon = new wCouponDis(2); //*Net 63-03-13
                //    oCoupon.cW_AlwDisChg = cAmount;
                //    oCoupon.ShowDialog();
                //    if (cVB.cVB_Amount == 0) return;
                //    if (!string.IsNullOrEmpty(cPayment.tC_XrcRef1))
                //    {
                //        if (cVB.tVB_DisChgTxt.IndexOf("%") > 0)
                //        {
                //            //*Net 63-03-16 ส่ง BarCpn ไปบันทึก RefCode ด้วย
                //            cPayment.C_ADDxDisChgBill(cVB.cVB_Amount, cVB.cVB_Amount, cVB.tVB_DisChgTxt,cAmount, 5, cPayment.tC_XrcRef1, oCoupon.aoW_ProrateDT);
                //        }
                //        else
                //        {
                //            //*Net 63-03-16 ส่ง BarCpn ไปบันทึก RefCode ด้วย
                //            cPayment.C_ADDxDisChgBill(cVB.cVB_Amount, cVB.cVB_Amount, cVB.tVB_DisChgTxt, cAmount, 5, cPayment.tC_XrcRef1, oCoupon.aoW_ProrateDT);
                //        }
                //        //cSale.C_PRCxSummary2HD();
                //        //cPayment.C_PRCxPaymentCoupon(2); //*Net 63-03-17
                //    }
                //}
                //else
                //{
                //    // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                //}
                
                cVB.tVB_KbdScreen = "PAYMENT";

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCashCoupon : " + oEx.Message); }
            finally
            {
                //*Arm 63-07-17 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// คูปองแลกซื้อ //*Net 63-03-16
        /// </summary>
        public void C_KBDxRedeemCoupon()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            //wCoupon oCoupon = null; //*Net 63-03-13
            wCouponRedeem oCoupon = null; //*Net 63-03-13
            bool bRole = false;
            StringBuilder oSql = new StringBuilder();
            decimal cAmount = 0;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;

                //ไม่อนุญาตให้ลดหลังชาร์จ
                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(SUM(FCXhdAmt),0.00) AS 'Amount'");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHDDis + " WITH (NOLOCK)");
                oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "' AND FTXhdDisChgType IN ('3','4')");
                cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                if (cAmount > 0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwDisAftChg"), 3);
                    return;
                }

                if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                    return;
                }

                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                if (cAmount > 0)
                {
                    cPayment.tC_XrcRef1 = "";
                    cPayment.tC_XrcRef2 = "";
                    //oCoupon = new wCoupon(2); //*Net 63-03-13
                    oCoupon = new wCouponRedeem(); //*Net 63-03-13
                    //oCoupon.cW_B4DisChg = cAmount;
                    oCoupon.ShowDialog();
                    if (cVB.cVB_Amount == 0) return;
                    if (!string.IsNullOrEmpty(cPayment.tC_XrcRef1))
                    {
                        //*Net 63-03-16 ส่ง BarCpn ไปบันทึก RefCode ด้วย
                        cPayment.C_ADDxDisChgBill(cVB.cVB_Amount, cVB.cVB_Amount, cVB.tVB_DisChgTxt, cAmount, 6, cPayment.tC_XrcRef1, oCoupon.aoW_ProrateDT);
                        //cSale.C_PRCxSummary2HD();
                        //cPayment.C_PRCxPaymentCoupon(2);
                    }
                }
                else
                {
                    // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                }


                cVB.tVB_KbdScreen = "PAYMENT";

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxCashCoupon : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        ///<summary>
        /// ReferSO
        /// </summary>
        public void C_KBDxReferSO() //*Arm 63-02-19
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            bool bRole = false;

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == false) return;
                if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                {
                    new cLog().C_WRTxLog("wSale", "Process So Strat... ", cVB.bVB_AlwPrnLog);

                    new wReferSO(cVB.tVB_CstCode).ShowDialog();
                    if (cVB.oVB_ReferSO != null)
                    {
                        cVB.oVB_Sale.W_DATxLoadSO2Order();
                    }
                    new cLog().C_WRTxLog("wSale", "Process So End... ", cVB.bVB_AlwPrnLog);
                    //if (cVB.aoVB_PdtReferSO != null)
                    //{
                    //    cVB.oVB_Sale.W_DATxLoadSO2Order();
                    //}
                }
                //else
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotChooseCst"), 1);
                //    C_PRCxCallByName("C_KBDxCustomer");

                //}
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxReferSO : " + oEx.Message);
            }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        ///<summary>
        ///*Arm 63-03-09
        /// Redeem Get Product
        /// </summary>
        public void C_KBDxRedeemProduct() //*Arm 63-02-19
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wRedeem oFrmRedeem;
            bool bRole = false;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                //if (bRole == false) return;

                //if (!string.IsNullOrEmpty(cVB.tVB_CstName))
                //{
                //    //oFrmRedeem = new wRedeem(1);
                //    ////oFrmRedeem.ShowDialog();
                //    //if (oFrmRedeem.IsDisposed == false) oFrmRedeem.ShowDialog();    //*Em 63-06-04

                //    if (cVB.bVB_Flag == true) //*Arm 63-06-11 - Check Flag
                //    {
                //        oFrmRedeem = new wRedeem(1);
                //        //oFrmRedeem.ShowDialog();
                //        if (oFrmRedeem.IsDisposed == false) oFrmRedeem.ShowDialog();    //*Em 63-06-04
                //    }
                //    else
                //    {
                //        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwRedeem"), 1); //*Arm 63-06-11
                //    }

                //}
                //else
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotChooseCst"), 1);
                //}


                //*Arm 63-07-14
                if (bRole)
                {
                    if (!string.IsNullOrEmpty(cVB.tVB_CstName)) // ตรวจสอบลูกค้า
                    {
                        //ต้องมีการเลือกลูกค้า

                        if (cVB.bVB_Flag == true)   // ตรวจสอบ Flag อนุญาตให้ลูกค้าคนนี้ใช้แต้มแลกส่วนลด ได้หรือไม่ true:อนุญาต, 2:ไม่อนุญาต
                        {
                            //อนุญาตให้ลูกค้าคนนี้ใช้แต้มแลกส่วนลดได้

                            //*Arm 63-07-13 Check Discount Policy
                            //******************** Start ****************************

                            oDiscPolicy = new cDiscPolicy();
                            //ตรวจสอบอนุญาตให้ลดจำนวนท้ายบิล หรือไม่
                            bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxRedeemProduct", 2);

                            if (bStaDis)
                            {
                                // อนุญาตให้ทำรายการ
                                if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                                {
                                    if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลดท้ายบิล
                                    {
                                        oFrmRedeem = new wRedeem(1);
                                        if (oFrmRedeem.IsDisposed == false) oFrmRedeem.ShowDialog();
                                    }
                                    else
                                    {
                                        // ไม่ได้กำหนดสถานะ Price ก่อนลดบิล
                                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                                    }
                                }
                            }
                            else
                            {
                                // ไม่อนุญาตให้ทำรายการ
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                            }

                            //******************** End ****************************
                            //*Arm 63-07-13 Check Discount Policy
                        }
                        else
                        {
                            //ไม่อนุญาตให้ลูกค้าคนนี้ใช้แต้มแลกส่วนลด
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwRedeem"), 3);
                        }
                    }
                    else
                    {
                        //ไม่ได้เลือกลูกค้า
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotChooseCst"), 3);
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRedeemProduct : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        ///<summary>
        ///*Arm 63-03-13
        /// Redeem Get Discount
        /// </summary>
        public void C_KBDxRedeemDiscount() //*Arm 63-03-13
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wRedeem oFrmRedeem;
            bool bRole = false;

            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            bRole = true;
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                //if (bRole == false) return;

                //if (!string.IsNullOrEmpty(cVB.tVB_CstName))
                //{
                //    //oFrmRedeem = new wRedeem(2);
                //    ////oFrmRedeem.ShowDialog();
                //    //if (oFrmRedeem.IsDisposed == false) oFrmRedeem.ShowDialog();    //*Em 63-06-04

                //    if (cVB.bVB_Flag == true) //*Arm 63-06-11 - Check Flag
                //    {
                //        oFrmRedeem = new wRedeem(2);
                //        //oFrmRedeem.ShowDialog();
                //        if (oFrmRedeem.IsDisposed == false) oFrmRedeem.ShowDialog();    //*Em 63-06-04
                //    }
                //    else
                //    {
                //        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwRedeem"), 1); //*Arm 63-06-11
                //    }
                //}
                //else
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotChooseCst"), 1);
                //}


                //*Arm 63-07-14
                if (bRole)
                {
                    if (!string.IsNullOrEmpty(cVB.tVB_CstName)) // ตรวจสอบลูกค้า
                    {
                        //ต้องมีการเลือกลูกค้า

                        if (cVB.bVB_Flag == true)   // ตรวจสอบ Flag อนุญาตให้ลูกค้าคนนี้ใช้แต้มแลกส่วนลด ได้หรือไม่ true:อนุญาต, 2:ไม่อนุญาต
                        {
                            //อนุญาตให้ลูกค้าคนนี้ใช้แต้มแลกส่วนลดได้

                            //*Arm 63-07-13 Check Discount Policy
                            //******************** Start ****************************

                            oDiscPolicy = new cDiscPolicy();
                            //ตรวจสอบอนุญาตให้ลดจำนวนท้ายบิล หรือไม่
                            bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxRedeemDiscount", 2);

                            if (bStaDis)
                            {
                                // อนุญาตให้ทำรายการ
                                if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                                {
                                    if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลดท้ายบิล
                                    {
                                        oFrmRedeem = new wRedeem(2);
                                        if (oFrmRedeem.IsDisposed == false) oFrmRedeem.ShowDialog();
                                    }
                                    else
                                    {
                                        // ไม่ได้กำหนดสถานะ Price ก่อนลดบิล
                                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                                    }
                                }
                            }
                            else
                            {
                                // ไม่อนุญาตให้ทำรายการ
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                            }

                            //******************** End ****************************
                            //*Arm 63-07-13 Check Discount Policy
                        }
                        else
                        {
                            //ไม่อนุญาตให้ลูกค้าคนนี้ใช้แต้มแลกส่วนลด
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwRedeem"), 3);
                        }
                    }
                    else
                    {
                        //ไม่ได้เลือกลูกค้า
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotChooseCst"), 3);
                    } 
                }
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRedeemDiscount : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ขายอ้างอิงบิลคืน (*Arm 63-06-10)
        /// </summary>
        public void C_KBDxReferRefund()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wReferBill oReferBill = null;
            wSale oSale = null;
            Form oFormShow = null;
            bool bRole = false;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen);
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bRole = true;
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == true)
                {
                    if (cSale.nC_CntItem > 0)
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantBack"), 3);

                    }
                    else
                    {
                        cVB.oVB_Reason = null;
                        oReferBill = new wReferBill(1);
                        oReferBill.ShowDialog();
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxReferRefund : " + oEx.Message); }
            finally
            {
                if (oFormShow != null)
                {
                    oFormShow.Close();
                    oFormShow.Dispose();
                }


                if (oSignIn != null)
                    oSignIn.Close();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ส่วนลดบาทท้ายบิล (ลดรายการ) (*Arm 63-06-10)
        /// </summary>
        public void C_KBDxDisAmtBillByDT()
        {
            bool bChkAlw;
            string tFuncCode, tChkRole;
            wAuthentication oSignIn; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm;
            StringBuilder oSql = new StringBuilder();
            //decimal cAmount = 0;
            //ResourceManager oResource;
            //wChooseItemRef oChooseItem = new wChooseItemRef(1, "C_KBDxDisAmtBillByDT");
            List<cmlProrateDT> aoProDT;
            cmlProrateDT oProDT;
            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {
                // *[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxDisAmtBillByDT";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                bChkAlw = false;
                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bChkAlw = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bChkAlw = true;

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                ////ไม่อนุญาตให้ลดหลังชาร์จ
                //oSql.Clear();
                //oSql.AppendLine("SELECT ISNULL(SUM(FCXhdAmt),0.00) AS 'Amount'");
                //oSql.AppendLine("FROM " + cSale.tC_TblSalHDDis + " WITH (NOLOCK)");
                //oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "' AND FTXhdDisChgType IN ('3','4')");
                //cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());
                //if (cAmount > 0)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwDisAftChg"), 3);
                //    return;
                //}

                //ไม่อนุญาตให้ลดหลังมีการชำระเงินแล้ว
                if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                    return;
                }

                //if (bChkAlw)
                //{
                //    oChooseItem.ShowDialog(); //หน้าต่างเลือกรายการ
                //    if (cSale.nC_DTSeqNo == 0)
                //    {
                //        return;
                //    }

                //    if (cSale.C_PRCxCheckChg()) //ตรวจสอบ ชาจน์รายการถ้ามี Chg ไม่อนุญาตให้ลด
                //    {
                //        aoProDT = new List<cmlProrateDT>();
                //        cmlProrateDT oProDT = new cmlProrateDT();
                //        oProDT.FNSeqNo = (int)cSale.nC_DTSeqNo;
                //        oProDT.FTBchCode = cVB.tVB_BchCode; ;
                //        oProDT.FTSalDocNo = cVB.tVB_DocNo;
                //        aoProDT.Add(oProDT);

                //        oFrm = new wDiscount(1, 3, aoProDT);
                //        oFrm.ShowDialog();
                //    }
                //    else
                //    {
                //        switch (cVB.nVB_Language)
                //        {
                //            case 1:     // TH
                //                oResource = new ResourceManager(typeof(resGlobal_TH));
                //                break;

                //            default:    // EN
                //                oResource = new ResourceManager(typeof(resGlobal_EN));
                //                break;
                //        }
                //        MessageBox.Show(oResource.GetString("tMsgNotAlwDisAftChg"));
                //    }

                //}

                if (bChkAlw)
                {
                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();

                    //ตรวจสอบอนุญาตให้ลดจำนวน หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxDisAmtBillByDT", 2);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลด
                            {
                                //oChooseItem.ShowDialog(); //หน้าต่างเลือกรายการ
                                //if (cSale.nC_DTSeqNo == 0)
                                //{
                                //    return;
                                //}
                                //*Arm 63-08-18
                                cSP.SP_GETxListItem("C_KBDxDisAmtBillByDT");
                                if (cSale.nC_DTSeqNo == 0)
                                {
                                    return;
                                }
                                //+++++++++++++
                                cVB.oVB_PdtOrder.cSetPrice = oDiscPolicy.C_GETcGetAmtAlwDisc(3, cVB.tVB_StaPrice, cSale.nC_DTSeqNo); // Get Amount ที่สามารถลดได้

                                if (cVB.oVB_PdtOrder.cSetPrice > 0)
                                {
                                    // มียอดสำหรับการทำราการส่วนลด
                                    aoProDT = new List<cmlProrateDT>();
                                    oProDT = new cmlProrateDT();
                                    oProDT.FNSeqNo = (int)cSale.nC_DTSeqNo;
                                    oProDT.FTBchCode = cVB.tVB_BchCode; ;
                                    oProDT.FTSalDocNo = cVB.tVB_DocNo;
                                    aoProDT.Add(oProDT);
                                    oFrm = new wDiscount(1, 3, aoProDT);
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดสำหรับการทำราการส่วนลด
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนลด
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }

                    //******************** End ****************************
                    //*Arm 63-07-13 Check Discount Policy
                }

                cVB.tVB_KbdScreen = "PAYMENT";
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDisAmtBillByDT : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oSignIn = null;
                oFrm = null;
                oSql = null;
                aoProDT = null;
                oProDT = null;

                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ส่วนลด % ท้ายบิล (ลดรายการ) (*Arm 63-06-10)
        /// </summary>
        public void C_KBDxDisPerBillByDT()
        {
            bool bChkAlw;
            string tFuncCode, tChkRole;
            wAuthentication oSignIn; //ยืนยันสิทธิ์ไปใช้ wAuthen
            wDiscount oFrm;
            StringBuilder oSql = new StringBuilder();
            //decimal cAmount = 0;
            //ResourceManager oResource;
            //wChooseItemRef oChooseItem = new wChooseItemRef(1, "C_KBDxDisPerBillByDT");
            List<cmlProrateDT> aoProDT;
            cmlProrateDT oProDT;
            cDiscPolicy oDiscPolicy;    //*Arm 63-07-13 Class Discount Policy
            bool bStaDis = false;       //*Arm 63-07-13 สถานะตรวจสอบลำดับการให้ทำรายการ ได้หรือไม่ true:อนุญาตให้ทำได้ false:ไม่อนุญาต

            try
            {
                // *[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxDisPerBillByDT";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                bChkAlw = false;
                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bChkAlw = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bChkAlw = true;

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                ////ไม่อนุญาตให้ลดหลังชาร์จ
                //oSql.Clear();
                //oSql.AppendLine("SELECT ISNULL(SUM(FCXhdAmt),0.00) AS 'Amount'");
                //oSql.AppendLine("FROM " + cSale.tC_TblSalHDDis + " WITH (NOLOCK)");
                //oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "' AND FTXhdDisChgType IN ('3','4')");
                //cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());
                //if (cAmount > 0)
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotAlwDisAftChg"), 3);
                //    return;
                //}

                //ไม่อนุญาตให้ลดหลังมีการชำระเงินแล้ว
                if ((decimal)cVB.oVB_Payment.cW_AmtTotalPay > (decimal)0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgFirstPay"), 3);
                    return;
                }

                //if (bChkAlw)
                //{
                //    oChooseItem.ShowDialog(); //หน้าต่างเลือกรายการ
                //    if (cSale.nC_DTSeqNo == 0)
                //    {
                //        return;
                //    }

                //    if (cSale.C_PRCxCheckChg()) //ตรวจสอบ ชาจน์รายการถ้ามี Chg ไม่อนุญาตให้ลด
                //    {
                //        aoProDT = new List<cmlProrateDT>();
                //        cmlProrateDT oProDT = new cmlProrateDT();
                //        oProDT.FNSeqNo = (int)cSale.nC_DTSeqNo;
                //        oProDT.FTBchCode = cVB.tVB_BchCode; ;
                //        oProDT.FTSalDocNo = cVB.tVB_DocNo;
                //        aoProDT.Add(oProDT);

                //        oFrm = new wDiscount(2, 3, aoProDT);
                //        oFrm.ShowDialog();
                //    }
                //    else
                //    {
                //        switch (cVB.nVB_Language)
                //        {
                //            case 1:     // TH
                //                oResource = new ResourceManager(typeof(resGlobal_TH));
                //                break;

                //            default:    // EN
                //                oResource = new ResourceManager(typeof(resGlobal_EN));
                //                break;
                //        }
                //        MessageBox.Show(oResource.GetString("tMsgNotAlwDisAftChg"));
                //    }
                //}

                if (bChkAlw)
                {
                    //*Arm 63-07-13 Check Discount Policy
                    //******************** Start ****************************

                    oDiscPolicy = new cDiscPolicy();
                    //ตรวจสอบอนุญาตให้ลดจำนวน หรือไม่
                    bStaDis = oDiscPolicy.C_CHKbkDiscPolicy("C_KBDxDisPerBillByDT", 2);

                    if (bStaDis)
                    {
                        // อนุญาตให้ทำรายการ
                        if (!string.IsNullOrEmpty(cVB.tVB_DisCode))     //ตรวจสอบ Discode != ค่าว่าง
                        {
                            if (!string.IsNullOrEmpty(cVB.tVB_StaPrice))    //ตรวจสอบสถานะ Price ก่อนลด
                            {
                                //oChooseItem.ShowDialog(); //หน้าต่างเลือกรายการ
                                //if (cSale.nC_DTSeqNo == 0)
                                //{
                                //    return;
                                //}

                                //*Arm 63-08-18
                                cSP.SP_GETxListItem("C_KBDxDisPerBillByDT");
                                if (cSale.nC_DTSeqNo == 0)
                                {
                                    return;
                                }
                                //+++++++++++++

                                cVB.oVB_PdtOrder.cSetPrice = oDiscPolicy.C_GETcGetAmtAlwDisc(3, cVB.tVB_StaPrice, cSale.nC_DTSeqNo); // Get Amount ที่สามารถลดได้

                                if (cVB.oVB_PdtOrder.cSetPrice > 0)
                                {
                                    // มียอดสำหรับการทำราการส่วนลด
                                    aoProDT = new List<cmlProrateDT>();
                                    oProDT = new cmlProrateDT();
                                    oProDT.FNSeqNo = (int)cSale.nC_DTSeqNo;
                                    oProDT.FTBchCode = cVB.tVB_BchCode; ;
                                    oProDT.FTSalDocNo = cVB.tVB_DocNo;
                                    aoProDT.Add(oProDT);

                                    oFrm = new wDiscount(2, 3, aoProDT);
                                    oFrm.ShowDialog();
                                }
                                else
                                {
                                    // ไม่มียอดสำหรับการทำราการส่วนลด
                                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                                }
                            }
                            else
                            {
                                // ไม่ได้กำหนดสถานะ Price ก่อนลด
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 3);
                            }
                        }
                    }
                    else
                    {
                        // ไม่อนุญาตให้ทำรายการ
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDiscPolicyNotAlw"), 3);
                    }
                    //******************** End ****************************
                    //*Arm 63-07-13 Check Discount Policy
                }
                cVB.tVB_KbdScreen = "PAYMENT";
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxDisPerBillByDT : " + oEx.Message);
            }
            finally
            {
                //*Arm 63-07-14 -Clear Parameter (ต้องเคลียร์ทุกครั้งเมื่อจบการทำงาน)
                cVB.tVB_DisCode = "";
                cVB.tVB_DisCodeRef = "";
                cVB.tVB_StaPrice = "";
                oDiscPolicy = null;
                //+++++++++++++++

                oSignIn = null;
                oFrm = null;
                oSql = null;
                aoProDT = null;
                oProDT = null;

                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Print Picking List.
        /// </summary>
        public void C_KBDxPrnPickingList()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        cPrint.C_PRNxPickinglist(cVB.tVB_DocNo);
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen); //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                        {
                            cPrint.C_PRNxPickinglist(cVB.tVB_DocNo);
                        }
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }
                cVB.tVB_KbdScreen = "PAYMENT";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxRetriveBill : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        /// <summary>
        /// ดูข้อมูลรถ (Arm 63-06-26)
        /// </summary>
        public void C_KBDxVehicle()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wVehicle oVehicle = null;
            bool bRole = false;
            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        bRole = true;
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen);
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            bRole = true;
                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                if (bRole == true)
                {
                    // verify
                    if (string.IsNullOrEmpty(cVB.tVB_ApiVehicle)) //*Arm 63-08-09
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlVehicleNotDefine"), 3);
                        return;
                    }
                    if (string.IsNullOrEmpty(cVB.tVB_ApiVehicle_Auth))  //*Arm 63-08-09
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgAuthNotDefine"), 3);
                        return;
                    }
                    if (string.IsNullOrEmpty(cVB.tVB_KubotaID))
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgKubotaIDNotFound"), 3);
                        return;
                    }
                    // end verify
                    oVehicle = new wVehicle(cVB.tVB_KubotaID);
                    oVehicle.ShowDialog();
                }
                else
                {
                    return;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxVehicle : " + oEx.Message); }
            finally
            {
                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ลงทะเบียนเครื่องจุดขาย (Arm 63-07-10)
        /// </summary>
        public void C_KBDxPosRegister()
        {
            //string tChkRole, tFuncCode;
            //wAuthentication oSignIn = null; //*Net 63-02-24 เปลี่ยนการยืนยันสิทธิ์ไปใช้ wAuthen
            wPosRegister oPosRegister = null;
            bool bRole = true;

            StringBuilder oSql;
            cDatabase oDB;


            try
            {
                //tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                //tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode);

                //switch (tChkRole)
                //{
                //    case "1":   // allowed.
                //        bRole = true;
                //        break;
                //    case "0":   // not permission.
                //    case "800": // data not found.
                //        oSignIn = new wAuthentication(1, cVB.tVB_KbdScreen);
                //        oSignIn.ShowDialog();

                //        if (oSignIn.DialogResult == DialogResult.OK)
                //            bRole = true;
                //        break;
                //    case "900":
                //        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                //        break;
                //}

                if (bRole == true)
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();
                    string tToken = "";
                    bool tChkReg = false;

                    oSql.AppendLine("SELECT FTPrgRegToken FROM TCNMPos WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                    tToken = oDB.C_GEToDataQuery<string>(oSql.ToString());
                    if (!string.IsNullOrEmpty(tToken))
                    {
                        //มีการลงทะเบียนไปแล้ว(ไม่ส่ง)

                        string tThisMac = new cSP().SP_GETtMacAddress(); //Get Mac. Address
                        string tTokenDecrypt = "";
                        tTokenDecrypt = new cEncryptDecrypt("2").C_CALtDecrypt(tToken); //ถอดรหัส Token
                        string[] atoken = tTokenDecrypt.Split(':');
                        if (atoken.Length == 1)
                        {
                            //มีการลงทะเบียนไปแล้วรอการอนุมัติ 
                            if (tThisMac == atoken[0].ToString())
                            {
                                //กรณีเป็นค่า Mac Addr ของเครื่องตัวเอง  แจ้ง  "ลงทะเบียนแล้วรอการอนุมัติ"
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosRegWaitApv"), 1);
                            }
                            else
                            {
                                //กรณีค่า Mac Addr เครื่องอื่น(เข้าหน้าจอลงทะเบียนปกติ)
                                oPosRegister = new wPosRegister();
                                oPosRegister.ShowDialog();
                            }
                            
                        }
                        else
                        {
                            //string tBchCode = atoken[0].ToString();
                            //string tPosCode = atoken[1].ToString();
                            //string tMac = atoken[2].ToString();
                            //DateTime dExpried = Convert.ToDateTime(atoken[3].ToString());
                            DateTime dExpried = DateTime.Today;
                            string tBchCode = "";
                            string tPosCode="";
                            string tMac="" ;
                            string tExpried = "";
                            try
                            {
                                tBchCode = atoken[0].ToString();
                                tPosCode = atoken[1].ToString();
                                tMac = atoken[2].ToString();
                                dExpried = Convert.ToDateTime(atoken[3].ToString());
                                tExpried = string.Format("{0: dd-MM-yyyy}", dExpried);
                            }
                            catch(Exception oEx)
                            {
                                new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxPosRegister :" + oEx.Message);
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosRegTokenFmtInvalid"), 3);
                            }
                            

                            if(tBchCode == cVB.tVB_BchCode && tPosCode == cVB.tVB_PosCode && tMac == tThisMac)
                            {
                                new cSP().SP_SHWxMsg(string.Format(cVB.oVB_GBResource.GetString("tMsgStaPosRegistered"), Environment.NewLine, tBchCode + Environment.NewLine, tPosCode + Environment.NewLine, tMac + Environment.NewLine, tExpried), 1);
                            }
                            else
                            {
                                oPosRegister = new wPosRegister();
                                oPosRegister.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        //อนุญาตให้ส่งไปลงทะเบียนใหม่
                        oPosRegister = new wPosRegister();
                        oPosRegister.ShowDialog();
                    }
                    
                }
                else
                {
                    return;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cFunctionKeyboard", "C_KBDxPosRegister : " + oEx.Message); }
            finally
            {
                //oSignIn = null;
                //tChkRole = null;
                //tFuncCode = null;
                oPosRegister = null;
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        #endregion End Keyboard Function 
    }
}
