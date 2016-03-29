using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Constante.Mensaje
{
    public static class Logging
    {
        /// <summary>
        /// Mensaje cuando el usuario hizo login
        /// </summary>
        public const string IngresoUsuario = "Login Usuario";
        /// <summary>
        /// Mensaje de actualización de datos del cliente
        /// </summary>
        public const string ClienteActualizado = "Cliente \"{0}\" Actualizado";
        /// <summary>
        /// Creación de un mensaje de respuesta de operador GCM
        /// </summary>
        public const string RespuestaOperadorGCMCreado = "Respuesta Operador GCM \"{0}\" Creada";
        /// <summary>
        /// Actualización de un mensaje de respuesta GCM
        /// </summary>
        public const string RespuestaOperadorGCMActualizado = "Respuesta Operador GCM \"{0}\" Actualizada";

        /// <summary>
        /// Creación del parámetro de configuración
        /// </summary>
        public const string ParametroConfiguracionCreado = "Parametro de Configuración \"{0}\" - \"{1}\" Creado";

        /// <summary>
        /// Actualización de un parámetro de configuración 
        /// </summary>
        public const string ParametroConfiguracionActualizado = "Parámetro de Configuración \"{0}\" - \"{1}\" actualizado a \"{2}\"";

        /// <summary>
        /// Eliminación de un parámetro de configuración 
        /// </summary>
        public const string ParametroConfiguracionEliminado = "Parámetro de Configuración \"{0}\" - \"{1}\" Eliminado";


        /// <summary>
        /// Eliminación de una unidad administrativa
        /// </summary>
        public const string UnidadAdministrativaEliminada = "Unidad Administrativa \"{0}\" Eliminada";


        /// <summary>
        /// Actualización de la unidad administrativa 
        /// </summary>
        public const string UnidadAdministrativaActualizada = "Unidad Administrativa \"{0}\" actualizada";


       

        /// <summary>
        /// Creación de una unidad administrativa 
        /// </summary>
        public const string UnidadAdministrativaCreada = "Unidad Administrativa \"{0}\" creada";
        
        
        /// <summary>
        /// Baja de empleado
        /// </summary>
        public const string EmpleadoBaja = "Empleado \"{0}\" dado de baja";


    }
}