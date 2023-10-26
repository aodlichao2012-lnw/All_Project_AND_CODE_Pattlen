using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTCNMTaxAddress
    {
        /// <summary>
        ///เลขประจำตัวผู้เสียภาษี
        /// <summary>
        public string FTAddTaxNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///(AUTONUMBER)ลำดับ
        /// <summary>
        public Nullable<Int64> FNAddSeqNo { get; set; }

        /// <summary>
        ///รหัส/ลำดับ อ้างอิง
        /// <summary>
        public string FTAddRefNo { get; set; }

        /// <summary>
        ///ชื่อ
        /// <summary>
        public string FTAddName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTAddRmk { get; set; }

        /// <summary>
        ///เก็บข้อมูลประเทศ
        /// <summary>
        public string FTAddCountry { get; set; }

        /// <summary>
        ///รหัสเขต/ภูมิภาค
        /// <summary>
        public string FTAreCode { get; set; }

        /// <summary>
        ///รหัสโซน
        /// <summary>
        public string FTZneCode { get; set; }

        /// <summary>
        ///1:ใช้งานแบบแยก 2:ใช้งานแบบรวม
        /// <summary>
        public string FTAddVersion { get; set; }

        /// <summary>
        ///บ้านเลขที่
        /// <summary>
        public string FTAddV1No { get; set; }

        /// <summary>
        ///ซอย
        /// <summary>
        public string FTAddV1Soi { get; set; }

        /// <summary>
        ///หมู่บ้าน/อาคาร
        /// <summary>
        public string FTAddV1Village { get; set; }

        /// <summary>
        ///ถนน
        /// <summary>
        public string FTAddV1Road { get; set; }

        /// <summary>
        ///ตำบล/แขวง
        /// <summary>
        public string FTAddV1SubDist { get; set; }

        /// <summary>
        ///รหัสอำเภอ/เขต
        /// <summary>
        public string FTAddV1DstCode { get; set; }

        /// <summary>
        ///รหัสจังหวัด
        /// <summary>
        public string FTAddV1PvnCode { get; set; }

        /// <summary>
        ///รหัสไปรษณีย์
        /// <summary>
        public string FTAddV1PostCode { get; set; }

        /// <summary>
        ///ทีอยู่1
        /// <summary>
        public string FTAddV2Desc1 { get; set; }

        /// <summary>
        ///ทีอยู่2
        /// <summary>
        public string FTAddV2Desc2 { get; set; }

        /// <summary>
        ///website address (Url)
        /// <summary>
        public string FTAddWebsite { get; set; }

        /// <summary>
        ///ตำแหน่งบนแผนที่ แนวตั้ง
        /// <summary>
        public string FTAddLongitude { get; set; }

        /// <summary>
        ///ตำแหน่งบนแผนที่ แนวนอน
        /// <summary>
        public string FTAddLatitude { get; set; }

        /// <summary>
        ///ประเภทธุระกิจ 1: นิติบุคคล, 2:บุคคลธรรมดา
        /// <summary>
        public string FTAddStaBusiness { get; set; }

        /// <summary>
        ///สถานประกอบการ 1: สำนักงานใหญ่ 2:สาขา
        /// <summary>
        public string FTAddStaHQ { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTAddStaBchCode { get; set; }

        /// <summary>
        ///เบอร์โทร
        /// <summary>
        public string FTAddTel { get; set; }    //*Arm 62-10-09  - Upload TaxAddress

        /// <summary>
        ///เบอร์ Fax
        /// <summary>
        public string FTAddFax { get; set; }    //*Arm 62-10-09  - Upload TaxAddress

        /// <summary>
        ///
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///
        /// <summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string FTCreateBy { get; set; }
    }
}
