(function (window, undefined) {
    var proveedorSAI = function () { }

    var idTablaPrincipal = '#tablaPrincipalAdministracion';
    var idFormularioCrearRegistro = '#formCrearRegistro';
    var idFormularioEditarRegistro = '#formEditarRegistro';

    var proveedorSAI = function () { }

    $(document).ready(function () {

        initTablaPrincipal();
        initAllDefault();
        asociaEventos();

    });

    //Inicializa la tabla de datatables
    function initTablaPrincipal() {
        utilSAI.initTablaPrincipal({
              idTabla: idTablaPrincipal
            , parametrosBusqueda: {}
            , serverSide: false
        });
    }


    //Inicializa las cosas que deban inicializarse en todal la página
    function initAllDefault() {
        utilSAI.setRightScroll();
        utilSAI.initSelect2();
        utilSAI.initDate();
    }

    //Asocia los eventos necesarios a la página
    function asociaEventos() {
        eventoConsultarRegistro();
        eventoCrearRegistro();
        eventoEditarRegistro();
    }

    //Evento para crear un nuevo registro
    function eventoCrearRegistro() {
        $('.icono-crear-registro').on('click', function () {
            //Se abre la ventana
            proveedorSAI.CrearRegistro();
        });
    }


    //Evento para crear un nuevo registro
    function eventoEditarRegistro() {
        $(idTablaPrincipal).on('click', '.icono-editar', function () {
            var icono = $(this);
            //Se abre la ventana
            proveedorSAI.EditarRegistro(icono.data('id-proveedor'));
        });
    }


    //Función para editar un usuario
    proveedorSAI.EditarRegistro = function (id) {
        utilSAI.openModal({
            idTitulo: '#divEditarRegistro'
              , url: $(idTablaPrincipal).data('url-editar') + '/' + id
              , buttons: [{
                  id: '#btnGuardar'
                              , class: utilSAI.buttonSuccessClass
                              , callback: function () {
                                  $('#btnGuardar').trigger('click');
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



    //Evento para consultar un registro
    function eventoConsultarRegistro() {

        $(idTablaPrincipal).on('click', '.icono-consultar', function () {
            var icono = $(this);
            //Se abre la ventana
            proveedorSAI.ConsultarRegistro(icono.data('id-proveedor'));
        });
    }

    //Función para consultar un usuario
    proveedorSAI.ConsultarRegistro = function (id) {

        utilSAI.openModal({
            idTitulo: '#divTabConsultarSocioComercial'
      , url: $(idTablaPrincipal).data('url-consultar') + '/' + id
      , buttons: [{
          id: '#cerrar'
                      , class: utilSAI.buttonDangerClass
                      , isCloseButton: true
      }
      ]
        , callbackOnShow: function () { }
        });
    }



    //Crea un nuevo registro en la tabla
    proveedorSAI.CrearRegistro = function () {
        utilSAI.openModal({
            idTitulo: '#divCrearRegistro'
               , url: $(idTablaPrincipal).data('url-crear')
               , buttons: [
                   {
                       id: '#btnGuardar'
                               , class: utilSAI.buttonSuccessClass
                               , callback: function (e) {
                                   $('#btnGuardar').trigger('click');
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
    function asociaEventosShowModalCrearRegistro() {

        var formulario = $(idFormularioCrearRegistro);

        asociaEventosNuevoModal(formulario);

        //Asociar eventos ajax al formulario                   
        $.validator.unobtrusive.parse(idFormularioCrearRegistro);


    }

    //Eventos en la página de edición de registro
    function asociaEventosShowModalEditarRegistro() {
        var formulario = $(idFormularioEditarRegistro);

        asociaEventosNuevoModal(formulario);

        //Asociar eventos ajax al formulario                   
        $.validator.unobtrusive.parse(idFormularioEditarRegistro);


    }



    //Asocia Eventos Crear Socio Comercial
    function asociaEventosNuevoModal(form) {
        utilSAI.resetValidations(form);
        initAllDefault();
        inicializaSelectDireccion(form);
    }


    //Inicializa select dirección
    function inicializaSelectDireccion(form) {
        //Pais
        utilSAI.setSelectAjax(form,
            {
                changeClass: '.pais'
                    , destinyClass: '.provincia'
                    , parameters: function () {
                        return {
                            pais: $('.pais').val()
                        };
                    }
            }
        );

        //Provincia
        utilSAI.setSelectAjax(form,
            {
                changeClass: '.provincia'
                , destinyClass: '.distrito'
                , parameters: function () {
                    return {
                        pais: $('.pais').val()
                        , provincia: $('.provincia').val()
                    };
                }
            }
        );


        //Distrito
        utilSAI.setSelectAjax(form,
            {
                changeClass: '.distrito'
                , destinyClass: '.corregimiento'
                , parameters: function () {
                    return {
                        pais: $('.pais').val()
                        , provincia: $('.provincia').val()
                        , distrito: $('.distrito').val()
                    };
                }
            }
        );
    }

    //Graba registros
    proveedorSAI.submitSuccess = function (data) {
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
    function recargarTablaPrincipal() {
        $(idTablaPrincipal).DataTable().ajax.reload();
    }



    window.proveedorSAI = proveedorSAI;

})(window);