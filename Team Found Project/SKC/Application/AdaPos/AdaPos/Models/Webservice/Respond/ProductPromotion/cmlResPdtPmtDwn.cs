﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductPromotion
{
    public class cmlResPdtPmtDwn
    {
        public List<cmlResInfoPdtPmtHD> raPdtPmtHD { get; set; }
        public List<cmlResInfoPdtPmtDT> raPdtPmtDT { get; set; }
        public List<cmlResInfoPdtPmtCD> raPdtPmtCD { get; set; }
        public List<cmlResInfoPdtPmtCB> raPdtPmtCB { get; set; }
        public List<cmlResInfoPdtPmtCG> raPdtPmtCG { get; set; }
        public List<cmlResInfoPdtPmtHD_L> raPdtPmtHD_L { get; set; }
        public List<cmlResInfoPdtPmtHDBch> raPdtPmtHDBch { get; set; }
        public List<cmlResInfoPdtPmtHDCst> raPdtPmtHDCst { get; set; }
        public List<cmlResInfoPdtPmtHDCstPri> raPdtPmtHDCstPri { get; set; }

    }
}
