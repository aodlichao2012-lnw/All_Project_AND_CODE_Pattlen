using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.KADS.CheckStock
{
    public class cmlResInfoResultStock
    {
        //public cmlResMetadata __metadata { get; set; }
        /// <summary>
        /// Bin Location
        /// </summary>
        public string BinLoc { get; set; }

        /// <summary>
        /// Material/ PdtCode
        /// </summary>
        public string MatNo { get; set; }

        /// <summary>
        /// FTPgpChain
        /// </summary>
        public string PicNum { get; set; }

        /// <summary>
        /// Plant/สาขาอ้างอิง
        /// </summary>
        public string PlantCode { get; set; }

        /// <summary>
        /// Storage/รหัสคลัง
        /// </summary>
        public string Sloc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SaleOrg { get; set; }

        /// <summary>
        /// จำนวนคงเหลือ
        /// </summary>
        public Nullable<decimal> MatQty { get; set; }

        /// <summary>
        /// หน่วย
        /// </summary>
        public string MatUnit { get; set; }
    }
}
