using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Servicio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.Controllers
{
    /// <summary>
    /// Controlador para obtener el listado de select via ajax
    /// </summary>
    [PreparaLog]
    public class SelectController : Controller
    {
        private Log4NetLogger log;
        public SelectController(){
            log = new Log4NetLogger();

        }
        /// <summary>
        /// Obtiene select de provincia
        /// </summary>
        /// <param name="pais"></param>
        /// <returns></returns>
        public JsonResult getProvincias(int? pais){
            JsonResult result;
            try{
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_Provincia(pais));                
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }


        /// <summary>
        /// Obtiene select de modelos
        /// </summary>
        /// <param name="marca"></param>
        /// <returns></returns>
        public JsonResult getModelos(int? marca)
        {
            JsonResult result;
            try
            {
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_ModeloVehiculo(marca));
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }


        /// <summary>
        /// Obtiene select de años de vehículos dada la marca y el modelo
        /// </summary>
        /// <param name="marca"></param>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public JsonResult getAnos(int? modelo)
        {
            JsonResult result;
            try{
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_AnoVehiculo(modelo));
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }

        /// <summary>
        /// Retorna el listado de Municipios
        /// </summary>
        /// <param name="pais"></param>
        /// <param name="provincia"></param>
        /// <returns></returns>
        public JsonResult getDistritos(int? pais, int? provincia){
            JsonResult result;
            try{
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_Distrito(pais, provincia));               
            }
            catch (Exception exc) {

                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);               
            }
            return result; 
        }

        /// <summary>
        /// Retorna el listado de corregimientos
        /// </summary>
        /// <param name="pais"></param>
        /// <param name="provincia"></param>
        /// <param name="municipio"></param>
        /// <returns></returns>
        public JsonResult getCorregimientos(int? pais, int? provincia, int? distrito){
            JsonResult result;
            try { 
               result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_Corregimiento(pais, provincia, distrito));
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                
            }
            return result;
        }



        /// <summary>
        /// Obtiene el listado de dispositivos via ajax
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public JsonResult getDispositivos(int? idCliente){
            JsonResult result;
            try {
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_DispositivosPorCliente(idCliente));
            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);               
            }
            return result;
        }


        /// <summary>
        /// Obtiene select de mensajes del operador al cliente
        /// </summary>
        /// <param name="pais"></param>
        /// <returns></returns>
        public JsonResult getRespuestaOperadorGCM(int? tpAtencion){
            JsonResult result;
            try
            {
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_RespuestaOperadorGCM(tpAtencion));

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);

            }
            return result;
        }




        /// <summary>
        /// Obtiene el select dualbox para los roles del empleado
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <param name="cdCargo"></param>
        /// <returns></returns>
        public JsonResult getRolesEmpleado(int? cdUsuario, int? cdCargo) {

            JsonResult result;
            try
            {
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_RolesEmpleado(cdUsuario, cdCargo));
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;


        }


        /// <summary>
        /// Obtiene select de los cargos
        /// </summary>
        /// <param name="cdUnidadAdministrativa"> Unidad Administrativa</param>
        /// <returns></returns>
        public JsonResult getCargo(int? cdUnidadAdministrativa){
            JsonResult result;
            try
            {
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_Cargo(cdUnidadAdministrativa));

            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);

            }
            return result;
        }

        /// <summary>
        /// Obtiene select de los supervisores 
        /// </summary>
        /// <param name="cdCargo">Código del cargo</param>
        /// <param name="cdUsuarioModif">Usuario al que se le éstán modificando los datos </param>
        /// <returns></returns>
        public JsonResult getSupervisor(int? cdCargo
                                        ,int? cdUsuarioModif)
        {
            JsonResult result;

            try {
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_Supervisor(cdCargo, cdUsuarioModif));

            }
            catch (Exception exc){
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);

            }
            return result;
        }


        /// <summary>
        /// Obtiene select de servicios de aplicación móvil
        /// </summary>
        /// <param name="CodigoSegmentoNegocio"></param>
        /// <returns></returns>
        public JsonResult getServiciosAppMovil(int? CodigoSegmentoNegocio)
        {
            JsonResult result;
            try
            {
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_ServiciosAppMovil(CodigoSegmentoNegocio));
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }


        /// <summary>
        /// Obtiene select de los dispositivos 
        /// </summary>
        /// <param name="IdDistribuidor"></param>
        /// <returns></returns>
        public JsonResult getDispositivosPorDistribuidor(int? IdDistribuidor)
        {
            JsonResult result;
            try
            {
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_DispositivosPorDistribuidor(IdDistribuidor));
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }

        /// <summary>
        /// Obtiene select de los socios comerciales
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public JsonResult getSocioComercialPorClienteId(int? IdCliente)
        {
            JsonResult result;
            try{
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_SocioComercial_ByUsuarioId(IdCliente));                 
            }
            catch (Exception exc)
            {
                log.Error(Resources.ErrorResource.Error100, exc);
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
            }
            return result;
        }


        /// <summary>
        /// Obtiene el listado de dispositivos asignables 
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        public JsonResult getSugerenciaVinAsignables(int IdCliente) {

            JsonResult result;
            try{
                result = Core.Utils.UtilJson.Exito(UtilRepositorio.obtenSelect_VinAsignables(IdCliente));
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