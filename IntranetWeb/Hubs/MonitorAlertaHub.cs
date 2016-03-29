using System;
using Microsoft.AspNet.SignalR;
using IntranetWeb.Core.Servicio.Logging;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace IntranetWeb.Hubs
{
    public class MonitorAlertaHub : Hub
    {
        private Log4NetLogger log;
        private IntranetWeb.Controllers.MonitorController monitorController;
      
        // Use this variable to track user count
        private static int _userCount = 0;

        public MonitorAlertaHub()
        {
            log = new Log4NetLogger();
            monitorController = Core.Utils.UtilHelper.CreateController<Controllers.MonitorController>(); 
        }


        public void ActualizaTickets(){
            this.ActualizaTickets(null);
        }

        
        public void ActualizaTickets(IHubContext ihc)
        {

            if (_userCount > 0){
                var result = new Object();

                try{
                    var salida = (PartialViewResult)monitorController._actualizaAlertas();
                    if (Context != null)                    
                          monitorController = Core.Utils.UtilHelper.CreateController<Controllers.MonitorController>(Core.Utils.MockHelper.FakeHttpContext());
                         result = IntranetWeb.Core.Utils.HtmlHelper.RenderViewToString(monitorController.ControllerContext, "_actualizaAlertas", salida.Model);
                }
                catch (Exception exc) {
                    log.Error(Resources.ErrorResource.Error100,exc);
                    result = Core.Utils.UtilJson.Error(Resources.ErrorResource.Error100); 
                }

                if (ihc == null)
                    Clients.All.actualizaTicketsAll(result);
                else
                    ihc.Clients.All.actualizaTicketsAll(result);
            }
        }
        

        public override Task OnConnected()
        {
            _userCount++;
            return base.OnConnected();

        }
        public override Task OnReconnected()
        {
            _userCount++;
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            _userCount--;
            return base.OnDisconnected(stopCalled);
        }


    }
}