using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductPromotion
{
    public class cmlResInfoPdtPmtHD
    {

        //*Arm 63-03-26 ปรับแก้ Standard 
        
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// <summary>
        public string rtPmhDocNo { get; set; }

        /// <summary>
        ///วันที่เริ่ม
        /// <summary>
        public Nullable<DateTime> rdPmhDStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด
        /// <summary>
        public Nullable<DateTime> rdPmhDStop { get; set; }

        /// <summary>
        ///เวลาเริ่ม
        /// <summary>
        public Nullable<DateTime> rdPmhTStart { get; set; }

        /// <summary>
        ///เวลาสิ้นสุด
        /// <summary>
        public Nullable<DateTime> rdPmhTStop { get; set; }

        /// <summary>
        ///คิดต่อสมาชิก/ทั้งหมด 1:ทั้งหมด 2: สมาชิก
        /// <summary>
        public string rtPmhStaLimitCst { get; set; }

        /// <summary>
        ///หยุดรายการ 0: เปิดใช้  1: หยุด
        /// <summary>
        public string rtPmhStaClosed { get; set; }

        /// <summary>
        ///สถานะเอกสาร ว่าง:ยังไม่สมบูรณ์, 1:สมบูรณ์
        /// <summary>
        public string rtPmhStaDoc { get; set; }

        /// <summary>
        ///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        /// <summary>
        public string rtPmhStaApv { get; set; }

        /// <summary>
        ///สถานะ prc เอกสาร ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// <summary>
        public string rtPmhStaPrcDoc { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        /// <summary>
        public Nullable<int> rnPmhStaDocAct { get; set; }

        /// <summary>
        ///รหัสผู้บันทึก
        /// <summary>
        public string rtUsrCode { get; set; }

        /// <summary>
        ///รหัสผู้อนุมัติ
        /// <summary>
        public string rtPmhUsrApv { get; set; }

        /// <summary>
        ///สถานะอนุญาต คำนวนสินค้าซ้อนโปรโมชั่น 1:อนญาต 2:ไม่อนญาต
        /// <summary>
        public string rtPmhStaOnTopPmt { get; set; }

        /// <summary>
        ///สถานะอนุญาตให้นำยอด ไปคำนวนแต้ม ปกติ 1:อนญาต 2:ไม่อนญาต
        /// <summary>
        public string rtPmhStaAlwCalPntStd { get; set; }

        /// <summary>
        ///สถานะรับของแถม 1:จุดขายคำนวนอัตโนมัติ 2:จุดขายเลือกได้ 3:จุดบริการ
        /// <summary>
        public string rtPmhStaRcvFree { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด จำนวนครั้ง ต่อวัน ต่อเดือน (1:ใช้งาน,2:ไม่ใช้งาน)
        /// <summary>
        public string rtPmhStaLimitGet { get; set; }

        /// <summary>
        ///คิดต่อวัน/เดือน 1:ต่อวัน 2: ต่อเดือน 3:ต่อปี
        /// <summary>
        public string rtPmhStaLimitTime { get; set; }

        /// <summary>
        ///เงื่อนไขการเลือกสินค้ากรณี>1รายการในบิล 1:ราคามากกว่า 2:ราคาน้อยกว่า 3:user เลือก
        /// <summary>
        public string rtPmhStaGetPdt { get; set; }

        /// <summary>
        ///รหัสอ้างอิงบัญชีของโปรโมชั่น
        /// <summary>
        public string rtPmhRefAccCode { get; set; }

        /// <summary>
        ///เก็บสถานะของกลุ่มยกเว้น ว่าเป็น 1:สินค้า หรือ 2:แบรนด์
        /// <summary>
        public string rtPmhStaPdtExc { get; set; }

        /// <summary>
        ///กลุ่มที่มีสิทธิอนุมัติ  ว่าง: ได้ Auto  ไม่ว่าง: popup user login
        /// <summary>
        public string rtRolCode { get; set; }

        /// <summary>
        ///จำกัดจำนวนครั้งที่จะได้รับ โปรโมชั่น
        /// <summary>
        public Nullable<Int64> rnPmhLimitQty { get; set; }

        /// <summary>
        ///เงื่อนไขจำกัดจำนวน 1:ต่อสาขา 2: ต่อบริษัท
        /// <summary>
        public string rtPmhStaChkLimit { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด เงื่อนไขเฉพาะสมาชิก (1:ใช้งาน,2:ไม่ใช้งาน)
        /// <summary>
        public string rtPmhStaChkCst { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string rtCreateBy { get; set; }

        //+++++++++++++



        ///// <summary>
        /////รหัสสาขา
        ///// <summary>
        //public string rtFTBchCode { get; set; }

        ///// <summary>
        /////รหัสโปรโมชั่น XXYY-######
        ///// <summary>
        //public string rtFTPmhDocNo { get; set; }

        ///// <summary>
        /////วันที่เริ่ม
        ///// <summary>
        //public Nullable<DateTime> rdFDPmhDStart { get; set; }

        ///// <summary>
        /////วันที่สิ้นสุด
        ///// <summary>
        //public Nullable<DateTime> rdFDPmhDStop { get; set; }

        ///// <summary>
        /////เวลาเริ่ม
        ///// <summary>
        //public Nullable<DateTime> rdFDPmhTStart { get; set; }

        ///// <summary>
        /////เวลาสิ้นสุด
        ///// <summary>
        //public Nullable<DateTime> rdFDPmhTStop { get; set; }

        ///// <summary>
        /////คิดต่อสมาชิก/ทั้งหมด 1:ทั้งหมด 2: สมาชิก
        ///// <summary>
        //public string rtFTPmhStaLimitCst { get; set; }

        ///// <summary>
        /////หยุดรายการ 0: เปิดใช้  1: หยุด
        ///// <summary>
        //public string rtFTPmhStaClosed { get; set; }

        ///// <summary>
        /////สถานะเอกสาร ว่าง:ยังไม่สมบูรณ์, 1:สมบูรณ์
        ///// <summary>
        //public string rtFTPmhStaDoc { get; set; }

        ///// <summary>
        /////สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        ///// <summary>
        //public string rtFTPmhStaApv { get; set; }

        ///// <summary>
        /////สถานะ prc เอกสาร ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        ///// <summary>
        //public string rtFTPmhStaPrcDoc { get; set; }

        ///// <summary>
        /////สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        ///// <summary>
        //public Nullable<int> rnFNPmhStaDocAct { get; set; }

        ///// <summary>
        /////รหัสผู้บันทึก
        ///// <summary>
        //public string rtFTUsrCode { get; set; }

        ///// <summary>
        /////รหัสผู้อนุมัติ
        ///// <summary>
        //public string rtFTPmhUsrApv { get; set; }

        ///// <summary>
        /////สถานะอนุญาต คำนวนสินค้าซ้อนโปรโมชั่น 1:อนญาต 2:ไม่อนญาต
        ///// <summary>
        //public string rtFTPmhStaOnTopPmt { get; set; }

        ///// <summary>
        /////สถานะอนุญาตให้นำยอด ไปคำนวนแต้ม ปกติ 1:อนญาต 2:ไม่อนญาต
        ///// <summary>
        //public string rtFTPmhStaAlwCalPntStd { get; set; }

        ///// <summary>
        /////สถานะรับของแถม 1:จุดขายคำนวนอัตโนมัติ 2:จุดขายเลือกได้ 3:จุดบริการ
        ///// <summary>
        //public string rtFTPmhStaRcvFree { get; set; }

        ///// <summary>
        /////สถานะใช้งาน กำหนด จำนวนครั้ง ต่อวัน ต่อเดือน (1:ใช้งาน,2:ไม่ใช้งาน)
        ///// <summary>
        //public string rtFTPmhStaLimitGet { get; set; }

        ///// <summary>
        /////คิดต่อวัน/เดือน 1:ต่อวัน 2: ต่อเดือน 3:ต่อปี
        ///// <summary>
        //public string rtFTPmhStaLimitTime { get; set; }

        ///// <summary>
        /////เงื่อนไขการเลือกสินค้ากรณี>1รายการในบิล 1:ราคามากกว่า 2:ราคาน้อยกว่า 3:user เลือก
        ///// <summary>
        //public string rtFTPmhStaGetPdt { get; set; }

        ///// <summary>
        /////รหัสอ้างอิงบัญชีของโปรโมชั่น
        ///// <summary>
        //public string rtFTPmhRefAccCode { get; set; }

        ///// <summary>
        /////เก็บสถานะของกลุ่มยกเว้น ว่าเป็น 1:สินค้า หรือ 2:แบรนด์
        ///// <summary>
        //public string rtFTPmhStaPdtExc { get; set; }

        ///// <summary>
        /////กลุ่มที่มีสิทธิอนุมัติ  ว่าง: ได้ Auto  ไม่ว่าง: popup user login
        ///// <summary>
        //public string rtFTRolCode { get; set; }

        ///// <summary>
        /////จำกัดจำนวนครั้งที่จะได้รับ โปรโมชั่น
        ///// <summary>
        //public Nullable<Int64> rnFNPmhLimitQty { get; set; }

        ///// <summary>
        /////เงื่อนไขจำกัดจำนวน 1:ต่อสาขา 2: ต่อบริษัท
        ///// <summary>
        //public string rtFTPmhStaChkLimit { get; set; }

        ///// <summary>
        /////สถานะใช้งาน กำหนด เงื่อนไขเฉพาะสมาชิก (1:ใช้งาน,2:ไม่ใช้งาน)
        ///// <summary>
        //public string rtFTPmhStaChkCst { get; set; }

        ///// <summary>
        /////วันที่ปรับปรุงรายการล่าสุด
        ///// <summary>
        //public Nullable<DateTime> rdFDLastUpdOn { get; set; }

        ///// <summary>
        /////ผู้ปรับปรุงรายการล่าสุด
        ///// <summary>
        //public string rtFTLastUpdBy { get; set; }

        ///// <summary>
        /////วันที่สร้างรายการ
        ///// <summary>
        //public Nullable<DateTime> rdFDCreateOn { get; set; }

        ///// <summary>
        /////ผู้สร้างรายการ
        ///// <summary>
        //public string rtFTCreateBy { get; set; }
    }
}
