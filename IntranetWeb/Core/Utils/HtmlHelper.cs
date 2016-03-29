using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;

namespace IntranetWeb.Core.Utils
{
    public static class HtmlHelper
    {
        public static object Url { get; private set; }

        /// <summary>
        /// Escribe un mesaje de éxito
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public static String escribirMensajeExito(string mensaje)
        {
            return "<div class=\"alert alert-block alert-success\">" +
                        "<button type= \"button\" class=\"close\" data-dismiss=\"alert\"> " +
                                                    "<i class=\"ace-icon fa fa-times\"></i> " +
                                                "</button> " +
                        "<p> " + mensaje + "</p>" +
                    "</div>";

        }

        /// <summary>
        /// Escribe mensaje de error
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public static String escribirMensajeError(string mensaje)
        {
            return "<div class=\"alert alert-danger\">" +
                        "<button type= \"button\" class=\"close\" data-dismiss=\"alert\"> " +
                                                    "<i class=\"ace-icon fa fa-times\"></i> " +
                                                "</button> " +
                        "<p> " + mensaje +
                        "</p>" +
                    "</div>";

        }

        /// <summary>
        /// Escribe mensaje de advertenecia
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public static String escribirMensajeAdvertencia(string mensaje, bool inCerrar)
        {
            return "<div class=\"alert alert-warning\">" +
                        
                            (inCerrar?"<button type= \"button\" class=\"close\" data-dismiss=\"alert\"> " +
                                                    "<i class=\"ace-icon fa fa-times\"></i> " +
                                                "</button> ":"")
                                                +
                        "<p> " + mensaje +
                        "</p>" +
                    "</div>";

        }


        
        /// <summary>
        /// Escribe mensaje de advertencia 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="mensaje"></param>
        /// <param name="inCerrar"></param>
        /// <returns></returns>
        public static MvcHtmlString escribirMensajeAdvertencia(
           this System.Web.Mvc.HtmlHelper htmlHelper
            , string mensaje, bool inCerrar)
        {
            return new MvcHtmlString(escribirMensajeAdvertencia(mensaje,  inCerrar));
        }

        /// <summary>
        /// Permite colocar una etiqueta bold
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelBoldFor<TModel, TValue>(
           this System.Web.Mvc.HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, TValue>> expression, object htmlAttributes

       )
        {
            object salida="";
            RouteValueDictionary diccionario =  new RouteValueDictionary(htmlAttributes);
            diccionario.TryGetValue("class", out salida);
            
           salida += salida == null? "bolder":" bolder";
            
            if (diccionario.ContainsKey("class"))
                diccionario["class"] = salida;
            else
                diccionario.Add("class", salida);
            return htmlHelper.LabelFor(expression, diccionario);
            
        }

        /// <summary>
        /// Crea un campo tipo label bold
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="field"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelBold<TModel>(
        this System.Web.Mvc.HtmlHelper<TModel> htmlHelper
         ,string field, object htmlAttributes

    )
        {
            object salida = "";
            RouteValueDictionary diccionario = new RouteValueDictionary(htmlAttributes);
            diccionario.TryGetValue("class", out salida);

            salida += salida == null ? "bolder" : " bolder";

            if (diccionario.ContainsKey("class"))
                diccionario["class"] = salida;
            else
                diccionario.Add("class", salida);
            return htmlHelper.Label(field, diccionario);

        }

        /// <summary>
        /// label bold para campos sin diccionario de parámetros
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelBoldFor<TModel, TValue>(
          this System.Web.Mvc.HtmlHelper<TModel> htmlHelper
           , Expression<Func<TModel, TValue>> expression

      )
        {

            var htmlAttributes = new { @class = "bolder" };
            RouteValueDictionary diccionario = new RouteValueDictionary(htmlAttributes);
            return htmlHelper.LabelFor(expression, diccionario);

        }

        /// <summary>
        /// Etiqueta bold para campos sin parámtros
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelBold<TModel>(
            this System.Web.Mvc.HtmlHelper<TModel> htmlHelper
        , string field
        )
        {
            var htmlAttributes = new { @class = "bolder" };
            RouteValueDictionary diccionario = new RouteValueDictionary(htmlAttributes);
            return htmlHelper.Label(field, diccionario);

        }

