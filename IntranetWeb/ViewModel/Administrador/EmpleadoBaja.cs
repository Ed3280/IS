using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Administrador
{
    public class EmpleadoBaja : IValidatableObject
    {
        [Key]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public int Id { get; set; }

        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Nombre y Apellido")]
        public string NombreApellido { get; set; }

        [Display(Name = "Unidad Administrativa")]
        public string UnidadAdministrativa { get; set; }

        [Display(Name = "Cargo")]
        public string Cargo { get; set; }     


         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Fecha de Retiro")]
        [DataType(DataType.Date, ErrorMessage = "Fecha de Retiro tiene un formato inválido"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? FechaRetiro { get; set; }

        public bool IndicadorRequiereReeemplazo { get; set; }

       
        [Display(Name = "Reemplazo")]
        public int? SupervisorReemplazoSeleccionado { get; set; }

        public IEnumerable<SelectListItem> SupervisorReemplazo { get; set; }

        public int CodigoCargo { get; set; }

        /// <summary>
        /// Validaciones para dar de baja
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext){
            
            if (IndicadorRequiereReeemplazo && SupervisorReemplazoSeleccionado == null)
                yield return new ValidationResult(Core.Constante.Mensaje.Error.EmpleadoReemplazoObligatorio);

        }
    }
  }