<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="ActividadesEconomicasBanxicoPage.aspx.cs" Inherits="ActividadesEconomicasBanxicoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function msj() {
             debugger
            var chk = document.getElementById('MainContent_RgdBanxico_ctl00_ctl02_ctl02_ChkTodo');
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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="ACTIVIDADES ECONÓMICAS DE BANXICO"></asp:Label>
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
                    <span style="left: 575px; width: 30px; position: relative">
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClick="btn_eliminar_Click"
                            OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdBanxico" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" PageSize="10" OnNeedDataSource="RgdBanxico_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdBanxico_EditCommand" OnUpdateCommand="RgdBanxico_UpdateCommand"
                    OnColumnCreated="RgdBanxico_ColumnCreated" OnDeleteCommand="RgdBanxico_DeleteCommand"
                    OnInsertCommand="RgdBanxico_InsertCommand" OnItemDataBound="RgdBanxico_ItemDataBound">
                     <ExportSettings FileName="ActividadesEconomicasBANXICO" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="ACTIVIDADES ECONÓMICAS DE BANXICO" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1},<span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvBanxico" DataKeyNames="Id" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar actividad económica"
                            RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="5%" ItemStyle-Width="5%" FooterStyle-Width="5%"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-Width="0%" ItemStyle-Width="0%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Id" DataField="Id" DataType="System.Int32"
                                MaxLength="38" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn HeaderText="Clave CLIC" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="ClaveCLIC" DataField="ClaveCLIC" FilterControlWidth="80%"
                                DataType="System.Int32" MaxLength="10">
                            </telerik:GridNumericColumn>
                            <telerik:GridNumericColumn HeaderText="Clave Buró" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="ClaveBuro" DataField="ClaveBuro" FilterControlWidth="80%"
                                DataType="System.Int32" MaxLength="20">
                            </telerik:GridNumericColumn>
                            <telerik:GridBoundColumn HeaderText="Actividad" HeaderStyle-Width="22%" ItemStyle-Width="22%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Actividad" DataField="Actividad" FilterControlWidth="80%"
                                DataType="System.String" MaxLength="250">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Tipo" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Tipo" DataField="Tipo.descripcion"
                                DataType="System.String" MaxLength="40" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Tipo" UniqueName="TipoTemp" AllowFiltering="true" HeaderStyle-Width="31%"
                                SortExpression="Tipo.descripcion" FilterControlWidth="90%"  DataField="Tipo.descripcion">
                                <HeaderStyle Width="30%" />
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboTipo" DataTextField="Descripcion" DataValueField="Id" >
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridNumericColumn HeaderText="Tipo Clave" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Tipoid" DataField="Tipo.id" FilterControlWidth="80%"
                                DataType="System.Int32" MaxLength="40" Visible="true" >
                            </telerik:GridNumericColumn>
                            <telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false"
                                SortExpression="EstatusTemp" Visible="false">
                                <HeaderStyle Width="10%" />
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboEstatus">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Activo" Value="0" />
                                            <telerik:RadComboBoxItem Text="Inactivo" Value="1" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-Width="0%" ItemStyle-Width="0%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Estatus" DataField="Estatus"
                                DataType="System.String" MaxLength="10" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false" HeaderStyle-Width="7%">
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
