<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="CreditosCFiduciariaPage.aspx.cs" Inherits="CreditosCFiduciariaPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">

        function msj() 
        {
            var chk = document.getElementById('MainContent_RgdFiduciaria_ctl00_ctl02_ctl02_ChkTodo');
            var total = document.getElementById('tot');

            var chks = getElementsByClassNameIE("chks");
            if (chk.checked) 
            {
                var answer = confirm("Esta seguro que desea eliminar " + total.innerHTML + " Registros de Esta Tabla");
                if (answer) 
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            else 
            {
                var num = 0;
                for (i = 0, j = 0; i < chks.length; i++) 
                {
                    chk1 = chks[i].getElementsByTagName('input')[0];
                    if (chk1.checked) num++;
                }

                if (num == 0) 
                {
                    alert("Debe seleccionar al menos un registro para eliminar");
                    return false;
                }
                else 
                {
                    var answer = confirm("Esta seguro que desea eliminar " + num + " Registros de Esta Tabla");
                    if (answer) 
                    {
                        return true;
                    }
                    else 
                    {
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
                <asp:ImageButton runat="server" ID="btnExportPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" Width="22px" OnClick="btnExportPDF_Click1" />
                <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" Width="22px" OnClick="ImageButton1_Click" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;
                <asp:Label runat="server" ID="lblTitle" Text="CRÉDITOS EN CONTABILIDAD FIDUCIARIA" />
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
                    <span style="right: 180px; width: 30px; position: absolute">
                        <asp:Button runat="server" ID="btn_eliminar" Text="Eliminar" OnClick="btn_eliminar_Click" OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdFiduciaria" AutoGenerateColumns="false" AllowSorting="true"
                                    AllowPaging="true" PageSize="10" OnNeedDataSource="RgdFiduciaria_NeedDataSource"
                                    AllowFilteringByColumn="True" OnEditCommand="RgdFiduciaria_EditCommand" OnUpdateCommand="RgdFiduciaria_UpdateCommand"
                                    OnColumnCreated="RgdFiduciaria_ColumnCreated" OnDeleteCommand="RgdFiduciaria_DeleteCommand"
                                    OnInsertCommand="RgdFiduciaria_InsertCommand" OnItemDataBound="RgdFiduciaria_ItemDataBound">
                    <ExportSettings FileName="CreditosContabilidadFiduciaria" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="CreditosContabilidadFiduciaria" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'>{5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvExceptuados" DataKeyNames="Id" NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar crédito" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="10%" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Id" UniqueName="Id" DataField="Id" DataType="System.Int32" MaxLength="10" Visible="false" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify" >
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn HeaderText="Crédito" UniqueName="Credito" DataField="Credito" DataType="System.String" MaxLength="50" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify">
                            </telerik:GridNumericColumn>
                            <telerik:GridBoundColumn HeaderText="RFC" UniqueName="RFC" DataField="RFC" DataType="System.String" MaxLength="15" HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Justify">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Nombre" UniqueName="Nombre" DataField="Nombre" DataType="System.String" MaxLength="50" HeaderStyle-Width="40%" ItemStyle-Width="40%" HeaderStyle-HorizontalAlign="Justify">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Estatus" UniqueName="EstatusTemp" AllowFiltering="false" SortExpression="EstatusTemp" Visible="false">
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
                            <telerik:GridBoundColumn HeaderText="Estatus" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify" UniqueName="Estatus" DataField="Estatus" DataType="System.String" MaxLength="10" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Seleccionar" AllowFiltering="false" HeaderStyle-Width="10%" >
                                <HeaderTemplate>
                                    <asp:CheckBox runat="server" ID="ChkTodo" Text="TODOS" AutoPostBack="true" OnCheckedChanged="ChkTodo_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chk" CssClass="chks" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    &nbsp;
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>

</asp:Content>
