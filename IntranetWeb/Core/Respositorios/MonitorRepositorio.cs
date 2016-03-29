using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Monitor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace IntranetWeb.Core.Respositorios
{
    public class MonitorRepositorio
    {

        public MonitorRepositorio() {}
        /// <summary>
        /// Método para obtener el listado de alertas
        /// </summary>
        /// <returns></returns>
        public IQueryable<Notificacion> obtenAlertas()
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.FUNC_001_BUSCA_ALERTAS()
                        orderby x.CD_PRIORIDAD_NOTIFICACION, x.FE_NOTIFICACION descending
                        select new Notificacion
                        {
                            Id = x.ID_NOTIFICACION
                            ,
                            IdOrigenNotificacion = x.ID_ORIGEN_NOTIFICACION
                            ,
                            DescipcionOrigenNotificacion = x.DE_ORIGEN_NOTIFICACION
                            
                            ,Icono = x.DE_ICONO

                            ,IdTipoNotificacion = x.TP_NOTIFICACION
                            ,
                            TipoNotificacion = x.DE_TIPO_NOTIFICACION
                            ,
                            ColorTitulo = x.DE_COLOR_TITULO
                            
                            ,
                            ColorCuerpo = x.DE_COLOR_BODY
                            
                            ,ColorEtiqueta = x.DE_COLOR_ETIQUETA
                            , FechaNotificacion = x.FE_NOTIFICACION
                            
                           ,IndicadorLocalizacion = x.IN_LOCALIZACION
                            
                           ,Imagen = x.DE_CONTENIDO_IMAGEN
                         
                           ,NombreImagen = x.NM_IMAGEN
                           
                            ,Distribuidor = new Distribuidor
                            {
                                Id = x.ID_DISTRIBUIDOR
                                ,
                                Nombre = x.NM_DISTRIBUIDOR

                            }
                            ,
                            Dispositivo = new Dispositivo
                            {
                                Id = x.ID_DISPOSITIVO
                                ,
                                Nombre = x.NM_DISPOSITIVO
                                ,
                                DVR = x.NU_DVR
                                ,
                                Observacion = x.DE_DISPOSITIVO
                                 ,
                                Vin = x.NU_VIN
                                ,
                                SimCard = x.NU_SIM_GPS
                            }
                            ,
                            Cliente = new UsuarioMovil
                            {
                                Id = x.CD_CLIENTE
                                ,NumeroDocumento = x.NU_DOCUMENTO_IDENTIDAD
                                ,NombreCompleto = x.DE_NOMBRE_APELLIDO_RAZON_SOCIAL 
                                ,TelefonoMovil = x.NU_TELEFONO_MOVIL
                                
                            }
                        }
                     ).Take(50).ToList().AsQueryable();
            }

        }


        /// <summary>
        /// Permite obtener un ticket según el origen del mismo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idOrigenNotificacion"></param>
        /// <returns>Objeto TICKET</returns>
        public TICKET obten_TICKET_ByOrigen(int id
                                          , int idOrigenNotificacion)
        {
            using (var db = new IntranetSAIEntities())
            {
                return (from x in db.TICKET
                        where x.ID_NOTIFICACION == id
                           && x.ID_ORIGEN_NOTIFICACION == idOrigenNotificacion
                        select x).FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca una notificación por Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idOrigenNotificacion"></param>
        /// <returns></returns>
        public FUNC_001_BUSCA_ALERTAS_Result obtenNotificacion_ById(int id
                                                                 , int idOrigenNotificacion) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()){

              return  (from x in db.FUNC_001_BUSCA_ALERTAS()
                        where x.ID_NOTIFICACION == id
                        && x.ID_ORIGEN_NOTIFICACION == idOrigenNotificacion
                        select x).FirstOrDefault();
            }
        }


        /// <summary>
        /// Devuelve el objeto TICKET  que corresponda con el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TICKET obten_TICKET_ById(int id){
           return  obten_TICKET_ById(id, null);            
        }


        /// <summary>
        /// Devuelve el objeto TICKET  que corresponda con el id, con susu respectivos include
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TICKET obten_TICKET_ById(int id, IList<Expression<Func<TICKET, object>>> includes)
        {
            using (var db = new IntranetSAIEntities())
            {
                var ticket = (from x in db.TICKET
                            where x.ID_TICKET == id
                            select x);

                if (includes != null)
                    includes.ToList().ForEach(x => ticket = ticket.Include(x));

                return ticket.FirstOrDefault();
            }
        }



        /// <summary>
        /// Retorna las notificación que no han sido gestionadas
        /// </summary>
        /// <param name="idNotificacion"></param>
        /// <param name="origenNotificacion"></param>
        /// <returns></returns>
        public NOTIFICACION_SIN_GESTION obten_NOTIFICACION_SIN_GESTION_ById(int idNotificacion
                                                                            ,int idOrigenNotificacion ) {
            using (var db = new IntranetSAIEntities()) {
                return (from x in db.NOTIFICACION_SIN_GESTION
                        where x.ID_NOTIFICACION == idNotificacion
                        && x.ID_ORIGEN_NOTIFICACION == idOrigenNotificacion
                        select x).FirstOrDefault();
            }
        }




        /// <summary>
        /// Guarda un ticket en la tabla de tickets
        /// </summary>
        /// <param name="ticket"> ticket a guardar</param>
        /// <returns></returns>
        public int guarda_TICKET(TICKET ticket)
        {

            using (var db = new IntranetSAIEntities())
            {
                db.TICKET.Add(ticket);
                db.SaveChanges();
                TICKET_HISTORICO tiHis = ticket.TICKET_to_TICKET_HISTORICO();                
                db.TICKET_HISTORICO.Add(tiHis);
                return db.SaveChanges();
            }
        }
        
        /// <summary>
        /// Guarda la notificación sin gestión
        /// </summary>
        /// <param name="notificacion"></param>
        /// <returns></returns>
        public int guarda_NOTIFICACION_SIN_GESTION(NOTIFICACION_SIN_GESTION notificacion) {
            using (var db = new IntranetSAIEntities()) {

                db.NOTIFICACION_SIN_GESTION.Add(notificacion);
                return db.SaveChanges();
                
            }            
        }
        

        /// <summary>
        /// Obtiene los datos de un ticket dado el id
        /// </summary>
        /// <param name="idTicket"></param>
        /// <returns> Objeto Ticket</returns>
        public Ticket obten_Ticket_ById(int idTicket) {

            Ticket ticket = new Ticket();
            
            //Se busca la información del ticket
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                ticket = (from x in db.TICKET
                          where x.ID_TICKET == idTicket
                           select new Ticket{
                                     Id = x.ID_TICKET
                                    ,EstatusSeleccionado = x.ID_ESTATUS_TICKET
                                    ,DescripcionEstatus     = x.TICKET_ESTATUS.DE_ESTATUS
                                    ,FechaEstatus           = x.FE_ESTATUS
                                    ,FechaCreacion          = x.FE_CREACION
                                    ,TipoAtencionSeleccionada = x.TP_ATENCION
                                    ,DescripcionTipoAtencion  = x.TIPO_ATENCION.DE_TIPO_ATENCION   
                                    ,DescripcionMensajeAlCliente   = x.RESPUESTA_OPERADOR_GCM.DE_TITULO 
                                    ,MensajeAlClienteSeleccionado = x.ID_RESPUESTA_OPERADOR
                                    ,NombreUsuarioTicket      = x.USUARIO.DE_NOMBRE_APELLIDO
                                    ,NombreUsuarioTransferencia = x.USUARIO1.DE_NOMBRE_APELLIDO
                                    ,FechaNotificacion      = x.FE_NOTIFICACION
                                    ,Observacion            = x.DE_OBSERVACION
                                    ,ObservacionSupervisor  = x.DE_OBSERVACION_SUPERVISOR
                                    ,InMensajeRespuestaEnviado = x.IN_RESPUESTA_ENVIADA
                                    ,Cliente = new UsuarioMovil() {
                                             Id = x.CD_CLIENTE
                                            ,NumeroDocumento    = x.CLIENTE != null ?   x.CLIENTE.USUARIO.TP_DOCUMENTO_IDENTIDAD+ "-" + x.CLIENTE.USUARIO.NU_DOCUMENTO_IDENTIDAD : null
                                            ,NombreCompleto     = x.CLIENTE !=null  ?   x.CLIENTE.DE_NOMBRE_APELLIDO_RAZON_SOCIAL   :null
                                            ,Apellido           = x.CLIENTE != null ?  x.CLIENTE.DE_APELLIDO : null
                                            ,Nombre             = x.CLIENTE != null ? x.CLIENTE.DE_NOMBRE : null
                                            ,TelefonoMovil      = x.CLIENTE != null ? x.CLIENTE.USUARIO.NU_TELEFONO_MOVIL: null
                                    }
                                    ,Dispositivo = new Dispositivo {
                                        Id = x.ID_DISPOSITIVO
                                        
                                    }
                                    ,Distribuidor = new Distribuidor
                                    {
                                        Id = x.ID_DISTRIBUIDOR
                                    }
                                    ,TipoNotificacionSeleccionada   = x.TP_NOTIFICACION
                                    ,DescripcionTipoNotificacion = x.TIPO_NOTIFICACION.DE_TIPO_NOTIFICACION
                                    ,DescripcionOrigenNotificacion = x.ORIGEN_NOTIFICACION.DE_ORIGEN_NOTIFICACION
                                    ,OrigenNotificacionSeleccionada = x.ID_ORIGEN_NOTIFICACION
                                    ,UsuarioTicketSeleccionado      = x.CD_USUARIO
                           }).FirstOrDefault();
                
            }

            if (ticket != null) {
                ClienteRepositorio cr = new ClienteRepositorio();

                ticket = obten_DetalleDispositivoTicket(ticket);

                DISTRIBUIDOR us = cr.obten_DISTRIBUIDOR_ById(ticket.Distribuidor.Id);

                if (us != null)
                    ticket.Distribuidor.Nombre = us.NM_DISTRIBUIDOR;
                
            }    
            return ticket;
        }


        /// <summary>
        /// Dado el id del cliente en el objeto ticket, devulve el resto de la data
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private Ticket obten_DetalleDispositivoTicket(Ticket ticket) {

           
           /* if (ticket.Cliente.Id != null){
               
                IntranetWeb.ViewModel.Cliente.Dispositivo dispo = (from x in cliente.Dispositivos
                                                                   where x.Id == ticket.Dispositivo.Id
                                                                   select x
                                          ).FirstOrDefault();

                if (dispo != null){

                    ticket.Dispositivo.DVR = dispo.NumeroDVR;
                    ticket.Dispositivo.Nombre = dispo.NombreDispositivo;
                    ticket.Dispositivo.SimCard = dispo.TarjetaSim;
                    ticket.Dispositivo.Vin = dispo.Vin;

                    //Se buscan los datos  del vehículo
                    IntranetWeb.ViewModel.Cliente.Vehiculo veh = (from x in cliente.Vehiculos
                                                                  where x.Vin == dispo.Vin
                                                                  select x).FirstOrDefault();

                    if (veh != null){
                        ticket.Dispositivo.Placa = veh.NumeroPlaca;
                        
                    }
                }
            }*/


               

                DISPOSITIVO dev = null;
                using (IntranetSAIEntities db = new IntranetSAIEntities()){

                    dev = (from x in db.DISPOSITIVO
                           where x.ID_DISPOSITIVO == ticket.Dispositivo.Id
                           && x.IN_ELIMINADO == false
                           select x).Include(x => x.DISPOSITIVO_CLIENTE.VEHICULO)                           
                           .FirstOrDefault();

                }

                if (dev != null){                    
                    ticket.Dispositivo.DVR = dev.NU_DVR;
                    ticket.Dispositivo.SimCard = dev.NU_SIM_GPS;
					ticket.Dispositivo.Nombre = dev.NM_DISPOSITIVO;
                    ticket.Dispositivo.Vin = dev.DISPOSITIVO_CLIENTE!= null&&dev.DISPOSITIVO_CLIENTE.CD_VEHICULO!=null? dev.DISPOSITIVO_CLIENTE.VEHICULO.NU_VIN:null ;
                    ticket.Dispositivo.Placa = dev.DISPOSITIVO_CLIENTE != null && dev.DISPOSITIVO_CLIENTE.CD_VEHICULO != null ? dev.DISPOSITIVO_CLIENTE.VEHICULO.NU_PLACA : null;
                }


            return ticket; 
        }



        /// <summary>
        /// Obtiene los datos de un ticket dada la notificación
        /// </summary>
        /// <param name="idTicket"></param>
        /// <returns> Objeto Ticket</returns>
        public Ticket obten_Ticket_ByNotificacion(Notificacion notificacion){

            var ticket = new Ticket();
            TICKET ticketDB = null;
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
               ticketDB = (from x in db.TICKET
                                where x.ID_NOTIFICACION == notificacion.Id
                                && x.ID_ORIGEN_NOTIFICACION == notificacion.IdOrigenNotificacion
                                select x
                      ).FirstOrDefault();
                
             }

            if (ticketDB != null)
                ticket = obten_Ticket_ById(ticketDB.ID_TICKET);
            else {

                var  notificacionResult = obtenNotificacion_ById(notificacion.Id
                                                                   , notificacion.IdOrigenNotificacion);


                ticket.Dispositivo.Id = notificacionResult.ID_DISPOSITIVO;
                ticket.Distribuidor.Id = notificacionResult.ID_DISTRIBUIDOR;
                using (IntranetSAIEntities db = new IntranetSAIEntities())
                {
                    ticket.Cliente = (from x in db.CLIENTE
                                      where x.CD_CLIENTE == notificacionResult.CD_CLIENTE
                                      select new UsuarioMovil()
                                      {
                                          Id = x.CD_CLIENTE
                                            ,
                                          NumeroDocumento = x.USUARIO.TP_DOCUMENTO_IDENTIDAD + "-" + x.USUARIO.NU_DOCUMENTO_IDENTIDAD
                                            ,
                                          NombreCompleto = x.DE_NOMBRE_APELLIDO_RAZON_SOCIAL
                                            ,
                                          Apellido = x.DE_APELLIDO
                                            ,
                                          Nombre = x.DE_NOMBRE
                                            ,
                                          TelefonoMovil = x.USUARIO.NU_TELEFONO_MOVIL
                                      }).FirstOrDefault();
                }

                ticket.Cliente = ticket.Cliente == null ? new UsuarioMovil() : ticket.Cliente;
                ticket.Cliente.Id =  notificacionResult.CD_CLIENTE;
                ticket.TipoNotificacionSeleccionada = notificacionResult.TP_NOTIFICACION;
                
               
                //Se busca la información del cliente
                ticket = obten_DetalleDispositivoTicket(ticket);

                ClienteRepositorio cr = new ClienteRepositorio();
                
                DISTRIBUIDOR us = cr.obten_DISTRIBUIDOR_ById(ticket.Distribuidor.Id);

                if (us != null) 
                    ticket.Distribuidor.Nombre = us.NM_DISTRIBUIDOR;
                
            }

            return ticket;
        }

        /// <summary>
        /// PERMITE ACTUALIZAR UN TICKET
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public int actualiza_TICKET(TICKET ticket){

            using (var db = new IntranetSAIEntities()){

                db.TICKET.Attach(ticket);
                TICKET_HISTORICO tiHis = ticket.TICKET_to_TICKET_HISTORICO();
                db.TICKET_HISTORICO.Add(tiHis);

                db.Entry(ticket).State = EntityState.Modified;
                return db.SaveChanges();
            }

        }


        /// <summary>
        /// Obtiene el listado de Tickets que tiene asociado el usuario
        /// </summary>
        /// <param name="showUser">Valida si debe mostrar o no el usuario asociado l ticket </param>
        /// <param name="cdUsuario"> Código de usuario conectado</param>
        /// <param name="feDesde"></param>
        /// <param name="feHasta"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IQueryable<Ticket> obten_Tickets(bool showUser, int? cdUsuario, int? tpAtencion, string nuSim, DateTime? feDesde, DateTime? feHasta,int? top) {
            IQueryable<Ticket> listadoTickets = new List<Ticket>().AsQueryable();


            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                if (showUser)
                {
                    var result = (from x in db.FUNC_002_BUSCA_TICKET(cdUsuario, tpAtencion, nuSim, feDesde, feHasta)
                                  orderby x.FE_STATUS descending
                                  select new Ticket
                                  {
                                      Id = x.ID_TICKET
                                      ,
                                      FechaEstatus = x.FE_STATUS
                                      ,
                                      NombreUsuarioTicket = x.NM_USUARIO
                                      ,
                                      DescripcionEstatus = x.DE_ESTATUS
                                     
                                      ,EstatusSeleccionado = (int)x.CD_ESTATUS
                                      , DescripcionTipoNotificacion = "<span class=\"label label-sm " + x.DE_COLOR_TITULO + "\">" + x.DE_TIPO_NOTIFICACION + "</span>"
                                      ,
                                      Cliente = new UsuarioMovil
                                      {
                                           Id = x.CD_CLIENTE
                                          ,NumeroDocumento = x.CD_CLIENTE!=null?x.TP_DOCUMENTO_IDENTIDAD+"-"+x.NU_DOCUMENTO_IDENTIDAD:null
                                          ,Nombre = x.DE_NOMBRE_CLIENTE
                                          ,Apellido = x.DE_APELLIDO_CLIENTE
                                          ,NombreCompleto = x.DE_NOMBRE_APELLIDO_RAZON_SOCIAL
                                      }
                                          ,
                                      Dispositivo = new Dispositivo
                                      {
                                          DVR = x.NU_DVR
                                          , SimCard = x.NU_SIM
                                      }

                                          ,
                                      Distribuidor = new Distribuidor
                                      {
                                          Nombre = x.NM_DISTRIBUIDOR
                                      }
                                  });
                            
                            if(top!=null)
                                result = result.Take((int)top);
                            
                            listadoTickets = result.ToList().AsQueryable();
                    
                }
                else
                {

                    var result = (from x in db.FUNC_002_BUSCA_TICKET(cdUsuario, tpAtencion, nuSim, feDesde, feHasta)
                                  orderby x.FE_STATUS descending
                                  select new Ticket
                                  {
                                      Id = x.ID_TICKET
                            ,
                                      FechaEstatus = x.FE_STATUS
                            ,
                                      DescripcionEstatus = x.DE_ESTATUS
                                        ,
                                      EstatusSeleccionado = (int)x.CD_ESTATUS
                            ,
                                      DescripcionTipoNotificacion = "<span class=\"label label-sm " + x.DE_COLOR_TITULO + "\">" + x.DE_TIPO_NOTIFICACION + "</span>"
                            ,
                                      Cliente = new UsuarioMovil
                                      {
                                             Id = x.CD_CLIENTE
                                            ,Nombre = x.DE_NOMBRE_CLIENTE
                                            ,Apellido = x.DE_APELLIDO_CLIENTE                                          
                                            ,NumeroDocumento = x.CD_CLIENTE != null ? x.TP_DOCUMENTO_IDENTIDAD + "-" + x.NU_DOCUMENTO_IDENTIDAD : null
                                            ,NombreCompleto = x.DE_NOMBRE_APELLIDO_RAZON_SOCIAL
                                      }
                                ,
                                      Dispositivo = new Dispositivo
                                      {
                                            DVR = x.NU_DVR
                                           ,SimCard = x.NU_SIM
                                      }

                                ,
                                      Distribuidor = new Distribuidor
                                      {
                                          Nombre = x.NM_DISTRIBUIDOR
                                      }
                                  });

                    if (top != null)
                        result = result.Take((int)top);

                    listadoTickets =  result.ToList().AsQueryable();
                    
                }
            }
            listadoTickets.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>(){ {"data-id-ticket",x.Id.ToString()}},true, true, false) );
           
            return listadoTickets;
        }

        /// <summary>
        /// Obtiene los datos para el envío de mensaje vía CMS dado un ticket
        /// </summary>
        /// <param name="ticketID"></param>
        /// <returns></returns>
        public dynamic obtenMensajeGCM_byTicketId(int ticketID) {
            IEnumerable<string> listGCM = new List<string>();
            int idMensaje=0;
            String titulo = "", mensaje= ""; 

            TICKET tick = this.obten_TICKET_ById(ticketID, new List<Expression<Func<TICKET, object>>> { (x => x.RESPUESTA_OPERADOR_GCM) });

            if (tick != null) {
                //Se obtiene el GCM_ID
                using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                    //Se busca el usuario asociado al dispositivo
                    listGCM = (
                                   from y in db.USUARIO_MOVIL_GCM 
                                   where y.CD_USUARIO == tick.CD_CLIENTE
                                   && y.IN_ACTIVO == true
                                   select y.ID_GCM).ToList();
                    
                }
               idMensaje = tick.RESPUESTA_OPERADOR_GCM.ID_RESPUESTA_OPERADOR;
               titulo = "Ticket Nº "+tick.ID_TICKET+": "+tick.RESPUESTA_OPERADOR_GCM.DE_TITULO;
               mensaje = tick.RESPUESTA_OPERADOR_GCM.DE_CONTENIDO;
                
            }

            return new  {
                 GCM_ID = listGCM
                ,IdMensaje = idMensaje
                ,Titulo = titulo
               ,Mensaje = mensaje
            }; 
        }


        /// <summary>
        /// Retorna los datos del usuario móvil dado el id de la notificación
        /// </summary>
        /// <param name="idNotificacion"></param>
        /// <returns></returns>
        public UsuarioMovil obtenUsuarioMovil_ByNotificacionId(int idNotificacion) {

            //Se busca la información del usuario
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (from x in db.USUARIO_NOTIFICACION
                               //from y in db.SAI_usersDevices
                               from w in db.IMAGEN_NOTIFICACION
                               from z in db.CLIENTE
                               where x.ID_NOTFICACION == idNotificacion
                               //&& y.deviceID.ToString() == x.deviceID
                               && w.ID_NOTIFICACION == x.ID_NOTFICACION
                               && x.CD_USUARIO == z.CD_CLIENTE
                               select new UsuarioMovil(){
                                     Id                 = z.CD_CLIENTE
                                    ,NumeroDocumento    =  x.USUARIO.NU_DOCUMENTO_IDENTIDAD  
                                    ,Apellido           = z.DE_APELLIDO
                                    ,NombreCompleto     = z.DE_NOMBRE_APELLIDO_RAZON_SOCIAL
                                    ,Latitud             = w.NU_LATITUD
                                    ,Longitud = w.NU_LONGITUD
                                    ,Nombre = z.DE_NOMBRE
                                    ,TelefonoMovil = z.USUARIO.NU_TELEFONO_MOVIL
                                    ,Ubicacion = w.DE_LOCALIZACION
                               }).FirstOrDefault();
            }
        }



        /// <summary>
        /// Retorna la bitácor del ticket
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IEnumerable<Ticket> obten_HistoricoTicket(int Id) {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.TICKET_HISTORICO                        
                        where x.ID_TICKET == Id                    
                        orderby x.FE_ESTATUS descending
                        select new Ticket {
                             NombreUsuarioTicket = (from y in db.USUARIO
                                                    where x.CD_USUARIO == y.CD_USUARIO
                                                    select y.DE_NOMBRE_APELLIDO).FirstOrDefault()
                            ,DescripcionTipoAtencion =  (from w in db.TIPO_ATENCION 
                                                         where w.TP_ATENCION == x.TP_ATENCION
                                                         select w.DE_TIPO_ATENCION).FirstOrDefault()
                            ,
                            DescripcionEstatus = (from z in db.TICKET_ESTATUS
                                                  where z.ID_ESTATUS_TICKET == x.ID_ESTATUS_TICKET
                                                  select z.DE_ESTATUS).FirstOrDefault()
                            ,FechaEstatus = x.FE_ESTATUS
                            ,EstatusSeleccionado = x.ID_ESTATUS_TICKET
                            ,Observacion = x.DE_OBSERVACION
                            ,ObservacionSupervisor = x.DE_OBSERVACION_SUPERVISOR
                        }).ToList().AsEnumerable();
            }
        }


        /// <summary>
        /// Obtiene el listado de distribuidores para el mapa 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public ArbolDispositivo obtenDispositivosNested(string token) {
            ArbolDispositivo result = new ArbolDispositivo();
            IList<int> distribuidoresExcluidos = new List<int>(new int[]{ 19, 22 });

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                //Se buscan los Distribuidores
                result.Distribuidores = (
                    from x in db.DISTRIBUIDOR
                    where !distribuidoresExcluidos.Contains(x.ID_DISTRIBUIDOR)
                    orderby x.NM_DISTRIBUIDOR
                    where x.IN_ELIMINADO == false
                    && (from y in db.DISPOSITIVO
                        where  y.ID_DISTRIBUIDOR == x.ID_DISTRIBUIDOR
                        && y.IN_ELIMINADO == false
                        && y.NM_DISPOSITIVO != ""
                        && y.NM_DISPOSITIVO != null
                        select y).Count() > 0
                    select  new NodoDistribuidor {
                          Distribuidor = new Distribuidor {
                               Id = x.ID_DISTRIBUIDOR
                              ,Nombre = x.NM_DISTRIBUIDOR
                          }                        
                        }
                    ).ToList().AsEnumerable();
            }

            result.Distribuidores.ToList().ForEach(x => x.DispositivosAsociados = ((from y in Core.WebService.ApiPlataforma.DevicesClient.GetDevicesList(x.Distribuidor.Id, token) //y in db.Devices
                                                                                    orderby y.Name
                                                                                    select new Dispositivo
                                                                                    {
                                                                                         Id = y.Id
                                                                                        ,Nombre = y.Name
                                                                                        ,DescripcionEstatus = y.Status
                                                                                        ,Serial = y.SerialNumber
                                                                                        ,Icono = y.Icon                                                                                        
                                                                                    }).ToList().AsEnumerable()));              

            return result;
        }


        /// <summary>
        /// Obtiene el listado de distribuidores para el mapa 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ArbolDispositivo obtenDispositivosNested(string token, int IdUsuario )
        {
            ArbolDispositivo result = new ArbolDispositivo();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                //Se buscan los Distribuidores
                result.Distribuidores = (
                    from x in db.DISTRIBUIDOR
                    orderby x.NM_DISTRIBUIDOR
                    where x.IN_ELIMINADO == false
                    && (from y in db.DISPOSITIVO
                        where y.ID_DISTRIBUIDOR == x.ID_DISTRIBUIDOR
                        && y.IN_ELIMINADO == false
                        && y.NM_DISPOSITIVO != ""
                        && y.NM_DISPOSITIVO != null
                        && y.USUARIO.Select(w => w.CD_USUARIO).Contains(IdUsuario)
                        select y).Count() > 0
                    select new NodoDistribuidor
                    {
                        Distribuidor = new Distribuidor
                        {
                            Id = x.ID_DISTRIBUIDOR
                              ,
                            Nombre = x.NM_DISTRIBUIDOR
                        }
                    }
                    ).ToList().AsEnumerable();


                result.Distribuidores.ToList().ForEach(x => x.DispositivosAsociados = ((from y in Core.WebService.ApiPlataforma.DevicesClient.GetDevicesList(x.Distribuidor.Id, token) //y in db.Devices
                                                                                        from w in db.DISPOSITIVO
                                                                                        where w.ID_DISPOSITIVO == y.Id
                                                                                        && w.USUARIO.Select(z => z.CD_USUARIO).Contains(IdUsuario)
                                                                                        orderby y.Name
                                                                                        select new Dispositivo
                                                                                        {
                                                                                            Id = y.Id
                                                                                            ,
                                                                                            Nombre = y.Name
                                                                                            ,
                                                                                            DescripcionEstatus = y.Status
                                                                                            ,
                                                                                            Serial = y.SerialNumber
                                                                                            ,
                                                                                            Icono = y.Icon
                                                                                        }).ToList().AsEnumerable()));
            }
            return result;
        }
    }
}


