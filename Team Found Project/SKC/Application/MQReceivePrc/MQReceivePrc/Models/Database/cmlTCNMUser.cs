using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTCNMUser
    {
        public string FTUsrCode { get; set; }
        public string FTDptCode { get; set; }
        public string FTRolCode { get; set; }
        public string FTUsrTel { get; set; }
        public string FTUsrPwd { get; set; }
        public string FTUsrEmail { get; set; }
        public string FDDateUpd { get; set; }
        public string FTTimeUpd { get; set; }
        public string FTWhoUpd { get; set; }
        public string FDDateIns { get; set; }
        public string FTTimeIns { get; set; }
        public string FTWhoIns { get; set; }

        // TCNMUser_L
        public string FNLngID { get; set; }
        public string FTUsrName { get; set; }
        public string FTUsrRmk { get; set; }


        // TPSTShiftDT
        public Nullable<DateTime> FDSdtDSignIn { get; set; }

        // TCNMShop
        public string FTShpCode { get; set; }
        public string FTMerCode { get; set; }
        // TCNTUsrGroup
        public string FTUsrStaShop { get; set; }

        /// <summary>
        ///ระดับ (สำหรับ กำหนดสิทธ์ แบบ ระดับ)
        /// <summary>
        public Nullable<Int64> FNRolLevel { get; set; } //*Em 62-09-03
    }
}
