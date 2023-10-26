using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    class cmlTFNTCouponHDBchTmp
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
		///รหัสสาขา
		///</summary>
		public string FTCphBchTo { get; set; }

		///<summary>
		///รหัสตัวแทน/เจ้าของกำเนินการ
		///</summary>
		public string FTCphMerTo { get; set; }

		///<summary>
		///ร้านค้า
		///</summary>
		public string FTCphShpTo { get; set; }

		///<summary>
		///1:Include 2:ยกเว้น
		///</summary>
		public string FTCphStaType { get; set; }


	}
}
