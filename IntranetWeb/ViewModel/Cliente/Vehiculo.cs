using IntranetWeb.Controllers;
using IntranetWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Cliente
{
    public class Vehiculo : IValidatableObject
    {
        [Key]
        [Display(Name = "Id",
        ResourceType = typeof(Resources.CampoResource))]
        public int Id { get; set; }

        public int IdCliente { get; set; }

        private string vin;
        [Key]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MaxLength(100)]
        [Display(Name = "NumeroChasis",
            ResourceType = typeof(Resources.CampoResource))]
        [Remote("verificaVINNoUsado", "Validacion", ErrorMessageResourceName = "VinEnUso", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Vin { get { return String.IsNullOrWhiteSpace(vin) ? vin : vin.ToUpper(); } set { vin = value; } }


        private string numeroPlaca;
        [Display(Name = "NumeroPlaca",
            ResourceType = typeof(Resources.CampoResource))]
        public string NumeroPlaca { get { return String.IsNullOrWhiteSpace(numeroPlaca) ? vin : numeroPlaca.ToUpper(); } set { numeroPlaca = value; } }

        [Display(Name = "Marca",
                ResourceType = typeof(Resources.CampoResource))]
        public int? MarcaSeleccionada { get; set; }
        public IEnumerable<SelectListItem> Marca { get; set; }


        public string DescripcionMarca { get; set; }

        [Display(Name = "Modelo",
            ResourceType = typeof(Resources.CampoResource))]
        public int? ModeloSeleccionado { get; set; }
        public IEnumerable<SelectListItem> Modelo { get; set; }
        
        public string DescripcionModelo { get; set; }

        [Display(Name = "Color",
            ResourceType = typeof(Resources.CampoResource))]
        public string Color { get; set; }

        [Display(Name = "Ano",
        ResourceType = typeof(Resources.CampoResource))]
        public int? AnoSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Ano { get; set; }
        public IEnumerable<SelectListItem> TipoCombustible { get; set; }

        [Display(Name = "TipoCombustible",
                ResourceType = typeof(Resources.CampoResource))]
        public int? TipoCombustibleSeleccionado { get; set; }

        public string DescripcionTipoCombustible { get; set; }


        public IEnumerable<SelectListItem> TipoTransmision { get; set; }


        [Display(Name = "TipoTransmision",
            ResourceType = typeof(Resources.CampoResource))]
        public int? TipoTransmisionSeleccionada { get; set; }

        public string DescripcionTipoTransmision { get; set; }


        public IEnumerable<SelectListItem> TipoCierre { get; set; }

        [Display(Name = "TipoCierre",
            ResourceType = typeof(Resources.CampoResource))]
        public int? TipoCierreSeleccionado { get; set; }

        public string DescripcionTipoCierre { get; set; }


        public IEnumerable<SelectListItem> CompaniaSeguro { get; set; }

        [Display(Name = "CompaniaSeguro",
            ResourceType = typeof(Resources.CampoResource))]
        public int? CompaniaSeguroSeleccionada { get; set; }


        public string DescripcionCompaniaSeguro { get; set; }


        [Display(Name = "VigenciaPoliza",
            ResourceType = typeof(Resources.CampoResource))]
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime? VigenciaPoliza { get; set; }

        
        public VEHICULO toModel() {
            VEHICULO vehiculo = new VEHICULO();

            vehiculo.CD_VEHICULO = Id;
            vehiculo.CD_ANO = AnoSeleccionado;
            vehiculo.CD_COMPANIA_SEGURO = CompaniaSeguroSeleccionada;
            vehiculo.CD_MARCA = MarcaSeleccionada;
            vehiculo.CD_MODELO = ModeloSeleccionado;
            vehiculo.FE_SEGURO_VIGENCIA_HASTA = VigenciaPoliza;
            vehiculo.NM_COLOR = Color;
            vehiculo.NU_PLACA = NumeroPlaca;
            vehiculo.NU_VIN = Vin;
            vehiculo.TP_CIERRE_VEHICULO = TipoCierreSeleccionado;
            vehiculo.TP_COMBUSTIBLE = TipoCombustibleSeleccionado;
            vehiculo.TP_TRANSMISION_VEHICULO = TipoTransmisionSeleccionada;
            vehiculo.CD_CLIENTE = IdCliente;
            
            return vehiculo;
        }


        /// <summary>
        /// Validaciones del vehículo
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            ValidacionController validador = new ValidacionController();
            JsonResult result;
            //Usuario nuevo
            if (Id == 0){

                //Se verifica si el usuario ya es un cliente
                result = validador.verificaVINNoUsado(Vin);

                if (result.Data.GetType() == typeof(String))
                    yield return new ValidationResult((String)result.Data);

            }
        }    
    }
}