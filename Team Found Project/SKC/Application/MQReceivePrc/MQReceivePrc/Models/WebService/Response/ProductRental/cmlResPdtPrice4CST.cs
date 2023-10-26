using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ProductRental
{
    public class cmlResPdtPrice4CST
    {
        /// <summary>
        ///รหัสกลุ่มราคาสินค้า(ตามลูกค้าในกลุ่ม)
        /// <summary>
        public string rtPplCode { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///รหัส ขนาด เพื่อแยกราคา กรณีราคาเดียวไม่กำหนด
        /// <summary>
        public string rtPzeCode { get; set; }

        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด
        /// <summary>
        public string rtRthCode { get; set; }

        /// <summary>
        ///วันที่เริ่ม
        /// <summary>
        public Nullable<DateTime> rdPghDStart { get; set; }

        /// <summary>
        ///เวลาเริ่มมีผล
        /// <summary>
        public string rtPghTStart { get; set; }

        /// <summary>
        ///วันที่หมดอายุ
        /// <summary>
        public Nullable<DateTime> rdPghDStop { get; set; }

        /// <summary>
        ///เวลาหมดอายุ
        /// <summary>
        public string rtPghTStop { get; set; }

        /// <summary>
        ///รหัสร้านค้า /ตู้Locker 1:1  บังคับกรณี FTPdtRentType=2
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string rtPghDocNo { get; set; }

        /// <summary>
        ///ประเภทราคา 1:BasePrice(FTPghStaAdj=1), 2:Price Off
        /// <summary>
        public string rtPghDocType { get; set; }

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
