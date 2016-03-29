using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Dashboard.Cumpleano
{
    public class Cumpleano{

        public Cumpleano() {
            this.Empleado = new List<EmpleadoCumpleano>();
        }
        public IEnumerable<EmpleadoCumpleano> Empleado { get; set; }
    }
}