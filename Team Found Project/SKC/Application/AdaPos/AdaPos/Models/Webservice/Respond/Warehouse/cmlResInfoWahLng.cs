using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Warehouse
{
    public class cmlResInfoWahLng
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; } //*Arm 63-03-27

        public string rtWahCode { get; set; }
        public int rnLngID { get; set; }
        public string rtWahName { get; set; }
        public string rtWahRmk { get; set; }
    }
}
