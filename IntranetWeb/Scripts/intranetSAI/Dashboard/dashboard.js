(function (window, undefined) {
    dashboardSAI = function () { }

    $(document).ready(function () {
        utilSAI.setRightScroll();
        initContenedores();
        cumpleanoSAI.initEventoMostrarTodo();
        directorioSAI.initDirectorio();
        ticketUsuarioSAI.initWidgetTicketChart();
    });



    //Inicializa los contenedores
    function initContenedores() {
        $('.widget-container-col').sortable({
            connectWith: '.widget-container-col',
            items: '> .widget-box',
            opacity: 0.8,
            revert: true,
            forceHelperSize: true,
            placeholder: 'widget-placeholder',
            forcePlaceholderSize: true,
            tolerance: 'pointer',
            start: function (event, ui) {
                //when an element is moved, its parent (.widget-container-col) becomes empty with almost zero height
                //we set a "min-height" for it to be large enough so that later we can easily drop elements back onto it
                ui.item.parent().css({ 'min-height': ui.item.height() })
            },
            update: function (event, ui) {
                //the previously set "min-height" is not needed anymore
                //now the parent (.widget-container-col) should take the height of its child (.wiget-box)
                ui.item.parent({ 'min-height': '' })
            }
        });
    }
    
    window.dasboardSAI = dashboardSAI;

})(window);