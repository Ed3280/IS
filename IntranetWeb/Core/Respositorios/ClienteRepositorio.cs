using IntranetWeb.ViewModel.Cliente;
using IntranetWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System;
using IntranetWeb.Core.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using IntranetWeb.ViewModel.Interface;
using IntranetWeb.ViewModel.Administrador;
using IntranetWeb.Core.Servicio.Logging;

namespace IntranetWeb.Core.Respositorios
{
    public class ClienteRepositorio
    {

        private Log4NetLogger log;

        public ClienteRepositorio() {
            log = new Log4NetLogger();
        }

        /// <summary>
        /// Carga el listado de clientes
        /// </summary>
        /// <returns></returns>
        public IList<Cliente> obtenClientes()
        {

            IList<Cliente> result = new List<Cliente>();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                result = (
                        from x in db.FUNC_001_BUSCA_CLIENTES()
                        select new Cliente
                        {
                             Id = x.CD_CLIENTE
                            ,TipoDocumentoSeleccionado = x.TP_DOCUMENTO_IDENTIDAD
                            ,Cedula = x.NU_DOCUMENTO_IDENTIDAD
                            ,Nombre = x.DE_NOMBRE_APELLIDO_RAZON_SOCIAL
                            ,Email = x.EMAIL
                            ,NumeroMovil = x.NU_MOVIL
                            ,ListadoSims = x.SIMS_CARDS
                            ,IndicadorEstaRegistrado = x.IN_REGISTRO_MOVIL
                            ,IndicadorEstaActivo = !x.IN_USUARIO_INACTIVO
                            ,DescripcionEstaActivo = !x.IN_REGISTRO_MOVIL? Resources.DescripcionResource.NoRegistrado:(!x.IN_USUARIO_INACTIVO ? Resources.DescripcionResource.Activo : Resources.DescripcionResource.Inactivo )
                        }).ToList();
            }

            result.ToList().ForEach(x => x.EditarRegistro = HtmlHelper.IconosGestionRegistro(new Dictionary<string, string>() {   { "data-id-cliente", x.Id.ToString()}
                                                                                                                                , { "data-nombre-cliente", x.Nombre } }
                                                                                                                                , false, true, false));

            return result;

        }


