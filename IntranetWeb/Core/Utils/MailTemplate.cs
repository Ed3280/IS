using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Models;

namespace IntranetWeb.Core.Utils
{
    public class MailTemplate
    {
     
        public static String PrepararTemplate(String saludo, String mensaje)
        {

            string hostImagenes = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("HOST_SAISERVICES_PUB");

            PLANTILLA_CORREO plantilla = IntranetWeb.Core.Respositorios.UtilRepositorio.obten_PLANTILLA_CORREO_ByTipoPLantilla(2, null);

            return String.Format(plantilla.DE_CONTENIDO_PLANTILLA, hostImagenes, saludo, mensaje);            
        }

        
        /// <summary>
        /// Método para prepar el template de registro en la aplicación
        /// </summary>
        /// <param name="CodigoUsuario"></param>
        /// <param name="CodigoUsuarioPadre"></param>
        /// <returns></returns>
        public static void PrepararTemplateRegistro(int CodigoUsuario 
                                                   ,List<int> CodigoUsuarioPadreList
                                                   ,out string Titulo
                                                   ,out string Plantilla) {
            int tipoPlantilla = 1;
            AuthRepositorio authRepo = new AuthRepositorio();
            
            //Se busca la información del host de las imágenes
            string hostImagenes = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("HOST_SAISERVICES_PUB");
            USUARIO usuario = authRepo.obten_USUARIO_ById(CodigoUsuario);

            string nombreCliente = usuario.DE_NOMBRE_APELLIDO, nombreUsuario = usuario.NM_USUARIO, contrasena = authRepo.obtenPassword(usuario.DE_CONTRASENA);
            int? codigoUsuarioPadre = null;

            codigoUsuarioPadre  = CodigoUsuarioPadreList.Count() == 1 ? CodigoUsuarioPadreList[0] : codigoUsuarioPadre;
            
            //Se busca el template que deba escribir el mensaje  
            PLANTILLA_CORREO plantilla = IntranetWeb.Core.Respositorios.UtilRepositorio.obten_PLANTILLA_CORREO_ByTipoPLantilla(tipoPlantilla, codigoUsuarioPadre);
            if (plantilla == null){
                Titulo = Resources.TituloCorreoResource.BienvenidaSAI;
                string Mensaje = Resources.MensajeCorreoResource.Registro;
                
                Plantilla = PrepararTemplate(Titulo, String.Format(Mensaje, nombreCliente, nombreUsuario, contrasena));
            }
            else{
                Titulo = plantilla.DE_TITULO_CORREO;
                Plantilla = String.Format(plantilla.DE_CONTENIDO_PLANTILLA, hostImagenes, nombreCliente, nombreUsuario, contrasena);
            }
        }


        /// <summary>
        /// Prepara la plantilla de registro de socio comercial para enviarla por correo
        /// </summary>
        /// <param name="CodigoUsuario"></param>
        /// <param name="Titulo"></param>
        /// <param name="Plantilla"></param>
        public static void PrepararTemplateRegistroSocio(int CodigoUsuario
                                                   , out string Titulo
                                                   , out string Plantilla){

            AuthRepositorio authRepo = new AuthRepositorio();

            //Se busca la información del host de las imágenes
            USUARIO usuario = authRepo.obten_USUARIO_ById(CodigoUsuario);

            string nombreSocio = usuario.DE_NOMBRE_APELLIDO, nombreUsuario = usuario.NM_USUARIO, contrasena = authRepo.obtenPassword(usuario.DE_CONTRASENA);
            Titulo = Resources.TituloCorreoResource.BienvenidaSAI;
            Plantilla = PrepararTemplate(String.Format(Resources.EncabezadoCorreoResource.Estimado, nombreSocio), String.Format(Resources.MensajeCorreoResource.RegistroSocio, nombreUsuario, contrasena));            
        }


        /// <summary>
        /// Prepara la plantilla de registro de un usuario interno para enviarla por correo
        /// </summary>
        /// <param name="CodigoUsuario"></param>
        /// <param name="Titulo"></param>
        /// <param name="Plantilla"></param>
        public static void PrepararTemplateRegistroUsuarioInterno(
                                                     int CodigoUsuario
                                                   , int CodigoUsuarioPadre
                                                   , out string Titulo
                                                   , out string Plantilla)
        {

            AuthRepositorio authRepo = new AuthRepositorio();

            //Se busca la información del host de las imágenes
            USUARIO usuario         = authRepo.obten_USUARIO_ById(CodigoUsuario);
            USUARIO usuarioPadre = authRepo.obten_USUARIO_ById(CodigoUsuarioPadre);

            string nombreUsario = usuario.DE_NOMBRE_APELLIDO, nombreUsuario = usuario.NM_USUARIO, contrasena = authRepo.obtenPassword(usuario.DE_CONTRASENA);
            Titulo = Resources.TituloCorreoResource.BienvenidaSAI;
            Plantilla = PrepararTemplate(String.Format(Resources.EncabezadoCorreoResource.Estimado, nombreUsario), String.Format(Resources.MensajeCorreoResource.RegistroUsuarioInterno, usuarioPadre.DE_NOMBRE_APELLIDO, nombreUsuario, contrasena));
        }



        /// <summary>
        /// Permite preparar la plantilla para el registro de un empleado
        /// </summary>
        /// <param name="CodigoUsuario"></param>
        /// <param name="Titulo"></param>
        /// <param name="Plantilla"></param>
        public static void PrepararTemplateRegistroEmpleado(
                                                     int CodigoUsuario                                                   
                                                   , out string Titulo
                                                   , out string Plantilla)
        {

            AuthRepositorio authRepo = new AuthRepositorio();

            //Se busca la información del host de las imágenes
            USUARIO usuario = authRepo.obten_USUARIO_ById(CodigoUsuario);            

            string nombreUsario = usuario.DE_NOMBRE_APELLIDO, nombreUsuario = usuario.NM_USUARIO, contrasena = authRepo.obtenPassword(usuario.DE_CONTRASENA);
            Titulo = Resources.TituloCorreoResource.BienvenidaSAI;
            Plantilla = PrepararTemplate(String.Format(Resources.EncabezadoCorreoResource.Estimado, nombreUsario), String.Format(Resources.MensajeCorreoResource.RegistroEmpleado,  nombreUsuario, contrasena));
        }

    }
}