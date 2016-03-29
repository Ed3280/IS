using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.ViewModel.Servicio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class ServicioController : Controller
    {
        private Log4NetLogger log;
        private ServicioRepositorio servicioRepo = new ServicioRepositorio();


        public ServicioController()
        {
            log = new Log4NetLogger();
            servicioRepo = new ServicioRepositorio();
        }


        /// <summary>
        /// Habilita la aplicación de mantenimiento inteligente
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.MantenimientoInteligente)]
        public ActionResult MantenimientoInteligente()
        {

            return View();
        }

        /// <summary>
        /// Lista las solicitudes de mantenimiento
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.MantenimientoInteligente)]
        public ActionResult consultarSolicitudesMantenimiento() {

            JsonResult result;

            try
            {

                var myData = servicioRepo.obtenSolicitudesMantenimiento();
                return Content(JsonConvert.SerializeObject(myData), "application/json");
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);

            }
            return result;

        }



        /// <summary>
        /// Consulta el detalle del servicio
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.MantenimientoInteligente)]
        public ActionResult _getConsultarServicio(int Id) {

            SolicitudMantenimiento solicitudMantenimiento = new SolicitudMantenimiento();
            JsonResult result;

            try{

                solicitudMantenimiento = servicioRepo.obtenSolicitudesMantenimiento_ById(Id);
                return View("Consultar/_getConsultarServicio", solicitudMantenimiento);

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);

            }
            return result;
        }
    }
}