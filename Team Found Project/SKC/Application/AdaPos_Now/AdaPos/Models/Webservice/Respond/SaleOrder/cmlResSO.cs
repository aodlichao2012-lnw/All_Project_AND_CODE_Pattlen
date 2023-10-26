using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.SaleOrder
{
    public class cmlResSO
    {

    }

    public class cmlResOrdersByList
    {
        public List<cmlDataOrdersByList> raItems { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
    
    public class cmlDataOrdersByList
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string rtDocNo { get; set; }

        /// <summary>
        /// วันที่เอกสาร
        /// </summary>
        public Nullable<DateTime> rdDocDate { get; set; }

        /// <summary>
        /// ยอดรวม (FCXshGrand)
        /// </summary>
        public Nullable<decimal> rcGrand { get; set; }

        /// <summary>
        /// พนักงาน Key
        /// </summary>
        public string rtUsrKey { get; set; }

        /// <summary>
        /// ผู้อนุมัติ
        /// </summary>
        public string rtUsrApv { get; set; }

        /// <summary>
        /// สด/เครดิต 1:สด 2:credit
        /// </summary>
        public string rtCshOrCrd { get; set; }
    }

    public class cmlResOrdersByDoc
    {
        public cmlDataOrdersByDoc roItem { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }

    public class cmlDataOrdersByDoc
    {
        public List<cmlResInfoTARTSoHD> aoTARTSoHD { get; set; }
        public List<cmlResInfoTARTSoHDCst> aoTARTSoHDCst { get; set; }
        public List<cmlResInfoTARTSoHDDis> aoTARTSoHDDis { get; set; }
        public List<cmlResInfoTARTSoDT> aoTARTSoDT { get; set; }
        public List<cmlResInfoTARTSoDTDis> aoTARTSoDTDis { get; set; }
    }
}
