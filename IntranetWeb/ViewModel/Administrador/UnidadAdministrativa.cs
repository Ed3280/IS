using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Administrador
{
    public class UnidadAdministrativa
    {
        [Key]
        [Display(Name ="Id")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MaxLength(150, ErrorMessage ="Nombre no puede exceder los 150 caracteres")]
        public string Nombre { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Display(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; } 
        
        public string EditarRegistro { get; set; }


        public UNIDAD_ADMINISTRATIVA toModel() {
            var ua = new UNIDAD_ADMINISTRATIVA();

            ua.CD_UNIDAD_ADMINISTRATIVA = Id;
            ua.NM_UNIDAD_ADMINISTRATIVA = Nombre;
            ua.FE_CREACION = FechaCreacion;

            return ua;


        }


    }
}