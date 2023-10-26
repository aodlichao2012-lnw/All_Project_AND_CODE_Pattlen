using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTPSMFunc
    {
        // TPSMFuncHD
        public string FTGhdCode { get; set; }
        public string FTGhdApp { get; set; }
        public Nullable<int> FNGhdMaxPerPage { get; set; }
        public string FTGhdLayOut { get; set; }
        public Nullable<int> FNGhdMaxLayOutX { get; set; }
        public Nullable<int> FNGhdMaxLayOutY { get; set; }
        public string FTGhdStaAlwChg { get; set; }
        public string FTKbdScreen { get; set; }
        public string FTKbdGrpName { get; set; }

        // TPSMFuncDT
        public string FTSysCode { get; set; }
        public Nullable<int> FNGdtPage { get; set; }
        public Nullable<int> FNGdtDefSeq { get; set; }
        public Nullable<int> FNGdtUsrSeq { get; set; }
        public Nullable<int> FNGdtBtnSizeX { get; set; }
        public Nullable<int> FNGdtBtnSizeY { get; set; }
        public string FTGdtCallByName { get; set; }
        public string FTGdtStaUse { get; set; }

        // TPSMFuncDT_L
        public Nullable<int> FNLngID { get; set; }
        public string FTGdtName { get; set; }

        // TSysFuncKB
        public Nullable<int> FNSysKeyShift { get; set; }
        public Nullable<int> FNSysKeyAscii { get; set; }
        public string FTSysKeyName { get; set; }
        public string FTSysKeyFunc { get; set; }
        public string FTSysStaUse { get; set; }

    }
}
