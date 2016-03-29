using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Servicio.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IntranetWeb.Core.WebService.GCM
{
    public static class GCMClient
    {
        /// <summary>
        /// Hace en el envío de mensajes por GCM
        /// </summary>
        /// <param name="gcmId"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Send(string gcmId
                               , string title
                               , string message) {
            bool result = false;
            Log4NetLogger log = new Log4NetLogger();
            try
            {
                GCMRepositorio GCMRepo = new GCMRepositorio();

                string url = "https://" + Core.Respositorios.UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("HOST_GCM");
                string API_KEY = Core.Respositorios.UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("GOOGLE_DEV_API_KEY");
                string error="";

                RestClient restClient = new RestClient(url);

                var request = new RestRequest("gcm/send", Method.POST) { RequestFormat = DataFormat.Json };

                request.AddHeader("Authorization", "key="+API_KEY);
                
                request.AddJsonBody(
                          new {
                                to = gcmId
                             ,  data = new{
                                    title = title
                                ,   message = message
                             }
                          }     
                          );

                
               var response = restClient.Execute(request);
                
                RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
                var JSONObj = deserial.Deserialize<Dictionary<string, string>>(response);

                string salida = JSONObj["results"];
                var gcmResult = JValue.Parse(salida);

                string messageId= gcmResult[0]["message_id"].ToString();
                DateTime fechaActual = DateTime.Now;
                
                if (JSONObj["success"] == "1")
                    result = true;
                else
                    error = gcmResult[0]["error"].ToString();

                try
                {

                    //Se busca el usuario gcm
                    GCMRepo.guarda_NOTIFICACION_GCM(gcmId
                            , message
                            , fechaActual
                            , result
                            , result ? messageId : error
                            , fechaActual);
                }
                catch (System.Exception exc) {
                    log.Error(Core.Constante.Mensaje.Error.FalloGuardarHistoricoGCM, exc);
                }
            }
            catch (System.Exception exc){                
                log.Error(Core.Constante.Mensaje.Error.Error300, exc);
                result = false;
            }
            return result;
        }






    }
}