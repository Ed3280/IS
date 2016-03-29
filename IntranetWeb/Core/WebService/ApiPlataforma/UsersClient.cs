using IntranetWeb.Core.Respositorios;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.WebService.ApiPlataforma
{
    public static class UsersClient
    {


        /// <summary>
        /// Usuario de Panama Main
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static dynamic Login(int userId) {

            string jsonString = "";
            dynamic result;

            try
            {
                ClienteRepositorio clienteRepo = new ClienteRepositorio();
                DISTRIBUIDOR usuario = clienteRepo.obten_DISTRIBUIDOR_ById(userId);

                if (usuario != null)
                {

                    string url = "http://" + Core.Respositorios.UtilRepositorio.obtenValorUnico_PARAMETRO_CONFIGURACION("HOST_API_PLATAFORMA");
                    RestClient restClient = new RestClient(url);

                    var request = new RestRequest("api/users/login", Method.POST) { RequestFormat = DataFormat.Json };
                    request.AddBody(new
                    {
                        Name = usuario.NM_USUARIO
                                            ,
                        Pass = usuario.DE_CONTRASENA
                    });
                    var response = restClient.Execute(request);
                    jsonString = response.Content;

                }
                result = JValue.Parse(jsonString);
            }
            catch (System.Exception exc) {
                Log4NetLogger log = new Log4NetLogger();
                log.Error(Core.Constante.Mensaje.Error.Error300, exc);
                result = null;
            }
                return result;
        }
    }
}