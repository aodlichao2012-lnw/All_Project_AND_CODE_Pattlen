﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SalePerson
{
    public class cmlResSpnDwn
    {
        public List<cmlResInfoSpn> raSpn { get; set; }
        public List<cmlResInfoSpnLng> raSpnLng { get; set; }
        public List<cmlResInfoSpnGrp> raSpnGrp { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }
    }
}
