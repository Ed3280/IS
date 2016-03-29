using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Dashboard.Directorio
{
    public class Empleado {

        public int Id { get; set; }
        public String Nombre { get; set; }

        public String UnidadAdministrativa { get; set; }

        [Display(Name = "NombreCargo"
               , ResourceType = typeof(Resources.CampoResource))]
        public String Cargo { get; set; }

        [Display(Name = "Extension"
             , ResourceType = typeof(Resources.CampoResource))]
        public int? Extension { get; set; }

        [Display(Name = "NumeroMovil"
             , ResourceType = typeof(Resources.CampoResource))]
        public int? NumeroMovil { get; set; }

        [Display(Name = "Email"
         , ResourceType = typeof(Resources.CampoResource))]
        public String Correo { get; set; }

    }
}