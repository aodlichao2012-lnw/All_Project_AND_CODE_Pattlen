using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMPriRateDTTmp
    {
        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด
        /// <summary>
        public string FTRthCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<Int64> FNRtdSeqNo { get; set; }

        /// <summary>
        ///จำนวน หน่วย ตาม ระดับ เช่น 4 ชั่วโมงแรก
        /// <summary>
        public Nullable<Int64> FNRtdMinQty { get; set; }

        /// <summary>
        ///คำนวนจากหน่วย
        /// <summary>
        public Nullable<Int64> FNRtdCalMin { get; set; }

        /// <summary>
        ///หน่วย 1:นาที 2:ชั่วโมง 3:วัน 4:เดือน 5: ปี   ตาม ระดับ เช่น ชั่วโมง
        /// <summary>
        public string FTRtdTmeType { get; set; }

        /// <summary>
        ///อัตราส่วน ต่อหน่วย ตาม ระดับ เช่น 4 ชั่วโมงแรก คิดเป็น 1 หนวย
        /// <summary>
        public Nullable<decimal> FCRtdTmeFact { get; set; }

        /// <summary>
        ///ราคา ตาม ระดับ
        /// <summary>
        public Nullable<decimal> FCRtdPrice { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }
    }
}
