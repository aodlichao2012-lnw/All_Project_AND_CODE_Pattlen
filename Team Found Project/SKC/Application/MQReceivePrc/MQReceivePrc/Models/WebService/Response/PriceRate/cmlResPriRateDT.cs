using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.PriceRate
{
    public class cmlResPriRateDT
    {
        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด
        /// <summary>
        public string rtRthCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<Int64> rnRtdSeqNo { get; set; }

        /// <summary>
        ///จำนวน หน่วย ตาม ระดับ เช่น 4 ชั่วโมงแรก
        /// <summary>
        public Nullable<Int64> rnRtdMinQty { get; set; }

        /// <summary>
        ///คำนวนจากหน่วย
        /// <summary>
        public Nullable<Int64> rnRtdCalMin { get; set; }

        /// <summary>
        ///หน่วย 1:นาที 2:ชั่วโมง 3:วัน 4:เดือน 5: ปี   ตาม ระดับ เช่น ชั่วโมง
        /// <summary>
        public string rtRtdTmeType { get; set; }

        /// <summary>
        ///อัตราส่วน ต่อหน่วย ตาม ระดับ เช่น 4 ชั่วโมงแรก คิดเป็น 1 หนวย
        /// <summary>
        public Nullable<decimal> rcRtdTmeFact { get; set; }

        /// <summary>
        ///ราคา ตาม ระดับ
        /// <summary>
        public Nullable<decimal> rcRtdPrice { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }
    }
}
