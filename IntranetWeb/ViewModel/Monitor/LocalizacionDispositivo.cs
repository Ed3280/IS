using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Monitor
{
    public class LocalizacionDispositivo
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name ="Distribuidor")]
        public string Dealer { get; set; }
        public string  Name { get; set; }

        [Display(Name="Actualizado")]
        public DateTime deviceUtcDate { get; set; }

        public float latitude { get; set; }

        public float longitude { get; set; }

        [Display(Name = "Velocidad")]
        public float speed { get; set; }

        public float course { get; set; }

        public int IsStop { get; set; }

        public string icon { get; set; }
        public float distance { get; set; }

        [Display(Name = "Min Detenido")]
        public float stopTimeMinute { get; set; }


        [Display(Name = "Estatus", ResourceType = typeof(Resources.CampoResource))]
        public string status { get; set; }
        public DateTime serverUtcTime { get; set; }

        [Display(Name ="Serial")]
        public string SerialNumber { get; set; }

        [Display(Name= "KilometrajeTotal", ResourceType = typeof(Resources.CampoResource))]
        public int KilometrajeTotal { get; set; }
    }
}