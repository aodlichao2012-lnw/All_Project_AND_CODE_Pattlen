using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTSysSetting
    {
        public string FTSysCode { get; set; }
        public string FTSysApp { get; set; }
        public string FTSysKey { get; set; }
        public string FTSysSeq { get; set; }
        public string FTGmnCode { get; set; }
        public string FTSysStaAlwEdit { get; set; }
        public string FTSysStaDataType { get; set; }
        public int FNSysMaxLength { get; set; }
        public string FTSysStaDefValue { get; set; }
        public string FTSysStaDefRef { get; set; }
        public string FTSysStaUsrValue { get; set; }
        public string FTSysStaUsrRef { get; set; }
        public Nullable<DateTime> FDDateUpd { get; set; }
        public string FTTimeUpd { get; set; }
        public string FTWhoUpd { get; set; }
        public Nullable<DateTime> FDDateIns { get; set; }
        public string FTTimeIns { get; set; }
        public string FTWhoIns { get; set; }
        public int FNLngID { get; set; }
        public string FTSysName { get; set; }
        public string FTSysDesc { get; set; }
        public string FTSysRmk { get; set; }

    }
}
