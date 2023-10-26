using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.AppModule
{
    public class cmlResSysApp
    {
        /// <summary>
        ///รหัสแอปพลิเคชั่น
        /// <summary>
        public string rtAppCode { get; set; }

        /// <summary>
        ///เวอร์ชั่น
        /// <summary>
        public string rtAppVersion { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string rtCreateBy { get; set; }
    }
}
