var utilSAI = (function (window) {

    var dateFormat = "dd/mm/yyyy"
        , hourFormat = "HH:mm"
        , dateFormatHour = "dd/mm/yyyy HH:mm"
        , etiqueta = {
              procesando: ''
            , cargando: ''
            , advertencia: ''
            
        }
        , culture = $("html").prop("lang")
        , isLocalStorageSupported = Storage? true : false; 
        
    //Función para permitir escribir en los select  en las modales
    var allowSelect2ypeModal = function () {
        try {
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        } catch (err) { console.error("Falló allowSelect2ypeModal"); }
    }

    var confirm = function (mensaje, callbackFunction, callbackCancelFunction) {
            bootbox.confirm({
                message: "<h4>"+mensaje+"</h4>",
                buttons: {
                    confirm: {
                        label: $('body').data('aceptar')
                       , className: utilSAI.buttonSuccessClass
                    }
                    ,cancel: {
                        label: $('body').data('cancelar')
                    , className: utilSAI.buttonDangerClass
                    }                
                },
                callback: function (result) {
                    if (result) {
                        callbackFunction();
                    } else {
                        if (callbackCancelFunction) callbackCancelFunction();
                    }
                }
            });
        }

    //Agrega valicación adecuada para las fechas
    var setDateValidation = function () {

        if (typeof $.validator != 'undefined') {
            $.validator.methods.date = function (value, element) {
                var val = Globalize.parseDate(value, dateFormat, culture);
                return this.optional(element) || val;
            }
        }
    }


    //Agrega estilos para el envío de alertas 
    var setEstilosNotificacion = function() {
        //Estilo para el envío de alerta
        $.notify.addStyle('info', {
            html:
              "<div class='row'>" +
                  " <div class='alert alert-info' data-notify-html='title'/> " +
               "</div>"
        });


        $.notify.addStyle('success', {
            html:
              "<div class='row' style='z-index:2147483647'>" +
                  " <div class='alert alert-block alert-success' data-notify-html='title'/> " +
               "</div>"
        });

        $.notify.addStyle('error', {
            html:
              "<div class='row'>" +
                  " <div class='alert alert-danger' data-notify-html='title'/> " +
               "</div>"
        });

        $.notify.addStyle('warning', {
            html:
              "<div class='row'>" +
                  " <div class='alert alert-warning' data-notify-html='title'/> " +
               "</div>"
        });
    }
    
    //Se coloca el efecto de tooltip
    var setToolTip = function () {
        $('body').tooltip({
            selector: '[data-rel="tooltip"]'
        });
    }

    //Asocia evento logout
    var asociaEventoLogOut = function() {
        $("#aLogOut").on('click', function () {
            return logOut();
        });
    }

    //Organiza posición menu
    var organizaMenu = function () {
        try { ace.settings.check('main-container', 'fixed') } catch (e) { }

        try { ace.settings.check('sidebar', 'fixed') } catch (e) { }
    }

    //Coloca evento click nodo
    var colocaEventoClickNodoActivo = function () {
        $('.liMenuActive').parents("li").addClass('active open');
    }

    var initPopover = function  () {
        $('[data-rel="popover"]').popover();
    }

    //Resize Chosen
    var resizeChosen = function () {
        if (!ace.vars['touch']) {

            $(window)
                .off('resize.chosen')
                .on('resize.chosen', function () {
                    $('.chosen-select').each(function () {
                        var $this = $(this);
                        $this.next().css({ 'width': $this.parent().width() });
                    })
                }).trigger('resize.chosen');
            //resize chosen on sidebar collapse/expand
            $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
                if (event_name != 'sidebar_collapsed') return;
                $('.chosen-select').each(function () {
                    var $this = $(this);
                    $this.next().css({ 'width': $this.parent().width() });
                })
            });
        }
    }


    //Función que retorna los parámetros de configuración de bootboxlist 
    var getDuallistboxOptions = function () {
        try {
            var lan = culture.substring(0, culture.indexOf('-'));
        } catch (err) { lan = 'en'; }

        switch (lan) {
            case 'es':
                return {
                    filterPlaceHolder: 'Filtro'
                    , moveSelectedLabel: 'Mover seleccionado'
                    , moveAllLabel: 'Mover todos'
                    , removeSelectedLabel: 'Remover seleccionado'
                    , removeAllLabel: 'Remover todos'
                    , infoText: 'Mostrando {0}'
                    , infoTextFiltered: '<span class="label label-warning">Filtrado</span> {0} de {1}'
                    , infoTextEmpty: 'Lista vacía'
                    , filterTextClear: 'Mostrar todos'
                    , selectorMinimalHeight: '30'
                }
                break;
            case 'en':
                return {
                    filterPlaceHolder: 'Filter'
                    , moveSelectedLabel: 'Move selected'
                    , moveAllLabel: 'Move all'
                    , removeSelectedLabel: 'Remove selected'
                    , removeAllLabel: 'Remove all'
                    , infoText: 'Showing all {0}'
                    , infoTextFiltered: '<span class="label label-warning">Filtered</span> {0} from {1}'
                    , infoTextEmpty: 'Empty list'
                    , filterTextClear: 'Show all'
                    , selectorMinimalHeight: '30'
                }
                break;
            default:
                return {}
        }
    }

    //Función para inicializar y para colocar los valores por defecto
    var initElementBootstrapDualListbox = function (element) {
        var minRecords = 5;
        var options = getDuallistboxOptions();
        var thisElement = $(element);
        var records = thisElement.attr('size'); options['selectorMinimalHeight'] = records != null && records != '' ? parseInt(records, 10) * parseInt(options['selectorMinimalHeight'], 10) : minRecords * parseInt(options['selectorMinimalHeight'], 10);
        thisElement.bootstrapDualListbox(options);
    }


    //Permite hacer logout de la aplicación
    var logOut = function () {
        confirm(
            $('body').data('mensaje-logout')
            , function () { var aLogOut = $("#aLogOut"); window.location.href = aLogOut.data("url"); });
    }


    //Manejo de error genérico
    var manejaErrorGenerico = function (idMensajeInfo, data) {

        if (idMensajeInfo) {
            this.notifyClose(idMensajeInfo);
        }
        console.error(data, this.MensajeError.ErrorDesconocido);
        this.notifyErrorJson(data);
    }

    //Mensaje de error
    var notifyError = function (mensaje, options) {
        var id = $.guid++;
        var optionsError = { style: "error", position: 'top center' };
        if (options != null && typeof options != 'undefined')
            for (var key in options) {
                optionsError[key] = options[key];
            }
        $.notify({ title: "<div data-id='" + id + "'><i class='ace-icon fa fa-times'></i> " + mensaje + "</div>" },
                             optionsError);

        return id;
    }

    //Fucnión ejecutada al cargar la página
    $(document).ready(function () {
       
        etiqueta.procesando = $('body').data('etiqueta-procesando');
        etiqueta.cargando = $('body').data('etiqueta-cargando');
        etiqueta.advertencia = $('body').data('etiqueta-advertencia');
        
        this.dateFormat = "dd/mm/yyyy";
        this.hourFormat = "HH:mm";
        this.dateFormatHour = "dd/mm/yyyy HH:mm";


        
        allowSelect2ypeModal();       
        culture = $("html").prop("lang");
        Globalize.culture(culture);
        
        setDateValidation();
        setToolTip();
        initPopover();        
        setEstilosNotificacion();
        asociaEventoLogOut();
        organizaMenu();
        resizeChosen();

        //Coloca evento click nodo del menu
        colocaEventoClickNodoActivo();
    });

       
             
    
    return {
              dateFormat              : dateFormat
            , dateFormatHour          : dateFormatHour
            , hourFormat              : hourFormat
            , isLocalStorageSupported: (function () { return isLocalStorageSupported; })()
            , MensajeError : {
                    ErrorDesconocido: 'Error al procesar la petición'
            }

            //Classes  
            , buttonDangerClass   : "btn-sm btn-danger"
            , buttonSuccessClass  : "btn btn-sm btn-success"
            , buttonInfoClass     : "btn btn-sm btn-primary"
            , buttonWarningClass  : "btn btn-sm btn-warning"
            , buttonPreviousClass : "btn-prev btn btn-sm"
            , buttonNextClass     : "btn-next btn btn-sm btn-success"  
            //Fin classes
            , etiqueta : etiqueta
            , culture  : culture
            //Función confirm
            ,confirm : confirm


            //Función para deshabilitar los elementosBootstrapDualListbox
            // options = { element : $('.dispositivo')
            //            ,disable : true }
            //
            //
            ,disableBootstrapDualListbox : function (options) {
                if (typeof options.element != 'undefined' && options.element != null) {
                    options.element.prev().find("*").not('.filter, .clear1, .clear2').not('').prop("disabled", options.disable);
                } else {
                    $(".bootstrap-duallistbox-container").find("*").not('.filter, .clear1, .clear2').prop("disabled", options.disable);
                }
            }

            //Función para ejecutar una acción mediante ajax
            //options = {
            //
            //  url                     : "Administrador/eliminarRegistro"
            //  mostrarMensajeCargando  : false
            //  mensajeAlterno          : 'Enviando mensaje ...'
            // ,parametros              : {  id             :   50
            //                              ,cdDocumento    :   '16971156'} 
            //  ,callback               :   function(data){alert('Hello World')}
            //
            //}

            ,executeActionCall : function (options) {
            var utilSAI = this;
            var inMostrarCargando = options.mostrarMensajeCargando && typeof options.mostrarMensajeCargando == 'boolean' ? options.mostrarMensajeCargando : true
                , idLoading = inMostrarCargando ? utilSAI.notifyLoadingInfo(options.mensajeAlterno ? options.mensajeAlterno : utilSAI.etiqueta.procesando, { clickToHide: false, autoHide: false }) : null
                , paramAjax = {
                    url: options.url
                                 , cache: false
                };

            paramAjax["dataType"] = options.dataType || paramAjax["dataType"];
            options.parametros ? (paramAjax["type"] = 'POST', paramAjax["contentType"] = 'application/json, charset=utf-8', paramAjax["data"] = JSON.stringify(options.parametros))
                                : (paramAjax["type"] = 'GET');

            return $.ajax(paramAjax).done(function (data) {

                data.content = JSON.parse(data.content);

                if (idLoading)
                    utilSAI.notifyClose(idLoading);

                if (typeof data.success != 'undefined' && !data.success) {

                    if (data.content != null &&
                        typeof data.content.mensajeAdvertencia != 'undefined'
                           && data.content.mensajeAdvertencia != null && data.content.mensajeAdvertencia != '')
                        utilSAI.notifyWarning(data.content.mensajeAdvertencia);
                    else
                        utilSAI.notifyError(data.mensaje);

                } else {
                    utilSAI.notifySuccess(data.mensaje);

                    if (options.callback)
                        options.callback(data);
                }
            }).fail(
                 function (data) {
                     utilSAI.manejaErrorGenerico(idLoading, data);
                 });
            }

            //Inicializa los select usando select 2
                ,initSelect2 : function (element) {

                    if (!ace.vars['touch']) {
                        try {
                            var lan = culture.substring(0, culture.indexOf('-'));
                        } catch (err) { lan = 'en'; }
                        if (typeof element != 'undefined' && element != null) {
                            element.find('.select2').select2({
                                language: lan
                                , width: '100% '
                            });
                        } else {
                            $('.select2').select2({
                                language: lan
                                , width: '100% '
                            });
                        }
                    } else {

                        if (typeof element != 'undefined' && element != null) {
                            element.find('.select2').removeClass('select2');
                        } else {
                            $('.select2').removeClass('select2');
                        }
                    }
                }

                    // Permite inicializar una tabla datatables
                    //Recibe un objeto con los parámetros de configuración
                    //param = {
                    //           idTabla : '#idTabla'
                    //          ,parametrosBusqueda : {cdUsuario: $('#UsuarioSeleccionado').val()}
                    //          ,serverSide : true
                    //};
                    ,initTablaPrincipal : function (param) {
                        var columnsArr = [];
                        $(param.idTabla).find('thead').find('th').each(
                                     function (index) {
                                         columnsArr[index] = index == 0 ? { data: $(this).data('name'), "bSortable": false } : { data: $(this).data('name'), "defaultContent": "" };
                                     });

                        var tablaLog =
                        $(param.idTabla).DataTable({
                            language: {
                                url: $(param.idTabla).data('url-localization')
                            }
                            , ajax: {
                                url: $(param.idTabla).data('url')
                                        , type: 'POST'
                                        , data: function (dataIn) {
                                            param.parametrosBusqueda.parametro = dataIn;
                                            return param.parametrosBusqueda;
                                        }
                             , dataSrc: function (json) {

                                 if (typeof json.success != 'undefined' && !json.success)
                                     notifyError(json.mensaje);
                                 return json.data != null ? JSON.parse(json.data) : (json.content != null ? json.content : json);
                             }
                                , error: function (data) {
                                    manejaErrorGenerico(null, data);
                                }
                            }
                            , processing: true
                            , serverSide: param.serverSide
                            , bDestroy: true
                            , bJQueryUI: true
                            , aoColumns: columnsArr
                            , responsive: true
                            , bAutoWidth: false
                            , paging: true
                            , deferRender: true
                            , "iDisplayLength": 10

                        });

                    //initiate TableTools extension
                        var tableTools_obj = new $.fn.dataTable.TableTools(tablaLog, {
                            "sRowSelector": "td:not(:last-child)"
                            , "sRowSelect": "multi"
                            , "sSelectedClass": "success"
                        });
                    //we put a container before our table and append TableTools element to it
                        $(tableTools_obj.fnContainer()).appendTo($('.tableTools-container'));
                        return tablaLog;
                }

                //Devuelve menesaje error
                ,genericError : function (data) {
                        return data.responseJSON && data.responseJSON.mensaje? data.responseJSON.mensaje : (data.mensaje? data.mensaje : this.MensajeError.ErrorDesconocido);
                    }
                //Función que retorna los parámetros de configuración de bootboxlist 
                ,getDuallistboxOptions : getDuallistboxOptions
                //Función para colocar height de de scroll
                ,getPixelHeight : function (porAlto) {
                        return isNaN(porAlto) ? 0 : parseInt(parseFloat(porAlto) * screen.availHeight / 100, 10);
                }
                //Inicializa las listas duales
                ,initBootstrapDualListbox : function (element) {

                        if (typeof element != 'undefined' && element != null) {
                            element.find('.duallistbox').each(function () {
                                initElementBootstrapDualListbox(this);
                            });
                        } else {
                            $('.duallistbox').each(function () {
                                initElementBootstrapDualListbox(this);
                            });
                        }
                    }

                //Inicializa los select usando chosen
                ,initChosen : function (element) {

                    if (!ace.vars['touch']) {

                        if (typeof element != 'undefined' && element != null) {
                            element.find('.chosen-select').chosen();
                        } else {
                            $('.chosen-select').chosen();
                        }
                    } else {

                        if (typeof element != 'undefined' && element != null) {
                            element.find('.chosen-select').removeClass('chosen-select');
                        } else {
                            $('.chosen-select').removeClass('chosen-select');
                        }

                    }
                }

               ,initDate : function () {
                    $('.date-picker:not([readonly])').datepicker({
                        format: this.dateFormat
                        , language: this.culture
                        , autoclose: true
                        , todayHighlight: true
                    });


                    $('.input-daterange').datepicker({
                        format: this.dateFormat
                        , language: this.culture
                        , autoclose: true
                        , todayHighlight: true
                    });

                    $('.date-time-picker').datetimepicker({
                        dateFormat: this.dateFormat
                        , timeFormat: this.hourFormat
                        , autoclose: true
                    });
                }    

                 //Inicia el popover   
                , initPopover: initPopover 
                //Función log out
                , logOut: logOut
            

                //Ejecución inmediata de la carag del Select DualListboxAjax
                //options = Idénticas al del setSelectDualListboxAjax
                ,loadDualListboxAjax : function (form, options) {
                        var utilSAI = this;
                        var campoChange = form.find(options.changeClass).first();

                        var destino = form.find(options.destinyClass);
                        var url = options.url || campoChange.data("url");

                        destino.empty();


                        var idNoti = utilSAI.notifyLoadingInfo(utilSAI.etiqueta.cargando, { clickToHide: false, autoHide: false });

                        $.getJSON(url, options.parameters()
                        ).done(function (data) {

                            if (typeof data.success != 'undefined' && !data.success)
                                utilSAI.notifyError(data.mensaje);
                            else {
                                data.content = JSON.parse(data.content);
                                var myData = data.content;

                                $.each(myData, function (index, itemData) {
                                    destino.append($("<option/>", { value: itemData.Value, text: itemData.Text, selected: (typeof itemData.Selected != 'undefined' && itemData.Selected != null ? itemData.Selected : false) }));
                                });

                                destino.bootstrapDualListbox('refresh', true);

                                if (typeof options.callback != 'undefined' && options.callback != null)
                                    options.callback(data);
                            }
                            destino.trigger('change');
                            utilSAI.notifyClose(idNoti);
                        }).fail(function (data) {
                            utilSAI.manejaErrorGenerico(idNoti, data);
                            destinyClass.trigger('change');
                        });
                    }

                //Función ppra cargar un segmento de código mediante ajax
                //options = {
                //   url        : "Administrador/_getCrearUsuario"
                //  ,parametros : {id           : 1
                //                ,cdDocumento  : '16971156'}
                //  ,dataType   : 'JSON'
                //  ,destino    : '#divDestino'
                //  ,callback   : function(){alert('Hello World');}
                //}
                    , loadSectionAjax: function (options) {
                        var utilSAI = this;
                        var idLoading = utilSAI.notifyLoadingInfo(utilSAI.etiqueta.procesando, { clickToHide: false, autoHide: false });

                        var paramAjax = {
                            url: options.url
                            , cache: false
                        };

                        if (typeof options.dataType != 'undefined' && options.dataType != null) {
                            paramAjax["dataType"] = options.dataType;
                        }

                        if (typeof options.parametros != 'undefined' && options.parametros != null) {
                            paramAjax["type"] = 'POST';
                            paramAjax["contentType"] = 'application/json, charset=utf-8';
                            paramAjax["data"] = JSON.stringify(options.parametros);
                        } else {
                            paramAjax["type"] = 'GET';
                        }

                        $.ajax(paramAjax)
                           .done(function (content) {
                               utilSAI.notifyClose(idLoading);

                               if (typeof content.success != 'undefined' && !content.success)
                                   utilSAI.notifyError(content.mensaje);
                               else {

                                   $(options.destino).html(content);

                                   if (typeof options.callback != 'undefined' && options.callback != null)
                                       options.callback();
                               }
                           }).fail(
                            function (data) {
                                utilSAI.manejaErrorGenerico(idLoading, data);
                            });
                    }

                //Manejo de error genérico
                , manejaErrorGenerico: manejaErrorGenerico
            
                //Método para cerrar una notificación
               , notifyClose : function (id) {
                    $('.notifyjs-info-base').has("div[data-id='"+id+"']").trigger('notify-hide');
               }      
                //Método para mostrar mensaje de notificación informativo
               ,notifyLoadingInfo : function (mensaje, options) {
                var optionsLoadingInfo = { style: "info", position: 'top center', autoHide: true, clickToHide: true }

                if (options != null && typeof options != 'undefined')
                    for (var key in options) {
                        optionsLoadingInfo[key] = options[key];
                    }

                var id = $.guid++;
                $.notify({ title: " <div data-id='" + id + "'><i class='ace-icon fa fa-spinner fa-spin blue bigger-125'></i> " + mensaje + "</div>" },
                                    optionsLoadingInfo);

                return id;
            }
            //Método para mostrar mensaje de notificación éxito
            ,notifySuccess : function (mensaje, options) {
                var id = $.guid++;
                var optionsSuccess = { style: "success", position: 'top center' };
                if (options != null && typeof options != 'undefined')
                    for (var key in options) {
                        optionsSuccess[key] = options[key];
                    }

                if (typeof mensaje != 'undefined' && mensaje != null && mensaje != '')
                    $.notify({ title: "<div data-id='" + id + "'> <i class='ace-icon fa fa-check'></i> " + mensaje + "</div>" },
                                     optionsSuccess);

                return id;
            }
            //Método para mostrar mensaje de Advertencia
            ,notifyWarning : function (mensaje, options) {
                var id = $.guid++;

                var optionsWarning = { style: "warning", position: 'top center' };
                if (options != null && typeof options != 'undefined')
                    for (var key in options) {
                        optionsWarning[key] = options[key];
                    }

                $.notify({ title: "<div data-id='" + id + "'><strong>" + this.etiqueta.advertencia + "</strong> " + mensaje + "</div>" },
                                   optionsWarning);

                return id;
            }
            //Notifica un error con Json
            ,notifyErrorJson : function (data, options) {
                this.notifyError(this.genericError(data), options);
            }
            //Método para mostrar mensaje de notificación de fallo
            ,notifyError : notifyError

            //función para abrir una ventana modal
            //Parámetros
            //options = {
            //   idTitulo   :   "#divTabEditarUsuario"
            //  ,url        : "Administrador/_getCrearUsuario"
            //  ,parametros : {id           : 1
            //                 cdDocumento  : '16971156'}
            //  ,buttons    : [{ id        : "#grabar" 
            //                class     : utilSAI.buttonSuccessClass
            //                callback  : function(){
            //                      $(this.id).click();
            //                      return false;
            //                    };  
            //              }
            //              ,{ id           : "#cerrar" 
            //                class         :  utilSAI.buttonDangerClass
            //                isCloseButton : true
            //              }         
            //              ]
            //  ,callbackOnShow : asociaEventosShowModalEditarRegistro
            //  , closeOnEscape  : false
            //}
            ,openModal : function (options) {
                var utilSAI = this;
                var idLoading = utilSAI.notifyLoadingInfo(utilSAI.etiqueta.procesando, { clickToHide: false, autoHide: false });

                var paramAjax = {
                    url: options.url
                , cache: false
                };

                if (typeof options.dataType != 'undefined' && options.dataType != null) {
                    paramAjax["dataType"] = options.dataType;
                }

                if (typeof options.parametros != 'undefined' && options.parametros != null) {
                    paramAjax["type"] = 'POST';
                    paramAjax["contentType"] = 'application/json, charset=utf-8';
                    paramAjax["data"] = JSON.stringify(options.parametros);
                } else {
                    paramAjax["type"] = 'GET';
                }

                $.ajax(paramAjax)
                    .done(function (content) {
                        utilSAI.notifyClose(idLoading);
                        if (typeof content.success != 'undefined' && !content.success) {
                            utilSAI.notifyError(content.mensaje);
                        } else {
                            var botones = {};

                            //Se asignan los botones
                            var contenido = $(content);


                            for (var botonKey in options.buttons) {
                                var opcionBoton = options.buttons[botonKey];
                                var boton = contenido.find(opcionBoton.id) != null && contenido.find(opcionBoton.id).length ? contenido.find(opcionBoton.id) : contenido.filter(opcionBoton.id);
                                //Aceptar
                                if (boton.length) {
                                    if (typeof opcionBoton.isCloseButton == 'undefined' || typeof opcionBoton.isCloseButton == null)
                                        opcionBoton["isCloseButton"] = false;

                                    if (opcionBoton.isCloseButton) {
                                        botones["cancel"] = {
                                            className: options.buttons[botonKey].class
                                            , callback: options.buttons[botonKey].callback
                                            , label: boton.html()
                                        };

                                    } else {
                                        botones[boton.html()] = {
                                            className: options.buttons[botonKey].class
                                            , callback: options.buttons[botonKey].callback
                                            , label: boton.html()
                                        };
                                    }
                                }
                            }

                            var titulo = contenido.filter(options.idTitulo).data('titulo');
                            var optionsModal = {
                                title: titulo
                                         , size: "large"
                                         , message: content
                                         , buttons: botones
                            };

                            if (typeof options.closeOnEscape == 'undefined' || options.closeOnEscape == null)
                                options["closeOnEscape"] = true;

                            if (options.closeOnEscape)
                                optionsModal["onEscape"] = function () { };

                            var dialog = bootbox.dialog(optionsModal);

                            if (typeof options.callbackOnShow != 'undefined' && options.callbackOnShow != null)
                                $(dialog).on("shown.bs.modal", options.callbackOnShow);

                        }
                    }).fail(
                  function (data) {
                      utilSAI.manejaErrorGenerico(idLoading, data);
                  });
            }

             //Función para hacer los select chosen medio responsive
            , resizeChosen: resizeChosen

            //Reinicia las validaciones de tipo unobtrusive en un formulario
            ,resetValidations : function (form) {
                if (form != null) {
                    form.removeData("validator")
                        .removeData("unobtrusiveValidation");

                    $.validator.unobtrusive.parse(form);
                } else {
                    $('form').removeData("validator") // added by the raw jquery.validate plugin 
                             .removeData("unobtrusiveValidation");  // added by the jquery unobtrusive plugin 

                    $.validator.unobtrusive.parse(form);
                }
            }
             // Coloca scroll de los div
             ,setRightScroll : function () {
                    if (!ace.vars['touch']) {
                        var obtenAlto = this.getPixelHeight;
                        $('.scrollable').each(function () {
                            var $this = $(this);
                            $this.attr('data-size', obtenAlto($this.data('size')));
                            $this.ace_scroll({
                                styleClass: 'scroll-margin  scroll-blue'
                            });
                            $this.ace_scroll('modify');
                        });
                    }
             }

            //Relaciona los select mediante ajax
            // Parámetros:
            //{ changeClass  : '.municipio'                                                        -> Clase del select onchange
            // ,destinyClass : '.corregimiento'                                                    -> clase del select a cargar
            // ,parameters   : function(){return  {cdPais : 1, cdProvincia : 2, cdMunicipio:4 }}   -> Parametro de entrada de la acción
            // ,isMandatory  : true                                                                -> Si el select a cargar tiene un solo valor, se carga ese valor
            // ,callback     : function(data){alert('hola mundo');}                                -> función a llamar cuando la data se ha cargado
            //}
            ,setSelectAjax : function (form, options) {

                    if (typeof options.isMandatory == 'undefined' || options.isMandatory == null)
                        options.isMandatory = true;
                    var utilSAI = this;
                    //Cambio de municipio
                    form.find(options.changeClass).on('change', function () {
                       
                        var campoChange = $(this);

                        var destino = form.find(options.destinyClass);

                        var primeraOpcion = destino.find(':nth-child(1)');
                        destino.empty();
                        destino.append($("<option/>", { value: primeraOpcion.val(), text: primeraOpcion.text() }));

                        var idNoti = utilSAI.notifyLoadingInfo(utilSAI.etiqueta.cargando, { clickToHide: false, autoHide: false });

                        $.getJSON(campoChange.data("url"), options.parameters()
                        ).done(function (data) {

                            if (typeof data.success != 'undefined' && !data.success)
                                utilSAI.notifyError(data.mensaje);
                            else {
                                data.content = JSON.parse(data.content);
                                var myData = data.content;

                                $.each(myData, function (index, itemData) {
                                    destino.append($("<option/>", { value: itemData.Value, text: itemData.Text }));
                                });
                                if (options.isMandatory) {
                                    if (destino.children('option').length == 2) {
                                        destino.find(':nth-child(2)').prop('selected', true);
                                    }
                                }

                                if (typeof options.callback != 'undefined' && options.callback != null)
                                    options.callback(data);
                            }
                            destino.trigger('change');
                            utilSAI.notifyClose(idNoti);
                        }).fail(function (data) {
                            utilSAI.manejaErrorGenerico(idNoti, data);
                            destinyClass.trigger('change');
                        });
                    });
             }


            //Relaciona los select tDualListbox mediante ajax
            // Parámetros:
            //{ changeClass  : '.municipio'                                                             -> Clase del select onchange
            // ,destinyClass : '.corregimiento'                                                         -> clase del select a cargar
            // ,parameters   : function(){return  {cdPais : 1, cdProvincia : 2, cdMunicipio:4 }}   -> Parametro de entrada de la acción
            // ,callback     : function(data){alert('hola mundo');}                                -> función a llamar cuando la data se ha cargado
            // , url         : url a invocar
            //} 
            ,setSelectDualListboxAjax : function (form, options) {
                        var utilSAI = this;
                    //Cambio de municipio
                    form.find(options.changeClass).on('change', function () {
                        utilSAI.loadDualListboxAjax(form, options);
                    });
                }

            //Iniciliza los elementos typeahead
            // Parámetros:
            //{ fieldClass  : '.vin_vehiculo'                           -> Clase del campo
            // ,parameters   : function(){return  {IdCliente : 1 }}     -> Parametro de entrada de la acción
            // ,showLoadingMessage : false                              -> Valor por defecto = false
            //}
            ,setTypeahead : function (form, options) {

                var campo = typeof form != 'undefined' && form != null ? form.find(options.fieldClass) : $(options.fieldClass);

                var inMostrarCargando = typeof options.mostrarMensajeCargando == 'boolean' && options.mostrarMensajeCargando != null ? options.mostrarMensajeCargando : false;

                var idLoading = inMostrarCargando ? this.notifyLoadingInfo(this.etiqueta.procesando, { clickToHide: false, autoHide: false }) : null;

                var resultados = {};
                var utilSAI = this;
                $.getJSON(campo.data("url"), options.parameters()
                   ).done(function (data) {

                       if (idLoading)
                           utilSAI.notifyClose(idLoading);

                       if (typeof data.success != 'undefined' && !data.success)
                           utilSAI.notifyError(data.mensaje);
                       else {

                           data.content = JSON.parse(data.content);
                           var myData = data.content;
                           $.each(myData, function (index, itemData) {
                               resultados[index] = { value: itemData.Value, text: itemData.Text };
                           });

                       }
                   }).fail(function (data) {
                       utilSAI.manejaErrorGenerico(idNoti, data);
                   });

                campo.typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    displayKey: 'value',
                    source: utilSAI.substringMatcher(resultados),
                    templates: {
                        suggestion: function (result) {
                            return result.text;
                        }
                    }
                });
            }

            //example taken from plugin's page at: https://twitter.github.io/typeahead.js/examples/
            ,substringMatcher : function (strs) {
                return function findMatches(q, cb) {
                    var matches, substringRegex;

                    // an array that will be populated with substring matches
                    matches = [];

                    // regex used to determine if a string contains the substring `q`
                    substrRegex = new RegExp(q, 'i');

                    // iterate through the pool of strings and for any string that
                    // contains the substring `q`, add it to the `matches` array
                    $.each(strs, function (i, str) {
                        if (substrRegex.test(str.text)) {
                            // the typeahead jQuery plugin expects suggestions to a
                            // JavaScript object, refer to typeahead docs for more info
                            matches.push({ value: str.value, text: str.text });
                        }
                    });

                    cb(matches);
                }
            }            
        };
})(window);