        /// <summary>
        /// Permite 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="parameters"></param>
        /// <param name="iconLink"></param>
        /// <param name="linkText"></param>
        /// <returns></returns>
        public static MvcHtmlString MenuLink(
            this System.Web.Mvc.HtmlHelper htmlHelper
            , string actionName
            , string controllerName
            , System.Web.Routing.RouteValueDictionary parameters
            , string iconLink
            , string linkText
            ,  bool isChildren 
            )
        {
            String result = "";
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string currentAction = htmlHelper.ViewContext.ParentActionViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.ParentActionViewContext.RouteData.GetRequiredString("controller");//htmlHelper.ViewContext.RouteData.GetRequiredString("controller");


            bool isActive = urlHelper.Action(actionName, controllerName, parameters) == htmlHelper.ViewContext.ParentActionViewContext.HttpContext.Request.Url.PathAndQuery ? true : false;

            result += isActive ? "<li class=\"active liMenuActive\">" : "<li> ";
            result += "<a  href=\"" + urlHelper.Action(actionName, controllerName, parameters) + "\">";


            if (isChildren){
                result += "<i class=\"menu-icon fa fa-caret-right\"></i>";                
            }
            else {
                if (!String.IsNullOrEmpty(iconLink))
                    result += "<i class=\"menu-icon " + iconLink + "\"></i>";
            }
                

            result += "<span class=\"menu-text\">"+((isChildren&& !String.IsNullOrEmpty(iconLink))?"<i class=\"menu-icon " + iconLink + "\"></i> ":"")+ linkText + "</span>"
                + "</a>"
                + "</li>";


            return new MvcHtmlString(result);

        }


        public static MvcHtmlString IconosGestionRegistro(
        this System.Web.Mvc.HtmlHelper htmlHelper
      , Dictionary<String, String> dataAttributtes
      , bool InIconoConsulta
      , bool InIconoEditar
      , bool InIconoEliminar
            )
        {
            return new MvcHtmlString(IconosGestionRegistro(
                                     dataAttributtes
                                   , InIconoConsulta
                                   , InIconoEditar
                                   , InIconoEliminar));
        }


        /// <summary>
        /// Método retorna los íconos de consulta, actualización y eliminación
        /// </summary>
        /// <param name="dataAttributtes"></param>
        /// <param name="InInconoConsulta"></param>
        /// <param name="InInconoEditar"></param>
        /// <returns></returns>
        public static String IconosGestionRegistro(
         Dictionary<String, String> dataAttributtes
       , bool InIconoConsultar
       , bool InIconoEditar
       , bool InIconoEliminar)
        {
            String result = "";

            result += "<div class=\"hidden-sm hidden-xs action-buttons\"> ";
            
            if (InIconoConsultar) {
                result += "<a class=\"tooltip-info\" data-placement=\"right\" data-rel=\"tooltip\" title=\""+Resources.EtiquetaResource.ConsultarRegistro+"\" href=\"javascript:void(0);\"> "
                        + "<span class=\"blue\"> "
                        + "<i ";

                foreach (var data in dataAttributtes) 
                    result += data.Key+"=\""+data.Value+"\"";
            
                result +=  " class=\"ace-icon fa fa-search-plus bigger-120 icono-consultar\"></i> "
                + "</span> "
                + "</a> ";
            }


            if (InIconoEditar)
            {
                result  += "<a class=\"tooltip-success\" data-rel=\"tooltip\" data-placement=\"right\" title=\"" + Resources.EtiquetaResource.EditarRegistro + "\" href=\"javascript:void(0);\"> "
                        + "<span class=\"green\">"
                        + "<i ";

                foreach (var data in dataAttributtes)
                    result += data.Key + "=\"" + data.Value + "\"";

                    result += " class=\"ace-icon fa fa-pencil-square-o bigger-120 icono-editar\"></i> "
                            + "</span> "
                            + "</a> ";
            }

            if (InIconoEliminar)
            {
                result += "<a class=\"tooltip-error\" data-rel=\"tooltip\" data-placement=\"right\" title=\"" + Resources.EtiquetaResource.EliminarRegistro + "\" href=\"javascript:void(0);\"> "
                        + "<span class=\"red\">"
                        + "<i ";

                foreach (var data in dataAttributtes)
                    result += data.Key + "=\"" + data.Value + "\"";

                result += " class=\"ace-icon fa fa-trash-o bigger-120 icono-eliminar\"></i> "
                        + "</span> "
                        + "</a> ";
            }

            result +=
                      "</div> "
                    + "<div class=\"hidden-md hidden-lg\"> "
                    + "<div class=\"inline pos-rel\"> "
                        + "<button class=\"btn btn-minier btn-yellow dropdown-toggle\" data-toggle=\"dropdown\" data-position=\"auto\"> "
                            + "<i class=\"ace-icon fa fa-caret-down icon-only bigger-120\"></i> "
                        + "</button> "

                        + "<ul class=\"dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close\"> ";

            if (InIconoConsultar) {

                result += "<li> "
                          + "<a href = \"javascript:void(0);\" class=\"tooltip-info\" data-rel=\"tooltip\" title=\"" + Resources.EtiquetaResource.ConsultarRegistro + "\"> "
                          + "<span class=\"blue\"> "
                          + "<i ";

                foreach (var data in dataAttributtes)
                    result += data.Key + "=\"" + data.Value + "\"";

                result +=   " class=\"ace-icon fa fa-search-plus bigger-120 icono-consultar\"></i> "
                          + "</span> "
                          + "</a> "
                          + "</li> ";
            }

            if (InIconoEditar){
                result += "<li> "
                          + "<a href = \"javascript:void(0);\" class=\"tooltip-success\" data-rel=\"tooltip\" title=\"" + Resources.EtiquetaResource.EditarRegistro + "\"> "
                          + "<span class=\"green\"> "
                          + "<i ";

                foreach (var data in dataAttributtes)
                    result += data.Key + "=\"" + data.Value + "\"";

                result +=  " class=\"ace-icon fa fa-pencil-square-o bigger-120 icono-editar\"></i> "
                          + "</span> "
                          + "</a> "
                          + "</li> " ;
            }

            if (InIconoEliminar){
                result += "<li> "
                          + "<a href = \"javascript:void(0);\" class=\"tooltip-error\" data-rel=\"tooltip\" title=\"" + Resources.EtiquetaResource.EliminarRegistro + "\"> "
                          + "<span class=\"red\"> "
                          + "<i ";

                foreach (var data in dataAttributtes)
                    result += data.Key + "=\"" + data.Value + "\"";

                result += " class=\"ace-icon fa fa-trash-o bigger-120 icono-eliminar\"></i> "
                          + "</span> "
                          + "</a> "
                          + "</li> ";
            }


            result += "</ul> "
                   + "</div> "
                   + "</div> ";

            return result;
        }

