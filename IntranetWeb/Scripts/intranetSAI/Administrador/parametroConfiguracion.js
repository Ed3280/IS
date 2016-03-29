(function (window, undefined) {

    var idTablaPrincipal = '#tableParametroConfiguracion';

    var parametroConfiguracionSAI = function () { }


    //Se inicializa la tabla 
    $(document).ready(
        function () {
            iniParametroTable();
            asociaEventos();
        }
     );

    //Asocia los eventos necesarios a la página
    function asociaEventos() {
        eventoCrearRegistro();
        eventoEditarRegistro();
        eventoEliminarRegistro();
    }

    //Evento para crear un nuevo registro
    function eventoCrearRegistro() {
        $('.icono-crear-registro').on('click', function () {
            //Se abre la ventana
            parametroConfiguracionSAI.crearRegistro();
        });
    }

    //Evento para crear un nuevo registro
    function eventoEditarRegistro() {
        $(idTablaPrincipal).on('click', '.icono-editar', function () {
            var icono = $(this);
            //Se abre la ventana
            parametroConfiguracionSAI.EditarRegistro(icono.data('nm-parametro')
                                                    ,icono.data('cd-configuracion'));
        });
    }

    //Evento para eliminar un registro
    function eventoEliminarRegistro() {
        $(idTablaPrincipal).on('click', '.icono-eliminar', function () {
            var icono = $(this);
            //Se abre la ventana
            parametroConfiguracionSAI.EliminarRegistro(icono.data('nm-parametro')
                                                    ,icono.data('cd-configuracion'));
        });    
    }

    //Crea un nuevo registro en la tabla
    parametroConfiguracionSAI.crearRegistro = function () {
        var tabla = $(idTablaPrincipal);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });

        $.ajax({
            url: tabla.data('url-crear')
            , cache: false
        }).done(function (content) {
            utilSAI.notifyClose(idLoading);

            if (typeof content.success != 'undefined' && !content.success) {
                utilSAI.notifyError(content.mensaje);
            } else {

                var buttonTicket = {};

                var contenido = $(content);
                var idGrabar = '#grabar';

                //Aceptar
                if (contenido.find(idGrabar).length) {
                    buttonTicket["Guardar"] = {
                        className: "btn btn-sm btn-success"
                        , callback: function () {
                            $(idGrabar).trigger('click');
                            return false;
                        }
                    };
                }

                buttonTicket["cancel"] = {
                    label: "Cerrar"
                    , className: "btn-sm btn-danger",
                };


                var titulo = contenido.find('#widgetBoxTabla').data('titulo');
                var options = {
                    title: titulo
                             , size: "large"
                             , message: content
                             , buttons: buttonTicket
                             , onEscape: function () { }
                };
                var dialog = bootbox.dialog(options);

                $(dialog).on("shown.bs.modal", function () {
                    utilSAI.initChosen();
                    utilSAI.resizeChosen();
                    utilSAI.setRightScroll();

                    var idFormulariocrearRegistro = '#formCrearRegistro';
                    var formulario = $(idFormulariocrearRegistro);
                    //Asociar eventos ajax al formulario
                    $.validator.unobtrusive.parse(idFormulariocrearRegistro);

                });
            }
        }).fail(
           function (data) {
               utilSAI.manejaErrorGenerico(idLoading, data);
           });
    }

    //Para eliminar un registro de la tabla 
    parametroConfiguracionSAI.EliminarRegistro = function (nmParametro, cdConfiguracion) {
            var tabla = $(idTablaPrincipal);

            utilSAI.confirm("¿Desea eliminar el registro? Podría ocasionar errores inesperados en el sistema", function () {

            var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });

            $.ajax({
                url: tabla.data('url-eliminar') + '?nmParametro=' + nmParametro + '&cdConfiguracion=' + cdConfiguracion
            , cache: false
            }).done(function (content) {

                utilSAI.notifyClose(idLoading);

                if (content.success) {
                    utilSAI.notifySuccess(content.mensaje);
                    recargarTablaPrincipal();
                } else                     
                    utilSAI.notifyError(content.mensaje);
                
                        
            }).fail(
           function (data) {
               utilSAI.manejaErrorGenerico(idLoading, data);
           });

        });

    }

    //Para editar un registro en la tabla 
    parametroConfiguracionSAI.EditarRegistro = function (nmParametro,cdConfiguracion) {
        var tabla = $(idTablaPrincipal);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });

        $.ajax({
            url: tabla.data('url-editar') + '?nmParametro=' + nmParametro + '&cdConfiguracion=' + cdConfiguracion
            , cache: false
        }).done(function (content) {
            utilSAI.notifyClose(idLoading);

            if (typeof content.success != 'undefined' && !content.success) {
                utilSAI.notifyError(content.mensaje);
            } else {

                var buttonTicket = {};

                var contenido = $(content);
                var idGrabar = '#grabar';

                //Aceptar
                if (contenido.find(idGrabar).length) {
                    buttonTicket["Guardar"] = {
                        className: "btn btn-sm btn-success"
                        , callback: function () {
                            $(idGrabar).trigger('click');
                            return false;
                        }
                    };
                }

                buttonTicket["cancel"] = {
                    label: "Cerrar"
                    , className: "btn-sm btn-danger",
                };


                var titulo = contenido.find('#widgetBoxTabla').data('titulo');
                var options = {
                    title: titulo
                             , size: "large"
                             , message: content
                             , buttons: buttonTicket
                };
                var dialog = bootbox.dialog(options);

                $(dialog).on("shown.bs.modal", function () {
                    utilSAI.initChosen();
                    utilSAI.resizeChosen();
                    utilSAI.setRightScroll();

                    var idFormulariocrearRegistro = '#formEditarRegistro';
                    var formulario = $(idFormulariocrearRegistro);
                    //Asociar eventos ajax al formulario
                    $.validator.unobtrusive.parse(idFormulariocrearRegistro);

                });
            }
        }).fail(
           function (data) {
               utilSAI.manejaErrorGenerico(idLoading, data);
           });
    }


    //Inicializa la tabla de Parámetro Configuración
    function iniParametroTable() {
        var columnsArr = [];
        $(idTablaPrincipal).find('thead').find('th').each(
                 function (index) {
                     columnsArr[index] = index == 0 ? { data: $(this).data('name'), "bSortable": false } : { data: $(this).data('name'), "defaultContent": "" };
                 });


        var tablaLog =
        $(idTablaPrincipal).DataTable({
            language: {
                url: $(idTablaPrincipal).data('url-localization')
            }
            , ajax: {
                url: $(idTablaPrincipal).data('url')
                     , type: 'POST'
                , error: function (data) {
                    utilSAI.manejaErrorGenerico(null, data);
                }
                , dataSrc: function (data) {
                    data.content = JSON.parse(data.content);

                    if (typeof data.success != 'undefined' && !data.success)
                        utilSAI.notifyError(data.mensaje);

                    return data.content != null ? JSON.parse(data.content.Content) : data;
                }
            }
            , processing: false
            , serverSide: false
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

    }
    //Recarga la tabla principal
    function recargarTablaPrincipal() {
        $(idTablaPrincipal).DataTable().ajax.reload();
    }


    //Graba registros
    parametroConfiguracionSAI.submitSuccess = function (data) {
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

    window.parametroConfiguracionSAI = parametroConfiguracionSAI;

})(window);