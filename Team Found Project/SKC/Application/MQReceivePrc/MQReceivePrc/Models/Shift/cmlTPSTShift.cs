using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MQReceivePrc.Models.Shift
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
        public List<cmlTPSTUsrLog> aoTPSTUsrLog { get; set; } //* Net 63-08-03 ยกมาจาก Moshi
    }
}