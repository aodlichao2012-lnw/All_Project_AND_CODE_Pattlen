using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Models
{
    public class cmlSQLConfig
    {
        public string tServer { get; set; }
        public string tUsername { get; set; }
        public string tPassword { get; set; }
        public string tPort { get; set; }
        public string tDatabase { get; set; }
        public string tAuthenMode { get; set; }
        public int nConnectTimeOut { get; set; }
        public int nCommandTimeOut { get; set; }
    }
}
