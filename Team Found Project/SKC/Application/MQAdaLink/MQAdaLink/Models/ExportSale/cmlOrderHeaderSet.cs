using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Model.ExportSale
{
    public class cmlOrderHeaderSet
    {
        /// <summary>
        /// Sales Organization
        /// </summary>
        public string SalesOrg { get; set; }

        /// <summary>
        /// KADS Order Type (Fix:ZPOS)
        /// </summary>
        public string SalesOrderType { get; set; }

        /// <summary>
        /// Plant (TCNMBranch.FTBchRefID)
        /// </summary>
        public string SalesPlantCode { get; set; }

        /// <summary>
        /// Sales Date
        /// </summary>
        public string SalesDocDate { get; set; }

        /// <summary>
        /// Customer Adviser
        /// </summary>
        public string SalesEmpCode { get; set; }

        /// <summary>
        /// Closed Sales Channel (TLKMWaHouse.FTWahStaChannel)
        /// </summary>
        public string SalesChannel { get; set; }

        /// <summary>
        /// Sold-to Party/CstCode (TPSTSalHD.FTCstCode)
        /// </summary>
        public string SalesCustCode { get; set; }

        /// <summary>
        /// KADS SO Document (ค่าว่าง)
        /// </summary>
        public string SODocNo { get; set; }

        /// <summary>
        /// POS Document No (TPSTSalHD.FTXshDocNo)
        /// </summary>
        public string POSDocNo { get; set; }

        /// <summary>
        /// Point Collect (TPSTSalHDCst.(FCXshCstPnt+FCXshCstPntPmt))
        /// </summary>
        public Nullable<int> SalesPointEarn { get; set; }

        /// <summary>
        /// Point Redeem (TPSTSalRD.SUM(FNXrdPntUse))
        /// </summary>
        public Nullable<int> SalesPointRedeem { get; set; }

        /// <summary>
        /// Sales Rounding (TPSTSalHD.FCXshRnd)
        /// </summary>
        public string SalesRounding { get; set; }

        /// <summary>
        /// Vehicle ID (TPSTSalHDCst.FTXshCtrName)
        /// </summary>
        public string SalesVIN { get; set; }

        /// <summary>
        /// Payment Method Cashdesk: Payment Method: Z1 - เงินสด, Z2 - เช็ค, Z3 - บัตรเครดิต, Z4 - โอนผ่านบัญชีธนาคาร, Z5 - จ่ายด้วย QR Code
        /// </summary>
        public string SalesRcvCode { get; set; }

        ///// <summary>
        ///// QR Transaction ID (TPSTSalRC.FTXrcRefNo1 Where FTFmtCode = '013')
        ///// </summary>
        //public string SalesQrTransId { get; set; }

        /// <summary>
        /// Remark (Header text) (TPSTSalHD.FCXsRmk)
        /// </summary>
        public string SalesRemark { get; set; }

        public List<cmlOrderItemSet> OrderItemSet { get; set; }

        public List<cmlOrderCondSet> OrderCondSet { get; set; }

        public List<cmlOrderMessageSet> OrderMessageSet { get; set; }

    }
}
