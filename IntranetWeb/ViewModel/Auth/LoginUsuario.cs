using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Auth
{
    public class LoginUsuario
    {
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display( Name = "NombreUsuarioCorreo", ResourceType = typeof(Resources.CampoResource))]
        [MaxLength(50, ErrorMessage = "Nombre de Usuario no puede exceder los 50 caracteres")]
        public string UserName { get; set; }


         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "NombreUsuarioCorreo", ResourceType = typeof(Resources.CampoResource))]
        [MaxLength(50, ErrorMessage = "Nombre de Usuario no puede exceder los 50 caracteres")]
        public string UserNameRecuperacion { get; set; }

         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Contrasena", ResourceType = typeof(Resources.CampoResource))]
        public string Password { get; set; }

        [HiddenInput]
        public String returnURL { get; set; }


    }
}