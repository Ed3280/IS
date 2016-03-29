using System.Linq;
using System.Data.Entity;
using IntranetWeb.Models;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using IntranetWeb.ViewModel.Auth;
using System;

namespace IntranetWeb.Core.Respositorios
{
    public class AuthRepositorio
    {
        public AuthRepositorio() { }

        /// <summary>
        /// Se obtiene el usuario por nombre de usuario y contraseña
        /// </summary>
        /// <param name="username"></param>
        /// <param name="contrasena"></param>
        /// <returns></returns>
        public USUARIO obten_USUARIO_ByNombreUsuarioCorreo(string usernameCorreo
                                                    ,string contrasena) {

            USUARIO result = null;
            using ( IntranetSAIEntities db = new IntranetSAIEntities()) {
                result = ( from x in db.USUARIO
                         where (x.NM_USUARIO == usernameCorreo
                         || x.DI_EMAIL_USUARIO.Trim().ToUpper() == usernameCorreo.Trim().ToUpper()
                         )                         
                         && x.IN_USUARIO_INACTIVO == false
                         && x.IN_USUARIO_BLOQUEADO == false                         
                         select x
                       ).Include(x=> x.ROL).FirstOrDefault();


                if (result != null) {
                    //Se valida si la clave es la correcta
                    string keyName = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("PASSWORD_KEY");
                    ObjectParameter salida = new ObjectParameter("P_SALIDA", typeof(bool));

                    db.PROC_001_ES_VALIDA_CADENA_ENCRIPTADA(result.DE_CONTRASENA, contrasena, keyName, salida);

                    if (!(bool)salida.Value){
                        result = null;
                    }                   
                }
            }
            return result;
        }


        /// <summary>
        /// ObtiEne el usuario por su nombre de usuario
        /// </summary>
        /// <param name="username"> Nombre de Usuario</param>
        /// <returns>Objeto USUARIO</returns>
        public USUARIO obten_USUARIO_ByUserName(string username) {
            USUARIO usuario = null;

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                usuario = (from x in db.USUARIO
                           where x.NM_USUARIO == username
                           select x).FirstOrDefault(); 

            }
            return usuario;
        }
        
