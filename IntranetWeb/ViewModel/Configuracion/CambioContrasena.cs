using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Configuracion
{
    public class CambioContrasena
    {
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Key]
        public int Id { get; set; }

        [Display(Name = "Contraseña actual")]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Password)]
        public string ContrasenaActual { get; set; }

        [Display(Name ="Contraseña nueva")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [RegularExpression(@"((?=.*\d)(?=.*[a-zA-Z])(?=.*[@#$%\*]).{6,20})",ErrorMessage = "Contraseña debe contener entre 6 y 20 caracteres, debe contener al menos un dígito, un caracter alfanumérico y un caracter especial (@#$%*)")]
        [DataType(DataType.Password)]
        public string ContrasenaNueva { get; set; }

        [Display(Name = "Repetir contraseña nueva")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Password)]
        [Compare("ContrasenaNueva",ErrorMessage ="Las contraseñas no coinciden. Por favor verifique")]
        public string RepetirContrasena { get; set; }    
    }
}