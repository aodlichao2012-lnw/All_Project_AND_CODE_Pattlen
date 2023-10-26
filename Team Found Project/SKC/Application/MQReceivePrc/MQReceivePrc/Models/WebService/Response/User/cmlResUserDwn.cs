using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.User
{
    public class cmlResUserDwn
    {
        public List<cmlResInfoUser> raUser { get; set; }
        public List<cmlResInfoUserLng> raUserLng { get; set; }
        public List<cmlResInfoImgPerson> raImage { get; set; }
        public List<cmlResInfoUserGrp> raUserGrp { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }
        public List<cmlResTCNMUsrLogin> raTCNMUsrLogin { get; set; }   //*Em 62-09-02
    }
}
