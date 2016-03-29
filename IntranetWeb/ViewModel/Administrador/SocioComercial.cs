using IntranetWeb.Controllers;
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
    public class SocioComercial : IValidatableObject, IModeloAplicacion, IModeloMonitorDispositivo
    {

        public SocioComercial() {
            Aplicaciones = new List<Aplicacion>();
        }

        [Key]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public int Id { get; set; }

        private string nombre;
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Nombre", ResourceType = typeof(Resources.CampoResource) )]
        [MaxLength(500)]
        public string Nombre { get { return String.IsNullOrWhiteSpace(nombre) ? nombre : nombre.ToUpper(); } set { nombre = value; } }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "TipoDocumento", ResourceType = typeof(Resources.CampoResource))]
        public string TipoDocumentoSeleccionado { get; set; }

        public IEnumerable<SelectListItem> TipoDocumento { get; set; }

        string numeroDocumento;
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MaxLength(50)]
        [Display(Name = "Documento", ResourceType = typeof(Resources.CampoResource))]
        [Remote("verificaNumeroDocumentoSocioComercialNoUsado", "Validacion", AdditionalFields = "TipoDocumentoSeleccionado", ErrorMessageResourceName = "NumeroDeDocumentoSocioComercialEnUso", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string NumeroDocumento { get { return String.IsNullOrWhiteSpace(numeroDocumento) ? numeroDocumento : numeroDocumento.ToUpper(); } set { numeroDocumento = value; } }

        [Display(Name = "Documento", ResourceType = typeof(Resources.CampoResource))]
        public string Documento
        {
            get
            {
                return (string.IsNullOrWhiteSpace(this.TipoDocumentoSeleccionado) ? "" : this.TipoDocumentoSeleccionado + "-")
                      + (!String.IsNullOrWhiteSpace(NumeroDocumento)?NumeroDocumento:"");
            }

        }
        [Display(Name = "NombreUsuario", ResourceType = typeof(Resources.CampoResource))]
        public string NombreUsuario { get; set; }

        public string DescripcionSegmentoNegocio { get; set; }

        [Display(Name = "NumeroTelefonoFijo", ResourceType = typeof(Resources.CampoResource))]
        [RegularExpression(@"\d{7}", ErrorMessageResourceName = "FormatoTenefonoFijoInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "TelefonoFijoInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public int? TelefonoFijo { get; set; }

        [Display(Name = "NumeroMovil", ResourceType = typeof(Resources.CampoResource))]
        [RegularExpression(@"\d{8}", ErrorMessageResourceName = "TelefonoMovilInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "TelefonoMovilInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public int? TelefonoMovil { get; set; }


        [Display(Name = "Email", ResourceType = typeof(Resources.CampoResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "FormatoEmailInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessageResourceName = "FormatoEmailInvalido", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Remote("verificaCorreoUsuarioGeneralDisponible", "Validacion", AdditionalFields = "Id", ErrorMessageResourceName = "CorreoUsuarioExistente", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> SegmentoNegocio { get; set; }
        
        [Display(Name ="SegmentoNegocio", 
                ResourceType =typeof(Resources.CampoResource))]
        public int SegmentoNegocioSeleccionado { get; set; }

        public bool IndicadorCorreoRegistroEnviado { get; set; }

        public IEnumerable<SelectListItem> Distribuidor { get; set; }

        [Display(Name = "DistribuidorPlataforma",
                ResourceType = typeof(Resources.CampoResource))]
        public int? DistribuidorSeleccionado { get; set; }


        public string DescripcionDistribuidor { get; set; }

        [Display(Name = "ListaServiciosApp",
            ResourceType = typeof(Resources.CampoResource))]
        public IEnumerable<int> ServiciosAppSeleccionados { get; set; }

        public IEnumerable<SelectListItem> ServiciosAppDisponibles { get; set; }

        [Display(Name = "Dispositivo",
                ResourceType = typeof(Resources.CampoResource))]
        public IEnumerable<int> DispositivosSeleccionados { get; set; }

        public IEnumerable<SelectListItem> DispositivosDisponibles { get; set; }



        public IEnumerable<SelectListItem> Pais { get; set; }

        [Display(Name = "Pais",
         ResourceType = typeof(Resources.CampoResource))]
        public int PaisSeleccionado { get; set; }

        public string DescripcionPais { get; set; }

        [Display(Name = "Provincia",
          ResourceType = typeof(Resources.CampoResource))]
        public int ProvinciaSeleccionada { get; set; }

        public string DescripcionProvincia { get; set; }

        public IEnumerable<SelectListItem> Provincia { get; set; }

        [Display(Name = "Distrito",
          ResourceType = typeof(Resources.CampoResource))]
        public int DistritoSeleccionado { get; set; }

        public string DescripcionDistrito { get; set; }

        public IEnumerable<SelectListItem> Distrito { get; set; }


        [Display(Name = "Corregimiento",
          ResourceType = typeof(Resources.CampoResource))]
        public int? CorregimientoSeleccionado { get; set; }


        public IEnumerable<SelectListItem> Corregimiento { get; set; }

        public string DescripcionCorregimiento { get; set; }

        [Display(Name = "Estatus"
           , ResourceType = typeof(Resources.CampoResource))]
        public string DescripcionEstatus { get; set; }
        
        [Required]
        [Display(Name = "Calle",
          ResourceType = typeof(Resources.CampoResource))]
        [MaxLength(800)]
        public string Calle { get; set; }


        [Display(Name = "Activo"
            , ResourceType = typeof(Resources.CampoResource))]
        public bool IndicadorActivo { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Display(Name = "FechaCreacion", ResourceType = typeof(Resources.CampoResource))]
        public DateTime FechaCreacion { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Display(Name = "FechaEstatus", ResourceType = typeof(Resources.CampoResource))]
        public DateTime FechaEstatus { get; set; }

        public IList<Aplicacion> Aplicaciones { get; set; }

        public string EditarRegistro { get; set; }


        public SOCIO_COMERCIAL toModel() {

            SOCIO_COMERCIAL socio = new SOCIO_COMERCIAL();
            USUARIO usuario = new USUARIO();
            socio.USUARIO                   = usuario;
            socio.CD_SOCIO_COMERCIAL        = Id;
            socio.CD_CORREGIMIENTO          = CorregimientoSeleccionado;
            socio.CD_DISTRITO               = DistritoSeleccionado;
            socio.CD_PAIS                   = PaisSeleccionado;
            socio.CD_PROVINCIA              = ProvinciaSeleccionada;
            socio.DI_EMAIL                  = Email;
            socio.DI_OFICINA                = Calle;
            socio.FE_CREACION               = FechaCreacion;
            socio.FE_ESTATUS                = FechaEstatus;
            socio.IN_INACTIVO               = !IndicadorActivo;
            socio.NM_SOCIO_COMERCIAL        = Nombre;
            socio.USUARIO.NU_DOCUMENTO_IDENTIDAD    = NumeroDocumento;
            socio.NU_TELEFONO_FIJO          = TelefonoFijo;
            socio.NU_TELEFONO_MOVIL         = TelefonoMovil;
            socio.USUARIO.TP_DOCUMENTO_IDENTIDAD = TipoDocumentoSeleccionado;
            socio.ID_DISTRIBUIDOR           =    DistribuidorSeleccionado;
            socio.CD_SEGMENTO_NEGOCIO       = SegmentoNegocioSeleccionado;
            
            return socio;
        }


        /// <summary>
        /// Validaciones del socio comercial
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            ValidacionController validador = new ValidacionController();
            JsonResult result;
            
            // Socio comercial nuevo
            if (Id == 0){
                

                result = validador.verificaNumeroDocumentoSocioComercialNoUsado(TipoDocumentoSeleccionado, NumeroDocumento);

                if (result.Data.GetType() == typeof(String))
                    yield return new ValidationResult((String)result.Data);
            }            
        }
    }
}