var usuarioSAI = (function (window) {

    var idTablaPrincipal = '#tablaPrincipalAdministracion'
        ,idFormularioCrearRegistro = '#formCrearRegistro'
        , idFormularioEditarRegistro = '#formEditarRegistro'
        , camposBloqueo = '#ContrasenaNueva,#RepetirContrasena,#NombreApellido,#NombreUsuario,#FechaNacimiento,#TelefonoFijo,#TelefonoMovil,#Email,#IndicadorActivo'
        ,//Inicializa la tabla de datatables
            initTablaPrincipal = function () {      
                utilSAI.initTablaPrincipal({
                    idTabla: idTablaPrincipal
                    ,parametrosBusqueda : {}
                    ,serverSide : true
                });
            }
        ,
        //Inicializa las cosas que deban inicializarse en todal la página
        initAllDefault = function () {
            utilSAI.setRightScroll();
            utilSAI.initSelect2();
            utilSAI.initDate();
            utilSAI.initBootstrapDualListbox();
        }
        //Asocia los eventos necesarios a la página
        ,asociaEventos = function () {
            eventoConsultarRegistro();
            eventoCrearRegistro();
            eventoEliminarRegistro();
            eventoEditarRegistro();
            eventoPrevieneSubmitEnter();
        }
    ,eventoConsultarRegistro = function(){
        $(idTablaPrincipal).on('click', '.icono-consultar', function () {
            ConsultarRegistro($(this).data('id-usuario'));
        });
    }
    //Evento para crear un nuevo registro
    ,eventoCrearRegistro = function () {
        $('#divHeaderTablaPrincipal').on('click', '.icono-crear-registro', function () {
            CrearRegistro();
        });
    }
    , eventoEliminarRegistro = function () {
        $(idTablaPrincipal).on('click', '.icono-eliminar', function () {
            EliminarRegistro($(this).data('id-usuario'));
        });
    }
    , eventoEditarRegistro = function () {
        $(idTablaPrincipal).on('click', '.icono-editar', function () {
            EditarRegistro($(this).data('id-usuario'));
        });
    }
    //Consulta el detalle de un registro
    , ConsultarRegistro = function (id) {
        utilSAI.openModal({
            idTitulo: '#divTabConsultarUsuario'
          , url: $(idTablaPrincipal).data('url-consultar') + '/' + id
          , buttons: [{
              id: '#cerrar'
                          , class: utilSAI.buttonDangerClass
                          , isCloseButton: true
          }
          ]
            , callbackOnShow: function () { asociaEventosShowModalConsultarRegistro(); }
        });
    }
    , EliminarRegistro = function (id) {
        var tablaPrinc = $(idTablaPrincipal); 
        utilSAI.confirm(tablaPrinc.data('mensaje-eliminar')
            , function () {
                    utilSAI.executeActionCall({
                         url: tablaPrinc.data('url-eliminar') + '/' + id
                        , mostrarMensajeCargando: true
                        , callback: function (data) { recargarTablaPrincipal();}
                    });
                }
            );
        }

    ,//Crea un nuevo registro en la tabla
    CrearRegistro = function () {

        utilSAI.openModal({
            idTitulo: '#wizadCrearRegistro'
              , url: $(idTablaPrincipal).data('url-crear')
              , buttons: [
                  {
                      id: '#btnAnterior'
                              , class: utilSAI.buttonPreviousClass
                              , callback: function (e) {
                                  $('#btnAnterior').trigger('click');
                                  return false;
                              }
                  }

                  , {
                      id: '#btnSiguiente'
                              , class: utilSAI.buttonNextClass
                              , callback: function (e) {
                                  $('#btnSiguiente').trigger('click');
                                  return false;
                              }
                  }
                         , {
                             id: '#cerrar'
                              , class: utilSAI.buttonDangerClass
                              , isCloseButton: true
                         }
              ]
              , callbackOnShow: function () { asociaEventosShowModalCrearRegistro(); }
        });
    }
    ,  //Función para editar un usuario
    EditarRegistro = function(id){      
        utilSAI.openModal({
            idTitulo   : '#divTabEditarUsuario'
              ,url        : $(idTablaPrincipal).data('url-editar') + '/' + id
              ,buttons    : [{ id           : '#grabar'
                              ,class        : utilSAI.buttonSuccessClass 
                              , callback    : function () {
                                  $('#grabar').trigger('click');
                                  return false;
                              }
                            }                         
                             , {
                                 id            : '#cerrar'
                                  , class         : utilSAI.buttonDangerClass
                                  , isCloseButton : true
                                }]
              , callbackOnShow: function () { asociaEventosShowModalEditarRegistro(); }              
        });
    }
    //Eventos luego de mostrado el modal de consultar registro
    , asociaEventosShowModalConsultarRegistro = function () {
        "use strict";
        let dispositivo = $('.dispositivo');
        //initNesteables();
        initAllDefault();

        utilSAI.disableBootstrapDualListbox({
            element: dispositivo
                , disable: true
        });

        $("#ddAplicaciones").find('.checkInActiva').attr("disabled", "disabled");
    }

    //Eventos luego de mostrado el modal de crear registro
    , asociaEventosShowModalCrearRegistro = function () {

        var formulario = $(idFormularioCrearRegistro);

        utilSAI.resetValidations(formulario);
        initAllDefault();
        asociaEventoBuscarUsuario(formulario);

        //Asociar eventos ajax al formulario                   
        $.validator.unobtrusive.parse(idFormularioCrearRegistro);

        $('#wizadCrearRegistro')
            .ace_wizard({}).on('actionclicked.fu.wizard', function (e) {
                if (!formulario.valid()) e.preventDefault();
            }).on('finished.fu.wizard', function (e) {
                formulario.submit();
            });
    }
    //busca la información del usuario 
    , asociaEventoBuscarUsuario = function (form) {
        //Tipo de documento    
        form.find('#TipoDocumentoSeleccionado').on('change', function () {
            buscaDatosUsuario(form);
        });

        //Número de Documento
        form.find('#NumeroDocumento').on('blur', function () {
            buscaDatosUsuario(form);
        });
    }
    , asociaEventosShowModalEditarRegistro = function () {

        var formulario = $(idFormularioEditarRegistro);

            utilSAI.resetValidations(formulario);
            initAllDefault();
            bloqueCamposEditar(formulario);
    }
    , //Se buscan los datos del usuario
    buscaDatosUsuario = function (form) {
        var tipoDocumento = form.find('#TipoDocumentoSeleccionado');
        var numeroDocumento = form.find('#NumeroDocumento');

        if (tipoDocumento.val().trim() != '' && numeroDocumento.val().trim() != '') {
            //Se realiza la búsqueda de los datos del usuario
            var options = {
                url: form.find('#divContenedorFormulario').data('url-consultar-usuario')
                , dataType: "json"
                , mostrarMensajeCargando: true
               , parametros: {
                   TipoDocumentoSeleccionado: tipoDocumento.val()
                  , NumeroDocumento: numeroDocumento.val()
               }
               , callback: function (data) {
                   if (data && data.content) {
                      cargaAplicaciones(form,data, data.content.Id);
                   }
               }
            }

            utilSAI.executeActionCall(options);

        }
    }
    //Recarga la tabla principal
    , recargarTablaPrincipal = function () {
        $(idTablaPrincipal).DataTable().ajax.reload();
    }

    //Previene submit ocn enter
    , eventoPrevieneSubmitEnter = function () {
        $('body').bind('keypress', 'form', function (e) {

            var keyCode = e.keyCode || e.which;
            if (keyCode == 13)
                return false;

        });
    }
    //Función para cargar las aplicaciones de usuario
    ,cargaAplicaciones = function(form,data, id) {

        var options = {
            url: form.find('#divContenedorFormulario').data('url-cargar-aplicaciones') + '/' + id
            , destino: '#divBloqueAplicaciones'
            , callback: function () {
                if (data && data.content) {
                   
                    form.find('#Id').val(data.content.Id);
                    form.find('#ContrasenaNueva').val(data.content.ContrasenaNueva);
                    form.find('#RepetirContrasena').val(data.content.RepetirContrasena);
                    form.find('#NombreApellido').val(data.content.NombreApellido);
                    form.find('#NombreUsuario').val(data.content.NombreUsuario);
                    form.find('#FechaNacimiento').val(data.content.FechaNacimiento);
                    form.find('#TelefonoFijo').val(data.content.TelefonoFijo);
                    form.find('#TelefonoMovil').val(data.content.TelefonoMovil);
                    form.find('#Email').val(data.content.Email);
                    form.find('#IndicadorActivo').val(data.content.IndicadorActivo);

                    if (data.content.Id && data.content.Id != '0') {
                        //ocultar los datos de contraseña
                        form.find('.bloqueContrasena').hide();
                        if (data.content.IndicadorRegistroNoEditable == true) {
                            form.find(camposBloqueo).prop("readonly", true);
                            colocaAplicacionesDisabled(true);
                        } else {
                            form.find(camposBloqueo).prop("readonly", false);
                            colocaAplicacionesDisabled(false);
                        }
                    } else {
                        //mostrar los datos de contraseña
                        form.find('.bloqueContrasena').show();
                        form.find(camposBloqueo).prop("readonly", false);
                        colocaAplicacionesDisabled(false);
                    }
                }
            }
        }
        utilSAI.loadSectionAjax(options);
    }

    ,//función para colocar no ediatable los campos de edición
    bloqueCamposEditar = function (form) {
        var indicadorEditable = form.find('#IndicadorRegistroNoEditable');
            if (indicadorEditable.val().toLowerCase() == 'true') {
                form.find(camposBloqueo).prop("readonly", true);
                colocaAplicacionesDisabled(true);
            } 
        
    }
    //función para colocar readonly los checks y aún así poder hacer post
    , colocaAplicacionesDisabled = function (IndicadorDisabled) {
        var  ddAplicaciones = $("#ddAplicaciones")
            , checksAplicacion = ddAplicaciones.find('.checkInActiva');

        checksAplicacion.each(function (index, el) {
            var newID = el.id.replace('__', '].').replace('_', '[');            
            $("input[name='" + newID + "']").get(1).value = el.checked;
        });
        
        IndicadorDisabled ? checksAplicacion.attr("disabled", "disabled")
                          : checksAplicacion.removeAttr("disabled");
    }
        //Función para hacer el envío de correo de registro al socio comercial
    // options = {
    //              IndicadorReenvio: true // Permite saber si se trata de un reenvio de correo
    //}
    ,enviaCorreoRegistro = function(idCliente) {
        var tablaPrincipal = $(idTablaPrincipal);

        utilSAI.executeActionCall({
            url: tablaPrincipal.data('url-enviar-correo') + '/' + idCliente
                        , mostrarMensajeCargando: true
                        , mensajeAlterno: tablaPrincipal.data('etiqueta-enviando-correo')
        }).then(function () {
            bootbox.hideAll();
        });
    }

    ;


    //Funciones ejecutadas al cargar la página
    $(document).ready(function () {
        initTablaPrincipal();
        initAllDefault();
        asociaEventos();
    });


    return {
            CrearRegistro       : CrearRegistro
        ,   ConsultarRegistro   : ConsultarRegistro
        ,   EliminarRegistro    : EliminarRegistro
        ,   submitSuccess: function (data) {

            var content = data.content ? JSON.parse(data.content) : {};

            if (content.mensajeAdvertencia && content.mensajeAdvertencia != '') {
                utilSAI.notifyWarning(content.mensajeAdvertencia);
            }

            if (data.success) {
                utilSAI.notifySuccess(data.mensaje);
                recargarTablaPrincipal();
                bootbox.hideAll();
                if (content.IN_ENVIA_CORREO)
                    enviaCorreoRegistro(content.ID_RESPUESTA);
            } else {
                $('#resultUpdate').html(data.mensajeHtml);
            }
        }
    };

})(window);