using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.User
{
    public class cmlResInfoUserGrp
    {
        public string rtUsrCode { get; set; }
        public string rtBchCode { get; set; }
        public string rtUsrStaShop { get; set; }
        public string rtShpCode { get; set; }
        public Nullable<DateTime> rdUsrStart { get; set; }
        public Nullable<DateTime> rdUsrStop { get; set; }
    }
}
