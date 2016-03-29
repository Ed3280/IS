(function (window, undefined) {

    var idTablaLog          = '#tableLog';
    var iLoadingTablaLog    = '#iLoadingTablaLog';

    var adminLogTransaccionesSAI = function () { }

    $(document).ready(function () {
        iniLogTable();
        utilSAI.initSelect2();
        utilSAI.initDate();
        asociarEventos();
    });

    ///Fucnión para asociar los eventos
    function asociarEventos() {
        //
        $('#btnConsultarLog').on('click', function () {
            recargarTablaTicket();
        });
        asociaEventoConsultaDetalleLog();
    }

    //Asocia el evento de abri la ventana de log
    function asociaEventoConsultaDetalleLog() {
        $(idTablaLog).on('click', '.icono-consultar', function () {
            var icono = $(this);
            var id = icono.data('log-id');
            abrirventanaDetalleLog(id);
        });

    }

    //Abre la ventana de log
    function abrirventanaDetalleLog(id) {
        var idLoading = utilSAI.notifyLoadingInfo("Procesando ...", { clickToHide: false, autoHide: false });
        var urlConsulta = $(idTablaLog).data('url-consulta-detalle');

        $.ajax(
            {
                url: urlConsulta + '/' + id
                , cache: false
                , type: 'GET'
                ,
            }
            ).done(function (content) {
                utilSAI.notifyClose(idLoading);

                if (typeof content.success != 'undefined' && !content.success)
                    utilSAI.notifyError(content.mensaje);
                else {
                    var dialog = bootbox.dialog({
                                                    title: "Registro de Log"
                                                , size: "large"
                                                , message: content
                                                , onEscape: function () { }
                                                , buttons: {
                                                 cancel: {
                                                     label: "Cerrar"
                                                         , className: "btn-sm btn-danger",
                                                             }
                                                         }
                                                    });

                    $(dialog).on("shown.bs.modal", function () { utilSAI.setRightScroll(); });
                }

            }).fail(function (data) {
                utilSAI.manejaErrorGenerico(idLoading, data);
            });
    }


    //inicializa la tabla de log de transacciones
    function iniLogTable() {
        var columnsArr = [];
        $(idTablaLog).find('thead').find('th').each(
                 function (index) {
                     columnsArr[index] = index == 0 ? { data: $(this).data('name'), "bSortable": false } : { data: $(this).data('name'), "defaultContent": "" };
                 });
        
       
        
        var tablaLog =
        $(idTablaLog).DataTable({
            language: {
                url: $(idTablaLog).data('url-localization')
            }
            , ajax: {
                        url: $(idTablaLog).data('url')                      
                        , type: 'POST'
                        , data: function (dataIn) {
                            return {
                                 cdUsuario: $('#UsuarioSeleccionado').val()
                                , feDesde: $('#FechaBusquedaDesde').val()
                                , feHasta: $('#FechaBusquedaHasta').val()
                                , cdNivel: $('#NivelSeleccionado').val()
                                , parametro: dataIn
                            };
                        }
             ,   dataSrc: function(json){

                 if (typeof json.success != 'undefined' && !json.success)
                     utilSAI.notifyError(json.mensaje);

                 return json.data!=null?JSON.parse(json.data):json.content;
                }              
                , error: function (data) {                   
                    utilSAI.manejaErrorGenerico(null, data);                    
                }
            }
           ,processing : true
             , serverSide: true
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
    
    //Recarga la tabla de tickets
    function recargarTablaTicket() {
        $(idTablaLog).DataTable().ajax.reload();
    }   

    window.adminLogTransaccionesSAI = adminLogTransaccionesSAI;

})(window);