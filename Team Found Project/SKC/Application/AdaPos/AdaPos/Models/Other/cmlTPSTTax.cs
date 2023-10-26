using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
    public class cmlTPSTTax
    {
        public List<cmlTPSTTaxHD> aoTPSTTaxHD { get; set; }
        public List<cmlTPSTTaxHDDis> aoTPSTTaxHDDis { get; set; }
        public List<cmlTPSTTaxHDCst> aoTPSTTaxHDCst { get; set; }
        public List<cmlTPSTTaxDT> aoTPSTTaxDT { get; set; }
        public List<cmlTPSTTaxDTDis> aoTPSTTaxDTDis { get; set; }
        public List<cmlTPSTTaxDTPmt> aoTPSTTaxDTPmt { get; set; }
        public List<cmlTPSTTaxRC> aoTPSTTaxRC { get; set; }
        public List<cmlTCNMTaxAddress> aoTCNMTaxAddress { get; set; }   //*Arm 62-10-09  - Upload TaxAddress
    }
}
