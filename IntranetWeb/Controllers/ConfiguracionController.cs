using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Seguridad;
using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Configuracion;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using IntranetWeb.Core.Exception;
using IntranetWeb.Core.Servicio.Logging;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class ConfiguracionController : Controller
    {
        private Log4NetLogger log;
        public ConfiguracionController() {
              log = new Log4NetLogger();
        }

        private ConfiguracionRepositorio configuracionRepo = new ConfiguracionRepositorio();
        private AuthRepositorio authRepo = new AuthRepositorio();

        // GET: Configuracion
        [Autoriza(Core.Constante.Aplicacion.ConfiguracionUsuario)]
        public ActionResult Perfil()
        {
            
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult _getEditarPerfil() {

            Perfil perf = new Perfil();

            try{
                //Se busca el usuario logueado
                int codigo_usuario_logueado = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();
                perf = configuracionRepo.obten_Perfil_ById(codigo_usuario_logueado);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);               
            }
            return View("Editar/_getEditarPerfil",perf);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult _getCambioContrasena() {
            CambioContrasena cambioCont = new CambioContrasena();

            try{
                int codigo_usuario_logueado = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();
                cambioCont.Id = codigo_usuario_logueado;
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return View("Editar/_getCambioContrasena", cambioCont);
        }


        /// <summary>
        /// Actualiza el perfil de usuario
        /// </summary>
        /// <param name="perfil"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.ConfiguracionUsuario)]
        public ActionResult actualizarPerfil(Perfil perfil) {

            JsonResult resultado;

            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                
                //Se obtiene el usuario de base de datos
                USUARIO us = configuracionRepo.obten_USUARIO_ById(perfil.Usuario.Id);


                if( authRepo.obten_USUARIO_ByNombreUsuarioCorreo(us.NM_USUARIO
                                                                , perfil.Usuario.ContrasenaActual)==null)
                    throw new BussinessException(Resources.ErrorResource.ContrasenaInvalida);

                USUARIO usModif = perfil.Usuario.toModel();

                us.FE_NACIMIENTO = usModif.FE_NACIMIENTO;
                us.DI_EMAIL_USUARIO = usModif.DI_EMAIL_USUARIO;
                us.NU_TELEFONO_FIJO = usModif.NU_TELEFONO_FIJO;
                us.NU_TELEFONO_MOVIL = usModif.NU_TELEFONO_MOVIL;

                if (configuracionRepo.actualiza_USUARIO(us)!=1)
                    throw new BussinessException(Resources.ErrorResource.FalloActulizarPerfil);

                log.Info(String.Format(Resources.LoggingResource.PerfilUsuarioActualizado, perfil.Usuario.Id));

                resultado = IntranetWeb.Core.Utils.UtilJson.Exito(Resources.ExitoResource.PerfilActualizado);

            }
            catch (BussinessException exc) {
                resultado = IntranetWeb.Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = IntranetWeb.Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cambiarContrasena"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.ConfiguracionUsuario)]
        public ActionResult cambiarContrasena(CambioContrasena cambiarContrasena)
        {
            String resultado = "";

            try {

                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                string username = identity.Claims.Where(x => x.Type == UserClaimType.UserName).FirstOrDefault().Value;
                               

                USUARIO usuario = authRepo.obten_USUARIO_ByNombreUsuarioCorreo(username
                                                                 , cambiarContrasena.ContrasenaActual);

                //Se busca el usuario y se compara con el que está registrado en la base de datos
                if (usuario != null){

                    byte[] nuevaContrasena = authRepo.encriptaCadena(cambiarContrasena.ContrasenaNueva);

                    USUARIO usu = configuracionRepo.obten_USUARIO_ById(cambiarContrasena.Id);
                    usu.DE_CONTRASENA = nuevaContrasena;
                    configuracionRepo.actualiza_USUARIO(usu);

                    bool enviado = false;
                    enviado = SmtpHelper.Send(IntranetWeb.Core.Constante.Mensaje.AsuntoCorreo.CambioContrasena, MailTemplate.PrepararTemplate(String.Format(Resources.EncabezadoCorreoResource.Hola, usuario.DE_NOMBRE_APELLIDO), String.Format(Resources.MensajeCorreoResource.CambioContrasena, cambiarContrasena.ContrasenaNueva)), usuario.DI_EMAIL_USUARIO);
                    
                    resultado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeExito(Resources.ExitoResource.CambioContrasena+(enviado?Resources.ExitoResource.ContrasenaEnviada:""));
                }
                else
                    throw new BussinessException(Resources.ErrorResource.ContrasenaInvalida); 
                   
            }
            catch (BussinessException exc) {
                resultado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(exc.Message);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(Resources.ErrorResource.Error100);
            }

            return Content(resultado, "text/thtml");
        }
    }
}