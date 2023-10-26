using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTPSTCouponHD_Tmp
    {
		///<summary>
		///
		///</summary>
		public string FTBchCode { get; set; }

		///<summary>
		///
		///</summary>
		public string FTCphDocNo { get; set; }

		///<summary>
		///
		///</summary>
		public string FTCptCode { get; set; }

		///<summary>
		///
		///</summary>
		public string FTCphDisType { get; set; }

		///<summary>
		///
		///</summary>
		public string FTPplCode { get; set; }

		///<summary>
		///
		///</summary>
		public string FTCpdBarCpn { get; set; }
		
		///<summary>
		///
		///</summary>
		public Int64? FNCpdSeqNo { get; set; }

		///<summary>
		///
		///</summary>
		public Int64? FNCpdAlwMaxUse { get; set; }

		///<summary>
		///
		///</summary>
		public string FTStaChkMember { get; set; }
        public string FTCptStaChkHQ { get; set; }

        ///<summary>
        ///
        ///</summary>
        public Int64? FNCphLimitUsePerBill { get; set; }

		///<summary>
		///
		///</summary>
		public decimal? FCCphDisValue { get; set; }

		///<summary>
		///
		///</summary>
		public decimal? FCCphMinValue { get; set; }

		///<summary>
		///
		///</summary>
		public string FTCphStaClosed { get; set; }

		///<summary>
		///
		///</summary>
		public string FTCphStaOnTopPmt { get; set; }

		///<summary>
		///
		///</summary>
		public string FTCphTimeStart { get; set; }

		///<summary>
		///
		///</summary>
		public string FTCphTimeStop { get; set; }

	}
}
