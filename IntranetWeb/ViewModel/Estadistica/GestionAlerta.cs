using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Estadistica
{
    public class GestionAlerta
    {
        
            public IEnumerable<SelectListItem> UsuarioAlerta { get; set; }
            [Display(Name = "UsuarioAlerta", ResourceType = typeof(Resources.CampoResource))]
            public int? UsuarioAlertaSeleccionado { get; set; }

            [DataType(DataType.Date, ErrorMessageResourceName = "FieldMustBeDate", ErrorMessageResourceType = typeof(Resources.ValidacionResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
            public DateTime? FechaNotificacionDesde { get; set; }
            [DataType(DataType.Date, ErrorMessageResourceName = "FieldMustBeDate", ErrorMessageResourceType = typeof(Resources.ValidacionResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
            public DateTime? FechaNotificacionHasta { get; set; }

            [DataType(DataType.Date, ErrorMessageResourceName = "FieldMustBeDate", ErrorMessageResourceType = typeof(Resources.ValidacionResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
            public DateTime? FechaRegistroDesde { get; set; }
            [DataType(DataType.Date, ErrorMessageResourceName = "FieldMustBeDate", ErrorMessageResourceType = typeof(Resources.ValidacionResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
            public DateTime? FechaRegistroHasta { get; set; }

            [Display(Name = "TipoNotificacion", ResourceType = typeof(Resources.CampoResource))]
            public int? TipoNotificacionSeleccionada { get; set; }

            public IEnumerable<SelectListItem> TipoNotificacion { get; set; }
                                    
            public string MensajeExito { get; set; }

        }
    
}