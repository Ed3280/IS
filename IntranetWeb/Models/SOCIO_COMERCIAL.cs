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
    
    public partial class SOCIO_COMERCIAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SOCIO_COMERCIAL()
        {
            this.TALLER = new HashSet<TALLER>();
        }
    
        public int CD_SOCIO_COMERCIAL { get; set; }
        public string NM_SOCIO_COMERCIAL { get; set; }
        public Nullable<int> NU_TELEFONO_FIJO { get; set; }
        public Nullable<int> NU_TELEFONO_MOVIL { get; set; }
        public string DI_EMAIL { get; set; }
        public int CD_PAIS { get; set; }
        public int CD_PROVINCIA { get; set; }
        public int CD_DISTRITO { get; set; }
        public Nullable<int> CD_CORREGIMIENTO { get; set; }
        public string DI_OFICINA { get; set; }
        public Nullable<int> ID_DISTRIBUIDOR { get; set; }
        public int CD_SEGMENTO_NEGOCIO { get; set; }
        public bool IN_INACTIVO { get; set; }
        public Nullable<System.DateTime> FE_INACTIVO { get; set; }
        public System.DateTime FE_ESTATUS { get; set; }
        public System.DateTime FE_CREACION { get; set; }
    
        public virtual SEGMENTO_NEGOCIO SEGMENTO_NEGOCIO { get; set; }
        public virtual CORREGIMIENTO CORREGIMIENTO { get; set; }
        public virtual DISTRIBUIDOR DISTRIBUIDOR { get; set; }
        public virtual DISTRITO DISTRITO { get; set; }
        public virtual USUARIO USUARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TALLER> TALLER { get; set; }
    }
}
