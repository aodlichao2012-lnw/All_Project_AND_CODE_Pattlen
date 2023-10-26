using MQReceivePrc.Models.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class.Standard
{
    public class cVB
    {
        public static string tVB_BchHQ;         //*Arm 63-01-07
        public static string tVB_BchCode;
        public static string tVB_ConnStr;
        public static string tVB_ErrContain = "sql,timeout,network"; //*Net 63-09-02 เมื่อ Queue มี Error ในชุดนี้จะ Retry Msg ใหม่
        public static string tVB_UniqueTimeCre;         //*Net 63-09-02 yyyyMMddHHmmssfff วันเวลาที่เปิดโปรแกรม

        public static int nVB_Language = 1;
        public static int nVB_CmdTime = 30;

        public static cmlShopDB oVB_ShopDB;
        public static SqlConnection oVB_ConnDB;
        public static bool bVB_StaUseCentralized;       //*Arm 63-03-31 ระบบ Centralized 1:ใช้งาน 2:ไม่ใช้งาน 
        public static bool bVB_StaAlwSendAdaLink;       //*Arm 63-08-04 สถานะอนุญาตส่งการขายไป MQAdaLink  1:อนุญาต, 2:ไม่อนุญาต
    }
}
