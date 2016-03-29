using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Auth
{
    public class Menu
    {

        public IQueryable<NodoAplicacion> ArbolMenu { get; set; }

        public IQueryable<NodoAplicacion> AtajoMenu { get; set; }
        
        

    }
}