using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Interface
{
    public interface IModeloRol
    {
        [Display(Name = "Rol", ResourceType = typeof(Resources.CampoResource))]
        IEnumerable<int> RolesSeleccionados { get; set; }
        IEnumerable<SelectListItem> RolesDisponibles { get; set; }
    }
}