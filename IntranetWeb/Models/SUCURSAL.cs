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
    
    public partial class SUCURSAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUCURSAL()
        {
            this.EMPLEADO = new HashSet<EMPLEADO>();
        }
    
        public int CD_SUCURSAL { get; set; }
        public string NM_SUCURSAL { get; set; }
        public string DE_SUCURSAL { get; set; }
        public Nullable<int> NU_TELEFONO { get; set; }
        public bool IN_INACTIVA { get; set; }
        public System.DateTime FE_ESTATUS { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLEADO> EMPLEADO { get; set; }
    }
}
