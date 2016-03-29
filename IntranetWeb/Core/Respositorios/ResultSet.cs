using IntranetWeb.ViewModel.Administrador;
using IntranetWeb.ViewModel.Cliente;
using IntranetWeb.ViewModel.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Web;
using System.Web.UI.WebControls;

namespace IntranetWeb.Core.Respositorios
{
    public class ResultSet
    {

        #region Log de Transacciones
        public List<LogTransacciones> GetResult(string search, string sortOrder, int start, int length, List<LogTransacciones> dtResult, List<string> columnFilters)
        {
            sortOrder = sortOrder== "EditarRegistro" ? "FechaOperacion DESC" : sortOrder;
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<LogTransacciones> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<LogTransacciones> FilterResult(string search, List<LogTransacciones> dtResult, List<string> columnFilters)
        {
            IQueryable<LogTransacciones> results = dtResult.AsQueryable();

            results = results.Where(
              p => (search == null || 
                   (p.NombreUsuario != null && p.NombreUsuario.ToLower().Contains(search.ToLower()) || 
                   p.FechaOperacion != null && p.FechaOperacion.ToString().ToLower().Contains(search.ToLower())||
                   p.Ip != null && p.Ip.ToString().ToLower().Contains(search.ToLower())||
                    p.Controlador != null && p.Controlador.ToString().ToLower().Contains(search.ToLower()) ||
                     p.Accion != null && p.Accion.ToString().ToLower().Contains(search.ToLower()) ||
                      p.Url != null && p.Url.ToString().ToLower().Contains(search.ToLower()) ||
                       p.DescripcionNivel != null && p.DescripcionNivel.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Mensaje != null && p.Mensaje.ToString().ToLower().Contains(search.ToLower())
                        )));

            return results;
        }
        #endregion

        #region Tickets
        public List<Ticket> GetResult(string search, string sortOrder, int start, int length, List<Ticket> dtResult, List<string> columnFilters)
        {
            Expression<Func<Ticket, object>> sortExpression = null;
           var data =  FilterResult(search, dtResult, columnFilters);
            sortOrder = sortOrder== "EditarRegistro" ? "FechaEstatus DESC" : sortOrder;

            if (sortOrder != null && sortOrder.Contains(".")) {
                string sortOrderAux = sortOrder.Substring(0, sortOrder.Contains(" ")?sortOrder.LastIndexOf(" "): sortOrder.Length);
                switch (sortOrderAux)
                {
                    case "Cliente.Id":
                        sortExpression = (x => x.Cliente.Id);
                        break;
                    case "Cliente.NombreCompleto":
                        sortExpression = (x => x.Cliente.NombreCompleto);
                        break;
                    case "Dispositivo.DVR":
                        sortExpression = (x => x.Dispositivo.DVR);
                        break;
                    case "Dispositivo.SimCard":
                        sortExpression = (x => x.Dispositivo.SimCard);
                        break;
                    case "Distribuidor.Nombre":
                        sortExpression = (x => x.Distribuidor.Nombre);
                        break;
                    
                }
                data = sortOrder.EndsWith("DESC") ? data.OrderByDescending(sortExpression): data.OrderBy(sortExpression);
                
            } else
                data = data.SortBy(sortOrder);
                    
                    
              return data.Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<Ticket> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<Ticket> FilterResult(string search, List<Ticket> dtResult, List<string> columnFilters)
        {
            IQueryable<Ticket> results = dtResult.AsQueryable();

            results = results.Where(
              p => (search == null ||
                   (p.Id.ToString().Contains(search.ToLower()) ||
                   p.Cliente.Id.ToString().ToLower().Contains(search.ToLower()) ||
                   p.Cliente.NombreCompleto != null && p.Cliente.NombreCompleto.ToString().ToLower().Contains(search.ToLower()) ||
                   p.DescripcionTipoNotificacion != null && p.DescripcionTipoNotificacion.ToString().ToLower().Contains(search.ToLower()) ||
                   p.Dispositivo.DVR != null && p.Dispositivo.DVR.ToString().ToLower().Contains(search.ToLower()) ||
                   p.Dispositivo.SimCard != null && p.Dispositivo.SimCard.ToString().ToLower().Contains(search.ToLower()) ||
                   p.Distribuidor.Nombre != null && p.Distribuidor.Nombre.ToString().ToLower().Contains(search.ToLower()) ||
                   p.NombreUsuarioTicket != null && p.NombreUsuarioTicket.ToString().ToLower().Contains(search.ToLower()) ||
                   p.FechaEstatus != null && p.FechaEstatus.ToString().ToLower().Contains(search.ToLower())||
                   p.DescripcionEstatus != null && p.DescripcionEstatus.ToString().ToLower().Contains(search.ToLower()) 
                        )));

            return results;
        }



        #endregion

        #region Clientes


        public List<Cliente> GetResult(string search, string sortOrder, int start, int length, List<Cliente> dtResult, List<string> columnFilters)
        {
            sortOrder = sortOrder == "EditarRegistro" ? "Nombre" : sortOrder;
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<Cliente> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<Cliente> FilterResult(string search, List<Cliente> dtResult, List<string> columnFilters)
        {
            IQueryable<Cliente> results = dtResult.AsQueryable();

            results = results.Where(
              p => (search == null ||
                   (
                       p.TipoDocumentoSeleccionado != null && p.TipoDocumentoSeleccionado.ToLower().Contains(search.ToLower()) ||
                       p.Cedula != null && p.Cedula.ToLower().Contains(search.ToLower()) ||
                       p.Nombre != null && p.Nombre.ToString().ToLower().Contains(search.ToLower()) ||
                       p.Email != null && p.Email.ToString().ToLower().Contains(search.ToLower()) ||
                       p.NumeroMovil != null && p.NumeroMovil.ToString().ToLower().Contains(search.ToLower()) ||
                       p.ListadoSims != null && p.ListadoSims.ToString().ToLower().Contains(search.ToLower()) || 
                       p.DescripcionEstaActivo != null && p.DescripcionEstaActivo.ToString().ToLower().Contains(search.ToLower()) 
                 )));

            return results;
        }



        #endregion
        
        #region Administrador/Usuario

        public List<IntranetWeb.ViewModel.Administrador.Usuario> GetResult(string search, string sortOrder, int start, int length, List<IntranetWeb.ViewModel.Administrador.Usuario> dtResult, List<string> columnFilters)
        {

            Expression<Func<Usuario, object>> sortExpression = null;
            var data = FilterResult(search, dtResult, columnFilters);
            sortOrder = sortOrder == "EditarRegistro" ? "NombreApellido" : sortOrder;

            if (sortOrder != null && sortOrder.Contains(".")){
                string sortOrderAux = sortOrder.Substring(0, sortOrder.Contains(" ") ? sortOrder.LastIndexOf(" ") : sortOrder.Length);
                switch (sortOrderAux)
                {
                    case "DatosEmpleado.NombreSupervisor":
                        sortExpression = (x => x.DatosEmpleado.NombreSupervisor);
                        break;
                    case "DatosEmpleado.NombreCargo":
                        sortExpression = (x => x.DatosEmpleado.NombreCargo);
                        break;
                    case "DatosEmpleado.NombreSucursal":
                        sortExpression = (x => x.DatosEmpleado.NombreSucursal);
                        break;

                    case "DatosEmpleado.FechaRetiro":
                        sortExpression = (x => x.DatosEmpleado.FechaRetiro);
                        break;                        

                }
                data = sortOrder.EndsWith("DESC") ? data.OrderByDescending(sortExpression) : data.OrderBy(sortExpression);
            }
            else
                data = data.SortBy(sortOrder);


            return data.Skip(start).Take(length).ToList(); 
        }

        public int Count(string search, List<IntranetWeb.ViewModel.Administrador.Usuario> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<IntranetWeb.ViewModel.Administrador.Usuario> FilterResult(string search, List<IntranetWeb.ViewModel.Administrador.Usuario> dtResult, List<string> columnFilters)
        {
            IQueryable<IntranetWeb.ViewModel.Administrador.Usuario> results = dtResult.AsQueryable();

            results = results.Where(
              p => (search == null ||
                   (p.NombreUsuario != null && p.NombreUsuario.ToLower().Contains(search.ToLower()) ||
                    p.Documento.ToString().ToLower().Contains(search.ToLower()) ||
                    p.NombreApellido != null && p.NombreApellido.ToString().ToLower().Contains(search.ToLower()) ||
                    p.Email != null && p.Email.ToString().ToLower().Contains(search.ToLower()) ||
                    p.DescripcionEstatus != null && p.DescripcionEstatus.ToString().ToLower().Contains(search.ToLower()) ||
                    p.DescripcionBloqueado != null && p.DescripcionBloqueado.ToString().ToLower().Contains(search.ToLower()) ||
                    p.FechaEstatus != null && p.FechaEstatus.ToString().ToLower().Contains(search.ToLower())||
                    p.DatosEmpleado != null && (
                                p.DatosEmpleado.NombreSucursal != null && p.DatosEmpleado.NombreSucursal.ToString().ToLower().Contains(search.ToLower())||
                                p.DatosEmpleado.NombreCargo != null && p.DatosEmpleado.NombreCargo.ToString().ToLower().Contains(search.ToLower()) ||
                                p.DatosEmpleado.NombreSupervisor != null && p.DatosEmpleado.NombreSupervisor.ToString().ToLower().Contains(search.ToLower()) ||
                                p.DatosEmpleado.FechaRetiro != null && p.DatosEmpleado.FechaRetiro.ToString().ToLower().Contains(search.ToLower())                            
                    )
                 )));
            return results;
        }


        #endregion

        #region Administrador/SocioComercial
        public List<SocioComercial> GetResult(string search, string sortOrder, int start, int length, List<SocioComercial> dtResult, List<string> columnFilters)
        {
            sortOrder = sortOrder == "EditarRegistro" ? "Nombre DESC" : sortOrder;
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<SocioComercial> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<SocioComercial> FilterResult(string search, List<SocioComercial> dtResult, List<string> columnFilters)
        {
            IQueryable<SocioComercial> results = dtResult.AsQueryable();

            results = results.Where(
              p => (search == null ||
                   (p.Documento != null && p.Documento.ToLower().Contains(search.ToLower()) ||
                   p.Nombre != null && p.Nombre.ToString().ToLower().Contains(search.ToLower()) ||
                   p.Email != null && p.Email.ToString().ToLower().Contains(search.ToLower()) ||
                    p.DescripcionEstatus != null && p.DescripcionEstatus.ToString().ToLower().Contains(search.ToLower()) ||
                    p.FechaEstatus != null && p.FechaEstatus.ToString().ToLower().Contains(search.ToLower()) 
                    )));

            return results;
        }

        #endregion
    }
}