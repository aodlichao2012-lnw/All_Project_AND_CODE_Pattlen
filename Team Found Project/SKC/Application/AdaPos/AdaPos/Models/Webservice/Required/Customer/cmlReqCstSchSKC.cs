using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required.Customer
{
    /// <summary>
    /// requst 3.1
    /// </summary>
    public class cmlReqCstSchSKC
    {
        /// <summary>
        /// เบอร์โทรศัพท์
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// หมาเลขบัตรประชาชน
        /// </summary>
        public string TaxID { get; set; }

        /// <summary>
        /// Sales Organization (Dealer code)
        /// </summary>
        public string SaleOrg { get; set; }
    }


    /// <summary>
    /// requst 3.2
    /// </summary>
    public class cmlReqChooseCst
    {
        /// <summary>
        /// KubotaID
        /// </summary>
        public string KubotaID { get; set; }

        /// <summary>
        /// Sales Organization (Dealer code)
        /// </summary>
        public string SaleOrg { get; set; }
    }
}
