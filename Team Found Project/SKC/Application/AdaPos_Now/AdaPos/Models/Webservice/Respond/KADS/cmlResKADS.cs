using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.KADS
{
    public class cmlResKADS<T>
    {
        public T d;
    }
    public class cmlResKADSResult<T> 
    {
        public List<T> results { get; set; }
    }
}
