using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTPSMFuncHDTmp
    {
        public string FTGhdCode { get; set; }
        public string FTGhdApp { get; set; }
        public string FTKbdScreen { get; set; }
        public string FTKbdGrpName { get; set; }
        public Nullable<int> FNGhdMaxPerPage { get; set; }
        public string FTGhdLayOut { get; set; }
        public Nullable<int> FNGhdMaxLayOutX { get; set; }
        public Nullable<int> FNGhdMaxLayOutY { get; set; }
        public string FTGhdStaAlwChg { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }
    }
}
