using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetWeb.Core.Atributos
{
    public class PreparaLogAttribute : ActionFilterAttribute,  IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName  = filterContext.RouteData.Values["controller"];
            var actionName      = filterContext.RouteData.Values["action"];
            var url             = filterContext.HttpContext.Request.Url.PathAndQuery;
            var diIp            = filterContext.HttpContext.Request.UserHostAddress;
            log4net.LogicalThreadContext.Properties["nm_controller"] = controllerName;
            log4net.LogicalThreadContext.Properties["nm_action"]     = actionName;
            log4net.LogicalThreadContext.Properties["nm_url"]        = url;
            log4net.LogicalThreadContext.Properties["di_ip"]         = diIp;

        }


    }
}