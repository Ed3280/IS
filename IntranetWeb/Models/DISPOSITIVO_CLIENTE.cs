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
    
    public partial class DISPOSITIVO_CLIENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DISPOSITIVO_CLIENTE()
        {
            this.USUARIO_NOTIFICACION = new HashSet<USUARIO_NOTIFICACION>();
            this.SOLICITUD_MANTENIMIENTO = new HashSet<SOLICITUD_MANTENIMIENTO>();
        }
    
        public int ID_DISPOSITIVO { get; set; }
        public int CD_CLIENTE { get; set; }
        public Nullable<int> CD_VEHICULO { get; set; }
        public string DE_CONTRASENA_DVR { get; set; }
        public bool IN_ADMITE_APERTURA_REMOTA { get; set; }
        public bool IN_INACTIVO { get; set; }
        public System.DateTime FE_ESTATUS { get; set; }
        public System.DateTime FE_CREACION { get; set; }
    
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual VEHICULO VEHICULO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_NOTIFICACION> USUARIO_NOTIFICACION { get; set; }
        public virtual DISPOSITIVO DISPOSITIVO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLICITUD_MANTENIMIENTO> SOLICITUD_MANTENIMIENTO { get; set; }
    }
}