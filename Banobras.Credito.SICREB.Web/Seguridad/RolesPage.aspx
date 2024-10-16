<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="RolesPage.aspx.cs" Inherits="Seguridad_RolesPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">
        
        (function (global, undefined) {

            function confirmAspUpdatePanelPostback(button, title, message) {
                function aspUpdatePanelCallbackFn(arg) {
                    if (arg) {
                        __doPostBack(button.name, '');
                    }
                }
                var description = button.getAttribute('data-id');
                radconfirm(message + "<br>" + "Rol: " + "<strong>" + description + "</strong><br><br>", aspUpdatePanelCallbackFn, 330, 180, null, title);
            }

            global.confirmAspUpdatePanelPostback = confirmAspUpdatePanelPostback;

        })(window);

    </script>

    <%--Botones principales--%>
    <table id="Section_Botones" runat="server"  cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td></td>
            <td width="100%" align="right">
                <asp:Button ID="btnRoleAdd" runat="server" Text="Agregar" CssClass="btnSmall" OnClick="btnRoleAdd_Click" />
                <asp:ImageButton ID="btnExportPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" runat="server" Width="22px" OnClick="btnExportPDF_Click" />
                <asp:ImageButton ID="btnExportExcel" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" runat="server" Width="22px" OnClick="btnExportExcel_Click" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">&nbsp;</td>
            <td id="tdRubroAll" class="titleCenter">&nbsp;<asp:Label runat="server" ID="lblTitle" Text="ADMINISTRACIÓN DE ROLES"></asp:Label></td>
            <td id="tdRubroDer" class="titleRight">&nbsp;</td>
        </tr>
    </table>

    <%--Listado de Roles --%>
    <table id="Section_Listado" runat="server" cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td colspan="4" id="td1" width="100%">

                <telerik:RadGrid runat="server" ID="RgdRoles" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="true" EnableLinqExpressions ="false" AllowPaging="true" PageSize="10" 
                    OnNeedDataSource="RgdRoles_NeedDataSource"
                    OnItemDataBound="RgdRoles_ItemDataBound">
                    <ExportSettings FileName="Administracion Roles" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Crédito Exceptuados" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true"
                        FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior"
                        NextPageToolTip="Página Siguiente"
                        LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: "
                        PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvRol" NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace" Width="100%" DataKeyNames="Id,Idrol">
                        <CommandItemSettings ShowRefreshButton="true" ShowAddNewRecordButton="false" AddNewRecordText="Agregar Rol" RefreshText="Actualizar Datos" />
                        <Columns>

                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="Id" DataField="Id" Display="false" AllowFiltering ="false"
                                DataType="System.Int32" MaxLength="5" Visible="false">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="Identificador" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="Idrol" DataField="Idrol" Display="true"
                                DataType="System.Int32" MaxLength="5" Visible="true">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="Descripción" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="Rol" DataField="Rol"
                                DataType="System.String" MaxLength="50">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="Facultades" HeaderStyle-Width="37%" ItemStyle-Width="37%" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="Facultades" DataField="Facultades" Visible="true"
                                DataType="System.String">
                            </telerik:GridBoundColumn>

						    <Telerik:GridBoundColumn HeaderText="Fecha Creación" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						        UniqueName="CreationDate" DataField="CreationDate" AllowFiltering ="false" DataFormatString="{0:dd/MM/yyyy}" Visible="true"
						        DataType="System.String" MaxLength="20" >
					        </Telerik:GridBoundColumn>

						    <Telerik:GridBoundColumn HeaderText="Fecha Edición" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						        UniqueName="TransactionDate" DataField="TransactionDate" AllowFiltering ="false" DataFormatString="{0:dd/MM/yyyy}" Visible="true"
						        DataType="System.String" MaxLength="20" >
					        </Telerik:GridBoundColumn>

						    <Telerik:GridBoundColumn HeaderText="Usuario que editó" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
						        UniqueName="TransactionLogin" DataField="TransactionLogin" AllowFiltering ="true" Visible="true"
						        DataType="System.String" MaxLength="20" >
					        </Telerik:GridBoundColumn>

						    <Telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
						        UniqueName="Estatus" DataField="Estatus" AllowFiltering ="true"
						        DataType="System.String" MaxLength="10" >
					        </Telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-Width="7%" HeaderText="Acciones" AllowFiltering ="false">
                                <ItemTemplate>                                            
                                    <div style="display: flex; text-align:center; align-items:center;">
                                        <div style="width: 50%;">
                                            <asp:ImageButton ID="imgbDelete" runat="server" title="Eliminar usuario" ImageUrl="~/App_Themes/Banobras2011/Grid/Delete.gif" data-id='<%# Eval("Rol") %>' OnClientClick="confirmAspUpdatePanelPostback(this, 'Eliminación de rol', 'Confirma la eliminación del rol?'); return false;" OnClick="btnGridDelete_Click" />
                                            <asp:ImageButton ID="imgbActivate" runat="server" title="Activar usuario" ImageUrl="~/App_Themes/Banobras2011/Grid/off-button-26.png" data-id='<%# Eval("Rol") %>' OnClientClick="confirmAspUpdatePanelPostback(this, 'Activación de rol', 'Confirma la activacción del rol?'); return false;" OnClick="btnGridActivate_Click" />
                                        </div>
                                        <div style="width: 50%;">
                                            <asp:ImageButton ID="imgbEdit" runat="server" title="Editar usuario" ImageUrl="~/App_Themes/Banobras2011/Grid/Edit.gif" OnClick="btnGridEdit_Click" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                    </MasterTableView>

                </telerik:RadGrid>
            </td>

        </tr>
    </table>

    <br />

    <%--Agregar Rol--%>
    <table id="Section_Agregar" runat="server" cellpadding="0" cellspacing="0" width="100%" align="center" class="tableinicio">
        <tr>
            <td id="td2">&nbsp;</td>
            <td colspan="3" id="td3" class="titleCenter" style="text-align:center;">
                <asp:Label runat="server" ID="Label1" Text="AGREGAR ROL"></asp:Label></td>
            <td id="td4">&nbsp;</td>
        </tr>
        <%--Rol--%>
        <tr style="height: 40px;">
            <td></td>
            <td colspan="3">
                <asp:Label ID="LabelRol" runat="server" Text="Rol:" CssClass="formulario"></asp:Label>
                <asp:TextBox ID="txtRolDescription" runat="server" Enabled="false"></asp:TextBox>
                <asp:TextBox ID="txtRolId" runat="server" Enabled="false" Visible="false"></asp:TextBox>
            </td>
            <td></td>
        </tr>
        <%--Facultades--%>
        <tr>
            <td></td>
            <td colspan="3">
                <table style="width: 100%;">
                    <tr style="height: 40px;">
                        <td style="width:5%;"></td>
                        <td style="width:40%;">
                            <asp:Label ID="LabelNoAsignadas" runat="server" Text="Facultades:" CssClass="formulario"></asp:Label>
                        </td>
                        <td style="width:10%;"></td>
                        <td style="width:40%;">
                            <asp:Label ID="LabelAsignadas" runat="server" Text="Facultades Asignadas:" CssClass="formulario"></asp:Label></td>
                        <td style="width:5%;"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:ListBox ID="ListFacultadDisponibles" runat="server" Enabled="false" Width="100%" SelectionMode="Multiple" Rows="10"></asp:ListBox>
                        </td>
                        <td style="text-align:center;">
                            <asp:ImageButton ID="btnFacultadAdd" runat="server" ImageUrl="~/Resources/Images/BarsButtons/BNB-SIC-ButtonCtrl_Agregar.png" onclick="btnFacultadAdd_Click" Enabled="false" />
                            <br />
                            <br />
                            <asp:ImageButton ID="btnFacultadRemove" runat="server" ImageUrl="~/Resources/Images/BarsButtons/BNB-SIC-ButtonCtrl_Quitar.png" onclick="btnFacultadRemove_Click" Enabled="false" />
                        </td>
                        <td>
                            <asp:ListBox ID="ListFacultadAsginadas" runat="server" Enabled="false" Width="100%" SelectionMode="Multiple" Rows="10"></asp:ListBox></td>
                        <td></td>
                    </tr>
                </table>
            </td>
            <td></td>
        </tr>        
        <%--Espacio--%>
        <tr>
            <td></td>
            <td colspan="3"></td>
            <td></td>
        </tr>        
        <%--Botonera--%>
        <tr>
            <td></td>
            <td colspan="3" align="center">
                <br />
                <asp:Button ID="btnRoleSave" runat="server" Text="Guardar" Enabled="false" OnClick="btnRoleSave_Click" CssClass="btnSmall" />
                <asp:Button ID="btnRoleCancel" runat="server" Text="Cancelar" Enabled="false" CssClass="btnSmall" OnClick="btnRoleCancel_Click" />
            </td>
            <td></td>
        </tr>
    </table>

</asp:Content>

