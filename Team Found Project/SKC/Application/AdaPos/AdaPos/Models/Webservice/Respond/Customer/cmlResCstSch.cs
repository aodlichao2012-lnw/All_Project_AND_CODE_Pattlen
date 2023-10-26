using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Customer
{
    public class cmlResCst
    {
        public List<cmlResCstSch> raItems { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
    public class cmlResCstSch
    {
        /// <summary>
        /// Customer code.
        /// </summary>
        public string rtCstCode { get; set; }

        /// <summary>
        /// Customer name.
        /// </summary>
        public string rtCstName { get; set; }

        /// <summary>
        /// Customer name other.
        /// </summary>
        public string rtCstNameOth { get; set; }

        /// <summary>
        /// Customer telephone.
        /// </summary>
        public string rtCstTel { get; set; }

        /// <summary>
        /// Customer email.
        /// </summary>
        public string rtCstEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstCardID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstSex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> rdCstDob { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtPplCodeRet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstDiscRet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstCrdNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstTaxNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> rdCstCrdExpire { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<decimal> rtTxnPntQty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<decimal> rtTxnBuyTotal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstStaAlwPosCalSo { get; set; }
    }
}
