(function (window, undefined) {
    var socioComercialSAI = function () { }

    var idTablaPrincipal            = '#tablaPrincipalAdministracion';
    var idFormularioCrearRegistro   = '#formCrearRegistro';
    var idFormularioEditarRegistro  = '#formEditarRegistro';

    var socioComercialSAI = function () { }

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
            , serverSide: true
        });
    }


    //Inicializa las cosas que deban inicializarse en todal la página
    function initAllDefault() {
        utilSAI.setRightScroll();
        utilSAI.initSelect2();
        utilSAI.initDate();
        utilSAI.initBootstrapDualListbox();
    }

    //Asocia los eventos necesarios a la página
    function asociaEventos() {
        eventoConsultarRegistro();
        eventoCrearRegistro();
        eventoEditarRegistro();
        eventoReenvioCorreo();
    }

    //Evento para crear un nuevo registro
    function eventoCrearRegistro() {
        $('.icono-crear-registro').on('click', function () {
            //Se abre la ventana
            socioComercialSAI.CrearRegistro();
        });
    }


    //Evento para crear un nuevo registro
    function eventoEditarRegistro() {
        $(idTablaPrincipal).on('click', '.icono-editar', function () {
            var icono = $(this);
            //Se abre la ventana
            socioComercialSAI.EditarRegistro(icono.data('id-socio'));
        });
    }


    //Función para editar un usuario
    socioComercialSAI.EditarRegistro = function (id) {
        utilSAI.openModal({
            idTitulo: '#divTabEditarSocioComercial'
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
            socioComercialSAI.ConsultarRegistro(icono.data('id-socio'));
        });
    }

    //Función para consultar un usuario
    socioComercialSAI.ConsultarRegistro = function (id) {

        utilSAI.openModal({
            idTitulo: '#divTabConsultarSocioComercial'
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



    //Crea un nuevo registro en la tabla
    socioComercialSAI.CrearRegistro = function () {
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

    //Evento para el reenvio de correo
    function eventoReenvioCorreo() {
        $('body').on('click','.icono-reenviar-correo-registro', function(){
            var iconoReenviar = $(this);
            var tablaPrincipal = $(idTablaPrincipal);
            utilSAI.confirm(tablaPrincipal.data('mensaje-advertencia-envio-correo')
            , function () { enviaCorreoRegistro(iconoReenviar.data('id-socio')); });
                
        });
    }


    //Eventos luego de mostrado el modal de crear registro
    function asociaEventosShowModalCrearRegistro() {
        
        var formulario = $(idFormularioCrearRegistro);

        asociaEventosNuevoModal(formulario);
        asociaEventoBuscarUsuario(formulario);

        //Asociar eventos ajax al formulario                   
        $.validator.unobtrusive.parse(idFormularioCrearRegistro);

        $('#wizadCrearRegistro')
           .ace_wizard({}).on('actionclicked.fu.wizard', function (e) {
             if (!$(idFormularioCrearRegistro).valid()) e.preventDefault();
           }).on('finished.fu.wizard', function (e) {
               $(idFormularioCrearRegistro).submit();
           });        
    }


    //Eventos en la página de edición de registro
    function asociaEventosShowModalEditarRegistro() {
        var formulario = $(idFormularioEditarRegistro);

        asociaEventosNuevoModal(formulario);

        var dispositivos = formulario.find('.dispositivo');

        if (formulario.find('#DistribuidorSeleccionado').val() != null && formulario.find('#DistribuidorSeleccionado').val() != 0) {
            
            utilSAI.disableBootstrapDualListbox({
                element: dispositivos
                , disable: true
            });

        } else {
            utilSAI.disableBootstrapDualListbox({
                element: dispositivos
               , disable: false
            });
        }

        dispositivos.bootstrapDualListbox('refresh', true);

        //Asociar eventos ajax al formulario                   
        $.validator.unobtrusive.parse(idFormularioEditarRegistro);


    }


    //Eventos en la página de consulta de registros 
    function asociaEventosShowModalConsultarRegistro() {
        initAllDefault();
        var divConsulta = $('#divTabConsultarSocioComercial');
        var dispositivos = divConsulta.find('.dispositivo');
        utilSAI.disableBootstrapDualListbox({
            element: dispositivos
                , disable: true
        });

        var serviciosAppMovil = divConsulta.find('.serviciosAppMovil');
        utilSAI.disableBootstrapDualListbox({
              element: serviciosAppMovil
            , disable: true
        });

        $("#ddAplicaciones").find('.checkInActiva').attr("disabled", "disabled");

    }


    //Asocia Eventos Crear Socio Comercial
    function asociaEventosNuevoModal(form) {
        utilSAI.resetValidations(form);
        initAllDefault();
        inicializaSelectSegmentoNegocio(form);
        inicializaSelectDistribuidor(form);
        inicializaSelectDireccion(form);       
    }


    //Inicializa select segmento de negocio
    function inicializaSelectSegmentoNegocio(form) {
        //Segemnto negocio
        utilSAI.setSelectDualListboxAjax(form,
            {
                changeClass: '.segmentoNegocio'
                    , destinyClass: '.serviciosAppMovil'
                    , parameters: function () {
                        return {
                            CodigoSegmentoNegocio: form.find('.segmentoNegocio').val()
                        };
                    }               
            }
        );
    }

    //Inicializa select distribuidor 
    function inicializaSelectDistribuidor(form) {
        //Segemnto negocio
        utilSAI.setSelectDualListboxAjax(form,
            {
                changeClass: '.distribuidor'
                    , destinyClass: '.dispositivo'
                    , parameters: function () {
                        return {
                            IdDistribuidor: form.find('.distribuidor').val()
                        };
                    }
                , callback: function () {
                    var dispositivos = form.find('.dispositivo');
                    if (form.find('.distribuidor').val() != null && form.find('.distribuidor').val() != 0) {
                        dispositivos.find('option').prop('selected', true);
                        utilSAI.disableBootstrapDualListbox({
                            element: dispositivos
                            , disable: true
                        });
                    }
                    else {
                        utilSAI.disableBootstrapDualListbox({
                            element: dispositivos
                           , disable: false
                        });
                    }
                    dispositivos.bootstrapDualListbox('refresh', true);
                }
            }
        );
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
                            pais: form.find('.pais').val()
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
                        pais: form.find('.pais').val()
                        , provincia: form.find('.provincia').val()
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
                        pais: form.find('.pais').val()
                        , provincia: form.find('.provincia').val()
                        , distrito: form.find('.distrito').val()
                    };
                }
            }
        );
    }

    //Graba registros
    socioComercialSAI.submitSuccess = function (data,indicadorCrear) {
        var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
        if (content.mensajeAdvertencia != null && typeof content.mensajeAdvertencia != 'undefined' && content.mensajeAdvertencia != '')
            utilSAI.notifyWarning(content.mensajeAdvertencia);

        if (data.success) {
            utilSAI.notifySuccess(data.mensaje);
            recargarTablaPrincipal();
            indicadorCrear && enviaCorreoRegistro(content.CD_SOCIO_COMERCIAL);
            bootbox.hideAll();
        } else {
            $('#resultUpdate').html(data.mensajeHtml);
        }
    }
 

    //Recarga la tabla principal
    function recargarTablaPrincipal() {
        $(idTablaPrincipal).DataTable().ajax.reload();
    }

    //busca la información del usuario 
    function asociaEventoBuscarUsuario(form) {
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
    function buscaDatosUsuario(form) {
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
                   if (data.content != null) {
                       
                       form.find('#Id').val(data.content.Id);
                       form.find('#Nombre').val(data.content.Nombre);
                       form.find('#FechaNacimiento').val(data.content.FechaNacimiento);
                       form.find('#TelefonoFijo').val(data.content.TelefonoFijo);
                       form.find('#TelefonoMovil').val(data.content.TelefonoMovil);
                       form.find('#Email').val(data.content.Email);

                       cargaAplicaciones(form, data.content.Id);

                   }
               }
            }

            utilSAI.executeActionCall(options);

        }
    }

    

    //Función para hacer el envío de correo de registro al socio comercial
    // options = {
    //              IndicadorReenvio: true // Permite saber si se trata de un reenvio de correo
    //}
    function enviaCorreoRegistro(idCliente) {
        var tablaPrincipal = $(idTablaPrincipal);

        utilSAI.executeActionCall({
            url: tablaPrincipal.data('url-enviar-correo-registro') + '/' + idCliente
                        , mostrarMensajeCargando: true
                        , mensajeAlterno: tablaPrincipal.data('etiqueta-enviando-correo')
        }).then(function () {
            bootbox.hideAll();
        });
    }



    //Función para cargar las aplicaciones de usuario
    function cargaAplicaciones(form, id) {
        
        var options = {
            url : form.find('#divContenedorFormulario').data('url-cargar-aplicaciones')+'/'+id
            ,destino    : '#divBloqueAplicaciones'
        }
       
        utilSAI.loadSectionAjax(options);
            
    }



    window.socioComercialSAI = socioComercialSAI;

})(window);