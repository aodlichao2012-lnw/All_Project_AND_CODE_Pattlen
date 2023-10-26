using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
    public class cmlPayItem
    {
        public int nSeq { get; set; }

        public string tRcvCode { get; set; }

        public string tRcvName { get; set; }

        public decimal cXrcUsrPayAmt { get; set; }

        public decimal cXrcNet { get; set; }

        public decimal cXrcChg { get; set; }
    }
}
