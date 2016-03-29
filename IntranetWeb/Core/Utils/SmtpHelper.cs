using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using IntranetWeb.Core.Respositorios;
using System.IO;
using Microsoft.Win32;
using System.Net.Mime;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.Core.Utils
{
    public class SmtpHelper
    {

        public static bool Send(String Titulo, String Mensaje, params String[] destinatarios)
        {
            return Send(Titulo
                              , Mensaje
                              , destinatarios != null ? destinatarios.ToList() : new List<string>()
                              , null
                              , null);
        }




        /// <summary>
        /// Método para el envío de correos
        /// </summary>
        /// <param name="Titulo">Título del correo</param>
        /// <param name="Mensaje">Mensaje del correo</param>
        /// <param name="destinatarios">Destinatarios del correo</param>
        /// <param name="files">Archvivos adjuntos del correo</param>
        /// <param name="filenames">Nombre de los archivos adjutnos</param>
        /// <returns>Éxto o fracaso</returns>
        public static bool Send(String Titulo
                              , String Mensaje
                              , List<string> destinatarios
                              , List<Stream> files
                              , List<string> filenames)
        {
            string tpDocumento;

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            Attachment attachment;

            string from = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("EMAIL_FROM"); 
            var SMTP_HOST = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("SMTP_HOST"); 
            if (String.IsNullOrWhiteSpace(SMTP_HOST)) return false;
            smtpClient.Host = SMTP_HOST;

            var SMTP_PORT = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("SMTP_PORT"); 
            smtpClient.Port = Convert.ToInt32(SMTP_PORT ?? "25"); //Si no consigue puertos entonces usa el default 25

            String SMTP_USER = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("SMTP_USER"); 
            String SMTP_PASSWORD = UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("SMTP_PASSWORD");  
            smtpClient.Credentials = new NetworkCredential(SMTP_USER, SMTP_PASSWORD);
            smtpClient.EnableSsl = false;


            // Enviar mensaje
            // Dirección desde
            MailAddress fromAddress = new MailAddress(from, from);
            message.From = fromAddress;

            // Destinatario
            int i = 0;
            foreach (string to in destinatarios)
            {
                if (i == 0)
                    message.To.Add(to);
                else
                    message.Bcc.Add(to);
                i++;
            }

            // Asunto
            message.Subject = Titulo;

            // Formato del cuerpo
            message.IsBodyHtml = true;

            // Contenido
            message.Body = Mensaje;
            if (filenames != null && files != null)
            {
                if (filenames.Count != files.Count)
                    throw new System.Exception("La cantidad de archivos no corresponde con la cantidad de nombres de archivos");

                // Attach
                int cont = 0;
                string[] fileNamesArr = filenames.ToArray();
                string filename;
                foreach (Stream fileStream in files)
                {
                    filename = fileNamesArr[cont];
                    tpDocumento = System.IO.Path.GetExtension(filename).ToLower();

                    RegistryKey key = Registry.ClassesRoot.OpenSubKey(tpDocumento);
                    fileStream.Seek(0, SeekOrigin.Begin);
                    attachment = new Attachment(fileStream, new ContentType(key.GetValue("Content Type").ToString()));
                    attachment.TransferEncoding = TransferEncoding.Base64;
                    attachment.Name = filename;
                    message.Attachments.Add(attachment);
                    cont++;
                }
            }

            // Enviar mensaje
            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (System.Exception exc)
            {
                var st = new System.Diagnostics.StackTrace(exc, true);
                var frame = st.GetFrame(0);
                                
                return false;
            }
        }
     }
}