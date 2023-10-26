using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.AppModule
{
    public class cmlResAppModule
    {
        /// <summary>
        ///รหัส Application
        /// <summary>
        public string rtAppCode { get; set; }

        /// <summary>
        ///โมดูล 1:LogIn 2:Drop 3:Pick
        /// <summary>
        public string rtAppModule { get; set; }

        /// <summary>
        ///ลำดับรายการ
        /// <summary>
        public Nullable<int> rnAppSeqNo { get; set; }

        /// <summary>
        ///รูปแบบ LogIn 1:Password 2:Pincode 3:RFID 4:Finger 5:Face
        /// <summary>
        public string rtAppSignType { get; set; }
    }
}
