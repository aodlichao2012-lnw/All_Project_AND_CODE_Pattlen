using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMPdtPriDOWTmp
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///0:Sun 1:Mon 2:Tue 3:Wed 4:Ths 5:Fri 6:Sat
        /// <summary>
        public Nullable<int> FNPpdDayOfWeek { get; set; }

        /// <summary>
        ///รหัสร้านค้า /ตู้Locker 1:1  บังคับกรณี FTPdtRentType=2
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัส ขนาด เพื่อแยกราคา กรณีราคาเดียวไม่กำหนด
        /// <summary>
        public string FTPzeCode { get; set; }

        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด
        /// <summary>
        public string FTRthCode { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }
    }
}
