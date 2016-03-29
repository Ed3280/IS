using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntranetWeb.ViewModel.Kilometraje
{
    public class Vehiculo {

         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Id Usuario")]
        public int IdUsuario { get; set; }
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Id Vehículo")]
        public int IdVehiculo { get; set; }
        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Vehículo")]
        public string NombreVehiculo { get; set; }

        
        [Display(Name = "Kilometraje Inicial")]
        public Double DistanciaInicial { get; set; }

        [Display(Name = "Kilómetros Recorridos")]
        public Double DistanciaRecorrida { get; set; } 
        
        public String EditarRegistro { get; set; }


        public IntranetWeb.Models.KILOMETRAJE_TOTAL toModel() {
            var kilometrajeTotal = new IntranetWeb.Models.KILOMETRAJE_TOTAL();
            kilometrajeTotal.DeviceID = this.IdVehiculo;
            kilometrajeTotal.UserID = this.IdUsuario;
            kilometrajeTotal.MontoKilometrajeInicial = this.DistanciaInicial;
            kilometrajeTotal.MontoKilometrajeTotal = this.DistanciaRecorrida;
            return kilometrajeTotal;
        }

    }
}