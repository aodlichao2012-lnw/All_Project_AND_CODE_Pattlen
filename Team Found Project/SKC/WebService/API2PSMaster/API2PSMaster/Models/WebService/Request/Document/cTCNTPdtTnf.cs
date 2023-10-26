using API2PSMaster.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Document
{
    public class cTCNTPdtTnf
    {
        public TCNTPdtTnfHD oTCNTPdtTnfHD { get; set; }
        public IEnumerable<TCNTPdtTnfHDRef> aTCNTPdtTnfHDRef { get; set; }
        public IEnumerable<TCNTPdtTnfDT> aTCNTPdtTnfDT { get; set; }
        public IEnumerable<TCNTPdtTnfDTSrn> aTCNTPdtTnfDTSrn { get; set; }
    }
}