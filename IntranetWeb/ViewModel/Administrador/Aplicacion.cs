using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Administrador
{
    public class Aplicacion
    {
        [Key]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public int Id { get; set; }

        [Display(Name = "NombreAplicacion",
        ResourceType = typeof( Resources.CampoResource))]
        public string Nombre  {get; set;}

        public string RolesAsociados { get; set; }

        [Display(Name = "Aplicación Activa")]
        public bool IndicadorActiva { get; set; }

        public int?  IdAplicacionPadre { get; set; }

        public int NumeroSecuencia { get; set; }

        public string Icono { get; set; }

        public bool IndicadorDashBoard { get; set; }

        public bool IndicadorPerfilUsuario { get; set; }

        public bool IndicadorCheckAsignacionDeshabilitado { get; set; }
    }
}