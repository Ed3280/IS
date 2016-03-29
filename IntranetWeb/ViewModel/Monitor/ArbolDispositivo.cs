using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Monitor
{
    public class ArbolDispositivo
    {
        public ArbolDispositivo() {
            this.Distribuidores = new List<NodoDistribuidor>();
        }
        public IEnumerable<NodoDistribuidor> Distribuidores { get; set; }

    }
}