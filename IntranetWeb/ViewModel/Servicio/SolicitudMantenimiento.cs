using IntranetWeb.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Servicio
{
    public class SolicitudMantenimiento
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Documento cliente")]
        public string Documento { get; set; }

        [Display(Name = "Nombre cliente")]
        public string NombreCliente { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(CustomDateConverter))]
        [Display(Name = "Fecha solicitud")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime FechaSolicitud { get; set; }

        [Display(Name = "Estatus")]
        public string EstatusSolicitud { get; set; }

        [Display(Name = "Tipo mantenimiento")]
        public string TipoMantenimiento { get; set; }


        [JsonProperty]
        [JsonConverter(typeof(CustomDateConverter))]
        [Display(Name = "Fecha estatus")]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime FechaEstatus { get; set; }

        [Display(Name = "Vehiculo")]
        public string Vehiculo { get; set; }

        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        [Display(Name = "Dispositivo")]
        public string Dispositivo { get; set; }
        
        [Display(Name = "Kilometraje")]
        public Double Kilometraje { get; set; }

        public IEnumerable<Operacion> Operaciones { get; set; }

        public String EditarRegistro { get; set; }
    }
}