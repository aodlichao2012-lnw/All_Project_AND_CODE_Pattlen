using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMUserTmp
    {
        public string FTUsrCode { get; set; }
        public string FTDptCode { get; set; }
        public string FTRolCode { get; set; }
        public string FTUsrTel { get; set; }
        public string FTUsrPwd { get; set; }
        public string FTUsrEmail { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }

    }
}
