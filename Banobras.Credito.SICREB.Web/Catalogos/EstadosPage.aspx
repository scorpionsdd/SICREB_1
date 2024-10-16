<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="EstadosPage.aspx.cs" Inherits="EstadoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function msj() {

            var chk = document.getElementById('MainContent_RgdEstados_ctl00_ctl02_ctl02_ChkTodo');
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
    <!-- JAGH se agrega manager-->
    <%--<telerik:RadAjaxManager ID="ManagerEstados" runat="server" EnableAJAX="true" EnableViewState="true"> 
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RgdEstados">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RgdEstados" />
                <telerik:AjaxUpdatedControl ControlID="lblMsg" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>--%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <%--JAGH se agrega etiqueta para mostrar mensajes y eliminar ventana popup 20/01/13--%>
            <%--  <td width="90%">
                <asp:Label ID="lblMsg" runat="server" Font-Names="Verdana,Arial,Helvetica,sans-serif"  Font-Size="14px" Font-Bold="true" ForeColor="Red" Visible="false">Messages</asp:Label>
            </td>--%>
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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="ESTADOS"></asp:Label>
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
                    <span style="right: 140px; width: 90px; position: absolute">
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClick="btn_eliminar_Click"
                            OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdEstados" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" PageSize="10" OnNeedDataSource="RgdEstados_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdEstados_EditCommand" OnUpdateCommand="RgdEstados_UpdateCommand"
                    OnColumnCreated="RgdEstados_ColumnCreated" OnDeleteCommand="RgdEstados_DeleteCommand"
                    OnInsertCommand="RgdEstados_InsertCommand" OnItemDataBound="RgdEstados_ItemDataBound">
                    <ExportSettings FileName="Estado" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Estado" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvEstados" DataKeyNames="ClaveClic" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Estado" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="10%" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridNumericColumn HeaderText="Clave CLIC" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="ClaveClic" DataField="ClaveClic"
                                DataType="System.Int32" MaxLength="38">
                            </telerik:GridNumericColumn>

                            <telerik:GridBoundColumn HeaderText="Clave Buró" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="ClaveBuro" DataField="ClaveBuro"
                                DataType="System.String  " MaxLength="4">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Descripción" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="Descripcion" DataField="Descripcion"
                                DataType="System.String  " MaxLength="30">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Tipo de Persona" UniqueName="TipoPersonaTemp"
                                AllowFiltering="false" SortExpression="TipoPersonaTemp" Visible="false">
                                <HeaderStyle Width="20%" />
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboPersona">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Física" Value="F" />
                                            <telerik:RadComboBoxItem Text="Moral" Value="M" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" AllowFiltering="false" UniqueName="Estatus"
                                DataField="Estatus" DataType="System.String  " MaxLength="1" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false"
                                SortExpression="EstatusTemp" Visible="false">
                                <HeaderStyle Width="10%" />
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboEstatus">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Activo" Value="1" />
                                            <telerik:RadComboBoxItem Text="Inactivo" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="checke" AllowFiltering="false" UniqueName = "checke">
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
