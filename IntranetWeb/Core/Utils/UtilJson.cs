using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using IntranetWeb.ViewModel.Shared;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace IntranetWeb.Core.Utils
{
    public static class UtilJson
    {
        /// <summary>
        /// Retorna el objeto json para un mensaje de error
        /// </summary>
        /// <param name="mensaje">Mensaje de error a mostar</param>
        /// <param name="data">Data a retornar</param>
        /// <returns>Objeto Json </returns>
        public static JsonResult Error(string mensaje, object data)
        {
            JsonResult resultado = new JsonResult { ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            resultado.Data = new { status = "501", success = false, mensaje = mensaje, mensajeHtml = String.IsNullOrWhiteSpace(mensaje) ? "" : IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeError(mensaje), content = data };
            return resultado;
        }


        /// <summary>
        /// Retorna el objeto json para un mensaje de error
        /// </summary>
        /// <param name="mensaje">Mensaje de error a mostar</param>
        /// <returns></returns>
        public static JsonResult Error(string mensaje){
            return Error(mensaje, null);
        }



        /// <summary>
        /// Retorna el objeto json para un mensaje de exito
        /// </summary>
        /// <param name="mensaje">Mensaje de éxito a mostar a mostar</param>
        /// <param name="data">Data a retornar</param>
        /// <returns></returns>
        public static JsonResult Exito(string mensaje, object data)
        {
            JsonResult resultado = new JsonResult { ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            resultado.Data = new { status = "200", success = true, mensaje = mensaje, mensajeHtml = String.IsNullOrWhiteSpace(mensaje) ? "" : IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeExito(mensaje), content = JsonConvert.SerializeObject(data) };
            return resultado;
        }


        /// <summary>
        /// Éxito al procesar una consulta de registros paginado por datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mensaje"></param>
        /// <param name="resultData"></param>
        /// <returns></returns>
        public static JsonResult ExitoDataTableOnServer<T>(string mensaje, DTResult<T> resultData ){
            JsonResult resultado = new JsonResult { ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            resultado.Data = new {
                  status = "200"
                , success = true
                , mensaje = mensaje
                , mensajeHtml = String.IsNullOrWhiteSpace(mensaje) ? "" : IntranetWeb.Core.Utils.HtmlHelper.escribirMensajeExito(mensaje)
                ,draw = resultData.draw
                ,data = JsonConvert.SerializeObject(resultData.data)
                ,recordsFiltered = resultData.recordsFiltered
                ,recordsTotal = resultData.recordsTotal
            };
            return resultado;
        }


        /// <summary>
        /// Éxito al procesar una consulta de registros paginado por datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultData"></param>
        /// <returns></returns>
        public static JsonResult ExitoDataTableOnServer<T>( DTResult<T> resultData){

            return  ExitoDataTableOnServer< T > (null, resultData );
            
        }



        /// <summary>
        /// Data a retornar
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult Exito( object data){
            return Exito(null, data);
        }
        /// <summary>
        /// Envñia un mensaje de éxito sólo con el mensaje a actualizar
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult Exito(string mensaje)
        {
            return Exito(mensaje,null);
        }


    }

   
}