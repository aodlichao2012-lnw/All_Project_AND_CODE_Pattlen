using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.RabbitMQ
{
    public class cmlNotification
    {
        public string ptPOSCode { get; set; }
        public List<cmlNotiDetail> aoDetail { get; set; }

        public cmlNotification()
        {
            aoDetail = new List<cmlNotiDetail>();
        }
    }

    public class cmlNotiDetail
    {
        public string ptFTPdtCode { get; set; }
        public string pnFNLayRow { get; set; }
        public string pnFNLayCol { get; set; }
        public string pnFCStkQty { get; set; }
        public string pnFCPdtMin { get; set; }
    }
}
