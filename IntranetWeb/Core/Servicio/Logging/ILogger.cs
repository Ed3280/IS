using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntranetWeb.Core.Servicio.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void Warn(string message);

        void Debug(string message);

        void Error(string message);
        void Error(string message, System.Exception x);
        void Error(System.Exception x);

        void Fatal(string message);
        void Fatal(System.Exception x);
    }
}
