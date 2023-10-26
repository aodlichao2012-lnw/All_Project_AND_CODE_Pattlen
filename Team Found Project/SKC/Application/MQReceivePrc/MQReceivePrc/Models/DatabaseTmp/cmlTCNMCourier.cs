using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCourier
    {
        /// <summary>
        ///รหัสบริษัทส่งพัสดุ
        /// <summary>
        public string FTCryCode { get; set; }

        /// <summary>
        ///จำนวนวันให้เครดิต
        /// <summary>
        public Nullable<Int64> FNCryCrTerm { get; set; }

        /// <summary>
        ///วงเงินเครดิต
        /// <summary>
        public Nullable<decimal> FCCryCrLimit { get; set; }

        /// <summary>
        ///เลขที่บัตรประจำตัวประชาชน/Passport
        /// <summary>
        public string FTCryCardID { get; set; }

        /// <summary>
        ///หมายเลขประจำตัวผู้เสียภาษี
        /// <summary>
        public string FTCryTaxNo { get; set; }

        /// <summary>
        ///เบอร์โทรศัพฑ์
        /// <summary>
        public string FTCryTel { get; set; }

        /// <summary>
        ///เบอร์โทรสาร
        /// <summary>
        public string FTCryFax { get; set; }

        /// <summary>
        ///เบอร์ E-mail
        /// <summary>
        public string FTCryEmail { get; set; }

        /// <summary>
        ///เพศ 1:ชาย, 2:หญิง
        /// <summary>
        public string FTCrySex { get; set; }

        /// <summary>
        ///วันเกิด
        /// <summary>
        public Nullable<DateTime> FDCryDob { get; set; }

        /// <summary>
        ///รหัสกลุ่ม
        /// <summary>
        public string FTCgpCode { get; set; }

        /// <summary>
        ///รหัสประเภท
        /// <summary>
        public string FTCtyCode { get; set; }

        /// <summary>
        ///ประเภทกิจการ 1:นิติบุคคล, 2:บุคคลธรรมดา
        /// <summary>
        public string FTCryBusiness { get; set; }

        /// <summary>
        ///สาขาของลูกค้าเป็นสำนักงานใหญ่    1:สำนักงานใหญ่   ฯลฯ : สาขา
        /// <summary>
        public string FTCryBchHQ { get; set; }

        /// <summary>
        ///รหัสสาขาของลูกค้า
        /// <summary>
        public string FTCryBchCode { get; set; }

        /// <summary>
        ///รหัสขั่นข้อมูล QR
        /// <summary>
        public string FTCryDelimeterQR { get; set; }

        /// <summary>
        ///สถานะติดต่อ 1:ติดต่อ, 2:เลิกติดต่อ
        /// <summary>
        public string FTCryStaActive { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งานผู้ส่ง 1:Pin 2:RFID 3:QR
        /// <summary>
        public string FTCryLoginType { get; set; }

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
