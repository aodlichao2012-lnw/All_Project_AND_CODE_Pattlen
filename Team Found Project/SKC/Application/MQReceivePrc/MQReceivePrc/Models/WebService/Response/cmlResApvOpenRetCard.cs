using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response
{
    public class cmlResApvOpenRetCard
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        public string rtCrdCode { get; set; }

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// สถานะ 1 สำเร็จ , 2 ไม่สำเร็จ ,3 บัตรเบิกใช้งานแล้ว ,4 ไม่พบข้อมูลบัตร, 5 ประเภทบัตรไม่ใช่แบบปกติ
        /// </summary>
        public string rtStatus { get; set; }

    }
}
