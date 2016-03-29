using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IntranetWeb.Controllers
{
    [PreparaLog]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidacionController : Controller
    {
        // GET: Validacion
        private Log4NetLogger log;
       
        public ValidacionController() {
            log = new Log4NetLogger();
        }

        private ValidacionRepositorio validacionRepo = new  ValidacionRepositorio();

        /// <summary>
        /// Verifica si el nombre de usuario está disponible
        /// </summary>
        /// <param name="NombreUsuario"></param>
        /// <returns></returns>
        [Autoriza(
                  Core.Constante.Aplicacion.AdministrarUsuario
                 ,Core.Constante.Aplicacion.AdministrarUsuarioAdmin
                 ,Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult verificaNombreUsuarioDisponible(string NombreUsuario, int? Id) {

            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);
            try
            {
                Id = Id == null ? 0 : Id;

                if (!validacionRepo.usuarioExiste((int)Id,NombreUsuario))
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.NombreUsuarioExistente;


            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }
            
            return result;
        }





        /// <summary>
        /// Verifica si la dirección de correo de usuario está disponible
        /// </summary>
        /// <param name="CorreoUsuario"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.ConfiguracionUsuario)]
        public JsonResult verificaCorreoPerfilUsuarioDisponible( [Bind(Prefix = "Usuario.Email")]string Email
                                                                ,[Bind(Prefix = "Usuario.Id")]int    Id
                                                            ){  
            return verificaCorreoUsuarioDisponible(Email, Id);
        }


        /// <summary>
        /// Verifica si la dirección de correo de usuario está disponible
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Autoriza(   Core.Constante.Aplicacion.AdministrarUsuarioAdmin
                    ,Core.Constante.Aplicacion.AdministrarUsuario
                    ,Core.Constante.Aplicacion.AdministrarEmpleado
                    ,Core.Constante.Aplicacion.EditarCliente
            )]
        public JsonResult verificaCorreoUsuarioGeneralDisponible(string Email
                                                               , int? Id
                                                           )
        {
            Id = Id == null ? 0 : Id;
            return verificaCorreoUsuarioDisponible(Email, (int)Id);
        }

        /// <summary>
        /// Vefirica si la dirección de correo está disponible
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [NonAction]
        private JsonResult verificaCorreoUsuarioDisponible(string Email
                                                         ,int Id
                                                           )
        {

            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);
            try
            {

                if (!validacionRepo.correoExiste(Id, Email))
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.CorreoUsuarioExistente;

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }



        /// <summary>
        /// Verifica si el documento de identificación se encuentra en uso por otro usuario
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin
                , Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult verificaNumeroDocumentoUsuarioNoUsado( string TipoDocumentoSeleccionado
                                                    ,string NumeroDocumento  ) {
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try
            {
                if (!validacionRepo.numeroDocumentoUsuarioEnUso(TipoDocumentoSeleccionado
                                                  ,NumeroDocumento))
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.NumeroDeDocumentoEnUso;

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }

       /// <summary>
       /// Valida si el vin no pertenece a otro vehículo
       /// </summary>
       /// <param name="Vin"></param>
       /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public JsonResult verificaVINNoUsado(string Vin) {
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try
            {
                if (!validacionRepo.numeroVINEnUso(Vin))
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.VinEnUso;

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }



        /// <summary>
        /// Verifica si el documento de identificación se encuentra en uso por otro socio comercial
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarSocioComercial)]
        public JsonResult verificaNumeroDocumentoSocioComercialNoUsado(string TipoDocumentoSeleccionado
                                                                        , string NumeroDocumento)
        {
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try
            {
                
                if (!validacionRepo.numeroDocumentoSocioComercialEnUso(TipoDocumentoSeleccionado
                                                  , NumeroDocumento))
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.NumeroDeDocumentoSocioComercialEnUso;
                    
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }


        /// <summary>
        /// Verifica si el documento de identificación se encuentra en uso por otro usuario interno
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuario)]
        public JsonResult verificaNumeroDocumentoUsuarioInternoNoUsado(string TipoDocumentoSeleccionado
                                                                        , string NumeroDocumento)
        {
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try
            {

                int cdUsuarioPadre = Core.Utils.UtilHelper.obtenUsuarioLogueado();
                if (!validacionRepo.numeroDocumentoUsuarioInternoEnUso(
                                                    cdUsuarioPadre
                                                  , TipoDocumentoSeleccionado
                                                  , NumeroDocumento))
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.NumeroDeDocumentoEnUso;

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }

        /// <summary>
        /// Verifica si el documento de identificación se encuentra en uso por otro cliente
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.EditarCliente)]
        public JsonResult verificaNumeroDocumentoClienteNoUsado(string TipoDocumentoSeleccionado
                                                                        , string Cedula)
        {
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try
            {

                if (!validacionRepo.numeroDocumentoClienteEnUso(    TipoDocumentoSeleccionado
                                                                ,   Cedula))
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.NumeroDeDocumentoClienteEnUso;

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }




        /// <summary>
        /// Verifica si el documento de identificación se encuentra en uso por otro empleado
        /// </summary>
        /// <param name="TipoDocumentoSeleccionado"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult verificaNumeroDocumentoEmpleadoNoUsado(string TipoDocumentoSeleccionado
                                                                        , string NumeroDocumento)
        {
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try
            {

                if (!validacionRepo.numeroDocumentoEmpleadoEnUso(TipoDocumentoSeleccionado
                                                  , NumeroDocumento))
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.NumeroDeDocumentoEmpleadoEnUso;

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }
        

        /// <summary>
        /// Verifica si al empleado no se le ha dado de baja
        /// </summary>
        /// <param name="CodigoUsuario"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin
                , Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult verificaEmpleadoActivo(int CodigoUsuario){
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try
            {
                if (!validacionRepo.usuarioEnBaja(CodigoUsuario))
                    result.Data = true;
                else
                    result.Data = IntranetWeb.Core.Constante.Mensaje.Error.UsuarioBajaNoActualizable;

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }

        /// <summary>
        /// Vefirica si un empleado que está cambiando de cargo no tiene subornidados en el cargo anterior
        /// </summary>
        /// <param name="CodigoEmpleado">Código del empleado</param>
        /// <param name="CodigoCargo">Código del cargo</param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin
                , Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult verificaNoSubordinadosCambioCargoEstatus(     int CodigoEmpleado
                                                                ,int CodigoCargo
                                                                ,bool indicadorActivo ) {

            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try{
                AdministradorRepositorio adminRepo = new AdministradorRepositorio();

                EMPLEADO emp = adminRepo.obten_EMPLEADO_ById(CodigoEmpleado);

                if (emp == null)
                    result.Data = true;
                else {
                        IEnumerable<EMPLEADO> subordinados = adminRepo.obtenSubordinadosEmpleado(CodigoEmpleado);
                        string subordinadosStr = string.Join(", ", subordinados.Select(x => x.USUARIO.DE_NOMBRE_APELLIDO));

                    if (subordinados.Count() == 0)
                        result.Data = true;
                    else{

                        if (emp.CD_CARGO != CodigoCargo)
                            result.Data = String.Format(Resources.ErrorResource.UsuarioNuevoCargoNoActualizable, subordinadosStr);
                        else {
                            if (!indicadorActivo)
                                result.Data = String.Format(Resources.ErrorResource.UsuarioActivoNoActualizable, subordinadosStr);
                            else
                                result.Data = true;
                        }
                    }                        
                 }                                
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
            
        }


        /// <summary>
        /// Verifica si un cargo tiene cargo padre
        /// </summary>
        /// <param name="CodigoCargo"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarUsuarioAdmin
            , Core.Constante.Aplicacion.AdministrarEmpleado)]
        public JsonResult verificaSupervisorObligatorio(int CodigoCargo)
        {
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try
            {
                AdministradorRepositorio adminRepo = new AdministradorRepositorio();
                CARGO cargo = adminRepo.obten_CARGO_ById(CodigoCargo);

                if (cargo == null || cargo.CD_CARGO_PADRE == null)
                    result.Data = true;
                else
                    result.Data = Resources.ErrorResource.SupervisorObligatorio;

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }


        /// <summary>
        /// Valida si el cargo padre de un cargo puede ser modificado
        /// </summary>
        /// <param name="CodigoCargo"></param>
        /// <param name="CargoPagre"></param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.AdministrarCargo)]
        public JsonResult cargoPadreActualizables(int CodigoCargo
                                                 ,int? CargoPadre ) {
            JsonResult result = Json(false, JsonRequestBehavior.AllowGet);

            try{

                AdministradorRepositorio adminRepo = new AdministradorRepositorio();
                CARGO cargo = adminRepo.obten_CARGO_ById(CodigoCargo);

                if (cargo == null || CargoPadre == null || cargo.CD_CARGO_PADRE == CargoPadre)
                    result.Data = true;
                else {
                    //Se valida si en el cargo hay personas
                    IEnumerable<EMPLEADO> listaEmpleado = adminRepo.obten_EMPLEADO_ByCargo(cargo.CD_CARGO);

                    if (listaEmpleado.Count() == 0)
                        result.Data = true;
                    else {

                        CARGO cargoPadreRepo=null;
                        //Se verifica si en el nuevo cargo padre hay un sólo empleado
                       listaEmpleado = adminRepo.obten_EMPLEADO_ByCargo((int)CargoPadre);
                        while (listaEmpleado.Count() == 0)
                        {
                            //Se busca el cargo supervisor 
                            cargoPadreRepo = adminRepo.obten_CARGO_ById((int)CargoPadre);
                            if (cargoPadreRepo == null|| cargoPadreRepo.CD_CARGO_PADRE==null) break; else {
                                CargoPadre = cargoPadreRepo.CD_CARGO_PADRE!= null?cargoPadreRepo.CD_CARGO_PADRE:0;
                                listaEmpleado = adminRepo.obten_EMPLEADO_ByCargo((int)CargoPadre);
                            }                            
                        }

                        if (listaEmpleado.Count() == 1||(cargoPadreRepo!=null && cargoPadreRepo.CD_CARGO_PADRE == null))
                            result.Data = true;
                        else
                            result.Data = Resources.ErrorResource.CargoPadreNoActualizableReasignarSupervisor;

                    }
                }                    

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result.Data = Resources.ErrorResource.Error100;
            }

            return result;
        }
    }
}