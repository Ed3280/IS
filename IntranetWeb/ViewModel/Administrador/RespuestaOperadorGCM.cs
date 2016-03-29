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
    public class RespuestaOperadorGCM
    {
        [Key]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        
        public string DescripcionTipoAtencion { get; set; }

        
        public IEnumerable<SelectListItem> TipoAtencion { get; set; } 
        
        [Display(Name = "Tipo Atención")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public int TipoAtencionSeleccionada { get; set; }


        [Display(Name = "Título")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MaxLength(30, ErrorMessage = "Título no puede exceder 30 caracteres")]
        public string Titulo { get; set; }

        [Display(Name = "Contenido")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MaxLength(160, ErrorMessage = "Contenido no puede exceder 160 caracteres")]
        public string Contenido { get; set; }

        [Display(Name = "Activo")]
        public bool InActivo { get; set; }
        
        [Display(Name = "Estatus")]
        public string DescripcionEstatus { get; set; }

        [Display(Name = "Fecha Estatus")]
        [DataType(DataType.DateTime, ErrorMessage = "Fecha de Estatus tiene un formato inválido"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = IntranetWeb.Core.Constante.AppFormat.dateHour)]
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime FeEstatus { get; set; }

        public string EditarRegistro { get; set; }

        /// <summary>
        /// Retorna el objeto RESPUESTA_OPERADOR_GCM dado su ViewModel
        /// </summary>
        /// <returns>Objeto dle tipo IntranetWeb.Models.RESPUESTA_OPERADOR_GCM</returns>
        public IntranetWeb.Models.RESPUESTA_OPERADOR_GCM toModel()
        {
            var respuestaOperadorGCM = new IntranetWeb.Models.RESPUESTA_OPERADOR_GCM();

            respuestaOperadorGCM.ID_RESPUESTA_OPERADOR = this.Id;
            respuestaOperadorGCM.IN_INACTIVO = !this.InActivo;
            respuestaOperadorGCM.DE_CONTENIDO = this.Contenido;
            respuestaOperadorGCM.DE_TITULO = this.Titulo;
            respuestaOperadorGCM.FE_ESTATUS = this.FeEstatus;
            respuestaOperadorGCM.TP_ATENCION = this.TipoAtencionSeleccionada;
            
            return respuestaOperadorGCM;
        }

    }
}