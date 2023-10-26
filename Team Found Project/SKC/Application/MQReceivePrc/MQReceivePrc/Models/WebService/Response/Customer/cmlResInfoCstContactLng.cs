using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Customer
{
    public class cmlResInfoCstContactLng
    {
        public string rtCstCode { get; set; }
        public long rnLngID { get; set; }
        public long rnCtrSeq { get; set; }
        public string rtCtrName { get; set; }
        public string rtCtrFax { get; set; }
        public string rtCtrTel { get; set; }
        public string rtCtrEmail { get; set; }
        public string rtCtrRmk { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
