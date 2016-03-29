<%  Response.TrySkipIisCustomErrors = true
    Response.StatusCode = 500 %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>Sai Innovación - Error Interno del Servidor</title>
    <Link rel="stylesheet" type="text/css" href= "<%=ResolveUrl("~/Content/css/bootstrap.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/css/ace.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/css/ace-part2.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/css/open_sans.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/css/font-awesome.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/css/responsive.bootstrap.css")%>"/>
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/css/style.css")%>" />
</head>
<body>
    <div class="row">
        <div class="col-xs-12">
            <div class="error-container">
                <div class="well">
                    <h1 class="grey lighter smaller">
                        <span class="blue bigger-125">
                            <i class="ace-icon fa fa-random"></i>
                            500
                        </span>
                        Algo falló ...
                    </h1>

                    <hr>
                    <h3 class="lighter smaller">¡Pero estamos trabajando 
											<i class="ace-icon fa fa-wrench icon-animated-wrench bigger-125"></i>
                        en ello!
                    </h3>

                    <div class="space"></div>

                    <div>
                        <h4 class="lighter smaller">Por favor, reporte al departamento de sistemas del error</h4>


                    </div>

                    <hr>
                    <div class="space"></div>


                </div>
            </div>
        </div>
    </div>
</body>
</html>
