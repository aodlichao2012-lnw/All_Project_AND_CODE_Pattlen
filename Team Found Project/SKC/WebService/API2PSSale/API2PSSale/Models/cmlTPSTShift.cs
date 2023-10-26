using API2PSSale.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models
{
    public class cmlTPSTShift
    {
        public List<cmlTPSTShiftHD> aoTPSTShiftHD { get; set; }
        public List<cmlTPSTShiftDT> aoTPSTShiftDT { get; set; }
        public List<cmlTPSTShiftEvent> aoTPSTShiftEvent { get; set; }
        public List<cmlTPSTShiftSKeyBN> aoTPSTShiftSKeyBN { get; set; }
        public List<cmlTPSTShiftSKeyRcv> aoTPSTShiftSKeyRcv { get; set; }
        public List<cmlTPSTShiftSLastDoc> aoTPSTShiftSLastDoc { get; set; }
        public List<cmlTPSTShiftSRatePdt> aoTPSTShiftSRatePdt { get; set; }
        public List<cmlTPSTShiftSSumRcv> aoTPSTShiftSSumRcv { get; set; }
        public List<cmlTPSTUsrLog> aoTPSTUsrLog { get; set; } //*Arm 63-07-31 ยกมาจาก Moshi
    }
}