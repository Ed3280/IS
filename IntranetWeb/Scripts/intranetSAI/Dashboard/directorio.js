(function (window, undefined) {
    
    var directorioSAI = function () { }
    var idWidgetPrincipal = '#widgetDirectorio';
    // Función para inicializar el widget de directorio
    directorioSAI.initDirectorio = function () {
        asociaEventoBuscaEmpleado();
    }

    //Asocia evento buscar directorio de empleados
    function asociaEventoBuscaEmpleado() {

        $('#btnBuscarDirectorio').on('click', function () {
            utilSAI.loadSectionAjax({
              url: $(idWidgetPrincipal).data('url')
            , destino: '#divResultDirectorio'
            , parametros: {campoBusqueda: $('#txtQueryDirectorio').val()  }
            , callback: function () {
                utilSAI.setRightScroll();
             }
            });

        });

    }

    window.directorioSAI = directorioSAI;
     
})(window);