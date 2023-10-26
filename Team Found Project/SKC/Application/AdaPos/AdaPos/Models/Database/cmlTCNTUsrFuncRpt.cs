using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNTUsrFuncRpt
    {
        public string FTRolCode { get; set; }
        public string FTUfrType { get; set; }
        public string FTUfrGrpRef { get; set; }
        public string FTUfrRef { get; set; }
        public string FTUfrStaAlw { get; set; }
        public string FTUfrStaFavorite { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }

    }
}
