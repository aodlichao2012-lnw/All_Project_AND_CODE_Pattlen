﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductFashionClass
{
    public class cmlResPdtClassDwn
    {
        public List<cmlResInfoPdtClass> raPdtClass { get; set; }
        public List<cmlResInfoPdtClassLng> raPdtClassLng { get; set; }
    }
}
