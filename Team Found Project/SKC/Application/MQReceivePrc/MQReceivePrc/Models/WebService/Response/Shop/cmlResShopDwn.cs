using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Shop
{
    public class cmlResShopDwn
    {
        public List<cmlResInfoShop> raShop { get; set; }
        public List<cmlResInfoShopLng> raShopLng { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }
        public List<cmlResInfoImgObj> raImage { get; set; }
    }
}
