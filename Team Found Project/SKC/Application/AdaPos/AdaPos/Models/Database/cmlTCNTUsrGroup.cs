using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNTUsrGroup
    {
        public string FTUsrCode { get; set; }
        public string FTBchCode { get; set; }
        public string FTUsrStaShop { get; set; }
        public string FTShpCode { get; set; }
        public Nullable<DateTime> FDUsrStart { get; set; }
        public Nullable<DateTime> FDUsrStop { get; set; }
    }
}
