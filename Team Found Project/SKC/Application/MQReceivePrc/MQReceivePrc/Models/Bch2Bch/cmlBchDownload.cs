using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Bch2Bch
{
    public class cmlBchDownload
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
        public string ptData { get; set; }      //*Arm 63-01-04

        /// <summary>
        /// Connection String Database for process.
        /// </summary>
        public string ptConnStr { get; set; }   //*Arm 63-01-04
    }
}
