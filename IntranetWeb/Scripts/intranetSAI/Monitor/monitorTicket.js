(function (window, undefined) {

    var monitorSAI = function () { }
    var idTablaTicket           = '#dynamic-table-ticket';
    var idTablaTicketHistorico = '#dynamic-table-ticket-historico';
    var idWidgetAlertas         = '#widgetAlerta';
    

    monitorSAI.idTablaTicket        = idTablaTicket;
    monitorSAI.idWidgetAlertas      = idWidgetAlertas;
    monitorSAI.sonidoAlertaActivo   = true;

    var audioAlert; 
    
    //Inicializa el audio
    function initAudioAlert(){
        try {
           audioAlert = new Audio($('#h2Titulo').data('url-audio-alerta'));
        }catch(err){ audioAlert = null;}
    }

    function playAlert() { if (typeof audioAlert != 'undefined' && audioAlert != null && monitorSAI.sonidoAlertaActivo) audioAlert.play() };


    //Se actualizagit  la información cada 60 segundos
    $(document).ready(function () {
        //Inicializa las alertas
        initTablaTicket();
        initAudioAlert();
        asociaEventos();
        utilSAI.initSelect2();         
        utilSAI.initDate();
        initAlertasHub();
    });

    //Inicializa el hub de las alertas con signalr
    function initAlertasHub() {

        var hub = $.connection.monitorAlertaHub;

        hub.client.actualizaTicketsAll = function (data) {
                       
            //Se obtienen los objectos
            if (typeof data.Data != 'undefined' && typeof data.Data.success != 'undefined' && !data.Data.success) {
                utilSAI.notifyError(data.Data.mensaje);
                console.error("No se actualizó ninguna alerta", 'Error signalR');
            } else {
                $(idWidgetAlertas).find('.scroll-content').html(data);
                //Para cada elemento se asocia el evento de crearle un ticket
                asociaEventosWidgetAlerta();//asociaEventoAlerta();
            }
        };

        $.connection.hub.start().done(function () {
           
        });
    }

    //Función para activar o inactivar sonidos de las alertas
    function eventoToggleSonidoAlerta() {
        $('#iSonidoAlertas').on('click', function () {
            var iconoSonido = $(this);
            if (monitorSAI.sonidoAlertaActivo) {
                iconoSonido.removeClass('fa fa-volume-up white'); iconoSonido.addClass('fa fa-volume-off orange2');
                monitorSAI.sonidoAlertaActivo = false;
            } else {
                iconoSonido.removeClass('fa fa-volume-off orange2'); iconoSonido.addClass('fa fa-volume-up white');
                monitorSAI.sonidoAlertaActivo = true;
            }
        });
    }
    

    //Función para asociar el evento de mostrar alertas
    function eventoMotrarOcularAlertas(){
        $('#chkMostarAlerta').on('click', function () {
            $('#divColAlertas').toggle(0, function () {

                var colTabs = $('#divColTabs');
                
                if (colTabs.hasClass('col-md-9')) {
                    colTabs.removeClass('col-md-9'); colTabs.addClass('col-md-12');
                }else{
                    colTabs.removeClass('col-md-12'); colTabs.addClass('col-md-9');
                }

                if (colTabs.hasClass('col-lg-9')) {
                    colTabs.removeClass('col-lg-9'); colTabs.addClass('col-lg-12');
                }else{
                    colTabs.removeClass('col-lg-12'); colTabs.addClass('col-lg-9');
                }   
                
                $(monitorSAI.idTablaTicket).resize();

                if (monitorSAI.map != null && typeof monitorSAI.map != 'undefined')
                    google.maps.event.trigger(monitorSAI.map, 'resize');
            });
            
        });
    }



    //Función para asociar los eventos a la página 
    function asociaEventos() {
        eventoRealoadWidgetAlerta();
        eventoConsultarTicket();       
        eventoEditarTicket();
        eventoCrearTicket();
        eventoMotrarOcularAlertas();
        eventoToggleSonidoAlerta();
    }

    //Función para consultar un ticket
    function eventoConsultarTicket() {
        $(idTablaTicket).on('click', '.icono-consultar', function () {
            var icono = $(this);
            //Se abre la ventana
            monitorSAI.consultaTicket(icono.data('id-ticket'));
        });
    }


    //Función para consultar un ticket historico
    function eventoConsultarTicketHistorico() {
        $(idTablaTicketHistorico).on('click', '.icono-consultar', function () {
            var icono = $(this);
            //Se abre la ventana
            monitorSAI.consultaTicketHistorico(icono.data('id-ticket'));
        });
    }

    //Función para editar un ticket
    function eventoEditarTicket() {
        $(idTablaTicket).on('click', '.icono-editar', function () {
            var icono = $(this);
            //Se abre la ventana
            monitorSAI.editaTicket(icono.data('id-ticket'));
        });
    }


    //Fucnión para crear un ticket
    function eventoCrearTicket() {        
        $('.icono-crear-ticket').on('click',  function () {           
            //Se abre la ventana
            monitorSAI.crearTicket();
        });
    }

    //Evenos asociados al formulario de creación de Ticket
    function eventosFormulariosCrearTicket(form) {
        asociaEventoOnChangeSelect(form);
    }


    //Asocia evento onchenge select
    function asociaEventoOnChangeSelect(form) {
        //Cambio de Cliente
        form.find('.clienteSelect').on('change', function () {
            var campoCliente = $(this);

            var dispositivo = form.find(".dispositivoSelect");
            dispositivo.empty();
            dispositivo.append($("<option/>", { value: "", text: "Seleccione Dispositivo " }));

            var idNoti = utilSAI.notifyLoadingInfo("Cargando ...", { clickToHide: false, autoHide: false, });

            $.getJSON(campoCliente.data("url"), { idCliente: campoCliente.val() })
            .done(function (data) {
                if (typeof data.success != 'undefined' && !data.success)
                    utilSAI.notifyError(data.mensaje);
                else {
                    data.content = JSON.parse(data.content);
                    var myData = data.content;
                    $.each(myData, function (index, itemData) {
                        dispositivo.append($("<option/>", { value: itemData.Value, text: itemData.Text }));
                    });

                    if (dispositivo.children('option').length == 2) {
                        dispositivo.find(':nth-child(2)').prop('selected', true);
                    }
                }
                dispositivo.trigger('change');
                utilSAI.notifyClose(idNoti);
            }).fail(function (data) {
                utilSAI.manejaErrorGenerico(idNoti, data);
                dispositivo.trigger('change');
            });
        });

        //Cambio de Tipo Atención
        form.find('.tipoAtencionSelect').on('change', function () {
            var campoTipoAtencion = $(this);

            var mensajeAlCliente = form.find(".mensajeAlClienteSelect");
            mensajeAlCliente.empty();
            mensajeAlCliente.append($("<option/>", { value: "", text: "Seleccione Mensaje al Cliente" }));

            var idNoti = utilSAI.notifyLoadingInfo("Cargando ...", { clickToHide: false, autoHide: false, });

            $.getJSON(campoTipoAtencion.data("url"), { tpAtencion: campoTipoAtencion.val() })
            .done(function (data) {

                if (typeof data.success != 'undefined' && !data.success)
                    utilSAI.notifyError(data.mensaje);
                else {
                    data.content = JSON.parse(data.content);
                    var myData = data.content;
                    $.each(myData, function (index, itemData) {
                        mensajeAlCliente.append($("<option/>", { value: itemData.Value, text: itemData.Text }));
                    });

                    if (mensajeAlCliente.children('option').length == 2) {
                        mensajeAlCliente.find(':nth-child(2)').prop('selected', true);
                    }

                }
                mensajeAlCliente.trigger('change');
                utilSAI.notifyClose(idNoti);
            }).fail(function (data) {
                utilSAI.manejaErrorGenerico(idNoti, data);
                mensajeAlCliente.trigger('change');
            });
        });
    }



    //Función para editar la observacion 
    function eventoGrabaObservacion(form) {
        $(form).on('click', '#aGrabaObservacion', function () {
            var enlaceGrabar = $(this);
            var idInfo = utilSAI.notifyLoadingInfo("Procesando ...", {clickToHide: false, autoHide:false });

            $.ajax({
                url: enlaceGrabar.data('url')
                   , cache: false
                  , type: 'POST'
                   , contentType: 'application/json, charset=utf-8'
                   , dataType: 'JSON'
                  , data:JSON.stringify({
                      Id: $(form).find('#Id').val()
                           , Observacion: $(form).find('#Observacion').val()
                  },
                   function (key, val)
                   { return key === "source" ? '' : val; })                   
            }).done(function (data) {

                utilSAI.notifyClose(idInfo);

                if (data.success) 
                    utilSAI.notifySuccess(data.mensaje);                
                else {

                    if (data.content != null && typeof data.content.mensajeAdvertencia != 'undefined' && data.content.mensajeAdvertencia != '')
                        utilSAI.notifyWarning(data.content.mensajeAdvertencia);
                    else
                        utilSAI.notifyError(data.mensaje);
                }
                refrescarAlertas();

            }).fail(function (data) {
                utilSAI.manejaErrorGenerico(idInfo, data);
            });
        });
    }


    //Realiza la búsqueda de un ticket
    monitorSAI.consultaTicket = function (idTicket) {
        var tablaTicket = $(idTablaTicket);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", {clickToHide: false, autoHide:false });
        $.ajax({
            url    :   tablaTicket.data('url-consultar')+"/"+idTicket
            ,cache  :   false
        }).done(function(content){
            utilSAI.notifyClose(idLoading);

            if (typeof content.success != 'undefined' && !content.success) {
                utilSAI.notifyError(content.mensaje);
            } else {

                var dialog = bootbox.dialog({
                    title: "Consulta de Ticket"
                    , size: "large"
                    , message: content
                    , onEscape: function() {}
                    , buttons: {
                     cancel: {
                         label: "Cerrar"
                       , className: "btn-sm btn-danger"
                     }
                 }
                });

                $(dialog).on("shown.bs.modal", function () {
                    initTablaTicketHistorico();
                    setScrollAlertas();
                    asociaEventoReenviarGCM();
                });
            }
        }).fail(
           function(data){
               utilSAI.manejaErrorGenerico(idLoading, data);
           });
    }


    //Realiza la búsqueda de un ticket Historico
    monitorSAI.consultaTicketHistorico = function (idTicket) {
        var tablaTicket = $(idTablaTicketHistorico);
       
        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });
        $.ajax({
            url: tablaTicket.data('url-consultar') + "/" + idTicket
            , cache: false
        }).done(function (content) {
            utilSAI.notifyClose(idLoading);
            if (typeof content.success != 'undefined' && !content.success)
                utilSAI.notifyError(content.mensaje);
            else{
                var dialog = bootbox.dialog({
                    title: "Consulta de Ticket"
                    , size: "large"
                    , message: content
                 , buttons: {
                     cancel: {
                         label: "Cerrar"
                             , className: "btn-sm btn-danger",
                     }
                 }
                });

                $(dialog).on("shown.bs.modal", function () {
                    setScrollAlertas();
                });
            }                 

        }).fail(
           function (data) {
               utilSAI.manejaErrorGenerico(idMensajeInfo, data);
           });
    }



    //Reliza la búsqueda de un ticket
    monitorSAI.editaTicket = function (idTicket) {
        var tablaTicket = $(idTablaTicket);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", {clickToHide: false, autoHide:false });
        
        $.ajax({
            url: tablaTicket.data('url-editar') + "/" + idTicket
            , cache: false
        }).done(function (content) {


            utilSAI.notifyClose(idLoading);

            if (typeof content.success != 'undefined' && !content.success) {
                utilSAI.notifyError(content.mensaje);
            } else {


                var buttonTicket = {};

                var contenido =  $(content);
                var idAceptar = '#aceptar';
                var idAtender = '#atender';
                var idTransferir = '#transferir';
                var idDescartar = '#descartar';
                
                //Aceptar
                if (contenido.find(idAceptar).length) {
                    buttonTicket["Guardar"] = {
                        className: "btn btn-sm btn-success"
                        , callback: function () {
                            $(idAceptar).trigger('click');
                            return false;
                        }
                    };
                }

                //Atender
                if (contenido.find(idAtender).length) {
                    buttonTicket["Atender"] = {
                        className: "btn btn-sm btn-success"
                        , callback: function () {
                            $(idAtender).trigger('click');
                            return false;
                        }
                    };
                }

                //Transferir
                if (contenido.find(idTransferir).length) {
                    buttonTicket["Transferir"] = {
                        className: "btn btn-sm btn-primary"
                        , callback: function () { 
                            $(idTransferir).trigger('click');
                            return false;                            
                        }
                    };
                }
                
                //Descartar
                if (contenido.find(idDescartar).length) {
                    buttonTicket["Descartar Ticket"] = {
                        className: "btn btn-sm btn-warning"
                        , callback: function () {                             
                            var desartarFunc = function () {
                                $(idDescartar).trigger('click');
                                return false;
                            }
                            utilSAI.confirm("¿Desea descartar el ticket?", desartarFunc);
                            return false;
                        }
                    };
                }

                buttonTicket["cancel"] ={
                    label: "Cerrar"
                    , className: "btn-sm btn-danger",
                };
                        

                var dialog = bootbox.dialog({
                    title: "Ticket"
                             , size: "large"
                             , message: content
                             , buttons: buttonTicket
                             , onEscape: function() {}
                });                
                        
                $(dialog).on("shown.bs.modal", function () {
                    utilSAI.initSelect2();                     
                    initTablaTicketHistorico();               
                    setScrollAlertas();
                    var formulario = $('#formActualizarTicket');
                    eventoGrabaObservacion(formulario);
                    asociaEventoOnChangeSelect(formulario);
                });
            }
        }).fail(
           function (data) {
               utilSAI.manejaErrorGenerico(idLoading, data);
           });
    }

    //Reliza la carga del formulario para crear un ticket
    monitorSAI.crearTicket = function () {
        var tablaTicket = $(idTablaTicket);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });

        $.ajax({
            url: tablaTicket.data('url-crear') 
            , cache: false
        }).done(function (content) {
            utilSAI.notifyClose(idLoading);

            if (typeof content.success != 'undefined' && !content.success) {
                utilSAI.notifyError(content.mensaje);
            } else {

                var buttonTicket = {};

                var contenido = $(content);
                var idAceptar = '#aceptar';
                
                //Aceptar
                if (contenido.find(idAceptar).length) {
                    buttonTicket["Guardar"] = {
                        className: "btn btn-sm btn-success"
                        , callback: function () {
                            $(idAceptar).trigger('click');
                            return false;
                        }
                    };
                }
                                
                buttonTicket["cancel"] = {
                    label: "Cerrar"
                    , className: "btn-sm btn-danger",
                };


                var dialog = bootbox.dialog({
                    title: "Crear Ticket"
                             , size: "large"
                             , message: content
                             , buttons: buttonTicket
                             , onEscape: function () { }
                });
                
                $(dialog).on("shown.bs.modal", function () {
                    utilSAI.initSelect2();
                     
                    setScrollAlertas();
                    var idFormularioCrearTicket = '#formCrearTicket';
                    var formulario = $(idFormularioCrearTicket);
                    //Asociar eventos ajax al formulario

                    var currentTime = new Date();
                    $(formulario).find('.date-time-picker').datetimepicker({
                        dateFormat: utilSAI.dateFormat
                        , timeFormat: utilSAI.hourFormat
                        , autoclose: true
                        , maxDate: currentTime
                    });
                    $('.date-time-picker').removeAttr("data-val-date");
                    $.validator.unobtrusive.parse(idFormularioCrearTicket);
                    //Se asocian los eventos al formulario
                    eventosFormulariosCrearTicket(formulario);
                });
            }
        }).fail(
           function (data) {
               utilSAI.manejaErrorGenerico(idInfo, data);
           });
    }

    

    //Reacarga el widget de las alertas 
    function eventoRealoadWidgetAlerta() {
        //Se coloca el scroll de las alertas
        setScrollAlertas();
        asociaEventosWidgetAlerta();//asociaEventoAlerta();
       
        var divAlerta = $('#divAlertas');

        $(idWidgetAlertas).on('reload.ace.widget', function (event) {
            toggleLoadingWidgetAlertas();
            $.ajax({
                url: divAlerta.data('url')
                , cache: false
            }).done(function (content) {
                if (typeof content.success != 'undefined' && !content.success) {
                    utilSAI.notifyError(content.mensaje);
                } else {
                    $(idWidgetAlertas).find('.scroll-content').html(content);
                    //Para cada elemento se asocia el evento de crearle un ticket
                    asociaEventosWidgetAlerta();//asociaEventoAlerta();
                }
                $(idWidgetAlertas).trigger('reloaded.ace.widget');
                toggleLoadingWidgetAlertas();
                
           }).fail(
                function (data) {
                    utilSAI.manejaErrorGenerico(null, data);
                    $(idWidgetAlertas).trigger('reloaded.ace.widget');
                    toggleLoadingWidgetAlertas();
                }
                );
        });       
      }


    //Funcion para asociar eventos a la alertas
    function asociaEventoAlerta() {
        $(idWidgetAlertas).find('.widget-body').first().find('.widget-title').each(
            function (index, element) {
                if (index == 0) { playAlert(); }
                $(this).on('click', function () {
                    var notificacion = $(this).parent();
                    crearTicket(notificacion.data('id'), notificacion.data('origen-notificacion'));
                    ubicaCliente(notificacion.data('id'), notificacion.data('indicador-localizacion'), notificacion.data('id-dispositivo'));
                });
            });
    }


    //Función para asociar evento de búsqueda de información de la notificación
    function asociaEventoBuscaDetalleNotificacion(){
        
        $(idWidgetAlertas).find('.widget-body').first().find('.aConsultar').each(
          function (index, element) {
              $(this).on('click', function () {
                  
                  var notificacion = $(this).closest('.widget-header');
                  monitorSAI.consultarDetalleNotificacion(notificacion.data('id'), notificacion.data('origen-notificacion'));
                  ubicaCliente(notificacion.data('id'), notificacion.data('indicador-localizacion'), notificacion.data('id-dispositivo'));
              });
          });
    }
    
    //Función para asociar evento eliminar a la notificación
    function asociaEventoEliminaNotificacion() {
        $(idWidgetAlertas).find('.widget-body').first().find('.aEliminar').each(
             function (index, element) {
                 $(this).on('click', function () {

                     var notificacion = $(this).closest('.widget-header');
                     monitorSAI.eliminarNotificacion(notificacion.data('id'), notificacion.data('origen-notificacion'));
                     ubicaCliente(notificacion.data('id'), notificacion.data('indicador-localizacion'), notificacion.data('id-dispositivo'));
                 });
             }
            );
        }



    //Función para eliminar la notificación 
    monitorSAI.eliminarNotificacion = function (idNotificacion, idOrigenNotificacion) {

        var mensaje = $(idWidgetAlertas).data('mensaje-confirmar-eliminar');
        
        utilSAI.confirm(mensaje, function () {

            var url = $(idWidgetAlertas).data('url-eliminar-notificacion'); 
           

            var notificacionData = {
                Id: idNotificacion
            , IdOrigenNotificacion: idOrigenNotificacion
            };

            var options = {
                 url: url
               , parametros: notificacionData
                , callback: function () {
                    try {
                        var hub = $.connection.monitorAlertaHub;
                        hub.server.actualizaTickets();
                    } catch (err) { console.error(err, "No se logró actualizar widget alerta en todos los clientes") }
                }
            };

            utilSAI.executeActionCall(options);
        });


        
    }


    //Función para hacer la búsqueda del detalle de l notificación 
    monitorSAI.consultarDetalleNotificacion = function(idNotificacion, idOrigenNotificacion) {
        var url = $(idWidgetAlertas).data('url-consultar-notificacion');
        var notificacionData = {
            Id: idNotificacion
            , IdOrigenNotificacion: idOrigenNotificacion
        };

        var options = {
                idTitulo: "#divTabConsultarNotificacion"
            , url: url 
            , parametros: notificacionData
            , buttons: [{
                id: "#cerrar"
                        , class: utilSAI.buttonDangerClass
                       , isCloseButton: true
                     }]
            , callbackOnShow: function () { initTablaTicketHistorico(); }
            , closeOnEscape  : true
           };
        utilSAI.openModal(options);
    }



    //Asocia todos los eventos al widget de alertas
    function asociaEventosWidgetAlerta() {
        asociaEventoAlerta();
        asociaEventoBuscaDetalleNotificacion();
        asociaEventoEliminaNotificacion();
        muestraImagenNotificacion();

    }

    //Función para asociar el evento de mostrar las imagenes 

    function muestraImagenNotificacion() {
        $('[data-rel="colorbox"]').click(function () {
            var $overflow = '';
            var colorbox_params = {
                reposition: true,
                close: '&times;',
                scalePhotos: true,
                scrolling: false,
                maxWidth: '100%',
                maxHeight: '100%',
                href: $(this).attr('src'),
                photo: true,
                onOpen: function () {
                    $overflow = document.body.style.overflow;
                    document.body.style.overflow = 'hidden';
                },
                onClosed: function () {
                    document.body.style.overflow = $overflow;
                },
                onComplete: function () {
                    $.colorbox.resize();
                }
            }
            $(this).colorbox(colorbox_params);           
        });

    }


    //Ubica el cliente al crear la alerta, o al consultat o eliminar notificiacion
    function ubicaCliente(idNotificacion, indicadorLocalizacion, idDispositivo) {
        try {
            if (indicadorLocalizacion.toLowerCase() == 'true')
                monitorMapaSAI.ubicaClienteSAI(idNotificacion);
            else {
                 google.maps.event.trigger(monitorMapaSAI.markers[idDispositivo], 'click');

            }
        } catch (err) { console.error(err.message, 'Fallo Ubicando Vehículo'); }
    }


    //Evento para reenviar las alertas
    function asociaEventoReenviarGCM() {
        var widgetConsulta = $('#widgetBodyTicket');

        widgetConsulta.find('.icono-reenviar-gcm').on('click', function () {
            utilSAI.confirm("¿Desea reenviar mensaje al cliente?", function () {
                enviaTicketGCM(widgetConsulta.data('id'));
                bootbox.hideAll();
            });
        });
    }


    //Toggle Loading Ticket
    /*function toggleLoadingTicketTable(){
        $("#iLoadingTablaTicket").toggle(500);
    }*/

    //Toggle Loading Alertas
    function toggleLoadingWidgetAlertas() {
        $("#iLoadingWidgetAlertas").toggle(500);
    }

    //Función para inciar la tabla de Tickts 
    function initTablaTicket() {
        //toggleLoadingTicketTable();
        var columns = [];
        $(idTablaTicket).find('thead').find('th').each(
                 function (index) {
                         columns[index] = index == 0 ? { data: $(this).data('name'), "bSortable": false } : { data: $(this).data('name'), "defaultContent": " " };
                 });

        var tableTicket =
        $(idTablaTicket).DataTable({
            language: {
                url: $(idTablaTicket).data('url-localization')
            }
            , ajax: {
                url: $(idTablaTicket).data('url')
                        ,serverSide: true
                        ,bDestroy: true
                        ,bJQueryUI: true
                        , type           : 'POST'
                        , data: function (d) {
                           // toggleLoadingTicketTable();
                            return {
                                FechaBusquedaDesde        : $('#FechaBusquedaDesde').val()
                              , FechaBusquedaHasta        : $('#FechaBusquedaHasta').val()
                              , UsuarioTicketSeleccionado : $('#UsuarioTicketSeleccionado').val()
                              , TipoAtencionSeleccionada  : $('#TipoAtencionSeleccionada').val()
                              , parametro: d
                            };
                        }
                , dataSrc: function (json) {                   
                 //   toggleLoadingTicketTable();

                    if (typeof json.success != 'undefined' && !json.success)
                        utilSAI.notifyError(json.mensaje);

                    return json.data != null ? JSON.parse(json.data) : json.content;
                }
                , error: function (data) {
                   // toggleLoadingTicketTable();                    
                    utilSAI.manejaErrorGenerico(null, data);
                }
            }
             , processing: true
             , serverSide: true
            , aoColumns: columns
            , responsive: true
             , bAutoWidth: false
            , "iDisplayLength": 10
            
        });

        //initiate TableTools extension
        var tableTools_obj = new $.fn.dataTable.TableTools(tableTicket, {
            "sRowSelector": "td:not(:last-child)"
            ,"sRowSelect": "multi"
            ,"sSelectedClass": "success"
        });
        //we put a container before our table and append TableTools element to it
        $(tableTools_obj.fnContainer()).appendTo($('.tableTools-container'));


        //Se inicializan los tab        
        var nuSegundosLlamado = 100;        
        window.setInterval(function () {            
            recargarTablaTicket();            
        }, nuSegundosLlamado * 1000);
        

        $('#iReloadTablaTicket').parent('a').on('click', function () { recargarTablaTicket(); });

        return tableTicket;
    }

    
    //Función para inciar la tabla de histórico de tickets
    function initTablaTicketHistorico() {
               
        var tableTicketHisto =
        $(idTablaTicketHistorico).DataTable({
            language: {
                url: $(idTablaTicket).data('url-localization')
            }
            , responsive: true
             , bAutoWidth: false
             , paging: false
            , bFilter: false
            , bJQueryUI: true
            , sDom: 'lfrtip'
            
        });

       
        

        //initiate TableTools extension
        var tableTools_obj = new $.fn.dataTable.TableTools(tableTicketHisto, {
            "sRowSelector": "td:not(:last-child)"
            ,"sRowSelect": "multi"
            ,"sSelectedClass": "success"
        });
        //we put a container before our table and append TableTools element to it
        $(tableTools_obj.fnContainer()).appendTo($('.tableTools-container'));

        eventoConsultarTicketHistorico();
        
        return tableTicketHisto;
    }



    
    function recargarTablaTicket() {
       // toggleLoadingTicketTable();
        $(idTablaTicket).DataTable().ajax.reload();
    }

    //Permite setear el scroll a las alertas
    function setScrollAlertas() {
        utilSAI.setRightScroll();
    }
   
    //Función para actualizar la ventana de alertas
    function refrescarAlertas() {        
        $('#widgetAlerta').trigger('reload.ace.widget');        
    }

    //Función para crear un ticket 
    function crearTicket(idNotificacion, origenNotificacion) {

        notificacionData = {
             Id                     : idNotificacion
            ,IdOrigenNotificacion   : origenNotificacion
        };
        //Se crea el ticket
        ingresaTicketAjax(notificacionData);        
    }

    //Función para guardar la notificación por Ajax
    function ingresaTicketAjax(notificacion) {
        var divAlerta = $('#divAlertas');
        var idInfo = utilSAI.notifyLoadingInfo("Procesando Ticket ...", {clickToHide: false, autoHide:false } );

        $.ajax({
            url: divAlerta.data('url-atender')
               , cache: false
               , data: JSON.stringify(notificacion, function (key, val) { return key === "source" ? '' : val; })
               , type: 'POST'
               , contentType: 'application/json, charset=utf-8'
               , dataType: 'JSON'
        }).done(function (data) {

               utilSAI.notifyClose(idInfo);
                
               if (data.success === true) {
                   data.content = JSON.parse(data.content);
                   utilSAI.notifySuccess(data.mensaje);

                   try {
                       var  hub = $.connection.monitorAlertaHub;
                            hub.server.actualizaTickets();
                   } catch (err) {console.error(err,"No se logró actualizar widget alerta en todos los clientes") }

                   recargarTablaTicket();
                   monitorSAI.editaTicket(data.content.ID_TICKET);
                   
               } else {
                  
                   if (data.content != null && typeof data.content.mensajeAdvertencia != 'undefined' && data.content.mensajeAdvertencia != '')
                       utilSAI.notifyWarning(data.content.mensajeAdvertencia);
                   else
                       utilSAI.notifyError(data.mensaje);
               }
               

               refrescarAlertas();
        }).fail(function (data) {
                utilSAI.manejaErrorGenerico(idInfo, data);
                refrescarAlertas();
           });
    }

        //Función para manejar la respuesta del Servidor
        monitorSAI.submitSuccess = function (data) {
            if (data.success) {
                utilSAI.notifySuccess(data.mensaje);
                recargarTablaTicket();
                bootbox.hideAll();
                data.content = JSON.parse(data.content);
                //Se verifica si se debe enviar el CMS
                if (data.content!=null&&data.content.InEnvioCMS) {
                    enviaTicketGCM(data.content.IdTicket);
                }

            } else {
                $('#resultUpdate').html(data.mensajeHtml);
            }
        }

        //Envía mensaje GCM al cliente
        function enviaTicketGCM(ticketId) {
           
            var tabTicket = $('#divTabTicket');

            var idInfo = utilSAI.notifyLoadingInfo("Enviando mensaje al cliente ...", { clickToHide: false, autoHide: false });
           
            $.ajax({
                url: tabTicket.data('url-gcm') + '/' + ticketId
               , cache: false               
               , type: 'GET'               
            }).done(function (data) {

                utilSAI.notifyClose(idInfo);

                if (data.success === true) 
                    utilSAI.notifySuccess(data.mensaje);                    
                 else 
                    utilSAI.notifyError(data.mensaje);

            }).fail(function (data) {
                  utilSAI.manejaErrorGenerico(idInfo, data);  
            });
        }

        //Función para manejar la respuesta del Servidor al crear un ticket desde cero
        monitorSAI.submitSuccessCrear = function (data) {
            var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
            if (content.mensajeAdvertencia != null && typeof content.mensajeAdvertencia != 'undefined' && content.mensajeAdvertencia != '')
                utilSAI.notifyWarning(content.mensajeAdvertencia);

            if (data.success) {
                utilSAI.notifySuccess(data.mensaje);
                recargarTablaTicket();

                bootbox.confirm({
                    message: "<h4>¿Desea gestionar el Ticket?</h4>",
                    buttons: {
                        confirm: {
                            label: "Aceptar"
                           , className: "btn-sm btn-success",
                        }
                        , cancel: {
                            label: "Cerrar"
                        , className: "btn-sm btn-danger",
                        }
                    },
                    callback: function (result) {
                        bootbox.hideAll();
                        if (result) {
                            var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
                            monitorSAI.editaTicket(content.ID_TICKET);
                        }
                    }
                })
                
            } else {
                $('#resultUpdate').html(data.mensajeHtml);
            }
        }

    window.monitorSAI = monitorSAI;

})(window);