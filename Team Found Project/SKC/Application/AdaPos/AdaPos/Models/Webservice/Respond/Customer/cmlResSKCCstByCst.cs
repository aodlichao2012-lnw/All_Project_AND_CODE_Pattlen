using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Customer
{
    public class cmlResSKCCstByCst
    {
        /// <summary>
        /// Return code
        /// </summary>
        public string ErrorCode { get; set; }

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

        /// <summary>
        /// สิทธิประโยชน์ของลูกค้า
        /// </summary>
        public cmlPrivileges Privileges { get; set; }
        
    }

    public class cmlPrivileges
    {
        /// <summary>
        /// Flag บอกว่าได้ Privilege หรือไม่ 
        /// </summary>
        public bool flag { get; set; }

        /// <summary>
        /// ประเภท Privilege
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// จำนวนแต้ม
        /// </summary>
        public int Point { get; set; }
        
        public List<cmlQuotas> Quotas { get; set; }

    }

    public class cmlQuotas
    {
        /// <summary>
        /// รหัสสินค้า
        /// </summary>
        public string MatNo { get; set; }

        /// <summary>
        /// จำนวน quota
        /// </summary>
        public int Qty { get; set; }
    }
}
