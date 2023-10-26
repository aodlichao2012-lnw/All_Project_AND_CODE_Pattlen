using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Courier
{
    public class cmlResCourierItemDwn
    {
        public List<cmlResCourier> raTCNMCourier { get; set; }
        public List<cmlResCourier_L> raTCNMCourier_L { get; set; }
        public List<cmlResCourieMan> raTCNMCourieMan { get; set; }
        public List<cmlResCourieMan_L> raTCNMCourieMan_L { get; set; }
        public List<cmlResCourierType> raTCNMCourierType { get; set; }
        public List<cmlResCourierType_L> raTCNMCourierType_L { get; set; }
        public List<cmlResCourierGrp> raTCNMCourierGrp { get; set; }
        public List<cmlResCourierGrp_L> raTCNMCourierGrp_L { get; set; }
        public List<cmlResCourieLogin> raTCNMCourieLogin { get; set; }
    }
}
