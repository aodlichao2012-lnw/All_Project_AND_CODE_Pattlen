using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Events
{
    public class cmlResInfoEventDTLng
    {
        /// <summary>
        ///รหัส
        /// <summary>
        public string rtEvhCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> rnEvdSeqNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ
        /// <summary>
        public string rtEvdName { get; set; }
    }
}
