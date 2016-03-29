using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Constante.Mensaje
{
    public static class Error
    {
        /// <summary>
        /// Error genérico de administrador 
        /// </summary>
       
        public const string Error200 = "Usuario no se encuentra logueado. Por favor inicie sesión (200)";

        public const string Error300 = "Error en web service (300)";

        public const string Error400 = "Error debido a acción no registrada (400)";

        public const string Error500 = "Ocurrió un error fatal al procesar petición. (500)";
        
        public const string FormularioInvalido = "Por favor verifique: ";

        public const string RegistroNoCreado = "El registro no fue creado. Por favor intente de nuevo";

        public const string FalloActualizarCliente = "No se pudo actualizar la información del cliente. Consulte con el administrador";

        public const string FalloGuardarHistoricoGCM = "Fallo al guardar histórico de GCM";

        public const string FalloEnvioCMS = "El mensaje no pudo ser enviado al cliente";
        
        public const string FalloActualizarTicket = "Ocurrió un fallo al actualizar el ticket";

        public const string TicketAtendidoNoModificable = "Ticket atendido no puede ser modificado. Contacte a su supervisor";

        
        public const string UsuarioNoAutorizado = "Disculpe, no está autorizado para realizar esta acción"; 

        public const string UsuarioBajaNoActualizable = "Empleado se dio de baja. No puede ser actualizado";
                        
        public const string VehiculoNoExiste = "Vehículo no existe";

        public const string SeleccioneTipoAtencion = "Debe indicar el Tipo de Caso"; 

        public const string SeleccioneUsuarioTranfTicket = "Debe seleccionar el usuario al que desea transferir el ticket";

        public const string AsigneRolUsuario = "Debe asignar al menos un rol al usuario";
        
        public const string FalloEnviarInvitacionExcel = "Invitación a Excel Connect no pudo ser enviada. Consulte con el administrador"; 

        public const string UsuarioSinDashBoard = "Usuario no posee Dashboard (página de inicio) asociada. Por favor consulte con el administrador"; 
        
        public const string FalloGenerarArchivoExcel = "Ocurrió un fallo al genera el archivo excel. Consulte con el administrador"; 
        
        public const string FechaNotificacionInvalida = "Fecha de Notificación inválida";

        public const string TicketNoAtendido = "Ticket debe ser atendido para poder enviar mensaje al cliente";

        public const string SeleccioneMensajeAlCliente = "Debe indicar el Mensaje al Cliente";

        public const string ParametroConfiguracionExiste = "El parámetro de sistema indicado ya se encuentra registrado";

        public const string ParametroConfiguracionNoExiste = "El parámetro de sistema indicado no existe";

        public const string UnidadAdministrativaNoExiste = "La unidad administrativa indicada no existe";

        public const string EmpleadoReemplazoObligatorio = "Debe indicar el empleado que lo sustituye";

    }
}