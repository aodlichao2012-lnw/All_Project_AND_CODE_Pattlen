using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
    public class cmlCstCard
    {
        /// <summary>
        /// รหัสลูกค้า
        /// </summary>
        public string tCstCode { get; set; }

        /// <summary>
        /// ชื่อลูกค้า
        /// </summary>
        public string tCstName { get; set; }

        /// <summary>
        /// เบอร์โทรลูกค้า
        /// </summary>
        public string tCstTel { get; set; }

        /// <summary>
        /// email ลูกค้า
        /// </summary>
        public string tCstEmail { get; set; }

        /// <summary>
        /// เพศ 1:ชาย 2:หญิง
        /// </summary>
        public string tCstSex { get; set; }

        /// <summary>
        /// หมายเลขบัตรประจำตัวประชาชน
        /// </summary>
        public string tCstIDCard { get; set; }

        /// <summary>
        /// รหัสระดับ
        /// </summary>
        public string tCstlvCode { get; set; }

        /// <summary>
        /// หมายเลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string tCstTaxNo { get; set; }

        /// <summary>
        /// กลุ่มราคาลูกค้า
        /// </summary>
        public string tCstPriceGroup { get; set; }

        /// <summary>
        /// ชื่อกลุ่มราคา
        /// </summary>
        public string tPriceGrpName { get; set; }

        /// <summary>
        /// หมายเลขบัตรลูกค้า
        /// </summary>
        public string tCstCardNo { get; set; }

        /// <summary>
        /// อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต
        /// </summary>
        public string tCstStaAlwPosCalSo { get; set; }

        /// <summary>
        /// แต้มคงเหลือ
        /// </summary>
        public int nCstPoint { get; set; }

        /// <summary>
        /// วันเกิด
        /// </summary>
        public DateTime? dCstDOB { get; set; }

        /// <summary>
        /// วันที่สมัคร
        /// </summary>
        public DateTime? dCstApply { get; set; }

        /// <summary>
        /// วันที่ออกบัตร
        /// </summary>
        public DateTime? dCstCardIssue { get; set; }

        /// <summary>
        /// วันที่หมดอายุบัตร
        /// </summary>
        public DateTime? dCstCrdExpire { get; set; }

        /// <summary>
        /// สถานะการเป็นสมาชิก
        /// </summary>
        /// <returns></returns>
        public bool bStaAge() 
        {
            if (dCstCrdExpire == null) return false;
            return (DateTime.Now < dCstCrdExpire);
        }
    }
}
