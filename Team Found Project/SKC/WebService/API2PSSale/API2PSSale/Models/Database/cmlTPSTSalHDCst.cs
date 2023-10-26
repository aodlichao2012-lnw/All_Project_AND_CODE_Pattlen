﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.Database
{
    public class cmlTPSTSalHDCst
    {
        //*Arm 63-01-24 - ปรับโครงสร้าง Database ใหม่

        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///เลขที่บัตรประจำตัวประชาชน/Passport
        /// </summary>
        public string FTXshCardID { get; set; }

        /// <summary>
        ///เลขบัตรสมาชิก
        /// </summary>
        public string FTXshCardNo { get; set; }

        /// <summary>
        ///ระยะเครดิต
        /// </summary>
        public Nullable<int> FNXshCrTerm { get; set; }

        /// <summary>
        ///วันที่ครบกำหนด
        /// </summary>
        public Nullable<DateTime> FDXshDueDate { get; set; }

        /// <summary>
        ///วันที่จะรับ/วางบิล
        /// </summary>
        public Nullable<DateTime> FDXshBillDue { get; set; }

        /// <summary>
        ///ชื่อผู้ตืดต่อ
        /// </summary>
        public string FTXshCtrName { get; set; }

        /// <summary>
        ///วันที่ส่งของ
        /// </summary>
        public Nullable<DateTime> FDXshTnfDate { get; set; }

        /// <summary>
        ///เลขที่ ใบขนส่ง
        /// </summary>
        public string FTXshRefTnfID { get; set; }

        /// <summary>
        ///ที่อยู่ส่งของ
        /// </summary>
        public Nullable<Int64> FNXshAddrShip { get; set; }

        /// <summary>
        ///ที่อยู่ใบกำกับภาษี
        /// </summary>
        public Nullable<Int64> FNXshAddrTax { get; set; }

        /// <summary>
        ///ชื่อผู้ตืดต่อ
        /// </summary>
        public string FTXshCstName { get; set; }

        /// <summary>
        /// เบอร์โทรลูกค้า
        /// </summary>
        public string FTXshCstTel { get; set; }

        /// <summary>
        /// แต้มที่ได้รับจากบิล
        /// </summary>
        public decimal FCXshCstPnt { get; set; }     //*Arm 63-04-15

        /// <summary>
        /// แต้มที่ได้รับจาก Promotion
        /// </summary>
        public decimal FCXshCstPntPmt { get; set; }     //*Arm 63-04-15

    }
}