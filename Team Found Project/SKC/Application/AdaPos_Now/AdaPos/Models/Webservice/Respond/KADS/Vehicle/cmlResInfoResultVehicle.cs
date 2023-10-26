using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.KADS.Vehicle
{
    public class cmlResInfoResultVehicle
    {
        public string KubotaId { get; set;}
        public string PlantCode { get; set; }
        public string VhVin { get; set; }
        public string EngNo { get; set; }
        public string Model { get; set; }
        public string Kunnr { get; set; }
        public string SaleDate { get; set; }

    }
}
