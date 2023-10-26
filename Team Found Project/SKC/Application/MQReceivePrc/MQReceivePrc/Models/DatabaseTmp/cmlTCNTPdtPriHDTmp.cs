using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTPdtPriHDTmp
    {
        public string FTBchCode { get; set; }
        public string FTPghDocNo { get; set; }
        public string FTPghDocType { get; set; }
        public string FTPghStaAdj { get; set; }
        public Nullable<DateTime> FDPghDocDate { get; set; }
        public string FTPghDocTime { get; set; }
        public string FTPghName { get; set; }
        public string FTPplCode { get; set; }
        public string FTAggCode { get; set; }
        public Nullable<DateTime> FDPghDStart { get; set; }
        public string FTPghTStart { get; set; }
        public Nullable<DateTime> FDPghDStop { get; set; }
        public string FTPghTStop { get; set; }
        public string FTPghPriType { get; set; }
        public string FTPghStaDoc { get; set; }
        public string FTPghStaPrcDoc { get; set; }
        public Nullable<int> FNPghStaDocAct { get; set; }
        public string FTUsrCode { get; set; }
        public string FTPghUsrApv { get; set; }
        public string FTPghZneTo { get; set; }
        public string FTPghBchTo { get; set; }
        public string FTCphRmk { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
