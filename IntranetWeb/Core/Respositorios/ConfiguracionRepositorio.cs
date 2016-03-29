using IntranetWeb.ViewModel.Configuracion;
using IntranetWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace IntranetWeb.Core.Respositorios
{
    public class ConfiguracionRepositorio
    {

        /// <summary>
        /// Permite obtener el perfil del usuario por el id del usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Perfil obten_Perfil_ById(int idUsuario) {
            Perfil per = null;

            using (IntranetSAIEntities db = new IntranetSAIEntities()) {

                per = (from x in db.USUARIO
                       where x.CD_USUARIO == idUsuario
                       select new Perfil() {
                           Usuario = new Usuario{
                           Id = x.CD_USUARIO
                           ,Nombre = x.DE_NOMBRE_APELLIDO
                           ,FechaNacimiento = x.FE_NACIMIENTO
                           ,Email = x.DI_EMAIL_USUARIO
                           ,TelefonoFijo = x.NU_TELEFONO_FIJO
                           ,TelefonoMovil = x.NU_TELEFONO_MOVIL
                           ,NumeroDocumentoIdentidad = x.NU_DOCUMENTO_IDENTIDAD
                           ,TipoDocumentoIdentidad = x.TP_DOCUMENTO_IDENTIDAD                           
                            }
                           ,Empleado = x.EMPLEADO!=null?new Empleado {
                                Id                      = x.EMPLEADO.CD_USUARIO
                               ,Cargo                   = x.EMPLEADO.CARGO.DE_CARGO
                               ,Email                   = x.EMPLEADO.DI_CORREO
                               ,Extension               = x.EMPLEADO.NU_EXTENSION
                               ,FechaIngreso            = x.EMPLEADO.FE_INGRESO
                               ,Sucursal                = x.EMPLEADO.SUCURSAL.NM_SUCURSAL
                               ,Supervisor              = x.EMPLEADO.EMPLEADO2.USUARIO.DE_NOMBRE_APELLIDO
                               ,UnidadAdministrativa    = x.EMPLEADO.CARGO.UNIDAD_ADMINISTRATIVA.NM_UNIDAD_ADMINISTRATIVA
                           } :null
                        }
                       ).FirstOrDefault();
            }

            return per; 
        }


        /// <summary>
        /// Permite obtener el objeto USUARIO dado el id del usuario
        /// </summary>
        /// <param name="cdUsuario"></param>
        /// <returns></returns>
        public USUARIO obten_USUARIO_ById(int cdUsuario) {
            using (IntranetSAIEntities db = new IntranetSAIEntities()) {
                return (from x in db.USUARIO
                        where x.CD_USUARIO == cdUsuario
                        select x
                        ).FirstOrDefault();
                    }

        }

        


        /// <summary>
        /// Permite actualizar la entidad USUARIO
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int actualiza_USUARIO(USUARIO usuario) {

            using (var db = new IntranetSAIEntities()){

                db.USUARIO.Attach(usuario);
                db.Entry(usuario).State = EntityState.Modified;
                return db.SaveChanges();

            }

        }
    }
}