using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.UserRole
{
    public class cmlResUserRoleDwn
    {
        public List<cmlResInfoUserRole> raUserRole { get; set; }
        public List<cmlResInfoUserRoleLng> raUserRoleLng { get; set; }
    }
}
