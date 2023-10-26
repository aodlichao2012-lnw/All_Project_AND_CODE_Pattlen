using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlEmail
    {
        public string tEmailCred { get; set; }
        public string tPwdCred { get; set; }
        public string tEmailFrom { get; set; }
        public string tEmailTo { get; set; }
        public int nPort { get; set; }
        public string tSMTPHost { get; set; }
        public int nTimeout { get; set; }
        public string tSubject { get; set; }
        public string tBody { get; set; }
    }
}
