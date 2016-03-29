using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.Core.Utils;
using IntranetWeb.Models;
using IntranetWeb.ViewModel.Administrador;
using IntranetWeb.ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace IntranetWeb.Core.Respositorios
{
    public class AdministradorRepositorio
    {

        private Log4NetLogger log;

        public AdministradorRepositorio() {
            log = new Log4NetLogger();
        }

        /// <summary>
        /// Obtiene el listado de respuestas del operador por GCM
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RespuestaOperadorGCM> obtenRespuestaOperadorGCM() {

            IEnumerable<RespuestaOperadorGCM> result = new List<RespuestaOperadorGCM>();

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                result = (from x in db.RESPUESTA_OPERADOR_GCM
                          orderby x.FE_ESTATUS descending
                          select new RespuestaOperadorGCM()
                          {
                              Id = x.ID_RESPUESTA_OPERADOR
                              , Contenido = x.DE_CONTENIDO
                              , DescripcionEstatus = (x.IN_INACTIVO ? Resources.DescripcionResource.Inactivo : Resources.DescripcionResource.Activo)
                              , DescripcionTipoAtencion = x.TIPO_ATENCION.DE_TIPO_ATENCION
                              , FeEstatus = x.FE_ESTATUS
                              , Titulo = x.DE_TITULO
                          }).ToList();

            }

            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() { { "data-id-respuesta", x.Id.ToString() } }
                                                                                                                             , false, true, false));

            return result;
        }

        /// <summary>
        /// Retorna el listado de roles y sus aplicaciones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rol> obtenRolAplicacion() {
            IEnumerable<Rol> result = new List<Rol>();

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                result = (from x in db.ROL
                          where x.CD_ROL != 1
                          orderby x.NM_ROL, x.FE_ESTATUS descending
                          select new Rol()
                          {
                              Id = x.CD_ROL
                              , Nombre = x.NM_ROL
                              , FechaCreacion = x.FE_CREACION
                              , FechaEstatus = x.FE_ESTATUS
                              , DescripcionEstatus = x.IN_ROL_ACTIVO ? Resources.DescripcionResource.Activo : Resources.DescripcionResource.Inactivo
                              , IndicadorActivo = x.IN_ROL_ACTIVO
                          }).ToList();

            }

            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() { { "data-id-rol", x.Id.ToString() } }
                                                                                                                                    , false, true, false));


            return result;
        }



        /// <summary>
        /// Retorna los datos del socio comercial no registrado dado el tipo de documento y el número de documento 
        /// </summary>
        /// <returns></returns>
        public SocioComercial obten_SocioComercialNoRegistrado_By_NumeroDocumento(string TipoDocumento
                                                                                , string NumeroDocumento)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.USUARIO
                        where x.TP_DOCUMENTO_IDENTIDAD == TipoDocumento
                        && x.NU_DOCUMENTO_IDENTIDAD == NumeroDocumento
                        select new SocioComercial
                        {
                            Id = x.CD_USUARIO
                            ,
                            Nombre = x.DE_NOMBRE_APELLIDO
                            ,
                            TipoDocumentoSeleccionado = x.TP_DOCUMENTO_IDENTIDAD
                            ,
                            NumeroDocumento = x.NU_DOCUMENTO_IDENTIDAD
                            ,
                            TelefonoFijo = x.NU_TELEFONO_FIJO
                            ,
                            TelefonoMovil = x.NU_TELEFONO_MOVIL
                            ,
                            Email = x.DI_EMAIL_USUARIO
                        }
                                   ).FirstOrDefault();
            }
        }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="TipoDocumento"></param>
       /// <param name="NumeroDocumento"></param>
       /// <returns></returns>
        public Usuario obten_UsuarioInternoNoRegistrado_By_NumeroDocumento(string TipoDocumento
                                                                               , string NumeroDocumento)
        {
            String contrasenaDummy = "cambiar*001";

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return  (from x in db.USUARIO
                        where x.TP_DOCUMENTO_IDENTIDAD == TipoDocumento
                        && x.NU_DOCUMENTO_IDENTIDAD == NumeroDocumento
                        select new Usuario
                        {
                             Id = x.CD_USUARIO
                            ,ContrasenaNueva = contrasenaDummy
                            ,RepetirContrasena = contrasenaDummy
                            ,NombreUsuario     =  x.NM_USUARIO
                            ,NombreApellido = x.DE_NOMBRE_APELLIDO
                            ,TipoDocumentoSeleccionado = x.TP_DOCUMENTO_IDENTIDAD
                            ,NumeroDocumento = x.NU_DOCUMENTO_IDENTIDAD
                            ,TelefonoFijo = x.NU_TELEFONO_FIJO
                            ,TelefonoMovil = x.NU_TELEFONO_MOVIL
                            ,Email = x.DI_EMAIL_USUARIO
                            ,FechaNacimiento = x.FE_NACIMIENTO
                            ,IndicadorActivo = !x.IN_USUARIO_INACTIVO
                            ,IndicadorRegistroNoEditable = (x.SOCIO_COMERCIAL!=null&&!x.IN_USUARIO_INACTIVO)
                                                           ||(x.EMPLEADO!=null&&x.EMPLEADO.FE_RETIRO==null)
                                                           ||(x.USUARIO2.Count()>0)? true : false

                        }).FirstOrDefault();
            }
        }



        /// <summary>
        /// Obtiene los datos del empleado no registrado
        /// </summary>
        /// <param name="TipoDocumento"></param>
        /// <param name="NumeroDocumento"></param>
        /// <returns></returns>
        public Empleado obten_EmpleadoNoRegistrado_By_NumeroDocumento(string TipoDocumento
                                                                      , string NumeroDocumento)
        {
            String contrasenaDummy = "cambiar*001";

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.USUARIO
                        where x.TP_DOCUMENTO_IDENTIDAD == TipoDocumento
                        && x.NU_DOCUMENTO_IDENTIDAD == NumeroDocumento
                        select new Empleado
                        {
                            Id = x.CD_USUARIO
                            ,
                            ContrasenaNueva = contrasenaDummy
                            ,
                            RepetirContrasena = contrasenaDummy
                            ,
                            NombreUsuario = x.NM_USUARIO
                            ,
                            NombreApellido = x.DE_NOMBRE_APELLIDO
                            ,
                            TipoDocumentoSeleccionado = x.TP_DOCUMENTO_IDENTIDAD
                            ,
                            NumeroDocumento = x.NU_DOCUMENTO_IDENTIDAD
                            ,
                            TelefonoFijo = x.NU_TELEFONO_FIJO
                            ,
                            TelefonoMovil = x.NU_TELEFONO_MOVIL
                            ,
                            Email = x.DI_EMAIL_USUARIO
                            ,
                            FechaNacimiento = x.FE_NACIMIENTO
                            ,
                            IndicadorActivo = !x.IN_USUARIO_INACTIVO                            

                        }).FirstOrDefault();
            }


        }


        /// <summary>
        /// Retorna los usuarios registrados en el sistema
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Usuario> obtenUsuario(int? cdUsuarioPadre)
        {
            IEnumerable<Usuario> result = new List<Usuario>();
            
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                
                    result = (from x in db.USUARIO
                              where (x.USUARIO2.Select(y => y.CD_USUARIO).Contains((int)(cdUsuarioPadre==null?-1:cdUsuarioPadre))
                              )|| cdUsuarioPadre == null
                              select new Usuario
                          {
                               Id = x.CD_USUARIO
                              ,DescripcionBloqueado             = x.IN_USUARIO_BLOQUEADO? Resources.DescripcionResource.Bloqueado : ""
                              ,DescripcionEstatus               = x.IN_USUARIO_INACTIVO? Resources.DescripcionResource.Inactivo : Resources.DescripcionResource.Activo
                              ,Email                            = x.DI_EMAIL_USUARIO
                              ,FechaCreacion                    = x.FE_CREACION
                              ,FechaEstatus                     = x.FE_ESTATUS
                              ,FechaNacimiento                  = x.FE_NACIMIENTO
                              ,IndicadorActivo                  = !x.IN_USUARIO_INACTIVO
                              ,IndicadorBloqueado               =  x.IN_USUARIO_BLOQUEADO
                              ,NombreApellido                   = x.DE_NOMBRE_APELLIDO
                              ,NombreUsuario                    = x.NM_USUARIO
                              ,NumeroDocumento                  = x.NU_DOCUMENTO_IDENTIDAD
                              ,TelefonoFijo                     = x.NU_TELEFONO_FIJO
                              ,TelefonoMovil                    = x.NU_TELEFONO_MOVIL
                              ,TipoDocumentoSeleccionado        = x.TP_DOCUMENTO_IDENTIDAD                              
                          }
                          ).ToList();
            }
            
            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() { { "data-id-usuario", x.Id.ToString() } }
                                                                                                                           , true, true, cdUsuarioPadre!=null? true : false));

            return result;
        }


        /// <summary>
        /// Retorna los socios comerciales registrados en el sistema
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SocioComercial> obtenSocioComercial()
        {
            IEnumerable<SocioComercial> result = new List<SocioComercial>();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                result = (from x in db.SOCIO_COMERCIAL
                          select new SocioComercial
                          {
                              Id = x.CD_SOCIO_COMERCIAL
                             
                              ,Nombre = x.NM_SOCIO_COMERCIAL
                              ,
                              DescripcionEstatus = x.IN_INACTIVO ? Resources.DescripcionResource.Inactivo : Resources.DescripcionResource.Activo
                              ,
                              Email = x.DI_EMAIL
                              ,
                              FechaCreacion = x.FE_CREACION
                              ,FechaEstatus = x.FE_ESTATUS
                              
                              ,NumeroDocumento = x.USUARIO.NU_DOCUMENTO_IDENTIDAD
                              
                              ,TelefonoFijo = x.NU_TELEFONO_FIJO
                              ,
                              TelefonoMovil = x.NU_TELEFONO_MOVIL
                              ,
                              TipoDocumentoSeleccionado = x.USUARIO.TP_DOCUMENTO_IDENTIDAD
                          }
                          ).ToList();
            }


            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() { { "data-id-socio", x.Id.ToString() } }
                                                                                                                                    , true, true, false));

            return result;
        }



       



        /// <summary>
        /// Retorna el listado de cargos
        /// </summary>
        /// <returns></returns>
        public IList<Cargo> obtenCargo() {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.CARGO
                        select new Cargo()
                        {
                            Id = x.CD_CARGO
                            ,
                            DescripcionCargo = x.DE_CARGO
                            ,
                            Nombre = x.NM_CARGO
                            ,
                            CargoPadreSeleccionado = x.CD_CARGO_PADRE
                            ,
                            NombreUnidadAdministrativa = x.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA
                            ,
                            UnidadAdministrativaSeleccionada = x.CD_UNIDAD_ADMINISTRATIVA
                        }
                        ).ToList();

            }
        }



        /// <summary>
        /// Retorna un cargo dado el Id
        /// </summary>
        /// <returns></returns>
        public Cargo obtenCargo_ById(int Id)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.CARGO
                        where x.CD_CARGO == Id
                        select new Cargo()
                        {
                            Id = x.CD_CARGO
                            ,
                            DescripcionCargo = x.DE_CARGO
                            ,
                            Nombre = x.NM_CARGO
                            ,
                            CargoPadreSeleccionado = x.CD_CARGO_PADRE
                            ,
                            NombreUnidadAdministrativa = x.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA
                            ,
                            UnidadAdministrativaSeleccionada = x.CD_UNIDAD_ADMINISTRATIVA
                            
                            ,RolesSeleccionados = (from y in x.ROL
                                                   select y.CD_ROL)

                        }
                        ).FirstOrDefault();
            }
        }





        /// <summary>
        /// Retorna los empleados registrados en el sistema
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Usuario> obtenEmpleado()
        {
            IEnumerable<Usuario> result = new List<Usuario>();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                result = (from x in db.EMPLEADO
                          select new Usuario
                          {
                              Id = x.CD_USUARIO
                              ,
                              DescripcionBloqueado = x.USUARIO.IN_USUARIO_BLOQUEADO ? Resources.DescripcionResource.Bloqueado : ""
                              ,
                              DescripcionEstatus = x.USUARIO.IN_USUARIO_INACTIVO ? Resources.DescripcionResource.Inactivo  : Resources.DescripcionResource.Activo
                              ,
                              Email = x.USUARIO.DI_EMAIL_USUARIO
                              ,
                              FechaCreacion = x.USUARIO.FE_CREACION
                              ,
                              FechaEstatus = x.USUARIO.FE_ESTATUS
                              ,
                              FechaNacimiento = x.USUARIO.FE_NACIMIENTO
                              ,
                              IndicadorActivo = !x.USUARIO.IN_USUARIO_INACTIVO
                              ,
                              IndicadorBloqueado = x.USUARIO.IN_USUARIO_BLOQUEADO
                              ,
                              NombreApellido = x.USUARIO.DE_NOMBRE_APELLIDO
                              ,
                              NombreUsuario = x.USUARIO.NM_USUARIO
                              ,
                              NumeroDocumento = x.USUARIO.NU_DOCUMENTO_IDENTIDAD
                              ,
                              TelefonoFijo = x.USUARIO.NU_TELEFONO_FIJO
                              ,
                              TelefonoMovil = x.USUARIO.NU_TELEFONO_MOVIL
                              ,
                              TipoDocumentoSeleccionado = x.USUARIO.TP_DOCUMENTO_IDENTIDAD
                              ,DatosEmpleado = new Empleado() {
                                   Id                   = x.CD_USUARIO
                                  ,NombreCargo          = x.CARGO.NM_CARGO
                                  ,NombreSucursal       = x.SUCURSAL.DE_SUCURSAL
                                  ,NombreSupervisor     = x.EMPLEADO2!= null? x.EMPLEADO2.USUARIO.DE_NOMBRE_APELLIDO : ""   
                                  ,FechaRetiro          = x.FE_RETIRO                               
                              }
                          }
                          ).ToList();
            }


            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() { { "data-id-usuario", x.Id.ToString() } }
                                                                                                                           , true, true, false));

            return result;

        }


        /// <summary>
        /// Obtiene el objeto Empleado dado el Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Empleado obtenEmpleado_ById(int Id)
        {
            Empleado empleado;
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                string contrasena = "cambiar*001";

                empleado = (from x in db.EMPLEADO
                           where x.CD_USUARIO == Id                           
                           select new Empleado()
                           {
                               Id = x.USUARIO.CD_USUARIO
                                 ,
                               Email = x.USUARIO.DI_EMAIL_USUARIO
                                 ,
                               FechaCreacion = x.USUARIO.FE_CREACION
                                 ,
                               FechaEstatus = x.USUARIO.FE_ESTATUS
                                 ,
                               FechaNacimiento = x.USUARIO.FE_NACIMIENTO
                                 ,
                               IndicadorActivo = !x.USUARIO.IN_USUARIO_INACTIVO
                                 ,
                               IndicadorBloqueado = x.USUARIO.IN_USUARIO_BLOQUEADO
                                 ,
                               DescripcionEstatus = !x.USUARIO.IN_USUARIO_INACTIVO ? Resources.DescripcionResource.Si : Resources.DescripcionResource.No
                                 ,
                               DescripcionBloqueado = x.USUARIO.IN_USUARIO_BLOQUEADO ? Resources.DescripcionResource.Si : Resources.DescripcionResource.No
                                 ,
                               NombreApellido = x.USUARIO.DE_NOMBRE_APELLIDO
                                 ,
                               NombreUsuario = x.USUARIO.NM_USUARIO
                                 ,
                               NumeroDocumento = x.USUARIO.NU_DOCUMENTO_IDENTIDAD
                                 ,
                               TelefonoFijo = x.USUARIO.NU_TELEFONO_FIJO
                                 ,
                               TelefonoMovil = x.USUARIO.NU_TELEFONO_MOVIL
                                 ,
                               TipoDocumentoSeleccionado = x.USUARIO.TP_DOCUMENTO_IDENTIDAD
                                 ,
                               ContrasenaNueva = contrasena
                               ,
                               
                               RepetirContrasena = contrasena
                                 ,

                               CargoSeleccionado = x.CD_CARGO
                                                                   ,
                               NombreCargo = x.CARGO.DE_CARGO
                                                                   ,
                               EmailCompania = x.DI_CORREO
                                                                   ,
                               Extension = x.NU_EXTENSION
                                                                   ,
                               FechaIngreso = x.FE_INGRESO
                                                                   ,
                               FechaRetiro = x.FE_RETIRO
                                                                   ,
                               SucursalSeleccionada = x.CD_SUCURSAL
                                                                   ,
                               NombreSucursal = x.SUCURSAL.DE_SUCURSAL
                                                                   ,
                               SupervisorSeleccionado = x.CD_USUARIO_SUPERVISOR
                                                                   ,
                               NombreSupervisor = x.EMPLEADO2 != null ? x.EMPLEADO2.USUARIO.DE_NOMBRE_APELLIDO : ""
                                                                   ,
                               UnidadAdministrativaSeleccionada = x.CARGO.CD_UNIDAD_ADMINISTRATIVA
                                                                   ,
                               NombreUnidadAdministrativa = x.CARGO.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA

                           }
                    ).FirstOrDefault();
            }

         
            empleado.Aplicaciones = obtenAplicacionesUsuario_ByUsuarioId(empleado.Id);
            
            return empleado;
        }




        /// <summary>
        /// Obtiene el objeto Usuario dado el Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UsuarioAdmin obtenUsuario_ById(int? IdUsuarioPadre, int Id) {
            UsuarioAdmin usuario;
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                string contrasena = "cambiar*001";
                int IdUsuarioPadreInt = IdUsuarioPadre != null ? (int)IdUsuarioPadre : 0;
                usuario = (from x in db.USUARIO
                        where x.CD_USUARIO == Id
                        && (x.USUARIO2.Select(y => y.CD_USUARIO).Contains(IdUsuarioPadreInt)
                        || IdUsuarioPadreInt == 0
                        )
                        select new UsuarioAdmin() {
                                Id                          = x.CD_USUARIO
                              , Email                       = x.DI_EMAIL_USUARIO
                              , FechaCreacion               = x.FE_CREACION
                              , FechaEstatus                = x.FE_ESTATUS
                              , FechaNacimiento             = x.FE_NACIMIENTO
                              , IndicadorActivo             = !x.IN_USUARIO_INACTIVO
                              , IndicadorBloqueado          = x.IN_USUARIO_BLOQUEADO
                              , DescripcionEstatus          = !x.IN_USUARIO_INACTIVO?Resources.DescripcionResource.Si: Resources.DescripcionResource.No
                              , DescripcionBloqueado        = x.IN_USUARIO_BLOQUEADO? Resources.DescripcionResource.Si : Resources.DescripcionResource.No
                              , NombreApellido              = x.DE_NOMBRE_APELLIDO
                              , NombreUsuario               = x.NM_USUARIO
                              , NumeroDocumento             = x.NU_DOCUMENTO_IDENTIDAD
                              , TelefonoFijo                = x.NU_TELEFONO_FIJO
                              , TelefonoMovil               = x.NU_TELEFONO_MOVIL
                              , TipoDocumentoSeleccionado   = x.TP_DOCUMENTO_IDENTIDAD
                              , ContrasenaNueva             = contrasena
                              , CodigoUsuarioPadre          = IdUsuarioPadre
                              , IndicadorRegistroNoEditable = (x.SOCIO_COMERCIAL != null && !x.IN_USUARIO_INACTIVO)
                                                            || (x.EMPLEADO != null && x.EMPLEADO.FE_RETIRO == null)
                                                            || (x.USUARIO2.Count() > 1) ? true : false
                              , RepetirContrasena           = contrasena
                              , DispositivosSeleccionados   = x.DISPOSITIVO.Select(y => y.ID_DISPOSITIVO)
                              , DatosEmpleado               = x.EMPLEADO!=null?new Empleado() {
                                                                Id = x.EMPLEADO.CD_USUARIO
                                                                ,CargoSeleccionado = x.EMPLEADO.CD_CARGO
                                                                ,NombreCargo = x.EMPLEADO.CARGO.DE_CARGO
                                                                ,Email = x.EMPLEADO.DI_CORREO
                                                                ,Extension = x.EMPLEADO.NU_EXTENSION
                                                                ,FechaIngreso = x.EMPLEADO.FE_INGRESO
                                                                ,FechaRetiro = x.EMPLEADO.FE_RETIRO
                                                                ,SucursalSeleccionada = x.CD_USUARIO
                                                                ,NombreSucursal = x.EMPLEADO.SUCURSAL.DE_SUCURSAL
                                                                ,SupervisorSeleccionado = x.EMPLEADO.CD_USUARIO_SUPERVISOR
                                                                ,NombreSupervisor = x.EMPLEADO.EMPLEADO2!=null?x.EMPLEADO.EMPLEADO2.USUARIO.DE_NOMBRE_APELLIDO:""
                                                                ,UnidadAdministrativaSeleccionada = x.EMPLEADO.CARGO.CD_UNIDAD_ADMINISTRATIVA
                                                                ,NombreUnidadAdministrativa = x.EMPLEADO.CARGO.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA
                              } : null

                        }
                    ).FirstOrDefault();
                }

            // usuario.Roles = obtenRoles_ByCargoUsuario(usuario.Id, usuario.DatosEmpleado != null ? (int?)usuario.DatosEmpleado.CargoSeleccionado : null);
                if (IdUsuarioPadre == null){
                    usuario.Aplicaciones = obtenAplicacionesUsuario_ByUsuarioId(usuario.Id);
                }
                else {
                    usuario.Aplicaciones = obtenAplicacionesUsuarioInterno_ById((int)IdUsuarioPadre, usuario.Id).Aplicaciones;
                }
            return usuario;
            }


        /// <summary>
        /// Obtiene el objeto SocioComercial dado el Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SocioComercial obtenSocioComercial_ById(int Id) {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (from x in db.SOCIO_COMERCIAL
                        where x.CD_SOCIO_COMERCIAL == Id
                        select new SocioComercial()
                        {
                            Id = x.CD_SOCIO_COMERCIAL
                            ,Calle = x.DI_OFICINA
                            ,CorregimientoSeleccionado  = x.CD_CORREGIMIENTO
                            ,DescripcionCorregimiento   = x.CD_CORREGIMIENTO!= null?x.CORREGIMIENTO.NM_CORREGIMIENTO:""
                            ,DescripcionEstatus         = x.IN_INACTIVO ? Resources.DescripcionResource.Inactivo : Resources.DescripcionResource.Activo
                            ,DescripcionDistrito        = x.DISTRITO.NM_DISTRITO
                            ,DistritoSeleccionado       = x.CD_DISTRITO
                            ,DescripcionPais            = x.DISTRITO.PROVINCIA.PAIS.NM_PAIS
                            ,PaisSeleccionado           = x.CD_PAIS
                            ,IndicadorCorreoRegistroEnviado = x.USUARIO.IN_CORREO_REGISTRO_ENVIADO
                            ,DescripcionProvincia       = x.DISTRITO.PROVINCIA.NM_PROVINCIA
                            ,ProvinciaSeleccionada      = x.CD_PROVINCIA
                            ,Email                      = x.DI_EMAIL
                            ,FechaCreacion              = x.FE_CREACION
                            ,FechaEstatus               = x.FE_ESTATUS
                            ,Nombre                     = x.NM_SOCIO_COMERCIAL
                            ,TelefonoFijo               = x.NU_TELEFONO_FIJO
                            ,TelefonoMovil              = x.NU_TELEFONO_MOVIL
                            ,TipoDocumentoSeleccionado  = x.USUARIO.TP_DOCUMENTO_IDENTIDAD
                            ,NumeroDocumento            = x.USUARIO.NU_DOCUMENTO_IDENTIDAD
                            ,IndicadorActivo            = !x.IN_INACTIVO
                            ,DescripcionSegmentoNegocio = x.SEGMENTO_NEGOCIO.NM_SEGMENTO_NEGOCIO
                            ,NombreUsuario              = x.USUARIO.NM_USUARIO
                            ,SegmentoNegocioSeleccionado = x.CD_SEGMENTO_NEGOCIO
                            ,ServiciosAppSeleccionados  = x.USUARIO.MENU_SERVICIO.Select(y => y.CD_MENU_SERVICIO)
                            ,DispositivosSeleccionados  = x.USUARIO.DISPOSITIVO.Select(y => y.ID_DISPOSITIVO) 
                            ,DescripcionDistribuidor    = (x.ID_DISTRIBUIDOR!=null?x.DISTRIBUIDOR.NM_DISTRIBUIDOR:null)
                            ,DistribuidorSeleccionado   = x.ID_DISTRIBUIDOR
                        }).FirstOrDefault();

            }

        }


       



        /// <summary>
        /// Retorna el rol dado el id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Rol obtenRolAplicacion_ById(int Id)
        {
            Rol rolAplicacion;

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                rolAplicacion = (from x in db.ROL
                                 where x.CD_ROL == Id
                                 select new Rol() {
                                      Id = x.CD_ROL
                                     ,Nombre = x.NM_ROL
                                     ,Descripcion = x.DE_ROL
                                     ,FechaCreacion = x.FE_CREACION
                                     ,FechaEstatus = x.FE_ESTATUS
                                     ,IndicadorActivo = x.IN_ROL_ACTIVO                                     
                                 }).FirstOrDefault();


                rolAplicacion.Aplicaciones = obtenAplicacionesRol_ByRolId(Id);
                
            }
            return rolAplicacion;
       }



        /// <summary>
        /// Obtiene el usuario para darle de baja
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EmpleadoBaja obtenEmpleadoBaja_ById(int Id) {

            EmpleadoBaja result= null;
            
            EMPLEADO usu = obten_EMPLEADO_ById(Id, new List<Expression<Func<EMPLEADO, object>>> { (x => x.USUARIO)
                                                                                                 ,(x => x.CARGO)
                                                                                                 ,(x => x.CARGO.UNIDAD_ADMINISTRATIVA)});

            if (usu != null) {
                result = new EmpleadoBaja();
                result.Id               = usu.USUARIO.CD_USUARIO;
                result.NombreUsuario    = usu.USUARIO.NM_USUARIO;
                result.NombreApellido   = usu.USUARIO.DE_NOMBRE_APELLIDO;
                result.Cargo            = usu.CARGO.DE_CARGO;
                result.CodigoCargo      = usu.CD_CARGO;
                result.UnidadAdministrativa = usu.CARGO.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA;

            }

            return result;
        }

        /// <summary>
        /// Busca todos los parámetros de configuración
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParametroConfiguracion> obtenParametroConfiguracion() {
            IEnumerable<ParametroConfiguracion> result = new List<ParametroConfiguracion>();

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                result = (from x in db.PARAMETRO_CONFIGURACION
                          orderby x.NM_PARAMETRO ascending
                          select new ParametroConfiguracion()
                          {
                                CodigoConfiguracion = x.CD_CONFIGURACION
                              , NombreParametro  = x.NM_PARAMETRO
                              , Descripcion = x.DE_CONFIGURACION
                              , Uso  = x.DE_USO_CONFIGURACION
                          }).ToList();
            }

            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() {  {"data-cd-configuracion" , x.CodigoConfiguracion}
                                                                                                                                ,{"data-nm-parametro" , x.NombreParametro } }
                            , false, true, true));

            return result;

        }
        

        /// <summary>
        /// Busca todas las unidades administrativas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UnidadAdministrativa> obtenUnidadAdministrativa()
        {
            IEnumerable<UnidadAdministrativa> result = new List<UnidadAdministrativa>();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                result = (from x in db.UNIDAD_ADMINISTRATIVA
                          orderby x.NM_UNIDAD_ADMINISTRATIVA 
                          select new UnidadAdministrativa()
                          {
                              Id = x.CD_UNIDAD_ADMINISTRATIVA
                              ,
                              Nombre = x.NM_UNIDAD_ADMINISTRATIVA
                              ,
                              FechaCreacion = x.FE_CREACION
                          }).ToList();
            }

            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() {  {"data-id" , x.Id.ToString()}}
                            , false, true, true));

            return result;

        }


        /// <summary>
        /// Obtiene objeto RespuestaOperadorGCM by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RespuestaOperadorGCM obtenRespuestaOperadorGCM_ById(int id) {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.RESPUESTA_OPERADOR_GCM
                        where x.ID_RESPUESTA_OPERADOR == id
                        select new RespuestaOperadorGCM()
                        {
                            Id = x.ID_RESPUESTA_OPERADOR
                            ,Contenido = x.DE_CONTENIDO
                            ,FeEstatus = x.FE_ESTATUS
                            ,InActivo = !x.IN_INACTIVO
                            ,TipoAtencionSeleccionada = x.TP_ATENCION
                            ,Titulo = x.DE_TITULO
                        }).FirstOrDefault();
            }
        }





        /// <summary>
        /// Dado el id obtiene el objeto RESPUESTA_OPERADOR_GCM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RESPUESTA_OPERADOR_GCM obten_RESPUESTA_OPERADOR_GCM_ById(int id)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.RESPUESTA_OPERADOR_GCM
                        where x.ID_RESPUESTA_OPERADOR == id
                        select x).FirstOrDefault();
            }
        }

        /// <summary>
        /// Dado el nmParametro y el cdConfiguracion obtiene el objeto PARAMETRO_CONFIGURACION
        /// </summary>
        /// <param name="nmParametro"></param>
        /// <param name="cdConfiguracion"></param>
        /// <returns></returns>
        public PARAMETRO_CONFIGURACION obten_PARAMETRO_CONFIGURACION_ById(string nmParametro
                                                                          ,string cdConfiguracion  )
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.PARAMETRO_CONFIGURACION
                        where x.CD_CONFIGURACION == cdConfiguracion
                        && x.NM_PARAMETRO == nmParametro
                        select x).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retorna el cargo dado el Id
        /// </summary>
        /// <param name="cdCargo">Id del cargo</param>
        /// <returns></returns>
        public CARGO obten_CARGO_ById(int Id)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (
                        from x in db.CARGO
                        where x.CD_CARGO == Id
                        select x
                    ).FirstOrDefault();
            }

        }


        /// <summary>
        /// Retorna el objeto empleado dado el Id
        /// </summary>
        /// <param name="Id">Id del empleado</param>
        /// <returns></returns>
        public EMPLEADO obten_EMPLEADO_ById(int Id) {
            return obten_EMPLEADO_ById(Id, null);
        }


        /// <summary>
        /// Devuelve el objeto TICKET  que corresponda con el id, con susu respectivos include
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes">lista de includes que requieo</param>
        /// <returns></returns>
        public EMPLEADO obten_EMPLEADO_ById(int Id, IList<Expression<Func<EMPLEADO, object>>> includes)
        {
            using (var db = new IntranetSAIEntities())
            {
                var empleado = (from x in db.EMPLEADO
                              where x.CD_USUARIO == Id
                              select x);

                if (includes != null)
                    includes.ToList().ForEach(x => empleado = empleado.Include(x));

                return empleado.FirstOrDefault();
            }
        }


        /// <summary>
        /// Retorna la lista de empleados que tienen un cargo específico
        /// </summary>
        /// <param name="cdCargo"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEnumerable<EMPLEADO> obten_EMPLEADO_ByCargo(int cdCargo, IList<Expression<Func<EMPLEADO, object>>> includes) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                var empleados =  (from x in db.EMPLEADO
                        where x.CD_CARGO == cdCargo
                        select x);

                if (includes != null)
                    includes.ToList().ForEach(x => empleados = empleados.Include(x));

                return empleados.ToList();
            }
        }


        /// <summary>
        /// Retorna la lista de empleados que tienen un cargo específico
        /// </summary>
        /// <param name="cdCargo"></param>
        /// <returns></returns>
        public IEnumerable<EMPLEADO> obten_EMPLEADO_ByCargo(int cdCargo)
        {
            return obten_EMPLEADO_ByCargo(cdCargo, null);
        }




        /// <summary>
        /// Obtiene el listado del log de transacciones
        /// </summary>
        /// <returns></returns>
        public IList<LogTransacciones> obtenLogTransacciones( int? cdUsuario
                                                             ,DateTime? feDesde
                                                             ,DateTime? feHasta
                                                             ,string nivel) {
            if (feHasta != null)
                feHasta = feHasta.Value.AddDays(1).AddSeconds(-1);

            IList<LogTransacciones> result = new List<LogTransacciones>();
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                result = (from x in db.LOG_TRANSACCIONES
                          where ( cdUsuario == null
                        ||cdUsuario == x.CD_USUARIO)
                        &&(feDesde == null 
                        || x.FE_OPERACION >= feDesde
                        )&&(
                            feHasta == null
                         ||x.FE_OPERACION<=feHasta    
                          
                        )&&(
                              nivel== null|| nivel==""|| nivel == x.NM_LEVEL
                        )
                        select new LogTransacciones
                        {
                             Id = x.ID_LOG
                            ,Accion = x.NM_ACTION
                            ,NombreUsuario = x.USUARIO.DE_NOMBRE_APELLIDO
                            ,Controlador = x.NM_CONTROLLER
                            ,DescripcionNivel = x.NM_LEVEL
                            ,Mensaje = x.DE_MESSAGE
                            ,FechaOperacion = x.FE_OPERACION
                            ,Ip = x.DI_IP
                            ,Url = x.NM_URL
                        }).ToList();
            }

            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() { { "data-log-id", x.Id.ToString()} }
                                                                                                                              , true ,false, false));
            return result;
        }


        /// <summary>
        /// Guarda la respuesta del operador por GCM
        /// </summary>
        /// <param name="respuestaOperadorGCM"></param>
        /// <returns>Cantidad de registros guardados</returns>
        public int guarda_RESPUESTA_OPERADOR_GCM(RESPUESTA_OPERADOR_GCM respuestaOperadorGCM)
        {

            using (var db = new IntranetSAIEntities()){
                db.RESPUESTA_OPERADOR_GCM.Add(respuestaOperadorGCM);
                return db.SaveChanges();
            }
        }
        
        /// <summary>
        /// Guarda la Unidad Administrativa
        /// </summary>
        /// <param name="unidadAdministrativa"></param>
        /// <returns>Id del registro creado</returns>
        public int guarda_UNIDAD_ADMINISTRATIVA(UNIDAD_ADMINISTRATIVA unidadAdministrativa)
        {
            int cantReg; 
            using (var db = new IntranetSAIEntities())
            {
                db.UNIDAD_ADMINISTRATIVA.Add(unidadAdministrativa);
                cantReg = db.SaveChanges();
            }

            return cantReg > 0 ? unidadAdministrativa.CD_UNIDAD_ADMINISTRATIVA : 0;
        }



        /// <summary>
        /// Método para insertar un socio comercial
        /// </summary>
        /// <param name="socioComercial"></param>
        /// <returns></returns>
        public bool guarda_SocioComercial(ref SocioComercial socioComercial) {

            bool result = false;
            int Id = socioComercial.Id;
            DateTime feActual = DateTime.Now;
            AuthRepositorio authRepo = new AuthRepositorio();
            string contrasenaNueva = Core.Utils.UtilHelper.CreateRandomPassword(8);

            ValidacionRepositorio validaRepo = new ValidacionRepositorio();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try {
                        
                        //Se guarda la información de usuario 
                        var usuario = (from x in db.USUARIO
                                       where x.CD_USUARIO == Id
                                       select x).FirstOrDefault();

                        if (usuario==null||usuario.CD_USUARIO == 0) {

                            usuario = new USUARIO();
                            usuario.CD_USUARIO = Core.Respositorios.UtilRepositorio.obtenSecuenciaUsuario();

                            String nmUsuario = socioComercial.Email.Split('@')[0];
                            if (!validaRepo.usuarioExiste(Id,nmUsuario))
                                usuario.NM_USUARIO = nmUsuario;
                            else{
                                int cont = 1;
                                while (validaRepo.usuarioExiste(Id, nmUsuario + cont)) { cont++; }
                                usuario.NM_USUARIO = nmUsuario+cont;
                            }
                            usuario.IN_USUARIO_BLOQUEADO = false;
                            usuario.IN_USUARIO_INACTIVO = false;
                            usuario.IN_REGISTRO_MOVIL = true;
                            usuario.DE_CONTRASENA = authRepo.encriptaCadena(contrasenaNueva);
                            usuario.DE_NOMBRE_APELLIDO = socioComercial.Nombre;
                            usuario.DI_EMAIL_USUARIO = socioComercial.Email;
                            usuario.FE_CREACION = feActual;
                            usuario.FE_ESTATUS = feActual;
                            usuario.NU_DOCUMENTO_IDENTIDAD = socioComercial.NumeroDocumento;
                            usuario.NU_TELEFONO_FIJO = socioComercial.TelefonoFijo;
                            usuario.NU_TELEFONO_MOVIL = socioComercial.TelefonoMovil;
                            usuario.TP_DOCUMENTO_IDENTIDAD = socioComercial.TipoDocumentoSeleccionado;
                            

                            db.USUARIO.Add(usuario);

                            db.SaveChanges();
                        }
                        else {

                            usuario.IN_USUARIO_BLOQUEADO = false;
                            usuario.IN_USUARIO_INACTIVO = false;
                            usuario.FE_ESTATUS = feActual;

                            db.USUARIO.Attach(usuario);
                            db.Entry(usuario).State = EntityState.Modified;

                            db.SaveChanges();
                        }

                        //Se guarda la información del socio
                        socioComercial.Id = usuario.CD_USUARIO;
                        socioComercial.IndicadorActivo = true;
                        SOCIO_COMERCIAL socioComercialDB = socioComercial.toModel();
                        socioComercialDB.USUARIO = usuario;

                        db.SOCIO_COMERCIAL.Add(socioComercialDB);

                        db.SaveChanges();

                        IEnumerable<int> serviciosSeleccionados = socioComercial.ServiciosAppSeleccionados;
                        serviciosSeleccionados = serviciosSeleccionados == null ? new List<int>() : serviciosSeleccionados;

                        //Se ingresan/actualizan las aplicaciones móviles 
                        List<MENU_SERVICIO> menuServicio = (from x in db.MENU_SERVICIO
                                                                    where !x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuario.CD_USUARIO)
                                                                    && serviciosSeleccionados.Contains(x.CD_MENU_SERVICIO)
                                                                    select x).ToList();

                        menuServicio.ForEach(x => usuario.MENU_SERVICIO.Add(x));
                        
                        db.USUARIO.Attach(usuario);
                        db.Entry(usuario).State = EntityState.Modified;

                        db.SaveChanges();

                        //Se ingresan los dispositivos
                        IEnumerable<int> dispositivosSeleccionados = socioComercial.DispositivosSeleccionados;
                        dispositivosSeleccionados = dispositivosSeleccionados == null ? new List<int>() : dispositivosSeleccionados;
                        List<DISPOSITIVO> dispositivosUsuario = (from x in db.DISPOSITIVO
                                                                        where !x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuario.CD_USUARIO)
                                                                        && dispositivosSeleccionados.Contains(x.ID_DISPOSITIVO)
                                                                        select x
                                                                        ).ToList();

                        dispositivosUsuario.ForEach(x => usuario.DISPOSITIVO.Add(x));
                        

                        db.USUARIO.Attach(usuario);
                        db.Entry(usuario).State = EntityState.Modified;

                        db.SaveChanges();


                        //Se buscan los roles que deban serle otorgados
                        List<ROL> roles = (from x in db.ROL
                                                  from y in db.PARAMETRO_CONFIGURACION
                                                  where !x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuario.CD_USUARIO)
                                                  && y.CD_CONFIGURACION == x.CD_ROL.ToString()
                                                  && y.NM_PARAMETRO == "ROL_SOCIO_COMERCIAL"
                                                  select x
                                                  ).ToList();

                        roles.ForEach(x=>usuario.ROL.Add(x));

                        db.USUARIO.Attach(usuario);
                        db.Entry(usuario).State = EntityState.Modified;

                        db.SaveChanges();
                        

                        //Se actualizan las aplicaciones
                        foreach (var aplicacionSocio in socioComercial.Aplicaciones) {

                            var aplicacionDB = (from x in db.USUARIO_APLICACION
                                                where x.CD_APLICACION == aplicacionSocio.Id
                                                && x.CD_USUARIO == usuario.CD_USUARIO
                                                select x).FirstOrDefault();

                            if (aplicacionDB != null){
                                
                                aplicacionDB.FE_ESTATUS = feActual;
                                aplicacionDB.IN_INACTIVO = !aplicacionSocio.IndicadorActiva;
                               
                                db.USUARIO_APLICACION.Attach(aplicacionDB);
                                db.Entry(aplicacionDB).State = EntityState.Modified;

                                db.SaveChanges();
                            }                            
                        }

                        dbTransaction.Commit();
                        result = true;
                    }
                    catch(System.Exception exc ){
                        log.Error(exc);
                        dbTransaction.Rollback();
                        result = false;
                    }
                }
            }

          return result;
        }

        /// <summary>
        /// Guarda el socio comercial
        /// </summary>
        /// <param name="socioComercial"></param>
        /// <returns></returns>
        public int guarda_SOCIO_COMERCIAL(SOCIO_COMERCIAL socioComercial)
        {
            int cantReg;
            using (var db = new IntranetSAIEntities())
            {
                db.SOCIO_COMERCIAL.Add(socioComercial);
                cantReg = db.SaveChanges();
            }

            return cantReg > 0 ? socioComercial.CD_SOCIO_COMERCIAL : 0;
        }


        /// <summary>
        /// Guarda el proveedor
        /// </summary>
        /// <param name="socioComercial"></param>
        /// <returns></returns>
      /*  public int guarda_PROVEEDOR(PROVEEDOR proveedor)
        {
            int cantReg;
            using (var db = new IntranetSAIEntities())
            {
                db.PROVEEDOR.Add(proveedor);
                cantReg = db.SaveChanges();
            }

            return cantReg > 0 ? proveedor.CD_PROVEEDOR : 0;
        }
        */

        /// <summary>
        /// Permite guardar un nuevo usuario en el sistema
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int guarda_Usuario(UsuarioAdmin usuario) {
            USUARIO usuarioFinal = new USUARIO();
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                using (var transaction = db.Database.BeginTransaction()){
                    try{

                        DateTime fechaActual = DateTime.Now;
                        usuario.FechaCreacion = fechaActual;
                        usuario.FechaEstatus = fechaActual;
                        usuario.Id = Core.Respositorios.UtilRepositorio.obtenSecuenciaUsuario();
                        
                        usuarioFinal = usuario.toModel();
                        usuarioFinal.IN_REGISTRO_MOVIL = true;

                        db.USUARIO.Add(usuarioFinal);
                        db.SaveChanges();
                       
                        //Se asocian los roles al usuario
                        ICollection<ROL> roles = (from x in usuario.RolesSeleccionados
                                                  from y in db.ROL
                                                  where y.IN_ROL_ACTIVO == true
                                                  && x == y.CD_ROL
                                                  select y).ToList();

                        usuarioFinal.ROL = roles;

                        

                        db.SaveChanges();

                   
                        IEnumerable<Aplicacion> app = usuario.Aplicaciones.AsEnumerable();

                        foreach (USUARIO_APLICACION usuarioApp in (from x in db.USUARIO_APLICACION
                                                                   where x.CD_USUARIO == usuarioFinal.CD_USUARIO
                                                                   select x 
                                                                   )) {

                            if ((from x in app select x.Id ).Contains(usuarioApp.CD_APLICACION)){

                                usuarioApp.IN_INACTIVO = (from x in usuario.Aplicaciones
                                                          where x.Id == usuarioApp.CD_APLICACION
                                                          select !x.IndicadorActiva).FirstOrDefault();

                                db.USUARIO_APLICACION.Attach(usuarioApp);
                                db.Entry(usuarioApp).State = EntityState.Modified;
                                
                                db.SaveChanges();
                            }
                        }
                       
                        transaction.Commit();
                    }
                    catch (System.Exception exc) {
                        transaction.Rollback();
                        throw exc;
                    }
                }
            }
            return usuarioFinal.CD_USUARIO;
        }


        /// <summary>
        /// Guarda la información del usuario interno
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool guarda_UsuarioInterno(ref Usuario usuario, int CodigoUsuarioPadre)
        {
            bool result = false;
            int Id = usuario.Id;
            String contrasenaNueva = usuario.ContrasenaNueva;
            DateTime feActual = DateTime.Now;

            AuthRepositorio authRepo = new AuthRepositorio();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Se guarda la información de usuario 
                        var usuarioBD = (from x in db.USUARIO
                                       where x.CD_USUARIO == Id
                                       select x).FirstOrDefault();

                        if (usuarioBD == null || usuarioBD.CD_USUARIO == 0)
                        {

                            usuarioBD = new USUARIO();
                            usuarioBD.CD_USUARIO = Core.Respositorios.UtilRepositorio.obtenSecuenciaUsuario();
                            usuarioBD.NM_USUARIO = usuario.NombreUsuario;
                            usuarioBD.IN_USUARIO_BLOQUEADO = false;
                            usuarioBD.IN_USUARIO_INACTIVO = false;
                            usuarioBD.IN_REGISTRO_MOVIL = true;
                            usuarioBD.FE_NACIMIENTO = usuario.FechaNacimiento;
                            usuarioBD.DE_CONTRASENA = authRepo.encriptaCadena(contrasenaNueva);
                            usuarioBD.DE_NOMBRE_APELLIDO = usuario.NombreApellido;
                            usuarioBD.DI_EMAIL_USUARIO = usuario.Email;
                            usuarioBD.FE_CREACION = feActual;
                            usuarioBD.FE_ESTATUS = feActual;
                            usuarioBD.NU_DOCUMENTO_IDENTIDAD = usuario.NumeroDocumento;
                            usuarioBD.NU_TELEFONO_FIJO = usuario.TelefonoFijo;
                            usuarioBD.NU_TELEFONO_MOVIL = usuario.TelefonoMovil;
                            usuarioBD.TP_DOCUMENTO_IDENTIDAD = usuario.TipoDocumentoSeleccionado;

                            Id = usuarioBD.CD_USUARIO;

                            db.USUARIO.Add(usuarioBD);

                            db.SaveChanges();
                        }
                        else{

                            usuarioBD.FE_NACIMIENTO = usuario.FechaNacimiento;
                            usuarioBD.DE_NOMBRE_APELLIDO = usuario.NombreApellido;
                            usuarioBD.DI_EMAIL_USUARIO = usuario.Email;
                            usuarioBD.NU_TELEFONO_FIJO = usuario.TelefonoFijo;
                            usuarioBD.NU_TELEFONO_MOVIL = usuario.TelefonoMovil;
                            usuarioBD.FE_ESTATUS = feActual;

                            db.USUARIO.Attach(usuarioBD);
                            db.Entry(usuarioBD).State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        
                        //Se le asigan los roles que puedan ser heredables 
                        List<ROL> roles = (from x in db.ROL
                                           from y in db.PARAMETRO_CONFIGURACION
                                           where !x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuarioBD.CD_USUARIO)
                                           && x.USUARIO.Select(y => y.CD_USUARIO).Contains(CodigoUsuarioPadre)
                                           && y.CD_CONFIGURACION == x.CD_ROL.ToString()
                                           && (y.NM_PARAMETRO == "ROL_CLIENTE"
                                           || y.NM_PARAMETRO == "ROL_SOCIO_COMERCIAL"
                                           )
                                           select x).ToList();

                        roles.ForEach(x => usuarioBD.ROL.Add(x));
                        db.SaveChanges();

                        //Se asigna el usuario padre

                        usuarioBD.USUARIO2.Add((from x in db.USUARIO
                                              where x.CD_USUARIO == CodigoUsuarioPadre
                                              select x).FirstOrDefault());

                        db.SaveChanges();

                        //Se actualizan las aplicaciones de usuario interno

                        usuario.Aplicaciones.ToList().ForEach(x => {
                            var aplicacionDB = (from y in db.USUARIO_APLICACION
                                                where y.CD_APLICACION == x.Id
                                                && y.CD_USUARIO == Id
                                                select y).FirstOrDefault();

                            if (aplicacionDB != null)
                            {
                                aplicacionDB.FE_ESTATUS = feActual;
                                
                                aplicacionDB.IN_INACTIVO = !x.IndicadorActiva;

                                db.SaveChanges();
                            }
                        });


                        //Se guardan los dispositivos del usuario interno
                        IEnumerable<int> dispositivosSeleccionados = usuario.DispositivosSeleccionados == null ? new List<int>() : usuario.DispositivosSeleccionados;
                        
                        List<DISPOSITIVO> dispositivosAEliminar = ( from x in db.DISPOSITIVO
                                                                where x.USUARIO.Select(y => y.CD_USUARIO).Contains(CodigoUsuarioPadre)
                                                                && x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuarioBD.CD_USUARIO)
                                                                && !dispositivosSeleccionados.Contains(x.ID_DISPOSITIVO)
                                                                select x).ToList();

                        dispositivosAEliminar.ForEach(x => usuarioBD.DISPOSITIVO.Remove(x));

                        db.SaveChanges();

                        
                        List<DISPOSITIVO> dispositivosAAgregar = (from x in db.DISPOSITIVO
                                                                   where dispositivosSeleccionados.Contains(x.ID_DISPOSITIVO)
                                                                   select x).ToList();

                        dispositivosAAgregar.ForEach(x => usuarioBD.DISPOSITIVO.Add(x));
                        db.SaveChanges();

                        usuario.Id = usuarioBD.CD_USUARIO; 

                        transaction.Commit();
                        result = true;
                    }
                    catch (System.Exception exc)
                    {
                        transaction.Rollback();
                        log.Error(exc);
                        result = false;
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// Guarda la información del nuevo empleado
        /// </summary>
        /// <param name="empleado"></param>
        /// <returns></returns>
        public bool guarda_Empleado(ref Empleado empleado) {

            bool result = false;
            int Id = empleado.Id;
            String contrasenaNueva = empleado.ContrasenaNueva;
            DateTime feActual = DateTime.Now;

            AuthRepositorio authRepo = new AuthRepositorio();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Se guarda la información de usuario 
                        var usuarioBD = (from x in db.USUARIO
                                         where x.CD_USUARIO == Id
                                         select x).FirstOrDefault();

                        if (usuarioBD == null || usuarioBD.CD_USUARIO == 0)
                        {

                            usuarioBD = new USUARIO();
                            usuarioBD.CD_USUARIO = Core.Respositorios.UtilRepositorio.obtenSecuenciaUsuario();
                            usuarioBD.NM_USUARIO = empleado.NombreUsuario;
                            usuarioBD.IN_USUARIO_BLOQUEADO = false;
                            usuarioBD.IN_USUARIO_INACTIVO = false;
                            usuarioBD.IN_REGISTRO_MOVIL = true;
                            usuarioBD.FE_NACIMIENTO = empleado.FechaNacimiento;
                            usuarioBD.DE_CONTRASENA = authRepo.encriptaCadena(contrasenaNueva);
                            usuarioBD.DE_NOMBRE_APELLIDO = empleado.NombreApellido;
                            usuarioBD.DI_EMAIL_USUARIO = empleado.Email;
                            usuarioBD.FE_CREACION = feActual;
                            usuarioBD.FE_ESTATUS = feActual;
                            usuarioBD.NU_DOCUMENTO_IDENTIDAD = empleado.NumeroDocumento;
                            usuarioBD.NU_TELEFONO_FIJO = empleado.TelefonoFijo;
                            usuarioBD.NU_TELEFONO_MOVIL = empleado.TelefonoMovil;
                            usuarioBD.TP_DOCUMENTO_IDENTIDAD = empleado.TipoDocumentoSeleccionado;

                            Id = usuarioBD.CD_USUARIO;

                            db.USUARIO.Add(usuarioBD);

                            db.SaveChanges();
                        }
                        else
                        {
                            usuarioBD.IN_USUARIO_BLOQUEADO = false;
                            usuarioBD.IN_USUARIO_INACTIVO = false;
                            usuarioBD.IN_REGISTRO_MOVIL = true;
                            usuarioBD.NM_USUARIO = empleado.NombreUsuario;
                            usuarioBD.FE_NACIMIENTO = empleado.FechaNacimiento;
                            usuarioBD.DE_NOMBRE_APELLIDO = empleado.NombreApellido;
                            usuarioBD.DI_EMAIL_USUARIO = empleado.Email;
                            usuarioBD.NU_TELEFONO_FIJO = empleado.TelefonoFijo;
                            usuarioBD.NU_TELEFONO_MOVIL = empleado.TelefonoMovil;
                            usuarioBD.FE_ESTATUS = feActual;

                            db.USUARIO.Attach(usuarioBD);
                            db.Entry(usuarioBD).State = EntityState.Modified;

                            db.SaveChanges();
                        }


                        //Se crea la entidad empleado
                        EMPLEADO empleadoBD = new EMPLEADO();

                        empleadoBD.USUARIO = usuarioBD;

                        empleadoBD.CD_CARGO              = empleado.CargoSeleccionado;
                        empleadoBD.CD_SUCURSAL           = empleado.SucursalSeleccionada;
                        empleadoBD.CD_USUARIO_SUPERVISOR = empleado.SupervisorSeleccionado;
                        empleadoBD.DI_CORREO             = empleado.EmailCompania;
                        empleadoBD.FE_INGRESO            = empleado.FechaIngreso;

                        db.EMPLEADO.Add(empleadoBD);

                        db.SaveChanges();


                        normalizaRelacionSuperiorSubordinado(db, empleadoBD.CD_USUARIO);

                        //Se le asigan los roles que pertenezcan al empleado
                        IList<int> rolesSeleccionados = empleado.RolesSeleccionados == null ? new List<int>() : empleado.RolesSeleccionados.ToList();
                        IEnumerable<int> rolesNoSeleccionados   = empleado.RolesDisponibles == null ? new List<int>() : empleado.RolesDisponibles.Where(x=> !rolesSeleccionados.Contains(Convert.ToInt32(x.Value))).Select(x=> Convert.ToInt32(x.Value));


                        List<ROL> rolAEliminar = (from x in db.ROL
                                                                   where x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuarioBD.CD_USUARIO)
                                                                   && rolesNoSeleccionados.Contains(x.CD_ROL)
                                                                   select x).ToList();

                        rolAEliminar.ForEach(x => usuarioBD.ROL.Remove(x));

                        db.SaveChanges();


                        List<ROL> rolAAgregar = (from x in db.ROL
                                                                  where rolesSeleccionados.Contains(x.CD_ROL)
                                                                  select x).ToList();

                        rolAAgregar.ForEach(x => usuarioBD.ROL.Add(x));
                        db.SaveChanges();



                        //Se actualizan las aplicaciones de usuario interno
                        empleado.Aplicaciones.Where(
                             x=> x.RolesAsociados.Split('#').ToList().Intersect(rolesSeleccionados.Select(w=> w.ToString())).Any() == true                             
                            ).ToList().ForEach(x => {
                            var aplicacionDB = (from y in db.USUARIO_APLICACION
                                                where y.CD_APLICACION == x.Id
                                                && y.CD_USUARIO == Id
                                                select y).FirstOrDefault();

                            if (aplicacionDB != null)
                            {
                                aplicacionDB.FE_ESTATUS = feActual;

                                aplicacionDB.IN_INACTIVO = !x.IndicadorActiva;

                                db.SaveChanges();
                            }
                        });

                        empleado.Id = usuarioBD.CD_USUARIO;

                        transaction.Commit();
                        result = true;
                    }
                    catch (System.Exception exc)
                    {
                        transaction.Rollback();
                        log.Error(exc);
                        result = false;
                    }
                }
            }
            return result;
        }

                
        /// <summary>
        /// Permite crear un nuevo cargo
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        public int guarda_Cargo(Cargo cargo) {
            CARGO cargoFinal = new CARGO();
            DateTime fechaActual = DateTime.Now;
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                using (var transaction = db.Database.BeginTransaction()) {
                    try {

                        cargoFinal = cargo.toModel();
                        cargoFinal.FE_CREACION = fechaActual;

                        cargoFinal.UNIDAD_ADMINISTRATIVA = (from x in db.UNIDAD_ADMINISTRATIVA
                                                            where x.CD_UNIDAD_ADMINISTRATIVA == cargoFinal.CD_UNIDAD_ADMINISTRATIVA
                                                            select x).FirstOrDefault();

                        cargoFinal.ROL = (from x in db.ROL
                                          where cargo.RolesSeleccionados.Contains(x.CD_ROL)
                                          select x).ToList() ;
                                          
                        db.CARGO.Add(cargoFinal);
                        
                        db.SaveChanges();
                        
                        transaction.Commit();
                    } catch (System.Exception exc) {
                        transaction.Rollback();
                        throw exc;
                    }

                }
            }
            return cargoFinal.CD_CARGO;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        public int guarda_Rol(Rol rol) {
            int cantReg = 0, id=0;
            DateTime fechaActual = DateTime.Now;
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                ROL rolModel = rol.toModel();
                rolModel.FE_CREACION = fechaActual;
                rolModel.FE_ESTATUS = fechaActual;

                //Se coloca las aplicaciones seleccionadas por el usuario
                rol.Aplicaciones.ToList().ForEach(x => {
                    if (x.IndicadorActiva) {
                        rolModel.APLICACION.Add(
                            (from y in db.APLICACION
                             where y.CD_APLICACION == x.Id
                             && y.IN_APLICACION_INACTIVA == false
                             select  y).FirstOrDefault()
                            );
                    }
                });

                db.ROL.Add(rolModel);
                cantReg = db.SaveChanges();
                id = rolModel.CD_ROL;
            }
                return cantReg>0? id:0;
        }


        /// <summary>
        /// Actualiza la información de un cargo
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        public int actualizaCargo(Cargo cargo) {
            int totalActualizado = 0;


            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var transaction = db.Database.BeginTransaction())
                {

                    try
                    {                       
                        CARGO cargoFinal = (from x in db.CARGO
                                            where x.CD_CARGO == cargo.Id
                                            select x).FirstOrDefault();

                        if (cargoFinal != null){
                            CARGO cargoModel = cargo.toModel();
                            //Se actualizan los roles 

                            //Se eliminan los que no esten en la lista de seleccionados
                            cargoFinal.ROL.ToList().ForEach(x => {
                                if (!cargo.RolesSeleccionados.Contains(x.CD_ROL))
                                    cargoFinal.ROL.Remove(x);                                
                            });

                            db.SaveChanges();

                            //Se actualiza el cargo padre
                            cargo.RolesSeleccionados.ToList().ForEach(
                                x => {
                                    if (!(from y in cargoFinal.ROL
                                          select y.CD_ROL).Contains(x))
                                    {
                                        cargoFinal.ROL.Add((from y in db.ROL
                                                            where y.CD_ROL == x
                                                            select y).FirstOrDefault());
                                    }
                                }                                
                                );
                            db.SaveChanges();


                            //Se actualizan los supervisores de los empleados, si es necesario
                            if (cargoFinal.CD_CARGO_PADRE != cargo.CargoPadreSeleccionado)
                            {
                                EMPLEADO nuevoSupervisor = null;
                                if (cargo.CargoPadreSeleccionado != null) { 
                                //Se busca al empleado con el nuevo cargo supervisor
                                int CargoPadre = (int)cargo.CargoPadreSeleccionado;
                                IEnumerable<EMPLEADO> nuevoSupervisorList = obten_EMPLEADO_ByCargo(CargoPadre);

                                while (nuevoSupervisorList.Count() == 0)
                                {
                                    //Se busca el cargo supervisor 
                                    CARGO cargoPadreRepo = obten_CARGO_ById(CargoPadre);
                                    if (cargoPadreRepo == null) break;
                                    else
                                    {
                                        CargoPadre = cargoPadreRepo.CD_CARGO_PADRE != null ? (int)cargoPadreRepo.CD_CARGO_PADRE : 0;
                                        nuevoSupervisorList = obten_EMPLEADO_ByCargo((int)CargoPadre);
                                    }
                                }

                                nuevoSupervisor = nuevoSupervisorList.FirstOrDefault();
                            }

                                IEnumerable<EMPLEADO> empleadosNuevoSupervidor =
                                                            (from x in db.EMPLEADO
                                                            where x.CD_CARGO == cargoFinal.CD_CARGO
                                                            select x);


                                empleadosNuevoSupervidor.ToList().ForEach(x => {
                                    if (nuevoSupervisor != null)
                                        x.CD_USUARIO_SUPERVISOR = nuevoSupervisor.CD_USUARIO;
                                    else
                                        x.CD_USUARIO_SUPERVISOR = null;
                                    
                                    db.EMPLEADO.Attach(x);
                                    db.Entry(x).State = EntityState.Modified;                                    

                                });

                                db.SaveChanges();
                            }

                            //Se actualiza la información del cargo    
                            cargoFinal.NM_CARGO = cargoModel.NM_CARGO;
                            cargoFinal.CD_UNIDAD_ADMINISTRATIVA = cargoModel.CD_UNIDAD_ADMINISTRATIVA;
                            cargoFinal.CD_CARGO_PADRE = cargoModel.CD_CARGO_PADRE;
                            cargoFinal.DE_CARGO = cargoModel.DE_CARGO;


                            db.CARGO.Attach(cargoFinal);
                            db.Entry(cargoFinal).State = EntityState.Modified;


                            totalActualizado = db.SaveChanges();

                        }

                        transaction.Commit();
                    }
                    catch (System.Exception exc)
                    {
                        transaction.Rollback();
                        throw exc;
                    }
                }
            }
            return totalActualizado;
        }


        /// <summary>
        /// ctualización de l información de un rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        public int actualizaRol(Rol rol) {
            int totalActualizado = 0;
            DateTime fechaActual = DateTime.Now;
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var transaction = db.Database.BeginTransaction())
                {

                    try
                    {
                        ROL rolFinal = (from x in db.ROL
                                            where x.CD_ROL == rol.Id
                                            select x).FirstOrDefault();

                        if ( rolFinal != null)
                        {
                            ROL rolModel = rol.toModel();

                            //Se actualizan las aplicaciones asociadas
                            List<int> aplicacionesAsignadas = (from x in rol.Aplicaciones
                                                                where x.IndicadorActiva == true
                                                                select x.Id
                                                                ).ToList();
                            //Las aplicaciones no asignadas se eliminan 
                            rolFinal.APLICACION.ToList().ForEach(x => {
                                if (!aplicacionesAsignadas.Contains(x.CD_APLICACION))
                                    rolFinal.APLICACION.Remove(x);
                            });

                            //Las aplicaciones asgnadas se agregan
                            aplicacionesAsignadas.ForEach(x =>
                            {

                                if (!(from w in rolFinal.APLICACION
                                      select w.CD_APLICACION
                                    ).ToList().Contains(x))
                                {
                                    rolFinal.APLICACION.Add((from w in db.APLICACION
                                                             where w.CD_APLICACION == x
                                                             select w).FirstOrDefault() );
                                }
                            });

                            db.SaveChanges();

                            //Se actualizan los datos básicos 
                            rolFinal.FE_ESTATUS = fechaActual;
                            rolFinal.DE_ROL = rolModel.DE_ROL;
                            rolFinal.IN_ROL_ACTIVO = rolModel.IN_ROL_ACTIVO;
                            rolFinal.NM_ROL = rolModel.NM_ROL;

                            db.ROL.Attach(rolFinal);
                            db.Entry(rolFinal).State = EntityState.Modified;
                            
                            totalActualizado = db.SaveChanges();
                            
                        }
                            transaction.Commit();
                    }
                    catch (System.Exception exc)
                    {
                        transaction.Rollback();
                        throw exc;
                    }
                }
            }
            return totalActualizado;
        }



        /// <summary>
        /// Actualiza la informción del usuario
        /// </summary>
        /// <param name="usuario">Usuario a ser actualizado</param>
        /// <returns></returns>
        public int actualizaUsuario(UsuarioAdmin  usuario) {
            int totalActualizado = 0;
            using (IntranetSAIEntities db = new IntranetSAIEntities()){

                using (var transaction = db.Database.BeginTransaction()){

                    try {
                        DateTime fechaActual = DateTime.Now;

                        //Se obtiene el usuario registrado
                        USUARIO usuarioActualizar = (from x in db.USUARIO
                                                     where x.CD_USUARIO == usuario.Id
                                                     select x).FirstOrDefault();

                        if (usuarioActualizar != null) {
                            USUARIO usuarioActualizarModel = usuario.toModel();


                            usuarioActualizar.DE_NOMBRE_APELLIDO    = usuarioActualizarModel.DE_NOMBRE_APELLIDO;
                            usuarioActualizar.DI_EMAIL_USUARIO      = usuarioActualizarModel.DI_EMAIL_USUARIO;    
                            usuarioActualizar.FE_NACIMIENTO         = usuarioActualizarModel.FE_NACIMIENTO;
                            usuarioActualizar.NU_TELEFONO_FIJO      = usuarioActualizarModel.NU_TELEFONO_FIJO;
                            usuarioActualizar.NU_TELEFONO_MOVIL     = usuarioActualizarModel.NU_TELEFONO_MOVIL;
                            usuarioActualizar.IN_USUARIO_BLOQUEADO  = usuarioActualizarModel.IN_USUARIO_BLOQUEADO;
                            usuarioActualizar.IN_USUARIO_INACTIVO   = usuarioActualizarModel.IN_USUARIO_INACTIVO;

                            usuarioActualizar.FE_ESTATUS = fechaActual;

                            db.USUARIO.Attach(usuarioActualizar);
                            db.Entry(usuarioActualizar).State = EntityState.Modified;

                            totalActualizado = db.SaveChanges();

                            //Se actualiza o crea la información del empleado, según sea necesario
                            /*  if (usuario.IndicadorEmpleado) {

                                  if (usuarioActualizar.EMPLEADO == null){

                                      usuario.DatosEmpleado.Id    = usuarioActualizarModel.CD_USUARIO;
                                      EMPLEADO empleado           = usuario.DatosEmpleado.toModel();
                                      db.EMPLEADO.Add(empleado);
                                      db.SaveChanges();
                                  }
                                  else {
                                      EMPLEADO empleadoActualizar                 = usuarioActualizar.EMPLEADO;
                                      EMPLEADO empleadoActualizarModel            = usuario.DatosEmpleado.toModel();

                                      empleadoActualizar.CD_CARGO                 = empleadoActualizarModel.CD_CARGO;
                                      empleadoActualizar.CD_USUARIO_SUPERVISOR    = empleadoActualizarModel.CD_USUARIO_SUPERVISOR;
                                      empleadoActualizar.CD_SUCURSAL              = empleadoActualizarModel.CD_SUCURSAL;
                                      empleadoActualizar.NU_EXTENSION             = empleadoActualizarModel.NU_EXTENSION;
                                      empleadoActualizar.DI_CORREO                = empleadoActualizarModel.DI_CORREO;

                                      //Se actualizan los datos del empleado
                                      db.EMPLEADO.Attach(empleadoActualizar);
                                      db.Entry(empleadoActualizar).State = EntityState.Modified;
                                      db.SaveChanges();

                                      normalizaRelacionSuperiorSubordinado(db, empleadoActualizar.CD_USUARIO);

                                  }
                              }
                              */

                            //Se borran todos los roles
                            //Se le asigan los roles que pertenezcan al empleado
                            IList<int> rolesSeleccionados = usuario.RolesSeleccionados == null ? new List<int>() : usuario.RolesSeleccionados.ToList();
                            IEnumerable<int> rolesNoSeleccionados = usuario.RolesDisponibles == null ? new List<int>() : usuario.RolesDisponibles.Where(x => !rolesSeleccionados.Contains(Convert.ToInt32(x.Value))).Select(x => Convert.ToInt32(x.Value));


                            List<ROL> rolAEliminar = (from x in db.ROL
                                                      where x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuarioActualizar.CD_USUARIO)
                                                      && rolesNoSeleccionados.Contains(x.CD_ROL)
                                                      select x).ToList();

                            rolAEliminar.ForEach(x => usuarioActualizar.ROL.Remove(x));

                            db.SaveChanges();


                            List<ROL> rolAAgregar = (from x in db.ROL
                                                     where rolesSeleccionados.Contains(x.CD_ROL)
                                                     && !x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuarioActualizar.CD_USUARIO)
                                                     select x).ToList();

                            rolAAgregar.ForEach(x => usuarioActualizar.ROL.Add(x));
                            db.SaveChanges();

                            IEnumerable<Aplicacion> app = usuario.Aplicaciones.AsEnumerable();

                            foreach (USUARIO_APLICACION usuarioApp in (from x in db.USUARIO_APLICACION
                                                                       where x.CD_USUARIO == usuarioActualizar.CD_USUARIO
                                                                       select x
                                                                       )){

                                if ((from x in app select x.Id).Contains(usuarioApp.CD_APLICACION)){

                                    usuarioApp.IN_INACTIVO = (from x in usuario.Aplicaciones
                                                              where x.Id == usuarioApp.CD_APLICACION
                                                              select !x.IndicadorActiva).FirstOrDefault();

                                    db.USUARIO_APLICACION.Attach(usuarioApp);
                                    db.Entry(usuarioApp).State = EntityState.Modified;

                                    db.SaveChanges();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (System.Exception exc){
                        transaction.Rollback();                        
                        throw exc;
                    }
                }
            }
            return totalActualizado;
       }


        /// <summary>
        /// Método para actualizar los usuarios internos
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="CodigoUsuarioPadre"></param>
        /// <returns></returns>
         public bool actualiza_UsuarioInterno(ref Usuario usuario, int CodigoUsuarioPadre)
        {

            bool result = false;
            int Id = usuario.Id;

            DateTime feActual = DateTime.Now;

            AuthRepositorio authRepo = new AuthRepositorio();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Se guarda la información de usuario 
                        var usuarioBD = (from x in db.USUARIO
                                         where x.CD_USUARIO == Id
                                         select x).FirstOrDefault();

                       
                            usuarioBD.FE_NACIMIENTO         = usuario.FechaNacimiento;
                            usuarioBD.DE_NOMBRE_APELLIDO    = usuario.NombreApellido;
                            usuarioBD.DI_EMAIL_USUARIO      = usuario.Email;
                            usuarioBD.NU_TELEFONO_FIJO      = usuario.TelefonoFijo;
                            usuarioBD.NU_TELEFONO_MOVIL     = usuario.TelefonoMovil;                           
                            usuarioBD.FE_ESTATUS            = feActual;

                            db.USUARIO.Attach(usuarioBD);
                            db.Entry(usuarioBD).State = EntityState.Modified;

                            db.SaveChanges();
                       

                        //Se le asigan los roles que puedan ser heredables 
                        List<ROL> roles = (from x in db.ROL
                                           from y in db.PARAMETRO_CONFIGURACION
                                           where !x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuarioBD.CD_USUARIO)
                                           && x.USUARIO.Select(y => y.CD_USUARIO).Contains(CodigoUsuarioPadre)
                                           && y.CD_CONFIGURACION == x.CD_ROL.ToString()
                                           && (y.NM_PARAMETRO == "ROL_CLIENTE"
                                           || y.NM_PARAMETRO == "ROL_SOCIO_COMERCIAL"
                                           )
                                           select x).ToList();

                        roles.ForEach(x => usuarioBD.ROL.Add(x));
                        db.SaveChanges();

                      
                        //Se actualizan las aplicaciones de usuario interno

                        usuario.Aplicaciones.ToList().ForEach(x => {
                            var aplicacionDB = (from y in db.USUARIO_APLICACION
                                                where y.CD_APLICACION == x.Id
                                                && y.CD_USUARIO == Id
                                                select y).FirstOrDefault();

                            if (aplicacionDB != null)
                            {
                                aplicacionDB.FE_ESTATUS = feActual;

                                aplicacionDB.IN_INACTIVO = !x.IndicadorActiva;

                                db.SaveChanges();
                            }
                        });


                        //Se guardan los dispositivos del usuario interno
                        IEnumerable<int> dispositivosSeleccionados = usuario.DispositivosSeleccionados == null ? new List<int>() : usuario.DispositivosSeleccionados;

                        List<DISPOSITIVO> dispositivosAEliminar = (from x in db.DISPOSITIVO
                                                                   where x.USUARIO.Select(y => y.CD_USUARIO).Contains(CodigoUsuarioPadre)
                                                                   && x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuarioBD.CD_USUARIO)
                                                                   && !dispositivosSeleccionados.Contains(x.ID_DISPOSITIVO)
                                                                   select x).ToList();

                        dispositivosAEliminar.ForEach(x => usuarioBD.DISPOSITIVO.Remove(x));

                        db.SaveChanges();


                        List<DISPOSITIVO> dispositivosAAgregar = (from x in db.DISPOSITIVO
                                                                  where dispositivosSeleccionados.Contains(x.ID_DISPOSITIVO)
                                                                  select x).ToList();

                        dispositivosAAgregar.ForEach(x => usuarioBD.DISPOSITIVO.Add(x));
                        db.SaveChanges();


                        transaction.Commit();
                        result = true;
                    }
                    catch (System.Exception exc)
                    {
                        transaction.Rollback();
                        log.Error(exc);
                        result = false;
                    }
                }
            }
            return result;

        }




        /// <summary>
        /// Guarda el parámetro de Configuración 
        /// </summary>
        /// <param name="parametroConfiguracion"></param>
        /// <returns>Cantidad de registros guardados</returns>
        public int guarda_PARAMETRO_CONFIGURACION(PARAMETRO_CONFIGURACION parametroConfiguracion)
        {

            using (var db = new IntranetSAIEntities())
            {
                db.PARAMETRO_CONFIGURACION.Add(parametroConfiguracion);
                return db.SaveChanges();
            }
        }


        /// <summary>
        /// Elimina el parámetro de Configuración 
        /// </summary>
        /// <param name="parametroConiguracion"></param>
        /// <returns>Cantidad de registros eliminados</returns>
        public int elimina_PARAMETRO_CONFIGURACION(PARAMETRO_CONFIGURACION parametroConfiguracion)
        {

            using (var db = new IntranetSAIEntities())
            {
                db.PARAMETRO_CONFIGURACION.Attach(parametroConfiguracion);
                db.PARAMETRO_CONFIGURACION.Remove(parametroConfiguracion);
                return db.SaveChanges();
            }
        }

        
        /// <summary>
        /// Elimina un unidad administrativa
        /// </summary>
        /// <param name="parametroConiguracion"></param>
        /// <returns>Cantidad de registros eliminados</returns>
        public int elimina_UNIDAD_ADMINISTRATIVA(UNIDAD_ADMINISTRATIVA unidadAdministrativa){

            using (var db = new IntranetSAIEntities())
            {
                db.UNIDAD_ADMINISTRATIVA.Attach(unidadAdministrativa);
                db.UNIDAD_ADMINISTRATIVA.Remove(unidadAdministrativa);
                return db.SaveChanges();
            }
        }


        /// <summary>
        /// Permite desvincular un usuario de otro
        /// </summary>
        /// <param name="CodigoUsuarioPadre"></param>
        /// <param name="CodigoUsuario"></param>
        /// <returns></returns>
        public bool elimina_UsuarioInterno(int CodigoUsuarioPadre, int CodigoUsuario, out string NombreApellidoUsuario) {
            bool result = false;
            DateTime feActual = DateTime.Now;
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        USUARIO usuario = (from x in db.USUARIO
                                       where x.CD_USUARIO == CodigoUsuario
                                       select x).FirstOrDefault();

                        USUARIO usuarioPadre = (from x in db.USUARIO
                                                where x.CD_USUARIO == CodigoUsuarioPadre
                                                select x).FirstOrDefault();

                        NombreApellidoUsuario = usuario.DE_NOMBRE_APELLIDO;
                        
                        //Se elimina la relación entre usuarios
                        usuario.USUARIO2.Remove(usuarioPadre);

                        db.SaveChanges();

                        bool  inEsCliente = usuario.CLIENTE!=null && !usuario.CLIENTE.IN_INACTIVO ? true : false
                             ,inEsSocio = usuario.SOCIO_COMERCIAL != null && !usuario.SOCIO_COMERCIAL.IN_INACTIVO ? true : false
                             ,inEsEmpleado = usuario.EMPLEADO != null && usuario.EMPLEADO.FE_RETIRO==null ? true : false;


                        //Se verifica si no existe como usuario en alguna otra entidad
                        if (!inEsCliente && !inEsSocio && !inEsEmpleado){
                            usuario.IN_USUARIO_INACTIVO = true;
                            usuario.FE_ESTATUS = feActual;

                            if (usuario.USUARIO2.Count() == 0){
                                usuario.ROL.Clear();
                                usuario.DISPOSITIVO.Clear();
                            }
                            db.SaveChanges();
                        }
                       

                        //Se verifica aplicación a apicación si puede ser desactivada
                        usuario.USUARIO_APLICACION.ToList().ForEach(x => {
                            bool otroPadreTieneAplicacion = false;
                            foreach(USUARIO y in usuario.USUARIO2.ToList())
                            {
                                if (y.USUARIO_APLICACION.Select(w => w.CD_APLICACION).Contains(x.CD_APLICACION))
                                {
                                    otroPadreTieneAplicacion = true;
                                    break;
                                }
                            }
                            if (!otroPadreTieneAplicacion&&!x.APLICACION.IN_ES_DASHBOARD&&!x.APLICACION.IN_ES_PERFIL_USUARIO&& 
                                 usuarioPadre.USUARIO_APLICACION.Select(z=> z.CD_APLICACION).Contains(x.CD_APLICACION)&&!String.IsNullOrWhiteSpace(x.APLICACION.DE_ALIAS)
                            ) { x.IN_INACTIVO = true; }
                        });

                        db.SaveChanges();

                        //Los dispositivos de ese socio comercial también se desvinculan 
                        usuario.DISPOSITIVO.ToList().ForEach(x => {
                            bool otroPadreTieneDispositivo = false;
                            foreach (USUARIO y in usuario.USUARIO2.ToList())
                            {
                                if (y.DISPOSITIVO.Select(w => w.ID_DISPOSITIVO).Contains(x.ID_DISPOSITIVO))
                                {
                                    otroPadreTieneDispositivo = true;
                                    break;
                                }
                            }
                            if (!otroPadreTieneDispositivo&&(!inEsCliente||!usuario.CLIENTE.DISPOSITIVO_CLIENTE.Select(z=>z.ID_DISPOSITIVO).Contains(x.ID_DISPOSITIVO))) { usuario.DISPOSITIVO.Remove(x);}
                        });

                        db.SaveChanges();

                        result = true;
                        transaction.Commit();
                    }
                        catch (System.Exception exc)
                    {
                        transaction.Rollback();
                        throw exc;
                    }
                }
            }
            return result;
        }




        /// <summary>
        /// Elimina un cargo
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns>Cantidad de registros eliminados</returns>
        public int elimina_CARGO(CARGO cargo)
        {
            int result = 0;
            using (var db = new IntranetSAIEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Se borran los cargo asociados al rol
                        cargo = (from x in db.CARGO
                                 where x.CD_CARGO == cargo.CD_CARGO
                                 select x).FirstOrDefault();

                        cargo.ROL.ToList().ForEach(x => cargo.ROL.Remove(x));

                        db.SaveChanges();

                        //Se borra el rol
                        db.CARGO.Attach(cargo);
                        db.CARGO.Remove(cargo);


                        result= db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (System.Exception exc)
                    {
                        transaction.Rollback();
                        throw exc;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Actuliza un registro en RESPUESTA_OPERADOR_GCM
        /// </summary>
        /// <param name="respuestaOperadorGCM"></param>
        /// <returns></returns>
        public int actualiza_RESPUESTA_OPERADOR_GCM(RESPUESTA_OPERADOR_GCM respuestaOperadorGCM) {

            using (var db = new IntranetSAIEntities()) {
                RESPUESTA_OPERADOR_GCM respuestaOperadorGCM_Ori = this.obten_RESPUESTA_OPERADOR_GCM_ById(respuestaOperadorGCM.ID_RESPUESTA_OPERADOR);

                respuestaOperadorGCM_Ori.DE_CONTENIDO = respuestaOperadorGCM.DE_CONTENIDO;
                respuestaOperadorGCM_Ori.DE_TITULO = respuestaOperadorGCM.DE_TITULO;
                respuestaOperadorGCM_Ori.FE_ESTATUS = respuestaOperadorGCM.FE_ESTATUS;
                respuestaOperadorGCM_Ori.IN_INACTIVO = respuestaOperadorGCM.IN_INACTIVO;
                respuestaOperadorGCM_Ori.TP_ATENCION = respuestaOperadorGCM.TP_ATENCION;
                
                db.RESPUESTA_OPERADOR_GCM.Attach(respuestaOperadorGCM_Ori);               
                db.Entry(respuestaOperadorGCM_Ori).State = EntityState.Modified;
                return db.SaveChanges();

            }
        }


        /// <summary>
        /// Actualiza un registro en UNIDAD_ADMINISTRATIVA
        /// </summary>
        /// <param name="unidadAdministrativa"></param>
        /// <returns></returns>
        public int actualiza_UNIDAD_ADMINISTRATIVA(UNIDAD_ADMINISTRATIVA unidadAdministrativa)
        {
            DateTime feActual = DateTime.Now;
            using (var db = new IntranetSAIEntities())
            {
                db.UNIDAD_ADMINISTRATIVA.Attach(unidadAdministrativa);
                db.Entry(unidadAdministrativa).State = EntityState.Modified;
                return db.SaveChanges();

            }
        }

        /// <summary>
        /// Actualiza un registro en SOCIO_COMERCIAL
        /// </summary>
        /// <param name="socioComercial"></param>
        /// <returns></returns>
        public int actualiza_SOCIO_COMERCIAL(SOCIO_COMERCIAL socioComercial)
        {
            DateTime fechaActual = DateTime.Now;
            using (var db = new IntranetSAIEntities()) {
                socioComercial.FE_ESTATUS = fechaActual;
                db.SOCIO_COMERCIAL.Attach(socioComercial);
                db.Entry(socioComercial).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }


        /// <summary>
        /// Método para actualizar un socio comercial
        /// </summary>
        /// <param name="socioComercial"></param>
        /// <returns></returns>
        public bool actualiza_SocioComercial(ref SocioComercial socioComercial)
        {

            bool result = false;
            int Id = socioComercial.Id;
            DateTime feActual = DateTime.Now;
            AuthRepositorio authRepo = new AuthRepositorio();
            
            ValidacionRepositorio validaRepo = new ValidacionRepositorio();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try{

                        //Se guarda la información de usuario 
                        var socio = (from x in db.SOCIO_COMERCIAL
                                       where x.CD_SOCIO_COMERCIAL == Id
                                       select x).FirstOrDefault();
                                               

                        //Se guarda la información del socio
                        SOCIO_COMERCIAL socioComercialModelDB = socioComercial.toModel();

                        socio.NM_SOCIO_COMERCIAL = socioComercialModelDB.NM_SOCIO_COMERCIAL;
                        socio.DI_EMAIL = socioComercialModelDB.DI_EMAIL;
                        socio.IN_INACTIVO = socioComercialModelDB.IN_INACTIVO;
                        socio.NU_TELEFONO_FIJO = socioComercialModelDB.NU_TELEFONO_FIJO;
                        socio.NU_TELEFONO_MOVIL = socioComercialModelDB.NU_TELEFONO_MOVIL;
                        socio.CD_PAIS = socioComercialModelDB.CD_PAIS;
                        socio.CD_PROVINCIA = socioComercialModelDB.CD_PROVINCIA;
                        socio.CD_DISTRITO = socioComercialModelDB.CD_DISTRITO;
                        socio.CD_CORREGIMIENTO = socioComercialModelDB.CD_CORREGIMIENTO;
                        socio.DI_OFICINA = socioComercialModelDB.DI_OFICINA;

                        if (socioComercial.IndicadorActivo)  
                            socio.USUARIO.IN_USUARIO_INACTIVO = !socioComercial.IndicadorActivo;


                        db.SOCIO_COMERCIAL.Attach(socio);
                        db.Entry(socio).State = EntityState.Modified;
                        db.SaveChanges();

                        IEnumerable<int> serviciosSeleccionados = socioComercial.ServiciosAppSeleccionados;
                        serviciosSeleccionados = serviciosSeleccionados == null ? new List<int>() : serviciosSeleccionados;


                        socio.USUARIO.MENU_SERVICIO = socio.USUARIO.MENU_SERVICIO.Union(
                            (from x in db.MENU_SERVICIO
                             where serviciosSeleccionados.Contains(x.CD_MENU_SERVICIO)
                             select x)
                            ).ToList();

                        socio.USUARIO.MENU_SERVICIO.ToList().Where(x => !serviciosSeleccionados.Contains(x.CD_MENU_SERVICIO)
                         ).ToList().ForEach(x => { socio.USUARIO.MENU_SERVICIO.Remove(x); });

                        

                        db.USUARIO.Attach(socio.USUARIO);
                        db.Entry(socio.USUARIO).State = EntityState.Modified;

                        db.SaveChanges();

                        //Se ingresan los dispositivos
                        IEnumerable<int> dispositivosSeleccionados = socioComercial.DispositivosSeleccionados==null?new List<int>(): socioComercial.DispositivosSeleccionados;


                        socio.USUARIO.DISPOSITIVO = socio.USUARIO.DISPOSITIVO.Union(
                            (from x in db.DISPOSITIVO
                             where dispositivosSeleccionados.Contains(x.ID_DISPOSITIVO)
                             select x)
                            ).ToList();

                        socio.USUARIO.DISPOSITIVO.ToList().Where(x => !dispositivosSeleccionados.Contains(x.ID_DISPOSITIVO)
                         ).ToList().ForEach(x => { socio.USUARIO.DISPOSITIVO.Remove(x); });

                        db.SaveChanges();


                        //Se buscan los roles que deban serle otorgados
                        if (socioComercial.IndicadorActivo)
                        {
                            List<ROL> roles = (from x in db.ROL
                                               from y in db.PARAMETRO_CONFIGURACION
                                               where !x.USUARIO.Select(y => y.CD_USUARIO).Contains(socio.USUARIO.CD_USUARIO)
                                               && y.CD_CONFIGURACION == x.CD_ROL.ToString()
                                               && y.NM_PARAMETRO == "ROL_SOCIO_COMERCIAL"
                                               select x
                                                      ).ToList();

                            roles.ForEach(x => socio.USUARIO.ROL.Add(x));
                        }
                      


                        db.USUARIO.Attach(socio.USUARIO);
                        db.Entry(socio.USUARIO).State = EntityState.Modified;

                        db.SaveChanges();


                        //Se actualizan las aplicaciones
                        foreach (var aplicacionSocio in socioComercial.Aplicaciones)
                        {

                            var aplicacionDB = (from x in db.USUARIO_APLICACION
                                                where x.CD_APLICACION == aplicacionSocio.Id
                                                && x.CD_USUARIO == socio.USUARIO.CD_USUARIO
                                                select x).FirstOrDefault();

                            if (aplicacionDB != null)                           
                            {

                                aplicacionDB.FE_ESTATUS = feActual;
                                aplicacionDB.IN_INACTIVO = !aplicacionSocio.IndicadorActiva;

                                db.USUARIO_APLICACION.Attach(aplicacionDB);
                                db.Entry(aplicacionDB).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                        }


                        dbTransaction.Commit();
                        result = true;
                    }
                    catch (System.Exception exc)
                    {
                        log.Error(exc);
                        dbTransaction.Rollback();
                        result = false;
                    }
                }
            }

            return result;
        }



    

        /// <summary>
        /// Actuliza la información de un empleado
        /// </summary>
        /// <param name="empleado"></param>
        /// <returns></returns>
        public bool actualiza_Empleado(ref Empleado empleado)
        {

            bool result = false;
            int Id = empleado.Id;
           DateTime feActual = DateTime.Now;

            AuthRepositorio authRepo = new AuthRepositorio();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        

                        //Se guarda la información de usuario 
                        var empleadoBD = (from x in db.EMPLEADO
                                         where x.CD_USUARIO == Id
                                         select x).FirstOrDefault();

                        if (empleadoBD.CD_CARGO != empleado.CargoSeleccionado) {
                            List<ROL> rolAEliminarPorCargo = (from x in db.ROL
                                                      where x.USUARIO.Select(y => y.CD_USUARIO).Contains(empleadoBD.USUARIO.CD_USUARIO)
                                                      && x.CARGO.Select(y=>y.CD_CARGO ).Contains(empleadoBD.CD_CARGO)
                                                      select x).ToList();

                            rolAEliminarPorCargo.ForEach(x => empleadoBD.USUARIO.ROL.Remove(x));

                            db.SaveChanges();
                        }


                        empleadoBD.USUARIO.FE_NACIMIENTO = empleado.FechaNacimiento;
                        empleadoBD.USUARIO.DE_NOMBRE_APELLIDO = empleado.NombreApellido;
                        empleadoBD.USUARIO.DI_EMAIL_USUARIO = empleado.Email;
                        empleadoBD.USUARIO.NU_TELEFONO_FIJO = empleado.TelefonoFijo;
                        empleadoBD.USUARIO.NU_TELEFONO_MOVIL = empleado.TelefonoMovil;
                        empleadoBD.USUARIO.FE_ESTATUS = feActual;
                        empleadoBD.CD_CARGO = empleado.CargoSeleccionado;
                        empleadoBD.CD_SUCURSAL = empleado.SucursalSeleccionada;
                        empleadoBD.CD_USUARIO_SUPERVISOR = empleado.SupervisorSeleccionado;
                        empleadoBD.DI_CORREO = empleado.EmailCompania;
                        empleadoBD.FE_INGRESO = empleado.FechaIngreso;
                        
                        db.SaveChanges();

                        normalizaRelacionSuperiorSubordinado(db, empleadoBD.CD_USUARIO);

                        //Se le asigan los roles que pertenezcan al empleado
                        IList<int> rolesSeleccionados = empleado.RolesSeleccionados == null ? new List<int>() : empleado.RolesSeleccionados.ToList();
                        IEnumerable<int> rolesNoSeleccionados = empleado.RolesDisponibles == null ? new List<int>() : empleado.RolesDisponibles.Where(x => !rolesSeleccionados.Contains(Convert.ToInt32(x.Value))).Select(x => Convert.ToInt32(x.Value));


                        List<ROL> rolAEliminar = (from x in db.ROL
                                                  where x.USUARIO.Select(y => y.CD_USUARIO).Contains(empleadoBD.USUARIO.CD_USUARIO)                                                
                                                  && rolesNoSeleccionados.Contains(x.CD_ROL)
                                                  select x).ToList();

                        rolAEliminar.ForEach(x => empleadoBD.USUARIO.ROL.Remove(x));

                        db.SaveChanges();


                        List<ROL> rolAAgregar = (from x in db.ROL
                                                 where rolesSeleccionados.Contains(x.CD_ROL)
                                                 && !x.USUARIO.Select(y=> y.CD_USUARIO).Contains(empleadoBD.USUARIO.CD_USUARIO)
                                                 select x).ToList();

                        rolAAgregar.ForEach(x => empleadoBD.USUARIO.ROL.Add(x));
                        db.SaveChanges();



                        //Se actualizan las aplicaciones de usuario interno
                        empleado.Aplicaciones.Where(
                             x => x.RolesAsociados.Split('#').ToList().Intersect(rolesSeleccionados.Select(w => w.ToString())).Any() == true
                            ).ToList().ForEach(x => {
                                var aplicacionDB = (from y in db.USUARIO_APLICACION
                                                    where y.CD_APLICACION == x.Id
                                                    && y.CD_USUARIO == Id
                                                    select y).FirstOrDefault();

                                if (aplicacionDB != null)
                                {
                                    aplicacionDB.FE_ESTATUS = feActual;

                                    aplicacionDB.IN_INACTIVO = !x.IndicadorActiva;

                                    db.SaveChanges();
                                }
                            });

                        empleado.Id = empleadoBD.CD_USUARIO;

                        transaction.Commit();
                        result = true;
                    }
                    catch (System.Exception exc)
                    {
                        transaction.Rollback();
                        log.Error(exc);
                        result = false;
                    }
                }
            }
            return result;
        }




        /// <summary>
        /// Actuliza un registro en PARAMETRO_CONFIGURACION
        /// </summary>
        /// <param name="parametroConfiguracion"></param>
        /// <returns></returns>
        public int actualiza_PARAMETRO_CONFIGURACION(PARAMETRO_CONFIGURACION parametroConfiguracion)
        {

            using (var db = new IntranetSAIEntities())
            {
               return  db.Database.ExecuteSqlCommand("UPDATE UTILITARIO.PARAMETRO_CONFIGURACION "
                    + "SET CD_CONFIGURACION = {0} "
                    + ", DE_CONFIGURACION = {1} "
                    + ", DE_USO_CONFIGURACION = {2} "
                    + "WHERE NM_PARAMETRO = {3} AND CD_CONFIGURACION = {4} "
                    , parametroConfiguracion.CD_CONFIGURACION_EDITAR
                    , parametroConfiguracion.DE_CONFIGURACION
                    , parametroConfiguracion.DE_USO_CONFIGURACION
                    , parametroConfiguracion.NM_PARAMETRO
                    , parametroConfiguracion.CD_CONFIGURACION
                     );
               
                

            }
        }


        /// <summary>
        /// Devuelve el Log dado el Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LogTransacciones obtenLogTransacciones_ById(int id) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.LOG_TRANSACCIONES
                        where x.ID_LOG == id
                        select new LogTransacciones
                        {
                             Id = x.ID_LOG
                            ,Accion = x.NM_ACTION
                            ,CodigoUsuario = x.CD_USUARIO
                            ,Controlador = x.NM_CONTROLLER
                            ,DescripcionNivel = x.NM_LEVEL
                            ,Excepcion = x.DE_EXCEPTION
                            ,FechaOperacion = x.FE_OPERACION
                            ,Ip = x.DI_IP
                            ,Mensaje = x.DE_MESSAGE
                            ,NombreUsuario = x.USUARIO.DE_NOMBRE_APELLIDO
                            ,Url = x.NM_URL
                        }
                        ).FirstOrDefault();
            }
        }



        /// <summary>
        /// Retorna el parametro de configuración dado el nombre del parámetro y el código del parámetro
        /// </summary>
        /// <param name="nmParametro"></param>
        /// <param name="cdConfiguracion"></param>
        /// <returns></returns>
        public ParametroConfiguracion obtenParametroConfiguracion_ById(string nmParametro, string cdConfiguracion) {


            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.PARAMETRO_CONFIGURACION
                        where x.CD_CONFIGURACION == cdConfiguracion
                        && x.NM_PARAMETRO == nmParametro
                        select new ParametroConfiguracion
                        {
                            CodigoConfiguracion = x.CD_CONFIGURACION
                            ,Descripcion = x.DE_CONFIGURACION
                            ,NombreParametro = x.NM_PARAMETRO
                            ,Uso = x.DE_USO_CONFIGURACION
                        }).FirstOrDefault();

            }
        }

        /// <summary>
        /// Permite obtener la uidad administrativa dado el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UnidadAdministrativa obtenUnidadAdministrativa_ById(int id) {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (from x in db.UNIDAD_ADMINISTRATIVA
                        where x.CD_UNIDAD_ADMINISTRATIVA == id
                        select new UnidadAdministrativa
                        {
                             Id = x.CD_UNIDAD_ADMINISTRATIVA
                            ,Nombre = x.NM_UNIDAD_ADMINISTRATIVA
                            ,FechaCreacion = x.FE_CREACION
                        }).FirstOrDefault();
                }
        }


        /// <summary>
        /// Realiza la búsqueda de roles asignables al usuario
        /// </summary>
        /// <returns></returns>
        public IList<Rol> obtenRolesAsignables() {

           IList<Rol> result = new List<Rol>();

            using (IntranetSAIEntities db = new IntranetSAIEntities()){
                    result = (
                              from y in db.ROL.ToList().AsQueryable()
                              where y.CD_ROL != 1
                              && y.IN_ROL_ACTIVO == true
                              orderby y.NM_ROL                              
                              select new Rol{
                                    Id                      = y.CD_ROL
                                  , Nombre                  = y.NM_ROL
                                  , Descripcion              = y.DE_ROL
                                  , IndicadorSeleccionado   = false                                 
                              }).ToList();
                
            }
            return result;
        }

        /// <summary>
        /// Retormna el listado de empleados que tienen como supervisor al empleado indicado como parámetro
        /// </summary>
        /// <param name="CodigoEmepleado">Empleado supervisor</param>
        /// <returns></returns>
        public IEnumerable<EMPLEADO> obtenSubordinadosEmpleado(int CodigoEmpleadoSupervisor) {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.EMPLEADO
                        where x.CD_USUARIO_SUPERVISOR == CodigoEmpleadoSupervisor
                        && x.USUARIO.IN_USUARIO_INACTIVO == false
                        select x
                        ).Include(x=> x.USUARIO)
                         .ToList()
                         .AsEnumerable();
            }
        }

        


        /// <summary>
        /// Devuelve la lista de aplicaciones configurables por roles
        /// </summary>
        /// <returns></returns>
        public IList<Aplicacion> obtenAplicacionesUsuario() {
            IList<Aplicacion> result;
            
            //Se buscan las aplicaciones con sus respectivos roles
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                   result = (from y in (from x in db.APLICACION
                                        where x.IN_APLICACION_INACTIVA == false
                                        && x.IN_ASIGNABLE == true
                                        select x
                                        ).ToList()
                              select new Aplicacion()
                              {
                                   Id = y.CD_APLICACION
                                  ,IdAplicacionPadre = y.CD_APLICACION_PADRE
                                  ,IndicadorActiva = true
                                  ,Nombre = y.NM_APLICACION
                                  ,RolesAsociados = string.Join("#", y.ROL.Select(x => x.CD_ROL))
                                  ,NumeroSecuencia = y.NU_ORDEN
                                  ,Icono = y.NM_ICONO_MENU
                                  ,IndicadorDashBoard = y.IN_ES_DASHBOARD
                                  ,IndicadorPerfilUsuario = y.IN_ES_PERFIL_USUARIO
                              }
                            ).ToList();
             
            }
            return result;
        }


        /// <summary>
        /// Devuelve la lista de aplicaciones activas e inactivas del usuario
        /// </summary>
        /// <returns></returns>
        public IList<Aplicacion> obtenAplicacionesUsuario_ByUsuarioId(int IdUsuario){
            
            IList<Aplicacion> result = new List<Aplicacion>();
            result = obtenAplicacionesUsuario();

            //Se obtienen las aplicaiones del usuario
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                var aplicacionesUsuario = (from x in db.USUARIO
                                           where x.CD_USUARIO == IdUsuario
                                           select x).FirstOrDefault();
                if (aplicacionesUsuario != null) {

                    result.ToList().ForEach(
                        x => {
                        Boolean? inInactivo =(from y in aplicacionesUsuario.USUARIO_APLICACION
                                                 where y.CD_APLICACION == x.Id
                                                 select y.IN_INACTIVO).FirstOrDefault();

                            x.IndicadorActiva = inInactivo == null ? true : !(bool)inInactivo;
                            }                        
                        );
                }
            }
            return result;
        }


        /// <summary>
        /// Valida obtener las aplicaciones para los usuarios internos
        /// </summary>
        /// <param name="CodigoUsuarioPadre"></param>
        /// <param name="CodigoUsuario"></param>
        /// <returns></returns>
        public IModeloAplicacion obtenAplicacionesUsuarioInterno_ById(int CodigoUsuarioPadre, int CodigoUsuario) {
            Usuario result = new Usuario();
            //Se buscan las aplicaciones 
            using (IntranetSAIEntities db = new IntranetSAIEntities()){

                    //Se verifica el nivel de usuario
                    int nivel = 1;

                    if ((from x in db.USUARIO
                         where x.USUARIO1.Select(y => y.CD_USUARIO).Contains(CodigoUsuarioPadre)
                         select x
                      ).Count() > 0)
                        nivel++;


                //Se seleccionan las aplicaciones
                IList<APLICACION> aplicacionesPadre =  (from x in db.APLICACION
                                                                       where x.IN_APLICACION_INACTIVA == false
                                                                       && x.IN_ASIGNABLE == true
                                                                       && x.USUARIO_APLICACION.Select(y => y.CD_USUARIO).Contains(CodigoUsuarioPadre)
                                                                               == true
                                                                       select x).Include(x=> x.APLICACION1).Include(x => x.APLICACION2).ToList();
               
                 if (nivel == 2) 
                    Core.Utils.UtilHelper.remueveAplicacionDeListado(ref aplicacionesPadre, (from x in db.APLICACION
                                                                                             where x.DE_ALIAS == Core.Constante.Aplicacion.AdministrarUsuario
                                                                                             select x.CD_APLICACION).FirstOrDefault());
                
                result.Aplicaciones =  ( from y in aplicacionesPadre.ToList()
                     select new Aplicacion()
                     {
                         Id = y.CD_APLICACION
                         ,
                         IdAplicacionPadre = y.CD_APLICACION_PADRE
                         ,
                         IndicadorActiva = (CodigoUsuario!=0?(y.USUARIO_APLICACION.Where(x => x.CD_USUARIO == CodigoUsuario).Select(x => !x.IN_INACTIVO).FirstOrDefault())
                                            : (y.USUARIO_APLICACION.Where(x => x.CD_USUARIO == CodigoUsuarioPadre).Select(x => !x.IN_INACTIVO).FirstOrDefault()))
                         ,
                         Nombre = y.NM_APLICACION
                         ,
                         RolesAsociados = string.Join("#", y.ROL.Select(x => x.CD_ROL))
                         ,
                         NumeroSecuencia = y.NU_ORDEN
                         ,
                         Icono = y.NM_ICONO_MENU
                         ,
                         IndicadorDashBoard = y.IN_ES_DASHBOARD
                         ,
                         IndicadorPerfilUsuario = y.IN_ES_PERFIL_USUARIO
                     }).ToList();
            }
            return result;
        }
        

        /// <summary>
        /// Devuelve la lista de aplicaciones activas e inactivas del rol
        /// </summary>
        /// <returns></returns>
        public IList<Aplicacion> obtenAplicacionesRol_ByRolId(int Id)
        {

            IList<Aplicacion> result = new List<Aplicacion>();
            result = obtenAplicacionesUsuario();

            //Se obtienen las aplicaiones del usuario
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                var aplicacionesRol = (from x in db.ROL
                                           where x.CD_ROL == Id
                                           select x).FirstOrDefault();
                if (aplicacionesRol != null)
                {

                    result.ToList().ForEach(
                        x => {
                            Boolean? inActiva = (from y in aplicacionesRol.APLICACION
                                                   where y.CD_APLICACION == x.Id
                                                   select y).Count()>0?true: false;

                            x.IndicadorActiva = inActiva == null ? false : (bool)inActiva;
                        }
                        );
                }
            }
            return result;
        }

        /// <summary>
        /// Retorna el listado de aplicaciones relacionadas al socio comercial
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IModeloAplicacion obtenAplicacionesSocioComercial_ById(int Id) {
            SocioComercial result = new SocioComercial();
            result.Aplicaciones = new List<Aplicacion>();

            //Se obtienen las aplicaciones del usuario

            //List<Aplicacion> aplicacionesUsuario = new List<Aplicacion>();
            List<Aplicacion> aplicacionesRolSocio = new List<Aplicacion>();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

              
                //Se buscan las aplicaciones del rol socio comerecial
                aplicacionesRolSocio = 
                (from y in (from x in db.APLICACION
                           where x.IN_APLICACION_INACTIVA == false
                           && x.IN_ASIGNABLE == true
                           && x.ROL.Select(z=> z.CD_ROL.ToString()).Intersect((from w in db.PARAMETRO_CONFIGURACION
                                                                               where w.NM_PARAMETRO == "ROL_SOCIO_COMERCIAL"
                                                                               select w.CD_CONFIGURACION
                               ).ToList()).Any()==true
                           select x).ToList()
                select new Aplicacion()
                {
                    Id = y.CD_APLICACION
                    ,
                    IdAplicacionPadre = y.CD_APLICACION_PADRE
                    ,
                    IndicadorActiva = (y.USUARIO_APLICACION.Where(x => x.CD_USUARIO == Id).Select(x => !x.IN_INACTIVO).FirstOrDefault())
                    ,
                    Nombre = y.NM_APLICACION
                    ,
                    RolesAsociados = string.Join("#", y.ROL.Select(x => x.CD_ROL))
                    ,
                    NumeroSecuencia = y.NU_ORDEN
                    ,
                    Icono = y.NM_ICONO_MENU
                    ,
                    IndicadorDashBoard = y.IN_ES_DASHBOARD
                    ,
                    IndicadorPerfilUsuario = y.IN_ES_PERFIL_USUARIO
                }).ToList();

            }
           

            result.Aplicaciones = aplicacionesRolSocio;
            return result;
        }
        

        /// <summary>
        /// Método para dar de baja a un empleado
        /// </summary>
        /// <param name="empleado"></param>
        /// <returns></returns>
        public bool darBajaEmpleado(EmpleadoBaja empleado){
            bool result = false;
            using (IntranetSAIEntities db = new IntranetSAIEntities()){

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Se obtiene al empleado
                        //Se obtiene el usuario registrado
                        EMPLEADO empleadoActualizar = (from x in db.EMPLEADO
                                                       where x.CD_USUARIO == empleado.Id
                                                       select x).FirstOrDefault();

                        if (empleadoActualizar != null)
                        {
                            DateTime fechaActual = DateTime.Now;

                            USUARIO usu = empleadoActualizar.USUARIO;
                            bool IndicadorSocioComercial = usu.SOCIO_COMERCIAL != null && usu.SOCIO_COMERCIAL.IN_INACTIVO == false ? true : false;
                            bool IndicadorCliente = usu.CLIENTE != null && usu.CLIENTE.IN_INACTIVO == false ? true : false;
                            bool IndicadorUsuarioInterno = (from x in db.USUARIO where x.USUARIO1.Select(z => z.CD_USUARIO).Contains(usu.CD_USUARIO)select x).Count() > 0 ? true : false;
                            if ( !IndicadorSocioComercial &&
                                 !IndicadorCliente &&
                                 !IndicadorUsuarioInterno
                                ) {
                                usu.FE_ESTATUS = fechaActual;
                                usu.IN_USUARIO_INACTIVO = true;
                            }

                            db.USUARIO.Attach(usu);
                            db.Entry(usu).State = EntityState.Modified;
                            db.SaveChanges();

                            empleadoActualizar.FE_RETIRO = empleado.FechaRetiro;

                            db.EMPLEADO.Attach(empleadoActualizar);
                            db.Entry(empleadoActualizar).State = EntityState.Modified;
                            db.SaveChanges();

                            List<ROL> rolAEliminarPorCargo = (from x in db.ROL
                                                              where x.USUARIO.Select(y => y.CD_USUARIO).Contains(empleadoActualizar.USUARIO.CD_USUARIO)
                                                              && x.CARGO.Select(y => y.CD_CARGO).Contains(empleadoActualizar.CD_CARGO)
                                                              select x).ToList();

                            rolAEliminarPorCargo.ForEach(x => empleadoActualizar.USUARIO.ROL.Remove(x));

                            db.SaveChanges();

                            //Se actualizan los usuarios que lo tengan a el como supervisor 
                            if (empleado.IndicadorRequiereReeemplazo)
                            {
                                (from x in db.EMPLEADO
                                 where x.CD_USUARIO_SUPERVISOR == empleadoActualizar.CD_USUARIO
                                 select x).ToList().ForEach(x => {

                                     x.CD_USUARIO_SUPERVISOR = empleado.SupervisorReemplazoSeleccionado;
                                     db.EMPLEADO.Attach(x);
                                     db.Entry(x).State = EntityState.Modified;
                                     db.SaveChanges();
                                 });
                            }

                            transaction.Commit();
                            result = true;
                        }

                    }
                    catch (System.Exception exc){
                        transaction.Rollback();
                        throw exc;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Permite normalizar la relación entre superiores y subordinados
        /// </summary>
        /// <param name="codigoEmpleado">Código del empleado que se está creado u actualizan</param>
        public void normalizaRelacionSuperiorSubordinado(IntranetSAIEntities db, int codigoEmpleado) {
            
            EMPLEADO empleado = (from x in db.EMPLEADO
                                    where x.CD_USUARIO == codigoEmpleado
                                    select x).FirstOrDefault();

            if (empleado!= null && !empleado.USUARIO.IN_USUARIO_INACTIVO) {

                //Se verifica si él es primer empleado bajo ese cargo
                int totalEmpleadosCargo = (from x in db.EMPLEADO
                                            where x.CD_CARGO == empleado.CD_CARGO
                                            && x.USUARIO.IN_USUARIO_INACTIVO == false
                                            select x).Count();

                //Si es el primer empleado activo, se actualizan lo papas
                if (totalEmpleadosCargo == 1) {

                    //Se buscan los cargos que lo tengan a el como supervisor
                        (from x in db.CARGO
                        from y in db.EMPLEADO
                        where x.CD_CARGO_PADRE == empleado.CD_CARGO
                        && x.CD_CARGO == y.CD_CARGO
                        select y).ToList().ForEach(w => {
                            w.CD_USUARIO_SUPERVISOR = empleado.CD_USUARIO;
                            db.EMPLEADO.Attach(w);
                            db.Entry(w).State = EntityState.Modified;
                            db.SaveChanges();
                        });
                }
            }            
        }
    } 
}