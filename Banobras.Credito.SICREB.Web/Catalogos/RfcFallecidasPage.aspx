<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="RfcFallecidasPage.aspx.cs" Inherits="RfcFallecidasPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function msj() {

            var chk = document.getElementById('MainContent_RgdRfcFallecido_ctl00_ctl02_ctl02_ChkTodo');
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
                    runat="server" Width="22px" OnClick="btnExportPDF_Click1" />
                <asp:ImageButton ID="ImageButton1" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                    runat="server" Width="22px" OnClick="ImageButton1_Click" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="PERSONAS FALLECIDAS"></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td colspan="3" id="td1" width="100%">
                <%--<telerik:RadScriptManager runat="Server" ID="ScriptManager1" > </telerik:RadScriptManager>--%>
                <div class="textEntry">
                    Agregar Registros
                    <asp:FileUpload ID="file_txt_layout" runat="server" />
                    <asp:Button ID="btn_cargar" runat="server" Text="Cargar" OnClick="btn_cargar_Click" />
                    <span style="left: 575px; width: 90px; position: relative">
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClick="btn_eliminar_Click"
                            OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdRfcFallecido" AutoGenerateColumns="false" OnItemCommand="grids_ItemCommand"
                    AllowSorting="true" AllowPaging="true" PageSize="10" OnNeedDataSource="RgdRfcFallecido_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdRfcFallecido_EditCommand" OnUpdateCommand="RgdRfcFallecido_UpdateCommand"
                    OnDeleteCommand="RgdRfcFallecido_DeleteCommand"
                    OnInsertCommand="RgdRfcFallecido_InsertCommand" OnItemDataBound="RgdRfcFallecido_ItemDataBound">
                     <ExportSettings FileName="Fallecidas" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Fallecidas" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span>  Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvRfcFallecidas" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Registro"
                            RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="10%" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-HorizontalAlign="Justify" UniqueName="Id"
                                DataField="Id" Visible="false" DataType="System.Int32" MaxLength="30">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="RFC" HeaderStyle-HorizontalAlign="Justify" UniqueName="Rfc"
                                DataField="Rfc" DataType="System.String" MaxLength="13">
                            </telerik:GridBoundColumn>
                            <%--SICREB-INICIO ACA Sep-2012--%>
                            <telerik:GridBoundColumn HeaderText="NOMBRE" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="Nombre" DataField="Nombre" Visible="true" DataType="System.String"
                                MaxLength="16">
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn HeaderText="CREDITO" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="Credito" DataField="Credito" Visible="true" DataType="System.String"
                                MaxLength="10">
                            </telerik:GridNumericColumn>
                            <telerik:GridBoundColumn HeaderText="AUXILIAR" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="Auxiliar" DataField="Auxiliar" Visible="true" DataType="System.Int32"
                                MaxLength="20">
                            </telerik:GridBoundColumn>
                            <%--SICREB-FIN ACA Sep-2012--%>
                            <%--<telerik:GridBoundColumn HeaderText="FECHA" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="Fecha" DataField="Fecha" MaxLength="30" Visible="true">
                                <FilterTemplate>
                                    <telerik:RadDatePicker ID="FiltroDate" runat="server" AutoPostBack="true" OnSelectedDateChanged="FiltroDate_SelectedDateChanged">
                                    </telerik:RadDatePicker>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Fecha" UniqueName="FechaTemp" AllowFiltering="false"
                                SortExpression="FechaTemp" Visible="false">
                                <HeaderStyle />
                                <ItemTemplate>
                                    <telerik:RadDatePicker runat="server" ID="dtpFecha">
                                    </telerik:RadDatePicker>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridDateTimeColumn HeaderText="FECHA" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Fecha" DataField="Fecha" 
                                DataType="System.DateTime" DataFormatString="{0:dd MMMM yyyy}" PickerType="DatePicker" >
                            </telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-HorizontalAlign="Justify"
                                AllowFiltering="false" UniqueName="Estatus" DataField="Estatus" DataType="System.String  "
                                MaxLength="1" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false"
                                SortExpression="EstatusTemp" Visible="false">
                                <HeaderStyle />
                                <ItemTemplate> 
                                    <telerik:RadComboBox runat="server" ID="ComboEstatus"  AllowFiltering="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Activo" Value="1" />
                                            <telerik:RadComboBoxItem Text="Inactivo" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
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
        </tr>
    </table>
</asp:Content>
