﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductPrice
{
    public class cmlResInfoPdtApv4Pdt
    {
        public string rtPdtCode { get; set; }
        public string rtPunCode { get; set; }
        public DateTime rdPghDStart { get; set; }
        public string rtPghTStart { get; set; }
        public Nullable<DateTime> rdPghDStop { get; set; }
        public string rtPghTStop { get; set; }
        public string rtPghDocNo { get; set; }
        public string rtPghDocType { get; set; }
        public string rtPghStaAdj { get; set; }
        public Nullable<double> rcPgdPriceRet { get; set; }
        public Nullable<double> rcPgdPriceWhs { get; set; }
        public Nullable<double> rcPgdPriceNet { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        public string rtLastUpdBy { get; set; }     //*Arm 63-03-26
        public Nullable<DateTime> rdCreateOn { get; set; }      //*Arm 63-03-26
        public string rtCreateBy { get; set; }      //*Arm 63-03-26

        /// <summary>
        /// รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }   //*Arm 63-03-26

    }
}
