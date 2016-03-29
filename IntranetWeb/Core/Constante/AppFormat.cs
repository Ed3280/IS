using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Constante
{
    public static class AppFormat
    {
        /// <summary>
        /// Formato de Fecha
        /// </summary>
        public  const string date ="{0:dd/MM/yyyy}";
        /// <summary>
        /// Formato de Día y Mes
        /// </summary>
        public const string dateDayMonth = "{0:dd/MM}";

        /// <summary>
        /// Formato para fecha y hora
        /// </summary>
        public const string dateHour = "{0:dd/MM/yyyy HH:mm}";
        /// <summary>
        /// Formato para la hora
        /// </summary>
        public const string hour = "{0:HH:mm}";
        /// <summary>
        /// Formato para fecha  Json
        /// </summary>
        public  const string dateJson  = "dd/MM/yyyy";
        /// <summary>
        /// Formato para fecha y hora Json
        /// </summary>
        public  const string dateHourJson =  "dd/MM/yyyy HH:mm";
        /// <summary>
        /// Formato para la hora Json
        /// </summary>
        public const string hourJson = "HH:mm";
    }
}