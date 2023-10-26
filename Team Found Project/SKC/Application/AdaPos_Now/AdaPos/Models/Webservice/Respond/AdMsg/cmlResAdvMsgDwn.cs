using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.AdMsg
{
    public class cmlResAdvMsgDwn
    {
        public List<cmlResInfoAdvMsg> raAdvMsg { get; set; }
        public List<cmlResInfoAdvMsgLng> raAdvMsgLng { get; set; }
        public List<cmlTCNMMediaObj> raTCNMMediaObj { get; set; }   //*Em 62-09-15
        public List<cmlResInfoImgObj> raTCNMImgObj { get; set; }    //*Em 62-09-15
    }
}
