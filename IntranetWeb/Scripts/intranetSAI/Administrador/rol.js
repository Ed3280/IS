(
    function (window, undefined) {

        var rolSAI = function () { }
        var idTablaPrincipal = '#tablaPrincipalAdministracion';
        var idFormularioCrearRegistro = "#formCrearRegistro";
        var idFormularioEditarRegistro = "#formEditarRegistro";


        $(document).ready(function () {
            initTablaPrincipal();
            initAllDefault();
            asociaEventos();
        });

        //Inicializa la tabla de Respuestas GCM
        function initTablaPrincipal() {
            utilSAI.initTablaPrincipal({
                idTabla: idTablaPrincipal
            , parametrosBusqueda: {}
            , serverSide: false
            });
        }

        //Recarga la tabla principal
        rolSAI.recargarTablaPrincipal =  function() {
            $(idTablaPrincipal).DataTable().ajax.reload();
        }

        //Inicializa los elementos javascript
        function initAllDefault() {
            utilSAI.initDate();
            initNestable();
            utilSAI.setRightScroll();
        }

        //Asocia eventos a la página 
        function asociaEventos() {


            //Crear registro
            $('.icono-crear-registro').on('click',  function () {
                 rolSAI.CrearRegistro();
            });

            //Editar registro
            $(idTablaPrincipal).on('click','.icono-editar',function(){
                var icono = $(this);
                //Se abre la ventana
                rolSAI.EditarRegistro(icono.data('id-rol'));
            });

        }

        function initNestable() {
            
        }



        //Función para crear un nuevo registro
        rolSAI.CrearRegistro = function () {

            utilSAI.openModal({
                idTitulo: '#wizadCrearRegistro'
                  , url: $(idTablaPrincipal).data('url-crear')
                  , buttons: [
                                {
                                    id: '#btnAnterior'
                                  , class: utilSAI.buttonPreviousClass
                                  , callback: function (e) {
                                      $('#btnAnterior').trigger('click');
                                      return false;
                                  }
                                }

                      , {
                          id: '#btnSiguiente'
                                  , class: utilSAI.buttonNextClass
                                  , callback: function (e) {
                                      $('#btnSiguiente').trigger('click');
                                      return false;
                                  }
                      }
                             , {
                                 id: '#cerrar'
                                  , class: utilSAI.buttonDangerClass
                                  , isCloseButton: true
                             }
                  ]
                  , callbackOnShow: function () { asociaEventosShowModalCrearRegistro(); }
            });
        }



        //Función para crear un nuevo registro
        rolSAI.EditarRegistro = function (id) {
            console.log($(idTablaPrincipal).data('url-editar') + '/' + id, 'mensaj click');


            utilSAI.openModal({
                idTitulo: '#divTabEditarRol'
                  , url: $(idTablaPrincipal).data('url-editar') + '/' + id
                  , buttons: [
                                {
                                    id: '#grabar'
                                  , class: utilSAI.buttonSuccessClass
                                  , callback: function (e) {
                                      $('#grabar').trigger('click');
                                      return false;
                                  }
                                }
                             , {
                                 id: '#cerrar'
                                  , class: utilSAI.buttonDangerClass
                                  , isCloseButton: true
                             }
                  ]
                  , callbackOnShow: function () { asociaEventosShowModalEditarRegistro(); }
            });
        }


        //Eventos luego de mostrado el modal de crear registro
        function asociaEventosShowModalCrearRegistro() {
            initAllDefault();
            var formulario = $(idFormularioCrearRegistro);
            $("#divAdvertenciaMenu").hide();
            //Asociar eventos ajax al formulario                   
            $.validator.unobtrusive.parse(idFormularioCrearRegistro);

            $('#wizadCrearRegistro')
                .ace_wizard({}).on('actionclicked.fu.wizard', function (e) {
                    if (!$(idFormularioCrearRegistro).valid()) e.preventDefault();
                }).on('finished.fu.wizard', function (e) {
                    $(idFormularioCrearRegistro).submit();
                });
        }


        //Eventos luego de mostrar la ventana de editar registro
        function asociaEventosShowModalEditarRegistro() {
            initAllDefault();
            var formulario = $(idFormularioEditarRegistro);
            $("#divAdvertenciaMenu").hide();
            //Asociar eventos ajax al formulario                   
            $.validator.unobtrusive.parse(idFormularioEditarRegistro);
        }


        //Graba registros
        rolSAI.submitSuccess = function (data) {
            var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
            if (content.mensajeAdvertencia != null && typeof content.mensajeAdvertencia != 'undefined' && content.mensajeAdvertencia != '')
                utilSAI.notifyWarning(content.mensajeAdvertencia);

            if (data.success) {
                utilSAI.notifySuccess(data.mensaje);
                rolSAI.recargarTablaPrincipal();
                bootbox.hideAll();
            } else {
                $('#resultUpdate').html(data.mensajeHtml);
            }
        }


        window.rolSAI = rolSAI;

})(window);