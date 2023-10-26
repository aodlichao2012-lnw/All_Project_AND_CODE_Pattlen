using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMTxnAPITmp
    {
        /// <summary>
        ///รหัสเส้น API ที่ Interface
        /// </summary>
        public string FTApiCode { get; set; }

        /// <summary>
        ///ประเภทของเส้น Interface 1=นำเข้า 2=ส่งออก  3= API (STD)
        /// </summary>
        public string FTApiTxnType { get; set; }

        /// <summary>
        ///ประเภทของการทำงาน 1 =Batch  2=Real time
        /// </summary>
        public string FTApiPrcType { get; set; }

        /// <summary>
        ///กลุ่มของการ Interface Ex. กลุ่มสินค้า  , กลุ่มลูกค้า เพื่อใช้จัดกลุ่มเส้น Interface
        /// </summary>
        public string FTApiGrpPrc { get; set; }

        /// <summary>
        ///ลำดับของกลุ่ม Interface
        /// </summary>
        public Nullable<int> FNApiGrpSeq { get; set; }

        /// <summary>
        ///รหัสรูปแบบของ API
        /// </summary>
        public string FTApiFmtCode { get; set; }

        /// <summary>
        ///เก็บ URL ของเส้น Interface กรณีที่ Interface นั้นต้องใช้ Webservice
        /// </summary>
        public string FTApiURL { get; set; }

        /// <summary>
        ///เก็บรหัส User ที่ใช้สำหรับ Login เข้า API
        /// </summary>
        public string FTApiLoginUsr { get; set; }

        /// <summary>
        ///เก็บรหัสผ่าน เข้ารหัสตาม Format ADA
        /// </summary>
        public string FTApiLoginPwd { get; set; }

        /// <summary>
        ///เก็บ Token ที่ใช้ในการ Interface กับ 3Party
        /// </summary>
        public string FTApiToken { get; set; }

        /// <summary>
        ///วันที่สร้าง
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้าง
        /// </summary>
        public string FTCreateBy { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }
    }
}
