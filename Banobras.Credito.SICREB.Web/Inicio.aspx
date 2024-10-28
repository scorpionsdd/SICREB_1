<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Inicio.aspx.cs" Inherits="Inicio"%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Skin="Telerik"
        Animation="Slide" Position="TopCenter" ToolTipZoneID="Grafica" AutoTooltipify="true">
    </telerik:RadToolTipManager>

    <style type="text/css">

        #WindowLoad
        {
            position: fixed;
            top: 0px;
            left: 0px;
            z-index: 3200;
            filter: alpha(opacity=65);
            -moz-opacity: 65;
            opacity: 0.65;
            background: #ECECEC;
        }

    </style>

    <%--Funcionalidad de confirmación antes de cerrar--%>
    <%--<script type="text/javascript">
        window.addEventListener("beforeunload", function (event) {
            var message = "¿Estás seguro que deseas salir?";
            event.preventDefault();
            event.returnValue = message;
            return message;
        });
    </script>--%>

    <script type="text/javascript">
        
        function showRadConfirmM() {
            var width, height = "150px";
            var title = "Consumir WS-SICOFIN"
            var text = "¿Desea actualizar los saldos mensuales?";
            var imgUrl = "null";
            radconfirm(text, callBackFn_, width, height, null, title, imgUrl);
        }

        function callBackFn_(arg) {
            var dato = 0;
            if (arg == true) {
                dato = 1;
            }
            jsShowWindowLoad();
            PageMethods.ConsumirWSM(dato, OnSuccessProcesarMensual);
        }

        function OnSuccessProcesarMensual(response) {
            radalert(response.toString(), 320, 100, "");
            jsRemoveWindowLoad();
        }

        function showRadConfirmS() {
            var width, height = "150px";
            var title = "Consumir WS-SICOFIN"
            var text = "¿Desea actualizar los saldos semanales?";
            var imgUrl = "null";
            radconfirm(text, callBackFn, width, height, null, title, imgUrl);
        }

        function callBackFn(arg) {
            var dato = 0;
            if (arg == true) {
                dato = 1;
            }
            jsShowWindowLoad();
            PageMethods.ConsumirWSS(dato, OnSuccessProcesarSemanal);
        }

        function OnSuccessProcesarSemanal(response) {
            radalert(response.toString(), 320, 100, "");
            jsRemoveWindowLoad();
        }

        function jsRemoveWindowLoad() {
            $("#WindowLoad").remove();
        }

        function jsShowWindowLoad(mensaje) {
            jsRemoveWindowLoad();
            if (mensaje === undefined) mensaje = "Procesando la información <br /><br />Espere por favor";
            height = 20;
            var ancho = 0;
            var alto = 0;

            if (window.innerWidth == undefined) ancho = window.screen.width;
            else ancho = window.innerWidth;
            if (window.innerHeight == undefined) alto = window.screen.height;
            else alto = window.innerHeight;

            var heightdivsito = alto / 2 - parseInt(height) / 2;

            imgCentro = "<div style='text-align:center;height:" + alto + "px;'><div  style='color:#000;margin-top:" + heightdivsito + "px; font-size:20px;font-weight:bold'>" + mensaje + "</div><img width='200px' height='200px' src='Resources/Images/Loader.gif'></div>";
            //imgCentro = "";
            div = document.createElement("div");
            div.id = "WindowLoad"
            div.style.width = ancho + "px";
            div.style.height = alto + "px";
            $("body").append(div);

            input = document.createElement("input");
            input.id = "focusInput";
            input.type = "text"

            $("#WindowLoad").append(input);

            $("#focusInput").focus();
            $("#focusInput").hide();

            $("#WindowLoad").html(imgCentro);
        }
    </script>

    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td colspan="3">
                <table width="300px" class="tableinicio">
                    <tr>
                        <td>
                            <table width="290px" class="">
                                <tr>
                                    <td>
                                        <div class="titleCenter">
                                            &nbsp;SICOFIN
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table width="290px" class="">
                                <tr>
                                    <td>
                                        <div class="titleCenter">
                                            &nbsp;SIC
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table width="290px" class="">
                                <tr>
                                    <td>
                                        <div class="titleCenter">
                                            &nbsp;CALIFICACIONES
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="290px" class="tableinicio">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="LblSicofin" Text="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table width="290px" class="tableinicio">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="LblSic" Text="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table width="290px" class="tableinicio">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="LblCalif" Text="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td id="td1" class="titleLeft">
                        </td>
                        <td id="td2" class="titleCenter">
                            &nbsp;
                            <asp:Label Text="PERSONAS FÍSICAS" runat="server" ID="Label1" />
                        </td>
                        <td id="td3" class="titleRight">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <div id="chartPlaceholder">
                                <telerik:RadChart runat="server" ID="RadChartPersonasFisicas" SkinsOverrideStyles="true"
                                    Width="350px" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td id="td4" class="titleLeft">
                        </td>
                        <td id="td5" class="titleCenter">
                            &nbsp;
                            <asp:Label Text="HISTÓRICO" runat="server" ID="Label2" />
                        </td>
                        <td id="td6" class="titleRight">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <div id="Div1">
                                <telerik:RadChart runat="server" ID="RadChartHistorico" SkinsOverrideStyles="true"
                                    Width="420px" IntelligentLabelsEnabled="true" />
                                <div id="Div10" style="position: relative; left: 110px; top: -280px; width: 200px;
                                    height: 75px;">
                                    <asp:Label runat="server" ID="lblTituloRango" Text="Selección de Fecha" Style="position: absolute;
                                        left: 0px; top: 0px; width: 200px; text-align: center;" />
                                    <asp:Label runat="server" ID="lblFechaInicial" Text="Fecha Inicial:" Style="position: absolute;
                                        left: 5px; top: 22px;" />
                                    <telerik:RadDatePicker runat="server" ID="txtFechaInicial" Calendar-CultureInfo="es-MX"
                                        OnSelectedDateChanged="txtFechaInicial_SelectedDateChanged" AutoPostBack="True"
                                        Culture="es-MX" EnableTyping="False" Style="position: absolute; left: 100px;
                                        top: 20px; width: 95px;">
                                        <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                            ViewSelectorText="x" />
                                        <DateInput ID="DateInput1" runat="server" ReadOnly="true" LabelCssClass="" Width="" />
                                        <DatePopupButton CssClass="" ToolTip="Abrir el Calendario." />
                                    </telerik:RadDatePicker>
                                    <asp:Label runat="server" Text="Fecha Final:" ID="lblFechaFinal" Style="position: absolute;
                                        left: 5px; top: 47px;" />
                                    <telerik:RadDatePicker runat="server" ID="txtFechaFinal" Calendar-CultureInfo="es-MX"
                                        OnSelectedDateChanged="txtFechaFinal_SelectedDateChanged" AutoPostBack="True"
                                        Culture="es-MX" EnableTyping="False" Style="position: absolute; left: 100px;
                                        top: 45px; width: 95px;">
                                        <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                            ViewSelectorText="x" />
                                        <DateInput ID="DateInput2" runat="server" ReadOnly="true" LabelCssClass="" Width="" />
                                        <DatePopupButton CssClass="" ToolTip="Abrir el Calendario." />
                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" style="width: 280px" rowspan="3">
                <table width="100%" style="border: 1px solid #696969;">
                    <tr>
                        <td class="titleCenter" colspan="2">
                            Archivo Mensual:
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <%--<asp:Button runat="server" ID="btnSicofin2Mensual" Text="Procesar SICOFIN2 Mensual"
                                OnClick="btnSicofin2Mensual_Click" CssClass="btnLarge" OnClientClick="return msjValidaMensual();" />--%>
                            <telerik:RadAjaxPanel ID="pnl1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                                <asp:Button ID="btnSicofin2Mensual" runat="server" Text="Procesar SICOFIN2 Mensual"
                                    OnClientClick="showRadConfirmM(); return false;" CssClass="btnLarge" />
                            </telerik:RadAjaxPanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label5" Text="CLIC Mensual:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblClicMensual" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label7" Text="SICOFIN Mensual:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSICOFINMensual" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label6" Text="SICOFIN 2 Mensual:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSICOFIN2Mensual" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label61" Text="SIC Mensual:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSICMensual" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="titleCenter" colspan="2">
                            Archivo Semanal:
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button runat="server" ID="btnSicofin2Semanal" Text="Procesar SICOFIN2 Semanal"
                                OnClientClick="showRadConfirmS(); return false;" OnClick="btnSicofin2Semanal_Click"
                                CssClass="btnLarge" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label11" Text="CLIC Diario:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCLICDiaria" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label13" Text="SICOFIN Diario:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSICOFINDiaria" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label8" Text="SICOFIN 2 Diario:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSICOFIN2Diaria" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblEtiquetaSICDiario" Text="SIC Diario:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSICDiario" Text="" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #696969;
                    height: 100%">
                    <tr>
                        <td colspan="3" class="titleCenter">
                            Archivos
                        </td>
                    </tr>
                    <tr style="height: 37px">
                        <td>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelProceso" Text="Último Proceso:" CssClass="formulario" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblFechaProceso" Text="" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="height: 37px">
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:CheckBox runat="server" ID="chbNotificaciones" Text="Notificaciones" AutoPostBack="true"
                                OnCheckedChanged="chbNotificaciones_CheckedChanged" CssClass="formulario" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr align="center" style="border: border:hidden; height: 30px;">
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2" align="left">
                            <asp:Label runat="server" ID="Label9" Text="Grupos:" CssClass="formulario" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="center" style="border: border:hidden; height: 30px;">
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2" align="left">
                            <asp:CheckBoxList runat="server" ID="cbxGrupos" CellPadding="1" RepeatColumns="2"
                                OnSelectedIndexChanged="cbxGrupos_SelectedIndexChanged" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="center" style="border: border:hidden; height: 30px;">
                        <td>
                        </td>
                        <td align="left" colspan="2">
                            <asp:Label runat="server" ID="Label10" Text="Periodicidad:" CssClass="formulario" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr align="center" style="border: border:hidden; height: 30px;">
                        <td>
                        </td>
                        <td align="left">
                            <asp:RadioButton runat="server" ID="rdbSemanal" GroupName="1" Text="Diario" OnCheckedChanged="rdbSemanal_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton runat="server" ID="rdbMensual" GroupName="1" Text="Mensual" Checked="True" OnCheckedChanged="rdbMensual_CheckedChanged" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <%--Procesar Archivo--%>
                    <tr align="center" style="height: 60px;">
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:Button runat="server" ID="btnProcesarArchivos" Text="Procesar Archivos" OnClick="btnProcesarArchivos_Click" CssClass="btnLarge" />
                            <asp:LinkButton ID="lnkReload" runat="server">Actualizar</asp:LinkButton>
                            <br />
                            <br />
                            <asp:Label runat="server" ID="lblMensajePendiente" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <%--TXT Personas Físicas (PF) --%>
                    <tr align="center" style="height: 50px;">
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:Image runat="server" ID="ImagePersonasFisicas" ImageUrl="ResourcesSICREB/Images/BNB-SIC-ICO_TxtFile.png" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr align="center" style="height: 50px;">
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:LinkButton runat="server" ID="lnkArchivoPersonaFisica" OnClick="lnkArchivoPF_Click">LinkButton</asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <%--TXT Personas Morales (PM)--%>
                    <tr align="center" style="height: 50px;">
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:Image runat="server" ID="ImagePersonaMorales" ImageUrl="ResourcesSICREB/Images/BNB-SIC-ICO_TxtFile.png" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr align="center" style="height: 50px;">
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:LinkButton runat="server" ID="lnkArchivoPersonaMorales" OnClick="lnkArchivoPM_Click">LinkButton</asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td id="td7" class="titleLeft">
                        </td>
                        <td id="td8" class="titleCenter">
                            &nbsp;
                            <asp:Label Text="PERSONAS MORALES" runat="server" ID="Label3" />
                        </td>
                        <td id="td9" class="titleRight">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <div id="Div2">
                                <telerik:RadChart ID="RadChartPersonasMorales" runat="server" SkinsOverrideStyles="true"
                                    Width="350px" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td id="td10" class="titleLeft">
                        </td>
                        <td id="td11" class="titleCenter">
                            &nbsp;
                            <asp:Label runat="server" Text="CONCILIACIÓN" ID="Label4" />
                        </td>
                        <td id="td12" class="titleRight">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <div id="Div3_1">
                                <telerik:RadChart runat="server" ID="RadChartConciliacion" SkinsOverrideStyles="true"
                                    Width="420px" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <div style="width: 780px; height: 100px; background-color: white; position: relative;
                    left: 0px; top: 0px; border: solid 1px gray;">
                    <asp:Label runat="server" ID="Label12" Text="Archivo Grupos y Lineas" Style="position: absolute;
                        left: 10px; top: 10px; color: #000000; font-weight: bold; font-family: Calibri;
                        font-size: 18px;" />
                    <asp:Label runat="server" ID="lblEtiquetaGposMensual" Text="GPO's Mensual:" CssClass="formulario"
                        Style="position: absolute; left: 10px; top: 40px;" />
                    <asp:Label runat="server" ID="lblGruposMensual" Text="01/01/0001" Style="position: absolute;
                        left: 110px; top: 40px;" />
                    <asp:Label runat="server" ID="lblEtiquetaLineasMensual" Text="LCRyC Mensual:" CssClass="formulario"
                        Style="position: absolute; left: 10px; top: 70px;" />
                    <asp:Label runat="server" ID="lblLineasMensual" Text="01/01/0001" Style="position: absolute;
                        left: 110px; top: 70px;" />
                    <asp:Button runat="server" ID="btnProcesarGL" Text="Procesar Archivo GL" OnClick="btnProcesarGL_Click"
                        CssClass="btnLarge" Style="position: absolute; left: 300px; top: 10px;" />
                    <asp:Label runat="server" ID="lblMensajesGL" Style="position: absolute; left: 200px;
                        top: 55px; width: 400px; height: 35px; text-align: center;" />
                    <asp:Image runat="server" ID="imgArchivoGL" ImageUrl="ResourcesSICREB/Images/BNB-SIC-ICO_TxtFile.png"
                        Style="position: absolute; left: 675px; top: 10px;" />
                    <asp:LinkButton runat="server" ID="lnkArchivoGL" OnClick="lnkArchivoGL_Click" Text="GL_01011900_Mensual.txt"
                        Width="120px" Style="position: absolute; left: 625px; top: 60px; text-align: center;" />
                </div>
            </td>
        </tr>
    </table>

    <asp:HiddenField ID="hdfValidaMensual" runat="server" />
    <asp:HiddenField ID="hdfValidaSemanal" runat="server" />

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>

</asp:Content>
