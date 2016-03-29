using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.ViewModel.Monitor;
using IntranetWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace IntranetWeb.ViewModel.Monitor
{
    public class Dispositivo
    {
        [Key]
        public int Id{ get; set; }

        [Display(Name = "Dispositivo")]
        public string Nombre{ get; set; }

        [Display(Name = "DVR")]
        public string DVR { get; set; }

        [Display(Name = "VIN")]
        public string Vin { set; get; }

        [Display(Name = "Placa")]
        public string Placa { get; set; }
        
        [Display(Name = "SIM")]
        public string SimCard { set; get; }

        [Display(Name = "Observación Dispositivo")]
        public string Observacion { get; set; }

        [Display(Name = "Estatus")]
        public string DescripcionEstatus { get; set; }

        [Display(Name = "Serial")]
        public string Serial { get; set; }

        public string Icono { get; set; }

    }
}