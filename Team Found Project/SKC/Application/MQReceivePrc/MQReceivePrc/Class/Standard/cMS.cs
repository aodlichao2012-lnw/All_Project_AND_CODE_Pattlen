using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class.Standard
{
    class cMS
    {
        public readonly string tMS_CfgLoadFalse = "Load config false!: ";
        public readonly string tMS_CfgNotFound = "Config {0} not found.";

        // Response code.
        public readonly string tMS_RespCode001 = "001";
        public readonly string tMS_RespCode200 = "200";         //*Arm 62-12-27
        public readonly string tMS_RespCode400 = "400";         //*Arm 62-12-27
        public readonly string tMS_RespCode401 = "401";         //*Arm 63-01-07
        public readonly string tMS_RespCode700 = "700";         //*Arm 63-05-22
        public readonly string tMS_RespCode701 = "701";         //*Arm 63-05-22
        public readonly string tMS_RespCode800 = "800";         //*Arm 63-05-22
        public readonly string tMS_RespCode900 = "900";         //*Arm 63-05-22

        // Response descript.
        public readonly string tMS_RespDesc001 = "success.";                    //*Arm 63-05-22
        public readonly string tMS_RespDesc200 = "success.";                    //*Arm 62-12-27
        public readonly string tMS_RespDesc400 = "Recording failed.";           //*Arm 62-12-27
        public readonly string tMS_RespDesc401 = "Can't use Coupon!.";          //*Arm 63-01-07
        public readonly string tMS_RespDesc700 = "all parameter is null.";      //*Arm 63-05-22
        public readonly string tMS_RespDesc701 = "validate parameter model false.|";//*Arm 63-05-22
        public readonly string tMS_RespDesc800 = "data not found.";             //*Arm 63-05-22
        public readonly string tMS_RespDesc900 = "service process false.";      //*Arm 63-05-22
    }
}
