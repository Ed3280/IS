using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Cliente
{
    public class Direccion
    {

        public IEnumerable<SelectListItem> Pais { get; set; }

        [Display(Name = "Pais",
          ResourceType = typeof(Resources.CampoResource))]
        public int? PaisSeleccionado { get; set; }

        public string DescripcionPais { get; set; }

        [Display(Name = "Provincia",
          ResourceType = typeof(Resources.CampoResource))]
        public int? ProvinciaSeleccionada { get; set; }

        public string DescripcionProvincia { get; set; }

        public IEnumerable<SelectListItem> Provincia { get; set; }

        [Display(Name = "Distrito",
          ResourceType = typeof(Resources.CampoResource))]
        public int? MunicipioSeleccionado { get; set; }

        public string DescripcionMunicipio { get; set; }

        public IEnumerable<SelectListItem> Municipio { get; set; }


        [Display(Name = "Corregimiento",
          ResourceType = typeof(Resources.CampoResource))]
        public int? CorregimientoSeleccionado { get; set; }

        public string DescripcionCorregimiento { get; set; }

        [Display(Name = "Calle",
          ResourceType = typeof(Resources.CampoResource))]
        [MaxLength(8000)]
        public string Calle { get; set; }


        public IEnumerable<SelectListItem> Corregimiento { get; set; }

    }
}