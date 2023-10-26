using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Card
{
    public class cmlResInfoCard
    {
        public string rtCrdCode { get; set; }
        public Nullable<DateTime> rdCrdStartDate { get; set; }
        public Nullable<DateTime> rdCrdExpireDate { get; set; }
        public Nullable<DateTime> rdCrdResetDate { get; set; }
        public Nullable<DateTime> rdCrdLastTopup { get; set; }
        public string rtCtyCode { get; set; }
        public Nullable<double> rcCrdValue { get; set; }
        public Nullable<double> rcCrdDeposit { get; set; }
        public string rtCrdHolderID { get; set; }
        public string rtCrdRefID { get; set; }
        public string rtCrdStaType { get; set; }
        public string rtCrdStaLocate { get; set; }
        public string rtCrdStaActive { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
