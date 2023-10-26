using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTFNTCrdTopUpHD
    {
        public string FTBchCode { get; set; }
        public string FTCthDocNo { get; set; }
        public string FTCthDocType { get; set; }
        public Nullable<DateTime> FDCthDocDate { get; set; }
        public string FTCthDocFunc { get; set; }
        public string FTPosCode { get; set; }
        public string FTUsrCode { get; set; }
        public string FTCthRmk { get; set; }
        public string FTUsrName { get; set; }
        public string FTCthStaDoc { get; set; }
        public string FTCthStaPrcDoc { get; set; }
        public string FTShfCode { get; set; }
        public Nullable<DateTime> FDShfSaleDate { get; set; }
        public int FNCthStaDocAct { get; set; }
        public string FTCthApvCode { get; set; }
        public string FTVatCode { get; set; }
        public Nullable<DateTime> FDCthApvDate { get; set; }
        public double FCCthAmtTP { get; set; }
        public double FCCthTotalTP { get; set; }
        public double FCCthTotalQty { get; set; }
        public string FTCthStaPrc { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }
        public string FTCthStaDelMQ { get; set; }
    }
}
