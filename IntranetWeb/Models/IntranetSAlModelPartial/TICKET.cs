using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Models
{
    public partial class TICKET
    {
        /// <summary>
        /// Permite pasar un objeto de la tabla TICKET  a su equivalente histórico
        /// </summary>
        /// <returns></returns>
        public TICKET_HISTORICO TICKET_to_TICKET_HISTORICO() {

            TICKET_HISTORICO tiHis = new TICKET_HISTORICO();

            tiHis.CD_USUARIO                = this.CD_USUARIO;
            tiHis.DE_OBSERVACION            = this.DE_OBSERVACION;
            tiHis.DE_OBSERVACION_SUPERVISOR = this.DE_OBSERVACION_SUPERVISOR;
            tiHis.FE_CREACION               = this.FE_CREACION;
            tiHis.FE_ESTATUS                = this.FE_ESTATUS;
            tiHis.ID_DISPOSITIVO            = this.ID_DISPOSITIVO;
            tiHis.TP_ATENCION               = this.TP_ATENCION;
            tiHis.ID_DISTRIBUIDOR           = this.ID_DISTRIBUIDOR;
            tiHis.ID_RESPUESTA_OPERADOR     = this.ID_RESPUESTA_OPERADOR;
            tiHis.ID_ESTATUS_TICKET         = this.ID_ESTATUS_TICKET;
            tiHis.ID_NOTIFICACION           = this.ID_NOTIFICACION;
            tiHis.ID_ORIGEN_NOTIFICACION    = this.ID_ORIGEN_NOTIFICACION;
            tiHis.ID_TICKET                 = this.ID_TICKET;
            tiHis.CD_USUARIO_TRANSFERENCIA  = this.CD_USUARIO_TRANSFERENCIA;
            tiHis.ID_USUARIO_MOVIL          = this.ID_USUARIO_MOVIL;
            tiHis.TP_NOTIFICACION           = this.TP_NOTIFICACION;
            tiHis.FE_NOTIFICACION           =  this.FE_NOTIFICACION;
            tiHis.IN_RESPUESTA_ENVIADA      = this.IN_RESPUESTA_ENVIADA;


            return tiHis;
        }

    }
}