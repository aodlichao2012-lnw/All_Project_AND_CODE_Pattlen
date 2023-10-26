using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    class cmlTFNTCouponHDTmp_L
	{
		///<summary>
		///รหัสสาขา
		///</summary>
		public string FTBchCode { get; set; }
		///<summary>
		///รหัสเอกสารคูปอง
		///</summary>
		public string FTCphDocNo { get; set; }

		///<summary>
		///รหัสภาษา
		///</summary>
		public int FNLngID { get; set; }

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
