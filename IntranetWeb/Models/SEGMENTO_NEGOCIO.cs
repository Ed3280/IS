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
    
    public partial class SEGMENTO_NEGOCIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SEGMENTO_NEGOCIO()
        {
            this.SOCIO_COMERCIAL = new HashSet<SOCIO_COMERCIAL>();
            this.MENU_SERVICIO = new HashSet<MENU_SERVICIO>();
        }
    
        public int CD_SEGMENTO_NEGOCIO { get; set; }
        public string NM_SEGMENTO_NEGOCIO { get; set; }
        public bool IN_INACTIVO { get; set; }
        public System.DateTime FE_ESTATUS { get; set; }
        public System.DateTime FE_CREACION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOCIO_COMERCIAL> SOCIO_COMERCIAL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MENU_SERVICIO> MENU_SERVICIO { get; set; }
    }
}
