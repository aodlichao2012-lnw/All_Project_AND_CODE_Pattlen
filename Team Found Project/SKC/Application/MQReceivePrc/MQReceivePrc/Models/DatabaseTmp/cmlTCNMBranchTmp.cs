using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMBranchTmp
    {
        public string FTBchCode { get; set; }
        public string FTWahCode { get; set; }   //*Arm 63-03-27

        /// <summary>
        /// รหัสกลุ่มราคา
        /// </summary>
        public string FTPplCode { get; set; }  //*Arm 63-02-20

        public string FTBchType { get; set; }
        public string FTBchPriority { get; set; }
        public string FTBchRegNo { get; set; }
        public string FTBchRefID { get; set; }
        public Nullable<DateTime> FDBchStart { get; set; }
        public Nullable<DateTime> FDBchStop { get; set; }
        public Nullable<DateTime> FDBchSaleStart { get; set; }
        public Nullable<DateTime> FDBchSaleStop { get; set; }
        public string FTBchStaActive { get; set; }
        public string FTBchUriSrvMQ { get; set; }
        public string FTBchUriSrvSG { get; set; }
        public string FTBchStaHQ { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }

        /// <summary>
        /// ภาษา Default ของสาขา
        /// </summary>
        public Nullable<int> FNBchDefLang { get; set; }    //*Arm 63-03-27
    }
}
