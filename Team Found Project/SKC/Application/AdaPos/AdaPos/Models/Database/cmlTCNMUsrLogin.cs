using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNMUsrLogin
    {
        public string FTUsrCode { get; set; }
        public string FTUsrLogType { get; set; }
        public string FDUsrPwdStart { get; set; }
        public string FDUsrPwdExpired { get; set; }
        public string FTUsrLogin { get; set; }
        public string FTUsrLoginPwd { get; set; }
        public string FTUsrRmk { get; set; }
        public string FTUsrStaActive { get; set; }
    }
}
