using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Dashboard
{
    public class UsuarioDashboard
    {
        
        public bool EsEmpleado { get; set; }

        public IEnumerable<string> aplicaciones { get; set; }
    }
}