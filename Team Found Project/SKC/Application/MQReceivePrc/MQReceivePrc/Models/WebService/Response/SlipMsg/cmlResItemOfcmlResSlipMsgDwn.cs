﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SlipMsg
{
    public class cmlResItemOfcmlResSlipMsgDwn
    {
        public cmlResSlipMsgDwn roItem { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
}
