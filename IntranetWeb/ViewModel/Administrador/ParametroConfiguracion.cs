using IntranetWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetWeb.ViewModel.Administrador
{
    public class ParametroConfiguracion {


        public ParametroConfiguracion() {
            this.CodigoConfiguracionEditar = "sin valor";
        }

        [Key]        
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Nombre del Parámetro")]
        [MaxLength(20 , ErrorMessage ="Nombre de Parámetro no debe exceder los 20 caracteres")]
        public string NombreParametro { get; set; }

        [Key]
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name="Valor")]
        [MaxLength(80, ErrorMessage = "Valor no debe exceder los 80 caracteres")]
        public string  CodigoConfiguracion { get; set; }

      
         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [MaxLength(80, ErrorMessage = "Valor no debe exceder los 80 caracteres")]
        public string CodigoConfiguracionEditar { get; set; }


         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Descripción")]
        [MaxLength(200, ErrorMessage = "Descripción no debe exceder los 200 caracteres")]
        public string Descripcion { get; set; }

         [Required(ErrorMessageResourceType = typeof(Resources.ValidacionResource), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Uso")]
        [MaxLength(200, ErrorMessage = "Uso debe exceder los 200 caracteres")]
        public string Uso { get; set; }

        public string EditarRegistro { get; set; }


        public PARAMETRO_CONFIGURACION toModel() {
            PARAMETRO_CONFIGURACION parametroConfiguracion = new PARAMETRO_CONFIGURACION();

            parametroConfiguracion.CD_CONFIGURACION = this.CodigoConfiguracion;
            parametroConfiguracion.DE_CONFIGURACION = this.Descripcion;
            parametroConfiguracion.DE_USO_CONFIGURACION = this.Uso;
            parametroConfiguracion.NM_PARAMETRO = this.NombreParametro;
            parametroConfiguracion.CD_CONFIGURACION_EDITAR = this.CodigoConfiguracionEditar;
            return parametroConfiguracion;

        }

    }
}