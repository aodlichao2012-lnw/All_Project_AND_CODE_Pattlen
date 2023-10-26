using API2Wallet.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Class.Public
{
    public class cApp
    {
        public static AdaFCEntities oC_AdaAccFC;
       // private AdaFCEntities oC_AdaAccFC;
        public DefaultConfig oC_DefaultConfig = new DefaultConfig();
        public class DefaultConfig
        {
            public int nDecPntMoney { get; set; }
            public int nMaxChangeNote { get; set; }
            public int nAutoCardPartialRefund { get; set; }
            public string tRptTAXNameStdA4 { get; set; }
            public string tRptTAXNameStdA5 { get; set; }
            public string tRptTAXDefault { get; set; }
            public string tVatType { get; set; }
            public double cVatRate { get; set; }
            public double cMaxBalance { get; set; }
            public double cMinBalRemainCrd { get; set; }
        }
    }
}