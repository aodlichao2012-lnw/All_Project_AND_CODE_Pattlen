using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.RabbitMQ
{
    public class cmlCheckListSalDoc
    {
        public string ptFunction { get; set; }
        public string ptSource { get; set; }
        public string ptDest { get; set; }
        public string ptFilter { get; set; }
        public List<cmlListSalDoc> paData { get; set; }
    }
    public class cmlListSalDoc
    {
        public string ptFTBchCode { get; set; }
        public string ptFTShfCode { get; set; }
        public string ptFTPosCode { get; set; }
        public string ptFDXshDocDate { get; set; }
        public string ptFTBdhDocNo { get; set; }
    }
}
