using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Models
{
    public class cmlRequestTranferSAP
    {
        // ชื่อ Function
        public string ptFunction { get; set; }
        // ต้นทาง
        public string ptSource { get; set; }
        // ปลายทาง
        public string ptDest { get; set; }
        // ข้อมูล
        public cmlTranferSAPData ptData { get; set; }
    }
}
