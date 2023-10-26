using AdaPos.Models.Webservice.Respond.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.UserRole
{
    public class cmlResUserRoleDwn
    {
        public List<cmlResInfoUserRole> raUserRole { get; set; }
        public List<cmlResInfoUserRoleLng> raUserRoleLng { get; set; }
        public List<cmlResInfoTCNMUsrActRole> raUsrActRole { get; set; }
    }
}
