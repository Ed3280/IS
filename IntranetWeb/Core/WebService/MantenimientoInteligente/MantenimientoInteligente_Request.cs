using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.Core.Utils.CustomTextMessageEncoder;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace IntranetWeb.Core.WebService.MantenimientoInteligente
{
    /// <summary>
    /// Clase para invocar a los métodos de la plataforma de mantenimiento inteligente
    /// </summary>
    public class MantenimientoInteligente_Request
    {

        private Log4NetLogger log;

        public MantenimientoInteligente_Request() {
            log = new Log4NetLogger();
        }

        /// <summary>
        /// Realiza el llamado al proceso de envío de correo para los usuarios que no están registrados
        /// </summary>
        /// <param name="nombreCliente"></param>
        /// <param name="direccionCorreo"></param>
        /// <returns></returns>
        public bool CallCorreoInvitacion(string nombreCliente, string direccionCorreo){

            string resultado = "";
            bool salida = false;


            string host = "";

            try{
                             
                //Se obtiene el host de la base de datos
                host = IntranetWeb.Core.Respositorios.UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("HOST_MI");

                if (String.IsNullOrWhiteSpace(host))
                    throw new System.Exception("No se encuentra host de web service");

                string URL_ADDRESS = "http://" + host + "/correoInvitacion/"+ nombreCliente + "/"+ direccionCorreo;

                //Se coloca el tipo de request como post como post
                HttpWebRequest request = WebRequest.Create(new Uri(URL_ADDRESS)) as HttpWebRequest;
                request.Method = "GET";
                               
                //Se obtiene la respuesta 
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    resultado = reader.ReadToEnd();
                    reader.Close();
                }
                salida  =  true;
               
            }
            catch(System.Exception exc){
                //Se guarda en el log de transacciones
                log.Error(Core.Constante.Mensaje.Error.Error300, exc);
                salida = false;
            }

            return salida;
        }

        /// <summary>
        /// Permite enviar un mensaje vía GCM
        /// </summary>
        /// <param name="GCM_ID"></param>
        /// <param name="titulo"></param>
        /// <param name="mensaje"></param>
        public bool envioMensajeGCM(string  GCM_ID
                                   , int codigoMostrarDirecto
                                   ,string titulo
                                   ,string mensaje) {

            bool salida = false;

            try{

                MantenimientoInteligenteServiceReference.moduloGCMPortTypeClient mantenimientoIntWS = new MantenimientoInteligenteServiceReference.moduloGCMPortTypeClient();
                MantenimientoInteligenteServiceReference.requestGCM reqGCM = new MantenimientoInteligenteServiceReference.requestGCM();

                reqGCM.id = GCM_ID;
                reqGCM.title = titulo;
                reqGCM.code = codigoMostrarDirecto;
                reqGCM.message = mensaje;
                

                //MantenimientoInteligenteServiceReference.responseGCM 
                  bool  resp =  mantenimientoIntWS.sendMessage(reqGCM);
                if (resp/*.response*/ ) 
                    salida = true;
                
            }
            catch (System.Exception exc){
                //Se guarda en el log de transacciones
                log.Error(Core.Constante.Mensaje.Error.Error300, exc);
                salida = false;
            }
            return salida;
        }
    }
}