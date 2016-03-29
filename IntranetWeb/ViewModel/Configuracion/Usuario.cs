using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Configuracion
{
    public class Usuario
    {

        public Usuario() {          
        }

        [Key]
        public int Id { get; set; }


        [Display(Name = "Tipo de Documento")]
        public string TipoDocumentoIdentidad { get; set; }

        [Display(Name = "Número de Documento")]
        public string NumeroDocumentoIdentidad { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }


        [Display(Name = "Fecha de Nacimiento")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Date, ErrorMessage = "Fecha de nacimiento tiene un formato inválido"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? FechaNacimiento { get; set; }


        [Display(Name = "Contraseña actual")]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Password)]
        public string ContrasenaActual { get; set; }

        [Display(Name = "E-Mail Personal")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail Personal no es válido")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "E-mail Personal no es válido")]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Remote("verificaCorreoPerfilUsuarioDisponible", "Validacion", AdditionalFields ="Id",ErrorMessageResourceName = "CorreoUsuarioExistente", ErrorMessageResourceType =typeof(Resources.ErrorResource))]
        public string Email { get; set; }

        [Display(Name = "Teléfono Fijo")]
        [RegularExpression(@"\d{7}", ErrorMessage = "Ingrese un número de Téléfono Fijo válido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Teléfono Fijo Inválido")]
        public int? TelefonoFijo { get; set; }


        [Display(Name = "Teléfono Móvil")]
        [RegularExpression(@"\d{8}", ErrorMessage = "Ingrese un número de Teléfono Móvil válido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Número Móvil Inválido")]
        public int? TelefonoMovil { get; set; }


        public IntranetWeb.Models.USUARIO toModel()
        {
            var usuario = new IntranetWeb.Models.USUARIO();
            usuario.CD_USUARIO              = this.Id;
            usuario.DE_NOMBRE_APELLIDO      = this.Nombre;
            usuario.DI_EMAIL_USUARIO        = this.Email;
            usuario.FE_NACIMIENTO           = this.FechaNacimiento;
            usuario.NU_DOCUMENTO_IDENTIDAD  = this.NumeroDocumentoIdentidad;
            usuario.TP_DOCUMENTO_IDENTIDAD  = this.TipoDocumentoIdentidad;
            usuario.NU_TELEFONO_FIJO        = this.TelefonoFijo;
            usuario.NU_TELEFONO_MOVIL       = this.TelefonoMovil;
            return usuario;
        }

    }
}