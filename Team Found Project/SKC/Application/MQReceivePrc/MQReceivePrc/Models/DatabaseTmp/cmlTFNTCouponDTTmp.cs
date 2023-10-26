using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNTCouponDTTmp
    {
        ///<summary>
		///รหัสสาขา
		///</summary>
		public string FTBchCode { get; set; }

        ///<summary>
        ///เลขที่เอกสาร
        ///</summary>
        public string FTCphDocNo { get; set; }

        ///<summary>
        ///คูปองบาร์ (หมายเลขคูปอง)
        ///</summary>
        public string FTCpdBarCpn { get; set; }

        ///<summary>
        ///ลำดับ
        ///</summary>
        public Int64? FNCpdSeqNo { get; set; }

        ///<summary>
        ///จำนวนอนุญาตใช้งาน 0 : ครั้งเดียว
        ///</summary>
        public Int64? FNCpdAlwMaxUse { get; set; }
    }
}
