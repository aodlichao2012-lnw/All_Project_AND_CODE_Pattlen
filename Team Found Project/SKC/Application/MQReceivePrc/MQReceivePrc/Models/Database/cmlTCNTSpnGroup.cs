using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTCNTSpnGroup
    {
        public string FTSpnCode { get; set; }
        public string FTBchCode { get; set; }
        public string FTSpnStaShop { get; set; }
        public string FTShpCode { get; set; }
        public Nullable<DateTime> FDSpnStart { get; set; }
        public Nullable<DateTime> FDSpnStop { get; set; }
    }
}
