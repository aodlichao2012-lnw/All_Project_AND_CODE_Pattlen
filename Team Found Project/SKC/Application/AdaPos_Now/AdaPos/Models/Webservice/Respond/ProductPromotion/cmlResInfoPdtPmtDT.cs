using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductPromotion
{
    public class cmlResInfoPdtPmtDT
    {

        //*Arm 63-03-26 ปรับแก้ Standard 

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


        ///// <summary>
        /////รหัสสาขา
        ///// </summary>
        //public string rtFTBchCode { get; set; }

        ///// <summary>
        /////รหัสโปรโมชั่น XXYY-######
        ///// </summary>
        //public string rtFTPmhDocNo { get; set; }

        ///// <summary>
        /////ลำดับ
        ///// </summary>
        //public Nullable<Int64> rnFNPmdSeq { get; set; }

        ///// <summary>
        /////ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        ///// </summary>
        //public string rtFTPmdStaType { get; set; }

        ///// <summary>
        /////ชื่อกลุ่มจัดรายการ
        ///// </summary>
        //public string rtFTPmdGrpName { get; set; }

        ///// <summary>
        /////รหัสสินค้า
        ///// </summary>
        //public string rtFTPmdRefCode { get; set; }

        ///// <summary>
        /////รหัสหน่วย
        ///// </summary>
        //public string rtFTPmdSubRef { get; set; }

        ///// <summary>
        /////รหัสบาร์โค้ด ณ. บันทึก
        ///// </summary>
        //public string rtFTPmdBarCode { get; set; }
    }
}
