using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopPosLayoutTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง Pos/ตู้   [Refer Address]
        /// <summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///ลำดับช่อง
        /// <summary>
        public Nullable<Int64> FNLayNo { get; set; }

        /// <summary>
        ///Board No  เช่น 0-9
        /// <summary>
        public Nullable<int> FNLayBoardNo { get; set; }

        /// <summary>
        ///Box No  เช่น 0-F
        /// <summary>
        public string FTLayBoxNo { get; set; }
    }
}
