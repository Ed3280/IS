using IntranetWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Respositorios
{
    public class ValidacionRepositorio
    {

        public ValidacionRepositorio() { }

        /// <summary>
        /// Verifica si existe algún usuario empleando ese número de documento
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public bool numeroDocumentoUsuarioEnUso(string tipoDocumento
                                        , string numeroDocumento)
        {

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                int total = (from x in db.USUARIO
                             where x.TIPO_DOCUMENTO_IDENTIDAD.TP_DOCUMENTO_IDENTIDAD == tipoDocumento
                             && x.NU_DOCUMENTO_IDENTIDAD == numeroDocumento
                             select x).Count();

                return total > 0 ? true : false;
            }
        }


        /// <summary>
        /// Verifica si el número de VIN se encuentra en uso por algún otro vehículo
        /// </summary>
        /// <param name="VIN"></param>
        /// <returns></returns>

        public bool numeroVINEnUso(string VIN) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                int total = (from x in db.VEHICULO
                             where x.NU_VIN == VIN
                             select x
                              ).Count();

                return total > 0 ? true : false;

            }
        }



        /// <summary>
        /// Verifica si existe algún socio comercial empleando ese número de documento
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public bool numeroDocumentoSocioComercialEnUso( string tipoDocumento
                                                      , string numeroDocumento)
        {
            if (!String.IsNullOrWhiteSpace(tipoDocumento) && tipoDocumento.ToUpper() == "CI" &&
                   (!String.IsNullOrWhiteSpace(numeroDocumento) && (numeroDocumento == "0" || numeroDocumento == "1"))
                   )
                return true;

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
               

                int total = (from x in db.SOCIO_COMERCIAL
                             where x.USUARIO.TP_DOCUMENTO_IDENTIDAD == tipoDocumento
                             && x.USUARIO.NU_DOCUMENTO_IDENTIDAD == numeroDocumento                              
                             select x).Count();

                return total > 0 ? true : false;
            }
        }


        /// <summary>
        /// Verifica si existe algún empleado empleando ese número de documento
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public bool numeroDocumentoEmpleadoEnUso(string tipoDocumento
                                                      , string numeroDocumento)
        {
            if (!String.IsNullOrWhiteSpace(tipoDocumento) && tipoDocumento.ToUpper() == "CI" &&
                   (!String.IsNullOrWhiteSpace(numeroDocumento) && (numeroDocumento == "0" || numeroDocumento == "1"))
                   )
                return true;

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {


                int total = (from x in db.EMPLEADO
                             where x.USUARIO.TP_DOCUMENTO_IDENTIDAD == tipoDocumento
                             && x.USUARIO.NU_DOCUMENTO_IDENTIDAD == numeroDocumento
                             select x).Count();

                return total > 0 ? true : false;
            }
        }

        /// <summary>
        /// Verifica si existe algún usuario interno que esté empleando ese número de documento
        /// </summary>
        /// <param name="codigoUsuarioPadre"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public bool numeroDocumentoUsuarioInternoEnUso(
                                                        int codigoUsuarioPadre
                                                      , string tipoDocumento
                                                      , string numeroDocumento)
        {
            if (!String.IsNullOrWhiteSpace(tipoDocumento) && tipoDocumento.ToUpper() == "CI" &&
               (!String.IsNullOrWhiteSpace(numeroDocumento) && (numeroDocumento == "0" || numeroDocumento == "1"))
                   )
                return true;

            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {


                int total = (from x in db.USUARIO
                             where x.TP_DOCUMENTO_IDENTIDAD == tipoDocumento
                             && x.NU_DOCUMENTO_IDENTIDAD == numeroDocumento
                             && x.USUARIO2.Select(y => y.CD_USUARIO).Contains(codigoUsuarioPadre)   //Usuario Padre
                             select x).Count();

                return total > 0 ? true : false;
            }
        }


        /// <summary>
        /// Verifica si existe algún cliente empleando ese número de documento
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public bool numeroDocumentoClienteEnUso(string tipoDocumento
                                                      , string numeroDocumento)
        {

            if (!String.IsNullOrWhiteSpace(tipoDocumento) && tipoDocumento.ToUpper() == "CI" &&
                   (!String.IsNullOrWhiteSpace(numeroDocumento) && (numeroDocumento == "0" || numeroDocumento == "1"))
                   )
                return true;


            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                int total = (from x in db.CLIENTE
                             where x.USUARIO.TP_DOCUMENTO_IDENTIDAD == tipoDocumento
                             && x.USUARIO.NU_DOCUMENTO_IDENTIDAD == numeroDocumento
                             select x).Count();

                return total > 0 ? true : false;
            }
        }

    
     


        /// <summary>
        /// Verifica si un usuario existe
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <returns></returns>
        public bool usuarioExiste(int cdUsuario
                                    , string nombreUsuario)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                int total = (from x in db.USUARIO
                             where x.NM_USUARIO.Trim().ToLower() == nombreUsuario.Trim().ToLower()
                             && x.CD_USUARIO != cdUsuario
                             select x).Count();

                return total > 0 ? true : false;
            }
        }


        /// <summary>
        /// Verifica si un correo existe 
        /// </summary>
        /// <param name="correoUsuario"></param>
        /// <returns></returns>
        public bool correoExiste(    int cdUsuario
                                    ,string correoUsuario)
        {
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                int total = (from x in db.USUARIO
                             where x.DI_EMAIL_USUARIO.Trim().ToLower() == correoUsuario.Trim().ToLower()
                             && x.CD_USUARIO != cdUsuario
                             select x).Count();

                return total > 0 ? true : false;
            }
        }
        
        /// <summary>
        /// Verifica si el usuario fue dado de baja
        /// </summary>
        /// <param name="CodigoUsuario"></param>
        /// <returns></returns>
        public bool usuarioEnBaja(int CodigoUsuario){
            AuthRepositorio repo = new AuthRepositorio();
            USUARIO usu = repo.obten_USUARIO_ById(CodigoUsuario);
            return usu != null && usu.EMPLEADO != null && usu.EMPLEADO.FE_RETIRO.HasValue ? true : false;
        }

        /// <summary>
        /// Valida si una notificación ya ha sido tomada por un usuario
        /// </summary>
        /// <param name="idNotificacion"></param>
        /// <param name="idOrigenNotificacion"></param>
        /// <returns></returns>
        public bool validaNotificacionTomada( int idNotificacion 
                                             ,int idOrigenNotificacion) {
            int total = 0; 
            using (var db = new IntranetSAIEntities()) {
                total = (from x in db.NOTIFICACION_SIN_GESTION
                             where x.ID_NOTIFICACION == idNotificacion
                             && x.ID_ORIGEN_NOTIFICACION == idOrigenNotificacion
                             select 1
                             ).Union(
                                from x in db.TICKET
                                where x.ID_NOTIFICACION == idNotificacion
                                && x.ID_ORIGEN_NOTIFICACION == idOrigenNotificacion
                                select 2
                            ).Count();

            }

            return (total > 0? true : false); 
        }
    }
}