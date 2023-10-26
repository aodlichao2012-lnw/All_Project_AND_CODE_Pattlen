using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtHDTmp
    {
        //public string FTBchCode { get; set; }
        //public string FTPmhCode { get; set; }
        //public string FTPmhName { get; set; }
        //public string FTPmhNameSlip { get; set; }
        //public string FTSpmCode { get; set; }
        //public string FTSpmType { get; set; }
        //public Nullable<DateTime> FDPmhDStart { get; set; }
        //public Nullable<DateTime> FDPmhDStop { get; set; }
        //public Nullable<DateTime> FDPmhTStart { get; set; }
        //public Nullable<DateTime> FDPmhTStop { get; set; }
        //public string FTPmhClosed { get; set; }
        //public string FTPmhStatus { get; set; }
        //public string FTPmhRetOrWhs { get; set; }
        //public string FTPmhRmk { get; set; }
        //public string FTPmhStaPrcDoc { get; set; }
        //public Nullable<int> FNPmhStaAct { get; set; }
        //public string FTUsrCode { get; set; }
        //public string FTPmhApvCode { get; set; }
        //public string FTPmhBchTo { get; set; }
        //public string FTPmhZneTo { get; set; }
        //public string FTPmhStaExceptPmt { get; set; }
        //public string FTSpmStaRcvFree { get; set; }
        //public string FTSpmStaAlwOffline { get; set; }
        //public string FTSpmStaChkLimitGet { get; set; }
        //public Nullable<long> FNPmhLimitNum { get; set; }
        //public string FTPmhStaLimit { get; set; }
        //public string FTPmhStaLimitCst { get; set; }
        //public string FTSpmStaChkCst { get; set; }
        //public Nullable<long> FNPmhCstNum { get; set; }
        //public string FTSpmStaChkCstDOB { get; set; }
        //public Nullable<long> FNPmhCstDobNum { get; set; }
        //public Nullable<long> FNPmhCstDobPrev { get; set; }
        //public Nullable<long> FNPmhCstDobNext { get; set; }
        //public string FTSpmStaUseRange { get; set; }
        //public string FTSplCode { get; set; }
        //public Nullable<DateTime> FDPntSplStart { get; set; }
        //public Nullable<DateTime> FDPntSplExpired { get; set; }
        //public string FTPmgCode { get; set; }
        //public string FTAggCode { get; set; }
        //public Nullable<DateTime> FDLastUpdOn { get; set; }
        //public Nullable<DateTime> FDCreateOn { get; set; }
        //public string FTLastUpdBy { get; set; }
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
        ///วันที่เริ่ม
        /// <summary>
        public Nullable<DateTime> FDPmhDStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด
        /// <summary>
        public Nullable<DateTime> FDPmhDStop { get; set; }

        /// <summary>
        ///เวลาเริ่ม
        /// <summary>
        public Nullable<DateTime> FDPmhTStart { get; set; }

        /// <summary>
        ///เวลาสิ้นสุด
        /// <summary>
        public Nullable<DateTime> FDPmhTStop { get; set; }

        /// <summary>
        ///คิดต่อสมาชิก/ทั้งหมด 1:ทั้งหมด 2: สมาชิก
        /// <summary>
        public string FTPmhStaLimitCst { get; set; }

        /// <summary>
        ///หยุดรายการ 0: เปิดใช้  1: หยุด
        /// <summary>
        public string FTPmhStaClosed { get; set; }

        /// <summary>
        ///สถานะเอกสาร ว่าง:ยังไม่สมบูรณ์, 1:สมบูรณ์
        /// <summary>
        public string FTPmhStaDoc { get; set; }

        /// <summary>
        ///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        /// <summary>
        public string FTPmhStaApv { get; set; }

        /// <summary>
        ///สถานะ prc เอกสาร ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// <summary>
        public string FTPmhStaPrcDoc { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        /// <summary>
        public Nullable<int> FNPmhStaDocAct { get; set; }

        /// <summary>
        ///รหัสผู้บันทึก
        /// <summary>
        public string FTUsrCode { get; set; }

        /// <summary>
        ///รหัสผู้อนุมัติ
        /// <summary>
        public string FTPmhUsrApv { get; set; }

        /// <summary>
        ///สถานะอนุญาต คำนวนสินค้าซ้อนโปรโมชั่น 1:อนญาต 2:ไม่อนญาต
        /// <summary>
        public string FTPmhStaOnTopPmt { get; set; }

        /// <summary>
        ///สถานะอนุญาตให้นำยอด ไปคำนวนแต้ม ปกติ 1:อนญาต 2:ไม่อนญาต
        /// <summary>
        public string FTPmhStaAlwCalPntStd { get; set; }

        /// <summary>
        ///สถานะรับของแถม 1:จุดขายคำนวนอัตโนมัติ 2:จุดขายเลือกได้ 3:จุดบริการ
        /// <summary>
        public string FTPmhStaRcvFree { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด จำนวนครั้ง ต่อวัน ต่อเดือน (1:ใช้งาน,2:ไม่ใช้งาน)
        /// <summary>
        public string FTPmhStaLimitGet { get; set; }

        /// <summary>
        ///คิดต่อวัน/เดือน 1:ต่อวัน 2: ต่อเดือน 3:ต่อปี
        /// <summary>
        public string FTPmhStaLimitTime { get; set; }

        /// <summary>
        ///เงื่อนไขการเลือกสินค้ากรณี>1รายการในบิล 1:ราคามากกว่า 2:ราคาน้อยกว่า 3:user เลือก
        /// <summary>
        public string FTPmhStaGetPdt { get; set; }

        /// <summary>
        ///รหัสอ้างอิงบัญชีของโปรโมชั่น
        /// <summary>
        public string FTPmhRefAccCode { get; set; }

        /// <summary>
        ///เก็บสถานะของกลุ่มยกเว้น ว่าเป็น 1:สินค้า หรือ 2:แบรนด์
        /// <summary>
        public string FTPmhStaPdtExc { get; set; }

        /// <summary>
        ///กลุ่มที่มีสิทธิอนุมัติ  ว่าง: ได้ Auto  ไม่ว่าง: popup user login
        /// <summary>
        public string FTRolCode { get; set; }

        /// <summary>
        ///จำกัดจำนวนครั้งที่จะได้รับ โปรโมชั่น
        /// <summary>
        public Nullable<Int64> FNPmhLimitQty { get; set; }

        /// <summary>
        ///เงื่อนไขจำกัดจำนวน 1:ต่อสาขา 2: ต่อบริษัท
        /// <summary>
        public string FTPmhStaChkLimit { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด เงื่อนไขเฉพาะสมาชิก (1:ใช้งาน,2:ไม่ใช้งาน)
        /// <summary>
        public string FTPmhStaChkCst { get; set; }

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
