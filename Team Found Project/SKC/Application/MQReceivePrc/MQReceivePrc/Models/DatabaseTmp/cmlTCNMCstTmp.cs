using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCstTmp
    {
        public string FTCstCode { get; set; }
        public string FTCstCardID { get; set; }
        public string FTCstTaxNo { get; set; }
        public string FTCstTel { get; set; }
        public string FTCstFax { get; set; }
        public string FTCstEmail { get; set; }
        public string FTCstSex { get; set; }
        public Nullable<DateTime> FDCstDob { get; set; }
        public string FTCgpCode { get; set; }
        public string FTCtyCode { get; set; }
        public string FTClvCode { get; set; }
        public string FTPplCodeRet { get; set; }
        public string FTPplCodeWhs { get; set; }
        public string FTPplCodenNet { get; set; }
        public string FTPmgCode { get; set; }
        public string FTOcpCode { get; set; }
        public string FTSpnCode { get; set; }
        public string FTUsrCode { get; set; }
        public string FTCstDiscWhs { get; set; }
        public string FTCstDiscRet { get; set; }
        public string FTCstBusiness { get; set; }
        public string FTCstBchHQ { get; set; }
        public string FTCstBchCode { get; set; }
        public string FTCstStaActive { get; set; }

        /// <summary>
        /// อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต (default)
        /// </summary>
        public string FTCstStaAlwPosCalSo { get; set; } //*Arm 63-02-20

        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
