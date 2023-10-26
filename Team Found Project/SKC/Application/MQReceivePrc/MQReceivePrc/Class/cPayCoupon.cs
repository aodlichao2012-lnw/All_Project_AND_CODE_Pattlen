using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Coupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MQReceivePrc.Class.Standard;

namespace MQReceivePrc.Class
{
    public class cPayCoupon
    {
        public bool C_PRCbPayCoupon(cmlRcvData poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cMS oMS;
            cmlPayCoupon oPayCoupon;
            cmlResResult oResResult;

            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect;
            string tQueueName = "";
            string tErr = "";
            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oPayCoupon = JsonConvert.DeserializeObject<cmlPayCoupon>(poData.ptData);
                oMS = new cMS();
                oSql = new StringBuilder();
                oResResult = new cmlResResult();
                oDB = new cDatabase();
                string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);

                //oSql.AppendLine("SELECT DT.FNCpdAlwMaxUse-ISNULL(DTHis.QtyLef,0)");
                oSql.AppendLine("SELECT (CASE WHEN DT.FNCpdAlwMaxUse = 0 THEN 1 ELSE DT.FNCpdAlwMaxUse-ISNULL(DTHis.QtyLef,0) END) AS FNQty");  //*Em 63-01-14
                oSql.AppendLine("FROM TFNTCouponDT DT WITH(NOLOCK)");
                //oSql.AppendLine("LEFT JOIN (SELECT FTBchCode, FTCphDocNo, FTCpdBarCpn, COUNT(*) as QtyLef FROM TFNTCouponDTHis WHERE FTBchCode ='" + oPayCoupon.ptCpbFrmBch + "' AND FTCphDocNo = '" + oPayCoupon.ptCphDocNo + "' AND FTCpdBarCpn ='" + oPayCoupon.ptCpdBarCpn + "' AND FTCpbStaBook != '3'  GROUP BY FTBchCode,FTCphDocNo,FTCpdBarCpn) DTHis ");
                oSql.AppendLine("LEFT JOIN (SELECT FTCphDocNo, FTCpdBarCpn, COUNT(*) as QtyLef FROM TFNTCouponDTHis WHERE FTCphDocNo = '" + oPayCoupon.ptCphDocNo + "' AND FTCpdBarCpn ='" + oPayCoupon.ptCpdBarCpn + "' AND FTCpbStaBook != '3'  GROUP BY FTCphDocNo,FTCpdBarCpn) DTHis "); //*Arm 63-01-09  ไม่ตรวจสอบสาขาที่สร้างเอกสาร
                oSql.AppendLine("ON DT.FTCphDocNo = DTHis.FTCphDocNo AND DT.FTCpdBarCpn = DTHis.FTCpdBarCpn");
                //oSql.AppendLine("WHERE DT.FTBchCode= '" + oPayCoupon.ptCpbFrmBch + "' AND DT.FTCphDocNo = '" + oPayCoupon.ptCphDocNo + "' AND DT.FTCpdBarCpn = '" + oPayCoupon.ptCpdBarCpn + "'");
                oSql.AppendLine("WHERE DT.FTCphDocNo = '" + oPayCoupon.ptCphDocNo + "' AND DT.FTCpdBarCpn = '" + oPayCoupon.ptCpdBarCpn + "'");     //*Arm 63-01-09  ไม่ตรวจสอบสาขาที่สร้างเอกสาร


                // Check FNCpdAlwMaxUse > 0
                if (oDB.C_DAToExecuteQuery<int>(t_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut) > 0)
                {
                    oSql.AppendLine("INSERT INTO TFNTCouponDTHis (FTBchCode,FTCphDocNo,FTCpdBarCpn,FDCpbFrmStart,");
                    oSql.AppendLine("FTCpbFrmBch,FTCpbFrmPos,FTCpbFrmSalRef,FTCpbStaBook,FDLastUpdOn,");
                    oSql.AppendLine("FTLastUpdBy,FDCreateOn,FTCreateBy) VALUES (");
                    oSql.AppendLine("'" + cVB.tVB_BchCode + "', ");
                    oSql.AppendLine("'" + oPayCoupon.ptCphDocNo + "', ");
                    oSql.AppendLine("'" + oPayCoupon.ptCpdBarCpn + "', ");
                    //oSql.AppendLine("'" + oPayCoupon.pnCpdSeqNo + "', ");
                    oSql.AppendLine("GETDATE(), ");
                    oSql.AppendLine("'" + oPayCoupon.ptCpbFrmBch + "', ");
                    oSql.AppendLine("'" + oPayCoupon.ptCpbFrmPos + "', ");
                    oSql.AppendLine("'" + oPayCoupon.ptSaleDocNo + "', ");
                    oSql.AppendLine("'1',");
                    oSql.AppendLine("GETDATE(),");
                    oSql.AppendLine("'MQReceivePrc',");
                    oSql.AppendLine("GETDATE(),");
                    oSql.AppendLine("'MQReceivePrc' )");
                    
                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect, out tErr);
                    if (nRowAffect > 0)
                    {
                        //บันทึกสำเร็จ
                        oResResult.rtCode = oMS.tMS_RespCode200;
                        oResResult.rtDesc = oMS.tMS_RespDesc200;
                    }
                    else
                    {
                        //บันทึกสำเร็จ ไม่สำเร็จ
                        oResResult.rtCode = oMS.tMS_RespCode400;
                        oResResult.rtDesc = oMS.tMS_RespDesc400 + tErr;
                    }
                }
                else
                {
                    //คูปองใช้งานไม่ได้
                    oResResult.rtCode = oMS.tMS_RespCode401;
                    oResResult.rtDesc = oMS.tMS_RespDesc401;
                }
                
                cmlRcvData oRes = new cmlRcvData();
                oRes.ptFunction = "PayCoupon";
                oRes.ptSource = "MQRcvProcess";
                oRes.ptDest = "AdaPos";
                oRes.ptData = JsonConvert.SerializeObject(oResResult);

                string tMsgJson = JsonConvert.SerializeObject(oRes);
                tQueueName = "FN_PayRetCoupon" + oPayCoupon.ptCpbFrmBch + oPayCoupon.ptCpbFrmPos;
                cFunction.C_PRCxMQPublish(tQueueName, tMsgJson, out ptErrMsg);

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbPayCoupon");
                new cLog().C_WRTxLog("cPayCoupon", "C_PRCbPayCoupon : " + oEx.Message);
                return false;
            }

        }

    }
}
