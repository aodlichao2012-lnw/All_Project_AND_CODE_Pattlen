using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Company
{
    public class cmlResCompDwn
    {
        public List<cmlResInfoComp> raComp { get; set; }
        public List<cmlResInfoCompLng> raCompLng { get; set; }
        public List<cmlResInfoImgObj> raImage { get; set; }
    }
}
