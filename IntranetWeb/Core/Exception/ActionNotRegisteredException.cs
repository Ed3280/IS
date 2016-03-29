using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetWeb.Core.Exception
{
 


        public class ActionNotRegisteredException : System.Exception
        {
            public ActionNotRegisteredException()
            {
            }

            public ActionNotRegisteredException(string message)
            : base(message)
            {
            }

            public ActionNotRegisteredException(string message, System.Exception inner)
            : base(message, inner)
            {
            }

        }
    
}