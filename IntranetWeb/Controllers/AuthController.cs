using System;
using System.Web;
using System.Web.Mvc;
using IntranetWeb.ViewModel.Auth;
using System.Security.Claims;
using IntranetWeb.Core.Seguridad;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Models;
using IntranetWeb.Core.Utils;
using System.Linq;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Exception;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class AuthController : Controller
    {

        private Log4NetLogger log;
        public AuthController() {
            log = new Log4NetLogger(0);
        }

        private AuthRepositorio authRepo = new AuthRepositorio();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogIn(String returnUrl)
        {
            var model = new LoginUsuario { returnURL = returnUrl };    
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginUsuario model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //Se busca el usuario

                USUARIO usuario = authRepo.obten_USUARIO_ByNombreUsuarioCorreo(model.UserName
                                                                        , model.Password);
            
                    //Se busca el usuario y se compara con el que está registrado en la base de datos
                    if (usuario == null)
                        throw new BussinessException(Resources.ErrorResource.UsuarioInvalidoInactivo);

                AdministradorRepositorio adminRepo = new AdministradorRepositorio();

                EMPLEADO empleado = adminRepo.obten_EMPLEADO_ById(usuario.CD_USUARIO);

                log.setUser(usuario.CD_USUARIO);
                    
                var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, usuario.DE_NOMBRE_APELLIDO)
                            ,new Claim(ClaimTypes.Email, usuario.DI_EMAIL_USUARIO)
                            ,new Claim(UserClaimType.Id, usuario.CD_USUARIO.ToString())
                            ,new Claim(UserClaimType.UserName, usuario.NM_USUARIO)
                            ,new Claim(UserClaimType.IdEmpleado,empleado!=null?empleado.CD_USUARIO.ToString():"0")
                        }, "ApplicationCookie");

                    //Se agregan los roles
                    foreach (var rol in usuario.ROL)
                        identity.AddClaim(new Claim(UserClaimType.Roles, rol.CD_ROL.ToString()));


                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignIn(identity);
                    log.Info(Core.Constante.Mensaje.Logging.IngresoUsuario);
                return Redirect(GetRedirectUrl(usuario.CD_USUARIO, model.returnURL));

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

            return View(model);
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarContrasena(LoginUsuario usuario) {
            String envidado = "";
            //Se busca el usuario

            USUARIO usuarioEnti = authRepo.obten_USUARIO_ByUserNameEmail(usuario.UserNameRecuperacion);

            try{
                    if (usuarioEnti == null)
                        throw new BussinessException(Resources.ErrorResource.NombreUsuarioInvalido);

                    string password = authRepo.obtenPassword(usuarioEnti.DE_CONTRASENA);

                    if (!SmtpHelper.Send(IntranetWeb.Core.Constante.Mensaje.AsuntoCorreo.RecuperarContrasena, MailTemplate.PrepararTemplate(String.Format(Resources.EncabezadoCorreoResource.Hola, usuarioEnti.DE_NOMBRE_APELLIDO), String.Format(Resources.MensajeCorreoResource.RecuperarContrasena, password)), usuarioEnti.DI_EMAIL_USUARIO))
                        throw new BussinessException(Resources.ErrorResource.FalloEnvioContrasena);    

                     envidado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeExito(Resources.ExitoResource.ContrasenaEnviada);                                    
                
            }
            catch (BussinessException exc) {
                envidado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(exc.Message);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                envidado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(Resources.ErrorResource.Error100);
            }

            return Content(envidado, "text/thtml");
        }

        
      /// <summary>
      /// Permite hacer logout de la aplicación
      /// </summary>
      /// <returns></returns>
        public ActionResult LogOut(){

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            return Redirect("~/Auth/LogIn");

        }

        /// <summary>
        /// Escribe el menu lateral
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public ActionResult _getMenuLateral(Menu menu)
        {

            menu = menu == null ? new Menu() : menu;
            try{
                int codigo_usuario_logueado = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();

                menu.ArbolMenu = authRepo.obtenAplicacionesMenu(codigo_usuario_logueado);
                menu.AtajoMenu = authRepo.obtenAplicacionesAtajo(codigo_usuario_logueado);
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return View(menu);
        }
        

        /// <summary>
        /// Redirección de la url luego del logi de usuario
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private string GetRedirectUrl(int cdUsuario,string returnUrl)
        {
            try {
                if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                {
                    //Se busca el primer dashboard asociado al usuario 

                    var aplicacionDashBoard = authRepo.obtenDashBoardUsuario(cdUsuario);


                    if (aplicacionDashBoard != null)
                        return Url.Action(aplicacionDashBoard.Accion, aplicacionDashBoard.Controlador);
                    else
                        return Url.Action("NoDashBoard", "Home");
                }
            }
            catch (Exception exc ) {
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return returnUrl;
        }
    }
}