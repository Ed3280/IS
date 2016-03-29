using System.Web;
using System.Web.Optimization;

namespace IntranetWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            var bundleJquery = new ScriptBundle("~/bundles/jquery").Include(
                          "~/Scripts/jquery-{version}.js"
                         , "~/Scripts/jquery-ui.js"
                         ,"~/Scripts/jquery.globalize/globalize.js"
                         , "~/Scripts/jquery.globalize/cultures/globalize.cultures.js"

                          );
            bundleJquery.Orderer = new Core.Utils.NonOrderingBundleOrderer();
            bundles.Add(bundleJquery);


            var bundleValidate = new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js"
                        , "~/Scripts/localization/messages_es.js"
                        , "~/Scripts/jquery.validate.unobtrusive.js"
                        , "~/Scripts/jquery.unobtrusive-ajax.js"
                        , "~/Scripts/_extensions_validate.js"
                        , "~/Scripts/additional-methods.js"
                        , "~/Scripts/jquery.validate.globalize.js");

                        bundleValidate.Orderer = new Core.Utils.NonOrderingBundleOrderer();
            //Validación
            bundles.Add(bundleValidate);
            



            //JqueryTables
            bundles.Add(new ScriptBundle("~/bundles/jqueryTable").Include(
                   "~/Scripts/dataTables/jquery.dataTables.js"
                   , "~/Scripts/dataTables/jquery.dataTables.bootstrap.js"
                   , "~/Scripts/dataTables/extensions/TableTools/js/dataTables.tableTools.js"
                   , "~/Scripts/dataTables/extensions/ColVis/js/dataTables.colVis.js"
                   , "~/Scripts/dataTables/extensions/Responsive/dataTables.responsive.js"
                   ));

            //Jquery chosen 
            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                   "~/Scripts/chosen.jquery.js"
                   ));

            //Jquery select2
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                   "~/Scripts/select2.js"
                  ,"~/Scripts/i18n/es.js"                
                   ));


            //Color box (gallery)
            bundles.Add(new ScriptBundle("~/bundles/colorbox").Include(
                   "~/Scripts/jquery.colorbox.js"
                   ));


            //Bootstrap duallistbox
            bundles.Add(new ScriptBundle("~/bundles/duallistbox").Include(
                   "~/Scripts/jquery.bootstrap-duallistbox.js"
                  ));


            //Notificación
            bundles.Add(new ScriptBundle("~/bundles/notify").Include(
                   "~/Scripts/notify.js" //"~/Scripts/notify.min.js"
                   ));

            //Modal 
            bundles.Add(new ScriptBundle("~/bundles/modal").Include(
                "~/Scripts/bootbox.js"
                ));

            //Typeahead
            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                "~/Scripts/typeahead.jquery.js"
                ));
            
            //IntranetSAI
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI").Include(
                  "~/Scripts/IntranetSAI/functions.js"
                   ));
            
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //Ace Core
            bundles.Add(new ScriptBundle("~/bundles/ace").Include(
                      "~/Scripts/ace-extra.js" 
                     , "~/Scripts/ace-elements.js"
                     , "~/Scripts/ace.js"                     
                     , "~/Scripts/fontawesome-markers.min.js") 
                     );


            //Core
            bundles.Add(new StyleBundle("~/Content/css/cssIntra").Include(
                       "~/Content/css/bootstrap.css"
                       , "~/Content/css/ace-checkbox.css"
                       , "~/Content/css/ace.css"
                       , "~/Content/css/ace-part2.css"
                       , "~/Content/css/open_sans.css"
                       , "~/Content/css/font-awesome.css"
                       , "~/Content/css/material-icons.css"
                       , "~/Content/css/datepicker.css"                       
                      , "~/Content/css/responsive.bootstrap.css"
                       , "~/Content/css/responsive.dataTables.css"
                       , "~/Content/css/responsive.foundation.css"
                       , "~/Content/css/responsive.jqueryui.css"                       
                       , "~/Content/css/style.css")
                     );

            //Chosen css
            bundles.Add(new StyleBundle("~/Content/css/chosen").Include(
                       "~/Content/css/chosen.css"));
            //Colorbox
            bundles.Add(new StyleBundle("~/Content/css/colorbox").Include(
                       "~/Content/css/colorbox.min.css"));

            

            //Select2
            bundles.Add(new StyleBundle("~/Content/css/select2").Include(
                                "~/Content/css/select2.css"));

            //Bootstrap duallistbox
            bundles.Add(new StyleBundle("~/Content/css/duallistbox").Include(
                                "~/Content/css/bootstrap-duallistbox.css"));

            //Masked Input 
            bundles.Add(new ScriptBundle("~/bundles/masked").Include(
                      "~/Scripts/jquery.maskedinput.js"
                     ));

            //Date Picker
            bundles.Add(new ScriptBundle("~/bundles/date-time/datepicker").Include(
                 "~/Scripts/date-time/bootstrap-datepicker.js"
                 , "~/Scripts/date-time/moment.js"
                 , "~/Scripts/date-time/locales/es.js"
                 , "~/Scripts/date-time/bootstrap-datetimepicker.js"
                , "~/Scripts/date-time/locales/bootstrap-datepicker.es-PA.js"
                ));


            //SignalR
            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                  "~/Scripts/jquery.signalR-2.2.0.js"
                ));

            //Treeview 
            bundles.Add(new ScriptBundle("~/bundles/fuelux/treeview").Include(
                "~/Scripts/fuelux/fuelux.tree.js"
                ));

            //Nested List
            bundles.Add(new ScriptBundle("~/bundles/nestable").Include(
                "~/Scripts/jquery.nestable.js"
                ));

            //Wizard
            bundles.Add(new ScriptBundle("~/bundles/fuelux/wizard").Include(
                "~/Scripts/fuelux/fuelux.wizard.js"
                ));

            //Flot
            bundles.Add(new ScriptBundle("~/bundles/flot/chartPie").Include(
                "~/Scripts/flot/jquery.flot.js"
                , "~/Scripts/flot/jquery.flot.pie.js"
                , "~/Scripts/flot/jquery.flot.resize.js"
                ));

            //Cliente
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Cliente/cliente").Include(
                "~/Scripts/intranetSAI/Cliente/cliente.js"
                ));

            //Configuración
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Cliente/configuracion").Include(
                "~/Scripts/intranetSAI/Configuracion/configuracion.js"));


            //Dashboard
            var bundleDashboard = new ScriptBundle("~/bundles/intranetSAI/Dashboard/dashboard").Include(
                  "~/Scripts/intranetSAI/Dashboard/cumpleano.js"
                , "~/Scripts/intranetSAI/Dashboard/directorio.js"
                , "~/Scripts/intranetSAI/Dashboard/dashboard.js"
                , "~/Scripts/intranetSAI/Dashboard/ticketUsuario.js"

                );

            bundleDashboard.Orderer = new Core.Utils.NonOrderingBundleOrderer();
            bundles.Add(bundleDashboard);

            //Monitor
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Monitor/monitor").Include(
                "~/Scripts/intranetSAI/Monitor/monitorTicket.js"
                , "~/Scripts/intranetSAI/Monitor/monitorMapa.js"
                , "~/Scripts/jquery.easing.1.3.js"
                , "~/Scripts/markerAnimate.js"
                , "~/Scripts/SlidingMarker.min.js"
            ));
            
            //Localización
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Monitor/localizacion").Include(
                "~/Scripts/intranetSAI/Monitor/monitorMapa.js"
                , "~/Scripts/jquery.easing.1.3.js"
                , "~/Scripts/markerAnimate.js"
                , "~/Scripts/SlidingMarker.min.js"

            ));

            //Mantenimiento inteligente
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Servicio/mantenimientoInteligente").Include(
                "~/Scripts/intranetSAI/Servicio/mantenimientoInteligente.js"
            ));
            

            //Kilometraje
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Kilometraje/verKilometraje").Include(
                "~/Scripts/intranetSAI/Kilometraje/verKilometraje.js"));


            //Login
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Auth/login").Include(
                "~/Scripts/intranetSAI/Auth/login.js"));

            //Reporte estadística ticket          
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Estadistica/reporteEstadisticaTicket").Include(
                "~/Scripts/intranetSAI/Estadistica/reporteEstadisticaTicket.js"
                ));

            //Reporte estadística alerta
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Estadistica/reporteEstadisticaAlerta").Include(
                "~/Scripts/intranetSAI/Estadistica/reporteEstadisticaAlerta.js"
            ));

            
            #region Administrador
            
            //Cargo
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/cargo").Include(
               "~/Scripts/intranetSAI/Administrador/cargo.js"
           ));

            //Empleado
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/empleado").Include(
                "~/Scripts/intranetSAI/Administrador/empleado.js"
                ));

            //Log transacciones
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/logTransacciones").Include(
                "~/Scripts/intranetSAI/Administrador/adminLogTransacciones.js"));

            //Parametro configuración
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/parametroConfiguracion").Include(
                "~/Scripts/intranetSAI/Administrador/parametroConfiguracion.js"));

            //Proveedor
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/proveedor").Include(
                "~/Scripts/intranetSAI/Administrador/proveedor.js"));

            //Respuesta operador GCM
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/respuestaOperadorGCM").Include(
                "~/Scripts/intranetSAI/Administrador/respuestaOperadorGCM.js"
                ));

            //Rol
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/rol").Include(
                "~/Scripts/intranetSAI/Administrador/rol.js"));
            
            //Socio comercial
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/socioComercial").Include(
               "~/Scripts/intranetSAI/Administrador/socioComercial.js"               
           ));


            //Unidad administrativa
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/unidadAdministrativa").Include(
                "~/Scripts/intranetSAI/Administrador/unidadAdministrativa.js"
                ));

            //Usuario admin
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/usuarioAdmin").Include(
                "~/Scripts/intranetSAI/Administrador/usuarioAdmin.js"
                ));



            //Usuario
            bundles.Add(new ScriptBundle("~/bundles/intranetSAI/Administrador/usuario").Include(
                "~/Scripts/intranetSAI/Administrador/usuario.js"
                ));

            #endregion



            bundles.UseCdn = true;

            //Google Maps 
            bundles.Add(new ScriptBundle("~/bundles/google-maps", "http://maps.google.com/maps/api/js?libraries=geometry,places&sensor=false"));

        }
    }
}
