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
    
    public partial class PAIS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PAIS()
        {
            this.PROVINCIA = new HashSet<PROVINCIA>();
            this.CLIENTE = new HashSet<CLIENTE>();
            this.TALLER = new HashSet<TALLER>();
        }
    
        public int CD_PAIS { get; set; }
        public string NM_PAIS { get; set; }
        public System.DateTime FE_CREACION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROVINCIA> PROVINCIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TALLER> TALLER { get; set; }
    }
}