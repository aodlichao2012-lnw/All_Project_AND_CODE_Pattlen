using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.AppModule
{
    public class cmlResSysApp_L
    {
        /// <summary>
        ///รหัสแอปพลิเคชั่น
        /// <summary>
        public string rtAppCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อแอปพลิเคชั่น
        /// <summary>
        public string rtAppName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtAppRmk { get; set; }
    }
}
