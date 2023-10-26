using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleVD
{
    public class cmlTVDTSal
    {
        public List<cmlTVDTSalHD> aoTVDTSalHD { get; set; }
        public List<cmlTVDTSalHDCst> aoTVDTSalHDCst { get; set; }
        public List<cmlTVDTSalHDDis> aoTVDTSalHDDis { get; set; }
        public List<cmlTVDTSalDT> aoTVDTSalDT { get; set; }
        public List<cmlTVDTSalDTDis> aoTVDTSalDTDis { get; set; }
        public List<cmlTVDTSalDTPmt> aoTVDTSalDTPmt { get; set; }
        public List<cmlTVDTSalDTVD> aoTVDTSalDTVD { get; set; }
        public List<cmlTVDTSalRC> aoTVDTSalRC { get; set; }
    }
}
