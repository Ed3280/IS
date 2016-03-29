using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Dashboard.Cumpleano
{
    public class EmpleadoCumpleano
    {

        public String Nombre { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = IntranetWeb.Core.Constante.AppFormat.dateDayMonth)]
        public DateTime FechaNacimiento { get; set; }

    }
}