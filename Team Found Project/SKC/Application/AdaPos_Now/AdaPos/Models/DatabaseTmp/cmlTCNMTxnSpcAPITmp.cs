using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    class cmlTCNMTxnSpcAPITmp
    {
        /// <summary>
        ///รหัสเส้น API ที่ Interface
        /// </summary>
        public string FTApiCode { get; set; }

        /// <summary>
        ///รหัสบริษัท
        /// </summary>
        public string FTCmpCode { get; set; }

        /// <summary>
        ///รหัสตัวแทนขาย
        /// </summary>
        public string FTAgnCode { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มธุรกิจ
        /// </summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// </summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่องจุดขาย
        /// </summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///รหัสรูปแบบของ API
        /// </summary>
        public string FTApiFmtCode { get; set; }

        /// <summary>
        ///เก็บ URL ของเส้น Interface กรณีที่ Interface นั้นต้องใช้ Webservice
        /// </summary>
        public string FTApiURL { get; set; }

        /// <summary>
        ///รหัส User สำหรับ API authen
        /// </summary>
        public string FTSpaUsrCode { get; set; }

        /// <summary>
        ///รหัส Password
        /// </summary>
        public string FTSpaUsrPwd { get; set; }

        /// <summary>
        ///รหัส Token key
        /// </summary>
        public string FTSpaApiKey { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string FTSpaRmk { get; set; }

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
