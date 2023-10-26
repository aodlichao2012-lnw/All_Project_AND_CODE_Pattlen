using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTSysDocType
    {
        /// <summary>
        ///(AUTONUMBER)รหัสเอกสาร
        /// </summary>
        public Nullable<Int64> FNSdtID { get; set; }    //*Arm 63-01-30

        /// <summary>
        ///กลุ่มเอกสาร
        /// </summary>
        public string FTSdtDocGrp { get; set; }     //*Arm 63-01-30

        public string FTSdtTblName { get; set; }
        public string FTSdtFedTypeName { get; set; }
        public int FNSdtDocType { get; set; }
        public string FTSdtDocName { get; set; }
        public string FTSdtDocTypeRef { get; set; }

    }
}
