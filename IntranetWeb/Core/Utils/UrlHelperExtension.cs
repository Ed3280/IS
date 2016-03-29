using IntranetWeb.Core.Respositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.Core.Utils
{
    public static class UrlHelperExtension
    {

        /// <summary>
        /// Retorna el dashboard que le corresponde al usuario
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string obtenDashboard(this UrlHelper url) {
            AuthRepositorio repo = new AuthRepositorio();
            var aplicacionDashBoard = repo.obtenDashBoardUsuario(UtilHelper.obtenUsuarioLogueado());
            return url.Action(aplicacionDashBoard.Accion, aplicacionDashBoard.Controlador);
        }
    }
}