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
    
    public partial class RESPUESTA_OPERADOR_GCM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RESPUESTA_OPERADOR_GCM()
        {
            this.TICKET = new HashSet<TICKET>();
        }
    
        public int ID_RESPUESTA_OPERADOR { get; set; }
        public int TP_ATENCION { get; set; }
        public string DE_TITULO { get; set; }
        public string DE_CONTENIDO { get; set; }
        public bool IN_INACTIVO { get; set; }
        public System.DateTime FE_ESTATUS { get; set; }
    
        public virtual TIPO_ATENCION TIPO_ATENCION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TICKET> TICKET { get; set; }
    }
}