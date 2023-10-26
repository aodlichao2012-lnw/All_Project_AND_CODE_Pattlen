using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTFNMEdcTmp
    {
        public string FTEdcCode { get; set; }
        public string FTSedCode { get; set; }
        public string FTBnkCode { get; set; }
        public string FTEdcShwFont { get; set; }
        public string FTEdcShwBkg { get; set; }
        public string FTEdcOther { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
