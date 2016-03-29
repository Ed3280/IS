using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IntranetWeb.ViewModel.Monitor;
using IntranetWeb.Models;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Exception;
using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.ViewModel.Shared;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class MonitorController : Controller
    {
        private Log4NetLogger log;

        public MonitorController() {
            log = new Log4NetLogger();
          
        }
        private MonitorRepositorio monitorRepo = new MonitorRepositorio();

        #region Monitor de alertas (Admin y usuario normal)

        /// <summary>
        /// Página principal del monitor de alertas
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta)]
        public ActionResult VerMonitor()
        {

            return View();
        }

        /// <summary>
        /// Visual del administrador del monitor de alertas
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult VerMonitorAdmin(){

            return View();
        }

        /// <summary>
        /// Obtiene el listado de Alertas
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta, Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _obtenAlertas()
        {
            return View();
        }

        /// <summary>
        /// Permite obtener el listado de alertas
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta, Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _actualizaAlertas()
        {
            JsonResult result;
            IEnumerable<Notificacion> notiList = new List<Notificacion>();
            try {
                
                //Se cargan las listas de notificaciones            
                notiList = monitorRepo.obtenAlertas();
                return PartialView(notiList);
            }
            catch (Exception exc) {
                                
                log.Error(Resources.ErrorResource.Error100, exc);

                if (ControllerContext.IsChildAction)
                    return PartialView(notiList);
                else
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                
            }
            return result;
        }

        /// <summary>
        /// Permite Visualiazar el listado de tickets
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta, Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _getListadoTickets(Ticket ticket) {

            JsonResult result;
            try {
                cargaSelectTicket(ticket);
                ticket.UsuarioTicketSeleccionado = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();
                ticket.FechaBusquedaDesde = DateTime.Today.AddDays(-7);
                ticket.FechaBusquedaHasta = DateTime.Today;
                
                return PartialView(ticket);
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);

                if (ControllerContext.IsChildAction){
                    return PartialView(ticket);
                }
                else{
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                }
            }
            return result;
        }
        
        /// <summary>
        /// Muestra las alertas
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta)]
        public ActionResult _getConsultarTicket(int Id) {
            Ticket tk = new Ticket();
            JsonResult result;
            try {
                tk = monitorRepo.obten_Ticket_ById(Id);
                return View(tk);
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }

        /// <summary>
        /// Obtiene la consulta de un ticket (visual admin)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _getConsultarTicket_Admin(int Id)
        {
            JsonResult result;
            Ticket tk = new Ticket();
            try
            {
                tk = monitorRepo.obten_Ticket_ById(Id);
                return View(tk);
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }

        /// <summary>
        /// Consulta los datos del cliente dada la notificación
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta
                , Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _getConsultarNotificacion(Notificacion notificacion)
        {
            Ticket tk = new Ticket();
            JsonResult result;
            try{
                tk = monitorRepo.obten_Ticket_ByNotificacion(notificacion);
                return View(tk);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }

        

        /// <summary>
        /// Modal para la creación de un ticket desde cero
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta, Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _getCrearTicket()
        {
            NuevoTicket nt = new NuevoTicket();
            JsonResult result;

            try {
                nt.FechaNotificacion = DateTime.Now;
                cargaSelectNuevoTicket(nt);
                return View(nt);
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }


        /// <summary>
        /// Modal para editar un ticket (Visual agente)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta)]
        public ActionResult _getEditarTicket(int Id) {

            Ticket tk = new Ticket();
            JsonResult result;
            try{
                
                tk = monitorRepo.obten_Ticket_ById(Id);

                if (tk.EstatusSeleccionado == 2) //Atendido
                    throw new BussinessException(Core.Constante.Mensaje.Error.TicketAtendidoNoModificable);
                if (tk.EstatusSeleccionado == 4) //Cerrado
                    throw new BussinessException(Core.Constante.Mensaje.Error.TicketAtendidoNoModificable);
                
                cargaSelectTicket(tk);
                return View(tk);
            }
            catch (BussinessException exc)
            {
                result =  Core.Utils.UtilJson.Error(exc.Message); 
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
         }


        /// <summary>
        /// Muestra la modal para editar los tickets (visual admin)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _getEditarTicket_Admin(int Id) {
            Ticket tk = new Ticket();
            JsonResult result;

            try{

                tk = monitorRepo.obten_Ticket_ById(Id);
                cargaSelectTicket(tk);
                return View(tk);
            }
            catch (BussinessException exc){
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }


        /// <summary>
        /// Proceso de actualización del ticket (atender, descartar, transferir ...)
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta)]
        public JsonResult ActualizarTicket(Ticket ticket, string submitButton)
        {
            JsonResult resultado;
            
            try {
                
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                switch (submitButton)
                    {
                        case "atender":
                            resultado = atenderTicket(ticket);
                            break;
                        case "transferir":
                            resultado = transferirTicket(ticket);
                            break;
                        case "descartar":
                            resultado = descartarTicket(ticket);
                            break;
                        default:
                            throw new Core.Exception.ActionNotRegisteredException();
                    }
            }
            catch (BussinessException exc) {
                resultado = Core.Utils.UtilJson.Error(exc.Message);                
            }
            catch (ActionNotRegisteredException exc){
                log.Error(IntranetWeb.Core.Constante.Mensaje.Error.Error400, exc);
                resultado = Core.Utils.UtilJson.Error(IntranetWeb.Core.Constante.Mensaje.Error.Error400);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }



        /// <summary>
        /// Actualiza el monitor de alertas
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public JsonResult ActualizarTicket_Admin(Ticket ticket)
        {
            JsonResult resultado;

            try{

                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                if (ticket.EnviarRespuestaCliente){
                    if (ticket.EstatusSeleccionado != 2)
                        throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.TicketNoAtendido);

                    if(ticket.MensajeAlClienteSeleccionado==null)
                        throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.SeleccioneMensajeAlCliente);
                }

                TICKET tick_Modif = monitorRepo.obten_TICKET_ById(ticket.Id);
                tick_Modif.TP_ATENCION = ticket.TipoAtencionSeleccionada;
                tick_Modif.ID_ESTATUS_TICKET = ticket.EstatusSeleccionado;
                tick_Modif.FE_ESTATUS = DateTime.Now;
                tick_Modif.DE_OBSERVACION = ticket.Observacion;
                tick_Modif.DE_OBSERVACION_SUPERVISOR = ticket.ObservacionSupervisor;
                tick_Modif.ID_RESPUESTA_OPERADOR = ticket.MensajeAlClienteSeleccionado;
                if (ticket.UsuarioTicketSeleccionado != tick_Modif.CD_USUARIO)
                    tick_Modif.CD_USUARIO_TRANSFERENCIA = Core.Utils.UtilHelper.obtenUsuarioLogueado();
                tick_Modif.CD_USUARIO = ticket.UsuarioTicketSeleccionado;
                
                if (monitorRepo.actualiza_TICKET(tick_Modif) > 0)
                    resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.TicketActualizado, tick_Modif.ID_TICKET), new { IdTicket = tick_Modif.ID_TICKET, InEnvioCMS = ticket.EnviarRespuestaCliente });
                else
                    throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.FalloActualizarTicket);
                
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }

        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta
               , Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult getDetalleTicketHistorico(int id) {
            JsonResult resultado;
            try {

                Ticket tick = monitorRepo.obten_Ticket_ById(id);
                return PartialView("ConsultarTicket/_getDatosTicket_Admin", tick);

            }
            catch (Exception exc) {
               log.Error(Resources.ErrorResource.Error100, exc);
               resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }

        /// <summary>
        /// Método para obtener el listado de tickets vía Json
        /// </summary>
        /// <param name="FechaBusquedaDesde"></param>
        /// <param name="FechaBusquedaHasta"></param>
        /// <param name="UsuarioTicketSeleccionado"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta)]
        public ActionResult getTickets(
                          DateTime? FechaBusquedaDesde
                        , DateTime? FechaBusquedaHasta
                        , int? TipoAtencionSeleccionada
                        , int? UsuarioTicketSeleccionado
                        , DTParameters parametro) {
            JsonResult result;

            try{

                var dtsource = monitorRepo.obten_Tickets(false,  UsuarioTicketSeleccionado, TipoAtencionSeleccionada, null, FechaBusquedaDesde, FechaBusquedaHasta,null);


                List<Ticket> data = new ResultSet().GetResult(parametro.Search.Value, parametro.SortOrder, parametro.Start, parametro.Length, dtsource.ToList(), new List<string>());
                int count = new ResultSet().Count(parametro.Search.Value, dtsource.ToList(), new List<string>());

                DTResult<Ticket> resultData = new DTResult<Ticket>
                {
                    draw = parametro.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                result = Core.Utils.UtilJson.ExitoDataTableOnServer<Ticket>(resultData);

                return result;
            }
            catch (BussinessException exc){
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc){

                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);                
            }

            return result;
        }

        /// <summary>
        /// Obtiene el listado de tickets (Visual Admin)
        /// </summary>
        /// <param name="FechaBusquedaDesde"></param>
        /// <param name="FechaBusquedaHasta"></param>
        /// <param name="UsuarioTicketSeleccionado"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlertaAdmin )]
        public ActionResult getTickets_Admin(DateTime? FechaBusquedaDesde        
                        , DateTime? FechaBusquedaHasta
                        ,int? TipoAtencionSeleccionada
                        , int? UsuarioTicketSeleccionado
                        , DTParameters parametro)
        {
            JsonResult result;

            try
            {
                var dtsource = monitorRepo.obten_Tickets(true,UsuarioTicketSeleccionado, TipoAtencionSeleccionada, null, FechaBusquedaDesde, FechaBusquedaHasta,null);

                List<Ticket> data = new ResultSet().GetResult(parametro.Search.Value, parametro.SortOrder, parametro.Start, parametro.Length, dtsource.ToList(), new List<string>());
                int count = new ResultSet().Count(parametro.Search.Value, dtsource.ToList(), new List<string>());

                DTResult<Ticket> resultData = new DTResult<Ticket>
                {
                    draw = parametro.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                result = Core.Utils.UtilJson.ExitoDataTableOnServer<Ticket>(resultData);

                return result;
            }
            catch (BussinessException exc){
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);                
            }
            return result;
        }
        

        /// <summary>
        /// Método para guardar la observación del agente
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta)]
        public JsonResult grabarObservacionAgente(Ticket ticket) {
            JsonResult resultado; 
            String mensajeAdvertencia = "";
            DateTime feActual = DateTime.Now;
            try
            {
               
                TICKET tick_Modif = monitorRepo.obten_TICKET_ById(ticket.Id);

                tick_Modif.DE_OBSERVACION = ticket.Observacion;
                tick_Modif.FE_ESTATUS = feActual;

                if (monitorRepo.actualiza_TICKET(tick_Modif) > 0)
                    resultado = Core.Utils.UtilJson.Exito(IntranetWeb.Core.Constante.Mensaje.Exito.ObservacionTicketActualizada);
                else
                    throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.FalloActualizarTicket);
            }
            catch (BussinessException exc){
                resultado = Core.Utils.UtilJson.Error(exc.Message, new { mensajeAdvertencia = mensajeAdvertencia });
                
            }
            catch (Exception exc) {
              
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }

        /// <summary>
        /// Retorna el arbol de dispositivos 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta
                , Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _getNestedlDispositivos(){
            ArbolDispositivo ad;
            try {
                var resultWS = Core.WebService.ApiPlataforma.UsersClient.Login(1);
                string token = resultWS["Token"].Value;
                ad = monitorRepo.obtenDispositivosNested(token);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                ad = new ArbolDispositivo();
            }
            return View(ad);
        }


        /// <summary>
        /// Retorna el arbol de dispositivos 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.LocalizarVehiculo
            , Core.Constante.Aplicacion.MantenimientoInteligente
            )]
        public ActionResult cargaNestedlUsuarioDispositivos()
        {
            ArbolDispositivo ad;
            try
            {
                var resultWS = Core.WebService.ApiPlataforma.UsersClient.Login(1);
                string token = resultWS["Token"].Value;
                int IdUsuario = Core.Utils.UtilHelper.obtenUsuarioLogueado();
                ad = monitorRepo.obtenDispositivosNested(token, IdUsuario);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                ad = new ArbolDispositivo();
            }
            return View("_getNestedlDispositivos",ad);
        }

        /// <summary>
        /// Retorna los últimos 10 tickets del cliente
        /// </summary>
        /// <param name="nuSim"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta
                , Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _getHistoricoTicketsCliente(string nuSim) {
            IEnumerable<Ticket> listadoTickets = monitorRepo.obten_Tickets(true,null,null, nuSim, null, null, 10); //Obtiene top 10 de tickets
            return View("ConsultarTicket/_getHistoricoTicketsCliente", listadoTickets);
        }


        /// <summary>
        /// Retorna la bitácora del ticket
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta
                , Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public ActionResult _getBitacoraTicket(int Id)
        {
            IEnumerable<Ticket> listadoTickets = monitorRepo.obten_HistoricoTicket(Id);
            return View("ConsultarTicket/_getBitacoraTicket", listadoTickets);
        }


        /// <summary>
        /// Permite la creación de un nuevo ticket a partir de una notificación
        /// </summary>
        /// <param name="notificacion"> Modelo de Notificación</param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta, Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public JsonResult crearTicket(Notificacion notificacion)
        {
            String mensajeAdvertencia = "";
            JsonResult resultado;

            try{

                //Se verifica si la alerta tiene un ticket asociado
                TICKET ticket = monitorRepo.obten_TICKET_ByOrigen(notificacion.Id
                                                                , notificacion.IdOrigenNotificacion);
               
                if (ticket != null){
                    mensajeAdvertencia = String.Format(IntranetWeb.Core.Constante.Mensaje.Advertencia.TicketExiste, ticket.ID_TICKET);
                    throw new BussinessException(mensajeAdvertencia);
                }

                //Si no posee asociado número de ticket, se crea
                var notificacionAlerta = monitorRepo.obtenNotificacion_ById(notificacion.Id
                                                                          , notificacion.IdOrigenNotificacion);


                if (notificacionAlerta == null) {
                    mensajeAdvertencia = String.Format(IntranetWeb.Core.Constante.Mensaje.Advertencia.NotificacionNoDisponible);
                    throw new BussinessException(mensajeAdvertencia);
                }

                int codigo_usuario_logueado = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();
                var fechaActual = DateTime.Now;

                ticket = new TICKET();
                ticket.ID_DISPOSITIVO = notificacionAlerta.ID_DISPOSITIVO;
                ticket.ID_DISTRIBUIDOR = notificacionAlerta.ID_DISTRIBUIDOR;
                ticket.ID_ESTATUS_TICKET = 1; //Abierto
                ticket.CD_USUARIO = codigo_usuario_logueado;
                ticket.FE_CREACION = fechaActual;
                ticket.FE_ESTATUS = fechaActual;
                ticket.ID_NOTIFICACION = notificacionAlerta.ID_NOTIFICACION;
                ticket.ID_ORIGEN_NOTIFICACION = notificacionAlerta.ID_ORIGEN_NOTIFICACION;
                ticket.CD_CLIENTE = notificacionAlerta.CD_CLIENTE;
                ticket.TP_NOTIFICACION = notificacionAlerta.TP_NOTIFICACION;
                ticket.FE_NOTIFICACION = notificacionAlerta.FE_NOTIFICACION;
                monitorRepo.guarda_TICKET(ticket);


                resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.TicketAbierto, ticket.ID_TICKET), new { ID_TICKET = ticket.ID_TICKET, mensajeAdvertencia = mensajeAdvertencia });
                
            }
            catch(BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message, new { mensajeAdvertencia = mensajeAdvertencia });
                
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }

        /// <summary>
        /// Se emplea para el envío de mensajes cms
        /// </summary>
        /// <param name="id">Id del ticket</param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta, Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public JsonResult enviarMensajeGCM(int id)
        {
            JsonResult resultado;
           
            bool exito = false;


            try {
                //MantenimientoInteligente_Request wsMantenimientoInteligente = new MantenimientoInteligente_Request();

                var mensaje = monitorRepo.obtenMensajeGCM_byTicketId(id);

                foreach (string GCM_Id in mensaje.GCM_ID) {
                    if (Core.WebService.GCM.GCMClient.Send(GCM_Id
                                                           , mensaje.Titulo
                                                           , mensaje.Mensaje))
                        exito = true;
                }

                
                if (exito) {
                    TICKET ticket = monitorRepo.obten_TICKET_ById(id);
                    DateTime feActual = DateTime.Now;
                    ticket.FE_ESTATUS = feActual;
                    ticket.IN_RESPUESTA_ENVIADA = exito;
                    monitorRepo.actualiza_TICKET(ticket); 
                } else
                    throw new BussinessException(Core.Constante.Mensaje.Error.FalloEnvioCMS);

                resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.CMSEnviado));

            } catch (BussinessException exc){
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc){
                              
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return resultado;
        }

        /// <summary>
        /// Método para crear un ticket por usuario
        /// </summary>
        /// <param name="nuevoTicket"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta, Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public JsonResult CrearTicketPorUsuario(NuevoTicket nuevoTicket) {
            JsonResult resultado;
            try {
                string mensajeAdvertencia = "";
                int codigo_usuario_logueado = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();
                var fechaActual = DateTime.Now;
                
                if (nuevoTicket.FechaNotificacion > DateTime.Now)
                    throw new BussinessException(Core.Constante.Mensaje.Error.FechaNotificacionInvalida);

                TICKET ticket = ticket = new TICKET();
                ticket.ID_DISPOSITIVO = nuevoTicket.DispositivoSeleccionado;

                ClienteRepositorio clienRepo = new ClienteRepositorio();
                DISPOSITIVO dev = clienRepo.obten_DISPOSITIVO_byId(nuevoTicket.DispositivoSeleccionado);

                ticket.ID_DISTRIBUIDOR = dev.ID_DISTRIBUIDOR;

                ticket.ID_ESTATUS_TICKET = 1; //Abierto
                ticket.CD_USUARIO = codigo_usuario_logueado;
                ticket.FE_CREACION = fechaActual;
                ticket.FE_ESTATUS = fechaActual;
                ticket.ID_NOTIFICACION = 0;
                ticket.ID_ORIGEN_NOTIFICACION = nuevoTicket.OrigenNotificacionSeleccionado;
                if(nuevoTicket.ClienteSeleccionado!= null)
                    ticket.CD_CLIENTE = nuevoTicket.ClienteSeleccionado;

                ticket.TP_NOTIFICACION = 102;//Contact Center
                ticket.FE_NOTIFICACION = nuevoTicket.FechaNotificacion;
                monitorRepo.guarda_TICKET(ticket);

                resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.TicketAbierto, ticket.ID_TICKET), new { ID_TICKET = ticket.ID_TICKET, mensajeAdvertencia = mensajeAdvertencia });
        
            }            
            catch (BussinessException exc){
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }


        /// <summary>
        /// Permite eliminar una notificación de alerta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta
            , Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public JsonResult eliminarNotificacion(Notificacion notificacion)
        {

            JsonResult result;
            string mensajeAdvertencia = "";
            try
            {
                ValidacionRepositorio validacioRepositorio = new ValidacionRepositorio();
                //Se verifica si la alerta ha sido gestionada por otro usuario

                if (validacioRepositorio.validaNotificacionTomada(notificacion.Id
                                                                 , notificacion.IdOrigenNotificacion))
                {
                    mensajeAdvertencia = Resources.AdvertenciaResource.NotificacionGestionada;
                    throw new BussinessException(mensajeAdvertencia);
                }
                int codigo_usuario_logueado = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();
                var fechaActual = DateTime.Now;

                NOTIFICACION_SIN_GESTION notifSG = new NOTIFICACION_SIN_GESTION();

                var notificacionDB = monitorRepo.obtenNotificacion_ById(    notificacion.Id
                                                                        ,   notificacion.IdOrigenNotificacion);

                if (notificacionDB == null) {                    
                        mensajeAdvertencia = Resources.AdvertenciaResource.NotificacionGestionada;
                        throw new BussinessException(mensajeAdvertencia);
                }
                

                notifSG.CD_USUARIO=codigo_usuario_logueado;
                notifSG.FE_CREACION = fechaActual;
                notifSG.FE_NOTIFICACION = notificacionDB.FE_NOTIFICACION;
                notifSG.ID_NOTIFICACION = notificacionDB.ID_NOTIFICACION;
                notifSG.ID_ORIGEN_NOTIFICACION = notificacionDB.ID_ORIGEN_NOTIFICACION;
                notifSG.TP_NOTIFICACION = notificacionDB.TP_NOTIFICACION;
                notifSG.ID_DISTRIBUIDOR = notificacionDB.ID_DISTRIBUIDOR;
                notifSG.ID_DISPOSITIVO = notificacionDB.ID_DISPOSITIVO;
                notifSG.CD_CLIENTE = notificacionDB.CD_CLIENTE;
                

                
                if (monitorRepo.guarda_NOTIFICACION_SIN_GESTION(notifSG) > 0)
                {
                    result = Core.Utils.UtilJson.Exito(Resources.ExitoResource.NotificacionEliminada, new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoEliminado);
                    
            }
            catch (BussinessException exc)
            {
                result = Core.Utils.UtilJson.Error(exc.Message, new { mensajeAdvertencia = mensajeAdvertencia });

            }
            catch (Exception exc){

                String mensaje = exc.InnerException != null && exc.InnerException.InnerException != null ? exc.InnerException.InnerException.Message : exc.Message;

                if (!String.IsNullOrWhiteSpace(mensaje)&&mensaje.Contains("Cannot insert duplicate key in object 'MONITOR.NOTIFICACION_SIN_GESTION'")) {
                    mensajeAdvertencia = Resources.AdvertenciaResource.NotificacionGestionada;
                    result = Core.Utils.UtilJson.Error(exc.Message, new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                {
                    log.Error(Resources.ErrorResource.Error100, exc);
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                }
            }

            return result;
        }


        /// <summary>
        /// Coloca un ticket como atendido
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [NonAction]
        public JsonResult atenderTicket(Ticket ticket)
        {
            JsonResult resultado;
            bool inEnvioCMS = false;
            if (ticket.TipoAtencionSeleccionada == null)
                throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.SeleccioneTipoAtencion);

            TICKET tick_Modif = monitorRepo.obten_TICKET_ById(ticket.Id);
            tick_Modif.TP_ATENCION = ticket.TipoAtencionSeleccionada;
            tick_Modif.ID_ESTATUS_TICKET = 2; //Atendido
            tick_Modif.FE_ESTATUS = DateTime.Now;
            tick_Modif.ID_RESPUESTA_OPERADOR = ticket.MensajeAlClienteSeleccionado;
            tick_Modif.DE_OBSERVACION = ticket.Observacion;
            inEnvioCMS = ticket.MensajeAlClienteSeleccionado != null ? true : inEnvioCMS;

            if (monitorRepo.actualiza_TICKET(tick_Modif) > 0)
                resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.TicketActualizado, tick_Modif.ID_TICKET),new { IdTicket = tick_Modif.ID_TICKET,  InEnvioCMS = inEnvioCMS });
            else
                throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.FalloActualizarTicket);

            return resultado;
        }

        /// <summary>
        /// Retorna el detalle del cliente
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.VerMonitorAlerta
           , Core.Constante.Aplicacion.VerMonitorAlertaAdmin)]
        public JsonResult obtenDetalleClienteNotificacion(int Id){
            JsonResult result;

            try {
                var data = monitorRepo.obtenUsuarioMovil_ByNotificacionId(Id); 
                result= Core.Utils.UtilJson.Exito(data);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }


        /// <summary>
        /// Método para transferir un ticket a otro agente
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [NonAction]
        public JsonResult transferirTicket(Ticket ticket)
        {
            JsonResult resultado;
            bool inEnvioCMS = false;
            if (ticket.UsuarioTransferenciaSeleccionado == null)
                throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.SeleccioneUsuarioTranfTicket);

            TICKET tick_Modif = monitorRepo.obten_TICKET_ById(ticket.Id);
            tick_Modif.TP_ATENCION = ticket.TipoAtencionSeleccionada;
            tick_Modif.ID_ESTATUS_TICKET = 3; //Tranferido
            tick_Modif.CD_USUARIO_TRANSFERENCIA = Core.Utils.UtilHelper.obtenUsuarioLogueado();
            tick_Modif.CD_USUARIO = (int)ticket.UsuarioTransferenciaSeleccionado;

            tick_Modif.FE_ESTATUS = DateTime.Now;
            tick_Modif.DE_OBSERVACION = ticket.Observacion;

            if (monitorRepo.actualiza_TICKET(tick_Modif) > 0)
                resultado  = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.TicketActualizado, tick_Modif.ID_TICKET), new { IdTicket = tick_Modif.ID_TICKET, InEnvioCMS = inEnvioCMS });
            else
                throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.FalloActualizarTicket);

             return resultado;
        }


        /// <summary>
        /// Método par dar por descartado un ticket
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [NonAction]
        public JsonResult descartarTicket(Ticket ticket)
        {
            JsonResult resultado;
            bool inEnvioCMS = false;

            TICKET tick_Modif = monitorRepo.obten_TICKET_ById(ticket.Id);
            tick_Modif.TP_ATENCION = ticket.TipoAtencionSeleccionada;
            tick_Modif.ID_ESTATUS_TICKET = 4; //Cerrado
            tick_Modif.FE_ESTATUS = DateTime.Now;
            tick_Modif.DE_OBSERVACION = ticket.Observacion;

            if (monitorRepo.actualiza_TICKET(tick_Modif) > 0)
                resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.TicketActualizado, tick_Modif.ID_TICKET), new { IdTicket = tick_Modif.ID_TICKET, InEnvioCMS = inEnvioCMS });
            else
                throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.FalloActualizarTicket);

            return resultado;
        }


        [NonAction]
        private void cargaSelectTicket(Ticket ticket){
            ticket = ticket == null ? new Ticket() : ticket;

            ticket.TipoAtencion = UtilRepositorio.obtenSelect_TipoAtencion();
            ticket.TipoAtencionSeleccionada = ticket.TipoAtencion.Count() == 1 ? Convert.ToInt32(ticket.TipoAtencion.FirstOrDefault().Value) : ticket.TipoAtencionSeleccionada;
            ticket.MensajeAlCliente = UtilRepositorio.obtenSelect_RespuestaOperadorGCM(ticket.TipoAtencionSeleccionada);
            ticket.UsuarioTransferencia = UtilRepositorio.obtenSelect_UsuarioTransferenciaTicket(Core.Utils.UtilHelper.obtenUsuarioLogueado());
            ticket.UsuarioTransferenciaSeleccionado = ticket.UsuarioTransferencia.Count() == 1 ? Convert.ToInt32(ticket.UsuarioTransferencia.FirstOrDefault().Value) : ticket.UsuarioTransferenciaSeleccionado;
            ticket.UsuarioTicket = UtilRepositorio.obtenSelect_UsuarioTicket();
            ticket.UsuarioTicketSeleccionado = ticket.UsuarioTicket.Count() == 1 ? Convert.ToInt32(ticket.UsuarioTicket.FirstOrDefault().Value) : ticket.UsuarioTicketSeleccionado;
            ticket.Estatus = UtilRepositorio.obtenSelect_TICKET_ESTATUS();
        }
        
        [NonAction]
        private void cargaSelectNuevoTicket(NuevoTicket ticket)
        {
            ticket = ticket == null ? new NuevoTicket() : ticket;
            
            ticket.Cliente = UtilRepositorio.obtenSelect_Cliente();
            ticket.ClienteSeleccionado = ticket.Cliente.Count() == 1 ? Convert.ToInt32(ticket.Cliente.FirstOrDefault().Value) : ticket.ClienteSeleccionado;
            
            ticket.Dispositivo = UtilRepositorio.obtenSelect_DispositivosPorCliente(ticket.ClienteSeleccionado);
            ticket.DispositivoSeleccionado = ticket.Dispositivo.Count() == 1 ? Convert.ToInt32(ticket.Dispositivo.FirstOrDefault().Value) : ticket.DispositivoSeleccionado;
            
            ticket.OrigenNotificacion = UtilRepositorio.obtenSelect_OrigenNotificacion();
            ticket.OrigenNotificacionSeleccionado = ticket.OrigenNotificacion.Count() == 1 ? Convert.ToInt32(ticket.OrigenNotificacion.FirstOrDefault().Value) : ticket.OrigenNotificacionSeleccionado;
            
        }

        #endregion


        #region Localizacion
        /// <summary>
        /// Habilita la aplicación de localizacion
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.LocalizarVehiculo)]
        public ActionResult Localizacion(){

            return View();
        }


        #endregion

    }
}