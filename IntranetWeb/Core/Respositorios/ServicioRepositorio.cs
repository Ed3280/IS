using IntranetWeb.ViewModel.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.Models;
using IntranetWeb.Core.Utils;

namespace IntranetWeb.Core.Respositorios
{
    public class ServicioRepositorio
    {

        public ServicioRepositorio() { }


        public IEnumerable<SolicitudMantenimiento> obtenSolicitudesMantenimiento() {
            IEnumerable<SolicitudMantenimiento> result;
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                result= (
                            from x in db.SOLICITUD_MANTENIMIENTO
                            orderby x.FE_SOLICITUD
                            select new SolicitudMantenimiento {
                                Id = x.CD_SOLICITUD
                                ,Documento = x.CLIENTE.USUARIO.TP_DOCUMENTO_IDENTIDAD + "-"+x.CLIENTE.USUARIO.NU_DOCUMENTO_IDENTIDAD
                                ,NombreCliente = x.CLIENTE.DE_NOMBRE_APELLIDO_RAZON_SOCIAL
                                ,FechaSolicitud = x.FE_SOLICITUD
                                ,EstatusSolicitud = x.DE_ESTATUS_SOLICITUD
                                ,FechaEstatus = x.FE_ESTATUS
                                ,Vehiculo = x.VEHICULO.NU_VIN + (x.VEHICULO.CD_MARCA != null || x.VEHICULO.CD_MODELO != null || x.VEHICULO.CD_ANO != null ? " (" +
                                ((x.VEHICULO.CD_MARCA != null ? x.VEHICULO.MARCA.NM_MARCA + " " : "") +
                                (x.VEHICULO.CD_MODELO != null ? x.VEHICULO.MODELO.NM_MODELO + " " : "") +
                                (x.VEHICULO.CD_ANO != null ? x.VEHICULO.ANO_VEHICULO.CD_ANO + " " : "")
                                ).Trim() + ")" : "")
                                ,TipoMantenimiento = x.CONFIGURACION_KILOMETRAJE.MT_KILOMETRAJE+" KM"
                                ,Dispositivo = x.DISPOSITIVO_CLIENTE.DISPOSITIVO.NM_DISPOSITIVO
                                ,
                                Kilometraje = (from y in db.KILOMETRAJE_TOTAL
                                               where y.DeviceID == x.ID_DISPOSITIVO
                                               select y.MontoKilometrajeTotal).FirstOrDefault()
                            }
                    ).ToList().AsEnumerable();
            }


            result.ToList().ForEach(x => {
                x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() { { "data-id", x.Id.ToString() } }
, true, false, false); x.Kilometraje = Math.Round(x.Kilometraje, MidpointRounding.AwayFromZero);
            });
            return result;
        }

        /// <summary>
        /// Se obtienen las solicitudes por Id
        /// </summary>
        /// <returns></returns>
        public SolicitudMantenimiento obtenSolicitudesMantenimiento_ById(int Id)
        {
            SolicitudMantenimiento result = new SolicitudMantenimiento();
          using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                 result = (
                            from x in db.SOLICITUD_MANTENIMIENTO
                            where x.CD_SOLICITUD == Id
                            orderby x.FE_SOLICITUD
                            select new SolicitudMantenimiento
                            {
                                Id = x.CD_SOLICITUD
                                ,
                                Documento = x.CLIENTE.USUARIO.TP_DOCUMENTO_IDENTIDAD + "-" + x.CLIENTE.USUARIO.NU_DOCUMENTO_IDENTIDAD
                                ,
                                NombreCliente = x.CLIENTE.DE_NOMBRE_APELLIDO_RAZON_SOCIAL
                                ,
                                FechaSolicitud = x.FE_SOLICITUD
                                ,
                                EstatusSolicitud = x.DE_ESTATUS_SOLICITUD
                                ,
                                FechaEstatus = x.FE_ESTATUS
                                ,
                                Vehiculo = x.VEHICULO.NU_VIN + (x.VEHICULO.CD_MARCA != null || x.VEHICULO.CD_MODELO != null || x.VEHICULO.CD_ANO != null ? " (" +
                                ((x.VEHICULO.CD_MARCA != null ? x.VEHICULO.MARCA.NM_MARCA + " " : "") +
                                (x.VEHICULO.CD_MODELO != null ? x.VEHICULO.MODELO.NM_MODELO + " " : "") +
                                (x.VEHICULO.CD_ANO != null ? x.VEHICULO.ANO_VEHICULO.CD_ANO + " " : "")
                                ).Trim() + ")" : "")
                                ,
                                TipoMantenimiento = x.CONFIGURACION_KILOMETRAJE.MT_KILOMETRAJE + " KM"
                                ,Observacion = x.DE_OBSERVACION
                                ,Dispositivo = x.DISPOSITIVO_CLIENTE.DISPOSITIVO.NM_DISPOSITIVO
                                ,Operaciones = (from y in  x.CONFIGURACION_KILOMETRAJE.OPERACION_MANTENIMIENTO
                                                orderby y.CD_OPERACION
                                                select new Operacion {Id = y.CD_OPERACION
                                                                     ,Nombre = y.NM_OPERACION_MANTENIMIENTO
                                                ,Duracion = y.MT_MINUTO_DURACION_OPERACION}).ToList()
                                                  ,Kilometraje = (from y in db.KILOMETRAJE_TOTAL
                                                                 where y.DeviceID == x.ID_DISPOSITIVO
                                                                 select y.MontoKilometrajeTotal).FirstOrDefault()
                            }
                    ).FirstOrDefault();
            }
            result.Kilometraje = Math.Round(result.Kilometraje, MidpointRounding.AwayFromZero);
            return result;
        }
    }
}