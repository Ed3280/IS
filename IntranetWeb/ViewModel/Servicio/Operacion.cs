using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Servicio
{
    public class Operacion
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Operación")]
        public string Nombre { get; set; }

        public int Duracion { get; set; }
    }
}