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
    
    public partial class EMPLEADO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLEADO()
        {
            this.EMPLEADO1 = new HashSet<EMPLEADO>();
        }
    
        public int CD_USUARIO { get; set; }
        public Nullable<int> CD_USUARIO_SUPERVISOR { get; set; }
        public int CD_SUCURSAL { get; set; }
        public int CD_CARGO { get; set; }
        public Nullable<int> NU_EXTENSION { get; set; }
        public string DI_CORREO { get; set; }
        public System.DateTime FE_INGRESO { get; set; }
        public Nullable<System.DateTime> FE_RETIRO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLEADO> EMPLEADO1 { get; set; }
        public virtual EMPLEADO EMPLEADO2 { get; set; }
        public virtual CARGO CARGO { get; set; }
        public virtual SUCURSAL SUCURSAL { get; set; }
        public virtual USUARIO USUARIO { get; set; }
    }
}
