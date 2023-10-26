using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Shift
{
    public class cmlShiftSum
    {
        public int nCntOpDrw { get; set; }
        public int nCntCancelBill { get; set; }
        public int nCntHoldBill { get; set; }
        public int nMnyInCnt { get; set; }
        public decimal cMnyInAmt { get; set; }
        public int nMnyOutCnt { get; set; }
        public decimal cMnyOutAmt { get; set; }
        public int nSaleCnt { get; set; }
        public decimal cSaleAmt { get; set; }
        public int nRefundCnt { get; set; }
        public decimal cRefundAmt { get; set; }
        public decimal cRoundAmt { get; set; } //*Net 63-08-03 
    }
}
