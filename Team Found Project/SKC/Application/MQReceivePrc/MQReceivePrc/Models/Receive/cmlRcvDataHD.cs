using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cmlRcvDataHD
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
        /// ตัวกรอง
        /// </summary>
        public string ptFilter { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public object paData { get; set; }

        /// <summary>
        /// Connection String
        /// </summary>
        public string ptConnStr { get; set; }
    }
}
