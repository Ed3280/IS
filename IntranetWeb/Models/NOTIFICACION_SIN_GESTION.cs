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
    
    public partial class NOTIFICACION_SIN_GESTION
    {
        public int ID_NOTIFICACION { get; set; }
        public int ID_ORIGEN_NOTIFICACION { get; set; }
        public int TP_NOTIFICACION { get; set; }
        public System.DateTime FE_NOTIFICACION { get; set; }
        public int ID_DISTRIBUIDOR { get; set; }
        public int ID_DISPOSITIVO { get; set; }
        public Nullable<int> CD_CLIENTE { get; set; }
        public string ID_USUARIO_MOVIL { get; set; }
        public int CD_USUARIO { get; set; }
        public System.DateTime FE_CREACION { get; set; }
    
        public virtual TIPO_NOTIFICACION TIPO_NOTIFICACION { get; set; }
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual USUARIO USUARIO { get; set; }
        public virtual NOTIFICACION_SIN_GESTION NOTIFICACION_SIN_GESTION1 { get; set; }
        public virtual NOTIFICACION_SIN_GESTION NOTIFICACION_SIN_GESTION2 { get; set; }
    }
}
