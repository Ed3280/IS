using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using IntranetWeb.Models;
using System.Drawing;

namespace IntranetWeb.Core.Respositorios
{
    public class EstadisticaRepositorio
    {

        /// <summary>
        /// Retorna el listado de tickets para estadísticas
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <param name="tpNotificacion"></param>
        /// <param name="tpAtencion"></param>
        /// <param name="cdEstatus"></param>
        /// <param name="feStatusDesde"></param>
        /// <param name="feStatusHasta"></param>
        /// <param name="feAperturaDesde"></param>
        /// <param name="feAperturaHasta"></param>
        /// <returns></returns>
        public byte[] generarExcelListaTickets(int? cdUsuario
                                              ,int? tpNotificacion
                                              ,int? tpAtencion
                                              ,int? cdEstatus
                                              ,DateTime? feStatusDesde 
                                              ,DateTime? feStatusHasta
                                              ,DateTime? feAperturaDesde
                                              ,DateTime? feAperturaHasta) {
            byte[] salida = null;

            if (feAperturaHasta != null) {
                feAperturaHasta = feAperturaHasta.Value.AddDays(1);
                feAperturaHasta = feAperturaHasta.Value.AddSeconds(-1);
            }


            if (feStatusHasta != null) {
                feStatusHasta = feStatusHasta.Value.AddDays(1);
                feStatusHasta = feStatusHasta.Value.AddSeconds(-1);
            }
                                             


            //Se obtiene el listado de Tiketes
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
               var resultQuery =  (from x in db.FUNC_002_BUSCA_TICKET(cdUsuario, null , null, feStatusDesde, feStatusHasta)
                                   from y in db.TICKET 
                                   where y.ID_TICKET == x.ID_TICKET
                                   && x.CD_TP_ATENCION == (tpAtencion == null ? x.CD_TP_ATENCION : tpAtencion)
                                   && x.CD_TP_NOTIFICACION == (tpNotificacion == null ? x.CD_TP_NOTIFICACION : tpNotificacion)
                                   && x.CD_ESTATUS  == ( cdEstatus==  null? x.CD_ESTATUS: cdEstatus)
                                   && (feAperturaDesde == null || x.FE_CREACION >= feAperturaDesde)
                                   && (feAperturaHasta == null || x.FE_CREACION <= feAperturaHasta)
                                   select new {
                                       ID_TICKET =  x.ID_TICKET
                                       ,NU_DOCUMENTO_IDENTIDAD = x.NU_DOCUMENTO_IDENTIDAD
                                       ,NM_CLIENTE =  x.DE_NOMBRE_CLIENTE
                                       ,NM_APELLIDO_CLIENTE =  x.DE_APELLIDO_CLIENTE
                                       ,NM_APELLIDO_RAZON_SOCIAL = x.DE_NOMBRE_APELLIDO_RAZON_SOCIAL
                                       ,DE_TIPO_NOTIFICACION  = x.DE_TIPO_NOTIFICACION
                                       ,DE_TIPO_ATENCION =  x.DE_TIPO_ATENCION
                                       ,NU_DVR =  x.NU_DVR
                                       ,NU_SIM = x.NU_SIM
                                       ,NM_USUARIO = x.NM_USUARIO
                                       ,NM_DISTRIBUIDOR =  x.NM_DISTRIBUIDOR
                                       ,DE_ESTATUS = x.DE_ESTATUS
                                       ,FE_STATUS = x.FE_STATUS
                                       ,FE_CREACION = x.FE_CREACION
                                       ,DE_OBSERVACION = y.DE_OBSERVACION  
                                       ,
                                       DE_OBSERVACION_SUPERVISOR =y.DE_OBSERVACION_SUPERVISOR

                
                                   }
                                   );
            


            using (ExcelPackage pck = new ExcelPackage())
            {
                pck.Workbook.Properties.Author = "Sai Innovación";
                pck.Workbook.Properties.Title = "Tickets Agentes";
                var sheet = pck.Workbook.Worksheets.Add("Listado de Tickets");

                sheet.Cells[1, 1].Value = "Ticket";
                sheet.Cells[1, 2].Value = "Id Cliente";
                sheet.Cells[1, 3].Value = "Nombre Cliente";
                sheet.Cells[1, 4].Value = "Tipo Notificación";
                sheet.Cells[1, 5].Value = "Tipo Atención";
                sheet.Cells[1, 6].Value = "DVR";
                sheet.Cells[1, 7].Value = "SIM";
                sheet.Cells[1, 8].Value = "Usuario Atención";
                sheet.Cells[1, 9].Value = "Dealer";
                sheet.Cells[1, 10].Value = "Estatus";
                sheet.Cells[1, 11].Value = "Observación";
                sheet.Cells[1, 12].Value = "Observación Supervisor";
                sheet.Cells[1, 13].Value = "Fecha Estatus";
                sheet.Cells[1, 14].Value = "Hora Estatus";
                sheet.Cells[1, 15].Value = "Fecha Apertura";
                sheet.Cells[1, 16].Value = "Hora Apertura";

                    using (ExcelRange rng = sheet.Cells["A1:P1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    rng.Style.Font.Color.SetColor(Color.White);

                  
                }
                    var rowIndex = 2;
                    foreach (var item in resultQuery)
                    {
                        var col = 1;

                        sheet.Cells[rowIndex, col++].Value = item.ID_TICKET;
                        sheet.Cells[rowIndex, col++].Value = item.NU_DOCUMENTO_IDENTIDAD;
                        sheet.Cells[rowIndex, col++].Value = item.NM_APELLIDO_RAZON_SOCIAL; //((String.IsNullOrWhiteSpace(item.NM_CLIENTE)?"": item.NM_CLIENTE) + " "+(String.IsNullOrWhiteSpace(item.NM_APELLIDO_CLIENTE)?"": item.NM_APELLIDO_CLIENTE)).Trim();
                        sheet.Cells[rowIndex, col++].Value = item.DE_TIPO_NOTIFICACION;
                        sheet.Cells[rowIndex, col++].Value = item.DE_TIPO_ATENCION;
                        sheet.Cells[rowIndex, col++].Value = item.NU_DVR;
                        sheet.Cells[rowIndex, col++].Value = item.NU_SIM;
                        sheet.Cells[rowIndex, col++].Value = item.NM_USUARIO;
                        sheet.Cells[rowIndex, col++].Value = item.NM_DISTRIBUIDOR;
                        sheet.Cells[rowIndex, col++].Value = item.DE_ESTATUS;
                        sheet.Cells[rowIndex, col++].Value = item.DE_OBSERVACION;
                        sheet.Cells[rowIndex, col++].Value = item.DE_OBSERVACION_SUPERVISOR;
                        sheet.Cells[rowIndex, col++].Value = item.FE_STATUS;
                        sheet.Cells[rowIndex, col++].Value = item.FE_STATUS;
                        sheet.Cells[rowIndex, col++].Value = item.FE_CREACION;
                        sheet.Cells[rowIndex, col++].Value = item.FE_CREACION;

                        rowIndex++;

                    }
                    sheet.Column(13).Style.Numberformat.Format = Core.Constante.AppFormat.dateJson;
                    sheet.Column(14).Style.Numberformat.Format = Core.Constante.AppFormat.hourJson;
                    sheet.Column(15).Style.Numberformat.Format = Core.Constante.AppFormat.dateJson;
                    sheet.Column(16).Style.Numberformat.Format = Core.Constante.AppFormat.hourJson;
                    salida = pck.GetAsByteArray();

            }
            }
            return salida;
        }



