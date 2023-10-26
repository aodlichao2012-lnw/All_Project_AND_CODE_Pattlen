using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required.KADS.Customer
{
    public class cmlReqCustomerCreate
    {
        /// <summary>
        /// CustomerCode
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// คำนำหน้าชื่อ (EN)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// First Name (EN)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name (TH)
        /// </summary>
        public string LastName { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public string PhoneNo { get; set; }         //*Arm 63-08-20

        /// <summary>
        /// 
        /// </summary>
        public string PhoneSearch { get; set; }     //*Arm 63-08-20

        /// <summary>
        /// 
        /// </summary>
        public string Membership { get; set; }      //*Arm 63-08-20

        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> Point { get; set; }      //*Arm 63-08-31

        /// <summary>
        /// 
        /// </summary>
        public string BUGroup { get; set; }      //*Arm 63-08-31

        /// <summary>
        /// 
        /// </summary>
        public string KubotaID { get; set; }      //*Arm 63-08-31

        /// <summary>
        /// 
        /// </summary>
        public string TaxID { get; set; }      //*Arm 63-08-31
        
    }
}
