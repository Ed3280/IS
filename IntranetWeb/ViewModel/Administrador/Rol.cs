using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Administrador
{
    public class Rol : IModeloAplicacion
    {
         [Key]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
         [Display(Name = "Id"
                , ResourceType = typeof(Resources.CampoResource))]

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Rol"
            , ResourceType = typeof(Resources.CampoResource)
            )]
        [MaxLength(200)]
        public string Nombre { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName ="PropertyValueRequired")]
        [Display(Name = "Descripcion"
            ,ResourceType = typeof(Resources.CampoResource) )]
        [MaxLength(600)]
        public string Descripcion { get; set; }

        [Display(Name = "Activo"
            , ResourceType = typeof(Resources.CampoResource))]
        public bool IndicadorActivo { get; set; }
        
        public string DescripcionEstatus { get; set; }
        
        [JsonProperty]
        [JsonConverter(typeof(CustomDateConverter))]
            [Display(Name = "FechaEstatus"
            , ResourceType = typeof(Resources.CampoResource))]
        public DateTime FechaEstatus { get; set; }


        [JsonProperty]
        [JsonConverter(typeof(CustomDateConverter))]
        [Display(Name = "FechaCreacion"
            , ResourceType = typeof(Resources.CampoResource))]
        public DateTime FechaCreacion { get; set; }

        public bool IndicadorSeleccionado { get; set; }

        public string EditarRegistro { get; set; }

        public IList<Aplicacion> Aplicaciones { get; set; }

        public ROL toModel() {
            ROL rol = new ROL();

            rol.CD_ROL = Id;
            rol.NM_ROL = Nombre;
            rol.DE_ROL = Descripcion;
            rol.IN_ROL_ACTIVO = IndicadorActivo;
            
            return rol;
        }
    }
}