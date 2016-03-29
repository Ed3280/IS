var usuarioAdminSAI = (function (window) {

    var idTablaPrincipal = '#tablaPrincipalAdministracion'
        , idFormularioCrearRegistro = '#formCrearRegistro'
        , idFormularioEditarRegistro = '#formEditarRegistro'
        , idFormularioBaja = "#formBajaRegistro"

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

    //Evento para consultar un registro
    , eventoConsultarRegistro = function () {

        $(idTablaPrincipal).on('click', '.icono-consultar', function () {
            var icono = $(this);
            //Se abre la ventana
            ConsultarRegistro(icono.data('id-usuario'));
        });
    }
    //Evento para crear un nuevo registro
    , eventoCrearRegistro = function () {
        $('.icono-crear-registro').on('click', function () {
            //Se abre la ventana
            CrearRegistro();
        });
    }
    //Evento para ediatr un registro existente
    , eventoEditarRegistro = function () {
        $(idTablaPrincipal).on('click', '.icono-editar', function () {
            var icono = $(this);
            //Se abre la ventana
            EditarRegistro(icono.data('id-usuario'));
        });
    }

    //Función para consultar un usuario
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

    //Función para editar un usuario
    , EditarRegistro = function (id) {
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
                             id: '#cerrar'
                              , class: utilSAI.buttonDangerClass
                              , isCloseButton: true
                         }
              ]
              , callbackOnShow: function () { asociaEventosShowModalEditarRegistro(); }
        });
    }

    //Evento para cuando se indica si el usuario será empleado 
    , eventoCargaDatoEmpleado = function (form) {
        form.find('#IndicadorEmpleado').on('click', function () {
            //Se carga la data del empleado
            if (this.checked)
                cargaDatosEmpleado(this, form);
            else
                $('#divDatosEmpleado').html('');

            cargaRolesUsuario();
        });
    }

    //Asocia Eventos Crear Empleado
    , asociaEventosCrearEmpleado = function (form) {
        utilSAI.resetValidations(form);
        initAllDefault();
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
    //Eventos luego de mostrado el modal de crear registro
    , asociaEventosShowModalCrearRegistro = function () {

        var formulario = $(idFormularioCrearRegistro);

            initAllDefault();
            //Asociar eventos ajax al formulario                   
            $.validator.unobtrusive.parse(idFormularioCrearRegistro);
            mostrarMenu();

            formulario.find("#RolesSeleccionados").on('change', function () {
                mostrarMenu();
            });

            $('#wizadCrearRegistro')
            .ace_wizard({}).on('actionclicked.fu.wizard', function (e) {
                if (!$(idFormularioCrearRegistro).valid()) e.preventDefault();
            }).on('finished.fu.wizard', function (e) {
                $(idFormularioCrearRegistro).submit();
            });
    }
    //Eventos luego de mostrado el modal de consultar registro
    , asociaEventosShowModalConsultarRegistro = function () {
        initAllDefault();

        utilSAI.disableBootstrapDualListbox({
            element: $('.roles')
                , disable: true
        });

        $("#ddAplicaciones").find('.checkInActiva').attr("disabled", "disabled");
        mostrarMenu();
    }

    //Eventos luego de mostrado el modal de editar registro
    , asociaEventosShowModalEditarRegistro = function () {

        var formulario = $(idFormularioEditarRegistro);
            initAllDefault();
            mostrarMenu();
            
            formulario.find("#RolesSeleccionados").on('change', function () {
                mostrarMenu();
            });

            //Asociar eventos ajax al formulario                   
            $.validator.unobtrusive.parse(idFormularioEditarRegistro);

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
         mostrarApp = function (idRol, aplicaciones) {
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
        ,
    //Graba registros
        submitSuccess = function (data) {
            var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
            if (content.mensajeAdvertencia != null && typeof content.mensajeAdvertencia != 'undefined' && content.mensajeAdvertencia != '')
                utilSAI.notifyWarning(content.mensajeAdvertencia);

            if (data.success) {
                utilSAI.notifySuccess(data.mensaje);
                recargarTablaPrincipal();
                bootbox.hideAll();
            } else {
                $('#resultUpdate').html(data.mensajeHtml);
            }
        }

    //Recarga la tabla principal
    , recargarTablaPrincipal = function () {
        $(idTablaPrincipal).DataTable().ajax.reload();
    };

    $(document).ready(function () {

        initTablaPrincipal();
        initAllDefault();
        asociaEventos();
    });
    return {
          ConsultarRegistro : ConsultarRegistro
        , EditarRegistro    : EditarRegistro
        , CrearRegistro     : CrearRegistro
        , submitSuccess     : submitSuccess
    };

})(window);