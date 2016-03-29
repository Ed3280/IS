using IntranetWeb.Core.Seguridad;
using IntranetWeb.Core.Servicio.Logging;
using IntranetWeb.Core.Utils;
using IntranetWeb.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IntranetWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ValidacionResource";
            DefaultModelBinder.ResourceClassKey                   = "ValidacionResource";


            //Enabling Unobtrusive Client Validation
            System.Web.Mvc.HtmlHelper.ClientValidationEnabled = true;
            System.Web.Mvc.HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
            
            //Se inicializa el timer de signalr
            HostingEnvironment.RegisterObject(new BackgroundServerTimer());
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            
            Exception exception = Server.GetLastError();

            try{
                Log4NetLogger log = new Log4NetLogger();
                log.Fatal(Core.Constante.Mensaje.Error.Error500,exception);
            }
            catch { }

        }

    }
}
