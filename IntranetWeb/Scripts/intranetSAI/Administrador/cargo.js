(function (window, undefined) {

    var cargoSAI = function () { }
    var idWidgetPrincipal           = "#widgetCargos";
    var idFormularioCrearRegistro   = "#formCrearRegistro";
    var idFormularioEditarRegistro  = "#formEditarRegistro";
    

    $(document).ready(function () {

        initAllDefault();
        asociaEventos();
    });


    //Inicializa las cosas que deban inicializarse en todal la página
    function initAllDefault() {
        utilSAI.initSelect2();
        utilSAI.initDate();
        utilSAI.initBootstrapDualListbox();
        initNestable();
        utilSAI.setRightScroll();        
    }

    function initNestable() {
        $('#ddCargos').nestable();
    }
    
    //Asocia los eventos a la página de creación de cargos
    function asociaEventos() {
        //Crear registro
        $(idWidgetPrincipal).on('click', '.icono-crear-registro', function () {
            cargoSAI.CrearRegistro();
        });

        //Editar registro
        $(idWidgetPrincipal).on('click', '.icono-editar', function () {
            cargoSAI.EditarRegistro($(this).data('id'));
        });

        //Eliminar registro
        $(idWidgetPrincipal).on('click', '.icono-eliminar', function () {
            cargoSAI.EliminarRegistro($(this).data('id'));
        });

    }


    //Función para crear un nuevo registro
    cargoSAI.CrearRegistro = function () {
        utilSAI.openModal({
            idTitulo: '#wizadCrearRegistro'
              , url: $(idWidgetPrincipal).data('url-crear')
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


    //Función para crear un nuevo registro
    cargoSAI.EditarRegistro = function (id) {
        utilSAI.openModal({
            idTitulo: '#divTabEditarCargo'
              , url: $(idWidgetPrincipal).data('url-editar')+'/'+id
              , buttons: [
                            {
                                id: '#grabar'
                              , class: utilSAI.buttonSuccessClass
                              , callback: function (e) {
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

    //Eliminar registro de cargo
    cargoSAI.EliminarRegistro = function (id) {
        utilSAI.confirm( $(idWidgetPrincipal).data('confirm-eliminar'), function () {

            utilSAI.executeActionCall({
                 url: $(idWidgetPrincipal).data('url-eliminar') + '/' + id
                , callback: function () {
                    cargoSAI.RecargarWidgetPrincipal();
                }
            });
        });
    }


    //Recarga el listado de cargos 
    cargoSAI.RecargarWidgetPrincipal = function () {
        utilSAI.loadSectionAjax({
              url: $(idWidgetPrincipal).data('url')
            , destino: '#divWigetBodyCargos'
            , callback: function () { initAllDefault(); }
        });
    }



    //Eventos luego de mostrado el modal de crear registro
    function asociaEventosShowModalCrearRegistro() {
        initAllDefault();
        var formulario = $(idFormularioCrearRegistro);

        
        //Asociar eventos ajax al formulario                   
        $.validator.unobtrusive.parse(idFormularioCrearRegistro);

        $('#wizadCrearRegistro')
            .ace_wizard({}).on('actionclicked.fu.wizard', function (e) {
                if (!$(idFormularioCrearRegistro).valid()) e.preventDefault();
            }).on('finished.fu.wizard', function (e) {
                $(idFormularioCrearRegistro).submit();
            });
    }


    //Eventos luego de mostrado el modal de editar registro
    function asociaEventosShowModalEditarRegistro() {

        var formulario = $(idFormularioEditarRegistro);

        
        initAllDefault();
        //Asociar eventos ajax al formulario                   
        $.validator.unobtrusive.parse(idFormularioEditarRegistro);

    }



    //Graba registros
    cargoSAI.submitSuccess = function (data) {
        var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
        if (content.mensajeAdvertencia != null && typeof content.mensajeAdvertencia != 'undefined' && content.mensajeAdvertencia != '')
            utilSAI.notifyWarning(content.mensajeAdvertencia);

        if (data.success) {
            utilSAI.notifySuccess(data.mensaje);
            cargoSAI.RecargarWidgetPrincipal();
            bootbox.hideAll();
        } else {
            $('#resultUpdate').html(data.mensajeHtml);
        }
    }


    window.cargoSAI = cargoSAI;

})(window);