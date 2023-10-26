using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtHDBchTmp
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
        ///รหัสสาขา
        /// <summary>
        public string FTPmhBchTo { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// <summary>
        public string FTPmhMerTo { get; set; }

        /// <summary>
        ///ร้านค้า
        /// <summary>
        public string FTPmhShpTo { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// <summary>
        public string FTPmhStaType { get; set; }
    }
}
