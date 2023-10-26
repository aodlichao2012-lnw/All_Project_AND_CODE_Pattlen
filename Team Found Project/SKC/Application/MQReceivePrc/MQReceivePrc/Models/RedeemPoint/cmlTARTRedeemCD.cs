using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.RedeemPoint
{
    public class cmlTARTRedeemCD
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่นแลกคะแนน XXYY-######
        /// </summary>
        public string FTRdhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> FNRdcSeq { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string FTRddGrpName { get; set; }

        /// <summary>
        ///Redeem code
        /// </summary>
        public string FTRdcRefCode { get; set; }

        /// <summary>
        ///(1)ใช้แต้ม (2)ใช้แต้ม
        /// </summary>
        public Nullable<decimal> FCRdcUsePoint { get; set; }

        /// <summary>
        ///(1)ร่วมกับเงิน (2) ได้ส่วนลด
        /// </summary>
        public Nullable<decimal> FCRdcUseMny { get; set; }

        /// <summary>
        ///ยอดขั้นต่ำ/บิลที่อนุญาต / 0 ไม่มีขั้นต่ำ
        /// </summary>
        public Nullable<decimal> FCRdcMinTotBill { get; set; }
    }
}
