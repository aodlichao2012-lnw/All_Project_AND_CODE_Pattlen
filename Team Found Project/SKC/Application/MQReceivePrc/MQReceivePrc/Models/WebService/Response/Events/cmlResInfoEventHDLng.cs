using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Events
{
    public class cmlResInfoEventHDLng
    {
        /// <summary>
        ///รหัส
        /// <summary>
        public string rtEvhCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อรอบ
        /// <summary>
        public string rtEvhName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtEvhRmk { get; set; }
    }
}
