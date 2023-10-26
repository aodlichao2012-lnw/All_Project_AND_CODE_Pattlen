using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    class cmlTFNTCouponHDCstPriTmp
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
        ///รหัสกลุ่มราคา
        ///</summary>
        public string FTPplCode { get; set; }

        ///<summary>
        ///1:Include 2:ยกเว้น
        ///</summary>
        public string FTCphStaType { get; set; }


    }
}
