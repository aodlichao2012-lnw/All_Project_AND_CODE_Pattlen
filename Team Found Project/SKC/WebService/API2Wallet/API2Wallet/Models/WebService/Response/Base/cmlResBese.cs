﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Base
{
    /// <summary>
    /// ตอบรับสถานะการทำงาน
    /// </summary>
    public class cmlResBese
    {
        /// <summary>
        /// System process status.
        /// </summary>
        public string rtCode;

        /// <summary>
        /// System process description.
        /// </summary>
        public string rtDesc;
    }
}