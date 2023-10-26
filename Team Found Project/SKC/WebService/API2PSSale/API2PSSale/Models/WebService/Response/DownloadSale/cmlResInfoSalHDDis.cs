﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.WebService.Response.DownloadSale
{
    public class cmlResInfoSalHDDis
    {
        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string rtXshDocNo { get; set; }

        /// <summary>
        ///วัน/เวลาทำรายการ [dd/mm/yyyy H:mm:ss]
        /// </summary>
        public Nullable<DateTime> rdXhdDateIns { get; set; }

        /// <summary>
        ///ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// </summary>
        public string rtXhdDisChgTxt { get; set; }

        /// <summary>
        ///ประเภทลดชาร์จ 1:ลดบาท 2: ลด % 3: ชาร์จบาท 4: ชาร์จ %
        /// </summary>
        public string rtXhdDisChgType { get; set; }

        /// <summary>
        ///ยอดรวมหลังลด (FCXshTotalAfDisChgV+FCXshTotalAfDisChgNV)
        /// </summary>
        public Nullable<decimal> rcXhdTotalAfDisChg { get; set; }

        /// <summary>
        ///ยอดลด/ชาร์จ
        /// </summary>
        public Nullable<decimal> rcXhdDisChg { get; set; }

        /// <summary>
        ///มูลค่าลด/ชาร์จ
        /// </summary>
        public Nullable<decimal> rcXhdAmt { get; set; }

        /// <summary>
        /// เลขที่อ้างอิง
        /// </summary>
        public string rtXhdRefCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtDisCode { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public string rtRsnCode { get; set; }  
    }
}