        /// <summary>
        /// Checkbox con formato Ace
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxAceFor<TModel>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, bool>> expression,  object htmlAttributes)
        {

            object salida = "";
            RouteValueDictionary diccionario = new RouteValueDictionary(htmlAttributes);


            diccionario.Keys.ToList().ForEach(x => { if (x.StartsWith("data_")) {
                                                            Core.Utils.UtilHelper.RenameKey<string, object>(diccionario, x, x.Replace('_', '-'));
                                                    } });
            

            diccionario.TryGetValue("class", out salida);

            salida += salida == null ? "ace ace-switch ace-switch-5" : " ace ace-switch ace-switch-5";

            if (diccionario.ContainsKey("class"))
                diccionario["class"] = salida;
            else
                diccionario.Add("class", salida);

            string checkTotal = htmlHelper.CheckBoxFor(expression, diccionario).ToHtmlString().Trim();
            return HtmlHelper.GenerateCheckBoxAceFor(checkTotal,false);            
        }

      

        /// <summary>
        /// Checkbox con formato Ace (sólo expresión)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxAceFor<TModel>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, bool>> expression)
        {
            var htmlAttributes = new { @class = "ace ace-switch ace-switch-5" };
            RouteValueDictionary diccionario = new RouteValueDictionary(htmlAttributes);

            string checkTotal = htmlHelper.CheckBoxFor(expression, diccionario).ToHtmlString().Trim();
            return HtmlHelper.GenerateCheckBoxAceFor(checkTotal,false);
        }

        /// <summary>
        /// Checkbox con formato Ace (sólo expresión y en linea)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="inLine">Si el checkbox va en línea</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxAceFor<TModel>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, bool>> expression, bool inLine)
        {
            var htmlAttributes = new { @class = "ace ace-switch ace-switch-5" };
            RouteValueDictionary diccionario = new RouteValueDictionary(htmlAttributes);

            string checkTotal = htmlHelper.CheckBoxFor(expression, diccionario).ToHtmlString().Trim();
            return HtmlHelper.GenerateCheckBoxAceFor(checkTotal, inLine);
        }


