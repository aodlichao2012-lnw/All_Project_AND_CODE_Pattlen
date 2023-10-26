using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMShopTmp
    {
        public string FTBchCode { get; set; }
        public string FTShpCode { get; set; }
        public string FTWahCode { get; set; }
        public string FTMerCode { get; set; }

        /// <summary>
        /// 1:แสดง 2; ไม่แสดง   (เฉพาะ ShpType=4,5)   default  1
        /// </summary>
        public string FTShpStaShwPrice { get; set; }   //*Arm 63-01-30

        public string FTShpType { get; set; }
        public string FTShpRegNo { get; set; }
        public string FTShpRefID { get; set; }
        public Nullable<DateTime> FDShpStart { get; set; }
        public Nullable<DateTime> FDShpStop { get; set; }
        public Nullable<DateTime> FDShpSaleStart { get; set; }
        public Nullable<DateTime> FDShpSaleStop { get; set; }
        public string FTShpStaActive { get; set; }
        public string FTShpStaClose { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }
        
    }
}
