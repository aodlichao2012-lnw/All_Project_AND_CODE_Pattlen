using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTARTRedeemDTTmp
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
        public Nullable<Int64> FNRddSeq { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        /// </summary>
        public string FTRddStaType { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string FTRddGrpName { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// </summary>
        public string FTPunCode { get; set; }

        /// <summary>
        ///รหัสบาร์โค้ด ณ. บันทึก
        /// </summary>
        public string FTRddBarCode { get; set; }
    }
}
