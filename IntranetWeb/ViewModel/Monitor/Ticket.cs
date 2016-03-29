using IntranetWeb.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Monitor
{
    public class Ticket
    {
        public Ticket() {
            this.Distribuidor = new Distribuidor();

            this.Dispositivo = new Dispositivo();

            this.Cliente = new UsuarioMovil();
        }

        [Key]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Nº de ticket")]
        public int Id { get; set; }
        public Distribuidor Distribuidor { get; set; }
        
        public Dispositivo Dispositivo { get; set; }

        public UsuarioMovil Cliente { get; set; }
        
        public IEnumerable<SelectListItem> MensajeAlCliente { get; set; }

        [Display(Name = "Mensaje al cliente")]
        public int? MensajeAlClienteSeleccionado { get; set; }

        public string  DescripcionMensajeAlCliente { get; set; }
        
        [Display(Name = "Observación agente")]
        [MaxLength(800, ErrorMessage = "Observación Agente no debe exceder los 800 caracteres")]
        public String Observacion { get; set; }
        
        [Display(Name = "Observación supervisor")]
        [MaxLength(800, ErrorMessage = "Observación Supervisor no debe exceder los 800 caracteres")]

        public String ObservacionSupervisor { get; set; }
                
        public IEnumerable<SelectListItem> TipoAtencion { get; set; }

        public int? TipoAtencionSeleccionada { get; set; }

        [Display(Name = "Tipo de caso")]
        public string DescripcionTipoAtencion { get; set; }
        
        public IEnumerable<SelectListItem> UsuarioTransferencia { get; set; }
        [Display(Name = "Transferir a")]
        public int? UsuarioTransferenciaSeleccionado { get; set; }
        
        public IEnumerable<SelectListItem> UsuarioTicket { get; set; }
        [Display(Name = "Usuario ticket")]
        public int UsuarioTicketSeleccionado { get; set; }

        [Display(Name = "Usuario ticket")]
        public String NombreUsuarioTicket { get; set; }

        [Display(Name = "Transferido por")]
        public string NombreUsuarioTransferencia { get; set; }
        
        public int TipoNotificacionSeleccionada { get; set; }

        [Display(Name = "Tipo notificación")]
        public String DescripcionTipoNotificacion { get; set; }

        public int OrigenNotificacionSeleccionada { get; set; }

        [Display(Name = "Origen notificación")]
        public string DescripcionOrigenNotificacion { get; set; }
        

        [Display(Name = "Estatus")]
        public String DescripcionEstatus { get; set; }


        public IEnumerable<SelectListItem> Estatus { get; set; }
        [Display(Name = "Estatus")]
        public int EstatusSeleccionado { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime FechaBusquedaDesde { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime FechaBusquedaHasta { get; set; }


        [Display(Name = "Fecha apertura")]
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime FechaCreacion { get; set; }


        [Display(Name = "Fecha notificación")]
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime FechaNotificacion { get; set; }
        
        [Display(Name = "Fecha estatus")]
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime FechaEstatus { get; set; }
        
        public bool InMensajeRespuestaEnviado { get; set; }

        [Display(Name ="¿Enviar mensaje al cliente?")]
        public bool EnviarRespuestaCliente { get; set; }

        public String EditarRegistro { get; set; }

        public String ActionCrear { get; set; }
        public String ActionConsultar { get; set; }
        public String ActionEditar { get; set; }
        public String ActionListar { get; set; }
    }
}