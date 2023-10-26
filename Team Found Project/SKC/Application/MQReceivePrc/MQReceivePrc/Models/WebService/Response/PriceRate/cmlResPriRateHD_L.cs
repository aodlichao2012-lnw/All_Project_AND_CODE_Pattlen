using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.PriceRate
{
    public class cmlResPriRateHD_L
    {
        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด
        /// <summary>
        public string rtRthCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ ราคาค่าเช่า
        /// <summary>
        public string rtRthName { get; set; }
    }
}
