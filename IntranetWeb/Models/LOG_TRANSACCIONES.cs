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
    
    public partial class LOG_TRANSACCIONES
    {
        public long ID_LOG { get; set; }
        public int CD_USUARIO { get; set; }
        public System.DateTime FE_OPERACION { get; set; }
        public string NM_THREAD { get; set; }
        public string DI_IP { get; set; }
        public string NM_CONTROLLER { get; set; }
        public string NM_ACTION { get; set; }
        public string NM_URL { get; set; }
        public string NM_LEVEL { get; set; }
        public string NM_LOGGER { get; set; }
        public string DE_MESSAGE { get; set; }
        public string DE_EXCEPTION { get; set; }
    
        public virtual USUARIO USUARIO { get; set; }
    }
}
