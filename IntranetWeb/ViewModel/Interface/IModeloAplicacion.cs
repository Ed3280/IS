using IntranetWeb.ViewModel.Administrador;
using System.Collections.Generic;

namespace IntranetWeb.ViewModel.Interface
{
    public interface IModeloAplicacion
    {
        string EditarRegistro { get; set; }
        IList<Aplicacion> Aplicaciones { get; set; }
    }
}
