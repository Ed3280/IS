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
    
    public partial class CARGO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CARGO()
        {
            this.CARGO1 = new HashSet<CARGO>();
            this.EMPLEADO = new HashSet<EMPLEADO>();
            this.ROL = new HashSet<ROL>();
        }
    
        public int CD_CARGO { get; set; }
        public int CD_UNIDAD_ADMINISTRATIVA { get; set; }
        public string NM_CARGO { get; set; }
        public string DE_CARGO { get; set; }
        public Nullable<int> CD_CARGO_PADRE { get; set; }
        public System.DateTime FE_CREACION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARGO> CARGO1 { get; set; }
        public virtual CARGO CARGO2 { get; set; }
        public virtual UNIDAD_ADMINISTRATIVA UNIDAD_ADMINISTRATIVA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLEADO> EMPLEADO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROL> ROL { get; set; }
    }
}
