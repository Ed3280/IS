using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.Models;
using System.Web.Mvc;


namespace IntranetWeb.Core.Respositorios
{
    public class UtilRepositorio
    {
        public UtilRepositorio() { }
        /// <summary>
        /// Permite obtener un valor único de la tabla de configuración de parámetros
        /// </summary>
        /// <param name="nombreParametro"></param>
        /// <returns></returns>
        public static string obtenValorUnico_PARAMETRO_CONFIGURACION(string nombreParametro) {
            string result = null;
            using (IntranetSAIEntities db = new IntranetSAIEntities() ) {

                PARAMETRO_CONFIGURACION pc = (from x in db.PARAMETRO_CONFIGURACION
                                              where x.NM_PARAMETRO == nombreParametro
                                              select x).FirstOrDefault();
                if (pc != null)
                    result = pc.CD_CONFIGURACION;
            }
            return result;
        }


        /// <summary>
        /// Retorna la primera plantilla de correo dado el tipo de plantilla (el código de usuario es opcional)
        /// </summary>
        /// <param name="TipoPLantilla"></param>
        /// <param name="CodigoUsuario"></param>
        /// <returns></returns>
        public static PLANTILLA_CORREO obten_PLANTILLA_CORREO_ByTipoPLantilla(int TipoPLantilla, int? CodigoUsuario ) {
            
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (
                    from x in db.PLANTILLA_CORREO
                    where x.TP_PLANTILLA_CORREO == TipoPLantilla
                    && x.CD_USUARIO == CodigoUsuario
                    select x).FirstOrDefault();
                    
            }
        }


        #region Carga de Selects 

