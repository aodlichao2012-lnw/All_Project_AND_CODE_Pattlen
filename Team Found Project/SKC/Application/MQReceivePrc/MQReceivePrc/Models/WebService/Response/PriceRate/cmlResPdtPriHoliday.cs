using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.PriceRate
{
    public class cmlResPdtPriHoliday
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///วันที่เลือก
        /// <summary>
        public Nullable<DateTime> rdPphCheckIn { get; set; }

        /// <summary>
        ///ถึงวันที่(สำหรับกำหนดเป็นช่วง)
        /// <summary>
        public Nullable<DateTime> rdPphToSpcDay { get; set; }

        /// <summary>
        ///รหัสร้านค้า /ตู้Locker 1:1  บังคับกรณี FTPdtRentType=2
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัส ขนาด เพื่อแยกราคา กรณีราคาเดียวไม่กำหนด
        /// <summary>
        public string rtPzeCode { get; set; }

        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด
        /// <summary>
        public string rtRthCode { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }
    }
}
