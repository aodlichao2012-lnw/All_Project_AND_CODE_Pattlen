using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNMVoucherTmp
    {
        public string FTVocCode { get; set; }
        public string FTVocBarCode { get; set; }
        public Nullable<DateTime> FDVocExpired { get; set; }
        public string FTVotCode { get; set; }
        public Nullable<double> FCVocValue { get; set; }
        public Nullable<double> FCVocSalePri { get; set; }
        public Nullable<double> FCVocBalance { get; set; }
        public string FTVocComBook { get; set; }
        public string FTVocStaBook { get; set; }
        public string FTVocStaSale { get; set; }
        public string FTVocStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
