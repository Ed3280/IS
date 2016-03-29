using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.ViewModel.Monitor;
using RestSharp;
using RestSharp.Deserializers;
using System.Web.Script.Serialization;
using IntranetWeb.Core.Servicio.Logging;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

namespace IntranetWeb.Core.WebService.ApiPlataforma
{
    /// <summary>
    /// Cliente de Web Service para los dispositivos 
    /// </summary>
    public static class DevicesClient
    {
        /// <summary>
        /// Retorna el listado de dispositivos asociados al usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IEnumerable<DeviceWS> GetDevicesList(int userId, string token) {

            IEnumerable<DeviceWS> result = new List<DeviceWS>();
            try
            {
                
                string url = "http://" + Core.Respositorios.UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("HOST_API_PLATAFORMA");
                RestClient restClient = new RestClient(url);

                var request = new RestRequest("api/Devices/GetDevicesList", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(new
                {
                    UserID = userId
                   ,Token = token
                });
                var response = restClient.Execute(request);

                RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
                var JSONObj = deserial.Deserialize<Dictionary<string, string>>(response);

                //Procesó todo bien 
                if (JSONObj["State"] == "0"){

                    string salida = JSONObj["Item"];
                    var group = JValue.Parse(salida);
                   
                    var serializer = new JavaScriptSerializer();

                    group.ToList().ForEach(x => result = result.Union(serializer.Deserialize<IEnumerable<DeviceWS>>(x["Item"].ToString())));
                    
                }                
            }
            catch (System.Exception exc)
            {
                Log4NetLogger log = new Log4NetLogger();
                log.Error(Core.Constante.Mensaje.Error.Error300, exc);
                result = new List<DeviceWS>();
            }

            return result;

        }
    }
}