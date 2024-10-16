<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="ConvertidorActPage.aspx.cs" Inherits="ConvertidorActPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function msj() {

            var chk = document.getElementById('MainContent_RgdCuentas_ctl00_ctl02_ctl02_ChkTodo');
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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="CONVERTIDOR ACTUAL"></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
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
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClick="btn_eliminar_Click"
                            OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdCuentas" AutoGenerateColumns="false" AllowScroll="true" OnItemCommand="grids_ItemCommand"
                    AllowSorting="true" AllowPaging="true" PageSize="10" OnNeedDataSource="RgdCuentas_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdCuentas_EditCommand" OnUpdateCommand="RgdCuentas_UpdateCommand"
                    OnColumnCreated="RgdCuentas_ColumnCreated" OnDeleteCommand="RgdCuentas_DeleteCommand"
                    OnInsertCommand="RgdCuentas_InsertCommand" OnItemDataBound="RgdCuentas_ItemDataBound">
                     <ExportSettings FileName="Convertidor_Act" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Cuentas" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvCuentas" DataKeyNames="id_convertidor" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="99%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Cuenta" RefreshText="Actualizar Datos" />
                        <Columns>
                             <telerik:GridEditCommandColumn HeaderStyle-Width="50px" ItemStyle-Width="50px" FooterStyle-Width="50px"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>                           
                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-HorizontalAlign="Justify" UniqueName="id_convertidor"
                                DataField="id_convertidor" DataType="System.Int32" MaxLength="3" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Cuenta Actual" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="130px" FilterControlWidth="80%"
                                UniqueName="cuenta_act" DataField="cuenta_act" DataType="System.String" MaxLength="16">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Cuenta Anterior Vigente" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="130px" FilterControlWidth="80%"
                                UniqueName="cuenta_ant_vigente" DataField="cuenta_ant_vigente" DataType="System.String" MaxLength="16">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Cuenta Anterior Vencido" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="130px" FilterControlWidth="80%"
                                UniqueName="cuenta_ant_vencido" DataField="cuenta_ant_vencido" DataType="System.String" MaxLength="16">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Cuenta Anterior Moratorios" HeaderStyle-HorizontalAlign="Justify" UniqueName="Rol" HeaderStyle-Width="130px" FilterControlWidth="80%"
                                DataField="cuenta_ant_moratorios" DataType="System.String" MaxLength="16">
                            </telerik:GridBoundColumn>                            
                            
                            <telerik:GridBoundColumn HeaderText="Cuenta Capital Vigente" HeaderStyle-HorizontalAlign="Justify" UniqueName="cuenta_capital" HeaderStyle-Width="130px" FilterControlWidth="80%"
                                DataField="cuenta_capital" DataType="System.String" MaxLength="50">
                            </telerik:GridBoundColumn>
                                                        
                            <telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false" HeaderStyle-Width="130px"
                                SortExpression="EstatusTemp" Visible="false">
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboEstatus">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Activo" Value="1" />
                                            <telerik:RadComboBoxItem Text="Inactivo" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="130px"
                                UniqueName="estatus" DataField="estatus" DataType="System.String" MaxLength="1"
                                Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Seleccionar" AllowFiltering="false" HeaderStyle-Width="80px" >
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
                    <ClientSettings>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True">
                        </Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
</asp:Content>
