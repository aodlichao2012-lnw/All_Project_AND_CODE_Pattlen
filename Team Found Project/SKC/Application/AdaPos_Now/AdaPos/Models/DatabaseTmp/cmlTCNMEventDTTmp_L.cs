using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMEventDTTmp_L
    {
        /// <summary>
        ///รหัส
        /// <summary>
        public string FTEvhCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNEvdSeqNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อ
        /// <summary>
        public string FTEvdName { get; set; }
    }
}
