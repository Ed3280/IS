using IntranetWeb.Core.Atributos;
using IntranetWeb.Core.Exception;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.ViewModel.Kilometraje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace IntranetWeb.Controllers
{

    [PreparaLog]
    public class KilometrajeController : Controller
    {
        #region Kilometraje
        private Log4NetLogger log;
        private KilometrajeRepositorio kilometrajeRepo = new KilometrajeRepositorio();

        public KilometrajeController() {
            log = new Log4NetLogger();
        }
        /// <summary>
        /// Permite ver el Kilometraje de los vehículos andando
        /// </summary>
        /// <returns></returns>
        ///
        [Autoriza(Core.Constante.Aplicacion.VerKilometraje, Core.Constante.Aplicacion.ConsultarKilometraje)]
        public ActionResult VerKilometrajeTotal(Boolean? IndicadorNoEdicion)
        {
            KilometrajeTotal kt = new KilometrajeTotal();
            try {
                kt.UsuarioId = UtilRepositorio.obtenSelect_Distribuidor();
                kt.IndicadorNoEdicion = IndicadorNoEdicion;
            }
            catch (Exception exc) {
                log.Error(Resources.ErrorResource.Error100, exc);
            }
                return View(kt);
        }


        /// <summary>
        /// Obtiene por json el kilometraje total de los vehículos
        /// </summary>
        /// <param name="IndicadorNoEdicion">Indicador de si puede o no editar el kilometraje inicial</param>
        /// <returns></returns>
        [Autoriza(Core.Constante.Aplicacion.VerKilometraje, Core.Constante.Aplicacion.ConsultarKilometraje)]
        public JsonResult getVehiculosKilometraje(Boolean? IndicadorNoEdicion)
        {
            JsonResult result;

            try{
                var myData = getVehiculosKilometrajeList();

                if (IndicadorNoEdicion != null && IndicadorNoEdicion == true)
                    myData.ToList().ForEach(x => x.EditarRegistro = "");

                result = Core.Utils.UtilJson.Exito(myData);

            }
            catch (Exception exc) {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return result;
        }
        
        //Se muestra la vista parcial
        [HttpGet]
        [Autoriza(Core.Constante.Aplicacion.VerKilometraje)]
        public ActionResult _editarKilometraje(int userId,
                                                    int deviceId)
        {
            JsonResult result;

            try {
                Vehiculo vh = new Vehiculo();
                
                //Se busca la el vehículo
                vh = kilometrajeRepo.obten_Vehiculo_KILOMETRAJE_TOTALByID(userId
                                                                   , deviceId);
                return PartialView(vh);
            }
            catch (Exception exc)
            {
                result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }
            return result;
        }

        /// <summary>
        /// Guarda el kilometraje inicial
        /// </summary>
        /// <param name="vehiculo"></param>
        /// <returns></returns>
        [HttpPost]
        [Autoriza(Core.Constante.Aplicacion.VerKilometraje)]
        public ActionResult actualizaKilometrajeInicial(Vehiculo vehiculo)
        {
            JsonResult resultado;

            try{
                var kilometraje = kilometrajeRepo.obten_KILOMETRAJE_TOTALById(vehiculo.IdUsuario
                                                                        , vehiculo.IdVehiculo);

                if (kilometraje == null)
                    throw new BussinessException(IntranetWeb.Core.Constante.Mensaje.Error.VehiculoNoExiste);

                kilometraje.MontoKilometrajeInicial = vehiculo.DistanciaInicial;
                kilometrajeRepo.actualiza_KILOMETRAJE_TOTAL(kilometraje);
                resultado = Core.Utils.UtilJson.Exito(Core.Constante.Mensaje.Exito.KilometrajeInicialActualizado);
            }
            catch(BussinessException exc){                        
                resultado = Core.Utils.UtilJson.Error(exc.Message);
                        
            }
            catch (Exception exc){
                resultado = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100);
                log.Error(Resources.ErrorResource.Error100, exc);
            }

            return resultado;
        }
        #endregion
        [NonAction]
        public IEnumerable<Vehiculo> getVehiculosKilometrajeList()
        {
            IEnumerable<Vehiculo> listaVhiculos = new List<Vehiculo>();

            //Se buscan los automóviles
            var dispArr = kilometrajeRepo.obtenListado_KILOMETRAJE_TOTAL();
            listaVhiculos = kilometrajeRepo.obtenDisposotivoKilometrajeInicial(dispArr);
            listaVhiculos.ToList().ForEach(x => x.DistanciaRecorrida = Math.Round(x.DistanciaRecorrida, MidpointRounding.AwayFromZero));
            return listaVhiculos;
        }
    }
}
