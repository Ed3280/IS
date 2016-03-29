using IntranetWeb.Core.Servicio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.ViewModel.Dashboard;
using IntranetWeb.ViewModel.Dashboard.Cumpleano;
using IntranetWeb.Core.Respositorios;
using System.Web.Mvc;
using IntranetWeb.Core.Atributos;
using IntranetWeb.ViewModel.Dashboard.Directorio;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class DashboardController : Controller
    {
        private Log4NetLogger log;
        private DashboardRepositorio dashboarRepo;

        public DashboardController() {
            log             = new Log4NetLogger();
            dashboarRepo = new DashboardRepositorio();
        }

        // GET: Dashboard
        public ActionResult Index(){
            int usuarioId = Core.Utils.UtilHelper.obtenUsuarioLogueado();
            UsuarioDashboard usuario = new UsuarioDashboard();
            usuario.EsEmpleado = Core.Utils.UtilHelper.esEmpleado();
            usuario.aplicaciones = dashboarRepo.obtenAplicacionesActivas_byUsuarioId(usuarioId);
            return View(usuario);
        }


        /// <summary>
        /// Retorna a los cumpleaños de los empleados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.DashBoardGeneral)]
        public ActionResult _getCumpleano() {
            ViewModel.Dashboard.Cumpleano.Cumpleano cumple = new ViewModel.Dashboard.Cumpleano.Cumpleano();
            try{
                cumple.Empleado = dashboarRepo.obtenCumpleanoEmpleado();
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                cumple.Empleado = new List<EmpleadoCumpleano>();
            }
            return View("Widget/_getCumpleano", cumple);
        }



        /// <summary>
        /// Retorna estadística de gestiónn de tickets por usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.DashBoardGeneral)]
        public ActionResult _getTicketsPorUsuario() {

            try{
                //cumple.Empleado = dashboarRepo.obtenCumpleanoEmpleado();
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                //cumple.Empleado = new List<EmpleadoCumpleano>();
            }
            return View("Widget/_getTicketsPorUsuario"/*, cumple*/);

        }



        /// <summary>
        /// Permite realizar la búsqueda de los empleados
        /// </summary>
        /// <param name="campoBusqueda"></param>
        /// <returns></returns>

        [Autoriza(Core.Constante.Aplicacion.DashBoardGeneral)]
        public ActionResult consultarDirectorio(string campoBusqueda) {
            JsonResult result;
            try
            {
                IEnumerable<Empleado> listadoEmpleados = dashboarRepo.obtenEmpleados(campoBusqueda);
                return PartialView("Widget/_getDirectorioListadoEmpleado", listadoEmpleados);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }


        /// <summary>
        /// Se obtiene la data de los ticket
        /// </summary>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.DashBoardGeneral)]
        public JsonResult consultarGestionTicket(int frecuencia) {
            JsonResult resultado;
            try{

                //Se obtiene el usuario actual
                int usuario = Core.Utils.UtilHelper.obtenUsuarioLogueado();
                resultado = Core.Utils.UtilJson.Exito("",dashboarRepo.obtenGestionTicketUsuario(usuario,frecuencia));

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }
    }
}