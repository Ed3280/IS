(
 function (window, undefined) {
     ticketUsuarioSAI = function () { }
         
     
     var idWidgetPrincipal = '#widgetTicketUsuario';
     
     var placeholder = $('#piechar-ticket-usuario-placeholder').css({ 'width': '90%', 'min-height': '150px' });

         function drawPieChart(placeholder, isOnload , position) {
             if (placeholder.length) {
                 var idLoading = !isOnload ? utilSAI.notifyLoadingInfo(utilSAI.etiqueta.procesando, { clickToHide: false, autoHide: false }) : 0;


                 $.ajax({
                     url: $(idWidgetPrincipal).data('url') + '?frecuencia=' + $(idWidgetPrincipal).find('.btn-primary').data('value')
                    , cache: false
                    , type: 'GET'
                    , contentType: 'application/json, charset=utf-8'

                 }).done(function (data) {
                     utilSAI.notifyClose(idLoading);

                     if (typeof data.success != 'undefined' && !data.success) {
                         utilSAI.notifyError(data.mensaje);

                     } else {
                         data.content = JSON.parse(data.content);
                         $.plot(placeholder, data.content, {
                             series: {
                                 pie: {
                                     show: true,
                                     tilt: 0.8,
                                     highlight: {
                                         opacity: 0.25
                                     },
                                     stroke: {
                                         color: '#fff',
                                         width: 2
                                     },
                                     startAngle: 2
                                 }
                             },
                             legend: {
                                 show: true,
                                 position: position || "ne",
                                 labelBoxBorderColor: null,
                                 margin: [-30, 15]
                             }
                    ,
                             grid: {
                                 hoverable: true,
                                 clickable: true
                             }

                         });

                     }
                 }).fail(
              function (data) {
                  utilSAI.manejaErrorGenerico(idLoading, data);
              });
             }
         }   


         ticketUsuarioSAI.initWidgetTicketChart = function () {         
             asocioEventoBusqueda();
             asociaEventoCambioSelectFrecuencia();
             drawPieChart(placeholder, true);
        }

         var $tooltip = $("<div class='tooltip top in'><div class='tooltip-inner'></div></div>").hide().appendTo('body');
         var previousPoint = null;

         placeholder.on('plothover', function (event, pos, item) {
             if (item) {
                 if (previousPoint != item.seriesIndex) {
                     previousPoint = item.seriesIndex;
                     var tip = item.series['label'] + " : " + item.series['percent'] + '%';
                     $tooltip.show().children(0).text(tip);
                 }
                 $tooltip.css({ top: pos.pageY + 10, left: pos.pageX + 10 });
             } else {
                 $tooltip.hide();
                 previousPoint = null;
             }
         });
     
        //Coloca el evento de cambio de select 
         function asociaEventoCambioSelectFrecuencia() {

             $(idWidgetPrincipal).on('click', '.dropdown-menu a',
                 function () {

                     var aClick = $(this);

                     //Se elimina la clase active y el color azul
                     $(idWidgetPrincipal).find('.dropdown-menu .active i').addClass('invisible');
                     $(idWidgetPrincipal).find('.dropdown-menu .active').removeClass('active');
                     $(idWidgetPrincipal).find('.dropdown-menu .blue').removeClass('blue');
                     
                     //Se coloca la clase active
                     aClick.parent('li').addClass('active');
                     aClick.addClass('blue');
                     aClick.find('i').removeClass('invisible');
                    
                     $(idWidgetPrincipal).find('.btn-primary').data("value", aClick.data('value'));
                     $(idWidgetPrincipal).find('.btn-primary .etiqueta').html(aClick.find('.etiqueta').html());
                     $(idWidgetPrincipal).find('.btn-primary').trigger('click');

                 });
         }


     //Ejecuta el evento on click del botón
         function asocioEventoBusqueda() {
             $(idWidgetPrincipal).on('click', '.btn-primary',
                function(){
                    drawPieChart(placeholder, false);
                }
            );
         }
              
        window.ticketUsuarioSAI = ticketUsuarioSAI;

 }
)(window);