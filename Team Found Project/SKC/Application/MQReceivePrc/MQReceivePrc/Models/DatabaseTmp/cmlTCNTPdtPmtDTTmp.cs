using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtDTTmp
    {
        ///// <summary>
        /////รหัสสาขา
        ///// <summary>
        //public string FTBchCode { get; set; }

        ///// <summary>
        /////รหัสโปรโมชั่น XXYY-######
        ///// <summary>
        //public string FTPmhCode { get; set; }

        ///// <summary>
        /////ลำดับ
        ///// <summary>
        //public Nullable<Int64> FNPmdSeq { get; set; }

        ///// <summary>
        /////รหัสรูปแบบโปรโมชั่น
        ///// <summary>
        //public string FTSpmCode { get; set; }

        ///// <summary>
        /////ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        ///// <summary>
        //public string FTPmdGrpType { get; set; }

        ///// <summary>
        /////ชื่อกลุ่มจัดรายการ
        ///// <summary>
        //public string FTPmdGrpName { get; set; }

        ///// <summary>
        /////รหัสสินค้า
        ///// <summary>
        //public string FTPdtCode { get; set; }

        ///// <summary>
        /////รหัสหน่วยสินค้า
        ///// <summary>
        //public string FTPunCode { get; set; }

        ///// <summary>
        /////ราคาขาย หลังจาก แก้ไข, เครื่องชั่ง, น้ำหนัก, โปรโมชั่น หรือปกติ ถ้าไม่แก้ไข
        ///// <summary>
        //public Nullable<decimal> FCPmdSetPriceOrg { get; set; }

        ///// <summary>
        /////วันที่ปรับปรุงรายการล่าสุด
        ///// <summary>
        //public Nullable<DateTime> FDLastUpdOn { get; set; }

        ///// <summary>
        /////ผู้ปรับปรุงรายการล่าสุด
        ///// <summary>
        //public string FTLastUpdBy { get; set; }

        ///// <summary>
        /////วันที่สร้างรายการ
        ///// <summary>
        //public Nullable<DateTime> FDCreateOn { get; set; }

        ///// <summary>
        /////ผู้สร้างรายการ
        ///// <summary>
        //public string FTCreateBy { get; set; }




        //*Arm 63-03-27
        
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// <summary>
        public string FTPmhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<Int64> FNPmdSeq { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        /// <summary>
        public string FTPmdStaType { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// <summary>
        public string FTPmdGrpName { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string FTPmdRefCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// <summary>
        public string FTPmdSubRef { get; set; }

        /// <summary>
        ///รหัสบาร์โค้ด ณ. บันทึก
        /// <summary>
        public string FTPmdBarCode { get; set; }

    }
}
