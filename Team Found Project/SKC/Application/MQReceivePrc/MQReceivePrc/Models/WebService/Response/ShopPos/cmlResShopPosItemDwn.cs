using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopPosItemDwn
    {
        public List<cmlResShopPosLayout> raTRTMShopPosLayout { get; set; }
        public List<cmlResShopPos> raTRTMShopPos { get; set; }
        public List<cmlResShopType> raTRTMShopType { get; set; }
        public List<cmlResShopType_L> raTRTMShopType_L { get; set; }
        public List<cmlResShopLayout> raTRTMShopLayout { get; set; }
        public List<cmlResShopLayout_L> raTRTMShopLayout_L { get; set; }
        public List<cmlResShopRack> raTRTMShopRack { get; set; }
        public List<cmlResShopRack_L> raTRTMShopRack_L { get; set; }
        public List<cmlResShopSize> raTRTMShopSize { get; set; }
        public List<cmlResShopSize_L> raTRTMShopSize_L { get; set; }
    }
}
