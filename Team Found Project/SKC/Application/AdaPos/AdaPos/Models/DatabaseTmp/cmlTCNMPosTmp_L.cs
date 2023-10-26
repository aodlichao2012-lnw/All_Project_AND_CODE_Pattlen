using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMPosTmp_L
    {
        /// <summary>
        ///
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง POS
        /// </summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อเครื่อง POS
        /// </summary>
        public string FTPosName { get; set; }

        /// <summary>
        ///ชื่อ POS เพิ่มเติม
        /// </summary>
        public string FTPosNameOth { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string FTPosRmk { get; set; }
    }
}
