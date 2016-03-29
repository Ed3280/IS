using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Kilometraje;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Respositorios
{
    public class KilometrajeRepositorio
    {


        /// <summary>
        /// Obtiene Listado de Dispositivos con Kilometraje
        /// </summary>
        /// <returns></returns>
        public IQueryable<KILOMETRAJE_TOTAL> obtenListado_KILOMETRAJE_TOTAL()
        {

            using (IntranetSAIEntities dbIntra = new IntranetSAIEntities())
            {
                return (from kt in dbIntra.KILOMETRAJE_TOTAL select kt).ToList().AsQueryable();
            }
        }
        /// <summary>
        /// Obtiene le kilometraje total de vehículos
        /// </summary>
        /// <param name="dispositivos">Listado de dispositivos a filtar</param>
        /// <returns></returns>
        public IList<Vehiculo> obtenDisposotivoKilometrajeInicial(IEnumerable<KILOMETRAJE_TOTAL> dispositivos)
        {
            IList<Vehiculo> result = new  List<Vehiculo>(); 

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                result = (from kt in dispositivos
                        join dev in db.DISPOSITIVO on kt.DeviceID equals dev.ID_DISPOSITIVO
                        join usr in db.DISTRIBUIDOR on kt.UserID equals usr.ID_DISTRIBUIDOR
                        select new Vehiculo
                        {
                            IdUsuario = usr.ID_DISTRIBUIDOR
                            ,
                            NombreUsuario = usr.NM_DISTRIBUIDOR
                            ,
                            IdVehiculo = dev.ID_DISPOSITIVO
                            ,
                            NombreVehiculo = dev.NM_DISPOSITIVO
                            ,
                            DistanciaRecorrida = kt.MontoKilometrajeTotal
                            ,
                            DistanciaInicial = kt.MontoKilometrajeInicial
                           
                        }).ToList();
            }
            
            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() { { "data-user-id", x.IdUsuario.ToString()}
                                                                                                                              , { "data-device-id", x.IdVehiculo.ToString() } }
                                                                                                                              , false, true, false));
            
            return result;
                          
        }


        /// <summary>
        /// Obtiene un vehículo del tipo Vehículo buscado a partir de la tabla kilometraje total
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public Vehiculo obten_Vehiculo_KILOMETRAJE_TOTALByID(int userId
                                                        , int deviceId)
        {
            using (IntranetSAIEntities dbIntra = new IntranetSAIEntities())
            {

                return (from x in dbIntra.KILOMETRAJE_TOTAL
                        where x.UserID == userId
                        && x.DeviceID == deviceId
                        select new Vehiculo
                        {
                            IdUsuario = x.UserID
                            ,
                            IdVehiculo = x.DeviceID
                            ,
                            DistanciaInicial = x.MontoKilometrajeInicial
                            ,
                            DistanciaRecorrida = x.MontoKilometrajeTotal
                        }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Obtiene un objeto del tipo Kilometraje_Total por el id de la tabla
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public KILOMETRAJE_TOTAL obten_KILOMETRAJE_TOTALById(int userID
                                                             , int deviceID)
        {

            using (var db = new IntranetSAIEntities())
            {
                return (from x in db.KILOMETRAJE_TOTAL
                        where x.DeviceID == deviceID
                                && x.UserID == userID
                        select x).FirstOrDefault();
            }

        }

        /// <summary>
        /// Actualiza la tabla de kilometraje inical
        /// </summary>
        /// <param name="kilometraje"> Objeto con los valores a aactualizar</param>
        /// <returns></returns>
        public int actualiza_KILOMETRAJE_TOTAL(KILOMETRAJE_TOTAL kilometraje)
        {

            using (var db = new IntranetSAIEntities())
            {

                db.KILOMETRAJE_TOTAL.Attach(kilometraje);
                db.Entry(kilometraje).State = EntityState.Modified;
                return db.SaveChanges();

            }

        }
    }
}