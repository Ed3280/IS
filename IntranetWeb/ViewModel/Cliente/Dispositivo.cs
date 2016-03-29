using IntranetWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.ViewModel.Cliente
{
    public class Dispositivo
    {

        [Key]
        [Display(Name = "Id",
            ResourceType = typeof(Resources.CampoResource))]
        public int Id { get; set; }


        [Display(Name = "Dispositivo",
            ResourceType = typeof(Resources.CampoResource))]
        public IEnumerable<SelectListItem> Dispositivos { get; set; }

        [Key]
        [Display(Name = "Serial",
            ResourceType = typeof(Resources.CampoResource))]
        public string Imei { get; set; }

        [Display(Name = "Vin",
            ResourceType = typeof(Resources.CampoResource))]
        public string Vin { get; set; }
        
        //Id del dispositivo en la plataforma china
       /*  [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public int IdDispositivo { get; set; }
        */

         public int IdCliente { get; set; }


        [Display(Name = "NombreDispositivo",
            ResourceType = typeof(Resources.CampoResource))]
        public string NombreDispositivo { get; set; }

        [Display(Name = "TarjetaSim",
            ResourceType = typeof(Resources.CampoResource))]
        public string TarjetaSim { get; set; }

        [Display(Name = "NumeroDVR",
            ResourceType = typeof(Resources.CampoResource))]
        public string NumeroDVR { get; set; }

        [Display(Name = "AperturaRemota",
            ResourceType = typeof(Resources.CampoResource))]
        public bool IndicadorAperturaRemota { get; set; }

        [Display(Name = "LocalizacionBotonSOS",
            ResourceType = typeof(Resources.CampoResource))]
        public string LocalizacionBotonSOS { get; set; }

        [Display(Name = "TipoApagadoRemoto",
            ResourceType = typeof(Resources.CampoResource))]
        public string TipoApagadoRemoto { get; set; }


        [Display(Name = "GBHostSpot",
            ResourceType = typeof(Resources.CampoResource))]
        public string GBHostSpot { get; set; }

        public string NombreVehiculoAsociado { get; set; }

        [Display(Name = "Activo",
            ResourceType = typeof(Resources.CampoResource))]
        public bool IndicadorEstaActivo { get; set; }


        [Display(Name = "FechaRegistro",
         ResourceType = typeof(Resources.CampoResource))]

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime FechaRegistro { get; set; }
        

        [Display(Name = "ContrasenaDVR",
         ResourceType = typeof(Resources.CampoResource))]
        public String ContrasenaDVR { get; set; }

        [Display(Name = "Dispositivo",
            ResourceType = typeof(Resources.CampoResource))]
        public int DispositivosDisponiblesSeleccionado { get; set; }

        public IEnumerable<SelectListItem> DispositivosDisponibles { get; set; }

        [Display(Name = "KilometrajeInicial",
            ResourceType = typeof(Resources.CampoResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public Double KilometrajeInicial { get; set; }

        [Display(Name = "Vehiculo",
              ResourceType = typeof(Resources.CampoResource))]
        public int? VehiculosSeleccionado { get; set; }

        public IEnumerable<SelectListItem> Vehiculos { get; set; }


        public DISPOSITIVO_CLIENTE toModel() {
            DISPOSITIVO_CLIENTE dispositivoCliente = new DISPOSITIVO_CLIENTE();

            dispositivoCliente.ID_DISPOSITIVO = Id;
            dispositivoCliente.CD_CLIENTE = IdCliente;
            dispositivoCliente.CD_VEHICULO = VehiculosSeleccionado;
            dispositivoCliente.DE_CONTRASENA_DVR = ContrasenaDVR;
            dispositivoCliente.IN_ADMITE_APERTURA_REMOTA = IndicadorAperturaRemota;
            dispositivoCliente.IN_INACTIVO = !IndicadorEstaActivo;

            return dispositivoCliente;
        }
    }
}