<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="ClasificacionActPage.aspx.cs" Inherits="ClasificacionActPage" %>

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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="CLASIFICACIÓN ACTUAL"></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center" style="table-layout: fixed;">
        <tr>
            <td colspan="3" id="td1" width="100%">
                <!--<div class="textEntry">
                    Agregar Registros
                    <asp:FileUpload ID="file_txt_layout" runat="server" />
                    <asp:Button ID="btn_cargar" runat="server" Text="Cargar" OnClick="btn_cargar_Click" />
                    <span style="right: 150px; width: 90px; position: absolute">
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClick="btn_eliminar_Click"
                            OnClientClick="return msj();" />
                    </span>
                </div>-->                
                <telerik:RadGrid runat="server" ID="RgdCuentas" AutoGenerateColumns="false" 
                    AllowScroll="true" OnItemCommand="grids_ItemCommand"
                    AllowSorting="true" AllowPaging="true" PageSize="10" OnNeedDataSource="RgdCuentas_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdCuentas_EditCommand" OnUpdateCommand="RgdCuentas_UpdateCommand"
                    OnColumnCreated="RgdCuentas_ColumnCreated" OnDeleteCommand="RgdCuentas_DeleteCommand"
                    OnInsertCommand="RgdCuentas_InsertCommand" 
                    OnItemDataBound="RgdCuentas_ItemDataBound" 
                    oncancelcommand="RgdCuentas_CancelCommand">
                     <ExportSettings FileName="Clasificacion_Actual" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Clasificación Actual" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    
                    <MasterTableView Name="MtvCuentas" DataKeyNames="id" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="99%">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" AddNewRecordText="Agregar Cuenta" RefreshText="Actualizar Datos" />                        
                        <Columns>                             
                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-HorizontalAlign="Justify" UniqueName="id"
                                DataField="id" DataType="System.Int32" MaxLength="3" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Descripcion" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="130px" FilterControlWidth="80%"
                                UniqueName="descripcion" DataField="descripcion" DataType="System.String" MaxLength="300">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn  HeaderStyle-Width="7%" HeaderText="Tipo" HeaderStyle-HorizontalAlign="Justify"
                                DataType="System.String" UniqueName="Vigente" DataField="vigente" Visible="true" >
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="FiltroRadCombo" runat="server" AppendDataBoundItems="true" Width = "90%"
                                        AutoPostBack="true"  OnSelectedIndexChanged="FiltroRadCombo_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Todos" Value="-1" />
                                            <telerik:RadComboBoxItem Text="Vigente" Value="1" />
                                            <telerik:RadComboBoxItem Text="Vencido" Value="0" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn  HeaderStyle-Width="7%" HeaderText="Tipo" UniqueName="VigenteTemp"
                                Visible="false" AllowFiltering="false">
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboTipo" Width = "90%">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Vigente" Value="1" />
                                            <telerik:RadComboBoxItem Text="Vencido" Value="0" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
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
