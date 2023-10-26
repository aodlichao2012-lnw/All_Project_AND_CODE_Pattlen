using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.BankDepositSlip
{
    public class cmlTFNTBnkDpl
    {
        public List<cmlTFNTBnkDplHD> aoTFNTBnkDplHD { get; set; }
        public List<cmlTFNTBnkDplDT> aoTFNTBnkDplDT { get; set; }
        public List<cmlTFNMBnkDepType> aoTFNMBnkDepType { get; set; }
        public List<cmlTFNMBookBank> aoTFNMBookBank { get; set; }
        public List<cmlTFNMBank> aoTFNMBank { get; set; }
        public List<cmlTFNMBookCheque> aoTFNMBookCheque { get; set; }
        public List<cmlTFNTBnkStatement> aoTFNTBnkStatement { get; set; }
    }
}
