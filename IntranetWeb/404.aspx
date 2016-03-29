<%  Response.TrySkipIisCustomErrors = True
    Response.StatusCode = 404 %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>Sai Innovación - Página no encontrada</title>
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
												<i class="ace-icon fa fa-sitemap"></i>
												404
											</span>
											Página no encontrada
										</h1>

										<hr>
										<h3 class="lighter smaller">¡Buscamos en todas partes pero no  ubicamos la página que busca!</h3>

										<div>
											

											<div class="space"></div>
											<h4 class="smaller">Por favor, consulte con el departamento de sistemas</h4>

											
										</div>

										<hr>
										<div class="space"></div>

										
									</div>
            </div>
        </div>
    </div>
</body>
</html>