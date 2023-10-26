using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Product
{
    public class cmlResInfoPdt
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///ควบคุมสต๊อก 1:ใช่ 2:ไม่ใช่
        /// <summary>
        public string rtPdtStkControl { get; set; }

        /// <summary>
        ///สินค้าควบคุมพิเศษ 1:ควบคุม, 2:ไม่ควบคุม
        /// <summary>
        public string rtPdtGrpControl { get; set; }

        /// <summary>
        ///ระบบ 1:POS 2:REST 3:TICKET 4:RENTAL
        /// <summary>
        public string rtPdtForSystem { get; set; }

        /// <summary>
        ///จำนวนสั่งซื้อ ที่เหมาะสม
        /// <summary>
        public Nullable<decimal> rcPdtQtyOrdBuy { get; set; }

        /// <summary>
        ///ราคาต้นทุน กำหนดเอง
        /// <summary>
        public Nullable<decimal> rcPdtCostDef { get; set; }

        /// <summary>
        ///ต้นทุนอื่น
        /// <summary>
        public Nullable<decimal> rcPdtCostOth { get; set; }

        /// <summary>
        ///ต้นทุนมาตรฐาน
        /// <summary>
        public Nullable<decimal> rcPdtCostStd { get; set; }

        /// <summary>
        ///ระดับจำนวนต่ำสุด
        /// <summary>
        public Nullable<decimal> rcPdtMin { get; set; }

        /// <summary>
        ///ระดับจำนวนสูงสุด
        /// <summary>
        public Nullable<decimal> rcPdtMax { get; set; }

        /// <summary>
        ///1:ให้แต้ม, ไม่ให้แต้ม
        /// <summary>
        public string rtPdtPoint { get; set; }

        /// <summary>
        ///จำนวนแต้มสำหรับแลกสินค้า
        /// <summary>
        public Nullable<decimal> rcPdtPointTime { get; set; }

        /// <summary>
        ///ประเภทสินค้า 1:สินค้าทั่วไป 2:สินค้าบริการ 3: สินค้าอื่นๆ 4:ของแถม, 5:พิเศษ
        /// <summary>
        public string rtPdtType { get; set; }

        /// <summary>
        ///ใช้ราคาขาย 1:บังคับ, 2:แก้ไข, 3:เครื่องชั่ง, 4:น้ำหนัก 6:สินค้ารายการซ่อม
        /// <summary>
        public string rtPdtSaleType { get; set; }

        /// <summary>
        ///1:สินค้าปกติ 2:สินค้าปกติชุด 3: สินค้าSerial 4:สินค้าSerial Set
        /// <summary>
        public string rtPdtSetOrSN { get; set; }

        /// <summary>
        ///สถานะการใช้ราคา1:ใช้ราคาชุด, 2:ใช้ราคารายการย่อย
        /// <summary>
        public string rtPdtStaSetPri { get; set; }

        /// <summary>
        ///สถานะ แสดงรายการย่อย 1:แสดง, 2:ไม่แสดง
        /// <summary>
        public string rtPdtStaSetShwDT { get; set; }

        /// <summary>
        ///สถานะอนุญาต ลด/ชาร์จ  1:อนุญาต , 2:ไม่อนุญาต
        /// <summary>
        public string rtPdtStaAlwDis { get; set; }

        /// <summary>
        ///สถานะอนุญาต คืน  1:อนุญาต , 2:ไม่อนุญาต
        /// <summary>
        public string rtPdtStaAlwReturn { get; set; }

        /// <summary>
        ///สถานะภาษีซื้อ 1:มี 2:ไม่มี
        /// <summary>
        public string rtPdtStaVatBuy { get; set; }

        /// <summary>
        ///สถานะภาษีขาย 1:มี 2:ไม่มี
        /// <summary>
        public string rtPdtStaVat { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 1:ใช่, 2:ไม่ใช่
        /// <summary>
        public string rtPdtStaActive { get; set; }

        /// <summary>
        ///สถานะเปิดใช้งานคำนวน 1:Repack 2:Step Price 3:ไม่เปิด
        /// <summary>
        public string rtPdtStaAlwReCalOpt { get; set; }

        /// <summary>
        ///ประเภทสินค้า 1:ซื้อขาด, 2:ฝากขาย
        /// <summary>
        public string rtPdtStaCsm { get; set; }

        /// <summary>
        ///รหัสกลุ่มหน้าจอ Touch Screen
        /// <summary>
        public string rtTcgCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มสินค้า
        /// <summary>
        public string rtPgpChain { get; set; }

        /// <summary>
        ///ประเภทสินค้า
        /// <summary>
        public string rtPtyCode { get; set; }

        /// <summary>
        ///รหัสยี่ห้อ
        /// <summary>
        public string rtPbnCode { get; set; }

        /// <summary>
        ///รหัสรุ่น
        /// <summary>
        public string rtPmoCode { get; set; }

        /// <summary>
        ///รหัสภาษี
        /// <summary>
        public string rtVatCode { get; set; }

        /// <summary>
        ///รหัสเหตุการณ์ช่วงเวลาห้ามขาย
        /// <summary>
        public string rtEvhCode { get; set; }

        /// <summary>
        ///วันที่เริ่มขายสินค้า
        /// <summary>
        public Nullable<DateTime> rdPdtSaleStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุดขาย
        /// <summary>
        public Nullable<DateTime> rdPdtSaleStop { get; set; }

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
    }
}
