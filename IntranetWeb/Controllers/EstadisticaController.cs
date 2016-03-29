using System;
using System.Linq;
using System.Web.Mvc;
using IntranetWeb.ViewModel.Estadistica;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Exception;
using IntranetWeb.Core.Servicio.Logging;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class EstadisticaController : Controller
    {

        private EstadisticaRepositorio estadisticaRepo;
        private Log4NetLogger log;

        public EstadisticaController() {
            estadisticaRepo = new EstadisticaRepositorio();
            log = new Log4NetLogger();
        }

        /// <summary>
        /// Permite generar el reporte de tickets
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.GenerarReporteTickets)]
        // GET: Estadistica
        public ActionResult ReporteTickets()
        {
            GestionTicket gestionTicket = new GestionTicket();

            try {
                cargaSelectReporteTicket(gestionTicket);

            } catch(Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc); 
            }

            return View(gestionTicket);
        }


        /// <summary>
        /// Permite generar el reporte de alertas
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.GenerarReporteAlertas)]
        // GET: Estadistica
        public ActionResult ReporteAlertas()
        {
            GestionAlerta gestionAlerta = new GestionAlerta();

            try{
                cargaSelectReporteAlerta(gestionAlerta);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
            }

            return View(gestionAlerta);
        }


         


        /// <summary>
        /// Genera excel de reporte de tickets
        /// </summary>
        /// <param name="gestionTicket"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.GenerarReporteTickets)]
        public ActionResult GererarExcelTicket(GestionTicket gestionTicket) {
            try
            {
                cargaSelectReporteTicket(gestionTicket);
                if (!ModelState.IsValid)
                    return View("ReporteTickets", gestionTicket);

                byte[] salida = estadisticaRepo.generarExcelListaTickets(gestionTicket.UsuarioTicketSeleccionado
                                             , gestionTicket.TipoNotificacionSeleccionada
                                             , gestionTicket.TipoAtencionSeleccionada
                                             , gestionTicket.EstatusSeleccionado
                                             , gestionTicket.FechaStatusDesde
                                             , gestionTicket.FechaStatusHasta
                                             , gestionTicket.FechaAperturaDesde
                                             , gestionTicket.FechaAperturaHasta);

                if (salida == null)
                    throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.FalloGenerarArchivoExcel);

                String fileName = DateTime.Now.ToString("yyyyMMdd") + "_ReporteTickets.xlsx";

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Charset = "";

                Response.BinaryWrite(salida);

                Response.Flush();
                Response.End();
            }
            catch (BussinessException exc)
            {
                ModelState.AddModelError("", exc.Message);
            }
            catch (Exception exc) {
                ModelState.AddModelError("", Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }


            return View("ReporteTickets", gestionTicket);
        }




        /// <summary>
        /// Genera excel de reporte de alertas
        /// </summary>
        /// <param name="gestionAlerta"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.GenerarReporteAlertas)]
        public ActionResult GererarExcelAlerta(GestionAlerta gestionAlerta)
        {
            try
            {
                cargaSelectReporteAlerta(gestionAlerta);
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                byte[] salida =  estadisticaRepo.generarExcelListaAlertas(gestionAlerta.UsuarioAlertaSeleccionado
                                             , gestionAlerta.TipoNotificacionSeleccionada
                                             , gestionAlerta.FechaNotificacionDesde
                                             , gestionAlerta.FechaNotificacionHasta
                                             , gestionAlerta.FechaRegistroDesde
                                             , gestionAlerta.FechaRegistroHasta);
                                            
                if (salida == null)
                    return View("ReporteAlertas", gestionAlerta);

                String fileName = DateTime.Now.ToString("yyyyMMdd") + "_ReporteAlertas.xlsx";

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Charset = "";

                Response.BinaryWrite(salida);

                Response.Flush();
                Response.End();
            }
            catch (BussinessException exc)
            {
                ModelState.AddModelError("", exc.Message);
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }


            return View("ReporteAlertas", gestionAlerta);
        }






        /// <summary>
        /// Carga los selects para la gestión de tickets
        /// </summary>
        /// <param name="gestionTicket">Modelo Gestión Ticket</param>
        [NonAction]
        public void cargaSelectReporteTicket(GestionTicket gestionTicket ) {
            
            gestionTicket = gestionTicket == null ? new GestionTicket() : gestionTicket;

            gestionTicket.UsuarioTicket = UtilRepositorio.obtenSelect_UsuarioTicket();
            gestionTicket.UsuarioTicketSeleccionado = gestionTicket.UsuarioTicket.Count() == 1 ? Convert.ToInt32(gestionTicket.UsuarioTicket.FirstOrDefault().Value) : gestionTicket.UsuarioTicketSeleccionado; 
            gestionTicket.TipoAtencion = UtilRepositorio.obtenSelect_TipoAtencion();
            gestionTicket.TipoAtencionSeleccionada = gestionTicket.TipoAtencion.Count() == 1 ? Convert.ToInt32(gestionTicket.TipoAtencion.FirstOrDefault().Value) : gestionTicket.TipoAtencionSeleccionada;
            gestionTicket.TipoNotificacion = UtilRepositorio.obtenSelect_TipoNotificacion();
            gestionTicket.TipoNotificacionSeleccionada = gestionTicket.TipoNotificacion.Count() == 1 ? Convert.ToInt32(gestionTicket.TipoNotificacion.FirstOrDefault().Value) : gestionTicket.TipoNotificacionSeleccionada;
            gestionTicket.Estatus = UtilRepositorio.obtenSelect_TICKET_ESTATUS();
            gestionTicket.EstatusSeleccionado = gestionTicket.Estatus.Count() == 1 ? Convert.ToInt32(gestionTicket.Estatus.FirstOrDefault().Value) : gestionTicket.EstatusSeleccionado;

        }


        /// <summary>
        /// Carga los selects para la gestión de alertas
        /// </summary>
        /// <param name="gestionAlerta">Modelo Gestión de alertas</param>
        [NonAction]
        public void cargaSelectReporteAlerta(GestionAlerta gestionAlerta){

            gestionAlerta = gestionAlerta == null ? new GestionAlerta() : gestionAlerta;

            gestionAlerta.UsuarioAlerta = UtilRepositorio.obtenSelect_UsuarioTicket();
            gestionAlerta.TipoNotificacion = UtilRepositorio.obtenSelect_TipoNotificacion();
            gestionAlerta.TipoNotificacionSeleccionada = gestionAlerta.TipoNotificacion.Count() == 1 ? Convert.ToInt32(gestionAlerta.TipoNotificacion.FirstOrDefault().Value) : gestionAlerta.TipoNotificacionSeleccionada;
            
        }


    }
}