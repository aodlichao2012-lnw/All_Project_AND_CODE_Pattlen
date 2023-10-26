using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Model
{
    public class cmlRabMQData
    {
        /// <summary>
        /// ชื่อ Function
        /// </summary>
        public string ptFunction { get; set; }
        /// <summary>
        /// ต้นทาง
        /// </summary>
        public string ptSource { get; set; }
        /// <summary>
        /// ปลายทาง
        /// </summary>
        public string ptDest { get; set; }
        /// <summary>
        /// ข้อมูล
        /// </summary>
        public cmlData ptData { get; set; }
    }
}
