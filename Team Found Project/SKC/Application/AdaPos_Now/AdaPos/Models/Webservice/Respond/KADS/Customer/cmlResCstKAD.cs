using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.KADS.Customer
{
    public class Metadata
    {
        public string id { get; set; }
        public string uri { get; set; }
        public string type { get; set; }
    }

    public class cmlResPrivi
    {
        //public Metadata __metadata { get; set; }
        public string KubotaId { get; set; }
        public string MatNo { get; set; }
        public string QtyPrt { get; set; }
        public string QtyUse { get; set; }
        public string QtyBal { get; set; }
        public string MatUnit { get; set; }
     }

    public class cmlPrivilege
{
        public IList<cmlResPrivi> results { get; set; }
    }

    public class cmlResKunnr
{
        public Metadata __metadata { get; set; }
        public string CustomerCode { get; set; }
        public string PhoneSearch { get; set; }
        public string TaxID { get; set; }
        public string KubotaID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Membership { get; set; }
        public int Point { get; set; }
        public cmlPrivilege PrivilegePointSet { get; set; }

        /// <summary>
        /// Business Grouping (ZAR1 = AD's Customer, ZAR6 = KubotaID)
        /// </summary>
        public string BUGroup { get; set; }

        /// <summary>
        /// คำนำหน้าชื่อ (EN)
        /// </summary>
        public string Titlee { get; set; }

        /// <summary>
        /// First Name (EN)
        /// </summary>
        public string Namee { get; set; }

        /// <summary>
        /// Last Name (EN)
        /// </summary>
        public string Surnamee { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Addr { get; set; }

        /// <summary>
        /// ซอย
        /// </summary>
        public string Soi { get; set; }

        /// <summary>
        /// ถนน
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// แขวง/ตำบล
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// เขต/อำเภอ
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// จังหวัด
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// วันเกิด
        /// </summary>
        public string Birth { get; set; }

        /// <summary>
        /// อีเมลล์
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// เบอรโทร
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string Remark { get; set; }

    }

    public class D
    {
        public IList<cmlResKunnr> results { get; set; }
    }

    public class cmlResCstKAD
    {
        public D d { get; set; }
    }

    public class cmlResCreateCustomerCode
    {
        public cmlResKunnr d { get; set; }
    }

}
