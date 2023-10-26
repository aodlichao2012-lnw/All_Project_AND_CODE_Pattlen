using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Pos
{
    public class cmlResPosDwn
    {
        public List<cmlResInfoPos> raPos { get; set; }
        public List<cmlResInfoPosHW> raPosHW { get; set; }
        public List<cmlResInfoPosLastNo> raPosLastNo { get; set; }
        public List<cmlResInfoEdc> raEdc { get; set; }
        public List<cmlResInfoEdcLng> raEdcLng { get; set; }
        public List<cmlResInfoPrinter> raPrinter { get; set; }
        public List<cmlResInfoPrinterLng> raPrinterLng { get; set; }
        public List<cmlResInfoPosLng> raPosLng { get; set; }   //*Arm 63-04-08
    }
}
