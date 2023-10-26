using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Models
{
    public class cmlTranferSAPData
    {
        // รหัสสาขาตัวเอง
        public string ptFilter { get; set; }
        // จากวันที่ๆต้องการค้นหา
        public string ptDateFrm { get; set; }
        // ถึงวันที่ๆต้องการค้นหา
        public string ptDateTo { get; set; }
    }
}