        /// <summary>
        /// Obtiene la información del dispositivo dado el id
        /// </summary>
        /// <returns></returns>
        public DISPOSITIVO obten_DISPOSITIVO_byId(int id) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.DISPOSITIVO
                        where x.ID_DISPOSITIVO == id
                        select x).FirstOrDefault();
            }
        }

        /// <summary>
        /// Da la sugerencia de los dipositivos asignables
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dispositivo obten_DispostivoSugerencia_ById(int id) {
            
            using (IntranetSAIEntities db = new IntranetSAIEntities()){

                

                return (from x in db.DISPOSITIVO
                               where x.ID_DISPOSITIVO == id
                               select new Dispositivo()
                               {
                                   Id = x.ID_DISPOSITIVO
                                   ,
                                   Imei = x.NU_SERIAL
                                   ,
                                   TarjetaSim = x.NU_SIM_GPS
                                   ,
                                   NumeroDVR = x.NU_DVR
                                   ,
                                   VehiculosSeleccionado = (from y in db.VEHICULO
                                                            where y.NU_VIN == x.DE_DISPOSITIVO
                                                            select y.CD_VEHICULO).FirstOrDefault()
                                   , KilometrajeInicial = (from y in db.KILOMETRAJE_TOTAL
                                                           where y.DeviceID == x.ID_DISPOSITIVO
                                                           select y.MontoKilometrajeInicial).FirstOrDefault()
                               }).FirstOrDefault();
                
            }
         }


        /// <summary>
        /// Se busca la información del cliente
        /// </summary>
        /// <param name="id">Id del cliente</param>
        /// <returns></returns>
        public Cliente obten_Cliente_ById(int Id)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

               return (from x in db.CLIENTE
                           where x.CD_CLIENTE == Id
                           select new Cliente {
                                    Id = x.CD_CLIENTE
                                   ,NombreUsuario = x.USUARIO.NM_USUARIO
                                   ,TipoDocumentoSeleccionado = x.USUARIO.TP_DOCUMENTO_IDENTIDAD
                                   ,Cedula = x.USUARIO.NU_DOCUMENTO_IDENTIDAD
                                   ,Email = x.USUARIO.DI_EMAIL_USUARIO
                                   ,Nombre = x.DE_NOMBRE
                                   ,Apellido = x.DE_APELLIDO
                                   ,FechaNacimiento = x.FE_NACIMIENTO
                                   ,NumeroMovil = x.USUARIO.NU_TELEFONO_MOVIL
                                   ,NumeroTelefonoFijo = x.USUARIO.NU_TELEFONO_FIJO
                                   ,NumeroTelefonoOficina = x.NU_TELEFONO_OFICINA
                                   ,NumeroTelefonoPersonaContacto = x.NU_TELEFONO_MOVIL_PERSONA_CONTACTO
                                   ,CompaniaSeguroSeleccionada = x.CD_COMPANIA_SEGURO
                                   ,DescripcionCompaniaSeguro = x.CD_COMPANIA_SEGURO!=null? x.COMPANIA_SEGURO.NM_COMPANIA_SEGURO: null                                                                
                                   ,IndicadorEstaActivo     = !x.USUARIO.IN_USUARIO_INACTIVO 
                                   ,IndicadorEstaRegistrado = x.USUARIO.IN_REGISTRO_MOVIL
                                   ,IndicadorCorreoRegistroEnviado = x.USUARIO.IN_CORREO_REGISTRO_ENVIADO
                                   ,PersonaContacto         = x.NM_PERSONA_CONTACTO
                                   ,TipoSangreSeleccionado = x.TP_SANGRE
                                   ,DescripcionEstaActivo = !x.USUARIO.IN_REGISTRO_MOVIL ? Resources.DescripcionResource.NoRegistrado : (!x.USUARIO.IN_USUARIO_INACTIVO? Resources.DescripcionResource.Activo : Resources.DescripcionResource.Inactivo)
                                   ,Direccion = new Direccion
                                     {  PaisSeleccionado = x.CD_PAIS
                                       ,DescripcionPais = x.CD_PAIS!= null?  x.PAIS.NM_PAIS:null
                                       ,ProvinciaSeleccionada = x.CD_PROVINCIA 
                                       ,DescripcionProvincia = x.CD_PROVINCIA!= null? x.PROVINCIA.NM_PROVINCIA:null 
                                       ,MunicipioSeleccionado = x.CD_DISTRITO  
                                       ,DescripcionMunicipio = x.CD_DISTRITO != null ? x.DISTRITO.NM_DISTRITO : null
                                       ,CorregimientoSeleccionado = x.CD_CORREGIMIENTO
                                       ,DescripcionCorregimiento = x.CD_CORREGIMIENTO!=null?x.CORREGIMIENTO.NM_CORREGIMIENTO:null
                                       ,Calle = x.DI_COBRO
                                     }
                                   ,Dispositivos = (from y in db.DISPOSITIVO_CLIENTE
                                                   orderby y.DISPOSITIVO.DE_DISPOSITIVO 
                                                   where y.CD_CLIENTE == x.CD_CLIENTE
                                                   select new Dispositivo
                                                   {
                                                        Id = y.ID_DISPOSITIVO
                                                       ,FechaRegistro = y.FE_CREACION
                                                      // ,GBHostSpot = y.gbHotSpot
                                                      , Imei = y.DISPOSITIVO.NU_SERIAL
                                                      ,IndicadorAperturaRemota = y.IN_ADMITE_APERTURA_REMOTA
                                                      ,IndicadorEstaActivo = !y.IN_INACTIVO
                                                     // ,LocalizacionBotonSOS = y.locationSOSbutton
                                                      ,NombreDispositivo = y.DISPOSITIVO.NM_DISPOSITIVO
                                                      ,NumeroDVR = y.DISPOSITIVO.NU_DVR
                                                      ,TarjetaSim = y.DISPOSITIVO.NU_SIM_GPS
                                                      //,TipoApagadoRemoto = y.offRemote
                                                      ,Vin = y.CD_VEHICULO!= null?y.VEHICULO.NU_VIN:null 
                                                      ,VehiculosSeleccionado = y.CD_VEHICULO
                                                      ,ContrasenaDVR = y.DE_CONTRASENA_DVR
                                                      ,KilometrajeInicial = (from a in db.KILOMETRAJE_TOTAL
                                                                             where a.DeviceID == y.ID_DISPOSITIVO
                                                                             select a.MontoKilometrajeInicial
                                                                             ).FirstOrDefault()
                                                   }).ToList()
                                 ,Vehiculos = (from z in db.VEHICULO
                                               where z.CD_CLIENTE == x.CD_CLIENTE
                                               orderby z.NU_VIN
                                               select new Vehiculo
                                               {
                                                    Id                          = z.CD_VEHICULO
                                                   ,NumeroPlaca                 = z.NU_PLACA
                                                   ,Vin                         = z.NU_VIN
                                                   ,AnoSeleccionado             = z.CD_ANO
                                                   ,Color                       = z.NM_COLOR
                                                   ,CompaniaSeguroSeleccionada  = z.CD_COMPANIA_SEGURO
                                                   ,MarcaSeleccionada           = z.CD_MARCA
                                                   ,DescripcionMarca            = z.MARCA!= null ?z.MARCA.NM_MARCA:null  
                                                   ,ModeloSeleccionado          = z.CD_MODELO
                                                   ,DescripcionModelo           = z.MODELO!=null? z.MODELO.NM_MODELO:null
                                                   ,TipoCierreSeleccionado      = z.TP_CIERRE_VEHICULO
                                                   ,DescripcionTipoCierre       = z.TP_CIERRE_VEHICULO!= null?z.TIPO_CIERRE_VEHICULO.NM_TIPO_CIERRE_VEHICULO: null
                                                   ,TipoCombustibleSeleccionado = z.TP_COMBUSTIBLE
                                                   ,DescripcionTipoCombustible  = z.TP_COMBUSTIBLE!=null?z.TIPO_COMBUSTIBLE.NM_TIPO_COMBUSTIBLE:null 
                                                   ,TipoTransmisionSeleccionada = z.TP_TRANSMISION_VEHICULO
                                                   ,DescripcionTipoTransmision  = z.TP_TRANSMISION_VEHICULO!= null?z.TIPO_TRANSMISION_VEHICULO.NM_TRANSMISION_VEHICULO:null
                                                   ,VigenciaPoliza              = z.FE_SEGURO_VIGENCIA_HASTA
                                               }).ToList()

                                }
                           ).FirstOrDefault();
            }
        }


        /// <summary>
        /// Retorna las aplicaciones que manja el cliente
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IModeloAplicacion obtenAplicacionesCliente_ById(int Id) {

            Cliente result = new Cliente();
            result.Aplicaciones = new List<Aplicacion>();

            //Se obtienen las aplicaciones del usuario

            List<Aplicacion> aplicacionesUsuario = new List<Aplicacion>();

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                aplicacionesUsuario = (from y in (from x in db.APLICACION
                                                  where x.USUARIO_APLICACION.Select(w => w.CD_USUARIO).Contains(Id)
                                                  select x).ToList()
                                       select new Aplicacion()
                                       {
                                           Id = y.CD_APLICACION
                    ,
                                           IdAplicacionPadre = y.CD_APLICACION_PADRE
                    ,
                                           IndicadorActiva = (y.IN_ES_DASHBOARD|| y.IN_ES_PERFIL_USUARIO)? true : (y.USUARIO_APLICACION.Where(x => x.CD_USUARIO == Id).Select(x => !x.IN_INACTIVO).FirstOrDefault())
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

            //Se buscan las aplicaciones del rol socio comerecial
            List<Aplicacion> aplicacionesRolSocio = new List<Aplicacion>();
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                aplicacionesRolSocio =
               (from y in (from x in db.APLICACION
                           where x.IN_APLICACION_INACTIVA == false
                           && x.IN_ASIGNABLE == true
                           && x.ROL.Select(z => z.CD_ROL.ToString()).Intersect((from w in db.PARAMETRO_CONFIGURACION
                                                                                where w.NM_PARAMETRO == "ROL_CLIENTE"
                                                                                select w.CD_CONFIGURACION
                               ).ToList()).Any() == true
                           select x).ToList()
                select new Aplicacion()
                {
                    Id = y.CD_APLICACION
                    ,
                    IdAplicacionPadre = y.CD_APLICACION_PADRE
                    ,
                    IndicadorActiva = true
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
                }
                            ).ToList();


            }

            aplicacionesRolSocio.ForEach(x => {
                if (!aplicacionesUsuario.Select(w => w.Id).Contains(x.Id))
                {
                    aplicacionesUsuario.Add(x);
                }
            });
            result.Aplicaciones = aplicacionesUsuario;
            return result;
        }


        /// <summary>
        /// Retorna los datos del cliente dado el tipo de documento y el número de documento 
        /// </summary>
        /// <returns></returns>
        public Cliente obten_Cliente_By_NumeroDocumento( string TipoDocumento
                                                        ,string NumeroDocumento) {

            int IdCliente;  
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                IdCliente = (from x in db.CLIENTE
                             where x.USUARIO.TP_DOCUMENTO_IDENTIDAD == TipoDocumento
                             && x.USUARIO.NU_DOCUMENTO_IDENTIDAD == NumeroDocumento                             
                             select x.CD_CLIENTE
                             ).FirstOrDefault();
            }

            return obten_Cliente_ById(IdCliente);
        }


        /// <summary>
        /// Retorna los datos del cliente dado el tipo de documento y el número de documento 
        /// </summary>
        /// <returns></returns>
        public Cliente obten_ClienteNoRegistrado_By_NumeroDocumento(string TipoDocumento
                                                        , string NumeroDocumento)
        {
           using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
              return  (from x in db.USUARIO
                                  where x.TP_DOCUMENTO_IDENTIDAD == TipoDocumento
                                  && x.NU_DOCUMENTO_IDENTIDAD == NumeroDocumento
                                    select new Cliente {
                                         Id  = x.CD_USUARIO
                                        ,NombreApellidoUsuario      = x. DE_NOMBRE_APELLIDO 
                                        ,TipoDocumentoSeleccionado  = x.TP_DOCUMENTO_IDENTIDAD
                                        ,Cedula                     = x.NU_DOCUMENTO_IDENTIDAD
                                        ,FechaNacimiento            =  x.FE_NACIMIENTO
                                        ,NumeroTelefonoFijo         = x.NU_TELEFONO_FIJO
                                        ,NumeroMovil                = x.NU_TELEFONO_MOVIL
                                        ,Email                      = x.DI_EMAIL_USUARIO
                                    }
                                 ).FirstOrDefault();
            }            
        }


        /// <summary>
        /// Obtiene un usuario de la aplicación SAI de la base de datos de PanamaMain
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CLIENTE obten_CLIENTE_ById(int Id)
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.CLIENTE
                        where x.CD_CLIENTE == Id
                        select x).FirstOrDefault();

            }
        }


        //Obtiene el distribuidor
        public DISTRIBUIDOR obten_DISTRIBUIDOR_ById(int Id)
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                return (from x in db.DISTRIBUIDOR
                        where x.ID_DISTRIBUIDOR == Id
                        select x).FirstOrDefault();
            }

        }

        //Obtiene el Id de los distribuidores que tiene relacionado
        public List<int> obten_IdDistribuidoresCliente_ByClienteId(int Id) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (from x in db.SOCIO_COMERCIAL
                        where x.IN_INACTIVO == false
                        && x.USUARIO.USUARIO1.Select(y => y.CD_USUARIO).Contains(Id)
                        select x.CD_SOCIO_COMERCIAL
                        ).ToList();
            }
        }

        
        /// <summary>
        /// Permite actualizar la información del cliente 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public bool guarda_Cliente(ref Cliente clienteRef)
        {
            bool result = false;
            AuthRepositorio authRepo = new AuthRepositorio();

            ValidacionRepositorio validaRepo = new ValidacionRepositorio();

            DateTime feActual = DateTime.Now;
            Cliente cliente = clienteRef;

            string contrasenaNueva = Core.Utils.UtilHelper.CreateRandomPassword(8);
            String nombreUsuarioNuevo;

            //Se actualizan las tablas de Panama main
            using (IntranetSAIEntities db = new IntranetSAIEntities()){

                using (var dbContextTransaction = db.Database.BeginTransaction()){
                    try{
                       
                        //Se busca primero el usuario
                        USUARIO usuario =  (from x in db.USUARIO
                                             where x.CD_USUARIO == cliente.Id
                                             select x).FirstOrDefault();
                        
                        if (usuario == null|| usuario.CD_USUARIO ==0){

                            usuario = new USUARIO();
                            usuario.CD_USUARIO = Core.Respositorios.UtilRepositorio.obtenSecuenciaUsuario();


                            nombreUsuarioNuevo = cliente.Email.Split('@')[0];
                            if (!validaRepo.usuarioExiste(cliente.Id,nombreUsuarioNuevo))
                                usuario.NM_USUARIO = nombreUsuarioNuevo;
                            else
                            {
                                int cont = 1;
                                while (validaRepo.usuarioExiste(cliente.Id,nombreUsuarioNuevo + cont)) { cont++; }
                                usuario.NM_USUARIO = nombreUsuarioNuevo + cont;
                            }
                            
                            usuario.IN_USUARIO_BLOQUEADO = false;
                            usuario.IN_USUARIO_INACTIVO = false;
                            usuario.IN_REGISTRO_MOVIL = true;
                            usuario.FE_REGISTRO_MOVIL = feActual;
                            usuario.DE_CONTRASENA = authRepo.encriptaCadena(contrasenaNueva);
                            usuario.DE_NOMBRE_APELLIDO = cliente.Nombre + " " + cliente.Apellido;
                            usuario.DI_EMAIL_USUARIO = cliente.Email;
                            usuario.FE_CREACION = feActual;
                            usuario.FE_ESTATUS = feActual;
                            usuario.FE_NACIMIENTO = cliente.FechaNacimiento;
                            usuario.NU_DOCUMENTO_IDENTIDAD = cliente.Cedula;
                            usuario.NU_TELEFONO_FIJO = cliente.NumeroTelefonoFijo;
                            usuario.NU_TELEFONO_MOVIL = cliente.NumeroMovil;
                            usuario.TP_DOCUMENTO_IDENTIDAD = cliente.TipoDocumentoSeleccionado;

                         
                            db.USUARIO.Add(usuario);

                            db.SaveChanges();
                        }
                        else {
                            usuario.FE_NACIMIENTO           = cliente.FechaNacimiento;
                            usuario.IN_USUARIO_BLOQUEADO    = false;
                            usuario.IN_USUARIO_INACTIVO     = false;
                            usuario.FE_ESTATUS              = feActual;
                            db.USUARIO.Attach(usuario);
                            db.Entry(usuario).State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        
                        //Se guarda la información del socio
                        cliente.Id = usuario.CD_USUARIO;
                        cliente.IndicadorEstaActivo = true;

                        CLIENTE clienteDB = cliente.toModel(usuario);
                        clienteDB.FE_CREACION = feActual;
                        db.CLIENTE.Add(clienteDB);

                        db.SaveChanges();

                        IEnumerable<int> sociosSeleccionados = cliente.SocioComercialSeleccionados == null ? new List<int>() : cliente.SocioComercialSeleccionados;
                        //SE ACTUALIZA LA INFORMACIÓN DEL SOCIO COMERCIAL
                        List<USUARIO> sociosComercialesAEliminar = (from x in db.USUARIO
                                                                    where x.USUARIO1.Select(y => y.CD_USUARIO).Contains(usuario.CD_USUARIO)
                                                                    && x.SOCIO_COMERCIAL != null
                                                                    && x.USUARIO2.Select(y => y.CD_USUARIO)
                                                                    .Intersect(sociosSeleccionados).Any() == false
                                                                    select x).ToList();

                        sociosComercialesAEliminar.ForEach(x => usuario.USUARIO2.Remove(x) );
                   
                        db.SaveChanges();

                        //SE ACTUALIZA LA INFORMACIÓN DEL SOCIO COMERCIAL
                        List<USUARIO> sociosComercialesAAgregar = (from x in db.USUARIO
                                                                    where sociosSeleccionados.Contains(x.CD_USUARIO)
                                                                   select x).ToList();

                        sociosComercialesAAgregar.ForEach(x => usuario.USUARIO2.Add(x));
                        db.SaveChanges();
                        
                        //Se buscan los roles que deban serle otorgados
                        List<ROL> roles = (from x in db.ROL
                                           from y in db.PARAMETRO_CONFIGURACION
                                           where !x.USUARIO.Select(y => y.CD_USUARIO).Contains(usuario.CD_USUARIO)
                                           && y.CD_CONFIGURACION == x.CD_ROL.ToString()
                                           && y.NM_PARAMETRO == "ROL_CLIENTE"
                                           select x ).ToList();

                        roles.ForEach(x => usuario.ROL.Add(x));
                        db.SaveChanges();
                        
                        //Se actualizan las aplicaciones
                        foreach (var aplicacionCliente in cliente.Aplicaciones)
                        {

                            var aplicacionDB = (from x in db.USUARIO_APLICACION
                                                where x.CD_APLICACION == aplicacionCliente.Id
                                                && x.CD_USUARIO == usuario.CD_USUARIO
                                                select x).FirstOrDefault();

                            if (aplicacionDB != null){

                                aplicacionDB.FE_ESTATUS = feActual;
                                aplicacionDB.IN_INACTIVO = !aplicacionCliente.IndicadorActiva;                                
                                db.SaveChanges();
                            }
                        }
                                                
                        dbContextTransaction.Commit();
                        result = true;
                    }
                    catch(System.Exception exc){
                        log.Error(exc);
                        dbContextTransaction.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Permite actualizar la información del cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public bool actualiza_Cliente(ref Cliente clienteRef) {
            bool result = false;

            DateTime feActual = DateTime.Now;
            Cliente cliente = clienteRef;

            //Se actualizan las tablas de Panama main
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var dbContextTransaction = db.Database.BeginTransaction()){

                    try{
                        //Se busca primero el usuario
                        CLIENTE clienteDB = (from x in db.CLIENTE
                                           where x.CD_CLIENTE == cliente.Id
                                           select x).FirstOrDefault();

                        clienteDB.USUARIO.FE_NACIMIENTO = cliente.FechaNacimiento;
                        clienteDB.USUARIO.NU_TELEFONO_FIJO = cliente.NumeroTelefonoFijo;
                        clienteDB.USUARIO.NU_TELEFONO_MOVIL = cliente.NumeroMovil;
                        
                        clienteDB.USUARIO.FE_ESTATUS = feActual;
                        clienteDB.USUARIO.DI_EMAIL_USUARIO = cliente.Email;

                        clienteDB.IN_INACTIVO = !cliente.IndicadorEstaActivo;
                        clienteDB.FE_NACIMIENTO = cliente.FechaNacimiento;
                        clienteDB.DE_NOMBRE = cliente.Nombre;
                        clienteDB.DE_APELLIDO = cliente.Apellido;
                        clienteDB.NU_TELEFONO_OFICINA = cliente.NumeroTelefonoOficina;
                        clienteDB.TP_SANGRE = cliente.TipoSangreSeleccionado;
                        clienteDB.NM_PERSONA_CONTACTO = cliente.PersonaContacto;
                        clienteDB.NU_TELEFONO_MOVIL_PERSONA_CONTACTO = cliente.NumeroTelefonoPersonaContacto;
                        clienteDB.CD_COMPANIA_SEGURO    = cliente.CompaniaSeguroSeleccionada;
                        clienteDB.CD_PAIS               = cliente.Direccion.PaisSeleccionado;
                        clienteDB.CD_PROVINCIA          = cliente.Direccion.ProvinciaSeleccionada;
                        clienteDB.CD_DISTRITO           = cliente.Direccion.MunicipioSeleccionado;
                        clienteDB.CD_CORREGIMIENTO      = cliente.Direccion.CorregimientoSeleccionado;
                        clienteDB.DI_COBRO              = cliente.Direccion.Calle;

                        if (cliente.IndicadorEstaActivo)
                        {
                            clienteDB.USUARIO.IN_USUARIO_INACTIVO = !cliente.IndicadorEstaActivo;
                            clienteDB.FE_INACTIVO = null;
                        }
                        db.SaveChanges();

                        //Se buscan los roles que deban serle otorgados
                        if (cliente.IndicadorEstaActivo)
                        {
                            List<ROL> roles = (from x in db.ROL
                                               from y in db.PARAMETRO_CONFIGURACION
                                               where !x.USUARIO.Select(y => y.CD_USUARIO).Contains(clienteDB.USUARIO.CD_USUARIO)
                                               && y.CD_CONFIGURACION == x.CD_ROL.ToString()
                                               && y.NM_PARAMETRO == "ROL_CLIENTE"
                                               select x
                                                      ).ToList();

                            roles.ForEach(x => clienteDB.USUARIO.ROL.Add(x));
                        }
                        
                        db.SaveChanges();

                        //Se actualizan los vehículos 
                        cliente.Vehiculos.ToList().ForEach(x=> {
                            var vehiculoDB = (from y in db.VEHICULO
                                              where y.CD_VEHICULO == x.Id
                                              select y).FirstOrDefault();

                            vehiculoDB.NU_PLACA = x.NumeroPlaca;
                            vehiculoDB.CD_MARCA = x.MarcaSeleccionada;
                            vehiculoDB.CD_MODELO = x.ModeloSeleccionado;
                            vehiculoDB.NM_COLOR = x.Color;
                            vehiculoDB.CD_ANO = x.AnoSeleccionado;
                            vehiculoDB.CD_COMPANIA_SEGURO = x.CompaniaSeguroSeleccionada;
                            vehiculoDB.TP_TRANSMISION_VEHICULO = x.TipoTransmisionSeleccionada;
                            vehiculoDB.TP_CIERRE_VEHICULO = x.TipoCierreSeleccionado;
                            vehiculoDB.TP_COMBUSTIBLE = x.TipoCombustibleSeleccionado;
                            vehiculoDB.FE_SEGURO_VIGENCIA_HASTA = x.VigenciaPoliza;
                            db.SaveChanges();
                        });

                        //Se actualizan los dispositivos
                        cliente.Dispositivos.ToList().ForEach(
                            x => {

                                var dispositivoDB = (from y in db.DISPOSITIVO_CLIENTE
                                                  where y.ID_DISPOSITIVO == x.Id
                                                  select y).FirstOrDefault();

                                dispositivoDB.DE_CONTRASENA_DVR = x.ContrasenaDVR;
                                dispositivoDB.CD_VEHICULO = x.VehiculosSeleccionado;
                                dispositivoDB.IN_ADMITE_APERTURA_REMOTA  = x.IndicadorAperturaRemota;
                                
                                db.SaveChanges();

                                var kilometrajeBD = (from y in db.KILOMETRAJE_TOTAL
                                                     where y.DeviceID == x.Id
                                                     select y).FirstOrDefault();

                                if (kilometrajeBD != null) {
                                    kilometrajeBD.MontoKilometrajeInicial = x.KilometrajeInicial;
                                    db.SaveChanges();
                                }
                            });


                        

                        dbContextTransaction.Commit();
                        result = true;

                    }
                    catch (System.Exception exc)
                    {
                        log.Error(exc);
                        dbContextTransaction.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Permite actualizar los datos adicionales de un cliente
        /// </summary>
        /// <param name="clienteRef"></param>
        /// <returns></returns>
        public bool actualiza_DatosAdicionalesCliente(ref Cliente clienteRef)
        {
            bool result = false;

            DateTime feActual = DateTime.Now;
            Cliente cliente = clienteRef;
            cliente.SocioComercialSeleccionados = cliente.SocioComercialSeleccionados == null ? new List<int>():cliente.SocioComercialSeleccionados;
            //Se actualizan las tablas de Panama main
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {

                    try
                    {
                        //Se busca primero el usuario
                        CLIENTE clienteDB = (from x in db.CLIENTE
                                             where x.CD_CLIENTE == cliente.Id
                                             select x).FirstOrDefault();

                        //Se actualiza sus usuarios padres
                        clienteDB.USUARIO.USUARIO2 =  clienteDB.USUARIO.USUARIO2.Union(
                            (from x in db.SOCIO_COMERCIAL
                             where cliente.SocioComercialSeleccionados.Contains(x.CD_SOCIO_COMERCIAL)
                             select x.USUARIO)                            
                            ).ToList();

                        clienteDB.USUARIO.USUARIO2.ToList().Where(x => !cliente.SocioComercialSeleccionados.Contains(x.CD_USUARIO)
                        && x.SOCIO_COMERCIAL != null).ToList().ForEach(x => { clienteDB.USUARIO.USUARIO2.Remove(x); });
                        
                        db.SaveChanges();
                        
                        cliente.Aplicaciones.ToList().ForEach(x => {
                            var aplicacionDB = (from y in db.USUARIO_APLICACION
                                                where y.CD_APLICACION == x.Id
                                                && y.CD_USUARIO == cliente.Id
                                                select y).FirstOrDefault();

                            if (aplicacionDB != null){
                                aplicacionDB.FE_ESTATUS = feActual;
                                aplicacionDB.IN_INACTIVO = !x.IndicadorActiva;
                                
                                db.SaveChanges();
                            }
                        });
                       

                        dbContextTransaction.Commit();
                        result = true;

                    }
                    catch (System.Exception exc)
                    {
                        log.Error(exc);
                        dbContextTransaction.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }




        /// <summary>
        /// Almacen al información del nuevo vehículo
        /// </summary>
        /// <param name="vehiculo"></param>
        /// <returns></returns>
        public bool guarda_Vehiculo(ref Vehiculo vehiculo) {
            bool result = false;

            using (var db = new IntranetSAIEntities())
            {
                VEHICULO vehiculoDB =  vehiculo.toModel();
                db.VEHICULO.Add(vehiculoDB);
                int total = db.SaveChanges();
                result = total > 0 ? true : result;
                vehiculo.Id = vehiculoDB.CD_VEHICULO;
            }

            return result;
        }


        public bool guarda_Dispositivo(ref Dispositivo dispositivo) {
            bool result = false;
            DateTime fechaActual = DateTime.Now;
            using (var db = new IntranetSAIEntities()) {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try{
                       DISPOSITIVO_CLIENTE dispositivoDB =  dispositivo.toModel();
                        dispositivoDB.FE_CREACION = fechaActual;
                        dispositivoDB.FE_ESTATUS = fechaActual;
                        dispositivoDB.IN_INACTIVO = false;


                        dispositivoDB.DISPOSITIVO = (from x in db.DISPOSITIVO
                                         where x.ID_DISPOSITIVO == dispositivoDB.ID_DISPOSITIVO
                                         select x).FirstOrDefault();

                        dispositivoDB.CLIENTE = (from x in db.CLIENTE
                                                     where x.CD_CLIENTE == dispositivoDB.CD_CLIENTE
                                                     select x).FirstOrDefault();

                        //Se asocia el dispositivo al cliente
                        db.DISPOSITIVO_CLIENTE.Add(dispositivoDB);
                        db.SaveChanges();


                        //Se asocia el dispostivo al monitor
                        if (dispositivoDB.CLIENTE.USUARIO.DISPOSITIVO.Where(x => x.ID_DISPOSITIVO == dispositivoDB.ID_DISPOSITIVO).Count()==0)
                            dispositivoDB.CLIENTE.USUARIO.DISPOSITIVO.Add(dispositivoDB.DISPOSITIVO);
                        
                        db.SaveChanges();

                        //Se actualiza el kilometraje inicial del dispositivo

                        KILOMETRAJE_TOTAL ki = (from x in db.KILOMETRAJE_TOTAL
                                                where x.DeviceID == dispositivoDB.ID_DISPOSITIVO
                                                select x).FirstOrDefault();

                        if (ki != null){
                            ki.MontoKilometrajeInicial = (double)dispositivo.KilometrajeInicial;
                            db.SaveChanges();
                        }

                        
                        dbContextTransaction.Commit();
                        result = true;
                    }
                    catch (System.Exception exc) {
                        log.Error(exc);
                        dbContextTransaction.Rollback();
                        result = false;
                    }
               }
            }

            return result;
        }
        
   
        /// <summary>
        /// Permite geenrar excel con el listado de clientes
        /// </summary>
        /// <returns></returns>
        public byte[] generarExcelListaClientes()
        {
            byte[] salida = null;

            //Se obtiene el listado de clientes

            IEnumerable<Cliente> listadoClientesTemp = obtenClientes();
            IList<Cliente> listadoClientes = new List<Cliente>();
            listadoClientesTemp.ToList().ForEach(x => {
                var cliente = obten_Cliente_ById(x.Id);   //x.IndicadorEstaRegistrado ? obten_Cliente_byId(x.Id) : obten_Cliente_by_sysCutomerDocId(x.Id);
                    cliente.ListadoSims = x.ListadoSims;
                    listadoClientes.Add(cliente);
                });

           
                using (ExcelPackage pck = new ExcelPackage())
                {
                    pck.Workbook.Properties.Author = "Sai Innovación";
                    pck.Workbook.Properties.Title = Resources.TituloPaginaResource.Cliente;
                    var sheetDatosGenerales = pck.Workbook.Worksheets.Add(Resources.TituloWidget.DatosGenerales);

                    sheetDatosGenerales.Cells[1, 1].Value = Resources.CampoResource.Cedula;
                    sheetDatosGenerales.Cells[1, 2].Value = Resources.CampoResource.Nombre;
                    sheetDatosGenerales.Cells[1, 3].Value = Resources.CampoResource.Apellido;
                    sheetDatosGenerales.Cells[1, 4].Value = Resources.CampoResource.FechaNacimiento;
                    sheetDatosGenerales.Cells[1, 5].Value = Resources.CampoResource.NumeroTelefonoFijo;
                    sheetDatosGenerales.Cells[1, 6].Value = Resources.CampoResource.NumeroMovil;
                    sheetDatosGenerales.Cells[1, 7].Value = Resources.CampoResource.NumeroTelefonoOficina;
                    sheetDatosGenerales.Cells[1, 8].Value = Resources.CampoResource.TipoSangre;
                    sheetDatosGenerales.Cells[1, 9].Value = Resources.CampoResource.PersonaContacto;
                    sheetDatosGenerales.Cells[1,10].Value = Resources.CampoResource.NumeroTelefonoPersonaContacto;
                    sheetDatosGenerales.Cells[1, 11].Value = Resources.CampoResource.CompaniaSeguro;
                    sheetDatosGenerales.Cells[1, 12].Value = Resources.CampoResource.Email;
                    sheetDatosGenerales.Cells[1, 13].Value = Resources.CampoResource.Sims;
                    sheetDatosGenerales.Cells[1, 14].Value = Resources.CampoResource.IndicadorEstaActivo;

                    //Datos de la dirección
                    sheetDatosGenerales.Cells[1, 15].Value = Resources.CampoResource.Pais;
                    sheetDatosGenerales.Cells[1, 16].Value = Resources.CampoResource.Provincia;
                    sheetDatosGenerales.Cells[1, 17].Value = Resources.CampoResource.Distrito;
                    sheetDatosGenerales.Cells[1, 18].Value = Resources.CampoResource.Corregimiento;
                    sheetDatosGenerales.Cells[1, 19].Value = Resources.CampoResource.Calle;

                using (ExcelRange rng = sheetDatosGenerales.Cells["A1:S1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                var sheetVehiculos = pck.Workbook.Worksheets.Add(Resources.TituloWidget.Vehiculo);

                sheetVehiculos.Cells[1, 1].Value = Resources.CampoResource.Cedula;
                sheetVehiculos.Cells[1, 2].Value = Resources.CampoResource.Nombre;
                sheetVehiculos.Cells[1, 3].Value = Resources.CampoResource.Apellido;
                sheetVehiculos.Cells[1, 4].Value = Resources.CampoResource.Email;
                sheetVehiculos.Cells[1, 5].Value = Resources.CampoResource.NumeroChasis;
                sheetVehiculos.Cells[1, 6].Value = Resources.CampoResource.NumeroPlaca;
                sheetVehiculos.Cells[1, 7].Value = Resources.CampoResource.Marca;
                sheetVehiculos.Cells[1, 8].Value = Resources.CampoResource.Modelo;
                sheetVehiculos.Cells[1, 9].Value = Resources.CampoResource.Color;
                sheetVehiculos.Cells[1, 10].Value = Resources.CampoResource.Ano;
                sheetVehiculos.Cells[1, 11].Value = Resources.CampoResource.CompaniaSeguro;
                sheetVehiculos.Cells[1, 12].Value = Resources.CampoResource.TipoTransmision;
                sheetVehiculos.Cells[1, 13].Value = Resources.CampoResource.TipoCierre;
                sheetVehiculos.Cells[1, 14].Value = Resources.CampoResource.TipoCombustible;
                sheetVehiculos.Cells[1, 15].Value = Resources.CampoResource.VigenciaPoliza;

                using (ExcelRange rng = sheetVehiculos.Cells["A1:O1"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                        rng.Style.Font.Color.SetColor(Color.White);
                    }

                    var rowIndex = 2; var rowIndexVeh = 2;
                    foreach (var item in listadoClientes)
                    {
                        var col = 1;
                        if (item != null)
                        {
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Documento;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Nombre;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Apellido;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.FechaNacimiento;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.NumeroTelefonoFijo;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.NumeroMovil;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.NumeroTelefonoOficina;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.TipoSangreSeleccionado;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.PersonaContacto;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.NumeroTelefonoPersonaContacto;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.DescripcionCompaniaSeguro;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Email;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.ListadoSims;
                            sheetDatosGenerales.Cells[rowIndex, col++].Value = item.IndicadorEstaActivo ? Resources.DescripcionResource.Si : Resources.DescripcionResource.No;

                            if (item.Direccion != null)
                            {
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Direccion.DescripcionPais;
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Direccion.DescripcionProvincia;
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Direccion.DescripcionMunicipio;
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Direccion.DescripcionCorregimiento;
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = item.Direccion.Calle;
                            }
                            else
                            {
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = "";
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = "";
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = "";
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = "";
                                sheetDatosGenerales.Cells[rowIndex, col++].Value = "";
                            }
                            rowIndex++;

                        if (item.Vehiculos != null)
                        {


                            foreach (var vehiculo in item.Vehiculos)
                            {
                                var colVeh = 1;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = item.Documento;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = item.Nombre;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = item.Apellido;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = item.Email;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.Vin;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.NumeroPlaca;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.DescripcionMarca;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.DescripcionModelo;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.Color;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.AnoSeleccionado;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.DescripcionCompaniaSeguro;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.DescripcionTipoTransmision;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.DescripcionTipoCierre;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.DescripcionTipoCombustible;
                                sheetVehiculos.Cells[rowIndexVeh, colVeh++].Value = vehiculo.VigenciaPoliza;
                                rowIndexVeh++;
                            }
                        }
                    }                    
                    
                    //Se coloca el listado de vehículos
                  
                    }

                sheetVehiculos.Column(15).Style.Numberformat.Format = Core.Constante.AppFormat.dateJson;
                sheetDatosGenerales.Column(4).Style.Numberformat.Format = Core.Constante.AppFormat.dateJson;

                salida = pck.GetAsByteArray();

                }
        
            return salida;
        }
    }
}