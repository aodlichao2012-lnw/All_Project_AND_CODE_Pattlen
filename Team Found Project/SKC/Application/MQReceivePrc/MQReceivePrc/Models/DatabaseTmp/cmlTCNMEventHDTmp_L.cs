using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMEventHDTmp_L
    {
        /// <summary>
        ///รหัส
        /// <summary>
        public string FTEvhCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อรอบ
        /// <summary>
        public string FTEvhName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTEvhRmk { get; set; }
    }
}
