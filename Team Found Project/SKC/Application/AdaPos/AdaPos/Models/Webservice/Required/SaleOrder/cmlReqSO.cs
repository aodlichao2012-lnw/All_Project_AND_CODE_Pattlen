using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required.SaleOrder
{
    public class cmlReqSO
    {

    }

    public class cmlReqOrdersByList
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสลูกค้า
        /// </summary>
        public string ptCstCode { get; set; }
    }

    public class cmlReqOrdersByDoc
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสาร SO
        /// </summary>
        public string ptDocNo { get; set; }
    }

}
