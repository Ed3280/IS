using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Monitor
{
    public class DeviceWS
    {
        public int Id { get; set; }

        public string SerialNumber { get; set;}

        public string Name { get; set; }

        public string CarNo { get; set; }

        public string Status { get; set; }

        public string Icon { get; set; }

        public int Course { get; set; }
        
        public string Sim { get; set; }

        public float Speed { get; set; }

    }
}