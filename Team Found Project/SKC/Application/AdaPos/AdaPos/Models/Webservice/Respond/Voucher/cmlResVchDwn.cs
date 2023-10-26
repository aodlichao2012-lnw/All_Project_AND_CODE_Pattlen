using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Voucher
{
    public class cmlResVchDwn
    {
        public List<cmlResInfoVch> raVch { get; set; }
        public List<cmlResInfoVchLng> raVchLng { get; set; }
        public List<cmlResInfoVchType> raVchType { get; set; }
        public List<cmlResInfoVchTypeLng> raVchTypeLng { get; set; }
    }
}
