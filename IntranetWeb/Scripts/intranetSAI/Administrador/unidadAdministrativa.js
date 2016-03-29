(function (window, undefined) {

    var idTablaPrincipal = '#tableUnidadAdministrativa';

    var unidadAdministrativaSAI = function () { }


    //Se inicializa la tabla 
    $(document).ready(
        function () {
            initTablaPrincipal();
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
            unidadAdministrativaSAI.crearRegistro();
        });
    }

    //Evento para crear un nuevo registro
    function eventoEditarRegistro() {
        $(idTablaPrincipal).on('click', '.icono-editar', function () {
            var icono = $(this);
            //Se abre la ventana
            unidadAdministrativaSAI.EditarRegistro(icono.data('id'));
        });
    }

    //Evento para eliminar un registro
    function eventoEliminarRegistro() {
        $(idTablaPrincipal).on('click', '.icono-eliminar', function () {
            var icono = $(this);
            //Se abre la ventana
            unidadAdministrativaSAI.EliminarRegistro(icono.data('id'));
        });
    }

    //Crea un nuevo registro en la tabla
    unidadAdministrativaSAI.crearRegistro = function () {
        var tabla = $(idTablaPrincipal);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });

        $.ajax({
            url: tabla.data('url-crear')
            , cache: false
        }).done(function (content) {
            utilSAI.notifyClose(idLoading);

            if (typeof content.success != 'undefined'&& !content.success) {
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


                var titulo = contenido.find('#widgetBoxCrearRegistro').data('titulo');
                var options = {
                    title: titulo
                             , size: "large"
                             , message: content
                             , buttons: buttonTicket
                            ,  onEscape: function() {}
                };
                var dialog = bootbox.dialog(options);

                $(dialog).on("shown.bs.modal", function () {
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
    unidadAdministrativaSAI.EliminarRegistro = function (id) {
        var tabla = $(idTablaPrincipal);

        utilSAI.confirm("¿Desea eliminar el registro?", function () {

            var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });

            $.ajax({
                  url: tabla.data('url-eliminar') + '/' + id
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
    unidadAdministrativaSAI.EditarRegistro = function (id) {
        var tabla = $(idTablaPrincipal);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });

        $.ajax({
            url: tabla.data('url-editar') + '/'+id
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


                var titulo = contenido.find('#widgetBoxEditarRegistro').data('titulo');
                var options = {
                    title: titulo
                             , size: "large"
                             , message: content
                             , buttons: buttonTicket
                             , onEscape: function() {}
                };
                var dialog = bootbox.dialog(options);

                $(dialog).on("shown.bs.modal", function () {
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
    function initTablaPrincipal() {       
       
            utilSAI.initTablaPrincipal({
                idTabla: idTablaPrincipal
            , parametrosBusqueda: {}
            , serverSide: false
            });

    }
    //Recarga la tabla principal
    function recargarTablaPrincipal() {
        $(idTablaPrincipal).DataTable().ajax.reload();
    }


    //Graba registros
    unidadAdministrativaSAI.submitSuccess = function (data) {
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

    window.unidadAdministrativaSAI = unidadAdministrativaSAI;

})(window);