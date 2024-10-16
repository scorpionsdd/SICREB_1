<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="DiasInhabilesPage.aspx.cs" Inherits="DiasInhabilesPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!--JAGH se agrega Manager -->
    <script type="text/javascript">
        function msj() {

            var chk = document.getElementById('MainContent_RgdDiaInhabil_ctl00_ctl02_ctl02_ChkTodo');
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
                <asp:ImageButton ID="btnExportExcel" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="DÍAS INHÁBILES"></asp:Label>
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
                    Agregar Días
                    <asp:FileUpload ID="file_txt_layout" runat="server" />
                    <asp:Button ID="btn_cargar" runat="server" Text="Cargar" OnClick="btn_cargar_Click" />
                    <span style="right: 150px; width: 50px; position: absolute">
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClick="btn_eliminar_Click"
                            OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdDiaInhabil" AutoGenerateColumns="false" AllowSorting="true" OnItemCommand="grids_ItemCommand"
                    AllowPaging="true" PageSize="10" OnNeedDataSource="RgdDiaInhabil_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdDiaInhabil_EditCommand" OnUpdateCommand="RgdDiaInhabil_UpdateCommand"
                    OnColumnCreated="RgdDiaInhabil_ColumnCreated" OnDeleteCommand="RgdDiaInhabil_DeleteCommand"
                    OnInsertCommand="RgdDiaInhabil_InsertCommand" OnItemDataBound="RgdDiaInhabil_ItemDataBound">
                     <ExportSettings FileName="DiasInhabiles" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="DiasInhabiles" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1},  <span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvExceptuados" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar día inhábil"
                            RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="10%" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="ID" DataField="Id" DataType="System.Int32"
                                MaxLength="10" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Identificador de Día" HeaderStyle-Width="30%"
                                ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Justify" UniqueName="idententicadorDia"
                                DataField="idententicadorDia" DataType="System.String" MaxLength="300">
                            </telerik:GridBoundColumn>
                            <telerik:GridDateTimeColumn HeaderText="Fecha" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Fecha" DataField="dtFecha"
                                DataType="System.DateTime" DataFormatString="{0:dd MMMM yyyy}" PickerType="DatePicker">
                            </telerik:GridDateTimeColumn>
                            <telerik:GridDateTimeColumn HeaderText="Fecha Inhábil" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="FechaInhabil" DataField="dtFechaInhabil"
                                DataType="System.DateTime" DataFormatString="{0:dddd, dd MMMM yyyy}" PickerType="DatePicker">
                            </telerik:GridDateTimeColumn>
                            <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false">
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
