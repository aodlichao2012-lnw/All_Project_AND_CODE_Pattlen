using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopPosLayout
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง Pos/ตู้   [Refer Address]
        /// <summary>
        public string rtPosCode { get; set; }

        /// <summary>
        ///ลำดับช่อง
        /// <summary>
        public Nullable<Int64> rnLayNo { get; set; }

        /// <summary>
        ///Board No  เช่น 0-9
        /// <summary>
        public Nullable<int> rnLayBoardNo { get; set; }

        /// <summary>
        ///Box No  เช่น 0-F
        /// <summary>
        public string rtLayBoxNo { get; set; }
    }
}
