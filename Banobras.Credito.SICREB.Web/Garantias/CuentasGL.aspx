<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="CuentasGL.aspx.cs" Inherits="CuentasGL" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content Runat="Server" ID="Content1" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content Runat="Server" ID="Content2" ContentPlaceHolderID="MainContent">

    <script type="text/javascript">

        function msj() {

            var chk = document.getElementById('MainContent_RgdCuentas_ctl00_ctl02_ctl02_ChkTodo');
            var total = document.getElementById('tot');
            var chks = getElementsByClassNameIE("chks");

            if (chk.checked) {
                var answer = confirm("Esta seguro que desea eliminar " + total.innerHTML + " Registros del Catalogo de Créditos Exeptuados");
                if (answer) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                var num = 0;
                for (i = 0, j = 0; i < chks.length; i++) {
                    chk1 = chks[i].getElementsByTagName('input')[0];
                    if (chk1.checked)
                    { num++; }
                }

                if (num == 0) {
                    alert("Debe seleccionar al menos un registro para eliminar");
                    return false;
                }
                else {
                    var answer = confirm("Esta seguro que desea eliminar " + num + " registros del Catalogo de Créditos Exeptuados");
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
                <asp:ImageButton runat="server" ID="btnExportarPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" Width="22px" OnClick="btnExportarPDF_Click" />
                <asp:ImageButton runat="server" ID="btnExportarXLS" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" Width="22px" OnClick="btnExportarXLS_Click" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">&nbsp;</td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp; <asp:Label runat="server" ID="lblTitle" Text="Catalogo de Cuentas (Lineas y Garantias)" />
            </td>
            <td id="tdRubroDer" class="titleRight">&nbsp;</td>
        </tr>
    </table>

    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center" style="table-layout:fixed;">
        <tr>
            <td colspan="3" id="td1" width="100%">
                <div class="textEntry" >
                    <%--Agregar Registros--%>
                    <asp:FileUpload runat="server" ID="fluArchivo" style="display:none;" />
                    <asp:Button runat="server" ID="btnCargar" Text="Cargar" style="display:none;" />
                    <span style="float:right;">
                        <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" OnClientClick="return msj();" OnClick="btnEliminar_Click" />
                    </span>
                </div>
                
                <telerik:RadGrid runat="server" ID="RgdCuentas" AutoGenerateColumns="false" AllowScroll="true" 
                    AllowSorting="true" AllowPaging="true" PageSize="10" AllowFilteringByColumn="True"
                    OnNeedDataSource="RgdCuentas_NeedDataSource" OnUpdateCommand="RgdCuentas_UpdateCommand" OnInsertCommand="RgdCuentas_InsertCommand" ClientSettings-ClientEvents-OnKeyPress="RgdCuentas_KeyPress">
                    <ExportSettings FileName="CuentasGL" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Cuentas (Lineas y Garantias)" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvCuentas" DataKeyNames="CUENTA_ID" NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace" Width="99%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Crédito" RefreshText="Actualizar Datos" />
                        <Columns>
                             <telerik:GridEditCommandColumn HeaderStyle-Width="50px" ItemStyle-Width="50px" FooterStyle-Width="50px"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>                           
                            <telerik:GridBoundColumn HeaderText="Id"          DataField="CUENTA_ID"   UniqueName="ID" HeaderStyle-HorizontalAlign="Justify" DataType="System.Int32" MaxLength="3" Visible="false" />                            
                            <telerik:GridBoundColumn HeaderText="Código"      DataField="CODIGO"      UniqueName="Codigo" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="170px" FilterControlWidth="80%" DataType="System.String" MaxLength="30" />                            
                            <telerik:GridBoundColumn HeaderText="Descripción" DataField="DESCRIPCION" UniqueName="Descripcion" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="300px" FilterControlWidth="80%" DataType="System.String" MaxLength="200" />                                                                                                                                   
                            <telerik:GridTemplateColumn HeaderText="Seleccionar" AllowFiltering="false" HeaderStyle-Width="80px" >
                                <HeaderTemplate>
                                    <asp:CheckBox runat="server" ID="ChkTodo" Text="TODOS" AutoPostBack="true" OnCheckedChanged="ChkTodo_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" CssClass="chks" />
                                </ItemTemplate>
                                <EditItemTemplate>&nbsp;</EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True" />                        
                    </ClientSettings>
                </telerik:RadGrid>
            </td>
        </tr>        
    </table>

</asp:Content>



