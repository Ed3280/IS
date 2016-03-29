using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntranetWeb.Models;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IntranetWeb.Core.WebService.NewGPS2012_SOAP_Request
{
    public static class ReportPubClient
    {

        public static dynamic CallGetReportMileage(int UserID, string TimeZones, string StartDates, string EndDates, int DeviceID) {

            string jsonString = "";
            dynamic result, salida;
            

            string host = "";

            try {
                //Se obtiene el host de la base de datos
                using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                    host = (from x in db.PARAMETRO_CONFIGURACION
                            where x.NM_PARAMETRO == "HOST_DB_NewGPS2012"
                            select x.CD_CONFIGURACION).FirstOrDefault();
                }

                if (String.IsNullOrWhiteSpace(host))
                    throw new System.Exception("No se encuentra host de web service");

                string URL_ADDRESS = "http://"+host+"/Ajax/ReportAjax.asmx/GetReportMileage";

                //Se coloca el tipo de request como post como post
                HttpWebRequest request = WebRequest.Create(new Uri(URL_ADDRESS)) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";



                String jsonDataEntry = "{UserID:  " + UserID +
                            ",TimeZones:'" + TimeZones +"'"+
                            ",StartDates:'" + StartDates + "'" +
                            ",EndDates:'" + EndDates + "'" +
                            ",DeviceID:" + DeviceID + "}";

                StringBuilder data = new StringBuilder();
                data.Append(jsonDataEntry);
                byte[] byteData = Encoding.UTF8.GetBytes(data.ToString());      // Se crea un arreglo de Bytes con la data que queremos enviar
                request.ContentLength = byteData.Length;                        // Se coloca el content length en el header

                // Se escribe la data en la peticion
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                //Se obtiene la respuesta 
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    jsonString = reader.ReadToEnd();
                    reader.Close();
                }

                result = JValue.Parse(jsonString);
                dynamic report = JValue.Parse((string)result.d);
                salida =report.reports;

    }
            catch  {
                //Se guarda en el log de transacciones
                salida = "";  
            }
            return salida;

        }

    }
}