using IntranetWeb.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Monitor
{
    public class NuevoTicket
    {

        public IEnumerable<SelectListItem> Cliente { get; set; }

        [Display(Name="Cliente")]
        public int? ClienteSeleccionado { get; set; }


        public IEnumerable<SelectListItem> Dispositivo { get; set; }
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Dispositivo")]
        public int DispositivoSeleccionado { get; set; }


        public IEnumerable<SelectListItem> OrigenNotificacion { get; set; }

         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Origen Notificación")]
        public int OrigenNotificacionSeleccionado { get; set; }


         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Fecha de Notificación")]
        [DataType(DataType.DateTime, ErrorMessage = "Fecha de notificación tiene un formato inválido"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = IntranetWeb.Core.Constante.AppFormat.dateHour)]
        public DateTime FechaNotificacion { get; set; }
        
    }
}