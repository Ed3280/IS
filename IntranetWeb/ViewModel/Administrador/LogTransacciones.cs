using IntranetWeb.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Administrador
{
    public class LogTransacciones
    {
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Key]
        [Display(Name = "Id")]
        public long Id { get; set; } 
        public int CodigoUsuario { get; set; }

        [Display(Name = "Usuario")]
        public String NombreUsuario { get; set; }

        [Display(Name = "Fecha Operación")]
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime FechaOperacion { get; set; }
        [Display(Name = "Ip")]
        public string Ip { get; set; }

        [Display(Name = "Cotrolador")]
        public string Controlador { get; set; }

        [Display(Name = "Acción")]
        public string Accion { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "Nivel")]
        public string DescripcionNivel { get; set; }

        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; }

        [Display(Name = "Excepción")]
        public string Excepcion { get; set; }
        
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime FechaBusquedaDesde { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime FechaBusquedaHasta { get; set; }
        
        public IEnumerable<SelectListItem> Usuario { get; set; }
        [Display(Name = "Usuario")]
        public int UsuarioSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Nivel { get; set; }
        [Display(Name = "Nivel")]
        public string NivelSeleccionado { get; set; }
        


        public string EditarRegistro { get; set; }
    }
}