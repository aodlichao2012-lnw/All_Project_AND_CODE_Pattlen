using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtCDTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// <summary>
        public string FTPmhCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<Int64> FNPmcSeq { get; set; }

        /// <summary>
        ///รหัสรูปแบบโปรโมชั่น
        /// <summary>
        public string FTSpmCode { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// <summary>
        public string FTPmcGrpName { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มซื้อ 2:กลุ่มได้รับ 3:ทั้ง 2 กลุ่ม
        /// <summary>
        public string FTPmcStaGrpCond { get; set; }

        /// <summary>
        ///%เฉลี่ย รวมกันต้องเท่ากับ 100
        /// <summary>
        public Nullable<decimal> FCPmcPerAvgDis { get; set; }

        /// <summary>
        ///ซื้อครบมูลค่า
        /// <summary>
        public Nullable<decimal> FCPmcBuyAmt { get; set; }

        /// <summary>
        ///ซื้อครบจำนวน
        /// <summary>
        public Nullable<decimal> FCPmcBuyQty { get; set; }

        /// <summary>
        ///จากจำนวน ขั้นต้ำที่มีผล
        /// <summary>
        public Nullable<decimal> FCPmcBuyMinQty { get; set; }

        /// <summary>
        ///ถึงจำนวน  กรณีไม่ Limit กำหนดให้น้อยกว่าจากจำนวน จำนวนครั้ง= Max /Min
        /// <summary>
        public Nullable<decimal> FCPmcBuyMaxQty { get; set; }

        /// <summary>
        ///จากเวลา ขั้นต้ำที่มีผล
        /// <summary>
        public Nullable<DateTime> FDPmcBuyMinTime { get; set; }

        /// <summary>
        ///ถึงเวลา  ไม่เกิน 24:00:00 และไม่น้อยกว่า จากเวลา
        /// <summary>
        public Nullable<DateTime> FDPmcBuyMaxTime { get; set; }

        /// <summary>
        ///รูปแบบส่วนลด 1:ลดบาท 2:ลด % 3: ปรับราคา 4: ได้แต้ม
        /// <summary>
        public Nullable<decimal> FCPmcGetCond { get; set; }

        /// <summary>
        ///มูลค้า แปรผันตามรูปแบบส่วนลด
        /// <summary>
        public Nullable<decimal> FCPmcGetValue { get; set; }

        /// <summary>
        ///จำนวนที่จะได้รับ
        /// <summary>
        public Nullable<decimal> FCPmcGetQty { get; set; }

        /// <summary>
        ///1:ผลรวมมูลค่าเฉพาะกลุ่ม,2:ผลรวมมูลค่ากลุ่มซื้อทั้งหมด,3:ผลรวมจำนวนเฉพาะกลุ่ม,4:ผลรวมจำนวนกลุ่มซื้อทั้งหมด
        /// <summary>
        public string FTSpmStaBuy { get; set; }

        /// <summary>
        ///1:ได้รับเงื่อนไขเฉพาะกลุ่ม,2:ได้รับเงื่อนไขกลุ่มได้รับทั้งหมด,3:ได้รับเงื่อนไขเป็นแต้ม
        /// <summary>
        public string FTSpmStaRcv { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนดสินค้าทั้งร้าน (1:ใช้งาน,2:ไม่ใช้งาน)
        /// <summary>
        public string FTSpmStaAllPdt { get; set; }

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
