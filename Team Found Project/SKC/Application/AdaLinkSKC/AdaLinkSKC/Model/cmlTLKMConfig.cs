using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Model
{
    public class cmlTLKMConfig
    {       

        /// <summary>
        ///รหัสระบบ/Option
        /// <summary>
        public string FTCfgCode { get; set; }

        /// <summary>
        ///ชื่อโปรแกรม
        /// <summary>
        public string FTCfgApp { get; set; }

        /// <summary>
        ///กำหนดกลุ่มระบบ/หน้าจอ
        /// <summary>
        public string FTCfgKey { get; set; }

        /// <summary>
        ///รหัสลำดับ
        /// <summary>
        public string FTCfgSeq { get; set; }

        /// <summary>
        ///กลุ่มเมนู จาก TSysMenuGrp
        /// <summary>
        public string FTGmnCode { get; set; }

        /// <summary>
        ///แก้ไข 0:ไม่อนุญาต, 1:อนุญาต
        /// <summary>
        public string FTCfgStaAlwEdit { get; set; }

        /// <summary>
        ///ประเภทแก้ไข 0:Text 1:Int 2:Double 3:Date 4:Yes/No 5:Combo 6:Brows from master
        /// <summary>
        public string FTCfgStaDataType { get; set; }

        /// <summary>
        ///ความกว้างและค่าสูงสุด
        /// <summary>
        public Nullable<int> FNCfgMaxLength { get; set; }

        /// <summary>
        ///ค่าสำคัญ Default
        /// <summary>
        public string FTCfgStaDefValue { get; set; }

        /// <summary>
        ///ส่วนอ้างอิง Default
        /// <summary>
        public string FTCfgStaDefRef { get; set; }

        /// <summary>
        ///ค่าสำคัญ ผู้ใช้กำหนด
        /// <summary>
        public string FTCfgStaUsrValue { get; set; }

        /// <summary>
        ///ส่วนอ้างอิง ผู้ใช้กำหนด
        /// <summary>
        public string FTCfgStaUsrRef { get; set; }
              
        


    }
}
