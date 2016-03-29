(
    function (window, undefined) {

        cumpleanoSAI = function () { }
        var idWidgetPrincipal = '#widgetCumpleano';
        //Asocia el evento de al hacer click en el botón de ver todo, muestre todos los cumpleañeros
        cumpleanoSAI.initEventoMostrarTodo = function () {
           
            $('#btnVerCumpleanos').on('click', function () {
                mostrarCumpleaneros();
            });
        }

        //Función para mostrar los cumpleañeros
        function mostrarCumpleaneros() {
            $(idWidgetPrincipal).find('.memberdiv').removeClass('hidden');
            $('#divBotonCumpleanos').remove();
        }


        window.cumpleanoSAI = cumpleanoSAI;
    }
)(window);