using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMPdtPackSizeTmp
    {
        public string FTPdtCode { get; set; }
        public string FTPunCode { get; set; }
        public Nullable<decimal> FCPdtUnitFact { get; set; }
        public Nullable<decimal> FCPdtPriceRET { get; set; }
        public Nullable<decimal> FCPdtPriceWHS { get; set; }
        public Nullable<decimal> FCPdtPriceNET { get; set; }
        public string FTPdtGrade { get; set; }
        public Nullable<decimal> FCPdtWeight { get; set; }
        public string FTClrCode { get; set; }
        public string FTPszCode { get; set; }
        public string FTPdtUnitDim { get; set; }
        public string FTPdtPkgDim { get; set; }
        public string FTPdtStaAlwPick { get; set; }
        public string FTPdtStaAlwPoHQ { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }

    }
}
