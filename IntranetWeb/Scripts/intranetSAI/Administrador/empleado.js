var empleadoSAI = (
    function (window) {

        var idTablaPrincipal = '#tablaPrincipalAdministracion'
            , idFormularioCrearRegistro = '#formCrearRegistro'
            , idFormularioEditarRegistro = '#formEditarRegistro'
            , idFormularioBaja           = "#formBajaRegistro"
        //Inicializa la tabla de datatables
           , initTablaPrincipal = function () {
               utilSAI.initTablaPrincipal({
                   idTabla: idTablaPrincipal
                   , parametrosBusqueda: {}
                   , serverSide: true
               });
           }
        //Inicializa las cosas que deban inicializarse en todal la página
        , initAllDefault = function () {
            utilSAI.setRightScroll();
            utilSAI.initSelect2();
            utilSAI.initDate();
            utilSAI.initBootstrapDualListbox();
        }
        //Asocia los eventos necesarios a la página
        , asociaEventos = function () {
            eventoConsultarRegistro();
            eventoCrearRegistro();
            eventoEditarRegistro();           
        }

        , eventoConsultarRegistro = function () {
            $(idTablaPrincipal).on('click', '.icono-consultar', function () {
                var icono = $(this);
                //Se abre la ventana
                ConsultarRegistro(icono.data('id-usuario'));
            });
        }
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
        , asociaEventosShowModalConsultarRegistro = function () {
            "use strict";
            let roles = $('.roles');

            initAllDefault();
            mostrarMenu();
            utilSAI.disableBootstrapDualListbox({
                element: roles
                    , disable: true
            });

            $("#ddAplicaciones").find('.checkInActiva').attr("disabled", "disabled");
        }
        ,eventoEditarRegistro = function() {
            $(idTablaPrincipal).on('click', '.icono-editar', function () {
                var icono = $(this);
                //Se abre la ventana
                EditarRegistro(icono.data('id-usuario'));
            });
        }
        ,  //Función para editar un usuario
        EditarRegistro = function (id) {
        utilSAI.openModal({
            idTitulo: '#divTabEditarUsuario'
              , url: $(idTablaPrincipal).data('url-editar') + '/' + id
              , buttons: [{
                  id: '#grabar'
                              , class: utilSAI.buttonSuccessClass
                              , callback: function () {
                                  $('#grabar').trigger('click');
                                  return false;
                              }
              }
                         , {
                             id: '#darBaja'
                              , class: utilSAI.buttonWarningClass
                              , callback: function () {
                                  bootbox.hideAll();
                                  DarBajaEmpleado(id);
                              }
                         }
                         , {
                             id: '#cerrar'
                              , class: utilSAI.buttonDangerClass
                              , isCloseButton: true
                         }
              ]
              , callbackOnShow: function () { asociaEventosShowModalEditarRegistro(); }
        });
        }
        //Dar de baja a un empleado
       ,DarBajaEmpleado = function (id) {

            utilSAI.openModal({
                idTitulo: '#divTabEditarUsuario'
                  , url: $('#darBaja').data('url-baja') + '/' + id
                  , buttons: [{
                      id: '#grabar'
                                  , class: utilSAI.buttonSuccessClass
                                  , callback: function (e) {
                                      if (!$(idFormularioBaja).valid())
                                          e.preventDefault();
                                      else
                                          utilSAI.confirm($("#divTabEditarUsuario").data("confirm-baja"), function () { $('#grabar').trigger('click'); });

                                      return false;
                                  }
                  }
                             , {
                                 id: '#cerrar'
                                  , class: utilSAI.buttonDangerClass
                                  , isCloseButton: true
                             }
                  ]
                  , callbackOnShow: function () { asociaEventosShowModalDarBajaEmpleado(); }
            });
       }
        //Eventos cuando se da de baja a un empleado
        ,asociaEventosShowModalDarBajaEmpleado = function () {
            //var formulario = $(idFormularioBaja);
            initAllDefault();
            $.validator.unobtrusive.parse(idFormularioBaja);
        }
        //Eventos luego de mostrado el modal de editar registro
        ,asociaEventosShowModalEditarRegistro = function() {

            var formulario = $(idFormularioEditarRegistro);
                initAllDefault();
                utilSAI.resetValidations(formulario);     
                cargaSelectEmpleado(formulario);
                
                formulario.find("#RolesSeleccionados").on('change', function () {
                    mostrarMenu();
                });
                
                mostrarMenu();
                //Asociar eventos ajax al formulario                   
                $.validator.unobtrusive.parse(idFormularioEditarRegistro);

        }

        //Evento para crear un nuevo registro
        , eventoCrearRegistro = function () {
            $('#divHeaderTablaPrincipal').on('click', '.icono-crear-registro', function () {
                //Se abre la ventana
                CrearRegistro();
            });
        }
        //Crea un nuevo registro en la tabla
        , CrearRegistro = function () {

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
        ,
        //Eventos luego de mostrado el modal de crear registro
        asociaEventosShowModalCrearRegistro = function () {

            var formulario = $(idFormularioCrearRegistro);

                utilSAI.resetValidations(formulario);
                initAllDefault();
                asociaEventoBuscarUsuario(formulario);
                cargaSelectEmpleado(formulario);

                formulario.find("#RolesSeleccionados").on('change', function () {
                    mostrarMenu();
                });
           
                //Asociar eventos ajax al formulario                   
                $.validator.unobtrusive.parse(idFormularioCrearRegistro);

                $('#wizadCrearRegistro')
                    .ace_wizard({}).on('actionclicked.fu.wizard', function (e) {
                        if (!formulario.valid()) e.preventDefault();
                    }).on('finished.fu.wizard', function (e) {
                        formulario.submit();
                    });
        }
        , cargaSelectEmpleado = function (form) {
            //Unidad Administrativa
            utilSAI.setSelectAjax(form,
                {
                    changeClass: '.unidadAdministrativa'
                        , destinyClass: '.cargo'
                        , parameters: function () {
                            return {
                                cdUnidadAdministrativa: $('.unidadAdministrativa').val()
                            };
                        }
                }
            );

            //Cargo
            utilSAI.setSelectAjax(form,
                    {
                        changeClass: '.cargo'
                        , destinyClass: '.supervisor'
                        , parameters: function () {
                            return {
                                cdCargo: $('.cargo').val()
                                    , cdUsuarioModif: $('#Id').val()
                            };
                        }
                    }
                );

            utilSAI.setSelectDualListboxAjax(form,
               {
                   changeClass: '.cargo'
                       , destinyClass: '.roles'
                       , url: form.find('.cargo').data('url-roles')
                       , parameters: function () {
                           return {
                               cdUsuario: form.find('#Id').val()
                               , cdCargo: form.find('#CargoSeleccionado').val()
                           };
                       }
                   , callback: function () { mostrarMenu();}
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
     //Se buscan los datos del usuario
    ,buscaDatosUsuario = function (form) {
        var tipoDocumento = form.find('#TipoDocumentoSeleccionado');
        var numeroDocumento = form.find('#NumeroDocumento');

        if (tipoDocumento.val().trim() != '' && numeroDocumento.val().trim() != '') {
            //Se realiza la búsqueda de los datos del usuario
            var options = {
                url: form.find('#divContenedorFormulario').data('url-consultar-usuario')
                , dataType: "json"
                , mostrarMensajeCargando: true
               , parametros: {
                    TipoDocumentoSeleccionado    : tipoDocumento.val()
                  , NumeroDocumento              : numeroDocumento.val()
               }
                , callback: function (data) {
                   
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
                            form.find('.bloqueContrasena').hide();                            
                        } else {                            
                            form.find('.bloqueContrasena').show();                            
                        }
                    }
                }
            }
            utilSAI.executeActionCall(options);
        }
    }

        //Mostrar menú de aplicaciones
        , mostrarMenu = function () {
            var ddAplicaciones = $('#ddAplicaciones');
            var divAdvertenciaMenu = $("#divAdvertenciaMenu");

                ddAplicaciones.find('.dd-item').hide();
                divAdvertenciaMenu.removeClass('hidden');

            var aplicaciones = ddAplicaciones.nestable().nestable('serialize');
            
            $("#RolesSeleccionados :selected").each(function () {
                mostrarApp($(this).val(), aplicaciones);                
            });

        }
        ,//Muestra las aplicaciones que deban poderse asignar
         mostrarApp = function(idRol, aplicaciones) {
            var inMostrado = false;
            var divAdvertenciaMenu = $("#divAdvertenciaMenu");

            for (var key in aplicaciones) {
                var rolAplicacion = aplicaciones[key].roles;
                var rolesAppArr = rolAplicacion.toString().split('#');

                if ($.inArray(idRol + "", rolesAppArr) != -1) {
                    $('#ddAplicaciones').find(".dd-item[data-id='" + aplicaciones[key].id + "']").show();

                    if (!inMostrado) { divAdvertenciaMenu.addClass('hidden'); inMostrado = true; }

                    if (typeof aplicaciones[key].children != 'undefined' && aplicaciones[key].children != null)
                        mostrarApp(idRol, aplicaciones[key].children);
                }            
            }
         }

        ,//Recarga la tabla principal
        recargarTablaPrincipal = function () {
            $(idTablaPrincipal).DataTable().ajax.reload();
        }
             //Función para hacer el envío de correo de registro al socio comercial
            // options = {
            //              IndicadorReenvio: true // Permite saber si se trata de un reenvio de correo
            //}
            , enviaCorreoRegistro = function (idCliente) {
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
                       
            
        $(document).ready(function () {
            initTablaPrincipal();
            initAllDefault();
            asociaEventos();
        });

        return {
              ConsultarRegistro : ConsultarRegistro
            , CrearRegistro     : CrearRegistro
            , EditarRegistro    : EditarRegistro
            , DarBajaEmpleado   : DarBajaEmpleado
            ,  //Graba registros
            submitSuccess : function (data) {
                var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
                if (content&& content.mensajeAdvertencia && content.mensajeAdvertencia != '')
                    utilSAI.notifyWarning(content.mensajeAdvertencia);

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

    }
)(window);