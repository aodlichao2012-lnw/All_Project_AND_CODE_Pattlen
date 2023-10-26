using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Models
{
    class cmlReqSendMail
    {
        public string ptFunction { get; set; }
        // ต้นทาง
        public string ptSource { get; set; }
        // ปลายทาง
        public string ptDest { get; set; }
    }
}
