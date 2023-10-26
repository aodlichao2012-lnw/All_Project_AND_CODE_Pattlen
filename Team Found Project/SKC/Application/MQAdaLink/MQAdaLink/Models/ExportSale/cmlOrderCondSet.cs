using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Model.ExportSale
{
    public class cmlOrderCondSet
    {
        /// <summary>
        /// KADS SO Document
        /// </summary>
        public string SODocNo { get; set; }

        /// <summary>
        /// Order Item (TPSTSalDTDis.FNXsdSeqNo)
        /// </summary>
        public string SalesSeqNo { get; set; }

        /// <summary>
        /// Discount Sequence  (SORT TPSTSalDTDis : ORDER BY FNXsdSeqNo, FDXddDateIns)
        /// </summary>
        public string SalesDiscSeqNo { get; set; }

        /// <summary>
        /// Condition Type (TPSTSalDTDis.FTXddRefCode)
        /// </summary>
        public string SalesCondType { get; set; }

        /// <summary>
        /// Discount Amount(Exc.VAT)  (TPSTSalDTDis.FCXddValue)
        /// </summary>
        public string SalesDiscAmt { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string SalesCurr { get; set; }

    }
}

