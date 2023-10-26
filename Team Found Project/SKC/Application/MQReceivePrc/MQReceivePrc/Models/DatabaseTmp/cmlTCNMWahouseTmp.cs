using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMWahouseTmp
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }       //*Arm 63-01-30

        public string FTWahCode { get; set; }
        public string FTWahStaType { get; set; }
        public string FTWahRefCode { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }

    }
}
