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
    
    public partial class TIPO_NOTIFICACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_NOTIFICACION()
        {
            this.USUARIO_NOTIFICACION = new HashSet<USUARIO_NOTIFICACION>();
            this.NOTIFICACION_SIN_GESTION = new HashSet<NOTIFICACION_SIN_GESTION>();
            this.TICKET = new HashSet<TICKET>();
        }
    
        public int TP_NOTIFICACION { get; set; }
        public string DE_TIPO_NOTIFICACION { get; set; }
        public int CD_PRIORIDAD { get; set; }
        public string DE_COLOR_TITULO { get; set; }
        public string DE_COLOR_BODY { get; set; }
        public bool IN_INACTIVO { get; set; }
        public System.DateTime FE_ESTATUS { get; set; }
        public string DE_COLOR_LABEL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_NOTIFICACION> USUARIO_NOTIFICACION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICACION_SIN_GESTION> NOTIFICACION_SIN_GESTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TICKET> TICKET { get; set; }
    }
}