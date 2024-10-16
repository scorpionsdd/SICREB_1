<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="MonedasPage.aspx.cs" Inherits="Catalogos_MonedasPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function msj() {

            var chk = document.getElementById('MainContent_RgdMONEDAS_ctl00_ctl02_ctl02_ChkTodo');
            var total = document.getElementById('tot');

            var chks = getElementsByClassNameIE("chks");
            if (chk.checked) {
                var answer = confirm("Esta seguro que desea eliminar " + total.innerHTML + " Registros de Esta Tabla");
                if (answer) {
                    return true;
                }
                else {

                    return false;
                }
            } else {
                var num = 0;
                for (i = 0, j = 0; i < chks.length; i++) {
                    chk1 = chks[i].getElementsByTagName('input')[0];
                    if (chk1.checked) num++;
                }


                if (num == 0) {
                    alert("Debe seleccionar al menos un registro para eliminar");
                    return false;
                }
                else {

                    var answer = confirm("Esta seguro que desea eliminar " + num + " Registros de Esta Tabla");
                    if (answer) {
                        return true;
                    }
                    else {
                        return false;
                    }

                }

            }
            return false;
        }
    </script>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td>
            </td>
            <td width="100%" align="right">
                <asp:ImageButton ID="btnExportPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                    runat="server" Width="22px" OnClick="btnExportPDF_Click" />
                <asp:ImageButton ID="ImageButton1" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                    runat="server" Width="22px" OnClick="btnExportExcel_Click" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="MONEDAS"></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td colspan="3" id="td1" width="100%">
                <div class="textEntry">
                    Agregar Registros
                    <asp:FileUpload ID="file_txt_layout" runat="server" />
                    <asp:Button ID="btn_cargar" runat="server" Text="Cargar" OnClick="btn_cargar_Click" />
                    <span style="right: 150px; width: 90px; position: absolute">
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClick="btn_eliminar_Click"
                            OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdMONEDAS" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" PageSize="10" OnNeedDataSource="RgdMONEDAS_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdMONEDAS_EditCommand" OnUpdateCommand="RgdMONEDAS_UpdateCommand"
                    OnColumnCreated="RgdMONEDAS_ColumnCreated" OnDeleteCommand="RgdMONEDAS_DeleteCommand"
                    OnInsertCommand="RgdMONEDAS_InsertCommand" OnItemDataBound="RgdMONEDAS_ItemDataBound">
                    <ExportSettings FileName="Monedas" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Monedas" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1},  <span id='tot'>{5}</span>  Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvMONEDAS" DataKeyNames="ID" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Moneda" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="7%" ItemStyle-Width="7%" FooterStyle-Width="7%"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="ID" HeaderStyle-Width="0%" ItemStyle-Width="0%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="ID" DataField="ID" Display="false"
                                DataType="System.Int32" MaxLength="3" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn HeaderText="Clave Buró" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="CLAVEBURO" DataField="CLAVEBURO"
                                DataType="System.Int32" MaxLength="38">
                            </telerik:GridNumericColumn>
                            <telerik:GridNumericColumn HeaderText="Clave SIC" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="CLAVECLIC" DataField="CLAVECLIC"
                                DataType="System.Int32" MaxLength="38">
                            </telerik:GridNumericColumn>
                            <telerik:GridBoundColumn HeaderText="Descripción" HeaderStyle-Width="40%" ItemStyle-Width="40%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="DESCRIPCION" DataField="DESCRIPCION"
                                DataType="System.String" MaxLength="35">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false" HeaderStyle-Width="13%">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ChkTodo" Text="TODOS" AutoPostBack="true" OnCheckedChanged="ChkTodo_CheckedChanged"
                                        runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" CssClass="chks" />
                                </ItemTemplate>
                                <EditItemTemplate>&nbsp;</EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
