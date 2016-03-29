using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Estadistica
{
    public class GestionTicket
    {
        public IEnumerable<SelectListItem> UsuarioTicket { get; set; }
        [Display(Name = "UsuarioTicket", ResourceType = typeof(Resources.CampoResource))]
        public int? UsuarioTicketSeleccionado { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceName = "FieldMustBeDate", ErrorMessageResourceType = typeof(Resources.ValidacionResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? FechaStatusDesde { get; set; }
        [DataType(DataType.Date, ErrorMessageResourceName = "FieldMustBeDate", ErrorMessageResourceType = typeof(Resources.ValidacionResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? FechaStatusHasta { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceName = "FieldMustBeDate", ErrorMessageResourceType = typeof(Resources.ValidacionResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? FechaAperturaDesde { get; set; }
        [DataType(DataType.Date, ErrorMessageResourceName = "FieldMustBeDate", ErrorMessageResourceType = typeof(Resources.ValidacionResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? FechaAperturaHasta { get; set; }
        
        [Display(Name = "TipoNotificacion", ResourceType = typeof(Resources.CampoResource))]
        public int? TipoNotificacionSeleccionada { get; set; }

        public IEnumerable<SelectListItem> TipoNotificacion { get; set; }

        public IEnumerable<SelectListItem> TipoAtencion { get; set; }

        [Display(Name = "TipoCaso", ResourceType = typeof(Resources.CampoResource))]
        public int? TipoAtencionSeleccionada { get; set; }

        public IEnumerable<SelectListItem> Estatus { get; set; }
        [Display(Name = "Estatus", ResourceType = typeof(Resources.CampoResource))]
        public int? EstatusSeleccionado { get; set; }
        
        public string MensajeExito { get; set; }

    }
}