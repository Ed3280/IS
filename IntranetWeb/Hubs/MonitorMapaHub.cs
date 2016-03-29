using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.Core.Respositorios;
using IntranetWeb.ViewModel.Monitor;
using System.Threading.Tasks;

namespace IntranetWeb.Hubs
{
    public class MonitorMapaHub : Hub
    {
        private Log4NetLogger log;
        private string tokenUser;
        private MonitorRepositorio monitorRepo;
        private KilometrajeRepositorio kilometrajeRepo;

        private static int _userCount = 0;

        public MonitorMapaHub() {
            log = new Log4NetLogger();
            monitorRepo = new MonitorRepositorio();
            kilometrajeRepo = new KilometrajeRepositorio();

            this.LoginUser();
        }
        
        //Actualiza el usuario
        public void LoginUser() {
            if (this.tokenUser == null){
                var result = IntranetWeb.Core.WebService.ApiPlataforma.UsersClient.Login(1); //Superadmin
                if (result != null)
                   this.tokenUser = result["State"].Value==0?result["Token"].Value: null;                
            }
        }


        public void ActualizaLocalizacionVehiculos(){
            this.ActualizaLocalizacionVehiculos(null);            
        }

        /// <summary>
        /// Se coloca toda la lógica para buscar la información de los vehículos para 
        /// </summary>
        /// <param name="ihc">Hub Context para el monitor de alertas</param>
        public void ActualizaLocalizacionVehiculos(IHubContext ihc){

            if (_userCount > 0)
            {
                List<LocalizacionDispositivo> lld = new List<LocalizacionDispositivo>();

                if (this.tokenUser != null)
                {

                    LocalizacionDispositivo ld;
                    //Se buscan los dispositivos
                    ArbolDispositivo arbolDisp = monitorRepo.obtenDispositivosNested(this.tokenUser);
                    ViewModel.Kilometraje.Vehiculo vehiculokilometraje;
                    foreach (NodoDistribuidor nodo in arbolDisp.Distribuidores)
                        foreach (Dispositivo disp in nodo.DispositivosAsociados)
                        {
                            ld = IntranetWeb.Core.WebService.ApiPlataforma.LocationClient.GetTrack(disp.Id, tokenUser);
                            ld.Dealer = nodo.Distribuidor.Nombre;
                            ld.Name = disp.Nombre;
                            ld.status = disp.DescripcionEstatus;
                            ld.SerialNumber = disp.Serial;
                            vehiculokilometraje = kilometrajeRepo.obten_Vehiculo_KILOMETRAJE_TOTALByID(nodo.Distribuidor.Id, disp.Id);
                            ld.KilometrajeTotal = vehiculokilometraje!= null? Convert.ToInt32(vehiculokilometraje.DistanciaRecorrida):0;
                            if (ld.Id != 0) lld.Add(ld);
                        }
                }

                if (lld.Count == 0)
                    LoginUser();

                //Se actualiza la información del lado del cliente 
                if (ihc == null)
                    Clients.All.retornaLocalizacionAll(lld);
                else
                    ihc.Clients.All.retornaLocalizacionAll(lld);
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