function RadAlertImagen(tipo) {
    var alerta = $(".RadWindow .rwWindowContent .radalert");
    
    if(tipo == "error")
    {
        $(alerta).addClass("RadError");
    }
    else if(tipo== "success")
    {
        $(alerta).addClass("RadSuccess");
    }
}



function encabezados() {
    $(".divCard").each(function (index) {
        $(this).attr("id", "divCard" + index);

        var msg = $("#divCard" + index + " .divHeader").text();

        var exporters = $("#divCard" + index + " .divExporters");
        //if (msg != "") 
        {
            var dLeft = document.createElement("div");
            $(dLeft).addClass("Left");
            var dCenter = document.createElement("div");
            $(dCenter).text(msg).addClass("Center");
            var dCenterExport = document.createElement("div");
            $(dCenterExport).prepend(exporters);

            $(dCenterExport).addClass("CenterExporter");
            var dRight = document.createElement("div");
            $(dRight).addClass("Right");
            var dClear = document.createElement("div");
            $(dClear).attr("style", "clear:both");

            $("#divCard" + index).prepend(dClear);
            $("#divCard" + index).prepend(dRight);
            $("#divCard" + index).prepend(dCenterExport);
            $("#divCard" + index).prepend(dCenter);
            $("#divCard" + index).prepend(dLeft);
            $("#divCard" + index + " .divHeader").remove();
        }

    });
}