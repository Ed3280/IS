using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Seguridad;
using IntranetWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace IntranetWeb.Core.Utils
{
    public static class UtilHelper
    {

        

        /// <summary>
        /// Obtiene el usuario logueado
        /// </summary>
        /// <returns></returns>
        public static int obtenUsuarioLogueado(){
            try
            {
                int cdUsuario = 0;
                var user = HttpContext.Current!=null?HttpContext.Current.User:null;

                if (user != null){
                    var identity = (ClaimsIdentity)user.Identity;
                    var claim = identity.Claims.Where(x => x.Type == UserClaimType.Id).FirstOrDefault();
                        cdUsuario = claim != null ? Convert.ToInt32(claim.Value) : cdUsuario;
                }
                
                return cdUsuario;
            }
            catch (System.NullReferenceException exc) {
                throw new Core.Exception.BussinessException(Core.Constante.Mensaje.Error.Error200, exc);
            }
        }


        /// <summary>
        /// Obtiene los roles del usuario
        /// </summary>
        /// <returns></returns>
        public static IList<int> obtenRolesUsuarioLogueado()
        {
            try
            {
                IList<int> roles = new  List<int>();
                var user = HttpContext.Current != null ? HttpContext.Current.User : null;

                if (user != null){
                    var identity = (ClaimsIdentity)user.Identity;
                    var claim = identity.Claims.Where(x => x.Type == UserClaimType.Roles);

                    claim.ToList().ForEach(x => roles.Add(Convert.ToInt32(x.Value)));
                    
                }

                return roles;
            }
            catch (System.NullReferenceException exc)
            {
                throw new Core.Exception.BussinessException(Core.Constante.Mensaje.Error.Error200, exc);
            }
        }


        /// <summary>
        /// Permite identificar si el usuario conectado es empleado
        /// </summary>
        /// <returns></returns>
        public static bool esEmpleado() {
                try
                {
                    int IdEmpleado = 0; DateTime? feRetiro = null;
                    var user = HttpContext.Current != null ? HttpContext.Current.User : null;
                    AdministradorRepositorio adminRepo = new AdministradorRepositorio();
                    if (user != null)
                    {
                        var identity = (ClaimsIdentity)user.Identity;
                        var claim = identity.Claims.Where(x => x.Type == UserClaimType.IdEmpleado).FirstOrDefault();
                        IdEmpleado = claim != null ? Convert.ToInt32(claim.Value) : IdEmpleado;
                        if (IdEmpleado != 0) {
                            EMPLEADO emp = adminRepo.obten_EMPLEADO_ById(IdEmpleado);
                            feRetiro = emp != null ? emp.FE_RETIRO:null;
                        }
                    }

                    return IdEmpleado!=0&& feRetiro ==null? true :false;
                }
                catch (System.NullReferenceException exc)
                {
                    throw new Core.Exception.BussinessException(Core.Constante.Mensaje.Error.Error200, exc);
                }

            }

        /// <summary>
        /// Obtiene el FK que genra la excepción
        /// </summary>
        /// <param name="exc"></param>
        /// <returns></returns>
        public static string obtenFKRelacion(DbUpdateException exc) {
            string result = "";

            try {
                SqlException s = exc.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 547){
                    result  = s.Message.Substring(s.Message.IndexOf("\"", 0)+1, (s.Message.IndexOf("\"", s.Message.IndexOf("\"", 0)+1) - s.Message.IndexOf("\"", 0))-1);                    
                }
            } catch { }
            return result;
        }

        /// <summary>
        /// Cambia el Key de un diccionario
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="fromKey"></param>
        /// <param name="toKey"></param>
        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
                                    TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }


        /// <summary>
        /// Genera una contraseña aleatoria
        /// </summary>
        /// <param name="passwordLength"></param>
        /// <returns></returns>
        public static string CreateRandomPassword(int passwordLength)
        {
            if (passwordLength < 6)
                throw new System.Exception(Resources.ErrorResource.ContrasenaInvalida);

            string allowedChars = "abcdefghijkmnpqrstuvwxyz123456789@#$%*";

            char[] chars = new char[passwordLength-3]; Random rd = new Random();


            for (int i = 0; i < passwordLength-3; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            string contrasena = new string(chars);
            contrasena = "m" + contrasena + "5*";

            return contrasena;
        }

        public static T CreateController<T>(System.Web.HttpContext context = null, RouteData routeData = null)
         where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper;

            if (context != null)
                wrapper = new HttpContextWrapper(context); 
            else
            {
                if (System.Web.HttpContext.Current != null)
                    wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
                else
                    throw new InvalidOperationException(
                        Resources.ErrorResource.ControllerContextNoCreable
                        );
            }

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") &&
                !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller",
                                     controller.GetType()
                                               .Name.ToLower().Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }


        /// <summary>
        /// Remueve recursivamente el listado de aplicaciones 
        /// </summary>
        /// <param name="aplicaciones"></param>
        /// <param name="aplicacionAremover"></param>
        /// <returns></returns>
        public static void remueveAplicacionDeListado(ref IList<APLICACION> aplicaciones, int cdAplicacionARemover) {

            var aplicacionAremover = aplicaciones.Where(x => x.CD_APLICACION == cdAplicacionARemover).FirstOrDefault();
            aplicaciones.Remove(aplicacionAremover);
            if ((from x in aplicaciones
                 where x.CD_APLICACION_PADRE == aplicacionAremover.CD_APLICACION_PADRE
                 select x
                ).Count() == 0)
            {              
                if(aplicacionAremover.APLICACION2!=null)  
                remueveAplicacionDeListado(ref aplicaciones, aplicacionAremover.APLICACION2.CD_APLICACION);
            }    
        }
    }
}