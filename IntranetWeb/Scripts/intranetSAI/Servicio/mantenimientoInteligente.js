(function (window, undefined) {

    var idTablaPrincipal = '#tablaSolicitudServicio';
    var mantenimientoInteligenteSAI = function () { };
//Se inicializa la tabla 
$(document).ready(
    function () {
      
        initTablaPrincipal();
        asociaEventos();
        var nuSegundosLlamado = 30;
        window.setInterval(function () {
            recargarTablaPrincipal();
        }, nuSegundosLlamado * 1000);
    }
 );


    //Inicializa la tabla de Parámetro Configuración
function initTablaPrincipal() {
    utilSAI.initTablaPrincipal({
        idTabla: idTablaPrincipal
    , parametrosBusqueda: {}
    , serverSide: false
    });

}

function asociaEventos() {
    eventoConsultarRegistro();
    
}


    //Evento para consultar un registro
function eventoConsultarRegistro() {

    $(idTablaPrincipal).on('click', '.icono-consultar', function () {
        var icono = $(this);
        //Se abre la ventana
        mantenimientoInteligenteSAI.ConsultarRegistro(icono.data('id'));
    });
}


    //Función para consultar un usuario
mantenimientoInteligenteSAI.ConsultarRegistro = function (id) {

    utilSAI.openModal({
        idTitulo: '#divTabConsultarSolicitudMantenimiento'
  , url: $(idTablaPrincipal).data('url-consultar') + '/' + id
  , buttons: [{
      id: '#cerrar'
                  , class: utilSAI.buttonDangerClass
                  , isCloseButton: true
  }
  ]
    , callbackOnShow: function () {
        utilSAI.setRightScroll();///   asociaEventosShowModalConsultarRegistro();
    }
    });
}


function recargarTablaPrincipal() {    
    $(idTablaPrincipal).DataTable().ajax.reload();
}

window.mantenimientoInteligenteSAI = mantenimientoInteligenteSAI;

})(window);