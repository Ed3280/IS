using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace IntranetWeb.Core.Seguridad
{
    public class UserClaimType     {
        
        public static String Roles { get { return "Roles"; } private set { } }
        public static String UserName { get { return "UserName";  } private set { } }
        public static String Nombre { get { return "Name"; } private set { } }
        public static String Correo { get { return "Email"; } private set { } }
        public static String Id { get { return "ID"; } private set { } }
        public static String IdEmpleado {get { return "IdEmpleado"; } private set { } }
    }
}