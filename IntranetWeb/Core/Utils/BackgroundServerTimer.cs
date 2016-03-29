using Microsoft.AspNet.SignalR;
using System.Threading;
using System;
using Humanizer;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using IntranetWeb.Hubs;

namespace IntranetWeb.Core.Utils
{


    public class BackgroundServerTimer : IRegisteredObject
    {
        private readonly IHubContext _ImonitorMapaHub;
        private readonly IHubContext _ImonitorAlertaHub;

        MonitorMapaHub monitorMapaHub;
        MonitorAlertaHub monitorAlertaHub;

        private Timer _timerMapa;
        private Timer _timerTicket;

        public BackgroundServerTimer()
        {
            _ImonitorMapaHub = GlobalHost.ConnectionManager.GetHubContext<MonitorMapaHub>();
            _ImonitorAlertaHub = GlobalHost.ConnectionManager.GetHubContext<MonitorAlertaHub>();

            monitorMapaHub = new  MonitorMapaHub();
            monitorAlertaHub = new MonitorAlertaHub();
            StartTimer();
        }

        private void StartTimer()
        {
            
            _timerMapa = new Timer(BroadcastMonitorMapaToClients, null, 1.Seconds(), 9.Seconds());
            _timerTicket = new Timer(BroadcastMonitorAlertasToClients, null, 1.Seconds(), 10.Seconds());
        }
        private void BroadcastMonitorMapaToClients(object state)
        {
            monitorMapaHub.ActualizaLocalizacionVehiculos(_ImonitorMapaHub);
            
        }

        private void BroadcastMonitorAlertasToClients(object state)
        {
            monitorAlertaHub.ActualizaTickets(_ImonitorAlertaHub);

        }


        public void Stop(bool immediate)
        {
            _timerMapa.Dispose();
            _timerTicket.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }

    } 

}