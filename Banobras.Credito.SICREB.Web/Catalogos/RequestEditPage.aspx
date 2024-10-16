<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="RequestEditPage.aspx.cs" Inherits="RequestEditPage" %>

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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="REQUEST"></asp:Label>
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
                    <span style="right: 300px; width: 90px; position: absolute">
                        <asp:Button ID="btn_resetear" runat="server" Text="Resetear" OnClick="btn_resetear_Click" OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdCuentas" AutoGenerateColumns="false" AllowScroll="true" OnItemCommand="grids_ItemCommand"
                    AllowSorting="true" AllowPaging="true" PageSize="10" OnNeedDataSource="RgdCuentas_NeedDataSource"
                    AllowFilteringByColumn="True" OnItemDataBound="RgdCuentas_ItemDataBound">
                     <ExportSettings FileName="Request" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Request" />
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
                            <telerik:GridBoundColumn HeaderText="Fecha Inicio" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="160px"
                                UniqueName="fecha_inicio" DataField="fecha_inicio" DataType="System.String" ReadOnly="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Fecha Final" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="160px"
                                UniqueName="fecha_final" DataField="fecha_final" DataType="System.String" ReadOnly="true">
                            </telerik:GridBoundColumn>                                                        
                            <telerik:GridBoundColumn HeaderText="Estado" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="130px"
                                UniqueName="estado" DataField="estado" DataType="System.String" MaxLength="20">
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="FiltroRadCombo" runat="server" Width="100px" AppendDataBoundItems="true" AutoPostBack="true"  OnSelectedIndexChanged="FiltroRadCombo_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Todos" Value="0" />
                                            <telerik:RadComboBoxItem Text="COMPLETO" Value="COMPLETO" />
                                            <telerik:RadComboBoxItem Text="ERROR" Value="ERROR" />
                                            <telerik:RadComboBoxItem Text="INICIO" Value="INICIO" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Archivo PM" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="100px"
                                UniqueName="id_archivo_pm" DataField="id_archivo_pm" DataType="System.String" ReadOnly="true" FilterControlWidth="60%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Archivo PF" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="100px"
                                UniqueName="id_archivo_pf" DataField="id_archivo_pf" DataType="System.String" ReadOnly="true" FilterControlWidth="60%">
                            </telerik:GridBoundColumn>                            
                            <telerik:GridBoundColumn HeaderText="Reporte" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="80px" FilterControlWidth="60%" UniqueName="reporte" DataField="reporte" DataType="System.String" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Notificaciones" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="130px"
                                UniqueName="notificaciones" DataField="notificaciones" DataType="System.String" ReadOnly="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Grupos" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="130px"
                                UniqueName="grupos" DataField="grupos" DataType="System.String" ReadOnly="true">
                            </telerik:GridBoundColumn>                            
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
