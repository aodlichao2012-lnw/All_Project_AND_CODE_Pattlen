using AdaLinkSKC.Model;
using AdaLinkSKC.Model.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class
{
    public class cVB
    {
        #region string

        public static string tVB_Conn;        
        public static string tVB_MQHost;
        public static string tVB_MQUser;
        public static string tVB_MQPass;
        public static string tVB_MQVB;
        public static string tVB_MQQueue;
        public static string tVB_MQExhg;
        public static string tVB_PathXML;        
        public static string tVB_Branch;
        public static string tVB_XMLMaster;
        public static string tVB_XMLPrice;
        public static string tVB_XMLEmployee;

        public static string tVB_PathIN;
        public static string tVB_PathOUT;
        public static string tVB_PathBackUP;
        public static string tVB_MasBackUP;

        public static string tVB_CmpCode;
        public static string tVB_BchCode;
        public static string tVB_BchRefID;
        public static string tVB_AgnCode;
        public static string tVB_SaleOrg;
        public static string tVB_WahCode;
        public static string tVB_Sloc;
        public static string tVB_Channel;

        // Mail
        public static string tVB_MailSnd;
        public static string tVB_MailRcv;
        public static string tVB_MailCC;
        public static string tVB_MailBCC;
        public static string tVB_MailSubj;
        public static string tVB_MailPwd;
        public static string tVB_MailSMTP;
       

        // API
        public static string tVB_ApiExport;
        public static string tVB_ApiAuth;
        public static string tVB_ApiName;


        #endregion
        #region Integer
        public static int nVB_MQPort;
        public static int nVB_SplitURL;
        public static int nVB_SubStr;
        public static int nVB_ToSnd;            //*Arm 63-07-05 รอบการส่งสูงสุด
        public static int nVB_Desc;             //*Arm 63-07-05 จำนวนทศนิยม
        public static int nVB_Pending;          //*Arm 63-07-05 จำนวนเวลาที่รอ (second) ก่อน request api รอบถัดไป

        //Mail
        public static int nVB_MailPORT;

        #endregion

        #region bool
        public static bool bVB_OpenLogMonitor;   //*Arm 63-07-08 สถานะการเปิดใช้ Log Monitor
        #endregion

        #region Object
        public static cmlConfigDB oVB_Config;
        public static SqlConnection oVB_ConnDB;
        public static DataTable oTblAdj = new DataTable();
        public static cmlRabbitMQ oVB_RabbitMQ;
        #endregion

        //#region List Array
        //public static List<cmlTLKMConfig> aVB_TLKMConfig;
        //#endregion

    }
}
