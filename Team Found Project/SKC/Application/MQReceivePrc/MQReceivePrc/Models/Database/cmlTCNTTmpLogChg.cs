using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTCNTTmpLogChg
    {
       public string FTLogCode { get; set; }
       public string FTLogDocNo { get; set; }
       public string FNLogType { get; set; }
       public string FTWahCode { get; set; }
       public string FTLogStaPrc { get; set; }
       public DateTime FDCreateOn { get; set; }
       public DateTime FDLastUpdOn { get; set; }
    }
}
