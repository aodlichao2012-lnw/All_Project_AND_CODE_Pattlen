using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTTPdtPrice4CSTTmp
    {
        /// <summary>
        ///รหัสกลุ่มราคาสินค้า(ตามลูกค้าในกลุ่ม)
        /// <summary>
        public string FTPplCode { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///รหัส ขนาด เพื่อแยกราคา กรณีราคาเดียวไม่กำหนด
        /// <summary>
        public string FTPzeCode { get; set; }

        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด
        /// <summary>
        public string FTRthCode { get; set; }

        /// <summary>
        ///วันที่เริ่ม
        /// <summary>
        public Nullable<DateTime> FDPghDStart { get; set; }

        /// <summary>
        ///เวลาเริ่มมีผล
        /// <summary>
        public string FTPghTStart { get; set; }

        /// <summary>
        ///วันที่หมดอายุ
        /// <summary>
        public Nullable<DateTime> FDPghDStop { get; set; }

        /// <summary>
        ///เวลาหมดอายุ
        /// <summary>
        public string FTPghTStop { get; set; }

        /// <summary>
        ///รหัสร้านค้า /ตู้Locker 1:1  บังคับกรณี FTPdtRentType=2
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string FTPghDocNo { get; set; }

        /// <summary>
        ///ประเภทราคา 1:BasePrice(FTPghStaAdj=1), 2:Price Off
        /// <summary>
        public string FTPghDocType { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }
    }
}
