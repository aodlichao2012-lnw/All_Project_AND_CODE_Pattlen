using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.BankDepositSlip
{
    public class cmlDataBnkDpl
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptFTBchCode { get; set; }
        /// <summary>
        /// เลขที่เอกสารใบนำฝาก
        /// </summary>
        public string ptFTBdhDocNo { get; set; }
    }
}
