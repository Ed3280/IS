using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Kilometraje
{
    public class KilometrajeTotal
    {
        [Display(Name = "Distribuidor")]
        public String UsuarioIdSeleccionado { get; set; }
        public IEnumerable<SelectListItem> UsuarioId { get; set; }

        public Boolean? IndicadorNoEdicion { get; set; }

    }
}