        /// <summary>
        /// Retorna el listado de alertas para estadísticas
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <param name="tpNotificacion"></param>
        /// <param name="feNotificacionDesde"></param>
        /// <param name="feNotificacionHasta"></param>
        /// <param name="feRegistroDesde"></param>
        /// <param name="feRegistroHasta"></param>
        /// <returns></returns>
        public byte[] generarExcelListaAlertas(int? cdUsuario
                                              , int? tpNotificacion
                                              , DateTime? feNotificacionDesde
                                              , DateTime? feNotificacionHasta
                                              , DateTime? feRegistroDesde
                                              , DateTime? feRegistroHasta)
        {
            byte[] salida = null;

            if (feNotificacionHasta != null)
            {
                feNotificacionHasta = feNotificacionHasta.Value.AddDays(1);
                feNotificacionHasta = feNotificacionHasta.Value.AddSeconds(-1);
            }


            if (feRegistroHasta != null)
            {
                feRegistroHasta = feRegistroHasta.Value.AddDays(1);
                feRegistroHasta = feRegistroHasta.Value.AddSeconds(-1);
            }


            //Se obtiene el listado de Tiketes
            using (IntranetSAIEntities db = new IntranetSAIEntities())
            {
                var resultQuery = (from x in db.FUNC_003_BUSCA_GESTION_ALERTAS(cdUsuario,tpNotificacion,feNotificacionDesde,feNotificacionHasta,feRegistroDesde,feRegistroHasta)
                                   orderby x.FE_NOTIFICACION
                                   select x
                                    );



                using (ExcelPackage pck = new ExcelPackage())
                {
                    pck.Workbook.Properties.Author = "Sai Innovación";
                    pck.Workbook.Properties.Title = Resources.TituloPaginaResource.AlertasAgentes;
                    var sheet = pck.Workbook.Worksheets.Add(Resources.TituloWidget.ListadoAlertas);

                    sheet.Cells[1, 1].Value = Resources.CampoResource.Id;
                    sheet.Cells[1, 2].Value = Resources.CampoResource.Documento;
                    sheet.Cells[1, 3].Value = Resources.CampoResource.NombreApellido;
                    sheet.Cells[1, 4].Value = Resources.CampoResource.TipoNotificacion;
                    sheet.Cells[1, 5].Value = Resources.CampoResource.NumeroDVR;
                    sheet.Cells[1, 6].Value = Resources.CampoResource.TarjetaSim;
                    sheet.Cells[1, 7].Value = Resources.CampoResource.UsuarioAlerta;
                    sheet.Cells[1, 8].Value = Resources.CampoResource.Distribuidor;
                    sheet.Cells[1, 9].Value = Resources.CampoResource.Estatus;
                    sheet.Cells[1, 10].Value = Resources.CampoResource.NumeroTicket;
                    sheet.Cells[1, 11].Value = Resources.CampoResource.FechaNotificacion;
                    sheet.Cells[1, 12].Value = Resources.CampoResource.HoraNotificacion;
                    sheet.Cells[1, 13].Value = Resources.CampoResource.FechaRegistro;
                    sheet.Cells[1, 14].Value = Resources.CampoResource.HoraRegistro;

                    using (ExcelRange rng = sheet.Cells["A1:N1"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                        rng.Style.Font.Color.SetColor(Color.White);


                    }
                    var rowIndex = 2;
                    foreach (var item in resultQuery)
                    {
                        var col = 1;
                        
                        sheet.Cells[rowIndex, col++].Value = item.ID_NOTIFICACION;
                        sheet.Cells[rowIndex, col++].Value = item.NU_DOCUMENTO_IDENTIDAD;
                        sheet.Cells[rowIndex, col++].Value = item.DE_NOMBRE_APELLIDO_RAZON_SOCIAL; //((String.IsNullOrWhiteSpace(item.NM_CLIENTE) ? "" : item.NM_CLIENTE) + " " + (String.IsNullOrWhiteSpace(item.NM_APELLIDO_CLIENTE) ? "" : item.NM_APELLIDO_CLIENTE)).Trim();
                        sheet.Cells[rowIndex, col++].Value = item.DE_TIPO_NOTIFICACION;
                        sheet.Cells[rowIndex, col++].Value = item.NU_DVR;
                        sheet.Cells[rowIndex, col++].Value = item.NU_SIM;
                        sheet.Cells[rowIndex, col++].Value = item.NM_USUARIO;
                        sheet.Cells[rowIndex, col++].Value = item.NM_DISTRIBUIDOR;
                        sheet.Cells[rowIndex, col++].Value = item.ID_TICKET==null|| item.ID_TICKET==0?Resources.DescripcionResource.NoGestionada: Resources.DescripcionResource.Gestionada;
                        sheet.Cells[rowIndex, col++].Value = item.ID_TICKET;
                        sheet.Cells[rowIndex, col++].Value = item.FE_NOTIFICACION;
                        sheet.Cells[rowIndex, col++].Value = item.FE_NOTIFICACION;
                        sheet.Cells[rowIndex, col++].Value = item.FE_CREACION;
                        sheet.Cells[rowIndex, col++].Value = item.FE_CREACION;
                        rowIndex++;

                    }
                    sheet.Column(11).Style.Numberformat.Format = Core.Constante.AppFormat.dateJson;
                    sheet.Column(12).Style.Numberformat.Format = Core.Constante.AppFormat.hourJson;
                    sheet.Column(13).Style.Numberformat.Format = Core.Constante.AppFormat.dateJson;
                    sheet.Column(14).Style.Numberformat.Format = Core.Constante.AppFormat.hourJson;
                    salida = pck.GetAsByteArray();

                }
            }
            return salida;
        }
    }
}