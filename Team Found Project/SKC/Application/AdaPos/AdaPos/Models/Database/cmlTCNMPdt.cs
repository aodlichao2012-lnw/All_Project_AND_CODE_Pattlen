using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNMPdt
    {
        public string FTPdtCode { get; set; }
        public string FTPdtStkControl { get; set; }
        public string FTPdtGrpControl { get; set; }
        public string FTPdtForSystem { get; set; }
        public Nullable<double> FCPdtQtyOrdBuy { get; set; }
        public Nullable<double> FCPdtCostAvg { get; set; }
        public Nullable<double> FCPdtCostFiFo { get; set; }
        public Nullable<double> FCPdtCostLast { get; set; }
        public Nullable<double> FCPdtCostDef { get; set; }
        public Nullable<double> FCPdtCostOth { get; set; }
        public Nullable<double> FCPdtCostAmt { get; set; }
        public Nullable<double> FCPdtCostStd { get; set; }
        public Nullable<double> FCPdtMin { get; set; }
        public Nullable<double> FCPdtMax { get; set; }
        public string FTPdtPoint { get; set; }
        public Nullable<double> FCPdtPointTime { get; set; }
        public string FTPdtType { get; set; }
        public string FTPdtSaleType { get; set; }
        public string FTPdtSetOrSN { get; set; }
        public string FTPdtStaSetPri { get; set; }
        public string FTPdtStaSetShwDT { get; set; }
        public string FTPdtStaAlwDis { get; set; }
        public string FTPdtStaAlwReturn { get; set; }
        public string FTPdtStaVatBuy { get; set; }
        public string FTPdtStaVat { get; set; }
        public string FTPdtStaActive { get; set; }
        public string FTPdtStaAlwReCalOpt { get; set; }
        public string FTPdtStaCsm { get; set; }
        public string FTShpCode { get; set; }
        public string FTPdtRefShop { get; set; }
        public string FTTcgCode { get; set; }
        public string FTPgpChain { get; set; }
        public string FTPtyCode { get; set; }
        public string FTPbnCode { get; set; }
        public string FTPmoCode { get; set; }
        public string FTVatCode { get; set; }
        public string FTEvnCode { get; set; }
        public Nullable<DateTime> FDPdtSaleStart { get; set; }
        public Nullable<DateTime> FDPdtSaleStop { get; set; }
        public string FTBchCode { get; set; }
        public Nullable<double> FCPdtStkFac { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }
        public long FNLngID { get; set; }
        public string FTPdtName { get; set; }
        public string FTPdtNameOth { get; set; }
        public string FTPdtNameABB { get; set; }
        public string FTPdtRmk { get; set; }
        public string FTBarCode { get; set; }
        public string FTPunName { get; set; }
        public Nullable<double> FCPdtUnitFact { get; set; }
        public Nullable<double> FCPgdPriceRet { get; set; }
    }
}
