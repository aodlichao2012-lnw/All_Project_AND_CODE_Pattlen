using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Pos
{
    public class cmlResInfoPosLng
    {
        /// <summary>
        ///รหัสเครื่อง POS
        /// </summary>
        public string rtPosCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อเครื่อง POS
        /// </summary>
        public string rtPosName { get; set; }

        /// <summary>
        ///ชื่อ POS เพิ่มเติม
        /// </summary>
        public string rtPosNameOth { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtPosRmk { get; set; }
    }
}
