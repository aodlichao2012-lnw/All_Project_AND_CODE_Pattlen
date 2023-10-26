using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Class
{
    class cMS
    {

        public readonly string tMS_CfgLoadFalse = "Load config false!: ";
        public readonly string tMS_CfgNotFound = "Config {0} not found.";
        public readonly string tMS_Key = "Sk$4dF#z";
        // Response code.
        public readonly string tMS_RespCode200 = "200";         //*Arm 62-12-27
        public readonly string tMS_RespCode400 = "400";         //*Arm 62-12-27
        public readonly string tMS_RespCode401 = "401";         //*Arm 63-01-07

        // Response descript.
        public readonly string tMS_RespDesc200 = "success.";                    //*Arm 62-12-27
        public readonly string tMS_RespDesc400 = "Recording failed.";           //*Arm 62-12-27
        public readonly string tMS_RespDesc401 = "Can't use Coupon!.";          //*Arm 63-01-07
    }
}
