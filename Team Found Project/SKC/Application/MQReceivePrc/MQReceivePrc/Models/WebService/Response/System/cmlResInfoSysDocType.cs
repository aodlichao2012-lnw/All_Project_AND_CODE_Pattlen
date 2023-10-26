using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.System
{
    public class cmlResInfoSysDocType
    {
        /// <summary>
        ///(AUTONUMBER)รหัสเอกสาร
        /// </summary>
        public Nullable<Int64> rnSdtID { get; set; }    //*Arm 63-01-30

        /// <summary>
        ///กลุ่มเอกสาร
        /// </summary>
        public string rtSdtDocGrp { get; set; }     //*Arm 63-01-30

        public string rtSdtTblName { get; set; }
        public string rtSdtFedTypeName { get; set; }
        public int rnSdtDocType { get; set; }
        public string rtSdtDocName { get; set; }
        public string rtSdtDocTypeRef { get; set; }
    }
}
