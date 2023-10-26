using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required
{
    public class cmlReqCardHistory
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        public string ptCrdNo { get; set; }

        /// <summary>
        /// สาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// จำนวนรายการที่ต้องการแสดง
        /// </summary>
        public int pnQty { get; set; }

        /// <summary>
        /// Sort การเรียงลำดับข้อมูล 0 = น้อย, 1 = มาก
        /// </summary>
        public string ptSort { get; set; }
    }
}
