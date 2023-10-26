using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.UserRole
{
    public class cmlResInfoUserRole
    {
        public string rtRolCode { get; set; }
        public Nullable<long> rnRolLevel { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
