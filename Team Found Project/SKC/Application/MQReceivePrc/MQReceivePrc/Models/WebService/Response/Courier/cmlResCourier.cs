using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Courier
{
    public class cmlResCourier
    {
        /// <summary>
        ///รหัสบริษัทส่งพัสดุ
        /// <summary>
        public string rtCryCode { get; set; }

        /// <summary>
        ///จำนวนวันให้เครดิต
        /// <summary>
        public Nullable<Int64> rnCryCrTerm { get; set; }

        /// <summary>
        ///วงเงินเครดิต
        /// <summary>
        public Nullable<decimal> rcCryCrLimit { get; set; }

        /// <summary>
        ///เลขที่บัตรประจำตัวประชาชน/Passport
        /// <summary>
        public string rtCryCardID { get; set; }

        /// <summary>
        ///หมายเลขประจำตัวผู้เสียภาษี
        /// <summary>
        public string rtCryTaxNo { get; set; }

        /// <summary>
        ///เบอร์โทรศัพฑ์
        /// <summary>
        public string rtCryTel { get; set; }

        /// <summary>
        ///เบอร์โทรสาร
        /// <summary>
        public string rtCryFax { get; set; }

        /// <summary>
        ///เบอร์ E-mail
        /// <summary>
        public string rtCryEmail { get; set; }

        /// <summary>
        ///เพศ 1:ชาย, 2:หญิง
        /// <summary>
        public string rtCrySex { get; set; }

        /// <summary>
        ///วันเกิด
        /// <summary>
        public Nullable<DateTime> rdCryDob { get; set; }

        /// <summary>
        ///รหัสกลุ่ม
        /// <summary>
        public string rtCgpCode { get; set; }

        /// <summary>
        ///รหัสประเภท
        /// <summary>
        public string rtCtyCode { get; set; }

        /// <summary>
        ///ประเภทกิจการ 1:นิติบุคคล, 2:บุคคลธรรมดา
        /// <summary>
        public string rtCryBusiness { get; set; }

        /// <summary>
        ///สาขาของลูกค้าเป็นสำนักงานใหญ่    1:สำนักงานใหญ่   ฯลฯ : สาขา
        /// <summary>
        public string rtCryBchHQ { get; set; }

        /// <summary>
        ///รหัสสาขาของลูกค้า
        /// <summary>
        public string rtCryBchCode { get; set; }

        /// <summary>
        ///รหัสขั่นข้อมูล QR
        /// <summary>
        public string rtCryDelimeterQR { get; set; }

        /// <summary>
        ///สถานะติดต่อ 1:ติดต่อ, 2:เลิกติดต่อ
        /// <summary>
        public string rtCryStaActive { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งานผู้ส่ง 1:Pin 2:RFID 3:QR
        /// <summary>
        public string rtCryLoginType { get; set; }

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
