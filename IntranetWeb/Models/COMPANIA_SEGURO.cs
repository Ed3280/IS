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
    
    public partial class COMPANIA_SEGURO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMPANIA_SEGURO()
        {
            this.VEHICULO = new HashSet<VEHICULO>();
            this.CLIENTE = new HashSet<CLIENTE>();
        }
    
        public int CD_COMPANIA_SEGURO { get; set; }
        public string NM_COMPANIA_SEGURO { get; set; }
        public string NU_TELEFONO_FIJO { get; set; }
        public string DI_COMPANIA_SEGURO { get; set; }
        public bool IN_INACTIVO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VEHICULO> VEHICULO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE { get; set; }
    }
}
