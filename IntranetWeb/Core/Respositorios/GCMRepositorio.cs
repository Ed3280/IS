using IntranetWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Respositorios
{
    public class GCMRepositorio
    {
        public GCMRepositorio() { }

        /// <summary>
        /// Guarda en la tabla saiNotificationGCM
        /// </summary>
        /// <param name="saiNotificationGCM"></param>
        /// <returns></returns>
        public int guarda_NOTIFICACION_GCM(NOTIFICACION_GCM saiNotificationGCM)
        {

            using (var db = new IntranetSAIEntities())
            {
                db.NOTIFICACION_GCM.Add(saiNotificationGCM);
                return db.SaveChanges();
            }
        }






        /// <summary>
        /// Guarda la notificación enviada
        /// </summary>
        /// <param name="gcmId"></param>
        /// <param name="message"></param>
        /// <param name="dateSendMessage"></param>
        /// <param name="statusMessage"></param>
        /// <param name="responseMessage"></param>
        /// <param name="dateResponseMessage"></param>
        /// <returns></returns>
        public int guarda_NOTIFICACION_GCM(   string gcmId
                                             ,string message
                                             ,DateTime dateSendMessage 
                                             ,bool statusMessage
                                             ,string responseMessage
                                             , DateTime dateResponseMessage)
        {

            NOTIFICACION_GCM notificacionGCM = new NOTIFICACION_GCM();
            USUARIO_MOVIL_GCM usuarioMovilGCM = new USUARIO_MOVIL_GCM();

            usuarioMovilGCM = obten_USUARIO_MOVIL_GCM_ByIdGCM(gcmId);

            notificacionGCM.CD_USUARIO_GCM = usuarioMovilGCM!=null? usuarioMovilGCM.CD_USUARIO_GCM:0;
            notificacionGCM.DE_MENSAJE = message;
            notificacionGCM.FE_ENVIO_MENSAJE = dateSendMessage;
            notificacionGCM.IN_ENVIADO = statusMessage;
            notificacionGCM.DE_RESPUESTA_MENSAJE_GCM = responseMessage;
            notificacionGCM.FE_MENSAJE_RESPUESTA_GCM = dateResponseMessage;
            
            return this.guarda_NOTIFICACION_GCM(notificacionGCM);
        }



        /// <summary>
        /// Método para obtener el usuario de GCM
        /// </summary>
        /// <param name="idGCM"></param>
        /// <returns></returns>
        public USUARIO_MOVIL_GCM obten_USUARIO_MOVIL_GCM_ByIdGCM(string idGCM) {

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                return ( from x in db.USUARIO_MOVIL_GCM
                         where x.ID_GCM == idGCM
                         select x
                    ).FirstOrDefault()
                    ;

            }
        }
    }
}