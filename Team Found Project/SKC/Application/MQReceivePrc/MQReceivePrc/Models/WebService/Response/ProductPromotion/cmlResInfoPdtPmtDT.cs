using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductPromotion
{
    public class cmlResInfoPdtPmtDT
    {
        //public string rtBchCode { get; set; }
        //public string rtPmhCode { get; set; }
        //public long rnPmdSeq { get; set; }
        //public string rtSpmCode { get; set; }
        //public string rtPmdGrpType { get; set; }
        //public string rtPmdGrpName { get; set; }
        //public string rtPdtCode { get; set; }
        //public string rtPunCode { get; set; }
        //public Nullable<double> rcPmdSetPriceOrg { get; set; }
        //public string rtPmdGrpCode { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }

        
            
        //*Arm 63-03-26  

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string rtPmhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> rnPmdSeq { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        /// </summary>
        public string rtPmdStaType { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string rtPmdGrpName { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string rtPmdRefCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// </summary>
        public string rtPmdSubRef { get; set; }

        /// <summary>
        ///รหัสบาร์โค้ด ณ. บันทึก
        /// </summary>
        public string rtPmdBarCode { get; set; }

        //+++++++++++++
    }
}
