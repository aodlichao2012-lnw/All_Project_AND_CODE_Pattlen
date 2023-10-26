using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Pos
{
    public class cmlTCNMPos
    {
        /// <summary>
        ///รหัสเครื่อง POS
        /// <summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///ประเภทของเครื่องจุดขาย 1:จุดขาย/ร้านค้า 2:จุดเติมเงิน 3:จุดตรวจสอบมูลค่า 4:Vending 5:Smart Locker
        /// <summary>
        public string FTPosType { get; set; }

        /// <summary>
        ///หมายเลขจดทะเบียน
        /// <summary>
        public string FTPosRegNo { get; set; }

        /// <summary>
        ///รหัสหัวท้ายใบเสร็จ
        /// <summary>
        public string FTSmgCode { get; set; }

        /// <summary>
        ///สถานะ เครื่อง Null or 1:ขายปลีก, 2:ขายส่ง
        /// <summary>
        public string FTPosStaRorW { get; set; }

        /// <summary>
        ///สถานะพิมพ์ EJ 1:มี, 2:ไม่มี
        /// <summary>
        public string FTPosStaPrnEJ { get; set; }

        /// <summary>
        ///สถานะ ส่งภาษี 1:ส่ง, 2:ไม่ส่ง
        /// <summary>
        public string FTPosStaVatSend { get; set; }

        /// <summary>
        ///สถานะ ทำงาน 1:ใช้งาน, 2:ไม่ใช้งาน
        /// <summary>
        public string FTPosStaUse { get; set; }

        /// <summary>
        ///สถานะ เปิดรอบ 1:manual, 2:Auto
        /// <summary>
        public string FTPosStaShift { get; set; }

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนสแกน 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string FTPosStaSumScan { get; set; }     //*Arm 63-05-05

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string FTPosStaSumPrn { get; set; }    //*Arm 63-05-05
        
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
