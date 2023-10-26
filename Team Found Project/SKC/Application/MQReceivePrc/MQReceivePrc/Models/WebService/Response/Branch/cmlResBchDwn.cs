using System.Collections.Generic;

namespace MQReceivePrc.Models.Webservice.Response.Branch
{
    public class cmlResBchDwn
    {
        public List<cmlResInfoBch> raBch { get; set; }
        public List<cmlResInfoBchLng> raBchLng { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }
        public List<cmlResInfoImgObj> raImage { get; set; }
        public List<cmlResTCNTUrlObject> raTCNTUrlObject { get; set; }  //*Em 62-09-05
        public List<cmlResTCNTUrlObjectLogin> raTCNTUrlObjectLogin { get; set; }    //*Em 62-09-05
    }
}
