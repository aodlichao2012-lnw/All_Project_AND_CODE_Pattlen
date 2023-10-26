using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Branch
{
    public class cmlResInfoBch
    {
        public string rtBchCode { get; set; }
        public string rtWahCode { get; set; }   //*Arm 63-03-27
        public string rtPplCode { get; set; }
        public string rtBchType { get; set; }
        public string rtBchPriority { get; set; }
        public string rtBchRegNo { get; set; }
        public string rtBchRefID { get; set; }
        public Nullable<DateTime> rdBchStart { get; set; }
        public Nullable<DateTime> rdBchStop { get; set; }
        public Nullable<DateTime> rdBchSaleStart { get; set; }
        public Nullable<DateTime> rdBchSaleStop { get; set; }
        public string rtBchStaActive { get; set; }
        public string rtBchUriSrvMQ { get; set; }
        public string rtBchUriSrvSG { get; set; }
        public string rtBchStaHQ { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }

        /// <summary>
        /// ภาษา Default ของสาขา
        /// </summary>
        public Nullable<int> rnBchDefLang { get; set; }    //*Arm 63-03-27
    }
}
