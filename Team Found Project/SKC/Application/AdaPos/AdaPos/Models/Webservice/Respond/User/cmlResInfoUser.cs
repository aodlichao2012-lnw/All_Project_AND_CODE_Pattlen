using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.User
{
    public class cmlResInfoUser
    {
        public string rtUsrCode { get; set; }
        public string rtDptCode { get; set; }
        public string rtRolCode { get; set; }
        public string rtUsrTel { get; set; }
        public string rtUsrPwd { get; set; }
        public string rtUsrEmail { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
