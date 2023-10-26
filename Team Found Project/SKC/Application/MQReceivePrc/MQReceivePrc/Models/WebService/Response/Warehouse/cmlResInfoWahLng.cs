﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Warehouse
{
    public class cmlResInfoWahLng
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }       //*Arm 63-01-30

        public string rtWahCode { get; set; }
        public int rnLngID { get; set; }
        public string rtWahName { get; set; }
        public string rtWahRmk { get; set; }
    }
}
