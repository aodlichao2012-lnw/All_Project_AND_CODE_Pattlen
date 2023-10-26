using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNTUrlObjectLoginTmp
    {
        /// <summary>
        ///รหัส URL
        /// <summary>
        public string FTUrlRefID { get; set; }

        /// <summary>
        ///URL Address
        /// <summary>
        public string FTUrlAddress { get; set; }

        /// <summary>
        ///Virtual Host
        /// <summary>
        public string FTUolVhost { get; set; }

        /// <summary>
        ///ชื่อเข้าใช้งาน
        /// <summary>
        public string FTUolUser { get; set; }

        /// <summary>
        ///รหัสผ่าน
        /// <summary>
        public string FTUolPassword { get; set; }

        /// <summary>
        ///ล็อกอินคีย์
        /// <summary>
        public string FTUolKey { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:Active 2:Disable
        /// <summary>
        public string FTUolStaActive { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTUolgRmk { get; set; }
    }
}
