using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtDTTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// <summary>
        public string FTPmhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<Int64> FNPmdSeq { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        /// <summary>
        public string FTPmdStaType { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// <summary>
        public string FTPmdGrpName { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string FTPmdRefCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// <summary>
        public string FTPmdSubRef { get; set; }

        /// <summary>
        ///รหัสบาร์โค้ด ณ. บันทึก
        /// <summary>
        public string FTPmdBarCode { get; set; }

    }
}
