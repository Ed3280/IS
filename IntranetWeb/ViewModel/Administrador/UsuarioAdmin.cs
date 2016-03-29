using IntranetWeb.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Administrador
{
    public class UsuarioAdmin : Usuario, IValidatableObject
    {
        string numeroDocumento;
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MaxLength(50)]
        [Display(Name = "Documento", ResourceType = typeof(Resources.CampoResource))]
        [Remote("verificaNumeroDocumentoUsuarioNoUsado", "Validacion", AdditionalFields = "TipoDocumentoSeleccionado", ErrorMessageResourceName = "NumeroDeDocumentoEnUso", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public override string NumeroDocumento { get { return String.IsNullOrWhiteSpace(numeroDocumento) ? numeroDocumento : numeroDocumento.ToUpper(); } set { numeroDocumento = value; } }

        /// <summary>
        /// Validaciones del usuario
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
         public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            ValidacionController validador = new ValidacionController();
            JsonResult result;
            //Usuario nuevo
            if (Id == 0)
            {
                result = validador.verificaNombreUsuarioDisponible(NombreUsuario, Id);
                if (result.Data.GetType() == typeof(String))
                    yield return new ValidationResult((String)result.Data);

                result = validador.verificaNumeroDocumentoUsuarioNoUsado(TipoDocumentoSeleccionado, NumeroDocumento);

                if (result.Data.GetType() == typeof(String))
                    yield return new ValidationResult((String)result.Data);
            }

            //Se valida si el email es correcto
            result = validador.verificaCorreoUsuarioGeneralDisponible(Email
                                                               , Id);
            if (result.Data.GetType() == typeof(String))
                yield return new ValidationResult((String)result.Data);

            //Se valida si tiene al menos un rol
            if (RolesSeleccionados==null||RolesSeleccionados.Count() == 0)
                yield return new ValidationResult(Core.Constante.Mensaje.Error.AsigneRolUsuario);

        }
    }
}