<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="ClientesTMP.aspx.cs" Inherits="ClienteTMP" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%-- <script src="../Resources/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>--%>
    <script src="../ResourcesSICREB/Scripts/jquery-3.6.0.min.js"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            var tp;

            function getParameterByName(name, url) {
                if (!url) url = window.location.href;
                name = name.replace(/[\[\]]/g, '\\$&');
                var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                    results = regex.exec(url);
                if (!results) return null;
                if (!results[2]) return '';
                return decodeURIComponent(results[2].replace(/\+/g, ' '));
            }

            $(document).ready(function () {
                tp = getParameterByName("Persona");
                if (getParameterByName("Persona") == "PF")
                    $("#lblTitle").html("Clientes Temporales Personas Físicas");
                else
                    $("#lblTitle").html("Clientes Temporales Personas Morales");

            });

            var chkall;
            function msj(sender, eventArgs) {
                chkall = $(".rgHeaderDiv thead").find("input[type='checkbox']")[0]

                if (chkall.checked) {
                    radconfirm("¿Esta Seguro que desea eliminar todos las personas del catálogo?", confirmCallBackFn, 330, 100, null, "", null);
                }
                else {
                    if (selected.length == 0) {
                        radalert("Debe seleccionar al menos un registro para eliminar", 320, 100, "");
                    }
                    else {
                        radconfirm("¿Está seguro que desea eliminar " + selected.length + " personas del catálogo?", confirmCallBackFn, 330, 100, null, "", null);
                    }
                }
                return false;
            }

            function confirmCallBackFn(arg) {
                if (arg) {
                    PageMethods.Elimina(chkall.checked, tp, selected, OnSuccessGetCityNameArray);
                }
            }
            function OnSuccessGetCityNameArray(response) {
                var masterTable = $find("<%= RgdClientes.ClientID %>").get_masterTableView();
                masterTable.rebind();
                selected = [];
                radalert(response.toString(), 320, 100, "");
            }
            var selected = [];
            function rowSelected(sender, args) {
                var rfc = args.getDataKeyValue("RFC");
                if (selected.find(function (crfc) { return crfc == rfc; }) == undefined) {
                    selected.push(rfc);
                }
            }
            function rowDeselected(sender, args) {
                var rfc = args.getDataKeyValue("RFC");
                for (var i = 0; i < selected.length; i++) {
                    if (selected[i] === rfc) {
                        selected.splice(i, 1);
                    }
                }
            }
            function rowCreated(sender, args) {
                var rfc = args.getDataKeyValue("RFC");
                if (selected.find(function (crfc) { return crfc == rfc; }) != undefined) {
                    args.get_gridDataItem().set_selected(true);
                }
            }
            function gridCreated(sender, eventArgs) {
                var masterTable = sender.get_masterTableView();
                var selectColumn = masterTable.getColumnByUniqueName("ClientSelectColumn");
                var headerCheckBox = $(selectColumn.get_element()).find("[type=checkbox]")[0];

                if (headerCheckBox) {
                    headerCheckBox.checked = masterTable.get_selectedItems().length ==
                        masterTable.get_dataItems().length;
                }

                $(".rgHeader a").each(function (i, a) {
                    $(a).html(a.text.replace(/\_+/g, " "));
                    $(a).html(a.text.replace("PAIS", "PAÍS"));
                    $(a).html(a.text.replace("TELEFONO", "TELÉFONO"));
                    $(a).html(a.text.replace("COMPANIA", "COMPAÑÍA"));
                    $(a).html(a.text.replace("NUM EXT", "NÚM. EXT."));
                    $(a).html(a.text.replace("NUM INT", "NÚM. INT."));
                    $(a).html(a.text.replace("CODIGO POSTAL", "CÓDIGO POSTAL"));
                    $(a).html(a.text.replace("ACT ECO", "ACT. ECO."));
                    $(a).html(a.text.replace("FECHA NAC", "FECHA NAC."));
                });
            }
        </script>
    </telerik:RadScriptBlock>
    <style type="text/css">
        input {
            text-transform: uppercase;
        }
    </style>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td></td>
            <td width="100%" align="right">
                <asp:ImageButton ID="btnExportPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                    runat="server" Width="22px" OnClick="btnExportPDF_Click1" />
                <asp:ImageButton ID="ImageButton1" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                    runat="server" Width="22px" OnClick="ImageButton1_Click" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">&nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                <label id="lblTitle">Clientes</label>
            </td>
            <td id="tdRubroDer" class="titleRight">&nbsp;
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center" style="table-layout: fixed;">
        <tr>
            <td colspan="3" id="td1" width="100%">
                <div class="textEntry">
                    Agregar Registros
                    <asp:FileUpload ID="file_txt_layout" runat="server" />
                    <asp:Button ID="btn_cargar" runat="server" Text="Cargar" OnClick="btn_cargar_Click" />
                    <span style="right: 150px; width: 90px; position: absolute">
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClientClick="return msj(this, event);" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdClientes"
                    AllowSorting="True" AllowPaging="True"
                    AllowFilteringByColumn="True"
                    GridLines="None" OnUpdateCommand="RgdClientes_UpdateCommand"
                    OnInsertCommand="RgdClientes_InsertCommand"
                    OnNeedDataSource="RgdClientes_NeedDataSource"
                    AllowMultiRowSelection="true" OnPreRender="RgdClientes_PreRender">
                    <ClientSettings>
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="0" />
                        <ClientEvents OnRowCreated="rowCreated" OnRowSelected="rowSelected"
                            OnRowDeselected="rowDeselected" OnGridCreated="gridCreated" />
                        <Resizing AllowColumnResize="true" AllowRowResize="false" ResizeGridOnColumnResize="false"
                            ClipCellContentOnResize="true" EnableRealTimeResize="false" />
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                    </ClientSettings>
                    <ExportSettings FileName="Clientes" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Clientes" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvCuentas" ClientDataKeyNames="RFC" DataKeyNames="RFC" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="99%" AutoGenerateColumns="true" HeaderStyle-Width="180px">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Cliente" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="50px">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="60px" ItemStyle-Width="60px" FooterStyle-Width="60px"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1"
        DefaultLoadingPanelID="RadAjaxLoadingPanel1" EnablePageHeadUpdate="false">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RgdClientes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RgdClientes" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btn_eliminar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RgdClientes" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>
</asp:Content>
