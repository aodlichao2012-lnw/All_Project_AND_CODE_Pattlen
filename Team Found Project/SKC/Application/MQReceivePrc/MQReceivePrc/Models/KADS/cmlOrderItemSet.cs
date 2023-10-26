using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.KADS
{
    public class cmlOrderItemSet
    {
        /// <summary>
        /// KADS SO Document
        /// </summary>
        public string SODocNo { get; set; }

        /// <summary>
        /// Order Item (TPSTSalDT.FNXsdSeqNo)
        /// </summary>
        public string SalesSeqNo { get; set; }

        /// <summary>
        /// Material(TPSTSalDT.FTPdtCode)
        /// </summary>
        public string SalesMatNo { get; set; }

        /// <summary>
        /// Order Quantity
        /// </summary>
        public Nullable<decimal> SalesQtyAll { get; set; }

        /// <summary>
        /// Target Quantity UoM (TPSTSalDT.FTPunCode)
        /// </summary>
        public string SalesUnit { get; set; }

        /// <summary>
        /// Storage location (TLKMWaHouse.FTWahRefNo)
        /// </summary>
        public string SalesSloc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<cmlOrderCondSet> OrderCondSet { get; set; }

    }
}
