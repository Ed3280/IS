using IntranetWeb.Controllers;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Administrador
{
    public class Cargo : IValidatableObject, IModeloRol
    {

        [Key]
        [Display(Name = "Id",
        ResourceType = typeof(Resources.CampoResource))]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource)
            , ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "NombreCargo",
        ResourceType = typeof(Resources.CampoResource))]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [Display(Name = "NombreUnidadAdministrativa",
        ResourceType = typeof(Resources.CampoResource))]
        public string NombreUnidadAdministrativa { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource)
                , ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "NombreUnidadAdministrativa",
        ResourceType = typeof(Resources.CampoResource))]
        public int UnidadAdministrativaSeleccionada { get; set; }

        public IEnumerable<SelectListItem> UnidadAdministrativa { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource)
                , ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "ListaRol",
        ResourceType = typeof(Resources.CampoResource))]
        public IEnumerable<int> RolesSeleccionados { get; set; }

        public IEnumerable<SelectListItem> RolesDisponibles { get; set; }

        
        [Display(Name = "DescripcionCargo",
        ResourceType = typeof(Resources.CampoResource))]
        [MaxLength(800)]
        public string DescripcionCargo { get; set; }

        [Display(Name = "CargoPadre",
        ResourceType = typeof(Resources.CampoResource))]
        public int? CargoPadreSeleccionado { get; set; }
        public IEnumerable<SelectListItem> CargoPadre { get; set; }
        
        public CARGO toModel() {
            CARGO cargo = new CARGO();
            cargo.CD_CARGO = Id;
            cargo.CD_UNIDAD_ADMINISTRATIVA = UnidadAdministrativaSeleccionada;
            cargo.NM_CARGO = Nombre;
            cargo.DE_CARGO = DescripcionCargo;
            cargo.CD_CARGO_PADRE = CargoPadreSeleccionado;

            return cargo;

        }

        /// <summary>
        /// Validaciones del objeto cargo
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ValidacionController validador = new ValidacionController();
            JsonResult result;

            if (Id != 0) {
                result = validador.cargoPadreActualizables( Id
                                                                ,CargoPadreSeleccionado);
                if (result.Data.GetType() == typeof(String))
                    yield return new ValidationResult((String)result.Data);
            }            
        }
    }
}