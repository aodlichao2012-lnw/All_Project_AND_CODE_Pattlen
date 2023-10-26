using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Coupon
{
    public class cmlTFNTCouponHDPdt
    {
		///<summary>
		///รหัสสาขา
		///</summary>
		public string FTBchCode { get; set; }

		///<summary>
		///รหัสเอกสาร
		///</summary>
		public string FTCphDocNo { get; set; }

		///<summary>
		///รหัสสินค้า
		///</summary>
		public string FTPdtCode { get; set; }

		///<summary>
		///รหัสหน่วย
		///</summary>
		public string FTPunCode { get; set; }

		///<summary>
		///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
		///</summary>
		public string FTCphStaType { get; set; }


	}
}
