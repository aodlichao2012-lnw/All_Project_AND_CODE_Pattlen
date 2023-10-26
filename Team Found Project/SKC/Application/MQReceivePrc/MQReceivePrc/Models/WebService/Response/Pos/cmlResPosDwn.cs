using MQReceivePrc.Models.WebService.Response.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Pos
{
    public class cmlResPosDwn
    {
        public List<cmlResInfoPos> raPos { get; set; }
        public List<cmlResInfoPosLng> raPosLng { get; set; }
        public List<cmlResInfoPosHW> raPosHW { get; set; }
        public List<cmlResInfoPosLastNo> raPosLastNo { get; set; }
        public List<cmlResInfoEdc> raEdc { get; set; }
        public List<cmlResInfoEdcLng> raEdcLng { get; set; }
        public List<cmlResInfoPrinter> raPrinter { get; set; }
        public List<cmlResInfoPrinterLng> raPrinterLng { get; set; }
    }
}
