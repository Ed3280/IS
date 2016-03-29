using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.ViewModel.Monitor;
using IntranetWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace IntranetWeb.ViewModel.Monitor
{
    public class UsuarioMovil
    {
        
        [Key]
        public int? Id { set; get; }
        
        [Display(Name ="Documento")]
        public String NumeroDocumento { set; get; }

        
        [Display(Name = "Cliente")]
        public string NombreCompleto { get; set; }
        
        [Display(Name = "Nombre cliente")]
        public string Nombre { set; get; }
        
        [Display(Name = "Apellido cliente")]
        public string Apellido { set; get; }
        
        [Display(Name = "Teléfono móvil")]
        public int? TelefonoMovil { set; get; }
        
        public Double? Latitud { get; set; }

        public Double? Longitud { get; set; }

        [Display(Name = "Ubicación")]
        public string Ubicacion { set; get; }

    }
}