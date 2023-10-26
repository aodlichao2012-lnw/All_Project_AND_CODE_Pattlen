using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    class cmlTSysRcvAppTmp
    {
        /// <summary>
        ///รหัสระบบ
        /// <summary>
        public string FTAppCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNAppSeq { get; set; }

        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// <summary>
        public string FTFmtCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อประเภท
        /// <summary>
        public string FTAppName { get; set; }

        /// <summary>
        ///1: อนุญาต ให้ ทำรายการคืนได้  2 :ไม่อนุญาต ให้ ทำรายการคืน
        /// <summary>
        public string FTAppStaAlwRet { get; set; }

        /// <summary>
        ///1: อนุญาต ให้ ทำการยกเลิกรายการได้ 2 :ไม่อนุญาต ให้ ทำการยกเลิกรายการ
        /// <summary>
        public string FTAppStaAlwCancel { get; set; }

        /// <summary>
        ///1: อนุญาต ให้ มีรายการอื่น ต่อท้าย 2 :ไม่อนุญาต ให้ มีรายการอื่น ต่อท้าย
        /// <summary>
        public string FTAppStaPayLast { get; set; }
    }
}
