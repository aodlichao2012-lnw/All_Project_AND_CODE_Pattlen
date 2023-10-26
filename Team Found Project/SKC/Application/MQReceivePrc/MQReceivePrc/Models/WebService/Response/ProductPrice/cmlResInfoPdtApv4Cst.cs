using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductPrice
{
    public class cmlResInfoPdtApv4Cst
    {
        public string rtPplCode { get; set; }
        public string rtPdtCode { get; set; }
        public string rtPunCode { get; set; }
        public DateTime rdPghDStart { get; set; }
        public string rtPghTStart { get; set; }
        public Nullable<DateTime> rdPghDStop { get; set; }
        public string rtPghTStop { get; set; }
        public string rtPghDocNo { get; set; }
        public string rtPghDocType { get; set; }
        public string rtPghStaAdj { get; set; }
        public Nullable<double> rcPgdPriceRet { get; set; }
        public Nullable<double> rcPgdPriceWhs { get; set; }
        public Nullable<double> rcPgdPriceNet { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
    }
}
