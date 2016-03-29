var configuracionSAI = (function (window, undefined) {

    $(document).ready(
        function () {
           utilSAI.initDate();           
        });
    
    return {
        //Cuando se actualiza el perfil
        submitSuccess : function (data) {
            var content = typeof data.content != 'undefined' && data.content != null ? JSON.parse(data.content) : {};
            if (content&&content.mensajeAdvertencia && content.mensajeAdvertencia != '')
                utilSAI.notifyWarning(content.mensajeAdvertencia);

            if (data.success) {
                $('#Usuario_ContrasenaActual').val('');
            }
            $('#divUpdatePerfilTarget').html(data.mensajeHtml);
        }
    };
}
)(window);