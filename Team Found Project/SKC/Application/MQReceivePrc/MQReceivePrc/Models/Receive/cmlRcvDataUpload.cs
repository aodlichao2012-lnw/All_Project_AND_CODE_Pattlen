using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cmlRcvDataUpload
    {
        /// <summary>
        /// Data Json for upload.
        /// </summary>
        public string ptData { get; set; }

        /// <summary>
        /// Connection String Database for process.
        /// </summary>
        public string ptConnStr { get; set; }
    }
}
