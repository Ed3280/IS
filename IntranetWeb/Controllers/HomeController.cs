using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Servicio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class HomeController : Controller
    {
        private Log4NetLogger log;

        public HomeController() {
         log = new Log4NetLogger();   
         }

        [Autoriza(Core.Constante.Aplicacion.DashBoardGeneral)]
        public ActionResult Index()
        {
           
            return View();
        }

        
        [AllowAnonymous]
        public String NoAutorizado(){
            return IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(IntranetWeb.Core.Constante.Mensaje.Error.UsuarioNoAutorizado); 
        }

        [AllowAnonymous]
        public String PaginaEnBlanco()
        {
            return "";
        }

        public String NoDashBoard()
        {
            return IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(IntranetWeb.Core.Constante.Mensaje.Error.UsuarioSinDashBoard);
        }

    }
}