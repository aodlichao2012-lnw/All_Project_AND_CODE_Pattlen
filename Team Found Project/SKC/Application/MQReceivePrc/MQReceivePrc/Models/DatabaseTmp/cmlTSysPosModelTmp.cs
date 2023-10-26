using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTSysPosModelTmp
    {
        /// <summary>
        ///รหัส
        /// </summary>
        public string FTSpmCode { get; set; }

        /// <summary>
        ///ยี่ห้อ ผู้ผลิต
        /// </summary>
        public string FTSpmBrand { get; set; }

        /// <summary>
        ///ชื่อ
        /// </summary>
        public string FTSpmName { get; set; }

        /// <summary>
        ///ชื่ออื่น
        /// </summary>
        public string FTSpmNameEng { get; set; }

        /// <summary>
        ///ระบบ
        /// </summary>
        public string FTSpmSystem { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string FTSpmRemark { get; set; }
    }
}
