using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Receive
{
    public class cmlResInfoRcv
    {
        public string rtRcvCode { get; set; }
        public string rtFmtCode { get; set; }
        public string rtRcvStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///สถานะใฃ้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// </summary>
        public string rtRcvStaShwInSlip { get; set; }   //*Arm 63-07-30

        /// <summary>
        ///รหัสรูปแบบการรับชำระเงินกรณีทำรายการ คืน
        /// </summary>
        public string rtRcv4Ret { get; set; }           //*Arm 63-07-30

        /// <summary>
        ///รหัสรูปแบบการรับชำระเงินกรณีทำรายการ checkout
        /// </summary>
        public string rtRcv4ChkOut { get; set; }        //*Arm 63-07-30

        /// <summary>
        ///1: อนุญาต ให้ ทำรายการคืนได้  2 :ไม่อนุญาต ให้ ทำรายการคืน
        /// </summary>
        public string rtAppStaAlwRet { get; set; }      //*Arm 63-07-30

        /// <summary>
        ///1: อนุญาต ให้ ทำการยกเลิกรายการได้ 2 :ไม่อนุญาต ให้ ทำการยกเลิกรายการ
        /// </summary>
        public string rtAppStaAlwCancel { get; set; }   //*Arm 63-07-30

        /// <summary>
        ///1: อนุญาต ให้ มีรายการอื่น ต่อท้าย 2 :ไม่อนุญาต ให้ มีรายการอื่น ต่อท้าย
        /// </summary>
        public string rtAppStaPayLast { get; set; }     //*Arm 63-07-30

        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
