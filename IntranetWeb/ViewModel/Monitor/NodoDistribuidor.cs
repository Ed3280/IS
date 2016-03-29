using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Monitor
{
    public class NodoDistribuidor
    {
        public NodoDistribuidor() {
            this.Distribuidor = new Distribuidor();
            this.DispositivosAsociados = new List<Dispositivo>();
        }

        public Distribuidor Distribuidor { get; set; }

        public IEnumerable<Dispositivo> DispositivosAsociados { get; set; }
    }
}