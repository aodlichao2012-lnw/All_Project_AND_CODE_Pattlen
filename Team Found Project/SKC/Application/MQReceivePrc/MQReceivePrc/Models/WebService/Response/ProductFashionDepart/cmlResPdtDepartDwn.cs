using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductFashionDepart
{
    public class cmlResPdtDepartDwn
    {
        public List<cmlResInfoPdtDepart> raPdtDepart { get; set; }
        public List<cmlResInfoPdtDepartLng> raPdtDepartLng { get; set; }
    }
}
