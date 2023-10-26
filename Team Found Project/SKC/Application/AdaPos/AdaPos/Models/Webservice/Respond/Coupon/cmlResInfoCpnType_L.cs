using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Coupon
{
    public class cmlResInfoCpnType_L
    {
        /// <summary>
        ///รหัสประเภทคูปอง
        /// </summary>
        public string rtCptCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public int rnLngID { get; set; }

        /// <summary>
        ///ชื่อประเภท
        /// </summary>
        public string rtCptName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtCptRemark { get; set; }



        //*Net 63-03-05 Rearrange by script
        /*
        public string rtCptCode { get; set; }
        public int rnLngID { get; set; }
        public string rtCptName { get; set; }
        public string rtCptRemark { get; set; }*/
    }
}
