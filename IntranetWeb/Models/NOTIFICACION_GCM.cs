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
    
    public partial class NOTIFICACION_GCM
    {
        public long CD_NOTIFICACION_GCM { get; set; }
        public int CD_USUARIO_GCM { get; set; }
        public string DE_MENSAJE { get; set; }
        public System.DateTime FE_ENVIO_MENSAJE { get; set; }
        public bool IN_ENVIADO { get; set; }
        public string DE_RESPUESTA_MENSAJE_GCM { get; set; }
        public Nullable<System.DateTime> FE_MENSAJE_RESPUESTA_GCM { get; set; }
    
        public virtual USUARIO_MOVIL_GCM USUARIO_MOVIL_GCM { get; set; }
    }
}