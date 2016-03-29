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
    
    public partial class MODELO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MODELO()
        {
            this.VEHICULO = new HashSet<VEHICULO>();
            this.ANO_VEHICULO = new HashSet<ANO_VEHICULO>();
            this.ARTICULO = new HashSet<ARTICULO>();
        }
    
        public int CD_MODELO { get; set; }
        public string NM_MODELO { get; set; }
        public int CD_TIPO_ARTICULO { get; set; }
        public int CD_MARCA { get; set; }
        public int CD_UNIDAD_MEDIDA { get; set; }
        public bool IN_INACTIVO { get; set; }
        public System.DateTime FE_STATUS { get; set; }
        public System.DateTime FE_CREACION { get; set; }
    
        public virtual MARCA MARCA { get; set; }
        public virtual TIPO_ARTICULO TIPO_ARTICULO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VEHICULO> VEHICULO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ANO_VEHICULO> ANO_VEHICULO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARTICULO> ARTICULO { get; set; }
    }
}
