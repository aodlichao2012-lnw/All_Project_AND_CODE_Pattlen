using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond
{
    public class cmlResCardHistory
    {
        public List<cmlResCardHistoryList> roCrdHistory { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
}
