using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Customer
{
    public class cmlResSKCCstByList
    {
        /// <summary>
        /// Return code
        /// </summary>
        public string ErrorCode { get;set;}

        /// <summary>
        /// Object customer data
        /// </summary>
        public List<cmlDatas> Datas { get; set; }
    }
    public class cmlDatas
    {
        /// <summary>
        /// KubotaID
        /// </summary>
        public string KubotaID { get; set; }

        /// <summary>
        /// ชื่อ Customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// นามสกุล
        /// </summary>
        public string LastName { get; set; }
    }
}
