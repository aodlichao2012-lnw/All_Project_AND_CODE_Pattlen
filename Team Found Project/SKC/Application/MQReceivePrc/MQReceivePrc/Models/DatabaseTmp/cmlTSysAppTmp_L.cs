using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTSysAppTmp_L
    {
        /// <summary>
        ///รหัสแอปพลิเคชั่น
        /// <summary>
        public string FTAppCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อแอปพลิเคชั่น
        /// <summary>
        public string FTAppName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTAppRmk { get; set; }
    }
}
