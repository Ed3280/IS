(function (window, undefined) {

    var  clienteSAI = function () { }
        ,idTablaCliente                      = '#dynamic-table-cliente'
        ,idFormularioCrearRegistro           = '#formCrearRegistro'
        ,idFormularioAsociarVehiculo         = '#formCrearVehiculoCliente'
        ,idFormularioAsociarDispositivo      = '#formCrearDispositivoCliente'
        ,idFomularioEditarDatosAdicionales   = '#formEditarDatosAdicionalesRegistro'

    $(document).ready(function () {
        //Inicializa la tabla de clientes
        initTablaClientes();
        asociaEventos();
    });

    //Inicializa la tabla de clientes
    function initTablaClientes() {      
        
        var tableCliente = utilSAI.initTablaPrincipal({
            idTabla: idTablaCliente
           , parametrosBusqueda: {}
           , serverSide: true
        });
             
        //Se inicializan los tab
        setScrollTabs();

        return tableCliente;
    }

    function asociaEventos() {
        asociaEventoCrearRegistro();
        asociaEventoCerrarBotonTab();
        asociaEventoEditarCliente();
        asociaEventoCreadoTabCliente();
        asociaEventoRepintar();
        asociaEventoReenviarCorreo();
    }
    
    //Asocia el evento de enviar el correo de registro
    function asociaEventoReenviarCorreo() {
        $('.tab-content').on('click', '.icono-reenviar-correo-registro', function () {            
            var idCliente = $(this).data('id-cliente');
            enviaCorreoRegistro(idCliente, function () { clienteSAI.agregarTabEditarCliente(idCliente, $('#liCliente_' + idCliente).find('a[data-toggle="tab"]').text()); }, { IndicadorReenvio: true });
        });        
    }

    
    //Evento para crear un nuevo cliente
    function asociaEventoCrearRegistro() {
        $('#divWidgetToolbarCrearRegistroCliente').on('click', '.icono-crear-registro', function () {
            //Se abre la ventana
            clienteSAI.CrearRegistro();
        });
    }


    //Función para crear a un nuevo cliente
    clienteSAI.CrearRegistro = function () {

        utilSAI.openModal({
            idTitulo: '#wizadCrearRegistro'
             , url: $(idTablaCliente).data('url-crear')
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
             , callbackOnShow: function () {
                 asociaEventosShowModalCrearRegistro();
             }
        });
    }


    //Función para crear a un nuevo vehículo
    clienteSAI.CrearRegistroVehiculo = function (form, idCliente) {
        
        utilSAI.openModal({
            idTitulo: '#divWidgetBoxAsociarVehiculo'
             , url: $(idTablaCliente).data('url-crear-vehiculo') + '?IdCliente=' + idCliente
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
             , callbackOnShow: function () {
                asociaEventosShowModalCrearRegistroVehiculo();
             }
        });
    }



    //Función para asociar los eventos a la ventana de creación de vehículos
    function asociaEventosShowModalCrearRegistroVehiculo() {
        initAllDefault();
        var form = $(idFormularioAsociarVehiculo);
        $.validator.unobtrusive.parse(idFormularioAsociarVehiculo);
        asociaEventoOnChangeSelectModal(form);
        asociaEventoTypeahead(form);
    }

    //Asocia los eventos type ahead
    function asociaEventoTypeahead(form){
        var options = {
              fieldClass  : '.vin_vehiculo'
            , parameters: function () { return { IdCliente: form.find('#IdCliente').val() } }
        };
        utilSAI.setTypeahead(form, options); 
    }

    //Función para asociar los eventos de la ventana de creación de dispositivos
    function asociaEventosShowModalCrearRegistroDispositivo() {
        initAllDefault();
        var form = $(idFormularioAsociarDispositivo);
        $.validator.unobtrusive.parse(form);
        asociaEventoBuscarDetalleDispostivo(form);
    }

    //Se busca la información del usuario y se carga a los datos del cliente
    function asociaEventoBuscarDetalleDispostivo(form) {
        //Dispositivo    
        form.find('#Id').on('change', function () {
            buscaDatosDispositivo(form);
        });       
    }

    //Se busca la información del dispostivo
    function buscaDatosDispositivo(form) {
        var idDispositivo = form.find('#Id');
        
        if (idDispositivo.val().trim() != '' && idDispositivo.val().trim() != '') {
            //Se realiza la búsqueda de los datos del usuario
            var options = {
                url: form.find('#divContenedorFormularioDispositivo').data('url-consultar-dispositivo')
                , dataType: "json"
                , mostrarMensajeCargando: true
               , parametros: {
                   Id: idDispositivo.val()                                                
               }
               , callback: function (data) {
                   if (data.content != null) {
                       form.find('#Imei').val(data.content.Imei);
                       form.find('#TarjetaSim').val(data.content.TarjetaSim);
                       form.find('#NumeroDVR').val(data.content.NumeroDVR);
                       form.find('#VehiculosSeleccionado').val(data.content.VehiculosSeleccionado == '0' ? '' : data.content.VehiculosSeleccionado); 
                       form.find('#VehiculosSeleccionado').trigger('change');
                       form.find('#KilometrajeInicial').val(data.content.KilometrajeInicial);
                       
                   }
               }
            }

            utilSAI.executeActionCall(options);
        }
    }



    //Función para crear a un nuevo dispositivo
    clienteSAI.CrearRegistroDispositivo = function (form, idCliente) {

        utilSAI.openModal({
            idTitulo: '#divWidgetBoxAsociarDispositivo'
             , url: $(idTablaCliente).data('url-crear-dispositivo') + '?IdCliente=' + idCliente
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
             , callbackOnShow: function () {
                 asociaEventosShowModalCrearRegistroDispositivo();
             }
        });
    }


    //Función para editar la información adicional de un cliente
    clienteSAI.EditarDatosAdicionales = function (form, idCliente) {

        utilSAI.openModal({
            idTitulo: '#divTabEditarDatosAdicionalesCliente'
             , url: $(idTablaCliente).data('url-editar-adicionales') + '/' + idCliente
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
             , callbackOnShow: function () {
                 asociaEventosShowModalEditarDatosAdicionales();
             }
        });
    }



    //Eventos luego de mostrado el modal de crear registro
    function asociaEventosShowModalCrearRegistro() {

        var formulario = $(idFormularioCrearRegistro);

        initAllDefault();
        asociaEventoOnChangeSelectDireccion(formulario);
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

    //Eventos luego de mostrado el modal de edición de datos adicionales 
    function asociaEventosShowModalEditarDatosAdicionales() {
        var formulario = $(idFomularioEditarDatosAdicionales);
        initAllDefault();
        $.validator.unobtrusive.parse(idFomularioEditarDatosAdicionales);
    }

    //Asocia el evento cerrar botón 
    function asociaEventoCerrarBotonTab() {
        $("#ulTabCliente").on("click", ".button-close-tab", function () {
            var idCliente = $(this).parent().parent().data('id-cliente');
            var mensajeCerrarPestana = $('.page-header').data('confirma-cerrar-tab');

            utilSAI.confirm(
          mensajeCerrarPestana 
           , function () {               
                   clienteSAI.setActiveTab(clienteSAI.deleteTabCliente(idCliente));
                   $(idTablaCliente).resize();               
           });
        });
    }

    //Asocia el evento Editar Cliente
    function asociaEventoEditarCliente() {
        $("#divTabContenCliente").on("click", '.icono-editar', function () {
            var iconoEditar = $(this);
                        
            var idCliente     = iconoEditar.data('id-cliente');
            var nombreCliente = iconoEditar.data('nombre-cliente');
           clienteSAI.agregarTabEditarCliente(idCliente
                                             , nombreCliente);
        });
    }

    //Asocia evento Tab Creado
    function asociaEventoCreadoTabCliente() {

        $(document).on("tabClienteCreated", function (event, idCliente, nombreCliente) {
            var form = $("#formActualizarCliente_" + idCliente);
            utilSAI.initSelect2(form);        
            utilSAI.initDate();
            asociaEventoMayusculasOnBlur(form);
            asociaEventoOnChangeSelect(form);
            asociaEventoFullScreenWidget(form);
            asociaEventoMuestraWidget(form);
            asociaEventoAccoridonExpanded(form);
            asociaEventoCrearVehiculo(form, idCliente);
            asociaEventoCrearDispositivo(form, idCliente);
            asociaEventoEditarDatosAdicionales(form, idCliente);
            setScrollTabs();
        });
    }
   
    function asociaEventoFullScreenWidget(form) {
        form.on('fullscreened.ace.widget', '.widget-box', function (event) {
           
        });
    }

    function asociaEventoAccoridonExpanded(form) {
        form.find('.accordion-style1').on('shown.bs.collapse' , function (e) {
           
        });
    }
    
    function asociaEventoMuestraWidget(form) {
        form.on('shown.ace.widget', '.widget-box', function (event) {
           
        });
        
    }
    
    //Asocia evento de abrir la ventana para asociar vehículos al cliente 
    function asociaEventoCrearVehiculo(form, idCliente) {
        form.find('#divWidgetClienteVehiculo').on('click', '.icono-crear-registro', function () {
            //Se abre la ventana
            clienteSAI.CrearRegistroVehiculo(form, idCliente);
        });

    }

    //Asocia evento de abrir la ventana para asociar dispositivos al cliente
    function asociaEventoCrearDispositivo(form, idCliente) {
        form.find('#divWidgetClienteDispositivo').on('click', '.icono-crear-registro', function () {
            //Se abre la ventana
            clienteSAI.CrearRegistroDispositivo(form, idCliente);
        });
    }

    //Asocia evento de abrir la ventana para editar la información adicional del cliente
    function asociaEventoEditarDatosAdicionales(form, idCliente) {
        form.find('#divWidgetClienteDatosGenerales').on('click', '.icono-editar-registro', function () {
            //Se abre la ventana
            clienteSAI.EditarDatosAdicionales(form, idCliente);
        });

    }



    //Para colocar los campos en mayúscula
    function asociaEventoMayusculasOnBlur(form) {
        form.find('.text-uppercase').on('blur',function () {
            $(this).val($(this).val().toUpperCase());
        });            
    }


    //Asocia evento Repintar para los chosen
    function asociaEventoRepintar() {       
        $('#ulTabCliente').on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
           $(idTablaCliente).resize();
        });
    }

    function recargarTablaCliente() {
        $(idTablaCliente).DataTable().ajax.reload();
    }

    //Permite agregar un nuevo tab
    clienteSAI.agregarTabEditarCliente = function (idCliente, nombreTab) {

        //Se busca si existe un tab abierto, si es así se cierra
        clienteSAI.deleteTabCliente(idCliente);
       
        //Se busca la información del cliente
        var noti = utilSAI.notifyLoadingInfo("Cargando...", { clickToHide: false, autoHide:false });
        var tablaCliente = $(idTablaCliente);
        
        $.ajax({
            url: tablaCliente.data('url-editar') + '/' + idCliente
               , cache: false
               , type: 'GET'
               , error: function (request, status, error) {
                  
               }
            })
           .done(function (data) {

               utilSAI.notifyClose(noti);

               if (typeof data.success != 'undefined' && !data.success) {
                   utilSAI.notifyError(data.mensaje);
               } else {

                   var htmlContentPestana = data;

                   var htmlPestana = ' <li id="liCliente_' + idCliente + '"  data-id-cliente="' + idCliente + '">'
                            + '<a href="#tabCliente_' + idCliente + '" data-toggle="tab"> ' + nombreTab + '<span class="button-close-tab"> <i class="blue fa fa-times  pointer bigger-120"></i> </span></a>'
                            + '</li>';
                   var ulTabCliente = $("#ulTabCliente");
                   ulTabCliente.append(htmlPestana);

                   var htmlContent = '<div class="tab-pane" data-id-cliente="' + idCliente + '" id="tabCliente_' + idCliente + '"> '
                       + 'Formulario de edición del cliente ' + idCliente + ' '
                   + '</div>';

                   $("#divTabContenCliente").append(htmlContentPestana);

                   //Se coloca como pestana activa
                   clienteSAI.setActiveTab(idCliente);

                   //Se activan las validaciones por ajax
                   $.validator.unobtrusive.parse("#formActualizarCliente_" + idCliente);

                   //Se dispara el evento tab creado tabClienteCreated
                   ulTabCliente.trigger("tabClienteCreated", [idCliente, nombreTab]);

               }
           })
           .fail(function (data) {
               utilSAI.manejaErrorGenerico(noti, data);               
           });
       
        return true;
    }
    
    //Permite cerrar un tab. Retorna el número de tab presedente
    clienteSAI.deleteTabCliente = function (idCliente) {
        var liAEliminar = $("#ulTabCliente").find("li[data-id-cliente='" + idCliente + "']");
        var tabNumber = liAEliminar.prev().data('id-cliente');
        var divEliminar = $("#divTabContenCliente").find("div[data-id-cliente='" + idCliente + "']");
        liAEliminar.remove();
        divEliminar.remove();
        return tabNumber;
     }

    //Permite asignar un tab activo
    clienteSAI.setActiveTab = function (idCliente) {
        var ulTabCliente = $("#ulTabCliente");

        if (ulTabCliente.find("li[data-id-cliente='" + idCliente + "']").size() > 0) {
            ulTabCliente.find("li[data-id-cliente]").filter('.active').removeClass('active');
            ulTabCliente.find("li[data-id-cliente='" + idCliente + "']").addClass('active');

            var divTabContentCliente = $("#divTabContenCliente");
            divTabContentCliente.find('div[data-id-cliente]').filter('.active').removeClass('in active');
            divTabContentCliente.find("div[data-id-cliente='"+idCliente+"']").addClass('in active');
        }
    }
    

    

    //Función que se ejecuta una vez completado el submit de un formulario ajax de forma exitosa
    clienteSAI.submitSuccess = function(data){

        var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
        if (content.mensajeAdvertencia != 'undefined' && content.mensajeAdvertencia!=null && content.mensajeAdvertencia != '')
            utilSAI.notifyWarning(content.mensajeAdvertencia);

        if (data.success) {
            utilSAI.notifySuccess(data.mensaje);
            recargarTablaCliente();
            bootbox.hideAll();
           
            if (content.IN_ABRIR_TAB && content.IN_ENVIAR_CORREO)
                enviaCorreoRegistro(content.ID_RESPUESTA, function () { clienteSAI.agregarTabEditarCliente(content.ID_RESPUESTA, content.NOMBRE_CLIENTE); });
            else {
                if (content.IN_ABRIR_TAB) clienteSAI.agregarTabEditarCliente(content.ID_RESPUESTA, content.NOMBRE_CLIENTE);
            }
                

        } else {
            $('#resultUpdate').html(data.mensajeHtml);
        }
    }

    // Coloca scroll de los los div
    function setScrollTabs() {
        utilSAI.setRightScroll();
    }
    
    //Asocia los eventos de cambio de select para la dirección del cliente 
    function asociaEventoOnChangeSelectDireccion(form) {

        //Pais
        utilSAI.setSelectAjax(form,
            {
                changeClass: '.pais'
                    , destinyClass: '.provincia'
                    , parameters: function () {
                        return {
                            pais: $(form).find('.pais').val()
                        };
                    }
            }
        );

        //Provincia
        utilSAI.setSelectAjax(form,
                {
                    changeClass: '.provincia'
                    , destinyClass: '.municipio'
                    , parameters: function () {
                        return {
                                pais: $(form).find('.pais').val()
                             , provincia: $(form).find('.provincia').val()
                        };
                    }
                }
            );

        //Municipio
        utilSAI.setSelectAjax(form,
                {
                    changeClass: '.municipio'
                    , destinyClass: '.corregimiento'
                    , parameters: function () {
                        return {
                               pais: $(form).find('.pais').val()
                             , provincia: $(form).find('.provincia').val()
                             , distrito: $(form).find('.municipio').val()
                            };
                        }
                    }
                );
            }


    //Asocia los eventos de cambio de select 
    function asociaEventoOnChangeSelect(form) {

        asociaEventoOnChangeSelectDireccion(form);
        
        form.find("#accordionVehiculos").find('.panel').each(function (index, itemData) {
            var formPanel = $(itemData).find('.panel-body');
            utilSAI.setSelectAjax(formPanel, {
                changeClass: '.marca'
                  , destinyClass: '.modelo'
                  , parameters: function () {
                      return {
                          marca: $(formPanel).find('.marca').val()
                      };
                  }
            });

            utilSAI.setSelectAjax(formPanel, {
                changeClass: '.modelo'
              , destinyClass: '.ano'
              , parameters: function () {
                  return {
                      marca: $(formPanel).find('.marca').val()
                      , modelo: $(formPanel).find('.modelo').val()
                  };
              }
            });

        });        
    }

    //Función para aasociar el evento de cambio de select para la ventana modal
    function asociaEventoOnChangeSelectModal(form) {        

             utilSAI.setSelectAjax(form, {
                changeClass: '.marca'
                  , destinyClass: '.modelo'
                  , parameters: function () {
                      return {
                          marca: $(form).find('.marca').val()
                      };
                  }
            });

            utilSAI.setSelectAjax(form, {
                changeClass: '.modelo'
              , destinyClass: '.ano'
              , parameters: function () {
                  return {
                      marca: $(form).find('.marca').val()
                      , modelo: $(form).find('.modelo').val()
                  };
              }
            });       
    }


    //Se busca la información del usuario y se carga a los datos del cliente
    function asociaEventoBuscarUsuario(form) {

        //Tipo de documento    
        form.find('#TipoDocumentoSeleccionado').on('change', function () {
            buscaDatosUsuario(form);
        });

        //Número de Documento
        form.find('#Cedula').on('blur', function () {
            buscaDatosUsuario(form);
        });
    }

    //Se busca la información del usuario
    function buscaDatosUsuario(form) {
        var tipoDocumento       = form.find('#TipoDocumentoSeleccionado');
        var numeroDocumento     = form.find('#Cedula');

        if (tipoDocumento.val().trim() != '' && numeroDocumento.val().trim() != '') {
            //Se realiza la búsqueda de los datos del usuario
             var options = {
                 url: form.find('#divContenedorFormulario').data('url-consultar-usuario')
                 ,dataType : "json"
                 , mostrarMensajeCargando: true
                , parametros                :   {  TipoDocumentoSeleccionado  :   tipoDocumento.val()
                                                 , Cedula: numeroDocumento.val()
                }
                , callback: function (data) {
                    if (data.content != null) {
                        form.find('#Id').val(data.content.Id);
                        form.find('#Nombre').val(data.content.Nombre);
                        form.find('#Apellido').val(data.content.Apellido);
                        form.find('#FechaNacimiento').val(data.content.FechaNacimiento);
                        form.find('#NumeroTelefonoFijo').val(data.content.NumeroTelefonoFijo);
                        form.find('#NumeroMovil').val(data.content.NumeroMovil);
                        form.find('#Email').val(data.content.Email);

                        cargaAplicaciones(form, data.content.Id);
                        cargaSociosComerciales(form, data.content.Id);
                    }
                }
            }

            utilSAI.executeActionCall(options);
        }
    }

    //Función para cargar los socios comerciales dado el Id del cliente
    function cargaSociosComerciales(form, id){
        
        utilSAI.loadDualListboxAjax(form, {
            changeClass   : '#Id'                                                             
            ,destinyClass : '.socioComercial'                                                         
            , parameters: function () {
                return {
                    IdCliente : form.find('#Id').val()
                }
            }
        });

    }
        
    //Función para cargar las aplicaciones del cliente
    function cargaAplicaciones(form, id) {
        var options = {
            url: form.find('#divContenedorFormulario').data('url-cargar-aplicaciones') + '/' + id
            , destino: '#divBloqueAplicaciones'
        }
        utilSAI.loadSectionAjax(options);
    }


    //Función para hacer el envío de correo de registro al cliente
    // options = {IndicadorReenvio : true // Permite saber si se trata de un reenvio de correo
        //}
    function enviaCorreoRegistro(idCliente, funcionCallback,options) {
        var tablaCliente = $(idTablaCliente);
        utilSAI.confirm(tablaCliente.data('mensaje-advertencia-envio-correo')
            ,function () {
                    utilSAI.executeActionCall({
                        url: tablaCliente.data('url-enviar-correo-registro') + '/' + idCliente
                        ,    mostrarMensajeCargando: true
                        , mensajeAlterno: tablaCliente.data('etiqueta-enviando-correo')
                    }).then(function () {
                        if (funcionCallback)
                            funcionCallback();                       
                    });
            }
            , function(){
                if(!options||!options.IndicadorReenvio||options.IndicadorReenvio==false)
                    funcionCallback()
            });
    }

    //Inicializa las cosas que deban inicializarse en todal la página
    function initAllDefault() {
        utilSAI.setRightScroll();
        utilSAI.initSelect2();
        utilSAI.initDate();
        utilSAI.initBootstrapDualListbox();
    }


    //Se asocia el objeto a la ventana
    window.clienteSAI = clienteSAI;

})(window);