        /// <summary>
        /// Obtiene listado de User de la base de datos PanamaMain para listarlos en un select
        /// </summary>
        /// <returns>Lsitado de usuarios para el select </returns>
        public static IEnumerable<SelectListItem> obtenSelect_Distribuidor() {

                using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                    return (from x in db.DISTRIBUIDOR
                            orderby x.NM_DISTRIBUIDOR ascending
                            where x.IN_ELIMINADO == false
                                select new SelectListItem()
                                {
                                    Value = x.ID_DISTRIBUIDOR.ToString()
                                    ,
                                    Text = x.NM_DISTRIBUIDOR
                                }
                             ).ToList().AsEnumerable();
                }
            }


        /// <summary>
        /// Devuelve los niveles de log de transacciones
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_NivelLog() {

            return new List<SelectListItem>{
                 new SelectListItem { Text = "INFO", Value = "INFO" }
                ,new SelectListItem { Text = "ERROR", Value = "ERROR" }
                ,new SelectListItem { Text = "DEBUG", Value = "DEBUG" }
                ,new SelectListItem { Text = "FATAL", Value = "FATAL" }
            };            
        }


        /// <summary>
        /// Obtiene select Corregimiento db.sys_zones de la base de datos systemIM
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="stateCode"></param>
        /// <param name="municipalityCode"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Corregimiento(int? cdPais, int? cdProvincia, int? cdDistrito)
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                         from corr in db.CORREGIMIENTO
                         where corr.CD_PAIS == cdPais
                     && corr.CD_PROVINCIA == cdProvincia
                     && corr.CD_DISTRITO == cdDistrito
                     && corr.CD_CORREGIMIENTO !=0
                         select new SelectListItem()
                         {
                             Value = corr.CD_CORREGIMIENTO.ToString()
                                         ,
                             Text = corr.NM_CORREGIMIENTO
                         }
                    ).ToList().AsEnumerable();

            }
        }


        /// <summary>
        /// Obtiene select Marca db.sys_zones de la base de datos systemIM
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_MarcaVehiculo(){

            using (IntranetSAIEntities db = new IntranetSAIEntities()){

                return (
                         from x in db.MARCA
                         where   x.MODELO.FirstOrDefault(y => y.CD_TIPO_ARTICULO == 1)!= null 
                         orderby x.NM_MARCA
                         select  new SelectListItem()
                         {
                             Value = x.CD_MARCA.ToString()
                                         ,
                             Text = x.NM_MARCA
                         }
                    ).Distinct().ToList().AsEnumerable();
            }
        }


        /// <summary>
        /// Obtiene los modelos de Vehículos
        /// </summary>
        /// <param name="marca"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_ModeloVehiculo(int? marca) {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                         from x in db.MODELO
                         where x.CD_MARCA == marca
                         && x.IN_INACTIVO == false
                         orderby x.NM_MODELO
                         select new SelectListItem()
                         {
                             Value =  x.CD_MODELO.ToString()
                                    ,Text = x.NM_MODELO
                         }
                    ).Distinct().ToList().AsEnumerable();
            }
        } 

        

        /// <summary>
        /// Obtiene select Municipios db.sys_municipality de la base de datos systemIM
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Distrito(int? cdPais, int? cdProvincia) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()){
                return (
                         from x in db.DISTRITO
                         where x.CD_PAIS == cdPais
                         && x.CD_PROVINCIA == cdProvincia
                         && x.CD_DISTRITO !=0
                         orderby x.NM_DISTRITO
                         select new SelectListItem()
                         {
                             Value = x.CD_DISTRITO.ToString()
                                         ,
                             Text = x.NM_DISTRITO
                         }
                    ).ToList().AsEnumerable();

            }
           }

        /// <summary>
        /// Obtiene el listado de paises de la tabla  db.sys_country de la base de datos systemIM
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Pais()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (
                         from x in db.PAIS
                         where x.CD_PAIS != 0
                         orderby x.NM_PAIS
                         select new SelectListItem()
                         {
                             Value = x.CD_PAIS.ToString()
                                         ,
                             Text = x.NM_PAIS
                         }
                    ).ToList().AsEnumerable();

            }
        }
        

        /// <summary>
        /// Obtiene el listado de tipo de tención en la base de datos IntranetSAIEntities
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoAtencion()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (
                         from x in db.TIPO_ATENCION
                         where x.IN_INACTIVO == false
                         orderby x.DE_TIPO_ATENCION ascending
                         select new SelectListItem()
                         {
                             Value = x.TP_ATENCION.ToString()
                                         ,
                             Text = x.DE_TIPO_ATENCION
                         }
                    ).ToList().AsEnumerable();

            }
        }

        /// <summary>
        /// Devuelve el listado de tipo de notificaciones de Alertas (Monitor)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoNotificacion()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (
                         from x in db.TIPO_NOTIFICACION
                         where x.IN_INACTIVO == false
                         orderby x.DE_TIPO_NOTIFICACION ascending
                         select new SelectListItem()
                         {
                             Value = x.TP_NOTIFICACION.ToString()
                                         ,
                             Text = x.DE_TIPO_NOTIFICACION
                         }
                    ).ToList().AsEnumerable();

            }
        }

        /// <summary>
        /// Retoran el listado de los tipos de sangre
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoSangre() {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (
                        from x in db.TIPO_SANGRE
                        orderby x.NM_TIPO_SANGRE
                        select new SelectListItem()
                        {
                            Value = x.TP_SANGRE
                            ,
                            Text = x.NM_TIPO_SANGRE

                        }

                    ).ToList().AsEnumerable();
            }
        }


        /// <summary>
        /// Obtiene el listado de Provincias dado el pais. Carga de la tabla db.sys_state en la base de datos systemIM
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Provincia(int? cdPais) {


            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (
                         from x in db.PROVINCIA                         
                         where x.CD_PAIS == cdPais
                         && x.CD_PROVINCIA !=0
                         orderby x.NM_PROVINCIA
                         select new SelectListItem()
                         {
                             Value = x.CD_PROVINCIA.ToString()
                                         ,
                             Text = x.NM_PROVINCIA
                         }
                    ).ToList().AsEnumerable();
            }
        }

        /// <summary>
        /// Retorna el listado de mensajes al cliente
        /// </summary>
        /// <param name="tpAtencion">Tipo de atención que se le da al cliente</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_RespuestaOperadorGCM(int? tpAtencion)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities()){
                return (
                         from x in db.RESPUESTA_OPERADOR_GCM
                         where x.TP_ATENCION == tpAtencion
                         && x.IN_INACTIVO == false
                         orderby x.DE_TITULO
                         select new SelectListItem()
                         {
                             Value = x.ID_RESPUESTA_OPERADOR.ToString()
                            ,Text = x.DE_TITULO
                         }
                    ).ToList().AsEnumerable();
            }
        }

        /// <summary>
        /// Retorna los roles de un usuario
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Roles(int? IdUsuario) {
            IdUsuario = IdUsuario == null ? 0 : IdUsuario;
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (
                    from x in db.ROL
                    where x.IN_ROL_ACTIVO == true
                    && x.CD_ROL != 1
                    orderby x.NM_ROL
                    select new SelectListItem() {
                         Value = x.CD_ROL.ToString()
                        ,Text = x.NM_ROL
                        ,Selected = x.USUARIO.Select(y => y.CD_USUARIO).Contains((int)IdUsuario)
                        }
                    ).ToList().AsEnumerable();
            }
        }

        /// <summary>
        /// Retorna los roles dado un cargo
        /// </summary>
        /// <param name="IdCargo"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_RolesPorCargo(int? IdCargo)
        {
            IdCargo = IdCargo == null ? 0 : IdCargo;
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (
                    from x in db.ROL
                    where x.IN_ROL_ACTIVO == true
                    && x.CD_ROL != 1
                    orderby x.NM_ROL
                    select new SelectListItem()
                    {
                        Value = x.CD_ROL.ToString()
                        ,
                        Text = x.NM_ROL
                        ,
                        Selected = x.CARGO.Select(y => y.CD_CARGO).Contains((int)IdCargo)
                    }
                    ).ToList().AsEnumerable();

            }
        }



        /// <summary>
        /// Retorna la lista de dispositivos para asignar a un usuario en el monitor 
        /// </summary>
        /// <param name="IdUsuarioPadre"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_RolesPorUsuarioPadre(int? IdUsuario, int? IdUsuarioPadre)
        {
            IdUsuario       = IdUsuario == null ? 0 : IdUsuario;
            IdUsuarioPadre  = IdUsuarioPadre == null ? 0 : IdUsuarioPadre;

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.ROL
                        from y in db.PARAMETRO_CONFIGURACION
                        where x.IN_ROL_ACTIVO == true
                        && x.USUARIO.Select(w => w.CD_USUARIO).Contains((int)IdUsuarioPadre)
                        && (y.NM_PARAMETRO == "ROL_SOCIO_COMERCIAL"
                           || y.NM_PARAMETRO == "ROL_CLIENTE"
                        )
                        && x.CD_ROL.ToString() == y.CD_CONFIGURACION
                        select new SelectListItem
                        {
                             Value = x.CD_ROL.ToString()
                            ,Text = x.NM_ROL
                            ,Selected  = x.USUARIO.Select(w => w.CD_USUARIO).Contains((int)IdUsuario)
                        }).ToList().AsEnumerable();
            }
        }


        /// <summary>
        /// Retornas los roles para los empleados
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <param name="cdCargo"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_RolesEmpleado(int? cdUsuario, int? cdCargo) {

            cdUsuario = cdUsuario == null ? 0 : cdUsuario;
            cdCargo = cdCargo == null ? 0 : cdCargo;

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                      (from x in db.ROL
                       where x.IN_ROL_ACTIVO == true
                       && x.CARGO.Select(y => y.CD_CARGO).Contains((int)cdCargo)
                       orderby x.NM_ROL
                       select new SelectListItem
                       {
                           Value = x.CD_ROL.ToString()
                           ,
                           Text = x.NM_ROL
                           ,
                           Selected = (x.USUARIO.Select(y => y.CD_USUARIO).Contains((int)cdUsuario) ? true : false)
                       }).ToList().AsEnumerable()
                    );               
                }
            }



        /// <summary>
        /// Retorna la lista de dispositivos para asignar a un usuario en el monitor 
        /// </summary>
        /// <param name="IdUsuarioPadre"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_DispositivosPorUsuarioPadre(int? IdUsuarioPadre)
        {
            int idPadreInt = IdUsuarioPadre == null ? -1 : (int)IdUsuarioPadre;
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.DISPOSITIVO
                        where (x.USUARIO.Select(y => y.CD_USUARIO).Contains(idPadreInt)
                            || IdUsuarioPadre == null) &&
                        x.IN_ELIMINADO == false
                        orderby x.NM_DISPOSITIVO
                        select new SelectListItem
                        {
                            Value = x.ID_DISPOSITIVO.ToString()
                            ,
                            Text = x.NM_DISPOSITIVO != null && x.NM_DISPOSITIVO != "" ? x.NM_DISPOSITIVO : "S/N: " + x.NU_SERIAL
                        }).ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Retorna el listado de las compañías de seguros
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_CompaniaSeguros() {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return  (
                            from x in db.COMPANIA_SEGURO
                            select new SelectListItem() {
                                 Value = x.CD_COMPANIA_SEGURO.ToString()
                                ,Text = x.NM_COMPANIA_SEGURO
                            }
                    ).ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Muestra el listado de agentes a los cuales se les puede transferir un ticket 
        /// </summary>
        /// <param name="UsuarioLoguado"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_UsuarioTransferenciaTicket(int UsuarioLoguado) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (
                            from x in db.ROL
                            from y in db.USUARIO
                            orderby y.DE_NOMBRE_APELLIDO ascending
                            where (x.CD_ROL == 2 ) //Agente del call center
                            && y.IN_USUARIO_INACTIVO == false
                            && y.ROL.Contains(x)
                            && y.CD_USUARIO != UsuarioLoguado
                            select new SelectListItem()
                            {
                                Value = y.CD_USUARIO.ToString()
                                ,
                                Text = y.DE_NOMBRE_APELLIDO
                            }).ToList().AsEnumerable();

            }
        }

        /// <summary>
        /// Retorna el listado de vehículos dado el id del cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Vehiculos_By_ClienteId(int id) {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (
                            from x in db.VEHICULO
                            where x.CD_CLIENTE == id
                            select new SelectListItem()
                            {
                                Value = x.CD_VEHICULO.ToString()
                                ,
                                Text = x.NU_VIN+(x.CD_MARCA!=null||x.CD_MODELO!=null||x.CD_ANO!=null?" ("+
                                ((x.CD_MARCA!=null?x.MARCA.NM_MARCA+" ":"")+
                                (x.CD_MODELO != null ? x.MODELO.NM_MODELO + " " : "") +
                                (x.CD_ANO != null ? x.ANO_VEHICULO.CD_ANO+ " " : "") 
                                ).Trim()+")":"") 
                            }
                    ).ToList().AsEnumerable();
            }
        }


        /// <summary>
        /// Muestra el listado de tipos de documento de identidad 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoDocumentoIdentidad()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                            from x in db.TIPO_DOCUMENTO_IDENTIDAD
                            orderby x.NM_TIPO_DOCUMENTO ascending
                            select new SelectListItem()
                            {
                                Value = x.TP_DOCUMENTO_IDENTIDAD.ToString()
                                ,
                                Text = x.NM_TIPO_DOCUMENTO
                            }).ToList().AsEnumerable();

            }
        }

        /// <summary>
        /// Muestra el listado de tipos de documento de identidad (para personas naturales)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoDocumentoIdentidadPersonaNatural()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities()){

                return (
                            from x in db.TIPO_DOCUMENTO_IDENTIDAD
                            where x.IN_JURIDICO == false
                            orderby x.NM_TIPO_DOCUMENTO ascending
                            select new SelectListItem(){
                                 Value = x.TP_DOCUMENTO_IDENTIDAD.ToString()
                                ,Text = x.NM_TIPO_DOCUMENTO
                            }).ToList().AsEnumerable();

            }
        }


        /// <summary>
        /// Muestra el listado de tipos de documento de identidad (para personas jurídicas)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoDocumentoIdentidadPersonaJuridica()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities()){
                return (
                            from x in db.TIPO_DOCUMENTO_IDENTIDAD
                            where x.IN_JURIDICO == true
                            orderby x.NM_TIPO_DOCUMENTO ascending
                            select new SelectListItem(){
                                 Value = x.TP_DOCUMENTO_IDENTIDAD.ToString()
                                ,Text = x.NM_TIPO_DOCUMENTO
                            }).ToList().AsEnumerable();

            }
        }


        /// <summary>
        /// Retorn el listado de todos los usuarios en el sistema
        /// </summary>        
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_USUARIO()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                            from y in db.USUARIO
                            orderby y.DE_NOMBRE_APELLIDO ascending
                            where y.IN_USUARIO_INACTIVO == false
                            && y.CD_USUARIO > 0
                            select new SelectListItem()
                            {
                                Value = y.CD_USUARIO.ToString()
                                ,
                                Text = y.DE_NOMBRE_APELLIDO
                            }).ToList().AsEnumerable();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_UsuarioTicket()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                            from x in db.ROL
                            from y in db.USUARIO
                            orderby y.DE_NOMBRE_APELLIDO ascending
                            where (x.CD_ROL == 2 ) //Agente de call Center
                            && y.IN_USUARIO_INACTIVO == false
                            && y.ROL.Contains(x)
                            select new SelectListItem()
                            {
                                Value = y.CD_USUARIO.ToString()
                                ,
                                Text = y.DE_NOMBRE_APELLIDO
                            }).ToList().AsEnumerable();

            }
        }


        /// <summary>
        /// Muestra el listado de estatus de un ticket 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TICKET_ESTATUS()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                            from x in db.TICKET_ESTATUS
                            where x.IN_INACTIVO == false
                            orderby x.DE_ESTATUS ascending
                            select new SelectListItem()
                            {
                                Value = x.ID_ESTATUS_TICKET.ToString()
                                ,
                                Text = x.DE_ESTATUS
                            }).ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Permite obtener el tipo de combustible
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoCombustible(){
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (
                     from x in db.TIPO_COMBUSTIBLE
                     orderby x.NM_TIPO_COMBUSTIBLE
                     select new SelectListItem() {
                          Value = x.TP_COMBUSTIBLE.ToString()
                         ,Text = x.NM_TIPO_COMBUSTIBLE
                        }
                    ).ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Retorna el tipo de cerrado
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoCierreVehiculo() {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                     from x in db.TIPO_CIERRE_VEHICULO
                     orderby x.NM_TIPO_CIERRE_VEHICULO
                     select new SelectListItem()
                     {
                         Value = x.TP_CIERRE_VEHICULO.ToString()
                         ,
                         Text = x.NM_TIPO_CIERRE_VEHICULO
                     }
                    ).ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Retorna el tipo de cerrado
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_TipoTransmision()
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (
                     from x in db.TIPO_TRANSMISION_VEHICULO
                     orderby x.NM_TRANSMISION_VEHICULO
                     select new SelectListItem()
                     {
                         Value = x.TP_TRANSMISION_VEHICULO.ToString()
                         ,
                         Text = x.NM_TRANSMISION_VEHICULO
                     }
                    ).ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Permite obtener un listado de años dada la marca y el modelo
        /// </summary>
        /// <param name="cantAno"> Cntidad de años a desplegar en el listado</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_AnoVehiculo(int? modelo  )
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (from x in db.ANO_VEHICULO
                        where x.CD_MODELO == modelo
                        && x.IN_INACTIVO == false
                        orderby x.CD_ANO
                        select new SelectListItem()
                        {
                            Value = x.CD_ANO.ToString()
                        ,   Text = x.CD_ANO.ToString()
                        }

                        ).Distinct().ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Permite obtener un listado de años desde el año actual +1 hasta (año actual - cantAno)
        /// </summary>
        /// <param name="cantAno"> Cntidad de años a desplegar en el listado</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Ano(int cantAno) {
            List<SelectListItem> anos = new List<SelectListItem>();
            int anoActual = DateTime.Now.Year;

            for (int i = anoActual + 1; i > anoActual - cantAno; i--) 
                anos.Add(new SelectListItem {Text = i.ToString(), Value = i.ToString() });
            return anos;

        }


        /// <summary>
        /// Obtiene el listado de clientes registrados
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Cliente() {

            ClienteRepositorio repoCliente = new ClienteRepositorio();

            return  (from x in  repoCliente.obtenClientes()
                     where x.IndicadorEstaActivo == true
                     //&& x.IndicadorEstaRegistrado == true
                     select new SelectListItem {
                          Text = x.Nombre+" ("+x.TipoDocumentoSeleccionado+"-"+ x.Cedula+")"
                         ,Value = x.Id.ToString() 
                     }
                    );
        }

        /// <summary>
        /// Devuelve el origen de la notificación
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_OrigenNotificacion() {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (from x in db.ORIGEN_NOTIFICACION
                        where x.IN_GENERA_ALERTA == false
                        select new SelectListItem {
                            Text = x.DE_ORIGEN_NOTIFICACION
                            ,Value = x.ID_ORIGEN_NOTIFICACION.ToString()

                        }).ToList().AsEnumerable();
            }
        }


        /// <summary>
        /// Retorna las unidades Administrativas
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_UnidadAdministrativa()
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (from x in db.UNIDAD_ADMINISTRATIVA
                        orderby x.NM_UNIDAD_ADMINISTRATIVA
                        select new SelectListItem
                        {
                            Text = x.NM_UNIDAD_ADMINISTRATIVA
                            ,
                            Value = x.CD_UNIDAD_ADMINISTRATIVA.ToString()

                        }).ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Retorna los cargos de la unidadad administrativa
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Cargo(int? cdUnidadAdministrativa){
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (from x in db.CARGO
                        where x.CD_UNIDAD_ADMINISTRATIVA == cdUnidadAdministrativa
                        orderby x.NM_CARGO
                        select new SelectListItem
                        {
                            Text = x.NM_CARGO
                            ,
                            Value = x.CD_CARGO.ToString()

                        }).ToList().AsEnumerable();
            }
        }

        /// <summary>
        /// Retorna los posibles cargos padres
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_CargoPadre(int? cdCargo)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                    return (from x in db.CARGO
                            where x.CD_CARGO != cdCargo
                            orderby x.DE_CARGO
                            select new SelectListItem
                            {
                                Text = x.NM_CARGO + " (" + x.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA + ")"
                                ,
                                Value = x.CD_CARGO.ToString() 

                            }).ToList().AsEnumerable();                
            }
        }


        /// <summary>
        /// Lista el segmento de negocio 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_SegmentoNegocio() {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.SEGMENTO_NEGOCIO
                        where x.IN_INACTIVO == false
                        orderby x.NM_SEGMENTO_NEGOCIO
                        select new SelectListItem
                        {
                             Value = x.CD_SEGMENTO_NEGOCIO.ToString()
                            ,Text  =  x.NM_SEGMENTO_NEGOCIO
                        }).ToList().AsEnumerable();
            }
        }

        /// <summary>
        /// Retorna el listado de socios comerciales
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_SocioComercial() {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.SOCIO_COMERCIAL
                        where x.IN_INACTIVO == false
                        orderby x.NM_SOCIO_COMERCIAL
                        select new SelectListItem {
                            Value = x.CD_SOCIO_COMERCIAL.ToString()
                            ,Text = x.NM_SOCIO_COMERCIAL
                        }).ToList().AsEnumerable();
            }
        }

        /// <summary>
        /// Retorna el listado de socios comerciales que tiene el usuario 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_SocioComercial_ByUsuarioId(int? IdUsuario )
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                var resultado = (from x in db.SOCIO_COMERCIAL
                        where x.IN_INACTIVO == false
                        orderby x.NM_SOCIO_COMERCIAL
                        select new SelectListItem
                        {
                            Value = x.CD_SOCIO_COMERCIAL.ToString()
                            ,
                            Text = x.NM_SOCIO_COMERCIAL
                            ,Selected = (x.USUARIO.USUARIO1.Select(y => y.CD_USUARIO).Contains((int)IdUsuario)? true :false)
                        }).ToList().AsEnumerable();

                return resultado;
            }
        }


        /// <summary>
        /// Lista de servicios de la aplicación móvil
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_ServiciosAppMovil(int? CodigoSegmentoNegocio ) {

            using (IntranetSAIEntities db = new IntranetSAIEntities() ) {

                return (from x in db.MENU_SERVICIO
                          where x.SEGMENTO_NEGOCIO.Select(y=> y.CD_SEGMENTO_NEGOCIO).ToList().Contains((int)CodigoSegmentoNegocio) ==true      
                        select new SelectListItem {
                           Text = x.NM_SERVICIO
                           ,Value = x.CD_MENU_SERVICIO.ToString()
                       }).ToList().AsEnumerable();
            }
        }



        /// <summary>
        /// Obtiene el listado de sucursales
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Sucursal() {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.SUCURSAL
                        where x.IN_INACTIVA == false
                        orderby x.NM_SUCURSAL
                        select new SelectListItem() {
                             Text = x.NM_SUCURSAL
                            ,Value = x.CD_SUCURSAL.ToString()
                        }).ToList().AsEnumerable();

            }
        }

        /// <summary>
        /// Retorna los cargos de la unidadad administrativa
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_Supervisor(int? cdCargo, int? cdUsuarioModif){
            IEnumerable<SelectListItem> result = new List<SelectListItem>();
            
            using (IntranetSAIEntities db = new IntranetSAIEntities()){
                
                //Se obtiene el cargo padre
                int? cargoPadre = (from x in db.CARGO
                                  where x.CD_CARGO == cdCargo
                                  select x.CD_CARGO_PADRE
                                  ).FirstOrDefault();
                
                if (cargoPadre != null) {
                    result = (from x in db.EMPLEADO
                              where x.CD_CARGO == cargoPadre
                              && x.USUARIO.IN_USUARIO_INACTIVO == false
                              && x.USUARIO.CD_USUARIO != cdUsuarioModif
                              orderby x.USUARIO.DE_NOMBRE_APELLIDO
                              select new SelectListItem{
                                  Text = x.USUARIO.DE_NOMBRE_APELLIDO + " ("+ x.CARGO.DE_CARGO+ ")"
                                  ,Value = x.CD_USUARIO.ToString()
                              }).ToList().AsEnumerable();

                    if (result.Count() == 0)
                       return obtenSelect_Supervisor(cargoPadre, cdUsuarioModif);

               }               
            }
            return result;
        }
        

        /// <summary>
        /// Retorna los posibles sustitos del empleado
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_SupervisorReemplazo(int idEmpleado, int cdCargo){
            IEnumerable<SelectListItem> result = new List<SelectListItem>();

            using (IntranetSAIEntities db = new IntranetSAIEntities()){

                    result = (from x in db.EMPLEADO
                              where x.CD_CARGO == cdCargo
                              && x.USUARIO.IN_USUARIO_INACTIVO == false
                              && x.USUARIO.CD_USUARIO != idEmpleado
                              orderby x.USUARIO.DE_NOMBRE_APELLIDO
                              select new SelectListItem
                              {
                                  Text = x.USUARIO.DE_NOMBRE_APELLIDO + " (" + x.CARGO.DE_CARGO + ")"
                                  ,
                                  Value = x.CD_USUARIO.ToString()

                              }).ToList().AsEnumerable();

                if (result.Count() == 0) {
                    int? cargoPadre = (from x in db.CARGO
                                       where x.CD_CARGO == cdCargo
                                       select x.CD_CARGO_PADRE
                                ).FirstOrDefault();

                    if (cargoPadre != null)
                        return obtenSelect_SupervisorReemplazo(idEmpleado, (int)cargoPadre);

                }
            }
            return result;
        }

        /// <summary>
        /// Retorna la lista de dispositivos por distribuidor. Si el distribuidor es null, trae todos los dispositivos
        /// </summary>
        /// <param name="IdDistribuidor"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_DispositivosPorDistribuidor(int? IdDistribuidor) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.DISPOSITIVO
                        where x.ID_DISTRIBUIDOR == (IdDistribuidor == null|| IdDistribuidor == 0? x.ID_DISTRIBUIDOR: IdDistribuidor)
                        orderby x.NM_DISPOSITIVO
                        select new SelectListItem {
                             Value = x.ID_DISPOSITIVO.ToString()
                            ,Text = x.NM_DISPOSITIVO!=null&& x.NM_DISPOSITIVO!="" ? x.NM_DISPOSITIVO  : "S/N: "+x.NU_SERIAL 
                        }).ToList().AsEnumerable();
            }
        }
        
        /// <summary>
        /// Permite obtener el listado de dispositivos asociados a un usuario
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_DispositivosPorCliente(int? idCliente) {
            List<SelectListItem> result = new List<SelectListItem>();

            if (idCliente != null)
            {
                using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                    result = (from x in db.DISPOSITIVO_CLIENTE
                              where x.CD_CLIENTE == idCliente                              
                              orderby x.DISPOSITIVO.NM_DISPOSITIVO
                              select new SelectListItem
                              {
                                  Text = x.DISPOSITIVO.NU_SIM_GPS + " " + x.DISPOSITIVO.NM_DISPOSITIVO
                                  ,
                                  Value = x.ID_DISPOSITIVO.ToString()
                              }

                              ).ToList();

                }                    
            }
            else {
                //Se busca en device 
                using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                    result = (from x in db.DISPOSITIVO
                              where x.IN_ELIMINADO == false
                              orderby x.NM_DISPOSITIVO
                              select new SelectListItem
                              {
                                  Text = x.NU_SIM_GPS + " " + x.NM_DISPOSITIVO
                                  ,
                                  Value = x.ID_DISPOSITIVO.ToString()
                              }).ToList();
                 }
            }
            return result;
        }



        /// <summary>
        /// Permite obtener el listado de dispositivos adsignables a un cliente
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_DispositivosAsignablesCliente(int IdCliente)
        {
          
                using (IntranetSAIEntities db = new IntranetSAIEntities())
                {

                    return   (from x in db.DISPOSITIVO
                              where x.DISPOSITIVO_CLIENTE == null
                              && 
                              (( (from y in db.USUARIO
                                  where y.USUARIO1.Select(z=> z.CD_USUARIO).Contains(IdCliente)
                                  select y.USUARIO2.Select(z=>z.SOCIO_COMERCIAL)
                                  ).Count()==0        
                              )||( x.USUARIO.Select(z=> z.CD_USUARIO)
                              .Intersect(from c in db.SOCIO_COMERCIAL
                                          where c.USUARIO.USUARIO1.Select(z => z.CD_USUARIO).Contains(IdCliente)
                                          select c.CD_SOCIO_COMERCIAL
                               ).Any() == true
                              ))
                              && x.DISPOSITIVO_CLIENTE == null
                              && x.IN_ELIMINADO == false
                              orderby x.NM_DISPOSITIVO
                              select new SelectListItem
                              {
                                  Text = x.DE_DISPOSITIVO + " (" + (!(x.NM_DISPOSITIVO == null || x.NM_DISPOSITIVO.Trim() == string.Empty)? x.NM_DISPOSITIVO:"S/N: "+x.NU_SERIAL )+ " )"
                                  ,
                                  Value = x.ID_DISPOSITIVO.ToString()
                              }

                              ).ToList();
               }
        }


        /// <summary>
        /// Permite obtener el listado sugerencias de vin adsignables a un vehículo cliente
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> obtenSelect_VinAsignables(int IdCliente)
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                return (from x in db.DISPOSITIVO
                        where x.DISPOSITIVO_CLIENTE == null
                        &&
                        (((from y in db.USUARIO
                           where y.USUARIO1.Select(z => z.CD_USUARIO).Contains(IdCliente)
                           select y.USUARIO2.Select(z => z.SOCIO_COMERCIAL)
                            ).Count() == 0
                        ) || (x.USUARIO.Select(z => z.CD_USUARIO)
                        .Intersect(from c in db.SOCIO_COMERCIAL
                                   where c.USUARIO.USUARIO1.Select(z => z.CD_USUARIO).Contains(IdCliente)
                                   select c.CD_SOCIO_COMERCIAL
                         ).Any() == true
                        ))
                        && (from z in db.VEHICULO
                            where z.NU_VIN == x.DE_DISPOSITIVO
                            select z).Count()==0
                        && !(x.DE_DISPOSITIVO == null || x.DE_DISPOSITIVO.Trim() == string.Empty)
                        orderby x.NM_DISPOSITIVO
                        select new SelectListItem
                        {
                            Text = x.DE_DISPOSITIVO + " (" + (!(x.NM_DISPOSITIVO == null || x.NM_DISPOSITIVO.Trim() == string.Empty) ? x.NM_DISPOSITIVO : "S/N: " + x.NU_SERIAL) + " )"
                            ,
                            Value = x.DE_DISPOSITIVO
                        }

                          ).ToList();
            }
        }

        #endregion

        #region secuencia

        /// <summary>
        /// Retorna el número de secuenci para los usuarios
        /// </summary>
        /// <returns></returns>
        public static int obtenSecuenciaUsuario() {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                int sequence = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR SEGURIDAD.SEQ_USUARIO").FirstOrDefault();
                return sequence;
            }
        }        
        #endregion

    }
}