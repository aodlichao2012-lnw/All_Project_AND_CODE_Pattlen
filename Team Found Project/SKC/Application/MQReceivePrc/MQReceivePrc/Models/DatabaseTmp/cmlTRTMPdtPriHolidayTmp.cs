using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMPdtPriHolidayTmp
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///วันที่เลือก
        /// <summary>
        public Nullable<DateTime> FDPphCheckIn { get; set; }

        /// <summary>
        ///ถึงวันที่(สำหรับกำหนดเป็นช่วง)
        /// <summary>
        public Nullable<DateTime> FDPphToSpcDay { get; set; }

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
