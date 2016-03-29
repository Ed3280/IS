using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Exception;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.ViewModel.Administrador;
using IntranetWeb.ViewModel.Shared;
using IntranetWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using IntranetWeb.ViewModel.Interface;
using IntranetWeb.Core.Utils;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class AdministradorController : Controller
    {
        // GET: Administrador
        private Log4NetLogger log;
        private AdministradorRepositorio adminRepo;
        private ValidacionRepositorio validacionRepo;
        private AuthRepositorio authRepo;

        public AdministradorController(){
            log = new Log4NetLogger();
            adminRepo = new AdministradorRepositorio();
            validacionRepo = new ValidacionRepositorio();
            authRepo = new AuthRepositorio();
        }
      

        /// <summary>
        /// Consulta el listado de registros del log de transacciones
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.ConsultarLogTransacciones)]
        public ActionResult LogTransacciones() {
            LogTransacciones lt = new LogTransacciones();
            lt.FechaBusquedaDesde = DateTime.Today.AddMonths(-1);
            lt.FechaBusquedaHasta = DateTime.Today;

            cargaSelectLogTransacciones(lt);
            return View(lt);
        }
        /// <summary>
        /// Consulta del log de transacciones via ajax
        /// </summary>
        /// <param name="cdUsuario">Código de usuario</param>
        /// <param name="feDesde">Fecha Desde</param>
        /// <param name="feHasta">Fecha Hast</param>
        /// <param name="cdNivel">Nivel del log</param>
        /// <param name="parametro"> Parámetros de entrada</param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.ConsultarLogTransacciones)]
        [HttpPost]
        public ActionResult consultarLog(int? cdUsuario, DateTime? feDesde, DateTime? feHasta, string cdNivel,  DTParameters parametro) {
            JsonResult result;

            try{

                var dtsource = adminRepo.obtenLogTransacciones(
                                                                 cdUsuario
                                                                ,feDesde
                                                                ,feHasta
                                                                ,cdNivel
                                                                );

                List<LogTransacciones> data = new ResultSet().GetResult(parametro.Search.Value, parametro.SortOrder, parametro.Start, parametro.Length, dtsource.ToList(), new List<string>());
                int count = new ResultSet().Count(parametro.Search.Value, dtsource.ToList(), new List<string>());

                DTResult<LogTransacciones> resultData = new DTResult<LogTransacciones>
                {
                    draw = parametro.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                                
                result =  Core.Utils.UtilJson.ExitoDataTableOnServer<LogTransacciones>(resultData);
                return result;
            }
            catch (BussinessException exc) {
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc) {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
                
            }
            return result;
        }


       
        /// <summary>
        /// Busca el listado de usuarios
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin)]
        [HttpPost]
        public ActionResult consultarUsuariosAdmin( DTParameters parametro)
        {
            JsonResult result;

            try
            {

                var dtsource = adminRepo.obtenUsuario(null);

                List<Usuario> data = new ResultSet().GetResult(parametro.Search.Value, parametro.SortOrder, parametro.Start, parametro.Length, dtsource.ToList(), new List<string>());
                int count = new ResultSet().Count(parametro.Search.Value, dtsource.ToList(), new List<string>());

                DTResult<Usuario> resultData = new DTResult<Usuario>
                {
                    draw = parametro.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                result = Core.Utils.UtilJson.ExitoDataTableOnServer<Usuario>(resultData);
                return result;
            }
            catch (BussinessException exc)
            {
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);

            }
            return result;
        }




        /// <summary>
        /// Busca el listado de usuarios
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        [HttpPost]
        public ActionResult consultarUsuarios(DTParameters parametro)
        {
            JsonResult result;

            try
            {
                int cdUsuario = Core.Utils.UtilHelper.obtenUsuarioLogueado();
                var dtsource = adminRepo.obtenUsuario(cdUsuario);

                List<Usuario> data = new ResultSet().GetResult(parametro.Search.Value, parametro.SortOrder, parametro.Start, parametro.Length, dtsource.ToList(), new List<string>());
                int count = new ResultSet().Count(parametro.Search.Value, dtsource.ToList(), new List<string>());

                DTResult<Usuario> resultData = new DTResult<Usuario>
                {
                    draw = parametro.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                result = Core.Utils.UtilJson.ExitoDataTableOnServer<Usuario>(resultData);
                return result;
            }
            catch (BussinessException exc)
            {
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);

            }
            return result;
        }


        /// <summary>
        /// Busca el listado de socios comerciales 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        [HttpPost]
        public ActionResult consultarSociosComerciales(DTParameters parametro) {
            JsonResult result;

            try
            {

                var dtsource = adminRepo.obtenSocioComercial();

                List<SocioComercial> data = new ResultSet().GetResult(parametro.Search.Value, parametro.SortOrder, parametro.Start, parametro.Length, dtsource.ToList(), new List<string>());
                int count = new ResultSet().Count(parametro.Search.Value, dtsource.ToList(), new List<string>());

                DTResult<SocioComercial> resultData = new DTResult<SocioComercial>
                {
                    draw = parametro.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                result = Core.Utils.UtilJson.ExitoDataTableOnServer<SocioComercial>(resultData);
                return result;
            }
            catch (BussinessException exc)
            {
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);

            }
            return result;
        }

        /// <summary>
        /// Método para buscar los datos de un usuario existente 
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public JsonResult consultarDatosUsuarioSocioComercial(string TipoDocumentoSeleccionado
                                                             ,string NumeroDocumento) {
            JsonResult result = null;
            try {
                //Se busca l información del socio comercial
                SocioComercial socio = new SocioComercial();

                if (!validacionRepo.numeroDocumentoSocioComercialEnUso(TipoDocumentoSeleccionado
                                                              , NumeroDocumento))
                {
                    //Se busca la información del usuario
                    socio = adminRepo.obten_SocioComercialNoRegistrado_By_NumeroDocumento(TipoDocumentoSeleccionado
                                                                                     , NumeroDocumento);
                    socio = socio == null ? new SocioComercial() : socio;
                }

                result = IntranetWeb.Core.Utils.UtilJson.Exito(socio);                
            }
            catch (Exception exc) {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }

            return result;
        }


        /// <summary>
        /// Método para buscar los datos de un usuario existente para la creación de un usuario interno
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public JsonResult consultarDatosUsuarioInterno(string TipoDocumentoSeleccionado
                                                       , string NumeroDocumento)
        {
            JsonResult result = null;
            try
            {
                //Se busca la información del usuario interno
                Usuario usuarioInterno = new Usuario();
                int cdUsuarioPadre = Core.Utils.UtilHelper.obtenUsuarioLogueado(); 
                    
                if (!validacionRepo.numeroDocumentoUsuarioInternoEnUso(
                                                                 cdUsuarioPadre
                                                              ,  TipoDocumentoSeleccionado
                                                              ,  NumeroDocumento))
                {
                    //Se busca la información del usuario
                    usuarioInterno = adminRepo.obten_UsuarioInternoNoRegistrado_By_NumeroDocumento(TipoDocumentoSeleccionado
                                                                                                    , NumeroDocumento);
                    usuarioInterno = usuarioInterno == null ? new Usuario() : usuarioInterno;
                }

                result = IntranetWeb.Core.Utils.UtilJson.Exito(usuarioInterno);
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }

            return result;
        }



        /// <summary>
        /// Método para buscar los datos de un usuario existente para la creación de un usuario interno
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult consultarDatosEmpleado(string TipoDocumentoSeleccionado
                                                       , string NumeroDocumento)
        {
            JsonResult result = null;
            try
            {
                //Se busca la información del empleado
                Empleado empleado = new Empleado();
                

                if (!validacionRepo.numeroDocumentoEmpleadoEnUso(                                                                
                                                               TipoDocumentoSeleccionado
                                                              , NumeroDocumento))
                {
                    //Se busca la información del empleado
                    empleado = adminRepo.obten_EmpleadoNoRegistrado_By_NumeroDocumento(TipoDocumentoSeleccionado
                                                                                      , NumeroDocumento);
                    empleado = empleado == null ? new Empleado() : empleado;
                }

                result = IntranetWeb.Core.Utils.UtilJson.Exito(empleado);
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }

            return result;
        }

        

        /// <summary>
        /// Busca el listado de usuarios
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>

        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario
            , Core.Constante.Aplicacion.AdministrarEmpleado)]
        [HttpPost]
        public ActionResult consultarEmpleados(DTParameters parametro)
        {
            JsonResult result;

            try
            {

                var dtsource = adminRepo.obtenEmpleado();

                List<Usuario> data = new ResultSet().GetResult(parametro.Search.Value, parametro.SortOrder, parametro.Start, parametro.Length, dtsource.ToList(), new List<string>());
                int count = new ResultSet().Count(parametro.Search.Value, dtsource.ToList(), new List<string>());

                DTResult<Usuario> resultData = new DTResult<Usuario>
                {
                    draw = parametro.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                result = Core.Utils.UtilJson.ExitoDataTableOnServer<Usuario>(resultData);
                return result;
            }
            catch (BussinessException exc)
            {
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);

            }
            return result;
        }



        /// <summary>
        /// Busca el detalle del registro del log de transacciones
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.ConsultarLogTransacciones)]
        [HttpGet]
        public ActionResult _getConsultarLog(int id) {
            LogTransacciones lt = new LogTransacciones();
            JsonResult result;
            try{
                lt = adminRepo.obtenLogTransacciones_ById(id);
                return View("Consultar/_getDetalleLog", lt);

            }           
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);                
            }

            return result;
        }



        /// <summary>
        /// Busca el detalle del usuario
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        [HttpGet]
        public ActionResult _getConsultarUsuario(int Id)
        {
            Usuario usu = new Usuario();

            JsonResult result;
            try
            {
                int IdUsuarioPadre = Core.Utils.UtilHelper.obtenUsuarioLogueado(); 
                usu = adminRepo.obtenUsuario_ById(IdUsuarioPadre, Id);

                cargaSelectUsuario(usu);
                return View("Consultar/_getConsultarUsuario", usu);

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }




        /// <summary>
        /// Busca el detalle del usuario (Admin)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin
                , Core.Constante.Aplicacion.AdministrarEmpleado)]
        [HttpGet]
        public ActionResult _getConsultarUsuarioAdmin(int Id)
        {
            Usuario usu = new Usuario();
            
            JsonResult result;
            try{
                usu = adminRepo.obtenUsuario_ById(null,Id);
                cargaSelectUsuario(usu);
                return View("Consultar/_getConsultarUsuarioAdmin", usu);

            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }

        /// <summary>
        /// Busca el detalle del socio comercial
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        [HttpGet]
        public ActionResult _getConsultarSocioComercial(int Id) {

            SocioComercial socio = new SocioComercial();
            JsonResult result;

            try {

                socio = adminRepo.obtenSocioComercial_ById(Id);
                cargaSelectSocioComercial(socio);
                IModeloAplicacion aplicacionesSocio = adminRepo.obtenAplicacionesSocioComercial_ById(Id);
                socio.Aplicaciones = aplicacionesSocio.Aplicaciones;
                return View("Consultar/_getConsultarSocioComercial", socio);

            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);

            }
            return result;
        }



      


        /// <summary>
        ///  Crea registro de Respuesta de Operador por GCM
        /// </summary>
        /// <param name="respuestaOperadorGCM"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.RespuestaOperadorGCM)]
        public JsonResult crearRespuestaOperadorGCM(RespuestaOperadorGCM respuestaOperadorGCM) {
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";
                var fechaActual = DateTime.Now;
                respuestaOperadorGCM.FeEstatus = fechaActual;
                RESPUESTA_OPERADOR_GCM respModel = respuestaOperadorGCM.toModel();
                adminRepo.guarda_RESPUESTA_OPERADOR_GCM(respModel);
                log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.RespuestaOperadorGCMCreado, respModel.ID_RESPUESTA_OPERADOR));
                resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.RespuestaOperadorGCMCreada, respModel.ID_RESPUESTA_OPERADOR), new { ID_RESPUESTA = respModel.ID_RESPUESTA_OPERADOR, mensajeAdvertencia = mensajeAdvertencia });

            }
            catch (BussinessException exc)
            {
                resultado= Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;            
        }


        /// <summary>
        ///  Crea registro de Parámetro de configuración
        /// </summary>
        /// <param name="respuestaOperadorGCM"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.ParametroConfiguracion)]
        public JsonResult crearParametroConfiguracion(ParametroConfiguracion parametroConfiguracion)
        {
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));


                if (adminRepo.obtenParametroConfiguracion_ById(parametroConfiguracion.NombreParametro, parametroConfiguracion.CodigoConfiguracion) != null)
                    throw new BussinessException(Core.Constante.Mensaje.Error.ParametroConfiguracionExiste);

                string mensajeAdvertencia = "";
                if (adminRepo.guarda_PARAMETRO_CONFIGURACION(parametroConfiguracion.toModel()) > 0)
                {
                    log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.ParametroConfiguracionCreado, parametroConfiguracion.NombreParametro, parametroConfiguracion.CodigoConfiguracion));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.ParametroConfiguracionCreado, parametroConfiguracion.NombreParametro, parametroConfiguracion.CodigoConfiguracion), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
            }

            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }


        /// <summary>
        ///  Crea registro de Respuesta de Operador por GCM
        /// </summary>
        /// <param name="respuestaOperadorGCM"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.UnidadAdministrativa)]
        public JsonResult crearUnidadAdministrativa(UnidadAdministrativa unidadAdministrativa)
        {
            JsonResult resultado;
            string mensajeAdvertencia = "";
            int Id;

            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                unidadAdministrativa.FechaCreacion = DateTime.Now;

                Id = adminRepo.guarda_UNIDAD_ADMINISTRATIVA(unidadAdministrativa.toModel());
                if (Id > 0){
                    log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.UnidadAdministrativaCreada, Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.UnidadAdministrativaCreada, unidadAdministrativa.Nombre), new { ID_RESPUESTA = Id, mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }



        /// <summary>
        ///  Crea registro de rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarRol)]
        public JsonResult crearRol(Rol rol)
        {
            JsonResult resultado;
            string mensajeAdvertencia = "";
            int Id;

            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                Id = adminRepo.guarda_Rol(rol); 
                if (Id > 0){
                    log.Info(String.Format(Resources.LoggingResource.RolCreado, Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.RolCreado, rol.Nombre), new { ID_RESPUESTA = Id, mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }


        /// <summary>
        ///  Crea registro de Respuesta de Operador por GCM
        /// </summary>
        /// <param name="respuestaOperadorGCM"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin)]
        public JsonResult crearUsuarioAdmin(UsuarioAdmin usuario) {

            JsonResult resultado;
            string mensajeAdvertencia = "";
            int Id = 0;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                Id = adminRepo.guarda_Usuario(usuario);
                if (Id > 0){
                    log.Info(String.Format(Resources.LoggingResource.UsuarioCreado, Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.UsuarioCreado, usuario.NombreApellido), new { ID_RESPUESTA = Id, mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
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
        /// Método para crear un nuevo empleado
        /// </summary>
        /// <param name="empleado">Empleado a crear</param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza( Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult crearEmpleado(Empleado empleado){

            JsonResult resultado;
            string mensajeAdvertencia = "";
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                empleado.RolesDisponibles = UtilRepositorio.obtenSelect_RolesEmpleado(empleado.Id, empleado.CargoSeleccionado);

                if (adminRepo.guarda_Empleado(ref empleado))
                {
                    log.Info(String.Format(Resources.LoggingResource.EmpleadoCreado, empleado.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.EmpleadoCreado, empleado.NombreApellido), new { ID_RESPUESTA = empleado.Id, IN_ENVIA_CORREO = true, mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }


        /// <summary>
        /// Crea un nuevo usuario interno
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public JsonResult crearUsuario(Usuario usuario) {

            JsonResult resultado;
            string mensajeAdvertencia = "";
           
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));
                
                int CodigoUsuarioPadre = Core.Utils.UtilHelper.obtenUsuarioLogueado();
                if (adminRepo.guarda_UsuarioInterno(ref usuario, CodigoUsuarioPadre)){
                    log.Info(String.Format(Resources.LoggingResource.UsuarioCreado, usuario.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.UsuarioCreado, usuario.NombreApellido), new { ID_RESPUESTA = usuario.Id, IN_ENVIA_CORREO = true, mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }

        /// <summary>
        /// Edita un usuario interno
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public JsonResult editarUsuario(Usuario usuario) {
            JsonResult resultado;
            string mensajeAdvertencia = "";
            int Id = 0;

            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                int CodigoUsuarioPadre = Core.Utils.UtilHelper.obtenUsuarioLogueado();
                if (adminRepo.actualiza_UsuarioInterno(ref usuario, CodigoUsuarioPadre))
                {
                    log.Info(String.Format(Resources.LoggingResource.UsuarioCreado, Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.UsuarioActualizado, usuario.NombreApellido), new { ID_RESPUESTA = Id, mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);

            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }


        /// <summary>
        /// Crea registro de socio comercial
        /// </summary>
        /// <param name="socioComercial"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public JsonResult crearSocioComercial(SocioComercial socioComercial) {

            JsonResult resultado;
            string mensajeAdvertencia = "";
            DateTime fechaActual = DateTime.Now;

            try{
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                socioComercial.FechaCreacion = fechaActual;
                socioComercial.FechaEstatus = fechaActual;

                bool resultGuardar = adminRepo.guarda_SocioComercial(ref socioComercial);

                if (resultGuardar){
                    log.Info(String.Format(Resources.LoggingResource.SocioComercialCreado, socioComercial.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.SocioComercialCreado, socioComercial.Nombre), new {CD_SOCIO_COMERCIAL = socioComercial.Id, mensajeAdvertencia = mensajeAdvertencia } );
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
                
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



      



        /// <summary>
        /// Retorna la lista de cargos
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarCargo)]
        public JsonResult crearCargo(Cargo cargo)
        {
            JsonResult resultado;
            string mensajeAdvertencia = "";
            int Id = 0;
            AuthRepositorio authRepo = new AuthRepositorio();
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                Id = adminRepo.guarda_Cargo(cargo);

                if (Id > 0)
                {
                    log.Info(String.Format(Resources.LoggingResource.CargoCreado, Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.CargoCreado, cargo.Nombre), new { ID_CARGO = Id, mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }

        /// <summary>
        /// Editar la respuesta al usuario
        /// </summary>
        /// <param name="respuestaOperadorGCM"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.RespuestaOperadorGCM)]
        public JsonResult editarRespuestaOperadorGCM(RespuestaOperadorGCM respuestaOperadorGCM)
        {
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";
                var fechaActual = DateTime.Now;
                respuestaOperadorGCM.FeEstatus = fechaActual;
                RESPUESTA_OPERADOR_GCM respModel = respuestaOperadorGCM.toModel();
                adminRepo.actualiza_RESPUESTA_OPERADOR_GCM(respModel);
                log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.RespuestaOperadorGCMActualizado, respModel.ID_RESPUESTA_OPERADOR));
                resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.RespuestaOperadorGCMActualizada, respModel.ID_RESPUESTA_OPERADOR), new { ID_RESPUESTA = respModel.ID_RESPUESTA_OPERADOR, mensajeAdvertencia = mensajeAdvertencia });

            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }


        /// <summary>
        /// Método par modificar la información de un empleado
        /// </summary>
        /// <param name="empleado"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult editarEmpleado(Empleado empleado) {

            JsonResult resultado;
            string mensajeAdvertencia = "";
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                empleado.RolesDisponibles = UtilRepositorio.obtenSelect_RolesEmpleado(empleado.Id, empleado.CargoSeleccionado);

                if (adminRepo.actualiza_Empleado(ref empleado))
                {
                    log.Info(String.Format(Resources.LoggingResource.EmpleadoActualizado, empleado.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.EmpleadoActualizado, empleado.NombreApellido), new { ID_RESPUESTA = empleado.Id, mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Core.Constante.Mensaje.Error.RegistroNoCreado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;

        }


        /// <summary>
        /// Editar la información de un cargo
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarCargo)]
        public JsonResult editarCargo(Cargo cargo) {
            JsonResult resultado;
           
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";

                if ( adminRepo.actualizaCargo(cargo) > 0){
                    log.Info(String.Format(Resources.LoggingResource.CargoActualizado, cargo.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.CargoActualizado, cargo.Nombre), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoActualizado);

            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;

        }


        /// <summary>
        /// Editar el parámetro de configuración 
        /// </summary>
        /// <param name="parámetro de configuración "></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.ParametroConfiguracion)]
        public JsonResult editarParametroConfiguracion(ParametroConfiguracion parametroConfiguracion)
        {
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";

                //Se verifica si exuste algún otro registro con ese Id
                if (parametroConfiguracion.CodigoConfiguracion != parametroConfiguracion.CodigoConfiguracionEditar
                    && adminRepo.obtenParametroConfiguracion_ById(parametroConfiguracion.NombreParametro, parametroConfiguracion.CodigoConfiguracionEditar) != null)
                    throw new BussinessException(Core.Constante.Mensaje.Error.ParametroConfiguracionExiste);


                if (adminRepo.actualiza_PARAMETRO_CONFIGURACION(parametroConfiguracion.toModel()) > 0)
                {
                    log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.ParametroConfiguracionActualizado, parametroConfiguracion.NombreParametro, parametroConfiguracion.CodigoConfiguracion, parametroConfiguracion.CodigoConfiguracionEditar));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.ParametroConfiguracionActualizado, parametroConfiguracion.NombreParametro, parametroConfiguracion.CodigoConfiguracionEditar), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoActualizado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }



        /// <summary>
        /// Editar una unidad administrativa
        /// </summary>
        /// <param name="unidadAdministrativa">Unidad Administrativa a editar</param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.UnidadAdministrativa)]
        public JsonResult editarUnidadAdministrativa(UnidadAdministrativa unidadAdministrativa)
        {
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";

                
                if (adminRepo.actualiza_UNIDAD_ADMINISTRATIVA(unidadAdministrativa.toModel()) > 0)
                {
                    log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.UnidadAdministrativaActualizada, unidadAdministrativa.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.UnidadAdministrativaActualizada, unidadAdministrativa.Nombre), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoActualizado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
               
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }

        /// <summary>
        /// Edita un socio comercial
        /// </summary>
        /// <param name="socioComercial"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public JsonResult editarSocioComercial(SocioComercial socioComercial)
        {
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";


                if (adminRepo.actualiza_SocioComercial(ref socioComercial))
                {
                    log.Info(String.Format(Resources.LoggingResource.SocioComercialActualizado, socioComercial.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.SocioComercialActualizado, socioComercial.Nombre), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoActualizado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {

                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }
        

        /// <summary>
        /// Editar un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin)]
        public JsonResult editarUsuarioAdmin(UsuarioAdmin usuario){
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";
                usuario.RolesDisponibles = UtilRepositorio.obtenSelect_Roles(usuario.Id);

                if (adminRepo.actualizaUsuario(usuario) > 0){
                    log.Info(String.Format(Resources.LoggingResource.UsuarioActualizado, usuario.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.UsuarioActualizado, usuario.NombreApellido), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else                  
                    throw new BussinessException(Resources.ErrorResource.RegistroNoActualizado);

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
        /// Se emplea para el envío de correo de registro
        /// </summary>
        /// <param name="id">Id del ticket</param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public JsonResult enviarCorreoRegistroSocioComercial(int Id)
        {

            JsonResult resultado;

            try
            {
                String titulo, plantilla;

                //Se busca al cliente
                SocioComercial socioComercial = adminRepo.obtenSocioComercial_ById(Id);
                MailTemplate.PrepararTemplateRegistroSocio(Id
                                                   , out titulo
                                                   , out plantilla);
                
                //Se buscan los socios comerciales asociados al cliente
                if (SmtpHelper.Send( titulo
                                    , plantilla
                                    , socioComercial.Email)){

                    //Se actualiza el indicador de correo enviado
                    USUARIO usuarioBD = authRepo.obten_USUARIO_ById(Id);
                    usuarioBD.FE_ESTATUS = DateTime.Now;
                    usuarioBD.IN_CORREO_REGISTRO_ENVIADO = true;
                    authRepo.actualiza_USUARIO(usuarioBD);
                }
                else
                    throw new BussinessException(Resources.ErrorResource.CorreoRegistroNoEnviado);
                
                resultado = Core.Utils.UtilJson.Exito(Resources.ExitoResource.CorreoRegistroEnviado);

            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {

                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return resultado;
        }


        /// <summary>
        /// Se emplea para el envío de correo de registro
        /// </summary>
        /// <param name="id">Id del ticket</param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public JsonResult enviarCorreoRegistroUsuarioInterno(int Id)
        {

            JsonResult resultado;

            try
            {
                String titulo, plantilla;

                int IdUsuarioInterno = Core.Utils.UtilHelper.obtenUsuarioLogueado();

                //Se busca al cliente
                Usuario usuario = adminRepo.obtenUsuario_ById(null,Id);
                MailTemplate.PrepararTemplateRegistroUsuarioInterno(
                                                      Id
                                                    , IdUsuarioInterno
                                                   , out titulo
                                                   , out plantilla);

                //Se buscan los socios comerciales asociados al cliente
                if (SmtpHelper.Send(titulo
                                    , plantilla
                                    , usuario.Email))
                {

                    //Se actualiza el indicador de correo enviado
                    USUARIO usuarioBD = authRepo.obten_USUARIO_ById(Id);
                    usuarioBD.FE_ESTATUS = DateTime.Now;
                    usuarioBD.IN_CORREO_REGISTRO_ENVIADO = true;
                    authRepo.actualiza_USUARIO(usuarioBD);
                }
                else
                    throw new BussinessException(Resources.ErrorResource.CorreoRegistroNoEnviado);

                resultado = Core.Utils.UtilJson.Exito(Resources.ExitoResource.CorreoRegistroEnviado);

            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {

                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return resultado;
        }


        /// <summary>
        /// Se emplea para el envío de correo de registro
        /// </summary>
        /// <param name="id">Id del empleado</param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult enviarCorreoEmpleado(int Id)
        {

            JsonResult resultado;

            try
            {
                String titulo, plantilla;

                int IdUsuarioInterno = Core.Utils.UtilHelper.obtenUsuarioLogueado();

                //Se busca al cliente
                Usuario usuario = adminRepo.obtenUsuario_ById(null, Id);
                MailTemplate.PrepararTemplateRegistroEmpleado(
                                                      Id                                                    
                                                   , out titulo
                                                   , out plantilla);

                //Se buscan los socios comerciales asociados al empleado
                if (SmtpHelper.Send(titulo
                                    , plantilla
                                    , usuario.Email))
                {

                    //Se actualiza el indicador de correo enviado
                    USUARIO usuarioBD = authRepo.obten_USUARIO_ById(Id);
                    usuarioBD.FE_ESTATUS = DateTime.Now;
                    usuarioBD.IN_CORREO_REGISTRO_ENVIADO = true;
                    authRepo.actualiza_USUARIO(usuarioBD);
                }
                else
                    throw new BussinessException(Resources.ErrorResource.CorreoRegistroNoEnviado);

                resultado = Core.Utils.UtilJson.Exito(Resources.ExitoResource.CorreoRegistroEnviado);

            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {

                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return resultado;
        }


        /// <summary>
        /// Editar un rol
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarRol)]
        public JsonResult editarRol(Rol rol) {
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";

                if (adminRepo.actualizaRol(rol) > 0)
                {
                    log.Info(String.Format(Resources.LoggingResource.RolActualizado, rol.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.RolActualizado, rol.Nombre), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoActualizado);
            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }

        

        /// <summary>
        /// Dar de baja a un empleado
        /// </summary>
        /// <param name="empleado"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin
                , Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult darBajaEmpleado(EmpleadoBaja empleado)
        {
            JsonResult resultado;
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                string mensajeAdvertencia = "";
                
                if (adminRepo.darBajaEmpleado(empleado))
                {
                    log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.EmpleadoBaja, empleado.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.EmpleadoBaja, empleado.NombreApellido), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoActualizado);

            }
            catch (BussinessException exc)
            {
                resultado = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return resultado;
        }
        

        /// <summary>
        /// Retorna el formulario para crear una nueva respuesta de operador 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.RespuestaOperadorGCM)]
        public ActionResult _getCrearRespuestaOperadorGCM() {
            RespuestaOperadorGCM rGCM = new RespuestaOperadorGCM();
            JsonResult result;
            try
            {
                cargaSelectRespuestaGCM(rGCM);
                rGCM.InActivo = true;
                return View("Crear/_getCrearRespuestaOperadorGCM", rGCM);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }



        /// <summary>
        /// Retorna el formulario para crear una nueva respuesta de operador 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.ParametroConfiguracion)]
        public ActionResult _getCrearParametroConfiguracion()
        {
            JsonResult result;
            try{
                ParametroConfiguracion pc = new ParametroConfiguracion();
                return View("Crear/_getCrearParametroConfiguracion", pc);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }



        /// <summary>
        /// Retorna el formulario para crear una nueva respuesta de operador 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.UnidadAdministrativa)]
        public ActionResult _getCrearUnidadAdministrativa()
        {
            JsonResult result;
            try
            {
                return View("Crear/_getCrearUnidadAdministrativa");
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }


        /// <summary>
        /// Retorna el formulario para crear un nuevo usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin)]
        public ActionResult _getCrearUsuarioAdmin()
        {
            JsonResult result;
            try {
                UsuarioAdmin usu = new UsuarioAdmin();
                cargaSelectUsuario(usu);
                usu.Aplicaciones = adminRepo.obtenAplicacionesUsuario();
                usu.IndicadorActivo = true;            
                return View("Crear/_getCrearUsuarioAdmin", usu);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }



        /// <summary>
        /// Retorna el formulario para crear un nuevo usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public ActionResult _getCrearUsuario()
        {
            JsonResult result;
            try
            {
                Usuario usu = new Usuario();
                usu.CodigoUsuarioPadre = IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado();
                cargaSelectUsuario(usu);           
                usu.Aplicaciones = adminRepo.obtenAplicacionesUsuario();
                usu.IndicadorActivo = true;
                return View("Crear/_getCrearUsuario", usu);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }


        /// <summary>
        /// Acción para crear un cargo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarCargo)]
        public ActionResult _getCrearCargo() {

            JsonResult result;

            try {
                Cargo cargo = new Cargo();
                cargaSelectCargo(cargo);
                return View("Crear/_getCrearCargo", cargo);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }


        /// <summary>
        /// Acción para crear un rol
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarRol)]
        public ActionResult _getCrearRol() {
            JsonResult result;
            try
            {
                Rol   rolAplicacion = new Rol();
                rolAplicacion.Aplicaciones = adminRepo.obtenAplicacionesUsuario();

                rolAplicacion.Aplicaciones.ToList().ForEach(x => { x.IndicadorActiva = x.IndicadorDashBoard || x.IndicadorPerfilUsuario ? true : false;  });

                rolAplicacion.IndicadorActivo = true;

                return View("Crear/_getCrearRol", rolAplicacion);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;

        }

        /// <summary>
        /// Retorna el formulario para crear un nuevo usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarEmpleado)]
        public ActionResult _getCrearEmpleado()
        {
            JsonResult result;
            try
            {
                Empleado empleado = new Empleado();
                
                cargaSelectEmpleado(empleado);

                empleado.Aplicaciones = adminRepo.obtenAplicacionesUsuario();
                empleado.IndicadorActivo = true;
                empleado.FechaIngreso = DateTime.Today;
                return View("Crear/_getCrearEmpleado", empleado);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }

        /// <summary>
        /// Retorna el formulario de creación de socio comercial
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public ActionResult _getCrearSocioComercial() {

            JsonResult result;

            try{

                SocioComercial socio = new SocioComercial();
                cargaSelectSocioComercial(socio);
                socio.IndicadorActivo = true;
                return View("Crear/_getCrearSocioComercial", socio);
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }

        
     

        /// <summary>
        /// Retorna el formulario para editar un parámetro de configuración 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.ParametroConfiguracion)]
        public ActionResult _getEditarParametroConfiguracion(string nmParametro, string cdConfiguracion)
        {
            JsonResult result;
            ParametroConfiguracion pc = new ParametroConfiguracion();

            try {
                pc = adminRepo.obtenParametroConfiguracion_ById(nmParametro, cdConfiguracion);
                pc.CodigoConfiguracionEditar = pc.CodigoConfiguracion;
                return View("Editar/_getEditarParametroConfiguracion",pc);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }


        /// <summary>
        /// Retorna el formulario para editar un Unidad Administrativa 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.UnidadAdministrativa)]
        public ActionResult _getEditarUnidadAdministrativa(int Id)
        {
            JsonResult result;
            UnidadAdministrativa ua = new UnidadAdministrativa();

            try
            {
                ua = adminRepo.obtenUnidadAdministrativa_ById(Id);                
                return View("Editar/_getEditarUnidadAdministrativa", ua);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }
        
        /// <summary>
        /// Retorna el formulario para editar un usuario interno 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public ActionResult _getEditarUsuario(int Id)
        {
            JsonResult result;
            Usuario usu;
            try
            {
                int CodigoUsuarioPadre = Core.Utils.UtilHelper.obtenUsuarioLogueado(); 
                    usu = adminRepo.obtenUsuario_ById(CodigoUsuarioPadre, Id);
                    cargaSelectUsuario(usu);

                return View("Editar/_getEditarUsuario", usu);
            }
            catch (BussinessException exc)
            {
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
        /// Retorna el formulario para editar un empleado 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarEmpleado)]
        public ActionResult _getEditarEmpleado(int Id)
        {
            JsonResult result;
            Empleado empleado;
            try
            {

                ValidacionController validador = new ValidacionController();

                var resultVal = validador.verificaEmpleadoActivo(Id);

                if (resultVal.Data.GetType() == typeof(String))
                    throw new BussinessException((String)resultVal.Data);

                empleado = adminRepo.obtenEmpleado_ById(Id); 
                cargaSelectEmpleado(empleado);

                return View("Editar/_getEditarEmpleado", empleado);
            }
            catch (BussinessException exc)
            {
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
        /// Retorna el formulario para consultar un empleado 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarEmpleado)]
        public ActionResult _getConsultarEmpleado(int Id)
        {
            JsonResult result;
            Empleado empleado;
            try
            {
                empleado = adminRepo.obtenEmpleado_ById(Id);
                cargaSelectEmpleado(empleado);

                return View("Consultar/_getConsultarEmpleado", empleado);
            }
            catch (BussinessException exc)
            {
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
        /// Retorna el formulario para editar un Usuario 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin)]
        public ActionResult _getEditarUsuarioAdmin(int Id) {
            JsonResult result;
            UsuarioAdmin usu = new UsuarioAdmin();
            try
            {
                usu = adminRepo.obtenUsuario_ById(null,Id);
                cargaSelectUsuario(usu);
                return View("Editar/_getEditarUsuarioAdmin", usu);
            }
            catch (BussinessException exc)
            {
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
        /// Retorna el formulario para editar un Socio Comercial 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public ActionResult _getEditarSocioComercial(int Id)
        {
            JsonResult result;
            SocioComercial socio = new SocioComercial();
            try
            {
                socio = adminRepo.obtenSocioComercial_ById(Id);
                IModeloAplicacion aplicacionesSocio = adminRepo.obtenAplicacionesSocioComercial_ById(Id);
                socio.Aplicaciones = aplicacionesSocio.Aplicaciones;
                cargaSelectSocioComercial(socio);
                return View("Editar/_getEditarSocioComercial", socio);
            }
            catch (BussinessException exc)
            {
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
        /// Dado el id del cargo, permite su edición
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarCargo)]
        public ActionResult _getEditarCargo(int Id) {
            JsonResult result;
            Cargo cargo = new Cargo();
            try{

                cargo = adminRepo.obtenCargo_ById(Id);
                cargaSelectCargo(cargo);
                return View("Editar/_getEditarCargo", cargo);
            }
            catch (BussinessException exc)
            {
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
        /// Dado el id del rol, permite su edición
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarRol)]
        public ActionResult _getEditarRol(int Id) {

            JsonResult result;
            Rol rol = new Rol();
            try
            {

                rol = adminRepo.obtenRolAplicacion_ById(Id);
                return View("Editar/_getEditarRol", rol);
            }
            catch (BussinessException exc)
            {
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
        /// Dado el Id del usuario, se le da de baja
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin
            , Core.Constante.Aplicacion.AdministrarEmpleado)]
        public ActionResult _getEditarEmpleadoBaja(int Id) {
            JsonResult result;
            EmpleadoBaja usu = new EmpleadoBaja();
            try
            {
                ValidacionController validador = new ValidacionController();

                usu = adminRepo.obtenEmpleadoBaja_ById(Id);

                usu.IndicadorRequiereReeemplazo = adminRepo.obtenSubordinadosEmpleado(Id).Count() > 0 ? true : false;
                cargaSelectEmpleadoBaja(usu);

                return View("Editar/_getEditarEmpleadoBaja", usu);
            }
            catch (BussinessException exc)
            {
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }

/*
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin
                , Core.Constante.Aplicacion.AdministrarEmpleado)]
        public ActionResult _getRolesUsuario(int? cdUsuario, int? cdCargo) {

            JsonResult result;
            Usuario usu = new Usuario();
            try{
                usu.Roles = adminRepo.obtenRoles_ByCargoUsuario(cdUsuario,cdCargo);
                return View("../Shared/Configuracion/_getRoles", usu);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }
        */





        /// <summary>
        /// Permite eliminar un parámetro de configuración 
        /// </summary>
        /// <param name="nmParametro"></param>
        /// <param name="cdConfiguracion"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.ParametroConfiguracion)]
        public JsonResult eliminarParametroConfiguracion(string nmParametro, string cdConfiguracion) {

            JsonResult result;

            try{
                
                ParametroConfiguracion pc =  adminRepo.obtenParametroConfiguracion_ById(nmParametro, cdConfiguracion);
                if (pc == null)
                    throw new BussinessException(Core.Constante.Mensaje.Error.ParametroConfiguracionNoExiste);

                string mensajeAdvertencia = "";
                if (adminRepo.elimina_PARAMETRO_CONFIGURACION(pc.toModel()) > 0){
                    log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.ParametroConfiguracionEliminado, pc.NombreParametro, pc.CodigoConfiguracion));
                    result= Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.ParametroConfiguracionEliminado, pc.NombreParametro, pc.CodigoConfiguracion), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoEliminado);

            }
            catch (BussinessException exc)
            {
                result = Core.Utils.UtilJson.Error(exc.Message);
            }
            catch (DbUpdateException exc)
            {

                string relacion = Core.Utils.UtilHelper.obtenFKRelacion(exc);
                if (!String.IsNullOrWhiteSpace(relacion))
                    result = Core.Utils.UtilJson.Error(String.Format(Resources.ErrorResource.RegistroConFK, relacion));
                else
                {
                    log.Error(Resources.ErrorResource.Error100, exc);
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                }
            }
            catch (Exception exc){

                log.Error(Resources.ErrorResource.Error100, exc);
                result= Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }



        /// <summary>
        /// Permite eliminar una Unidad Administrativa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.UnidadAdministrativa)]
        public JsonResult eliminarUnidadAdministrativa(int Id)
        {

            JsonResult result;

            try
            {

                UnidadAdministrativa ua = adminRepo.obtenUnidadAdministrativa_ById(Id);
                if (ua == null)
                    throw new BussinessException(Core.Constante.Mensaje.Error.UnidadAdministrativaNoExiste);

                string mensajeAdvertencia = "";
                if (adminRepo.elimina_UNIDAD_ADMINISTRATIVA(ua.toModel()) > 0)
                {
                    log.Info(String.Format(IntranetWeb.Core.Constante.Mensaje.Logging.UnidadAdministrativaEliminada, ua.Id));
                    result = Core.Utils.UtilJson.Exito(String.Format(IntranetWeb.Core.Constante.Mensaje.Exito.UnidadAdministrativaEliminada, ua.Nombre), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoEliminado);

            }
            catch (DbUpdateException exc) {

                string relacion = Core.Utils.UtilHelper.obtenFKRelacion(exc);
                if (!String.IsNullOrWhiteSpace(relacion))
                     result = Core.Utils.UtilJson.Error(String.Format(Resources.ErrorResource.RegistroConFK, relacion));
                else{
                    log.Error(Resources.ErrorResource.Error100, exc);
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                }
            }
            catch (Exception exc)
            {                
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }


        /// <summary>
        /// Permite eliminar un usuario interno
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public JsonResult eliminarUsuarioInterno(int Id)
        {
            JsonResult result;
            try {
                result = null;

                string mensajeAdvertencia = "", nombreUsuario;

                int usuarioLogueado = Core.Utils.UtilHelper.obtenUsuarioLogueado();


                if (adminRepo.elimina_UsuarioInterno(usuarioLogueado, Id,  out nombreUsuario))
                {
                    log.Info(String.Format(Resources.LoggingResource.UsuarioInternoEliminado, Id, usuarioLogueado));
                    result = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.UsuarioEliminado, nombreUsuario), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoEliminado);

            }
            catch (DbUpdateException exc) {

                string relacion = Core.Utils.UtilHelper.obtenFKRelacion(exc);
                if (!String.IsNullOrWhiteSpace(relacion))
                     result = Core.Utils.UtilJson.Error(String.Format(Resources.ErrorResource.RegistroConFK, relacion));
                else{
                    log.Error(Resources.ErrorResource.Error100, exc);
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                }
            }
            catch (Exception exc)
            {                
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }


        /// <summary>
        /// Permite eliminar un Cargo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.AdministrarCargo)]
        public JsonResult eliminarCargo(int Id)
        {

            JsonResult result;

            try
            {

                Cargo cargo = adminRepo.obtenCargo_ById(Id);
                if (cargo == null)
                    throw new BussinessException(Resources.ErrorResource.CargoNoExiste);

                string mensajeAdvertencia = "";
                if (adminRepo.elimina_CARGO(cargo.toModel()) > 0)
                {
                    log.Info(String.Format(Resources.LoggingResource.CargoEliminado, cargo.Nombre));
                    result = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.CargoEliminado, cargo.Nombre), new { mensajeAdvertencia = mensajeAdvertencia });
                }
                else
                    throw new BussinessException(Resources.ErrorResource.RegistroNoEliminado);

            }
            catch (DbUpdateException exc){

                string relacion = Core.Utils.UtilHelper.obtenFKRelacion(exc);
                if (!String.IsNullOrWhiteSpace(relacion))
                    result = Core.Utils.UtilJson.Error(String.Format(Resources.ErrorResource.RegistroConFK, relacion));
                else
                {
                    log.Error(Resources.ErrorResource.Error100, exc);
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                }
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }






        /// <summary>
        /// Retorna el fomulario para editar una respuesta de operador
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.RespuestaOperadorGCM)]
        public ActionResult _getEditarRespuestaOperadorGCM(int id)
        {

            RespuestaOperadorGCM rGCM = new RespuestaOperadorGCM();
            JsonResult result; 

            try {
                rGCM = adminRepo.obtenRespuestaOperadorGCM_ById(id);
                cargaSelectRespuestaGCM(rGCM);

                return View("Editar/_getEditarRespuestaOperadorGCM", rGCM);
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;

        }

        /// <summary>
        /// Administración de los parámetros de configuración de la aplicación
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.ParametroConfiguracion)]
        public ActionResult ParametroConfiguracion()
        {
            ParametroConfiguracion pc = new ParametroConfiguracion();
            return View(pc);
        }


        /// <summary>
        /// Administración de las unidades administrativas
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.UnidadAdministrativa)]
        public ActionResult UnidadAdministrativa(){
            return View();
        }

        /// <summary>
        /// Adminsitraión de los usuarios del sistema
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin)]
        public ActionResult UsuarioAdmin() {
            return View();
        }

        /// <summary>
        /// Adminsitraión de los usuarios internos del sistema
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public ActionResult Usuario()
        {
            return View();
        }



        /// <summary>
        /// Administración de los soios comerciales
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public ActionResult SocioComercial() {
            return View();
        }

        /// <summary>
        /// Administración de los proveedores
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarProveedor)]
        public ActionResult Proveedor()
        {
            return View();
        }

        /// <summary>
        /// Adminsitraión de los empleados del sistema
        /// </summary>
        /// <returns></returns>

        [Autoriza(Core.Constante.Aplicacion.AdministrarCargo)]
        public ActionResult Cargo()
        {
            
            return View();
        }



        /// <summary>
        /// Busca las aplicaciones del socio comercial
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public ActionResult _getAplicacionesSocioComercial(int Id)
        {
            JsonResult result;
            
            try{
                IModeloAplicacion aplicaciones = adminRepo.obtenAplicacionesSocioComercial_ById(Id);
                return PartialView("../Shared/Configuracion/_getAplicaciones", aplicaciones);
            }
            catch (BussinessException exc)
            {
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
        /// Busca las aplicaciones disponibles para el usuario interno
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public ActionResult _getAplicacionesUsuarioInterno(int Id) {

            JsonResult result;

            try{
                int IdPadre = Core.Utils.UtilHelper.obtenUsuarioLogueado();
                IModeloAplicacion aplicaciones = adminRepo.obtenAplicacionesUsuarioInterno_ById(IdPadre, Id);
                return PartialView("../Shared/Configuracion/_getAplicaciones", aplicaciones);
            }
            catch (BussinessException exc)
            {
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
        /// Consulta de administrar cargos
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarCargo)]
        public ActionResult _getConsultarCargo()
        {

            IList<Cargo> cargos = new List<Cargo>();
            JsonResult result;
            try
            {
                //Se busca los cargos
                cargos = adminRepo.obtenCargo();
                return PartialView("Consultar/_getConsultarCargo", cargos);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                if (ControllerContext.IsChildAction)
                {
                    return PartialView("Consultar/_getConsultarCargo", cargos);
                }
                else
                {
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                }
            }
            return result;
        }


        /// <summary>
        /// Adminsitraión de los empleados del sistema
        /// </summary>
        /// <returns></returns>

        [Autoriza(Core.Constante.Aplicacion.AdministrarEmpleado)]
        public ActionResult Empleado()
        {
            return View();
        }

        [Autoriza(Core.Constante.Aplicacion.AdministrarRol)]
        public ActionResult Rol() {
            return View();
        }

        /// <summary>
        /// Carga la página de respuesta del operador GCM
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.RespuestaOperadorGCM)]
        public ActionResult RespuestaOperadorGCM(){
            RespuestaOperadorGCM lt = new RespuestaOperadorGCM();            
            return View(lt);

        }

        /// <summary>
        /// Muestra listado de registros en RESPUESTA_OPERADOR_GCM
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.RespuestaOperadorGCM)]
        public ActionResult consultarRespuestaOperadorGCM() {
            JsonResult result;

            try{
                var myData = adminRepo.obtenRespuestaOperadorGCM();
                return Content(JsonConvert.SerializeObject(myData), "application/json");
            }
            catch (Exception exc){
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return result;

        }

        /// <summary>
        /// Listado de roles
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.AdministrarRol)]
        public ActionResult consultarRoles() {
            JsonResult result;

            try
            {
                var myData = adminRepo.obtenRolAplicacion();
                return Content(JsonConvert.SerializeObject(myData), "application/json");
                
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return result;

        }
      
        /// <summary>
        /// Consulta de los parámetros de configuración del sistema
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.ParametroConfiguracion)]
        public ActionResult consultarParametroConfiguracion() {
            JsonResult result;

            try{

                var myData = adminRepo.obtenParametroConfiguracion();
                result = Core.Utils.UtilJson.Exito(Content(JsonConvert.SerializeObject(myData), "application/json"));
                return result;
            }
            catch (Exception exc){
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return result;            
        }


        /// <summary>
        /// Consulta de las unidades administrativas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.UnidadAdministrativa)]
        public ActionResult consultarUnidadAdministrativa(){

            JsonResult result;

            try{

                var myData = adminRepo.obtenUnidadAdministrativa();
                return Content(JsonConvert.SerializeObject(myData), "application/json");
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result  = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                
            }
            return result;
        }


        /// <summary>
        /// Carga select para el objeo respuesta GCM
        /// </summary>
        /// <param name="respuestaOperadorGCM"></param>
        [NonAction]
        public void cargaSelectRespuestaGCM(RespuestaOperadorGCM respuestaOperadorGCM) {
            respuestaOperadorGCM = respuestaOperadorGCM == null ? new RespuestaOperadorGCM() : respuestaOperadorGCM;
            respuestaOperadorGCM.TipoAtencion = UtilRepositorio.obtenSelect_TipoAtencion();
            respuestaOperadorGCM.TipoAtencionSeleccionada = respuestaOperadorGCM.TipoAtencion.Count() == 1 ? Convert.ToInt32(respuestaOperadorGCM.TipoAtencion.FirstOrDefault().Value) : respuestaOperadorGCM.TipoAtencionSeleccionada;
        }

        /// <summary>
        /// Carga select para el objeto log de transacciones
        /// </summary>
        /// <param name="logTransaccion"></param>
        [NonAction]
        public void cargaSelectLogTransacciones(LogTransacciones logTransaccion) {
            
            logTransaccion = logTransaccion == null ?  new LogTransacciones(): logTransaccion;

            logTransaccion.Usuario = UtilRepositorio.obtenSelect_USUARIO();
            logTransaccion.UsuarioSeleccionado = logTransaccion.Usuario.Count() == 1 ? Convert.ToInt32(logTransaccion.Usuario.FirstOrDefault().Value) : logTransaccion.UsuarioSeleccionado;
            logTransaccion.Nivel = UtilRepositorio.obtenSelect_NivelLog();
            logTransaccion.NivelSeleccionado = logTransaccion.Nivel.Count() == 1 ? logTransaccion.Nivel.FirstOrDefault().Value : logTransaccion.NivelSeleccionado;

        }


        [NonAction]
        public void cargaSelectUsuario(Usuario usuario) {

            usuario = usuario == null ? new Usuario() : usuario;
            
            usuario.TipoDocumento =  UtilRepositorio.obtenSelect_TipoDocumentoIdentidad();
            usuario.TipoDocumentoSeleccionado = usuario.TipoDocumento.Count() == 1 ? usuario.TipoDocumento.FirstOrDefault().Value : usuario.TipoDocumentoSeleccionado;
            usuario.DispositivosDisponibles = UtilRepositorio.obtenSelect_DispositivosPorUsuarioPadre(usuario.CodigoUsuarioPadre);
            usuario.RolesDisponibles = usuario.CodigoUsuarioPadre ==null? UtilRepositorio.obtenSelect_Roles(usuario.Id):UtilRepositorio.obtenSelect_RolesPorUsuarioPadre(usuario.Id,(int)usuario.CodigoUsuarioPadre);

            if (usuario.DatosEmpleado != null) {
                usuario.DatosEmpleado.UnidadAdministrativa = UtilRepositorio.obtenSelect_UnidadAdministrativa();
                usuario.DatosEmpleado.UnidadAdministrativaSeleccionada = usuario.DatosEmpleado.UnidadAdministrativa.Count() == 1 ? Convert.ToInt32(usuario.DatosEmpleado.UnidadAdministrativa.FirstOrDefault().Value) : usuario.DatosEmpleado.UnidadAdministrativaSeleccionada;
                usuario.DatosEmpleado.Cargo = UtilRepositorio.obtenSelect_Cargo(usuario.DatosEmpleado.UnidadAdministrativaSeleccionada);
                usuario.DatosEmpleado.CargoSeleccionado = usuario.DatosEmpleado.Cargo.Count() == 1 ? Convert.ToInt32(usuario.DatosEmpleado.Cargo.FirstOrDefault().Value) : usuario.DatosEmpleado.CargoSeleccionado;
                usuario.DatosEmpleado.Supervisor = UtilRepositorio.obtenSelect_Supervisor(usuario.DatosEmpleado.CargoSeleccionado, usuario.Id);
                usuario.DatosEmpleado.SupervisorSeleccionado = usuario.DatosEmpleado.Supervisor.Count() == 1 ? Convert.ToInt32(usuario.DatosEmpleado.Supervisor.FirstOrDefault().Value) : usuario.DatosEmpleado.SupervisorSeleccionado;
                usuario.DatosEmpleado.Sucursal = UtilRepositorio.obtenSelect_Sucursal();
                usuario.DatosEmpleado.SucursalSeleccionada = usuario.DatosEmpleado.Sucursal.Count() == 1 ? Convert.ToInt32(usuario.DatosEmpleado.Sucursal.FirstOrDefault().Value) : usuario.DatosEmpleado.SucursalSeleccionada;
            }
        }



        [NonAction]
        public void cargaSelectEmpleado(Empleado empleado)
        {

            empleado = empleado == null ? new Empleado() : empleado;

            empleado.TipoDocumento = UtilRepositorio.obtenSelect_TipoDocumentoIdentidad();
            empleado.TipoDocumentoSeleccionado = empleado.TipoDocumento.Count() == 1 ? empleado.TipoDocumento.FirstOrDefault().Value : empleado.TipoDocumentoSeleccionado;
            empleado.DispositivosDisponibles = UtilRepositorio.obtenSelect_DispositivosPorUsuarioPadre(empleado.CodigoUsuarioPadre);
            empleado.RolesDisponibles = UtilRepositorio.obtenSelect_RolesEmpleado(empleado.Id, empleado.CargoSeleccionado); 
            empleado.UnidadAdministrativa = UtilRepositorio.obtenSelect_UnidadAdministrativa();
            empleado.UnidadAdministrativaSeleccionada = empleado.UnidadAdministrativa.Count() == 1 ? Convert.ToInt32(empleado.UnidadAdministrativa.FirstOrDefault().Value) : empleado.UnidadAdministrativaSeleccionada;
            empleado.Cargo = UtilRepositorio.obtenSelect_Cargo(empleado.UnidadAdministrativaSeleccionada);
            empleado.CargoSeleccionado = empleado.Cargo.Count() == 1 ? Convert.ToInt32(empleado.Cargo.FirstOrDefault().Value) : empleado.CargoSeleccionado;
            empleado.Supervisor = UtilRepositorio.obtenSelect_Supervisor(empleado.CargoSeleccionado, empleado.Id);
            empleado.SupervisorSeleccionado = empleado.Supervisor.Count() == 1 ? Convert.ToInt32(empleado.Supervisor.FirstOrDefault().Value) : empleado.SupervisorSeleccionado;
            empleado.Sucursal = UtilRepositorio.obtenSelect_Sucursal();
            empleado.SucursalSeleccionada = empleado.Sucursal.Count() == 1 ? Convert.ToInt32(empleado.Sucursal.FirstOrDefault().Value) : empleado.SucursalSeleccionada;
            
        }

        [NonAction]
        public void cargaSelectEmpleadoBaja(EmpleadoBaja empleado) {
            empleado = empleado == null ? new EmpleadoBaja() : empleado;

            empleado.SupervisorReemplazo = UtilRepositorio.obtenSelect_SupervisorReemplazo(empleado.Id, empleado.CodigoCargo);
            empleado.SupervisorReemplazoSeleccionado = empleado.SupervisorReemplazo.Count() == 1 ? Convert.ToInt32(empleado.SupervisorReemplazo.FirstOrDefault().Value) :empleado.SupervisorReemplazoSeleccionado;
            
        }

        [NonAction]
        public void cargaSelectSocioComercial(SocioComercial socioComercial) {

            socioComercial = socioComercial == null ? new SocioComercial() : socioComercial;

            socioComercial.TipoDocumento = UtilRepositorio.obtenSelect_TipoDocumentoIdentidadPersonaJuridica();
            socioComercial.TipoDocumentoSeleccionado = socioComercial.TipoDocumento.Count() == 1 ? socioComercial.TipoDocumento.FirstOrDefault().Value : socioComercial.TipoDocumentoSeleccionado;

            socioComercial.SegmentoNegocio = UtilRepositorio.obtenSelect_SegmentoNegocio();
            socioComercial.SegmentoNegocioSeleccionado = socioComercial.SegmentoNegocio.Count() == 1 ? Convert.ToInt32(socioComercial.SegmentoNegocio.FirstOrDefault().Value) : socioComercial.SegmentoNegocioSeleccionado;

            socioComercial.Distribuidor = UtilRepositorio.obtenSelect_Distribuidor();
            socioComercial.DistribuidorSeleccionado = socioComercial.Distribuidor.Count() == 1 ? Convert.ToInt32(socioComercial.Distribuidor.FirstOrDefault().Value) : socioComercial.DistribuidorSeleccionado;

            socioComercial.Pais = UtilRepositorio.obtenSelect_Pais();
            socioComercial.PaisSeleccionado = socioComercial.Pais.Count() == 1 ? Convert.ToInt32(socioComercial.Pais.FirstOrDefault().Value) : socioComercial.PaisSeleccionado;

            socioComercial.Provincia = UtilRepositorio.obtenSelect_Provincia(socioComercial.PaisSeleccionado);
            socioComercial.ProvinciaSeleccionada = socioComercial.Provincia.Count() == 1 ? Convert.ToInt32(socioComercial.Provincia.FirstOrDefault().Value) : socioComercial.ProvinciaSeleccionada;

            socioComercial.Distrito = UtilRepositorio.obtenSelect_Distrito(socioComercial.PaisSeleccionado, socioComercial.ProvinciaSeleccionada);
            socioComercial.DistritoSeleccionado = socioComercial.Distrito.Count() == 1 ? Convert.ToInt32(socioComercial.Distrito.FirstOrDefault().Value) : socioComercial.DistritoSeleccionado;

            socioComercial.ServiciosAppDisponibles = UtilRepositorio.obtenSelect_ServiciosAppMovil(socioComercial.SegmentoNegocioSeleccionado);

            socioComercial.Corregimiento = UtilRepositorio.obtenSelect_Corregimiento(socioComercial.PaisSeleccionado, socioComercial.ProvinciaSeleccionada, socioComercial.DistritoSeleccionado);

            socioComercial.DispositivosDisponibles = UtilRepositorio.obtenSelect_DispositivosPorDistribuidor(socioComercial.DistribuidorSeleccionado);

        }
        

        [NonAction]
        public void cargaSelectCargo(Cargo cargo){
            cargo = cargo == null ? new Cargo() : cargo;

            cargo.UnidadAdministrativa = UtilRepositorio.obtenSelect_UnidadAdministrativa();
            cargo.UnidadAdministrativaSeleccionada = cargo.UnidadAdministrativa.Count() == 1 ? Convert.ToInt32(cargo.UnidadAdministrativa.FirstOrDefault().Value) : cargo.UnidadAdministrativaSeleccionada;
            cargo.CargoPadre = UtilRepositorio.obtenSelect_CargoPadre(cargo.Id);
            cargo.RolesDisponibles = UtilRepositorio.obtenSelect_RolesPorCargo(cargo.Id);
        }

    }
}