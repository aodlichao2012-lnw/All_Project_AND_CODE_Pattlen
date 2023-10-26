using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPdtTmp
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ควบคุมสต๊อก 1:ใช่ 2:ไม่ใช่
        /// <summary>
        public string FTPdtStkControl { get; set; }

        /// <summary>
        ///สินค้าควบคุมพิเศษ 1:ควบคุม, 2:ไม่ควบคุม
        /// <summary>
        public string FTPdtGrpControl { get; set; }

        /// <summary>
        ///ระบบ 1:POS 2:REST 3:TICKET 4:RENTAL
        /// <summary>
        public string FTPdtForSystem { get; set; }

        /// <summary>
        ///จำนวนสั่งซื้อ ที่เหมาะสม
        /// <summary>
        public Nullable<decimal> FCPdtQtyOrdBuy { get; set; }

        /// <summary>
        ///ราคาต้นทุน กำหนดเอง
        /// <summary>
        public Nullable<decimal> FCPdtCostDef { get; set; }

        /// <summary>
        ///ต้นทุนอื่น
        /// <summary>
        public Nullable<decimal> FCPdtCostOth { get; set; }

        /// <summary>
        ///ต้นทุนมาตรฐาน
        /// <summary>
        public Nullable<decimal> FCPdtCostStd { get; set; }

        /// <summary>
        ///ระดับจำนวนต่ำสุด
        /// <summary>
        public Nullable<decimal> FCPdtMin { get; set; }

        /// <summary>
        ///ระดับจำนวนสูงสุด
        /// <summary>
        public Nullable<decimal> FCPdtMax { get; set; }

        /// <summary>
        ///1:ให้แต้ม, ไม่ให้แต้ม
        /// <summary>
        public string FTPdtPoint { get; set; }

        /// <summary>
        ///จำนวนแต้มสำหรับแลกสินค้า
        /// <summary>
        public Nullable<decimal> FCPdtPointTime { get; set; }

        /// <summary>
        ///ประเภทสินค้า 1:สินค้าทั่วไป 2:สินค้าบริการ 3: สินค้าอื่นๆ 4:ของแถม, 5:พิเศษ
        /// <summary>
        public string FTPdtType { get; set; }

        /// <summary>
        ///ใช้ราคาขาย 1:บังคับ, 2:แก้ไข, 3:เครื่องชั่ง, 4:น้ำหนัก 6:สินค้ารายการซ่อม
        /// <summary>
        public string FTPdtSaleType { get; set; }

        /// <summary>
        ///1:สินค้าปกติ 2:สินค้าปกติชุด 3: สินค้าSerial 4:สินค้าSerial Set
        /// <summary>
        public string FTPdtSetOrSN { get; set; }

        /// <summary>
        ///สถานะการใช้ราคา1:ใช้ราคาชุด, 2:ใช้ราคารายการย่อย
        /// <summary>
        public string FTPdtStaSetPri { get; set; }

        /// <summary>
        ///สถานะ แสดงรายการย่อย 1:แสดง, 2:ไม่แสดง
        /// <summary>
        public string FTPdtStaSetShwDT { get; set; }

        /// <summary>
        ///สถานะอนุญาต ลด/ชาร์จ  1:อนุญาต , 2:ไม่อนุญาต
        /// <summary>
        public string FTPdtStaAlwDis { get; set; }

        /// <summary>
        ///สถานะอนุญาต คืน  1:อนุญาต , 2:ไม่อนุญาต
        /// <summary>
        public string FTPdtStaAlwReturn { get; set; }

        /// <summary>
        ///สถานะภาษีซื้อ 1:มี 2:ไม่มี
        /// <summary>
        public string FTPdtStaVatBuy { get; set; }

        /// <summary>
        ///สถานะภาษีขาย 1:มี 2:ไม่มี
        /// <summary>
        public string FTPdtStaVat { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 1:ใช่, 2:ไม่ใช่
        /// <summary>
        public string FTPdtStaActive { get; set; }

        /// <summary>
        ///สถานะเปิดใช้งานคำนวน 1:Repack 2:Step Price 3:ไม่เปิด
        /// <summary>
        public string FTPdtStaAlwReCalOpt { get; set; }

        /// <summary>
        ///ประเภทสินค้า 1:ซื้อขาด, 2:ฝากขาย
        /// <summary>
        public string FTPdtStaCsm { get; set; }

        /// <summary>
        ///รหัสกลุ่มหน้าจอ Touch Screen
        /// <summary>
        public string FTTcgCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มสินค้า
        /// <summary>
        public string FTPgpChain { get; set; }

        /// <summary>
        ///ประเภทสินค้า
        /// <summary>
        public string FTPtyCode { get; set; }

        /// <summary>
        ///รหัสยี่ห้อ
        /// <summary>
        public string FTPbnCode { get; set; }

        /// <summary>
        ///รหัสรุ่น
        /// <summary>
        public string FTPmoCode { get; set; }

        /// <summary>
        ///รหัสภาษี
        /// <summary>
        public string FTVatCode { get; set; }

        /// <summary>
        ///รหัสเหตุการณ์ช่วงเวลาห้ามขาย
        /// <summary>
        public string FTEvhCode { get; set; }

        /// <summary>
        ///วันที่เริ่มขายสินค้า
        /// <summary>
        public Nullable<DateTime> FDPdtSaleStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุดขาย
        /// <summary>
        public Nullable<DateTime> FDPdtSaleStop { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }

    }
}
