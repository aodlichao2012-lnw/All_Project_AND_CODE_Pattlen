using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Coupon
{
    public class cmlTFNTCouponDTHis
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
		///คูปองบาร์ (หมายเลขคูปอง)
		///</summary>
		public string FTCpdBarCpn { get; set; }

		///<summary>
		///(AUTONUMBER)ลำดับ
		///</summary>
		public Int64 FNCpbSeqID { get; set; }

		///<summary>
		///วันเวลาที่เริ่ม
		///</summary>
		public DateTime? FDCpbFrmStart { get; set; }

		///<summary>
		///จากสาขา
		///</summary>
		public string FTCpbFrmBch { get; set; }

		///<summary>
		///จากเครื่องจุดขาย
		///</summary>
		public string FTCpbFrmPos { get; set; }

		///<summary>
		///เลขที่เอกสารอ้างอิง
		///</summary>
		public string FTCpbFrmSalRef { get; set; }

		///<summary>
		///สถานะการจอง 1:สมบูรณ์ 2:จอง 3:ยกเลิก
		///</summary>
		public string FTCpbStaBook { get; set; }

		///<summary>
		///วันที่ปรับปรุงรายการล่าสุด
		///</summary>
		public DateTime? FDLastUpdOn { get; set; }

		///<summary>
		///ผู้ปรับปรุงรายการล่าสุด
		///</summary>
		public string FTLastUpdBy { get; set; }

		///<summary>
		///วันที่สร้างรายการ
		///</summary>
		public DateTime? FDCreateOn { get; set; }

		///<summary>
		///ผู้สร้างรายการ
		///</summary>
		public string FTCreateBy { get; set; }


	}
}
