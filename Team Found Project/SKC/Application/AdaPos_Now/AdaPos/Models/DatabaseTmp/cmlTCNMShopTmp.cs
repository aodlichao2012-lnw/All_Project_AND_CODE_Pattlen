using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMShopTmp
    {
        //public string FTBchCode { get; set; }
        //public string FTShpCode { get; set; }
        //public string FTWahCode { get; set; }
        //public string FTMerCode { get; set; }
        //public string FTShpType { get; set; }
        //public string FTShpRegNo { get; set; }
        //public string FTShpRefID { get; set; }
        //public Nullable<DateTime> FDShpStart { get; set; }
        //public Nullable<DateTime> FDShpStop { get; set; }
        //public Nullable<DateTime> FDShpSaleStart { get; set; }
        //public Nullable<DateTime> FDShpSaleStop { get; set; }
        //public string FTShpStaActive { get; set; }
        //public string FTShpStaClose { get; set; }
        //public Nullable<DateTime> FDLastUpdOn { get; set; }
        //public Nullable<DateTime> FDCreateOn { get; set; }
        //public string FTLastUpdBy { get; set; }
        //public string FTCreateBy { get; set; }


        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// </summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัสคลังร้านค้า
        /// </summary>
        public string FTWahCode { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string FTPplCode { get; set; }

        /// <summary>
        ///1:แสดง 2; ไม่แสดง   (เฉพาะ ShpType=4,5)   default  1
        /// </summary>
        public string FTShpStaShwPrice { get; set; }

        /// <summary>
        ///ประเภท 1:ร้านค้า 2:ฝากขาย 3: Partner เช่น MOL 4:Vending 5: Smart Locker //model
        /// </summary>
        public string FTShpType { get; set; }

        /// <summary>
        ///รหัสสาขาร้านค้าที่จดทะเบียนไว้กับสรรพากร
        /// </summary>
        public string FTShpRegNo { get; set; }

        /// <summary>
        ///รหัสอ้างอิง
        /// </summary>
        public string FTShpRefID { get; set; }

        /// <summary>
        ///วันที่เริ่มดำเนินการ
        /// </summary>
        public Nullable<DateTime> FDShpStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุดดำเนินการ
        /// </summary>
        public Nullable<DateTime> FDShpStop { get; set; }

        /// <summary>
        ///วันที่เริมขาย
        /// </summary>
        public Nullable<DateTime> FDShpSaleStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุดการขาย
        /// </summary>
        public Nullable<DateTime> FDShpSaleStop { get; set; }

        /// <summary>
        ///สถานะ ว่าง Null: ยังไม่เปิดใช้งาน 1: เปิดใช้งาน
        /// </summary>
        public string FTShpStaActive { get; set; }

        /// <summary>
        ///ฝากขาย 1:คำนวณแบบ ปิดบิลลด,2: คำนวณแบบ ปิดบิลเต็ม
        /// </summary>
        public string FTShpStaClose { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }
    }
}
