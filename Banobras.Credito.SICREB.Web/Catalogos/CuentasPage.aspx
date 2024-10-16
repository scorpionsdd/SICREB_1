<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="CuentasPage.aspx.cs" Inherits="EstadoPage" %>

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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="CUENTAS"></asp:Label>
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
                <telerik:RadGrid runat="server" ID="RgdCuentas" AutoGenerateColumns="false" AllowScroll="true"
                    OnItemCommand="grids_ItemCommand" AllowSorting="true" AllowPaging="true" PageSize="10"
                    OnNeedDataSource="RgdCuentas_NeedDataSource" AllowFilteringByColumn="True" OnUpdateCommand="RgdCuentas_UpdateCommand"
                    OnDeleteCommand="RgdCuentas_DeleteCommand" OnInsertCommand="RgdCuentas_InsertCommand"
                    OnItemDataBound="RgdCuentas_ItemDataBound">
                    <ExportSettings FileName="Cuentas" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Cuentas" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvCuentas" DataKeyNames="Id" NoMasterRecordsText="No existen Registros"
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
                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-HorizontalAlign="Justify" UniqueName="Id"
                                DataField="Id" DataType="System.Int32" MaxLength="3" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Código" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="170px" FilterControlWidth="80%" UniqueName="Codigo" DataField="Codigo"
                                DataType="System.String" MaxLength="30">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Descripción" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="300px" FilterControlWidth="80%" UniqueName="Descripcion" DataField="Descripcion"
                                DataType="System.String" MaxLength="100">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Sector" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="60px" FilterControlWidth="45%" UniqueName="Sector" DataField="Sector"
                                DataType="System.String" MaxLength="200">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Rol" HeaderStyle-HorizontalAlign="Justify" UniqueName="Rol"
                                HeaderStyle-Width="130px" FilterControlWidth="80%" DataField="Rol" DataType="System.String"
                                MaxLength="50">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Clasificación" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="130px" FilterControlWidth="80%" UniqueName="descripcionClasificacion"
                                DataField="descripcionClasificacion" MaxLength="100" SortExpression="descripcionClasificacion"
                                Visible="false">
                                <%--				<FilterTemplate>
								 <telerik:RadComboBox ID="FiltroRadCombo" runat ="server" AppendDataBoundItems="true" AutoPostBack ="true"
								  OnSelectedIndexChanged="FiltroRadCombo_SelectedIndexChanged" >
								</telerik:RadComboBox> 
									
								 </FilterTemplate>--%>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Clasificación" UniqueName="Clasificacion"
                                HeaderStyle-Width="130px" FilterControlWidth="80%" SortExpression="descripcionClasificacion"
                                Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblClasificacion" runat="server" Text='<%#Eval("descripcionClasificacion")%>'></asp:Label>
                                    <telerik:RadComboBox runat="server" ID="ComboClasificacion" MaxLength="38">
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Formato SIC" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Justify"
                                FilterControlWidth="80%" UniqueName="FormatSic" DataField="FormatSic" DataType="System.String  "
                                MaxLength="100">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Formato SICOFIN" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="130px" FilterControlWidth="80%" UniqueName="FormatSicofin"
                                DataField="FormatSicofin" DataType="System.String" MaxLength="100">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Persona" UniqueName="TipoPersonaTemp" AllowFiltering="false"
                                HeaderStyle-Width="80px" FilterControlWidth="80%">
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboPersona" ReadOnly="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Física" Value="F" />
                                            <telerik:RadComboBoxItem Text="Moral" Value="M" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Persona" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="60px" FilterControlWidth="80%" UniqueName="Persona" DataField="Persona"
                                DataType="System.String" MaxLength="1" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn HeaderText="Grupo" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="60px" FilterControlWidth="50%" UniqueName="Grupo" DataField="Grupo"
                                MaxLength="38">
                            </telerik:GridNumericColumn>
                            <telerik:GridTemplateColumn HeaderText="Tipo Cuenta" UniqueName="TipoCredito" AllowFiltering="true"
                                HeaderStyle-Width="130px" SortExpression="TipoCredito">
                                <ItemTemplate>
                                    <asp:Label ID="lblTipoCuenta" runat="server" Text='<%#Eval("tipoCuenta")%>'></asp:Label>
                                    <telerik:RadComboBox runat="server" ID="ComboTipoCredito" Width="100px" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Hipotecario" Value="H" />
                                            <telerik:RadComboBoxItem Text="Consumo" Value="C" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Tipo Cuenta" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="130px" UniqueName="tipoc" DataField="tipoCuenta" DataType="System.String"
                                MaxLength="1" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false"
                                HeaderStyle-Width="130px" SortExpression="EstatusTemp" Visible="false">
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboEstatus">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Activo" Value="1" />
                                            <telerik:RadComboBoxItem Text="Inactivo" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="130px" UniqueName="Estatus" DataField="Estatus" DataType="System.String"
                                MaxLength="1" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Seleccionar" AllowFiltering="false" HeaderStyle-Width="80px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ChkTodo" Text="TODOS" AutoPostBack="true" OnCheckedChanged="ChkTodo_CheckedChanged"
                                        runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" CssClass="chks" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    &nbsp;</EditItemTemplate>
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
