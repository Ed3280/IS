using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Auth
{
    public class NodoAplicacion
    {
        [Key]
        public int IdAplicacion { get; set; }

        public string NombreAplicacion { get; set; }
        
        public string Controlador { get; set; }

        public string Accion { get; set; }

        public string IconoMenu {get; set;}

        public string IconoAccesoRapido { get; set; }

        public int NumeroOrden { get; set; }

        public int? AplicacionPadre { get; set; }

        public String Parametros { get; set; }

        public IQueryable<NodoAplicacion> AplicacionesHijas { get; set; }

        public System.Web.Routing.RouteValueDictionary getDiccionarioParametros()
        {
            if (String.IsNullOrWhiteSpace(this.Parametros)) return null;

            var diccionario = new System.Web.Routing.RouteValueDictionary();
            var parametros = this.Parametros.Split(',');
            foreach (var parametro in parametros)
            {
                if (String.IsNullOrWhiteSpace(parametro)) continue; //Puede venir en blanco si la cadena viene de la forma "pepe=hola,"
                var paramArr = parametro.Split('=');
                diccionario.Add(paramArr[0].Trim(), paramArr[1].Trim());
            }
            return diccionario;
        }

        


    }
}