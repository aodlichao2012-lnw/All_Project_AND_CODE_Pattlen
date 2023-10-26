﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required.SaleDocRefer
{
    public class cmlReqSaleDwn
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string ptDocNo { get; set; }

        /// <summary>
        /// วันที่
        /// </summary>
        public Nullable<DateTime> pdSaleDate { get; set; }

        /// <summary>
        /// ประเภท
        /// </summary>
        public Nullable<int> pnDoctype { get; set; }
    }
}
