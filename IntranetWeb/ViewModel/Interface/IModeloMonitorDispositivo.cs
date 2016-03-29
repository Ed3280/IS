using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Interface
{
    public interface IModeloMonitorDispositivo
    {
        [Display(Name = "Dispositivo" ,ResourceType = typeof(Resources.CampoResource))]
        IEnumerable<int> DispositivosSeleccionados { get; set; }
        IEnumerable<SelectListItem> DispositivosDisponibles { get; set; }
    }
}