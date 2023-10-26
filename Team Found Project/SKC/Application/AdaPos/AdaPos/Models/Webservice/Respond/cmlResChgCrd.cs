using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond
{
    public class cmlResChgCrd
    {
        public List<cmlResChgCrdInfo> raoChangeCard { get; set; }

        public string rtCode { get; set; }

        public string rtDesc { get; set; }
    }

    public class cmlResChgCrdInfo
    {
        public string rtFrmCrdCode { get; set; }

        public string rtToCrdCode { get; set; }

        public string rtBchCode { get; set; }

        public string rtStatus { get; set; }
    }
}
