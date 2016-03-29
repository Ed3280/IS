using IntranetWeb.Controllers;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Administrador
{
    public class Usuario : IValidatableObject, IModeloRol, IModeloAplicacion , IModeloMonitorDispositivo
    {

        [Key]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public int Id { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MinLength(5)]
        [MaxLength(50)]        
        [Display(Name = "NombreUsuario", ResourceType = typeof(Resources.CampoResource))]
        [Remote("verificaNombreUsuarioDisponible","Validacion", AdditionalFields = "Id", ErrorMessageResourceName = "NombreUsuarioInvalido" , ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [RegularExpression(@"^[a-zA-Z0-9_.]*$", ErrorMessageResourceName = "FormatoNombreUsuarioInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource) )]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "TipoDocumento", ResourceType = typeof(Resources.CampoResource))]
         public string TipoDocumentoSeleccionado { get; set; }

        public IEnumerable<SelectListItem> TipoDocumento { get; set; }

        string numeroDocumento;
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MaxLength(50)]
        [Display(Name = "Documento", ResourceType = typeof(Resources.CampoResource))]
        [Remote("verificaNumeroDocumentoUsuarioInternoNoUsado", "Validacion", AdditionalFields = "TipoDocumentoSeleccionado", ErrorMessageResourceName = "NumeroDeDocumentoEnUso", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public virtual string NumeroDocumento { get { return String.IsNullOrWhiteSpace(numeroDocumento) ? numeroDocumento : numeroDocumento.ToUpper(); } set { numeroDocumento = value; } }

        [Display(Name = "Documento", ResourceType = typeof(Resources.CampoResource))]
        public string Documento
        {
            get { return (string.IsNullOrWhiteSpace(TipoDocumentoSeleccionado)?"":TipoDocumentoSeleccionado+"-")
                        + (!String.IsNullOrWhiteSpace(NumeroDocumento) ? NumeroDocumento : "" ); }
        }
        
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "NombreApellido", ResourceType = typeof(Resources.CampoResource))]
        [MaxLength(300)]
        public string NombreApellido { get; set; }

        [Display(Name = "Contrasena", ResourceType = typeof(Resources.CampoResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [RegularExpression(@"((?=.*\d)(?=.*[a-zA-Z])(?=.*[@#$%\*]).{6,20})", 
            ErrorMessageResourceName = "FormatoContrasenaInvalida"
            , ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [DataType(DataType.Password)]
        public string ContrasenaNueva { get; set; }

        [Display(Name = "RepetirContrasena", ResourceType = typeof(Resources.CampoResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("ContrasenaNueva", ErrorMessageResourceName = "ContrasenaNoCoincide", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string RepetirContrasena { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(CustomDateConverter))]
        [Display(Name = "FechaNacimiento", ResourceType = typeof(Resources.CampoResource))]
        [DataType(DataType.Date, ErrorMessageResourceName = "FormatoFechaNacimientoInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource)), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Dispositivo",
        ResourceType = typeof(Resources.CampoResource))]
        public IEnumerable<int> DispositivosSeleccionados { get; set; }

        public IEnumerable<SelectListItem> DispositivosDisponibles { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources.CampoResource))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "FormatoEmailInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessageResourceName = "FormatoEmailInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Remote("verificaCorreoUsuarioGeneralDisponible", "Validacion", AdditionalFields = "Id", ErrorMessageResourceName = "CorreoUsuarioExistente", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Email { get; set; }
        
        [Display(Name = "NumeroTelefonoFijo" , ResourceType = typeof(Resources.CampoResource))]
        [RegularExpression(@"\d{7}", ErrorMessageResourceName = "FormatoTenefonoFijoInvalido" , ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "TelefonoFijoInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public int? TelefonoFijo { get; set; }
        
        [Display(Name = "NumeroMovil", ResourceType = typeof(Resources.CampoResource) )]
        [RegularExpression(@"\d{8}", ErrorMessageResourceName = "TelefonoMovilInvalido" , ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "TelefonoMovilInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public int? TelefonoMovil { get; set; }
        
        [Display(Name = "Activo", ResourceType = typeof(Resources.CampoResource))]
        public bool IndicadorActivo { get; set; }
        
        public string DescripcionEstatus { get; set; }
        
        [Display(Name = "Bloqueado", ResourceType = typeof(Resources.CampoResource))]
        public bool IndicadorBloqueado { get; set; }
    
        public string DescripcionBloqueado { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Display(Name = "FechaCreacion" , ResourceType = typeof(Resources.CampoResource))]
        public DateTime FechaCreacion { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Display(Name = "FechaEstatus", ResourceType = typeof(Resources.CampoResource))]
        public DateTime FechaEstatus { get; set; }

        public bool IndicadorRegistroNoEditable { get; set; }
        
        public int? CodigoUsuarioPadre { get; set; }

        public Empleado DatosEmpleado { get; set; }

        [Display(Name = "ListaRol", ResourceType = typeof(Resources.CampoResource))]
        public IEnumerable<int> RolesSeleccionados { get; set; }
        public IEnumerable<SelectListItem> RolesDisponibles { get; set; }

        public IList<Aplicacion> Aplicaciones { get; set;}
        
        public string EditarRegistro { get; set; }



        public USUARIO toModel() {

            USUARIO usuario = new USUARIO();
            AuthRepositorio authRepo        = new AuthRepositorio();
            usuario.CD_USUARIO              = Id;
            usuario.DE_CONTRASENA           = authRepo.encriptaCadena(ContrasenaNueva);
            usuario.DE_NOMBRE_APELLIDO      = NombreApellido;
            usuario.DI_EMAIL_USUARIO        = Email;
            usuario.FE_CREACION             = FechaCreacion;
            usuario.FE_ESTATUS              = FechaEstatus;
            usuario.FE_NACIMIENTO           = FechaNacimiento;
            usuario.IN_USUARIO_INACTIVO     = !IndicadorActivo;
            usuario.IN_USUARIO_BLOQUEADO    = IndicadorBloqueado;
            usuario.NM_USUARIO              = NombreUsuario;
            usuario.NU_DOCUMENTO_IDENTIDAD  = NumeroDocumento;
            usuario.NU_TELEFONO_FIJO        = TelefonoFijo;
            usuario.NU_TELEFONO_MOVIL       = TelefonoMovil;
            usuario.TP_DOCUMENTO_IDENTIDAD  = TipoDocumentoSeleccionado;
            
            return usuario;
        }


        /// <summary>
        /// Validaciones del usuario
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext){

            ValidacionController validador = new ValidacionController();
            JsonResult result;
            //Usuario nuevo
            if (Id == 0){                
               result = validador.verificaNombreUsuarioDisponible(NombreUsuario,Id);
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
            
        }
    }
}