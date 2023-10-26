using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Model.ExportSale
{
    public class cmlDataExpSale
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptFilter { get; set; }

        /// <summary>
        /// รหัสคลัง
        /// </summary>
        public string ptWaHouse { get; set; }

        /// <summary>
        /// รหัสเครื่องจุดขาย
        /// </summary>
        public string ptPosCode { get; set; }

        /// <summary>
        /// รอบที่การทำงาน Def = 1
        /// </summary>
        public string ptRound { get; set; }

        /// <summary>
        /// จากวันที่
        /// </summary>
        public string ptDateFrm { get; set; }

        /// <summary>
        /// ถึงวันที่
        /// </summary>
        public string ptDateTo { get; set; }

        /// <summary>
        ///  จากเลขที่บิลขาย
        /// </summary>
        public string ptDocNoFrm { get; set; }

        /// <summary>
        ///  ถึงเลขที่บิลขาย
        /// </summary>
        public string ptDocNoTo { get; set; }
    }
}
