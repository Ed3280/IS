(function (window, undefined) {

    var idTablaRespuestaGCM = '#tableRespuestaOperadorGCM';

    var respuestaOperadorGCMSAI = function () { }


    //Se inicializa la tabla de respuestas GCM
    $(document).ready(
        function () {
            iniRespuestaTable();
            asociaEventos();
        }
     );

    //Asocia los eventos necesarios a la página
    function asociaEventos() {
        eventoCrearRespuesta();
        eventoEditarRespuesta();
    }

    //Evento para crear una nueva respuesta
    function eventoCrearRespuesta() {
        $('.icono-crear-respuesta').on('click', function () {
            //Se abre la ventana
            respuestaOperadorGCMSAI.crearRespuesta();
        });
    }

    //Evento para crear una nueva respuesta
    function eventoEditarRespuesta() {
        $(idTablaRespuestaGCM).on('click', '.icono-editar', function () {
            var icono = $(this);
            //Se abre la ventana
            respuestaOperadorGCMSAI.editarRespuesta(icono.data('id-respuesta'));
        });
    }


    //Crea una nueva respuesta GCM
    respuestaOperadorGCMSAI.crearRespuesta = function () {
        var tablaRespuestaGCM = $(idTablaRespuestaGCM);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });

        $.ajax({
            url: tablaRespuestaGCM.data('url-crear')
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


                var titulo = contenido.find('#widgetBoxRespuestaGCM').data('titulo'); 
                var options = {
                    title: titulo
                             , size: "large"
                             , message: content
                             , buttons: buttonTicket
                             , onEscape: function() {}
                };
                var dialog = bootbox.dialog(options);

                $(dialog).on("shown.bs.modal", function () {
                    utilSAI.initSelect2();
                    utilSAI.setRightScroll();

                    var idFormularioCrearRespuesta = '#formCrearRespuesta';
                    var formulario = $(idFormularioCrearRespuesta);
                    //Asociar eventos ajax al formulario
                    $.validator.unobtrusive.parse(idFormularioCrearRespuesta);

                });
            }
        }).fail(
           function (data) {
               utilSAI.manejaErrorGenerico(idLoading, data);
           });
    }



    //Crea una nueva respuesta GCM
    respuestaOperadorGCMSAI.editarRespuesta = function (id) {
        var tablaRespuestaGCM = $(idTablaRespuestaGCM);

        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });
        
        $.ajax({
            url: tablaRespuestaGCM.data('url-editar')+'/'+id
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


                var titulo = contenido.find('#widgetBoxRespuestaGCM').data('titulo');
                var options = {
                    title: titulo
                             , size: "large"
                             , message: content
                             , buttons: buttonTicket
                             , onEscape: function () { }
                };
                var dialog = bootbox.dialog(options);

                $(dialog).on("shown.bs.modal", function () {
                    utilSAI.initSelect2();
                    utilSAI.setRightScroll();

                    var idFormularioCrearRespuesta = '#formEditarRespuesta';
                    var formulario = $(idFormularioCrearRespuesta);
                    //Asociar eventos ajax al formulario
                    $.validator.unobtrusive.parse(idFormularioCrearRespuesta);

                });
            }
        }).fail(
           function (data) {
               utilSAI.manejaErrorGenerico(idLoading, data);
           });
    }


    //Inicializa la tabla de Respuestas GCM
    function iniRespuestaTable() {

        utilSAI.initTablaPrincipal({
            idTabla: idTablaRespuestaGCM
        , parametrosBusqueda: {}
        , serverSide: false
        });
    }
    //Recarga la tabla de respuestas
    function recargarTablaRespuesta() {
        $(idTablaRespuestaGCM).DataTable().ajax.reload();
    }


    //Graba las respuesta del operador gsm
    respuestaOperadorGCMSAI.submitSuccess = function (data) {
        var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
        if (content.mensajeAdvertencia != null && typeof content.mensajeAdvertencia != 'undefined' && content.mensajeAdvertencia != '')
            utilSAI.notifyWarning(content.mensajeAdvertencia);

        if (data.success) {
            utilSAI.notifySuccess(data.mensaje);
            recargarTablaRespuesta();
            bootbox.hideAll();
        } else {
            $('#resultUpdate').html(data.mensajeHtml);
        }
    }

    window.respuestaOperadorGCMSAI = respuestaOperadorGCMSAI;

})(window);