using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.ViewModel.Monitor;
using IntranetWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace IntranetWeb.ViewModel.Monitor
{
    public class Distribuidor
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Distribuidor")]
        public String Nombre { get; set; }


        /// <summary>
        /// Acción para el control nested de dispositivos
        /// </summary>
        public string AccionNestedDispositivos { get; set; }
    }
}