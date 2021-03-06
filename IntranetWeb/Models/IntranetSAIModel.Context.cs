﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IntranetSAIEntities : DbContext
    {
        public IntranetSAIEntities()
            : base("name=IntranetSAIEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PARAMETRO_CONFIGURACION> PARAMETRO_CONFIGURACION { get; set; }
        public virtual DbSet<USUARIO_APLICACION> USUARIO_APLICACION { get; set; }
        public virtual DbSet<TICKET_ESTATUS> TICKET_ESTATUS { get; set; }
        public virtual DbSet<TIPO_NOTIFICACION> TIPO_NOTIFICACION { get; set; }
        public virtual DbSet<TIPO_ATENCION> TIPO_ATENCION { get; set; }
        public virtual DbSet<RESPUESTA_OPERADOR_GCM> RESPUESTA_OPERADOR_GCM { get; set; }
        public virtual DbSet<LOG_TRANSACCIONES> LOG_TRANSACCIONES { get; set; }
        public virtual DbSet<EMPLEADO> EMPLEADO { get; set; }
        public virtual DbSet<UNIDAD_ADMINISTRATIVA> UNIDAD_ADMINISTRATIVA { get; set; }
        public virtual DbSet<CARGO> CARGO { get; set; }
        public virtual DbSet<TIPO_SANGRE> TIPO_SANGRE { get; set; }
        public virtual DbSet<APLICACION> APLICACION { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<ORIGEN_NOTIFICACION> ORIGEN_NOTIFICACION { get; set; }
        public virtual DbSet<SUCURSAL> SUCURSAL { get; set; }
        public virtual DbSet<CORREGIMIENTO> CORREGIMIENTO { get; set; }
        public virtual DbSet<PAIS> PAIS { get; set; }
        public virtual DbSet<PROVINCIA> PROVINCIA { get; set; }
        public virtual DbSet<DISTRITO> DISTRITO { get; set; }
        public virtual DbSet<COMPANIA_SEGURO> COMPANIA_SEGURO { get; set; }
        public virtual DbSet<TIPO_CIERRE_VEHICULO> TIPO_CIERRE_VEHICULO { get; set; }
        public virtual DbSet<TIPO_COMBUSTIBLE> TIPO_COMBUSTIBLE { get; set; }
        public virtual DbSet<TIPO_TRANSMISION_VEHICULO> TIPO_TRANSMISION_VEHICULO { get; set; }
        public virtual DbSet<KILOMETRAJE_TOTAL> KILOMETRAJE_TOTAL { get; set; }
        public virtual DbSet<NOTIFICACION_GCM> NOTIFICACION_GCM { get; set; }
        public virtual DbSet<MARCA> MARCA { get; set; }
        public virtual DbSet<MODELO> MODELO { get; set; }
        public virtual DbSet<TIPO_ARTICULO> TIPO_ARTICULO { get; set; }
        public virtual DbSet<DISTRIBUIDOR> DISTRIBUIDOR { get; set; }
        public virtual DbSet<USUARIO_MOVIL_GCM> USUARIO_MOVIL_GCM { get; set; }
        public virtual DbSet<USUARIO_NOTIFICACION> USUARIO_NOTIFICACION { get; set; }
        public virtual DbSet<ANO_VEHICULO> ANO_VEHICULO { get; set; }
        public virtual DbSet<VEHICULO> VEHICULO { get; set; }
        public virtual DbSet<IMAGEN_NOTIFICACION> IMAGEN_NOTIFICACION { get; set; }
        public virtual DbSet<TIPO_DOCUMENTO_IDENTIDAD> TIPO_DOCUMENTO_IDENTIDAD { get; set; }
        public virtual DbSet<NOTIFICACION_SIN_GESTION> NOTIFICACION_SIN_GESTION { get; set; }
        public virtual DbSet<TICKET> TICKET { get; set; }
        public virtual DbSet<TICKET_HISTORICO> TICKET_HISTORICO { get; set; }
        public virtual DbSet<SEGMENTO_NEGOCIO> SEGMENTO_NEGOCIO { get; set; }
        public virtual DbSet<SOCIO_COMERCIAL> SOCIO_COMERCIAL { get; set; }
        public virtual DbSet<CLIENTE> CLIENTE { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<DISPOSITIVO_CLIENTE> DISPOSITIVO_CLIENTE { get; set; }
        public virtual DbSet<DISPOSITIVO> DISPOSITIVO { get; set; }
        public virtual DbSet<MENU_SERVICIO> MENU_SERVICIO { get; set; }
        public virtual DbSet<PLANTILLA_CORREO> PLANTILLA_CORREO { get; set; }
        public virtual DbSet<TIPO_PLANTILLA_CORREO> TIPO_PLANTILLA_CORREO { get; set; }
        public virtual DbSet<ARTICULO> ARTICULO { get; set; }
        public virtual DbSet<CONFIGURACION_KILOMETRAJE> CONFIGURACION_KILOMETRAJE { get; set; }
        public virtual DbSet<OPERACION_MANTENIMIENTO> OPERACION_MANTENIMIENTO { get; set; }
        public virtual DbSet<SOLICITUD_MANTENIMIENTO> SOLICITUD_MANTENIMIENTO { get; set; }
        public virtual DbSet<TALLER> TALLER { get; set; }
    
        public virtual int PROC_001_ES_VALIDA_CADENA_ENCRIPTADA(byte[] p_CONTRASENA_USUARIO, string p_CONTRASENA_ESCRITA, string p_KEY_CLAVE, ObjectParameter p_SALIDA)
        {
            var p_CONTRASENA_USUARIOParameter = p_CONTRASENA_USUARIO != null ?
                new ObjectParameter("P_CONTRASENA_USUARIO", p_CONTRASENA_USUARIO) :
                new ObjectParameter("P_CONTRASENA_USUARIO", typeof(byte[]));
    
            var p_CONTRASENA_ESCRITAParameter = p_CONTRASENA_ESCRITA != null ?
                new ObjectParameter("P_CONTRASENA_ESCRITA", p_CONTRASENA_ESCRITA) :
                new ObjectParameter("P_CONTRASENA_ESCRITA", typeof(string));
    
            var p_KEY_CLAVEParameter = p_KEY_CLAVE != null ?
                new ObjectParameter("P_KEY_CLAVE", p_KEY_CLAVE) :
                new ObjectParameter("P_KEY_CLAVE", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_001_ES_VALIDA_CADENA_ENCRIPTADA", p_CONTRASENA_USUARIOParameter, p_CONTRASENA_ESCRITAParameter, p_KEY_CLAVEParameter, p_SALIDA);
        }
    
        public virtual int PROC_002_OBTENER_CONTRASENA(byte[] p_CONTRASENA_USUARIO, ObjectParameter p_SALIDA)
        {
            var p_CONTRASENA_USUARIOParameter = p_CONTRASENA_USUARIO != null ?
                new ObjectParameter("P_CONTRASENA_USUARIO", p_CONTRASENA_USUARIO) :
                new ObjectParameter("P_CONTRASENA_USUARIO", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_002_OBTENER_CONTRASENA", p_CONTRASENA_USUARIOParameter, p_SALIDA);
        }
    
        public virtual int PROC_003_OBTENER_CADENA_ENCRIPTADA(string p_KEY_CLAVE, string p_CADENA_A_ENCRIPTAR, ObjectParameter p_SALIDA)
        {
            var p_KEY_CLAVEParameter = p_KEY_CLAVE != null ?
                new ObjectParameter("P_KEY_CLAVE", p_KEY_CLAVE) :
                new ObjectParameter("P_KEY_CLAVE", typeof(string));
    
            var p_CADENA_A_ENCRIPTARParameter = p_CADENA_A_ENCRIPTAR != null ?
                new ObjectParameter("P_CADENA_A_ENCRIPTAR", p_CADENA_A_ENCRIPTAR) :
                new ObjectParameter("P_CADENA_A_ENCRIPTAR", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_003_OBTENER_CADENA_ENCRIPTADA", p_KEY_CLAVEParameter, p_CADENA_A_ENCRIPTARParameter, p_SALIDA);
        }
    
        [DbFunction("IntranetSAIEntities", "FUNC_003_BUSCA_GESTION_ALERTAS")]
        public virtual IQueryable<FUNC_003_BUSCA_GESTION_ALERTAS_Result> FUNC_003_BUSCA_GESTION_ALERTAS(Nullable<int> p_CD_USUARIO, Nullable<int> p_TP_NOTIFICACION, Nullable<System.DateTime> p_FE_NOTIFICACION_DESDE, Nullable<System.DateTime> p_FE_NOTIFICACION_HASTA, Nullable<System.DateTime> p_FE_CREACION_DESDE, Nullable<System.DateTime> p_FE_CREACION_HASTA)
        {
            var p_CD_USUARIOParameter = p_CD_USUARIO.HasValue ?
                new ObjectParameter("P_CD_USUARIO", p_CD_USUARIO) :
                new ObjectParameter("P_CD_USUARIO", typeof(int));
    
            var p_TP_NOTIFICACIONParameter = p_TP_NOTIFICACION.HasValue ?
                new ObjectParameter("P_TP_NOTIFICACION", p_TP_NOTIFICACION) :
                new ObjectParameter("P_TP_NOTIFICACION", typeof(int));
    
            var p_FE_NOTIFICACION_DESDEParameter = p_FE_NOTIFICACION_DESDE.HasValue ?
                new ObjectParameter("P_FE_NOTIFICACION_DESDE", p_FE_NOTIFICACION_DESDE) :
                new ObjectParameter("P_FE_NOTIFICACION_DESDE", typeof(System.DateTime));
    
            var p_FE_NOTIFICACION_HASTAParameter = p_FE_NOTIFICACION_HASTA.HasValue ?
                new ObjectParameter("P_FE_NOTIFICACION_HASTA", p_FE_NOTIFICACION_HASTA) :
                new ObjectParameter("P_FE_NOTIFICACION_HASTA", typeof(System.DateTime));
    
            var p_FE_CREACION_DESDEParameter = p_FE_CREACION_DESDE.HasValue ?
                new ObjectParameter("P_FE_CREACION_DESDE", p_FE_CREACION_DESDE) :
                new ObjectParameter("P_FE_CREACION_DESDE", typeof(System.DateTime));
    
            var p_FE_CREACION_HASTAParameter = p_FE_CREACION_HASTA.HasValue ?
                new ObjectParameter("P_FE_CREACION_HASTA", p_FE_CREACION_HASTA) :
                new ObjectParameter("P_FE_CREACION_HASTA", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FUNC_003_BUSCA_GESTION_ALERTAS_Result>("[IntranetSAIEntities].[FUNC_003_BUSCA_GESTION_ALERTAS](@P_CD_USUARIO, @P_TP_NOTIFICACION, @P_FE_NOTIFICACION_DESDE, @P_FE_NOTIFICACION_HASTA, @P_FE_CREACION_DESDE, @P_FE_CREACION_HASTA)", p_CD_USUARIOParameter, p_TP_NOTIFICACIONParameter, p_FE_NOTIFICACION_DESDEParameter, p_FE_NOTIFICACION_HASTAParameter, p_FE_CREACION_DESDEParameter, p_FE_CREACION_HASTAParameter);
        }
    
        [DbFunction("IntranetSAIEntities", "FUNC_001_BUSCA_ALERTAS")]
        public virtual IQueryable<FUNC_001_BUSCA_ALERTAS_Result> FUNC_001_BUSCA_ALERTAS()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FUNC_001_BUSCA_ALERTAS_Result>("[IntranetSAIEntities].[FUNC_001_BUSCA_ALERTAS]()");
        }
    
        [DbFunction("IntranetSAIEntities", "FUNC_001_BUSCA_CLIENTES")]
        public virtual IQueryable<FUNC_001_BUSCA_CLIENTES_Result> FUNC_001_BUSCA_CLIENTES()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FUNC_001_BUSCA_CLIENTES_Result>("[IntranetSAIEntities].[FUNC_001_BUSCA_CLIENTES]()");
        }
    
        [DbFunction("IntranetSAIEntities", "FUNC_002_BUSCA_TICKET")]
        public virtual IQueryable<FUNC_002_BUSCA_TICKET_Result> FUNC_002_BUSCA_TICKET(Nullable<int> p_CD_USUARIO, Nullable<int> p_TP_ATENCION, string p_NU_SIM_GPS, Nullable<System.DateTime> p_FE_DESDE, Nullable<System.DateTime> p_FE_HASTA)
        {
            var p_CD_USUARIOParameter = p_CD_USUARIO.HasValue ?
                new ObjectParameter("P_CD_USUARIO", p_CD_USUARIO) :
                new ObjectParameter("P_CD_USUARIO", typeof(int));
    
            var p_TP_ATENCIONParameter = p_TP_ATENCION.HasValue ?
                new ObjectParameter("P_TP_ATENCION", p_TP_ATENCION) :
                new ObjectParameter("P_TP_ATENCION", typeof(int));
    
            var p_NU_SIM_GPSParameter = p_NU_SIM_GPS != null ?
                new ObjectParameter("P_NU_SIM_GPS", p_NU_SIM_GPS) :
                new ObjectParameter("P_NU_SIM_GPS", typeof(string));
    
            var p_FE_DESDEParameter = p_FE_DESDE.HasValue ?
                new ObjectParameter("P_FE_DESDE", p_FE_DESDE) :
                new ObjectParameter("P_FE_DESDE", typeof(System.DateTime));
    
            var p_FE_HASTAParameter = p_FE_HASTA.HasValue ?
                new ObjectParameter("P_FE_HASTA", p_FE_HASTA) :
                new ObjectParameter("P_FE_HASTA", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FUNC_002_BUSCA_TICKET_Result>("[IntranetSAIEntities].[FUNC_002_BUSCA_TICKET](@P_CD_USUARIO, @P_TP_ATENCION, @P_NU_SIM_GPS, @P_FE_DESDE, @P_FE_HASTA)", p_CD_USUARIOParameter, p_TP_ATENCIONParameter, p_NU_SIM_GPSParameter, p_FE_DESDEParameter, p_FE_HASTAParameter);
        }
    }
}
