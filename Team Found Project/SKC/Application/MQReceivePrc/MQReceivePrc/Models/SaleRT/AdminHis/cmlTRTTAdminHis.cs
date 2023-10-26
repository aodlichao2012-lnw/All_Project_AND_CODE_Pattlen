using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleRT.AdminHis
{
    class cmlTRTTAdminHis
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        /// รหัส Shop
        /// </summary>
        public string FTShpCode { get; set; }

        /// <summary>
        /// รหัสเครื่อง Pos/ตู้   [Refer Address]
        /// </summary>
        public string FTPosCode { get; set; }

        /// <summary>
        /// วันที่เวลาทำรายการ
        /// </summary>
        public DateTime? FDHisDateTime { get; set; }

        /// <summary>
        /// ลำดับช่อง
        /// </summary>
        public int FNHisLayNo { get; set; }

        /// <summary>
        /// รหัสผู้บันทึก
        /// </summary>
        public string FTHisUsrCode { get; set; }

        /// <summary>
        /// เบอร์โทร
        /// </summary>
        public string FTHisCstTel { get; set; }

        /// <summary>
        /// รหัสเหตุผล
        /// </summary>
        public string FTHisRsnCode { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public DateTime? FDLastUpdOn { get; set; }

        /// <summary>
        /// ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        public DateTime? FDCreateOn { get; set; }

        /// <summary>
        /// ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }
    }
}
