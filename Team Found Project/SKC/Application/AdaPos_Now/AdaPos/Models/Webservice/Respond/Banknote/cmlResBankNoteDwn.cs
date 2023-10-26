using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Banknote
{
    public class cmlResBankNoteDwn
    {
        public List<cmlResInfoBankNote> raBankNote { get; set; }
        public List<cmlResInfoBankNoteLng> raBankNoteLng { get; set; }

        public List<cmlResInfoImgObj> raImage { get; set; }
    }
}
