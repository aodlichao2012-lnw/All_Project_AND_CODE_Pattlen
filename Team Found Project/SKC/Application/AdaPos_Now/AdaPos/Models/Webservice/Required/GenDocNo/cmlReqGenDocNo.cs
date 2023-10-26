using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required.GenDocNo
{
    public class cmlReqGenDocNo
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// ประเภท
        /// </summary>
        public int pnSaleType { get; set; }
    }
}
