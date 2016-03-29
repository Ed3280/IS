using System;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.ViewModel.Monitor;
using RestSharp.Deserializers;
using System.Web.Script.Serialization;
using IntranetWeb.Core.Servicio.Logging;

namespace IntranetWeb.Core.WebService.ApiPlataforma
{
    public static class LocationClient
    {

     
        /// <summary>
        /// Permite obtener la posicición de un dispositivo en un momento dado
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static LocalizacionDispositivo GetTrack(int deviceId, string token)
        {
            LocalizacionDispositivo result = new LocalizacionDispositivo();
            try {
                ClienteRepositorio clienteRepo = new ClienteRepositorio();

                string url = "http://" + Core.Respositorios.UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("HOST_API_PLATAFORMA");
                RestClient restClient = new RestClient(url);

                var request = new RestRequest("api/Location/GetTrack", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(new
                {
                    DeviceID = deviceId
                   , Token = token
                });
                var response = restClient.Execute(request);

                RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
                var JSONObj = deserial.Deserialize<Dictionary<string, string>>(response);

                //Procesó todo bien 
                if (JSONObj["State"] == "0") {

                    string salida = JSONObj["Item"];
                    var serializer = new JavaScriptSerializer();
                    result = serializer.Deserialize<LocalizacionDispositivo>(salida);
                }
                result.Id = deviceId;
            }
            catch (System.Exception exc) {
                Log4NetLogger log = new Log4NetLogger();
                log.Error(Core.Constante.Mensaje.Error.Error300, exc);
                result = new LocalizacionDispositivo();
            }

            return result;
        }


    }
}