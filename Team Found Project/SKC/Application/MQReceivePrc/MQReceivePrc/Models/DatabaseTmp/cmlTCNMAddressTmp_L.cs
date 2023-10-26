using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMAddressTmp_L
    {
        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///1:Branch 2: User 3:Saleman 4:ร้านค้า 5:Agency 6:Pos,7:Merchant
        /// <summary>
        public string FTAddGrpType { get; set; }

        /// <summary>
        ///รหัสอ้างอิง Branch , User ,Saleman , ร้านค้า,เครื่องจุดขาย
        /// <summary>
        public string FTAddRefCode { get; set; }

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
        ///หมายเลขประจำตัวผู้เสียภาษี
        /// <summary>
        public string FTAddTaxNo { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTAddRmk { get; set; }

        /// <summary>
        ///เก็บข้อมูลประเทศ
        /// <summary>
        public string FTAddCountry { get; set; }

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
