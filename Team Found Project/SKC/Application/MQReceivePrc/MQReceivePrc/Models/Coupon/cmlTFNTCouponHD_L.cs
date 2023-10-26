using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Coupon
{
    public class cmlTFNTCouponHD_L
    {
		///<summary>
		///รหัสเอกสารคูปอง
		///</summary>
		public string FTCphDocNo { get; set; }

		///<summary>
		///รหัสภาษา
		///</summary>
		public Int64 FNLngID { get; set; }

		///<summary>
		///ชื่อคูปอง
		///</summary>
		public string FTCpnName { get; set; }

		///<summary>
		///ข้อความแสดงบนคูปองบันทัดที่ 1
		///</summary>
		public string FTCpnMsg1 { get; set; }

		///<summary>
		///ข้อความแสดงบนคูปองบันทัดที่ 2
		///</summary>
		public string FTCpnMsg2 { get; set; }

		///<summary>
		///เงื่อนไขแสดงบนคูปอง
		///</summary>
		public string FTCpnCond { get; set; }



	}
}