        public static MvcHtmlString CheckBoxAceFor<TModel>(
            this System.Web.Mvc.HtmlHelper<TModel> htmlHelper
            , Expression<Func<TModel, bool>> expression
            , object htmlAttributes
            , bool inLine)
        {

            object salida = "";
            RouteValueDictionary diccionario = new RouteValueDictionary(htmlAttributes);


            diccionario.Keys.ToList().ForEach(x => {
                if (x.StartsWith("data_"))
                {
                    Core.Utils.UtilHelper.RenameKey<string, object>(diccionario, x, x.Replace('_', '-'));
                }
            });


            diccionario.TryGetValue("class", out salida);

            salida += salida == null ? "ace ace-switch ace-switch-5" : " ace ace-switch ace-switch-5";

            if (diccionario.ContainsKey("class"))
                diccionario["class"] = salida;
            else
                diccionario.Add("class", salida);

            string checkTotal = htmlHelper.CheckBoxFor(expression, diccionario).ToHtmlString().Trim();

         
            return HtmlHelper.GenerateCheckBoxAceFor(checkTotal,  inLine);
        }


        /// <summary>
        /// Genera String checkbox Ace
        /// </summary>
        /// <param name="htmlCheck"></param>
        /// <returns></returns>
        private static MvcHtmlString GenerateCheckBoxAceFor(String htmlCheck, bool inline)
        {
            

            string checkBoxWithHidden = htmlCheck.Trim(); 

            string pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1));
            string inputHiddenCheckBox = checkBoxWithHidden.Substring(checkBoxWithHidden.IndexOf("<input", 1), checkBoxWithHidden.Length- checkBoxWithHidden.IndexOf("<input", 1));
            string span = "<span class=\"lbl middle\"></span>";
            string checkboxAce = (inline?"":"<br \\>") +pureCheckBox + span+ inputHiddenCheckBox;
            return new MvcHtmlString(checkboxAce);
        }


        /// <summary>
        /// Retonra los mensajes de error del Model state
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string getFormErrorsMessage(ModelStateDictionary model)
        {

            StringBuilder result = new StringBuilder();
            result.Append("<div class= \"scrollable\" data-size=\"10\"> <ul>");
            foreach (var item in model)
            {
                string key = item.Key;
                var errors = item.Value.Errors;
                var arrErrores = new List<string>();
                foreach (var error in errors){
                    if (!arrErrores.Contains(error.ErrorMessage)){
                        result.Append("<li>" + error.ErrorMessage + "</li>");
                        arrErrores.Add(error.ErrorMessage);
                    }
                }
            }
            result.Append("</ul></div>");
            return String.Format("<div class= \"bolder\" >" + IntranetWeb.Core.Constante.Mensaje.Error.FormularioInvalido + "</div> {0}", result.ToString());
        }



        /// <summary>
        /// Dado el ícono de la plataforma, retorna el ícono de la intranet
        /// </summary>
        /// <param name="iconoPlataforma"></param>
        /// <returns></returns>
        public static string getIconoDispositivo(
              this System.Web.Mvc.HtmlHelper htmlHelper
            ,string iconoPlataforma)
        {
            iconoPlataforma = String.IsNullOrWhiteSpace(iconoPlataforma) ? "0": iconoPlataforma;

            string iconStr = iconoPlataforma.Replace("offline_", "");

            string iconNumber = iconStr.Split('_')[0];

            switch (Convert.ToInt32(iconNumber))
            {

                case 21:
                    return "fa fa-taxi";                    
                case 22:
                    return "fa fa-car";
                case 23:
                    return "fa fa-motorcycle";
                case 24:
                    return "fa fa-bus";
                case 25:
                case 27:
                    return "fa fa-truck";

                    case 26:
                    return "fa fa-ship";
                default:
                    return "fa fa-car";
            }
        }


        /// <summary>
        /// Rendea una vista a string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            var uicul = (System.Web.Configuration.GlobalizationSection)(System.Configuration.ConfigurationManager.GetSection("system.web/globalization"));

            CultureInfo ci = CultureInfo.GetCultureInfo(uicul.Culture);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        public static string RenderViewToString(ControllerContext context, PartialViewResult view  )
        {
            var viewData = new ViewDataDictionary(view.Model);

            using (var sw = new StringWriter())
            {
                var viewContext =   new ViewContext(context, view.View, viewData, new TempDataDictionary(), sw);
                view.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        public static string RenderPartialView(string controllerName, string partialView, object model)
        {
            var context = new HttpContextWrapper(System.Web.HttpContext.Current) as HttpContextBase;

            var routes = new System.Web.Routing.RouteData();
            routes.Values.Add("controller", controllerName);

            var requestContext = new RequestContext(context, routes);

            string requiredString = requestContext.RouteData.GetRequiredString("controller");
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            var controller = controllerFactory.CreateController(requestContext, requiredString) as ControllerBase;

            controller.ControllerContext = new ControllerContext(context, routes, controller);

            var ViewData = new ViewDataDictionary();

            var TempData = new TempDataDictionary();

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialView);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, ViewData, TempData, sw);

                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
  }