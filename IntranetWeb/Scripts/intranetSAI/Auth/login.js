(function(window, undefined) {

    $(document).ready(function () {
        $(document).on('click', '.toolbar a[data-target]', function (e) {
            e.preventDefault();
            var target = $(this).data('target');
            $('.widget-box.visible').removeClass('visible');//hide others
            $(target).addClass('visible');//show target
        });


        $("#UserName").on('blur', function (e) {
            $("#UserNameRecuperacionHidden").val($(this).val());
        });

        $("#btnIngresar").on("click", function () {           
            $("#UserName").blur();
            return true;
        });

    });

})(window);