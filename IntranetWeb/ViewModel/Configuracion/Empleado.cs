using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Configuracion
{
    public class Empleado
    {

        [Key]
        public int Id { get; set; }
        

        [Display(Name = "Unidad Administrativa")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public string UnidadAdministrativa { get; set; }

        [Display(Name = "Cargo")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public string Cargo { get; set; }


        [Display(Name = "Extensión")]
        [RegularExpression(@"\d{3,7}", ErrorMessage = "Ingrese una extensión válida")]
        public int? Extension { get; set; }

        
        [Display(Name = "E-Mail Compañía")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail Compañía no es válido")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "E-mail Compañía no es válido")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        public string Email { get; set; }


        [Display(Name = "Supervisor")]
        public string Supervisor { get; set; }


        [Display(Name = "Sucursal")]
        public string Sucursal { get; set; }


        [Display(Name = "Fecha de Ingreso")]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Date, ErrorMessage = "Fecha de ingreso tiene un formato inválido"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Core.Constante.AppFormat.date)]
        public DateTime FechaIngreso { get; set; }

        public IntranetWeb.Models.EMPLEADO toModel()
        {
            var usuario             = new IntranetWeb.Models.EMPLEADO();

            usuario.CD_USUARIO      = this.Id;
            usuario.DI_CORREO       = this.Email;
            usuario.NU_EXTENSION    = this.Extension;

         
            return usuario;
        }



    }
}