using IntranetWeb.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Configuracion
{
    public class Perfil : IValidatableObject {

        public Perfil() {
            this.Usuario = new Usuario();
        }

         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]        
        public Usuario Usuario { get; set; }


        public Empleado Empleado { get; set; }



        /// <summary>
        /// Validaciones del usuario
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            ValidacionController validador = new ValidacionController();
            JsonResult result;

            //Se valida  si la dirección de correo ya se encuentra en uso
            
            result = validador.verificaCorreoPerfilUsuarioDisponible(  Usuario.Email
                                                                ,Usuario.Id);
            if(result.Data.GetType() == typeof(String))
                    yield return new ValidationResult((String)result.Data);
            
        }
    }
}