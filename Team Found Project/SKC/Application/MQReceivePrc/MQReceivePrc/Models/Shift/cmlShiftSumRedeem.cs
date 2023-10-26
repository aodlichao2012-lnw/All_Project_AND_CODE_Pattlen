using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Shift
{
    public class cmlShiftSumRedeem
    {
        public string FTXshCardNo { get; set; }
        public int FNXrdPntUse { get; set; }
        public decimal FCXhdAmt { get; set; }
        public string FTXsdBarCode { get; set; }  //*Arm 63-05-28
    }
}
