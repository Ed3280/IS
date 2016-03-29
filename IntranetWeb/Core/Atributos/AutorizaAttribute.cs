using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IntranetWeb.Core.Atributos
{
    public class AutorizaAttribute : ActionFilterAttribute, IAuthorizationFilter
    {

        public String[] Apps { get; set; }
        public bool IndicadorMotrarMensaje = true;


        public AutorizaAttribute(bool indicadorMostrarMensaje, params String[] apps)
        {
            Apps = apps;
            IndicadorMotrarMensaje = indicadorMostrarMensaje;
        }


        public AutorizaAttribute(params String[] apps){
            Apps = apps;            
        }

        /// <summary>
        /// Permite acceso a una acción
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            AuthRepositorio repo = new AuthRepositorio();

            var identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;
            if (identity.IsAuthenticated){

                int id = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();
                var aplicaciones = repo.obtenListadoAplicaciones_ById(id);


                Boolean tienePermiso = Apps.Any(z => aplicaciones.Any(y => y == z));

                    if (!tienePermiso)
                    {
                    if (IndicadorMotrarMensaje)
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "NoAutorizado" }));
                    else
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PaginaEnBlanco" }));
                    }
                }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Auth", action = "LogIn" }));
            }
        }
    }        
}