using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Doc.PdtTnfBch
{
    public class cmlTCNTPdtTbxHDRef
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string FTXthDocNo { get; set; }

        /// <summary>
        ///ชื่อผู้ตืดต่อ
        /// <summary>
        public string FTXthCtrName { get; set; }

        /// <summary>
        ///วันที่ส่งของ
        /// <summary>
        public Nullable<DateTime> FDXthTnfDate { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่ ใบขนส่ง
        /// <summary>
        public string FTXthRefTnfID { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่ ยานพาหนะ ขนส่ง
        /// <summary>
        public string FTXthRefVehID { get; set; }

        /// <summary>
        ///จำนวนและลักษณะหีบห่อ
        /// <summary>
        public string FTXthQtyAndTypeUnit { get; set; }

        /// <summary>
        ///อ้างอิง ที่อยู่ ส่งของ null หรือ 0 ไม่กำหนด
        /// <summary>
        public Nullable<Int64> FNXthShipAdd { get; set; }

        /// <summary>
        ///รหัสการขนส่ง
        /// <summary>
        public string FTViaCode { get; set; }
    }
}
