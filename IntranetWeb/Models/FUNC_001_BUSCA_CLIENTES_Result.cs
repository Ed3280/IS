//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntranetWeb.Models
{
    using System;
    
    public partial class FUNC_001_BUSCA_CLIENTES_Result
    {
        public int CD_CLIENTE { get; set; }
        public string TP_DOCUMENTO_IDENTIDAD { get; set; }
        public string NU_DOCUMENTO_IDENTIDAD { get; set; }
        public string DE_NOMBRE_APELLIDO_RAZON_SOCIAL { get; set; }
        public string EMAIL { get; set; }
        public bool IN_USUARIO_INACTIVO { get; set; }
        public bool IN_REGISTRO_MOVIL { get; set; }
        public Nullable<int> NU_MOVIL { get; set; }
        public string SIMS_CARDS { get; set; }
    }
}
