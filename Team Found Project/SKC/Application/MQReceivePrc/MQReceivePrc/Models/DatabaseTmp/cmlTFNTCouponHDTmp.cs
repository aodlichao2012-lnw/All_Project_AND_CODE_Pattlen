using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNTCouponHDTmp
    {
        ///<summary>
		///รหัสสาขา
		///</summary>
		public string FTBchCode { get; set; }

        ///<summary>
        ///รหัสเอกสาร
        ///</summary>
        public string FTCphDocNo { get; set; }

        ///<summary>
        ///รหัสประเภทคูปอง
        ///</summary>
        public string FTCptCode { get; set; }

        ///<summary>
        ///วันที่เอกสาร
        ///</summary>
        public DateTime? FDCphDocDate { get; set; }

        ///<summary>
        ///ประเภทส่วนลด 1: ลดบาท , 2: ลด % 3:ใช้กลุ่มราคา
        ///</summary>
        public string FTCphDisType { get; set; }

        ///<summary>
        ///มูลค่าคูปอง
        ///</summary>
        public double? FCCphDisValue { get; set; }

        ///<summary>
        ///รหัสกลุ่มราคา
        ///</summary>
        public string FTPplCode { get; set; }

        ///<summary>
        ///วันที่เริ่มใช้งาน
        ///</summary>
        public DateTime? FDCphDateStart { get; set; }

        ///<summary>
        ///วันสิ้นสุดใช้งาน
        ///</summary>
        public DateTime? FDCphDateStop { get; set; }

        ///<summary>
        ///เวลาเริ่มใช้งาน
        ///</summary>
        public string FTCphTimeStart { get; set; }

        ///<summary>
        ///เวลาสิ้นสุดใช้งาน
        ///</summary>
        public string FTCphTimeStop { get; set; }

        ///<summary>
        ///สถานะหยุดใช้งาน 1:ใช้งาน , 2:หยุดใช้งาน
        ///</summary>
        public string FTCphStaClosed { get; set; }

        ///<summary>
        ///พนักงานคีย์
        ///</summary>
        public string FTUsrCode { get; set; }

        ///<summary>
        ///ผู้อนุมัติ
        ///</summary>
        public string FTCphUsrApv { get; set; }

        ///<summary>
        ///สถานะ เอกสาร  1:สมบูรณ์, 2:ไม่สมบูรณ์, 3:ยกเลิก
        ///</summary>
        public string FTCphStaDoc { get; set; }

        ///<summary>
        ///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        ///</summary>
        public string FTCphStaApv { get; set; }

        ///<summary>
        ///สถานะ ประมวลผลเอกสาร ว่าง หรือ Null:ยังไม่ทำ, 1:ทำแล้ว
        ///</summary>
        public string FTCphStaPrcDoc { get; set; }

        ///<summary>
        ///สถานะ ลบ MQ ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        ///</summary>
        public string FTCphStaDelMQ { get; set; }

        ///<summary>
        ///ยอดต่ำสุดที่สามารถใช้ได้ 0:ไม่ตรวจสอบ
        ///</summary>
        public double? FCCphMinValue { get; set; }

        ///<summary>
        ///คำนวนรวมสินค้าโปรโมชั่น 1:อนญาตคำนวน 2:ไม่อนญาตคำนวน
        ///</summary>
        public string FTCphStaOnTopPmt { get; set; }

        ///<summary>
        ///จำนวนจำกัดการใช้ต่อบิล 0: ไม่จำกัด
        ///</summary>
        public Int64? FNCphLimitUsePerBill { get; set; }

        ///<summary>
        ///รหัสอ้างอิงบัญชีของคูปอง
        ///</summary>
        public string FTCphRefAccCode { get; set; }

        ///<summary>
        ///สถานะตรวจสอบลูกค้า
        ///</summary>
        public string FTStaChkMember { get; set; }

        ///<summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        ///</summary>
        public DateTime? FDLastUpdOn { get; set; }

        ///<summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        ///</summary>
        public string FTLastUpdBy { get; set; }

        ///<summary>
        ///วันที่สร้างรายการ
        ///</summary>
        public DateTime? FDCreateOn { get; set; }

        ///<summary>
        ///ผู้สร้างรายการ
        ///</summary>
        public string FTCreateBy { get; set; }
    }
}
