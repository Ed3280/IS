using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Exception;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Cliente;
using IntranetWeb.ViewModel.Interface;
using IntranetWeb.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    public class ClienteController : Controller
    {
        private Log4NetLogger log;
        private ClienteRepositorio clienteRepo;
        private ValidacionRepositorio validacionRepo;
        private AuthRepositorio authRepo;

        public ClienteController() {
           log= new Log4NetLogger();
           clienteRepo = new ClienteRepositorio();
           validacionRepo = new ValidacionRepositorio();
            authRepo = new AuthRepositorio();
        }
        

        //Permite asociar un vehículo
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public ActionResult _getAsociarVehiculo(int IdCliente)
        {
            Vehiculo veh = new Vehiculo();
            veh.IdCliente = IdCliente;
            cargaSelectVehiculo(veh);
            return View("Crear/_getAsociarVehiculo", veh);
        }


        /// <summary>
        /// Permite asociar un dispositivo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public ActionResult _getAsociarDispositivo(int IdCliente) {
            Dispositivo dis = new Dispositivo();
            dis.IdCliente = IdCliente;
            cargaSelectDispositivo(dis);
            return View("Crear/_getAsociarDispositivo", dis);
        }

        /// <summary>
        /// Retorna las aplicaciones del socio comercial
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public ActionResult _getAplicacionesCliente(int Id)
        {
            JsonResult result;

            try
            {
                IModeloAplicacion aplicaciones = clienteRepo.obtenAplicacionesCliente_ById(Id);
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
        /// Principal
        /// </summary>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.EditarCliente )]
        public ActionResult AdministrarClientes()
        {
            return View();
        }

        [Autoriza(Core.Constante.Aplicacion.ConsultarCliente)]
        public ActionResult ConsultarClientes() {
            return View("AdministrarClientes");
        }

        /// <summary>
        /// Vista parcial para la edición de clientes
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente
                , Core.Constante.Aplicacion.ConsultarCliente)]
        public ActionResult _editarCliente(int Id) {
            Cliente cliente = new Cliente();
            JsonResult result ; 
            try {
                cliente = clienteRepo.obten_Cliente_ById(Id);
                cargaSelectCliente(cliente);
                return PartialView("Editar/_editarCliente",cliente);               
            }
            catch (Exception exc) {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return result; 
        }

        /// <summary>
        /// Vista parcial para la edición de los datos adicionales del cliente
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public ActionResult _editarDatosAdicionalesCliente(int Id) {
            Cliente cliente = new Cliente();
            JsonResult result;
            try
            {
                cliente = clienteRepo.obten_Cliente_ById(Id);
                IModeloAplicacion aplicaciones = clienteRepo.obtenAplicacionesCliente_ById(Id);
                cliente.Aplicaciones = aplicaciones.Aplicaciones;
                cargaSelectCliente(cliente);
                return PartialView("Editar/_editarDatosAdicionalesCliente", cliente);
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return result;            
        }

        
        /// <summary>
        /// Retorna la interfaz para crear un cliente
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public ActionResult _getCrearCliente() {
            Cliente cliente = new Cliente();
            JsonResult result;

            try{
                cargaSelectCliente(cliente);
                return PartialView("Crear/_getCrearCliente", cliente);
            }
            catch (Exception exc) {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }

            return result;
        }

        
        /// <summary>
        /// Obtiene las vista parcial para la actualización de los clientes
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public ActionResult _getDireccion(Cliente cliente){

            return View("Editar/Shared/_getDireccion", cliente);
        }

        /// <summary>
        /// Obtiene los datos básicos del cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public ActionResult _getDatosBasicos(
                                             Cliente cliente)
        {

            return View("Editar/Shared/_getDatosBasicos", cliente);
        }


        /// <summary>
        /// Obtiene los dispositivos asociados al cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public ActionResult _getDispositivos(
                                             Cliente cliente)
        {

            return View("Editar/Shared/_getDispositivos", cliente);
        }

        /// <summary>
        /// Permite visualizar los vehículos asociados a un Cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public ActionResult _getVehiculos( Cliente cliente){

            return View("Editar/Shared/_getVehiculos", cliente);

        }


        /// <summary>
        /// Actualizar la información del cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public ActionResult actualizarCliente(Cliente cliente) {

            String resultado = "";
            try{

                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                //Se actualiza la información del cliente
                if (clienteRepo.actualiza_Cliente(ref cliente)){

                    resultado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeExito(IntranetWeb.Core.Constante.Mensaje.Exito.ClienteActualizado);
                    log.Info(String.Format(Core.Constante.Mensaje.Logging.ClienteActualizado,cliente.Id));
                }
                else
                    resultado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(IntranetWeb.Core.Constante.Mensaje.Error.FalloActualizarCliente);
                    
            }
            catch (BussinessException exc){
                resultado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(exc.Message);
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
                resultado = IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(Resources.ErrorResource.Error100);
            }

            return Content(resultado, "text/thtml");
        }


        /// <summary>
        /// Editar la información adicional del cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public ActionResult editarDatosAdicionales(Cliente cliente)
        {

            JsonResult resultado;
            string mensajeAdvertencia = "";
            try
            {

                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                //Se actualiza la información del cliente
                if (clienteRepo.actualiza_DatosAdicionalesCliente(ref cliente)){
                    
                    resultado = Core.Utils.UtilJson.Exito(IntranetWeb.Core.Constante.Mensaje.Exito.ClienteActualizado, new { ID_RESPUESTA = cliente.Id, mensajeAdvertencia = mensajeAdvertencia });
                    log.Info(String.Format(Core.Constante.Mensaje.Logging.ClienteActualizado, cliente.Id));
                }
                else
                    resultado = Core.Utils.UtilJson.Error(IntranetWeb.Core.Constante.Mensaje.Error.FalloActualizarCliente);

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
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public JsonResult enviarCorreoRegistro(int Id){

            JsonResult resultado;
            
            try{
                String asunto, mensaje;

                //Se busca al cliente
                Cliente cliente = clienteRepo.obten_Cliente_ById(Id);

                //Se buscan los socios comerciales asociados al cliente
                List<int> sociosRelacionados = clienteRepo.obten_IdDistribuidoresCliente_ByClienteId(Id);
                
                MailTemplate.PrepararTemplateRegistro(Id
                                               , sociosRelacionados
                                               , out asunto
                                               , out mensaje);

                if (SmtpHelper.Send(asunto, mensaje, cliente.Email)){
                    
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
        /// Genera excel de los clientes registrados
        /// </summary>
        /// <param name="gestionTicket"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public FileContentResult GererarExcelClientes()
        {
            byte[] salida = null;
            try{
               
               salida = clienteRepo.generarExcelListaClientes();

                if (salida == null)
                    throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.FalloGenerarArchivoExcel);
                
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


            return new FileContentResult(salida, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = DateTime.Now.ToString("yyyyMMdd") + "_"+Resources.TituloPaginaResource.Cliente+".xlsx" };
        }




        /// <summary>
        /// Consulta los datos de usuario 
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="Cedula"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public JsonResult consultarDatosUsuario(string TipoDocumentoSeleccionado
                                               ,string Cedula) {

            JsonResult result;

            try{
                //Su busca el cliente en la tabla de clientes 
                Cliente cliente = new Cliente();
                if (!validacionRepo.numeroDocumentoClienteEnUso(TipoDocumentoSeleccionado
                                                                , Cedula))
                {
                    //Se busca la información del usuario
                    cliente = clienteRepo.obten_ClienteNoRegistrado_By_NumeroDocumento(TipoDocumentoSeleccionado
                                                                                     , Cedula);

                    cliente = cliente == null ? new Cliente() : cliente;
                }

                result = IntranetWeb.Core.Utils.UtilJson.Exito(cliente);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;
        }

        /// <summary>
        /// Se busca la información del dispostivo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public JsonResult consultarDatosDispositivos(int Id) {

            JsonResult result;

            try{
                Dispositivo dispositivo = new Dispositivo();

                //Se busca la información del dispositivo
                dispositivo = clienteRepo.obten_DispostivoSugerencia_ById(Id);
                    
                result = IntranetWeb.Core.Utils.UtilJson.Exito(dispositivo);
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }

            return result;

        }


        /// <summary>
        /// Listado de Clientes
        /// </summary>
        /// <returns></returns>
        public JsonResult getClientes(DTParameters parametro)
        {

            JsonResult result; 
            try{

                var dtsource = clienteRepo.obtenClientes();

                List<Cliente> data = new ResultSet().GetResult(parametro.Search.Value, parametro.SortOrder, parametro.Start, parametro.Length, dtsource.ToList(), new List<string>());
                int count = new ResultSet().Count(parametro.Search.Value, dtsource.ToList(), new List<string>());

                DTResult<Cliente> resultData = new DTResult<Cliente>
                {
                    draw = parametro.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                result = Core.Utils.UtilJson.ExitoDataTableOnServer<Cliente>(resultData);
            }
            catch (Exception exc) {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }

            return result;
        }

        
        /// <summary>
        /// Permite la creación de un nuevo cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public JsonResult crearCliente(Cliente cliente) {
            JsonResult resultado;
            string mensajeAdvertencia = "";
            
            try
            {
                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                
                if (clienteRepo.guarda_Cliente(ref cliente) && cliente.Id > 0)
                {
                   log.Info(String.Format(Resources.LoggingResource.ClienteCreado, cliente.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.ClienteCreado, cliente.Nombre+" "+cliente.Apellido), new { ID_RESPUESTA = cliente.Id, NOMBRE_CLIENTE = cliente.NombreApellidoRazonSocial, IN_ABRIR_TAB =1, IN_ENVIAR_CORREO=1, mensajeAdvertencia = mensajeAdvertencia});
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
        /// Permite asociar un vehículo a un cliente
        /// </summary>
        /// <param name="vehiculo"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public JsonResult crearVehiculoCliente(Vehiculo vehiculo) {
            JsonResult resultado;
            string mensajeAdvertencia = "";

            try{

                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                if (clienteRepo.guarda_Vehiculo(ref vehiculo) )
                {
                    log.Info(String.Format(Resources.LoggingResource.VehiculoCreado, vehiculo.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.VehiculoCreado, vehiculo.Vin ), new { ID_RESPUESTA = vehiculo.IdCliente, NOMBRE_CLIENTE = clienteRepo.obten_Cliente_ById(vehiculo.IdCliente).NombreApellidoRazonSocial, IN_ABRIR_TAB = 1, mensajeAdvertencia = mensajeAdvertencia });
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
        /// Permite asociar un nuevo dispositivo al cliente
        /// </summary>
        /// <param name="dispositivo"></param>
        /// <returns></returns>
     
        public JsonResult crearDispositivoCliente(Dispositivo dispositivo) {
            JsonResult resultado;
            string mensajeAdvertencia = "";

            try{

                if (!ModelState.IsValid)
                    throw new BussinessException(Core.Utils.HtmlHelper.getFormErrorsMessage(ModelState));

                if (clienteRepo.guarda_Dispositivo(ref dispositivo)){
                    log.Info(String.Format(Resources.LoggingResource.DispositivoVinculado, dispositivo.Id));
                    resultado = Core.Utils.UtilJson.Exito(String.Format(Resources.ExitoResource.DispositivoVinculado, dispositivo.Imei), new { ID_RESPUESTA = dispositivo.IdCliente, NOMBRE_CLIENTE = clienteRepo.obten_Cliente_ById(dispositivo.IdCliente).NombreApellidoRazonSocial, IN_ABRIR_TAB = 1, mensajeAdvertencia = mensajeAdvertencia });
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



        [NonAction]
        private void cargaSelectCliente(Cliente clien) {
            clien = clien == null ? new Cliente() : clien;

            clien.TipoDocumento                 = UtilRepositorio.obtenSelect_TipoDocumentoIdentidadPersonaNatural();
            clien.TipoDocumentoSeleccionado     = clien.TipoDocumento.Count() == 1 ? clien.TipoDocumento.FirstOrDefault().Value : clien.TipoDocumentoSeleccionado;

            clien.CompaniaSeguro                = UtilRepositorio.obtenSelect_CompaniaSeguros();
            clien.CompaniaSeguroSeleccionada    = clien.CompaniaSeguro.Count() == 1 ? Convert.ToInt32(clien.CompaniaSeguro.FirstOrDefault().Value) : clien.CompaniaSeguroSeleccionada;
            clien.TipoSangre                    = UtilRepositorio.obtenSelect_TipoSangre();
            clien.SocioComercialDisponibles     = UtilRepositorio.obtenSelect_SocioComercial_ByUsuarioId(clien.Id);
            
            foreach (Vehiculo veh in clien.Vehiculos)
                cargaSelectVehiculo(veh);
            
            
            foreach (Dispositivo disp in clien.Dispositivos){
                disp.IdCliente = clien.Id;
                cargaSelectDispositivo(disp);                
            }
            
            clien.Direccion.Pais = UtilRepositorio.obtenSelect_Pais();
            clien.Direccion.PaisSeleccionado = clien.Direccion.Pais.Count() == 1 ? Convert.ToInt32(clien.Direccion.Pais.FirstOrDefault().Value) : clien.Direccion.PaisSeleccionado;
            clien.Direccion.Provincia = UtilRepositorio.obtenSelect_Provincia(clien.Direccion.PaisSeleccionado);
            clien.Direccion.ProvinciaSeleccionada = clien.Direccion.Provincia.Count() == 1 ? Convert.ToInt32(clien.Direccion.Provincia.FirstOrDefault().Value) : clien.Direccion.ProvinciaSeleccionada;
            clien.Direccion.Municipio = UtilRepositorio.obtenSelect_Distrito(clien.Direccion.PaisSeleccionado, clien.Direccion.ProvinciaSeleccionada);
            clien.Direccion.MunicipioSeleccionado = clien.Direccion.Municipio.Count() == 1 ? Convert.ToInt32(clien.Direccion.Municipio.FirstOrDefault().Value) : clien.Direccion.MunicipioSeleccionado;
            clien.Direccion.Corregimiento = UtilRepositorio.obtenSelect_Corregimiento(clien.Direccion.PaisSeleccionado, clien.Direccion.ProvinciaSeleccionada, clien.Direccion.MunicipioSeleccionado);
            clien.Direccion.CorregimientoSeleccionado = clien.Direccion.Corregimiento.Count() == 1 ? Convert.ToInt32(clien.Direccion.Corregimiento.FirstOrDefault().Value) : clien.Direccion.CorregimientoSeleccionado;
        }


        [NonAction]
        public void cargaSelectVehiculo(Vehiculo veh) {
            veh = veh == null ? veh = new Vehiculo() : veh;

            veh.CompaniaSeguro =  UtilRepositorio.obtenSelect_CompaniaSeguros();
            veh.CompaniaSeguroSeleccionada = veh.CompaniaSeguro.Count() == 1 ? Convert.ToInt32(veh.CompaniaSeguro.FirstOrDefault().Value) : veh.CompaniaSeguroSeleccionada;
            veh.Marca = UtilRepositorio.obtenSelect_MarcaVehiculo();
            veh.Modelo = UtilRepositorio.obtenSelect_ModeloVehiculo(veh.MarcaSeleccionada);
            veh.Ano = UtilRepositorio.obtenSelect_AnoVehiculo(veh.ModeloSeleccionado);
            veh.AnoSeleccionado = veh.Ano.Count() == 1 ? Convert.ToInt32(veh.Ano.FirstOrDefault().Value) : veh.AnoSeleccionado;
            veh.TipoCierre = UtilRepositorio.obtenSelect_TipoCierreVehiculo();
            veh.TipoCierreSeleccionado = veh.TipoCierre.Count() == 1 ? Convert.ToInt32(veh.TipoCierre.FirstOrDefault().Value) : veh.TipoCierreSeleccionado;
            veh.TipoCombustible = UtilRepositorio.obtenSelect_TipoCombustible();
            veh.TipoCombustibleSeleccionado = veh.TipoCombustible.Count() == 1 ? Convert.ToInt32(veh.TipoCombustible.FirstOrDefault().Value) : veh.TipoCombustibleSeleccionado;
            veh.TipoTransmision = UtilRepositorio.obtenSelect_TipoTransmision();
            veh.TipoTransmisionSeleccionada = veh.TipoTransmision.Count() == 1 ? Convert.ToInt32(veh.TipoTransmision.FirstOrDefault().Value) : veh.TipoTransmisionSeleccionada;            
        }

        [NonAction]
        public void cargaSelectDispositivo(Dispositivo dis) {
            dis = dis == null ? dis = new Dispositivo() : dis;
            dis.Vehiculos = UtilRepositorio.obtenSelect_Vehiculos_By_ClienteId(dis.IdCliente);
            dis.Dispositivos = UtilRepositorio.obtenSelect_DispositivosAsignablesCliente(dis.IdCliente);
        }

    }
}