using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using IntranetWeb.Core.Utils;

namespace IntranetWeb.ViewModel.Monitor
{
    public class Notificacion
    {
        public Notificacion() {

            this.Distribuidor = new Distribuidor();

            this.Dispositivo = new Dispositivo();

            this.Cliente = new UsuarioMovil();
    }


        [Key]
        public int Id { get; set; }

        [Key]
        public int IdOrigenNotificacion {get; set;}

        [Display(Name ="Origen Notificación")]
        public String DescipcionOrigenNotificacion { get; set; } 

        
        public int IdTipoNotificacion { get; set; }

        [Display(Name = "Tipo Notificación")]
        public String TipoNotificacion { get; set; }


        [Display(Name = "Fecha Notificación")]
        [DataType(DataType.DateTime)]
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime FechaNotificacion { get; set; }

        public string Icono { get; set; }
        public String ColorTitulo { get; set; }
        public String ColorCuerpo { get; set; }

        public String ColorEtiqueta { get; set; }

        public bool IndicadorLocalizacion { get; set; }
        

        public String NombreImagen { get; set; }

        public Byte[] Imagen { get; set; }

        public Distribuidor Distribuidor {get; set;}

        
        public Dispositivo Dispositivo { get; set; }

        public UsuarioMovil Cliente { get; set; }


    }
}