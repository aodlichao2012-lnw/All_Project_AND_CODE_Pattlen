using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Model
{
    public class cmlData
    {
        /// <summary>
        /// รหัสสาขาตัวเอง
        /// </summary>
        public string ptFilter { get; set; }
        /// <summary>
        /// จากวันที่
        /// </summary>
        public string ptDateFrm { get; set; }
        /// <summary>
        /// ถึงวันที่
        /// </summary>
        public string ptDateTo { get; set; }
    }
}
