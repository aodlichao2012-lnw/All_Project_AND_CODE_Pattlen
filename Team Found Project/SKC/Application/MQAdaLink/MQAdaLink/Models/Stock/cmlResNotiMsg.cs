using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Models.Stock
{
    class cmlResNotiMsg
    {
        public string ptFunction { get; set; }
        public string ptSource { get; set; }
        public string ptDest { get; set; }      
        public cmlDataRole ptFilter { get; set; }
        public cmlDataNoti paData { get; set; }
    }
    public class cmlDataRole
    {
        public string ptRole { get; set; }
        public string ptUser { get; set; }
    }

    public class cmlDataNoti
    {
        public string ptMsgName { get; set; }
        public string ptMsgGroup { get; set; }
        public string ptMsgDesc { get; set; }
        public string ptMsgRef { get; set; }
        public string ptMsgDate { get; set; }
    }
}
