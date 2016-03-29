using System;
using IntranetWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.ViewModel.Dashboard.Cumpleano;
using IntranetWeb.ViewModel.Dashboard.Directorio;

namespace IntranetWeb.Core.Respositorios
{
    public class DashboardRepositorio
    {

        /// <summary>
        /// Función para buscar los cumpleaños
        /// </summary>
        /// <returns> Retorna los cumpleañeros de la compañía</returns>
        public IEnumerable<EmpleadoCumpleano> obtenCumpleanoEmpleado() {
            DateTime feActual = DateTime.Today;
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (
                    from x in db.EMPLEADO
                    where x.USUARIO.IN_USUARIO_INACTIVO == false
                    && x.USUARIO.FE_NACIMIENTO != null
                    && x.USUARIO.FE_NACIMIENTO.Value.Month >= feActual.Month
                    && x.USUARIO.FE_NACIMIENTO.Value.Day >= feActual.Day
                    orderby x.USUARIO.FE_NACIMIENTO.Value.Month
                            ,x.USUARIO.FE_NACIMIENTO.Value.Day
                    select new EmpleadoCumpleano() {
                            Nombre = x.USUARIO.DE_NOMBRE_APELLIDO
                        ,   FechaNacimiento = (DateTime)x.USUARIO.FE_NACIMIENTO
                    }
                    ).ToList();
            }
        }

        /// <summary>
        /// Realiza la búsqueda de los empleados
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public IEnumerable<Empleado> obtenEmpleados(string filtro) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (from x in db.EMPLEADO
                        where x.USUARIO.IN_USUARIO_INACTIVO == false
                        && (x.USUARIO.DE_NOMBRE_APELLIDO.ToUpper().Contains(filtro.Trim().ToUpper())
                        || x.CARGO.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA.ToUpper().Contains(filtro.Trim().ToUpper())
                        || x.CARGO.NM_CARGO.ToUpper().Contains(filtro.Trim().ToUpper())
                        || x.DI_CORREO.ToUpper().Contains(filtro.Trim().ToUpper())
                        )
                        orderby x.USUARIO.DE_NOMBRE_APELLIDO
                        select new Empleado() {
                             Id         = x.CD_USUARIO
                            ,Cargo      = x.CARGO.NM_CARGO
                            ,Correo     = x.DI_CORREO
                            ,Extension  = x.NU_EXTENSION
                            ,Nombre = x.USUARIO.DE_NOMBRE_APELLIDO 
                            ,NumeroMovil = x.USUARIO.NU_TELEFONO_MOVIL
                            ,UnidadAdministrativa = x.CARGO.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA
                        }).ToList();

            }
        }

        /// <summary>
        /// Búsueda de tickets segñun la frecuencia
        /// </summary>
        /// <param name="frecuencia"></param>
        /// <returns></returns>
        public object[] obtenGestionTicketUsuario(int cdUsuario, int frecuencia) {

            MonitorRepositorio monitorRepo = new MonitorRepositorio();

            DateTime fechaHasta = DateTime.Today;
            DateTime fechaDesde;

            switch (frecuencia) {

                case 1: //Esta semana
                    fechaDesde = fechaHasta.AddDays(-7);
                break;

                case 2: //Semana pasada
                    fechaHasta = fechaHasta.AddDays(-7);
                    fechaDesde = fechaHasta.AddDays(-7);
                    break;

                case 3: ///Este mes
                    fechaDesde = fechaHasta.AddMonths(-1);
                    break;

                case 4: //Último mes 
                    fechaHasta = fechaHasta.AddMonths(-1);
                    fechaDesde = fechaHasta.AddMonths(-1);
                    break;

                default:
                    fechaDesde = fechaHasta;
               break;
            }


            var result = monitorRepo.obten_Tickets(false, cdUsuario, null,null, fechaDesde, fechaHasta, null);

            int total = result.Count();

            int descartados = (from x in result
                               where x.EstatusSeleccionado == 4
                               select x
                                  ).Count();

            int atendido = (from x in result
                               where x.EstatusSeleccionado == 2
                               select x
                                  ).Count();

            int transferido = (from x in result
                            where x.EstatusSeleccionado == 3
                            select x
                                ).Count();

            int abierto = (from x in result
                               where x.EstatusSeleccionado == 1
                               select x
                            ).Count();

            return new object[] {
                    new  { label= Resources.EtiquetaResource.Atendido, data= Math.Round(((total!=0?(Convert.ToDouble(atendido)*100d)/Convert.ToDouble(total):0)),2,MidpointRounding.AwayFromZero), color= "#68BC31" }, //Verde
                    new  { label = Resources.EtiquetaResource.Abierto, data = Math.Round(((total!=0?(Convert.ToDouble(abierto)*100d)/Convert.ToDouble(total):0)),2,MidpointRounding.AwayFromZero), color = "#2091CF" }, //Azul
                    new  { label = Resources.EtiquetaResource.Descartado, data = Math.Round(((total!=0?(Convert.ToDouble(descartados)*100d)/Convert.ToDouble(total):0)),2,MidpointRounding.AwayFromZero), color = "#DA5430" }, //Rojo
                    new  { label = Resources.EtiquetaResource.Transferido, data = Math.Round(((total!=0?(Convert.ToDouble(transferido)*100d)/Convert.ToDouble(total):0)),2,MidpointRounding.AwayFromZero), color = "#FEE074" } //Amarillo

                };
            }

        /// <summary>
        /// Retorna las aplicaciones activas del usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<string> obtenAplicacionesActivas_byUsuarioId(int id)
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.USUARIO_APLICACION
                        where x.APLICACION.IN_APLICACION_INACTIVA == false
                        && x.IN_INACTIVO         == false
                        && x.CD_USUARIO == id
                        select x.APLICACION.DE_ALIAS).ToList().AsEnumerable();
            }
        }
    }
}