        /// <summary>
        /// Obtiene el usuario por su nombre de usuario
        /// </summary>
        /// <param name="usernameCorreo"> Nombre de Usuario</param>
        /// <returns>Objeto USUARIO</returns>
        public USUARIO obten_USUARIO_ByUserNameEmail(string usernameCorreo)
        {
            USUARIO usuario = null;

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                usuario = (from x in db.USUARIO
                           where (x.NM_USUARIO == usernameCorreo
                            || x.DI_EMAIL_USUARIO.Trim().ToUpper() == usernameCorreo.Trim().ToUpper()
                           )
                           select x).FirstOrDefault();

            }
            return usuario;
        }

        /// <summary>
        /// Permite actualizar una entidad usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int actualiza_USUARIO(USUARIO usuario){

            using (var db = new IntranetSAIEntities()){
                db.USUARIO.Attach(usuario);                
                db.Entry(usuario).State = EntityState.Modified;
                return db.SaveChanges();
            }

        }

        /// <summary>
        /// Retornar el objeto USUARIO dado el Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public USUARIO obten_USUARIO_ById(int Id) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.USUARIO
                        where x.CD_USUARIO == Id
                        select x).Include(x=>x.EMPLEADO).FirstOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el password en un string
        /// </summary>
        /// <param name="passwordByte"></param>
        /// <returns></returns>
        public string obtenPassword(byte[] passwordByte) {
            string password = "";

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                ObjectParameter salida = new ObjectParameter("P_SALIDA", typeof(byte[]));


                db.PROC_002_OBTENER_CONTRASENA(passwordByte
                    , salida);

                    password = System.Text.Encoding.UTF8.GetString((byte[])salida.Value);

            }
                return password;
        }

        /// <summary>
        /// Encripta una cadena de caracteres
        /// </summary>
        /// <param name="cadenaAEncriptar"></param>
        /// <returns></returns>
        public byte[] encriptaCadena(string cadenaAEncriptar) {

            byte[] cadenaEncriptada = null;

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {

                ObjectParameter salida = new ObjectParameter("P_SALIDA", typeof(byte[]));
                string keyName = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("PASSWORD_KEY");

                db.PROC_003_OBTENER_CADENA_ENCRIPTADA(keyName, cadenaAEncriptar, salida);
                cadenaEncriptada = (byte[])salida.Value;
            }
            return cadenaEncriptada;
        }
        
        
        /// <summary>
        /// Retorna el listado de aplicaciones activas que tiene el usuario
        /// </summary>
        /// <param name="idUsuario">Id del usuario del cuál se quieren conocer las aplicaciones</param>
        /// <returns></returns>
        public IList<string> obtenListadoAplicaciones_ById(int idUsuario) {
            IList<string> result = new List<string>();
            
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                result = (from x in db.USUARIO_APLICACION
                          join y in db.APLICACION on x.CD_APLICACION equals y.CD_APLICACION
                          where y.IN_APLICACION_INACTIVA == false
                          && x.CD_USUARIO == idUsuario
                          select y.DE_ALIAS).ToList();
            }
                return result;
        }


        /// <summary>
        /// Realiza la busqueda de las opciones del menu
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public IQueryable<NodoAplicacion> obtenAplicacionesMenu(int idUsuario) {
            return getNodoAplicacion(idUsuario,null);
        }

        /// <summary>
        /// Busca el ar
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="cdAplicacionPadre"></param>
        /// <returns></returns>
        private IQueryable<NodoAplicacion> getNodoAplicacion(int idUsuario
                                                        , int? cdAplicacionPadre) {

            IQueryable<NodoAplicacion> nodos = new List<NodoAplicacion>().AsQueryable();
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                if (cdAplicacionPadre == null)
                {
                    //Se obtiene el dashboard
                   var dashboard = (from x in db.USUARIO_APLICACION.ToList().AsQueryable()
                             join y in db.APLICACION.ToList().AsQueryable() on x.CD_APLICACION equals y.CD_APLICACION
                             orderby y.NU_ORDEN ascending, y.NM_APLICACION
                             where x.CD_USUARIO == idUsuario
                             && x.IN_INACTIVO == false
                             && y.IN_APLICACION_INACTIVA == false
                             && y.CD_APLICACION_PADRE == null
                             && y.IN_ES_DASHBOARD == true
                             select new NodoAplicacion
                             {
                                 IdAplicacion = x.CD_APLICACION
                                 ,
                                 Accion = y.NM_ACCION
                                 ,
                                 Controlador = y.NM_CONTROLADOR
                                 ,
                                 IconoAccesoRapido = y.NM_ICONO_ACCESO_RAPIDO
                                 ,
                                 IconoMenu = y.NM_ICONO_MENU
                                 ,
                                 NombreAplicacion = y.NM_APLICACION
                                 ,
                                 NumeroOrden = y.NU_ORDEN
                                 ,
                                 Parametros = y.DE_PARAMETROS
                                 ,
                                 AplicacionPadre = y.CD_APLICACION_PADRE
                                 ,
                                 AplicacionesHijas = new List<NodoAplicacion>().AsQueryable()
                             }
                             ).FirstOrDefault();


                    nodos = (from x in db.USUARIO_APLICACION.ToList().AsQueryable()
                             join y in db.APLICACION.ToList().AsQueryable() on x.CD_APLICACION equals y.CD_APLICACION
                             orderby y.NU_ORDEN ascending, y.NM_APLICACION
                             where x.CD_USUARIO == idUsuario
                             && x.IN_INACTIVO == false
                             && y.IN_APLICACION_INACTIVA == false
                             && y.CD_APLICACION_PADRE == null
                             && y.IN_ES_DASHBOARD == false
                             select new NodoAplicacion {
                                 IdAplicacion = x.CD_APLICACION
                                 , Accion = y.NM_ACCION
                                 , Controlador = y.NM_CONTROLADOR
                                 , IconoAccesoRapido = y.NM_ICONO_ACCESO_RAPIDO
                                 , IconoMenu = y.NM_ICONO_MENU
                                 , NombreAplicacion = y.NM_APLICACION
                                 , NumeroOrden = y.NU_ORDEN
                                 , Parametros = y.DE_PARAMETROS
                                 , AplicacionPadre = y.CD_APLICACION_PADRE
                                 , AplicacionesHijas = getNodoAplicacion(idUsuario, y.CD_APLICACION)
                             }
                             );
                    if (dashboard != null)
                    {
                        List<NodoAplicacion> dashBoardlist = new List<NodoAplicacion>();
                        dashBoardlist.Add(dashboard);
                        nodos = dashBoardlist.Union(nodos).AsQueryable();
                    }
                }
                else {

                    nodos = (from x in db.USUARIO_APLICACION.ToList().AsQueryable()
                             join y in db.APLICACION.ToList().AsQueryable() on x.CD_APLICACION equals y.CD_APLICACION
                             orderby y.NU_ORDEN ascending, y.NM_APLICACION
                             where x.CD_USUARIO == idUsuario
                             && x.IN_INACTIVO == false
                             && y.IN_APLICACION_INACTIVA == false
                             && y.CD_APLICACION_PADRE ==  cdAplicacionPadre
                             select new NodoAplicacion
                             {
                                  IdAplicacion = x.CD_APLICACION
                                 ,
                                 Accion = y.NM_ACCION
                                 ,
                                 Controlador = y.NM_CONTROLADOR
                                 ,
                                 IconoAccesoRapido = y.NM_ICONO_ACCESO_RAPIDO
                                 ,
                                 IconoMenu = y.NM_ICONO_MENU
                                 ,
                                 NombreAplicacion = y.NM_APLICACION
                                 ,
                                 NumeroOrden = y.NU_ORDEN
                                 
                                 ,
                                 AplicacionPadre = y.CD_APLICACION_PADRE
                                 ,
                                 Parametros = y.DE_PARAMETROS
                                 ,
                                 AplicacionesHijas = (
                                                          from w in db.USUARIO_APLICACION.ToList().AsQueryable()
                                                          join z in db.APLICACION.ToList().AsQueryable() on w.CD_APLICACION equals z.CD_APLICACION
                                                          where w.CD_USUARIO == idUsuario
                                                          && w.IN_INACTIVO == false
                                                          && z.IN_APLICACION_INACTIVA == false
                                                          && z.CD_APLICACION_PADRE == x.CD_APLICACION
                                                          select x
                                                        ).Count() == 0 ? new List<NodoAplicacion>().AsQueryable() :
                                                        getNodoAplicacion(idUsuario, x.CD_APLICACION)
                             }
                             ).ToList().AsQueryable();
                }
            }


            var nodosConHijos = (from x in nodos
                                 where !String.IsNullOrWhiteSpace(x.Accion) || x.AplicacionesHijas.Count() != 0
                                 select x
                                 );

           

            return nodosConHijos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <returns></returns>
        public NodoAplicacion obtenDashBoardUsuario(int cdUsuario) {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return (from x in db.USUARIO_APLICACION
                        where x.CD_USUARIO == cdUsuario
                        && x.IN_INACTIVO == false
                        && x.APLICACION.IN_APLICACION_INACTIVA == false
                        && x.APLICACION.IN_ES_DASHBOARD == true
                        select new NodoAplicacion
                        {
                            Accion = x.APLICACION.NM_ACCION
                            ,
                            Controlador = x.APLICACION.NM_CONTROLADOR
                            ,
                            IdAplicacion = x.CD_APLICACION

                        }).FirstOrDefault();
            }

        }

        /// <summary>
        /// Obtiene las apliciones atajo
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public IQueryable<NodoAplicacion> obtenAplicacionesAtajo(int idUsuario) {

            using (IntranetSAIEntities db =  new  IntranetSAIEntities() )
            {

                var dashboard = (from x in db.APLICACION
                             join y in db.USUARIO_APLICACION on
                             x.CD_APLICACION equals y.CD_APLICACION
                             orderby x.NU_ORDEN ascending
                             where x.IN_APLICACION_INACTIVA == false
                             && y.IN_INACTIVO == false
                             && y.CD_USUARIO == idUsuario
                             && x.IN_ES_DASHBOARD == true
                             && !(x.NM_ICONO_ACCESO_RAPIDO == null || x.NM_ICONO_ACCESO_RAPIDO.Trim() == string.Empty)
                             select new NodoAplicacion
                             {
                                 IdAplicacion = y.CD_APLICACION
                                    ,
                                 Accion = x.NM_ACCION
                                    ,
                                 Controlador = x.NM_CONTROLADOR
                                    ,
                                 IconoAccesoRapido = x.NM_ICONO_ACCESO_RAPIDO
                                    ,
                                 IconoMenu = x.NM_ICONO_MENU
                                    ,
                                 NombreAplicacion = x.NM_APLICACION
                                    ,
                                 NumeroOrden = x.NU_ORDEN
                                    ,
                                 Parametros = x.DE_PARAMETROS
                             }
                        ).FirstOrDefault();

                var nodos = (from x in db.APLICACION
                             join y in db.USUARIO_APLICACION on
                             x.CD_APLICACION equals y.CD_APLICACION
                             orderby x.NU_ORDEN ascending
                             where x.IN_APLICACION_INACTIVA == false
                             && y.IN_INACTIVO == false
                             && y.CD_USUARIO == idUsuario
                             && x.IN_ES_DASHBOARD == false
                             && !(x.NM_ICONO_ACCESO_RAPIDO == null || x.NM_ICONO_ACCESO_RAPIDO.Trim() == string.Empty)
                             select new NodoAplicacion
                             {
                                 IdAplicacion = y.CD_APLICACION
                                    ,
                                 Accion = x.NM_ACCION
                                    ,
                                 Controlador = x.NM_CONTROLADOR
                                    ,
                                 IconoAccesoRapido = x.NM_ICONO_ACCESO_RAPIDO
                                    ,
                                 IconoMenu = x.NM_ICONO_MENU
                                    ,
                                 NombreAplicacion = x.NM_APLICACION
                                    ,
                                 NumeroOrden = x.NU_ORDEN
                                    ,
                                 Parametros = x.DE_PARAMETROS
                             }
                        ).ToList().AsQueryable();

                if (dashboard != null) {
                    List<NodoAplicacion> dashboadList = new List<NodoAplicacion>();
                    dashboadList.Add(dashboard);
                    nodos = dashboadList.Union(nodos).AsQueryable();
                }

                return nodos;
            }
        }
    }
}