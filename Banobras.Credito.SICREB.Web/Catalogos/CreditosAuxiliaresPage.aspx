<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="CreditosAuxiliaresPage.aspx.cs" Inherits="CreditosAuxiliaresPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function msj() {

            var chk = document.getElementById('MainContent_RgdAuxiliares_ctl00_ctl02_ctl02_ChkTodo');
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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="CRÉDITOS AUXILIARES"></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
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
                <telerik:RadGrid runat="server" ID="RgdAuxiliares" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" PageSize="10" OnNeedDataSource="RgdAuxiliares_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdAuxiliares_EditCommand" OnUpdateCommand="RgdAuxiliares_UpdateCommand"
                    OnColumnCreated="RgdAuxiliares_ColumnCreated" OnDeleteCommand="RgdAuxiliares_DeleteCommand"
                    OnInsertCommand="RgdAuxiliares_InsertCommand" OnItemDataBound="RgdAuxiliares_ItemDataBound">
                    <ExportSettings FileName="Creditos_Auxiliares" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                    <Pdf PageTitle="Creditos_Auxiliares" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'>{5}</span>  Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvAuxiliares" DataKeyNames="Id" NoMasterRecordsText="No existen registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar auxiliar"
                            RefreshText="Actualizar datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="10%" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-Width="0%" ItemStyle-Width="0%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Id" DataField="Id" Display="false"
                                DataType="System.Int32" MaxLength="3">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Auxiliar" HeaderStyle-Width="15%" ItemStyle-Width="15%"  FilterControlWidth = "80%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Auxiliar" DataField="Auxiliar"
                                DataType="System.String" MaxLength="100">
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn HeaderText="Crédito" HeaderStyle-Width="10%" ItemStyle-Width="10%"  FilterControlWidth = "80%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Credito" DataField="Credito"
                                DataType="System.String" MaxLength="50">
                            </telerik:GridNumericColumn>
                            <telerik:GridBoundColumn HeaderText="RFC" HeaderStyle-Width="10%" ItemStyle-Width="10%" FilterControlWidth = "80%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="RFC" DataField="RFC" DataType="System.String"
                                MaxLength="15">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Nombre" HeaderStyle-Width="45%" ItemStyle-Width="45%" FilterControlWidth = "90%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Nombre" DataField="Nombre"
                                DataType="System.String" MaxLength="50">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-Width="0%" ItemStyle-Width="0%"
                                HeaderStyle-HorizontalAlign="Justify" AllowFiltering="false" UniqueName="Estatus"
                                DataField="Estatus" DataType="System.String" MaxLength="10" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false"
                                SortExpression="EstatusTemp" Visible="false">
                                <HeaderStyle Width="10%" />
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboEstatus">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Activo" Selected="true" Value="1" />
                                            <telerik:RadComboBoxItem Text="Inactivo" Value="0" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seleccionar"  AllowFiltering="false" HeaderStyle-Width="10%">
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
