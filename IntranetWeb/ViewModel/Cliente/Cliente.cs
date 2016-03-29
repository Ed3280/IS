using IntranetWeb.Controllers;
using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Administrador;
using IntranetWeb.ViewModel.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Cliente
{
    public class Cliente : IValidatableObject, IModeloAplicacion
    {

        public Cliente() {
            Direccion = new Direccion();
            Dispositivos = new List<Dispositivo>();
            Vehiculos = new List<Vehiculo>();
            Aplicaciones = new List<Aplicacion>();
        }

        [Key]
        [Display(Name = "Id",
        ResourceType = typeof(Resources.CampoResource))]
        public int Id { get; set; }

        public long? Consecutivo { get; set; }

        public string Agencia { get; set; }
                
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "TipoDocumento", ResourceType = typeof(Resources.CampoResource))]
        public string TipoDocumentoSeleccionado { get; set; }

        public IEnumerable<SelectListItem> TipoDocumento { get; set; }


        [Display(Name = "NombreUsuario", ResourceType = typeof(Resources.CampoResource))]
        public string NombreUsuario { get; set; }

        private string cedula;
        [Display(Name = "Cedula"
            , ResourceType = typeof(Resources.CampoResource))]
        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Remote("verificaNumeroDocumentoClienteNoUsado", "Validacion", AdditionalFields = "TipoDocumentoSeleccionado", ErrorMessageResourceName = "NumeroDeDocumentoEnUso", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Cedula { get { return String.IsNullOrWhiteSpace(cedula) ? cedula : cedula.ToUpper(); } set { cedula = value; } }

        [Display(Name = "Documento", ResourceType = typeof(Resources.CampoResource))]
        public string Documento
        {
            get
            {
                return (string.IsNullOrWhiteSpace(this.TipoDocumentoSeleccionado) ? "" : this.TipoDocumentoSeleccionado + "-")
                      + (!String.IsNullOrWhiteSpace(Cedula)?Cedula:"");
            }
        }
        
        private string nombre;
        [MaxLength(250)]
        [Display(Name = "Nombre",
          ResourceType = typeof(Resources.CampoResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public string Nombre { get { return String.IsNullOrWhiteSpace(nombre) ? nombre : nombre.ToUpper(); } set { nombre = value; } }

        private string apellido;
        [MaxLength(250)]
        [Display(Name = "Apellido",
          ResourceType = typeof(Resources.CampoResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public string Apellido { get { return String.IsNullOrWhiteSpace(apellido) ? apellido : apellido.ToUpper(); } set { apellido = value; } }

        
        private string nombreApellidoRazonSocial;
        [MaxLength(500)]
        [Display(Name = "Nombre", ResourceType = typeof(Resources.CampoResource))]
        public string NombreApellidoRazonSocial
        {
            get { return String.IsNullOrWhiteSpace(nombreApellidoRazonSocial) ? (((String.IsNullOrWhiteSpace(Nombre) ? "" : Nombre) + " " + (String.IsNullOrWhiteSpace(Apellido) ? "" : Apellido)).Trim()) : nombreApellidoRazonSocial; }

            set { nombreApellidoRazonSocial = value; }
        }




        public IList<Aplicacion> Aplicaciones { get; set; }
        
        private string nombreApellidoUsuario;
        public string NombreApellidoUsuario {
            private get { return  String.IsNullOrWhiteSpace(nombreApellidoUsuario)?( (  (String.IsNullOrWhiteSpace(Nombre)?"": Nombre) + " "+ (String.IsNullOrWhiteSpace(Apellido)?"":Apellido)).Trim()):nombreApellidoUsuario; }

            set {nombreApellidoUsuario = value;

                string[] NombreAp = nombreApellidoUsuario.Split(" ".ToCharArray(), 2);
                if (String.IsNullOrWhiteSpace(Nombre)&&String.IsNullOrWhiteSpace(Apellido)&&NombreAp.Length > 1)
                {
                    Nombre = NombreAp[0];
                    Apellido = NombreAp[1];
                }
                else
                    Nombre = NombreAp[0];
            }
        } 
        
        [Display(Name = "Email",
          ResourceType = typeof(Resources.CampoResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail no es válido")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "E-mail no es válido")]
        [Remote("verificaCorreoUsuarioGeneralDisponible", "Validacion", AdditionalFields = "Id", ErrorMessageResourceName = "CorreoUsuarioExistente", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Email { get; set; }
        
        [Display(Name ="Sims", 
            ResourceType = typeof(Resources.CampoResource))]
        public string ListadoSims  { get; set; }
               
        [JsonProperty]
        [JsonConverter(typeof(CustomDateConverter))]
        [Display(Name = "FechaNacimiento", ResourceType = typeof(Resources.CampoResource))]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? FechaNacimiento { get; set; }
        
        [Display(Name = "NumeroMovil",
          ResourceType = typeof(Resources.CampoResource))]
        [RegularExpression(@"\d{8}", ErrorMessage = "Ingrese un número de Móvil válido")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Número Móvil Inválido")]
        public int? NumeroMovil { get; set; }
        
        [Display(Name = "NumeroTelefonoFijo",
          ResourceType = typeof(Resources.CampoResource))]
        [RegularExpression(@"\d{7}", ErrorMessage = "Ingrese un número de Téléfono Fijo válido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Teléfono Fijo Inválido")]
        public int? NumeroTelefonoFijo { get; set; }
        
        [Display(Name = "NumeroTelefonoOficina",
          ResourceType = typeof(Resources.CampoResource))]
        [RegularExpression(@"\d{7}", ErrorMessage = "Ingrese un número de Téléfono de Oficina válido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Teléfono de Oficina Inválido")]
        public int? NumeroTelefonoOficina { get; set; }

        [MaxLength(5, ErrorMessage = "Tipo de sangre no debe exceder los 5 caracteres")]
        [Display(Name = "TipoSangre",
          ResourceType = typeof(Resources.CampoResource))]
        public string TipoSangreSeleccionado { get; set; }

        public IEnumerable<SelectListItem> TipoSangre { get; set; }
        
        [Display(Name = "PersonaContacto",
          ResourceType = typeof(Resources.CampoResource))]
        public string PersonaContacto { get; set; }

        [Display(Name = "NumeroTelefonoPersonaContacto",
          ResourceType = typeof(Resources.CampoResource))]
        [RegularExpression(@"\d{7,8}", ErrorMessage = "Ingrese teléfono de persona de contacto válido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Teléfono de persona de contacto Inválido")]
        public int? NumeroTelefonoPersonaContacto {get; set;}
                
        public IEnumerable<SelectListItem> CompaniaSeguro { get; set; }

        [Display(Name = "CompaniaSeguro",
          ResourceType = typeof(Resources.CampoResource))]
        public int? CompaniaSeguroSeleccionada { get; set; }
        
        public string DescripcionCompaniaSeguro { get; set; }

        [Display(Name = "Activo",
          ResourceType = typeof(Resources.CampoResource))]
        public bool IndicadorEstaActivo { get; set; }

        [Display(Name = "IndicadorEstaRegistrado",
          ResourceType = typeof(Resources.CampoResource))]
        public bool IndicadorEstaRegistrado { get; set; }
        
        public bool IndicadorCorreoRegistroEnviado { get; set; }

        [Display(Name = "SocioComercial",
           ResourceType = typeof(Resources.CampoResource))]
        public IEnumerable<int> SocioComercialSeleccionados { get; set; }

        public IEnumerable<SelectListItem> SocioComercialDisponibles { get; set; }

        [Display(Name = "Estatus",
          ResourceType = typeof(Resources.CampoResource))]
        public string DescripcionEstaActivo { get; set; }

        public string EditarRegistro { get; set; }

        public Direccion Direccion { get; set; }

        public IList<Dispositivo> Dispositivos { get; set; }

        public IList<Vehiculo> Vehiculos { get; set;}
        
        public CLIENTE toModel(USUARIO usuario) {
            DateTime? fechaActual = DateTime.Now;

            CLIENTE cliente = new CLIENTE();
            cliente.USUARIO = usuario==null?new USUARIO(): usuario;
            cliente.CD_CLIENTE = Id;
            cliente.DE_NOMBRE = Nombre;
            cliente.DE_APELLIDO = Apellido;
            cliente.DE_NOMBRE_APELLIDO_RAZON_SOCIAL = NombreApellidoRazonSocial;
            cliente.FE_NACIMIENTO = FechaNacimiento;
            cliente.CD_PAIS = Direccion.PaisSeleccionado;
            cliente.CD_PROVINCIA = Direccion.ProvinciaSeleccionada;
            cliente.CD_DISTRITO = Direccion.MunicipioSeleccionado;
            cliente.CD_CORREGIMIENTO = Direccion.CorregimientoSeleccionado;
            cliente.DI_COBRO = Direccion.Calle;
            cliente.NU_TELEFONO_OFICINA = NumeroTelefonoOficina;
            cliente.TP_SANGRE = TipoSangreSeleccionado;
            cliente.NM_PERSONA_CONTACTO = PersonaContacto;
            cliente.NU_TELEFONO_MOVIL_PERSONA_CONTACTO = NumeroTelefonoPersonaContacto;
            cliente.CD_COMPANIA_SEGURO = CompaniaSeguroSeleccionada;
            cliente.IN_INACTIVO = !IndicadorEstaActivo;
            cliente.FE_INACTIVO = IndicadorEstaActivo ? null : fechaActual;
            cliente.FE_ESTATUS = (DateTime)fechaActual;
            
            cliente.USUARIO.TP_DOCUMENTO_IDENTIDAD  = TipoDocumentoSeleccionado;
            cliente.USUARIO.NU_DOCUMENTO_IDENTIDAD  = Cedula;
            cliente.USUARIO.NU_TELEFONO_FIJO        = NumeroTelefonoFijo;
            cliente.USUARIO.CD_USUARIO              = cliente.CD_CLIENTE;
            cliente.USUARIO.DE_NOMBRE_APELLIDO      = NombreApellidoUsuario;
            cliente.USUARIO.DI_EMAIL_USUARIO        = Email;
            cliente.USUARIO.FE_NACIMIENTO           = FechaNacimiento;

            return cliente;
        }


        /// <summary>
        /// Validaciones del usuario
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext){

            ValidacionController validador = new ValidacionController();
            JsonResult result;
            //Usuario nuevo
            if (Id == 0 ){

                //Se verifica si el usuario ya es un cliente
                result = validador.verificaNumeroDocumentoClienteNoUsado(TipoDocumentoSeleccionado, Cedula);

                if (result.Data.GetType() == typeof(String))
                    yield return new ValidationResult((String)result.Data);
               
            }

            result = validador.verificaCorreoUsuarioGeneralDisponible(TipoDocumentoSeleccionado, Id);

            if (result.Data.GetType() == typeof(String))
                yield return new ValidationResult((String)result.Data);
        }
    }
}