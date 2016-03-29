using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Exception
{
    public class BussinessException : System.Exception    {
        public BussinessException()
        {
        }

        public BussinessException(string message)
        : base(message)
    {
        }

        public BussinessException(string message, System.Exception inner)
        : base(message, inner)
        {
        }

    }
}