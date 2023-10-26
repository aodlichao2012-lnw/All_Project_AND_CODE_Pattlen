using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Models.Receive
{
    class cmlRcvNoti
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
        /// ตัวกรองข้อมูล
        /// </summary>
        public string ptFilter { get; set; }

        /// <summary>
        /// ข้อมูล
        /// </summary>
        public string ptData { get; set; }
    }

    class cmlRcvNotiFilter
    {
        /// <summary>
        /// Array Role Code
        /// </summary>
        public string ptRole { get; set; }

        /// <summary>
        /// user code
        /// </summary>
        public string ptUser { get; set; }
    }
    class cmlRcvNotiData
    {
        /// <summary>
        /// ชื่อข้อความ
        /// </summary>
        public string ptMsgGroup { get; set; }

        /// <summary>
        /// กลุ่มของข้อความ
        /// </summary>
        public string ptMsgName { get; set; }

        /// <summary>
        /// รายละเอียด จำนวนรายการ
        /// </summary>
        public string ptMsgDesc { get; set; }

        /// <summary>
        /// อ้างอิงเลขที่เอกสาร
        /// </summary>
        public string ptMsgRef { get; set; }

        /// <summary>
        /// วันที่ เวลา เอกสารโอน
        /// </summary>
        public Nullable<DateTime> pdMsgDate { get; set; }
    }
}
