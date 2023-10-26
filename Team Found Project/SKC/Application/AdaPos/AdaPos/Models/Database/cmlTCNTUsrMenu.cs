using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNTUsrMenu
    {
        public string FTRolCode { get; set; }
        public string FTGmnCode { get; set; }
        public string FTMnuParent { get; set; }
        public string FTMnuCode { get; set; }
        public string FTAutStaFull { get; set; }
        public string FTAutStaRead { get; set; }
        public string FTAutStaAdd { get; set; }
        public string FTAutStaEdit { get; set; }
        public string FTAutStaDelete { get; set; }
        public string FTAutStaCancel { get; set; }
        public string FTAutStaAppv { get; set; }
        public string FTAutStaPrint { get; set; }
        public string FTAutStaPrintMore { get; set; }
        public string FTAutStaFavorite { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }

    }
}
