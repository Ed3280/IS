(function (window, undefined) {

    var verKilometrajeSAI = function () { };
    
    $(document).ready(function () {
        //Se inicializa chosen
       
        utilSAI.initSelect2();
        
        //Incializa la tabla de Kilometraje
        var table = initTablaKilometraje();

        //Se actualizagit  la información cada 60 segundos
        var nuSegundosLlamado = 60;
        window.setInterval(function () {
            $("#iLoadingTablaKilometraje").toggle(500);
            $('#dynamic-table').DataTable().ajax.reload();

        }, nuSegundosLlamado * 1000);

        
        $('#usuarioId').on("change", function () {
            table.draw();
        });

        $('#dynamic-table').on('click', '.icono-editar', function () {
            var iconoEditar = $(this);

            var userId = iconoEditar.data('user-id');
            var deviceId = iconoEditar.data('device-id');
            actualizaKilometrajeIni(userId, deviceId);
        });

    });


    


    function initTablaKilometraje() {

        var urlLanguage = $('#divLocalizacion');
        var urlAjax = $('#divVehiculoKilometraje');
        var inNoEditar = $('#IndicadorNoEdicion');
        
        $("#iLoadingTablaKilometraje").toggle(500);
        //initiate dataTables plugin
        var oTable1 =
        $('#dynamic-table').DataTable({
            language: {
                url: urlLanguage.data('url')
            }
            , ajax: {
                url: urlAjax.data('url') + '?IndicadorNoEdicion=' + inNoEditar.val()
                , dataSrc: function (data) {
                    
                    $("#iLoadingTablaKilometraje").toggle(500);                    
                    if (typeof data.success != 'undefined' && !data.success)
                        utilSAI.notifyError(data.mensaje);
                    data.content = JSON.parse(data.content);
                    return data.content != null ? data.content : data;
                }
                ,error: function (data) {
                    $("#iLoadingTablaKilometraje").toggle(500);
                    utilSAI.manejaErrorGenerico(null, data);
                }
            }
            , aoColumns: [
                          { data: 'EditarRegistro', "bSortable": false }
                        , { data: 'IdUsuario' }
                        , { data: 'NombreUsuario' }
                        , { data: 'IdVehiculo' }
                        , { data: 'NombreVehiculo' }
                        , { data: 'DistanciaInicial' }
                        , { data: 'DistanciaRecorrida' }
                        
            ]
            , "iDisplayLength": 10
            , bAutoWidth: false
            , responsive: true
        });



        //TableTools settings
        TableTools.classes.container = "btn-group btn-overlap";
        TableTools.classes.print = {
            "body": "DTTT_Print",
            "info": "tableTools-alert gritter-item-wrapper gritter-info gritter-center white",
            "message": "tableTools-print-navbar"
        }

        //initiate TableTools extension
        var tableTools_obj = new $.fn.dataTable.TableTools(oTable1, {
            "sSwfPath": "dist/js/dataTables/extensions/TableTools/swf/copy_csv_xls_pdf.swf", //in Ace demo dist will be replaced by correct assets path

            "sRowSelector": "td:not(:last-child)",
            "sRowSelect": "multi",
            "fnRowSelected": function (row) {
                //check checkbox when row is selected
                try { $(row).find('input[type=checkbox]').get(0).checked = true }
                catch (e) { }
            },
            "fnRowDeselected": function (row) {
                //uncheck checkbox
                try { $(row).find('input[type=checkbox]').get(0).checked = false }
                catch (e) { }
            }
            , "sSelectedClass": "success"
        });
        //we put a container before our table and append TableTools element to it
        $(tableTools_obj.fnContainer()).appendTo($('.tableTools-container'));

        //also add tooltips to table tools buttons
        //addding tooltips directly to "A" buttons results in buttons disappearing (weired! don't know why!)
        //so we add tooltips to the "DIV" child after it becomes inserted
        //flash objects inside table tools buttons are inserted with some delay (100ms) (for some reason)
        setTimeout(function () {
            $(tableTools_obj.fnContainer()).find('a.DTTT_button').each(function () {
                var div = $(this).find('> div');
                if (div.length > 0) div.tooltip({ container: 'body' });
                else $(this).tooltip({ container: 'body' });
            });
        }, 200);



        //ColVis extension
        var colvis = new $.fn.dataTable.ColVis(oTable1, {
            "buttonText": "<i class='fa fa-search'></i>",
            "aiExclude": [0, 6],
            "bShowAll": true,
            //"bRestore": true,
            "sAlign": "right",
            "fnLabel": function (i, title, th) {
                return $(th).text();//remove icons, etc
            }
        });

        //style it
        $(colvis.button()).addClass('btn-group').find('button').addClass('btn btn-white btn-info btn-bold')

        //and append it to our table tools btn-group, also add tooltip
        $(colvis.button())
        .prependTo('.tableTools-container .btn-group')
        .attr('title', 'Show/hide columns').tooltip({ container: 'body' });

        //and make the list, buttons and checkboxed Ace-like
        $(colvis.dom.collection)
        .addClass('dropdown-menu dropdown-light dropdown-caret dropdown-caret-right')
        .find('li').wrapInner('<a href="javascript:void(0)" />') //'A' tag is required for better styling
        .find('input[type=checkbox]').addClass('ace').next().addClass('lbl padding-8');



        /////////////////////////////////
        //table checkboxes
        $('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);

        //select/deselect all rows according to table header checkbox
        $('#dynamic-table > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
            var th_checked = this.checked;//checkbox inside "TH" table header

            $(this).closest('table').find('tbody > tr').each(function () {
                var row = this;
                if (th_checked) tableTools_obj.fnSelect(row);
                else tableTools_obj.fnDeselect(row);
            });
        });

        //select/deselect a row when the checkbox is checked/unchecked
        $('#dynamic-table').on('click', 'td input[type=checkbox]', function () {
            var row = $(this).closest('tr').get(0);
            if (!this.checked) tableTools_obj.fnSelect(row);
            else tableTools_obj.fnDeselect($(this).closest('tr').get(0));
        });

        $(document).on('click', '#dynamic-table .dropdown-toggle', function (e) {
            e.stopImmediatePropagation();
            e.stopPropagation();
            e.preventDefault();
        });


        //And for the first simple table, which doesn't have TableTools or dataTables
        //select/deselect all rows according to table header checkbox
        var active_class = 'active';
        $('#simple-table > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
            var th_checked = this.checked;//checkbox inside "TH" table header

            $(this).closest('table').find('tbody > tr').each(function () {
                var row = this;
                if (th_checked) $(row).addClass(active_class).find('input[type=checkbox]').eq(0).prop('checked', true);
                else $(row).removeClass(active_class).find('input[type=checkbox]').eq(0).prop('checked', false);
            });
        });

        //select/deselect a row when the checkbox is checked/unchecked
        $('#simple-table').on('click', 'td input[type=checkbox]', function () {
            var $row = $(this).closest('tr');
            if (this.checked) $row.addClass(active_class);
            else $row.removeClass(active_class);
        });



        /********************************/
        //add tooltip for small view action buttons in dropdown menu
        $('[data-rel="tooltip"]').tooltip({ placement: tooltip_placement });

        //tooltip placement on right or left
        function tooltip_placement(context, source) {
            var $source = $(source);
            var $parent = $source.closest('table')
            var off1 = $parent.offset();
            var w1 = $parent.width();
            var off2 = $source.offset();
            if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
            return 'left';
        }


        

        //Se le le coloca el filtro custom
        $.fn.dataTable.ext.search.push(
            function (settings, data, dataIndex) {
                var select = $('#usuarioId'); 
                var idUsuarioSelect = parseInt(select.val()==='' ? -1 : select.val(), 10);
                var idDatatable = parseInt(data[1],10) || 0; // use data for the age column
                if (idUsuarioSelect===-1||idUsuarioSelect === idDatatable) {
                    return true;
                }
                return false;
            }
        );


        return oTable1;


    }

    
    //Muestra la ventana para actualizar el kilomentraje inicial
    function actualizaKilometrajeIni(userId, deviceId) {

        var divEditar = $("#divEditarKilometrajeInicial");
        var notiId = utilSAI.notifyLoadingInfo("Cargando ...");
        $.ajax({
            url: divEditar.data("url") + '?userId=' + userId + "&deviceId=" + deviceId
            , cache: false
            , type: "GET"            
        }).done(function (data) {
            utilSAI.notifyClose(notiId);

            if (typeof data.success != 'undefined' && !data.success) {
                utilSAI.notifyError(data.mensaje);
            } else {
                bootbox.dialog({
                    title: "Editar Kilometraje Inicial"
                    , message: data
                    , buttons: {
                        success: {
                            label: "Guardar"
                            , className: "btn-sm btn-success"
                            , callback: function () {
                                actualizaKilometraje(divEditar.data("url-editar")
                                , userId
                                , deviceId
                                , $('#kilometrajeInicial').val())

                            }
                        }
                        , danger:
                        {
                            label: "Cerrar"
                            , className: "btn-sm btn-danger"
                            , callback: function () {
                                bootbox.hideAll()
                            }
                        }
                    }
                }
               );
            }
            
        })
          .fail(function (data) {
              utilSAI.manejaErrorGenerico(notiId, data);
          });
       }

    //Función para actualizar el kilometraje inicial
     function actualizaKilometraje(url
                            , userId
                            , deviceId
                            , kilometrajeInicial) {
         var noti = utilSAI.notifyLoadingInfo("Actualizando...");
         var vehiculoData;
        var tabla = $('#dynamic-table');
        if (deviceId && userId) {
            vehiculoData = {
                IdVehiculo: deviceId
                , IdUsuario: userId
                , DistanciaInicial: kilometrajeInicial
            };

            // Actualiza Kilometraje
            $.ajax({
                url: url
                , cache: false
                , data: JSON.stringify(vehiculoData, function (key, val) { return key === "source" ? '' : val; })
                , type: 'POST'
                , contentType: 'application/json, charset=utf-8'
                , dataType: 'JSON'
            })
            .done(function (data) {
                utilSAI.notifyClose(noti);                

                if (data.success) {
                    utilSAI.notifySuccess(data.mensaje);
                    tabla.DataTable().ajax.reload();
                } else
                    utilSAI.notifyError(data.mensaje);


            })
            .fail(function (data) {
                utilSAI.manejaErrorGenerico(noti, data);
                $("#iLoadingTablaKilometraje").toggle(500);
                tabla.DataTable().ajax.reload();
            });
        }
    }
    
     window.verKilometrajeSAI = verKilometrajeSAI;

})(window);


