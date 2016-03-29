using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Servicio.Logging
{
    public class Log4NetLogger : ILogger
    {

        private ILog _logger;

        public Log4NetLogger()
        {
            BuildLogger(null);
        }


        public Log4NetLogger(int? cdUsuario)
        {
            BuildLogger(cdUsuario);
        }

        private void BuildLogger(int? cdUsuario) {
            _logger = LogManager.GetLogger(this.GetType());
            log4net.LogicalThreadContext.Properties["usercode"] = cdUsuario == null ? IntranetWeb.Core.Utils.UtilHelper.obtenUsuarioLogueado() :  cdUsuario;
            log4net.LogicalThreadContext.Properties["nm_controller"] = "";
            log4net.LogicalThreadContext.Properties["nm_action"] = "";
            log4net.LogicalThreadContext.Properties["nm_url"] = "";
            log4net.LogicalThreadContext.Properties["di_ip"] = "";
        }

        public void setUser(int cdUsuario) {
            log4net.LogicalThreadContext.Properties["usercode"] = cdUsuario;

        }

        public void Info(string message)
        {
            
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(System.Exception x)
        {
            Error(LogUtility.BuildExceptionMessage(x));
        }

        public void Error(string message, System.Exception x)
        {
            _logger.Error(message, x);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(string message, System.Exception x)
        {
            _logger.Fatal(message, x);
        }

        public void Fatal(System.Exception x)
        {
            Fatal(LogUtility.BuildExceptionMessage(x));
        }



    }
}