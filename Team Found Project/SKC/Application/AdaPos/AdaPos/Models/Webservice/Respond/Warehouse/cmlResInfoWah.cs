using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Warehouse
{
    public class cmlResInfoWah
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; } //*Arm 63-03-27

        public string rtWahCode { get; set; }
        public string rtWahStaType { get; set; }
        public string rtWahRefCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
