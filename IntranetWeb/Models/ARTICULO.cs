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
    using System.Collections.Generic;
    
    public partial class ARTICULO
    {
        public int CD_ARTICULO { get; set; }
        public string NM_ARTICULO { get; set; }
        public string DE_ARTICULO { get; set; }
        public int CD_MODELO { get; set; }
        public long NU_CANTIDAD_DISPONIBLE { get; set; }
        public decimal MT_PRECIO_COMPRA { get; set; }
        public bool IN_INACTIVO { get; set; }
        public System.DateTime FE_STATUS { get; set; }
        public Nullable<System.DateTime> FE_CREACION { get; set; }
    
        public virtual MODELO MODELO { get; set; }
    }
}
