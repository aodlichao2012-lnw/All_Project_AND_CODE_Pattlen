using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMWahouseTmp_L
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }       //*Arm 63-01-30

        public string FTWahCode { get; set; }
        public int FNLngID { get; set; }
        public string FTWahName { get; set; }
        public string FTWahRmk { get; set; }
    }
}
