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
    
    public partial class IMAGEN_NOTIFICACION
    {
        public long CD_IMAGEN_NOTIFCACION { get; set; }
        public int ID_NOTIFICACION { get; set; }
        public int ID_ORIGEN_NOTIFICACION { get; set; }
        public Nullable<double> NU_LATITUD { get; set; }
        public Nullable<double> NU_LONGITUD { get; set; }
        public string DE_LOCALIZACION { get; set; }
        public byte[] DE_LOCALIZACION_IMAGEN { get; set; }
        public string NM_IMAGEN { get; set; }
        public string NM_EXTENSION_IMAGEN { get; set; }
        public System.DateTime FE_CREACION { get; set; }
        public string DE_IMAGEN { get; set; }
    
        public virtual USUARIO_NOTIFICACION USUARIO_NOTIFICACION { get; set; }
    }
}
