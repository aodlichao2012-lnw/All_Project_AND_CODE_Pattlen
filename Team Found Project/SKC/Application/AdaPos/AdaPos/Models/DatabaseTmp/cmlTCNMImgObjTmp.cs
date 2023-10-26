using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMImgObjTmp
    {
        public long FNImgID { get; set; }
        public string FTImgRefID { get; set; }
        public int FNImgSeq { get; set; }
        public string FTImgTable { get; set; }
        public string FTImgKey { get; set; }
        public string FTImgObj { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }
    }
}
