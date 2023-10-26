using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Shop
{
    public class cmlResInfoShop
    {
        //public string rtBchCode { get; set; }
        //public string rtShpCode { get; set; }
        //public string rtWahCode { get; set; }
        //public string rtMerCode { get; set; }
        //public string rtShpType { get; set; }
        //public string rtShpRegNo { get; set; }
        //public string rtShpRefID { get; set; }
        //public Nullable<DateTime> rdShpStart { get; set; }
        //public Nullable<DateTime> rdShpStop { get; set; }
        //public Nullable<DateTime> rdShpSaleStart { get; set; }
        //public Nullable<DateTime> rdShpSaleStop { get; set; }
        //public string rtShpStaActive { get; set; }
        //public string rtShpStaClose { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }


        //*Arm 63-06-16 -ปรับตามโครงสร้าง Database SKC

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัสคลังร้านค้า
        /// </summary>
        public string rtWahCode { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }       //*Arm 63-06-16 -เพิ่มตามโครงสร้าง Database SKC

        /// <summary>
        ///1:แสดง 2; ไม่แสดง   (เฉพาะ ShpType=4,5)   default  1
        /// </summary>
        public string rtShpStaShwPrice { get; set; }

        /// <summary>
        ///ประเภท 1:ร้านค้า 2:ฝากขาย 3: Partner เช่น MOL 4:Vending 5: Smart Locker //model
        /// </summary>
        public string rtShpType { get; set; }

        /// <summary>
        ///รหัสสาขาร้านค้าที่จดทะเบียนไว้กับสรรพากร
        /// </summary>
        public string rtShpRegNo { get; set; }

        /// <summary>
        ///รหัสอ้างอิง
        /// </summary>
        public string rtShpRefID { get; set; }

        /// <summary>
        ///วันที่เริ่มดำเนินการ
        /// </summary>
        public Nullable<DateTime> rdShpStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุดดำเนินการ
        /// </summary>
        public Nullable<DateTime> rdShpStop { get; set; }

        /// <summary>
        ///วันที่เริมขาย
        /// </summary>
        public Nullable<DateTime> rdShpSaleStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุดการขาย
        /// </summary>
        public Nullable<DateTime> rdShpSaleStop { get; set; }

        /// <summary>
        ///สถานะ ว่าง Null: ยังไม่เปิดใช้งาน 1: เปิดใช้งาน
        /// </summary>
        public string rtShpStaActive { get; set; }

        /// <summary>
        ///ฝากขาย 1:คำนวณแบบ ปิดบิลลด,2: คำนวณแบบ ปิดบิลเต็ม
        /// </summary>
        public string rtShpStaClose { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
    }
}
