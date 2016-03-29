using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Constante
{
    /// <summary>
    /// Almacena las aplicaciones disponibles en el sistema
    /// </summary>
    public static class Aplicacion
    {
        /// <summary>
        /// Configuración de Usuario
        /// </summary>
        public const string ConfiguracionUsuario = "conf_usu";
        /// <summary>
        /// Dashboard General
        /// </summary>
        public const string DashBoardGeneral = "dash_general";
        /// <summary>
        /// Edición de los datos del cliente
        /// </summary>
        public const string EditarCliente = "edit_cliente";
        /// <summary>
        /// Visualizar el monitor de alerta y generar Tickets (Perfil de Agentes)
        /// </summary>
        public const string VerMonitorAlerta = "ver_monitor";
        /// <summary>
        /// Administrador del Visor de Alertas
        /// </summary>
        public const string VerMonitorAlertaAdmin = "ver_monitor_admin";
        /// <summary>
        /// Ver Kilometraje Total de los Vehículos
        /// </summary>
        public const string VerKilometraje = "ver_kilometraje";
        /// <summary>
        /// Localización del vehículo
        /// </summary>
        public const string LocalizarVehiculo = "ver_monitor_localizacion";
        /// <summary>
        /// Consultar el Kilometraje total de los Vehículos
        /// </summary>
        public const string ConsultarKilometraje = "cons_kilometraje";
        /// <summary>
        /// Consultar el listado de clientes
        /// </summary>
        public const string ConsultarCliente = "cons_cliente";
        /// <summary>
        /// Generar reporte de Tickets
        /// </summary>
        public const string GenerarReporteTickets =  "gen_reporte_ticket";
        /// <summary>
        /// Generar reporte de Alertas
        /// </summary>
        public const string GenerarReporteAlertas = "gen_reporte_alerta";

        /// <summary>
        /// Consultar el log de transacciones
        /// </summary>
        public const string ConsultarLogTransacciones = "cons_log";

        /// <summary>
        /// Mantenimiento de la tabla RESPUESTA_OPERADOR_GCM
        /// </summary>
        public const string RespuestaOperadorGCM = "edit_respuesta_gcm";

        /// <summary>
        /// Mantenimiento de la tabla PARAMETRO_CONFIGURACION
        /// </summary>
        public const string ParametroConfiguracion = "edit_parametro_conf";
        
        /// <summary>
        /// Mantenimiento de la tabla UNIDAD_ADMINISTRATIVA
        /// </summary>
        public const string UnidadAdministrativa = "edit_unidad_administrativa";

        /// <summary>
        /// Mantenimiento de los usuarios del sistema
        /// </summary>
        public const string AdministrarUsuarioAdmin = "edit_admin_usuario_admin";

        /// <summary>
        /// Mantenimiento de los usuarios internos del sistema (usuario de usuarios)
        /// </summary>
        public const string AdministrarUsuario = "edit_admin_usuario";

        /// <summary>
        /// Mantenimiento de los empleados del sistema
        /// </summary>
        public const string AdministrarEmpleado = "edit_admin_empleado";

        /// <summary>
        /// Mantenimiento de los cargos del sistema
        /// </summary>
        public const string AdministrarCargo = "edit_admin_cargo";

        /// <summary>
        /// Mantenimiento de los roles del sistema
        /// </summary>
        public const string AdministrarRol = "edit_admin_rol";

        /// <summary>
        /// Mantenimiento de los socios comerciales
        /// </summary>
        public const string AdministrarSocioComercial = "edit_admin_socio_comercial";
        
        /// <summary>
        /// Mantenimiento de los proveedores
        /// </summary>
        public const string AdministrarProveedor = "edit_admin_proveedor";

        /// <summary>
        /// Mantenimiento inteligente
        /// </summary>
        public const string MantenimientoInteligente = "gest_mant_inteli";

    }
}