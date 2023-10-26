using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTFNMRcvSpcTmp
    {
        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// <summary>
        public string FTRcvCode { get; set; }

        /// <summary>
        ///รหัส APP
        /// <summary>
        public string FTAppCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<Int64> FNRcvSeq { get; set; }

        /// <summary>
        ///สาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของดำเนินการ
        /// <summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัสกลุ่ม agency
        /// <summary>
        public string FTAggCode { get; set; }

        /// <summary>
        ///หมายเหตุ (Refer external)
        /// <summary>
        public string FTPdtRmk { get; set; }

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
