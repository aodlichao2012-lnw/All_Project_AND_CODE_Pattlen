using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleRT.RentalSale
{
    class cmlTRTTSalDTSL
    {
        /// <summary>
        /// สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        /// ลำดับ
        /// </summary>
        public int FNXsdSeqNo { get; set; }

        /// <summary>
        /// รหัสร้านค้า /ตู้Locker//model 1:1  บังคับกรณี FTPdtRentType=2
        /// </summary>
        public string FTShpCode { get; set; }

        /// <summary>
        /// รหัสสินค้า //model
        /// </summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ลำดับช่อง
        /// </summary>
        public int FNXsdLayNo { get; set; }

        /// <summary>
        /// สถานะการสั่งเปิด  1:สำเร็จ 2:ไม่สำเร็จ
        /// </summary>
        public string FTXsdStaPayItem { get; set; }
    }
}
