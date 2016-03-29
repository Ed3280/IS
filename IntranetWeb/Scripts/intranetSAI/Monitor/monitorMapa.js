(function (window, undefined) {
    var monitorMapaSAI = function () { }
    var markerPrefix = 'monitorMarkerDevice_';
    var idDivMapa = '#map_canvas';
    var zoom = 18;

    var monitorSAI = typeof monitorSAI == 'undefined' ? null : monitorSAI;

    if (monitorSAI) monitorSAI.idDivMapa = idDivMapa;

    var panamaLoc = new google.maps.LatLng(10.1952454, -80.487615);
    var map;            //Mapa
    var markers = {};   //Marcadores
    var idMarkers = []; //IdMarcadores
    var checkedMark = null;
    
    var markerCliente = null;


    var MarkerIconMove = function(){return {
             path: fontawesome.markers.CAR
            ,scale: 0.4
            ,strokeWeight: 0.2
            ,strokeColor: 'SpringGreen'
            ,strokeOpacity: 1
            ,fillColor: '#73AD21'
            ,fillOpacity: 0.3
            }
    };

    var MarkerIconStop = function(){ return  {
             path: fontawesome.markers.CAR
            ,scale: 0.4
            ,strokeWeight: 0.2
            ,strokeColor: 'red'
            ,strokeOpacity: 1
            ,fillColor: 'red'
            ,fillOpacity: 0.3
            }
    };

    MarkerIconOffLine = function () {
        return {
             path: fontawesome.markers.CAR
            ,scale: 0.4
            ,strokeWeight: 0.2
            ,strokeColor: 'gray'
            ,strokeOpacity: 1
            ,fillColor: 'gray'
            ,fillOpacity: 0.3
        }
    };

    var MarkerIconClient = function(){return {
             path: fontawesome.markers.MAP_MARKER
            ,scale: 0.4
            ,strokeWeight: 0.2
            ,strokeColor: 'Red'
            ,strokeOpacity: 1
            ,fillColor: 'Red'
            ,fillOpacity: 1
            }
    };

    //Funcion para obtener el icono de acuerdo al status
    function getIcon(iconPlataform ,status) {
        var icon;
        switch (status.toLowerCase()) {
            case 'offline':
                icon = MarkerIconOffLine();
                break;
            case 'move':
                icon = MarkerIconMove();
                break;
            case 'stop':
                icon = MarkerIconStop();
                break;
            default:
                icon = MarkerIconOffLine();
        }
        try{
            icon = setIconShape(icon, iconPlataform);
        }catch(err){console.error(err, 'No se pudo dar forma al marker');};
        return icon;
    }

    function setIconShape(icon, iconPlataform) {
            iconPlataform = typeof iconPlataform == 'undefined' || iconPlataform == null ? '' : iconPlataform;
        var iconStr = iconPlataform.replace("offline_", "");
        var iconNumber = iconStr.split('_')[0];

        switch (parseInt(iconNumber,10)) {
        
            case 21:
                icon['path'] = fontawesome.markers.TAXI;               
                break;
            case 22:
                icon['path'] = fontawesome.markers.CAR;
            break;

            case 23:
                icon['path'] = fontawesome.markers.BICYCLE;
                break;

            case 24:
                icon['path'] = fontawesome.markers.BUS;
                break;

            case 25:
            case 27:
                icon['path'] = fontawesome.markers.TRUCK;
                break;

           /* case 26:
                icon['path'] = fontawesome.markers.;
                break;
                */
        }
        return icon;
    }


    //Funcion para tradcuir los estatus 
    function traducirEstatus(status) {

        switch (status.toLowerCase()) {
            case 'offline':
                return 'Fuera de línea';
                break;
            case 'move':
                return 'En movimiento';
                break;
            case 'stop':
                return 'Detenido';
                break;
            default:
                return '';
        }
    }

    //Actualiza el color del vehículo en el widget
    function updateColorWidget(id , status , iconPlataform ) {
        try {
            var disp = $("#iconoMapaDispWid_" + id);
            setStatusColor(disp, status);            
            setIconClass(disp, iconPlataform);
            ocultaMuestraDispositivoDistribuidor(disp.closest('.dd2ItemDispositivo'),status);
        } catch (err) {
            console.error(err.message, 'Fallo al actualizar color del vehículo ' + id);
        }
    }


    function setStatusColor(icon,status) {
        icon.removeClass("red gray green");
        switch (status.toLowerCase()) {
            case 'offline':
                icon.addClass("gray");
                break;
            case 'move':
                icon.addClass("green");
                break;
            case 'stop':
                icon.addClass("red");
                break;
            default:
                icon.addClass("gray");
        }
    }


    //Actuliza el ícono en la ventana de control, al hacer click en un vehículo
    function setIconClass(icon, iconPlataform) {

            icon.removeClass("fa fa-taxi fa-car fa-motorcycle fa-bus fa-truck fa-ship");
            iconPlataform = typeof iconPlataform == 'undefined' || iconPlataform == null ? '' : iconPlataform;
            var iconStr = iconPlataform.replace("offline_", "");
            var iconNumber = iconStr.split('_')[0];

            switch (parseInt(iconNumber,10)) {
        
                case 21:
                    icon.addClass('fa fa-taxi');
                    break;
                case 22:
                    icon.addClass('fa fa-car');
                    break;

                case 23:
                    icon.addClass('fa fa-motorcycle');
                    break;

                case 24:
                    icon.addClass('fa fa-bus');
                    break;

                case 25:
                case 27:
                    icon.addClass('fa fa-truck');
                    break;

                     case 26:
                         icon.addClass('fa fa-ship');
                         break;
                default:
                    icon.addClass('fa fa-car');
                    }
        }
    


    $(document).ready(
    function () {
        SlidingMarker.initializeGlobally();
        initMapa();
        initIdMarkers();
        initMapaHub();
        initNesteableDispositivos();
        asociaEventosGlobal();
    });


    //Fucnión para inicializar los id de los marcadores
    function initIdMarkers() {        
        var controlDispositivos = $('#divNestableMapControl');
        var dispositivoSelector = '.dd2ItemDispositivo';

        controlDispositivos.find(dispositivoSelector).each(function(index, element){
            element = $(element); idMarkers[index] = element.data('id');
            if (utilSAI.isLocalStorageSupported) {
                var dataDevice = $.parseJSON(localStorage.getItem(markerPrefix + idMarkers[index]));
                if (typeof dataDevice != 'undefined' && dataDevice != null)
                    crearMarcador(map, dataDevice);
            }
        });
    }


    //Función para mostrar el mapa de google
    function initMapa() {

        var mapOptions = { 
            navigationControl: true,
            scaleControl: true,
            zoom: zoom,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            mapTypeControlOptions: {
            mapTypeIds: [
            google.maps.MapTypeId.ROADMAP,
            google.maps.MapTypeId.SATELLITE
        ],
            position: google.maps.ControlPosition.BOTTOM_LEFT
        }
        };
        map = new google.maps.Map($(idDivMapa).get(0),mapOptions);

        // Try W3C Geolocation (Preferred)
        if (navigator.geolocation) {
            browserSupportFlag = true;
            navigator.geolocation.getCurrentPosition(function (position) {
                initialLocation = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                map.panTo(initialLocation);

            }, function () {
                handleNoGeolocation(browserSupportFlag);
            });
        } // Browser doesn't support Geolocation
       else {
            browserSupportFlag = false;
            handleNoGeolocation(browserSupportFlag);
        }


        google.maps.event.addListenerOnce(map, 'idle', function () {
            google.maps.event.trigger(map, 'resize');
        });

        if (monitorSAI)monitorSAI.map = map;

       //Se asocia el panel de control del mapa
        var widgetDiv = document.getElementById('divColControlMapa');
        map.controls[google.maps.ControlPosition.TOP_LEFT].push(widgetDiv);
        var widgetDispDiv = document.getElementById('divColControlMapaDisp');
        map.controls[google.maps.ControlPosition.TOP_RIGHT].push(widgetDispDiv);
        var widgetClientepDiv = document.getElementById('divColControlMapaCliente');
        map.controls[google.maps.ControlPosition.TOP_RIGHT].push(widgetClientepDiv);
        
        google.maps.event.addListenerOnce(map, 'idle', function () {
            var controlMapa = $('#divColControlMapa');

            if (!controlMapa.is(":visible"))
                controlMapa.show();
        });

        

    }

    //Inicia el servicio de hub con signalr    
    function initMapaHub() {

        var hub = $.connection.monitorMapaHub;

        hub.client.retornaLocalizacionAll = actualizaMarkersAsync;

        $.connection.hub.start().done(function () {
            hub.server.actualizaLocalizacionVehiculos();
        });
    }


    //AActualiza los marcadores de forma asícrona
    function actualizaMarkersAsync(data) {
        return $.Deferred(function (dfd) {

            //Se obtienen los objectos
            var promises = $.map(idMarkers, function (Id, index) {
                var dfdinterno = jQuery.Deferred();
                setTimeout(function () {
                    var dataDevice = $.grep(data, function (e, index) { return e && e.Id && e.Id === Id; })[0];
                    var marker = dataDevice && dataDevice.Id ? markers[dataDevice.Id] : null;

                    //Se actualiza la información en el local storage 
                    if (utilSAI.isLocalStorageSupported && marker)
                        localStorage.setItem(markerPrefix + dataDevice.Id, JSON.stringify(dataDevice));

                    if (marker) {

                        var iconByStatus = getIcon(dataDevice.icon, dataDevice.status)

                        updateColorWidget(dataDevice.Id, dataDevice.status, dataDevice.icon);

                        var new_marker_position = new google.maps.LatLng(dataDevice.latitude, dataDevice.longitude);
                        var iconOpacity = marker['icon']['fillOpacity'];
                        iconByStatus['fillOpacity'] = iconOpacity;

                        marker.setPosition(new_marker_position);
                        marker.setIcon(iconByStatus);

                        marker['speed'] = dataDevice.speed;
                        marker['IsStop'] = dataDevice.IsStop;
                        marker['course'] = dataDevice.course;
                        marker['distance'] = dataDevice.distance;
                        marker['stopTimeMinute'] = dataDevice.stopTimeMinute;
                        marker['deviceUtcDate'] = dataDevice.deviceUtcDate;
                        marker['KilometrajeTotal'] = dataDevice.KilometrajeTotal;
                        marker['status'] = dataDevice.status;

                        if (checkedMark != null && checkedMark["Id"] == dataDevice.Id) {
                            map.panTo(checkedMark.getPosition());

                            setValueControlDispMarker(checkedMark);
                        }
                        ocultaMuestraMarkerUnico(marker);
                    }

                    else
                        crearMarcador(map, dataDevice);

                    dfdinterno.resolve(); 
                }, (index * 10) + 10);
                return dfdinterno;
            });


            if ($(data).size() == 0)
                console.error("No se actualizó la localización de ningún vehículo. Verifique el log de transacciones del servidor", 'Error signalR');


            $.when.apply(this, promises).then(function () {
                dfd.resolve();
            });
			 return dfd.promise();
        });

    }

    //Función para inicializar un marker en un mapa dado 
    /// map         = mapa de google
    /// dataDevice  = información del dispositivo 
    function crearMarcador(map, dataDevice) {
        if (dataDevice) {
            var iconByStatus = getIcon(dataDevice.icon, dataDevice.status)

            updateColorWidget(dataDevice.Id, dataDevice.status, dataDevice.icon);

            var new_marker_position = new google.maps.LatLng(dataDevice.latitude, dataDevice.longitude);
            var marker = new google.maps.Marker({
                Id: dataDevice.Id
              , Name: dataDevice.Name
              , Dealer: dataDevice.Dealer
              , deviceUtcDate: dataDevice.deviceUtcDate
              , speed: dataDevice.speed
              , IsStop: dataDevice.IsStop
              , course: dataDevice.course
              , distance: dataDevice.distance
              , stopTimeMinute: dataDevice.stopTimeMinute
              , KilometrajeTotal: dataDevice.KilometrajeTotal
              , status: dataDevice.status
              , SerialNumber: dataDevice.SerialNumber
              , position: new_marker_position
              , map: map
              , animation: google.maps.Animation.DROP
              , title: dataDevice.Name
              , icon: getIcon(dataDevice.icon, dataDevice.status)
              , iconPlataform: dataDevice.icon
            });
            marker.addListener('click', function () { eventoClickonMarker(this); });
            marker.setMap(map);
            ocultaMuestraMarkerUnico(marker);
            markers[dataDevice.Id] = marker;
        }
    }

    //Inicia el arbol de dispositivos
    function initNesteableDispositivos() {
        $('#divNestableMapControl').nestable({ maxDepth:2});
        $('#divNestableMapControl').nestable('collapseAll');

        $('#divNestableMapControl').on('click', 'button[data-action="collapse"],button[data-action="expand"]', function () {
            utilSAI.setRightScroll();
        });

        $('.dd-handle a').on('mousedown', function (event) {
            event.stopPropagation();
        });

        utilSAI.setRightScroll();
    }
        
    //Manejar Errores de Geolocalización
    function handleNoGeolocation(errorFlag) {
        if (errorFlag == true) {
            console.log("Geolocation service failed.", "Error Geolocation");
            initialLocation = panamaLoc;
        } else {
            console.log("Your browser doesn't support geolocation. We've placed you in Panamá.");
            initialLocation = panamaLoc;
        }
        map.panTo(initialLocation);
    }

    //Asocia eventos
    function asociaEventosGlobal() {
        asociaEventoRepintar();
        asociaEventoUbicarMarker();
        asociaEventoWidgetDispClose();
        asociaEventoWidgetClienteClose();
        asocioEventoBuscaDispositivo();
        asociaEventoOcultaMuetraMarkerEstatus();
    }

    //Asocia evento ubicar Market
    function asociaEventoUbicarMarker() {
        $("#widgetControlMapa").on("click", ".icono-ubicar",
            function () {
                var id = $(this).closest("li").data('id');
                eventoClickonMarker(markers[id]);
            });
    }

    //Coloca el foko en el marker indicado
    function setFocusMarker(mark) {
        var markerIcon;
        var id = mark['Id'];
        map.panTo(mark.getPosition());
        mark.setAnimation(google.maps.Animation.BOUNCE, 500);
        window.setTimeout(function () { mark.setAnimation(null); }, 2000);
        if (checkedMark != null) {
            markerIcon = checkedMark['icon'];//getIconStatus(mark['status']);
            markerIcon['fillOpacity'] = 0.3;
            checkedMark.setIcon(markerIcon);
        }
        checkedMark = markers[id];
        markerIcon = checkedMark['icon'];
        markerIcon['fillOpacity'] = 1;
        checkedMark.setIcon(markerIcon);
        
    }

    //Asocia evento Repintar para los chosen
    function asociaEventoRepintar() {
        $('#ulTabMonitor').on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
              
            if (monitorSAI) $(monitorSAI.idTablaTicket).resize();
            google.maps.event.trigger(map, 'resize');
        });
    }

    //Evento de hacer click on marker
    function eventoClickonMarker(marker) {
        setValueControlDispMarker(marker);
        setFocusMarker(marker);        
    }

    //Coloca los valores al widget del mapa
    function setValueControlDispMarker(marker) {
        var prefijo = "#LocalizacionDispositivo_";
        if (marker != null) {            
            $(prefijo + 'Dealer').html(marker['Dealer']);
            $(prefijo + 'Name').html(marker['Name']);
            $(prefijo + 'status').html(traducirEstatus(marker['status']));
            $(prefijo + 'deviceUtcDate').html(marker['deviceUtcDate'].replace('T', ' ').replace('Z', ''));
            $(prefijo + 'stopTimeMinute').html(marker['stopTimeMinute']);
            $(prefijo + 'KilometrajeTotal').html(marker['KilometrajeTotal']+' km');
            $(prefijo + 'speed').html(marker['speed']+' km/h');
            setStatusColor($(prefijo + 'Car'), marker['status']);
            setIconClass($(prefijo + 'Car'), marker['iconPlataform']);
            $('#divColControlMapaDisp').show();
        }
    }


    //Coloca los valores al widget de cliente del mapa
    function setValueControlClienteMarker(marker) {
        var prefijo = "#LocalizacionCliente_";
        if (marker != null) {
            $(prefijo + 'Nombre').html(marker['Nombre']);
            $(prefijo + 'TelefonoMovil').html(marker['TelefonoMovil']);
            $(prefijo + 'Ubicacion').html(marker['Ubicacion']);
            $(prefijo + 'Id').html(marker['Id']);
            $("#Cliente_NombreCompleto").html(marker['Nombre']);
            $('#divColControlMapaCliente').show();
        }
    }




    //Asocia el evento de cerrar el widget de dispositivo
    function asociaEventoWidgetDispClose() {
        $('#divColControlMapaDisp').on('close.ace.widget', function (event) {
            removeCheckedMarker();
            $(this).hide();
            event.preventDefault();//action will be cancelled, widget box won't close
        });
    }

    //Asocia el evento de cerrar el widget de cliente  
    function asociaEventoWidgetClienteClose() {
        $('#divColControlMapaCliente').on('close.ace.widget', function (event) {
            $(this).hide();
            markerCliente.setMap(null);
            markerCliente= null;
            event.preventDefault();//action will be cancelled, widget box won't close
        });
    }

    //Elimina el marker que esté checkeado
    function removeCheckedMarker() {
        var markerIcon = checkedMark['icon'];//getIconStatus(mark['status']);
        markerIcon['fillOpacity'] = 0.3;
        checkedMark.setIcon(markerIcon);
        checkedMark = null;
    }



    //Asocia el evento de búsqueda de dispositivo en el control de dispositivos
    function asocioEventoBuscaDispositivo() {
        $('#btnBuscarDispositivo').on('click', function () {

            var controlDispositivos = $('#divNestableMapControl');

            var distribuidorSelector = '.dd2ItemDistribuidor';
            var dispositivoSelector = '.dd2ItemDispositivo';

            ocultaMuestraDispositivoDistribuidor(controlDispositivos.find(dispositivoSelector).has('i[class*="green"]'), 'move');
            ocultaMuestraDispositivoDistribuidor(controlDispositivos.find(dispositivoSelector).has('i[class*="red"]'), 'stop');
            ocultaMuestraDispositivoDistribuidor(controlDispositivos.find(dispositivoSelector).has('i[class*="gray"]'), 'offline');
          
        });

        $('#txtQueryDispositivo').keyup(function (e) {
            if (e.keyCode == 13) {
                $('#btnBuscarDispositivo').trigger("click");
            }
        });

    }


    //Ubica al cliente en el mapa
    monitorMapaSAI.ubicaClienteSAI = function (idNotificacion) {
      
        //Se busca y settea la información de usuario la información del usuario
        if (markerCliente != null) {
            markerCliente.setMap(null);
            $('#divColControlMapaCliente').hide();
        }
             var options = {
                  url               :   $('#divColControlMapaCliente').data('url-detalle-cliente')
                 ,parametros        : {Id : idNotificacion}
                 , mostrarMensajeCargando  : false
                 , callback         : function (data) {
                     if (data.content.Latitud != null && data.content.Latitud != '' && data.content.Longitud != 'null' && data.content.Longitud != '') {
                         var new_marker_position = new google.maps.LatLng(data.content.Latitud, data.content.Longitud);

                         markerCliente = new google.maps.Marker({
                             Id: data.content.Id
                             , Nombre: data.content.Nombre
                             , TelefonoMovil: data.content.TelefonoMovil
                             , Ubicacion: data.content.Ubicacion
                             , map: map
                             , animation: google.maps.Animation.DROP
                             , title: data.content.Nombr
                             , position: new_marker_position
                             , icon: MarkerIconClient()
                         });

                         setValueControlClienteMarker(markerCliente);

                         markerCliente.setAnimation(google.maps.Animation.BOUNCE, 500);
                         window.setTimeout(function () { markerCliente.setAnimation(null); }, 2000);

                         map.panTo(markerCliente.getPosition());
                     }
                 }
            };
             utilSAI.executeActionCall(options);
    }


    //Función para ocultar o mostrar los vehículos según el estatus
    function asociaEventoOcultaMuetraMarkerEstatus() {
        var status;
        var disp = $('#divNestableMapControl').find('.dd2ItemDispositivo');

        $("#btnMostrarEnMovimiento").on("click", function (e) {
            status = 'move';
            ocultaMuestraMarkerPorEstatus($(this), status);
            ocultaMuestraDispositivoDistribuidor(disp.has('i[class*="green"]'), status, { inClick: true });
        });

        $("#btnMostrarDetenido").on("click", function (e) {           
            status = 'stop';
            ocultaMuestraMarkerPorEstatus($(this), status);
            ocultaMuestraDispositivoDistribuidor(disp.has('i[class*="red"]'), status, { inClick: true });
        });

        $("#btnMostrarFueraDeLinea").on("click", function (e) {
            status = 'offline';
            ocultaMuestraMarkerPorEstatus($(this), status);
            ocultaMuestraDispositivoDistribuidor(disp.has('i[class*="gray"]'), status, { inClick: true });
        });
    }

    //Oculta y muestra marker por estatus
    function ocultaMuestraMarkerPorEstatus(boton,status) {
        
        var inMostrar = !boton.hasClass('active'); 
        var result = $.map(markers, function (e, index) { if (e && e.status && e.status.toLowerCase() === status) return e; });
        
        $.map(result,function (element, index) {
            ocultaMuestraMarker(element, inMostrar);
        });
    }

    //Oculta o muetra los vehículos en el listado de dispositivos por estatus en el widget de dispositivos
    //opciones = {inClick = true;   //Bool. Inidica si se hizo click en el botón o viene por otro evento    
                //} 
    function ocultaMuestraDispositivoDistribuidor(disp, status, opciones) {

        var IndicadorClick = opciones && opciones.inClick ? opciones.inClick : false;        
        var botonMovimiento = $("#btnMostrarEnMovimiento");
        var botonDetenido = $("#btnMostrarDetenido");
        var botonFueraLinea = $("#btnMostrarFueraDeLinea");

        var inBotonMovimientoClicked = IndicadorClick?!botonMovimiento.hasClass('active'):botonMovimiento.hasClass('active');
        var inBotonDetenidoClicked = IndicadorClick ? !botonDetenido.hasClass('active') : botonDetenido.hasClass('active');
        var inBotonFueraLineaClicked = IndicadorClick ? !botonFueraLinea.hasClass('active') : botonFueraLinea.hasClass('active');
        var textoFiltro = $('#txtQueryDispositivo').val();

        if ($.trim(textoFiltro) != '') {
            disp.not('[data-nombre*="' + $.trim(textoFiltro.toUpperCase()) + '"]').addClass('hidden');
            disp = disp.filter('[data-nombre*="' + $.trim(textoFiltro.toUpperCase()) + '"]');           
        }


        switch (status.toLowerCase()) {
            case 'offline':
                if (inBotonFueraLineaClicked)
                    disp.removeClass("hidden");
                else
                    disp.addClass("hidden");                
                break;
            case 'move':
                if (inBotonMovimientoClicked)
                    disp.removeClass("hidden");
                else
                    disp.addClass("hidden");                
                break;
            case 'stop':
                
                if (inBotonDetenidoClicked)
                    disp.removeClass("hidden");
                else
                    disp.addClass("hidden");               
                break;
            default:
                if (inBotonFueraLineaClicked)
                    disp.removeClass("hidden");
                else
                    disp.addClass("hidden");
        }      

      
        $('#divNestableMapControl').find('.dd2ItemDistribuidor').each(function () {
            if ($(this).find('.dd2ItemDispositivo:not(.hidden)').size() > 0)
                $(this).removeClass('hidden');
            else
                $(this).addClass('hidden');
        });
        
    }


    //Función para evaluar si se debe mostrar u ocultar el marcador
    function ocultaMuestraMarker(marker, inMostrar) {       
        marker.setVisible(inMostrar); 
    }

    //Función para ocultar o mostrar el marcador  (update o create) 
    function ocultaMuestraMarkerUnico(marker) {
        var boton;
         switch (marker.status.toLowerCase()) {
            case 'offline':
                boton = $("#btnMostrarFueraDeLinea");
                break;
            case 'move':
                boton = $("#btnMostrarEnMovimiento");
                break;
            case 'stop':
                boton = $("#btnMostrarDetenido");
                break;
            default:
                 boton = $("#btnMostrarFueraDeLinea");
            break;
            
        }
       
        ocultaMuestraMarker(marker, boton.hasClass('active'));
    } 
    
    monitorMapaSAI.markers = markers;
    monitorMapaSAI.markerPrefix = markerPrefix;

    window.monitorMapaSAI = monitorMapaSAI;



    



})(window);