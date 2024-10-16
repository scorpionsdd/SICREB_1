<%@ Page MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeFile="AdminHistorico.aspx.cs" Inherits="Reportes_AdminHistorico" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="HISTÓRICO DE CRÉDITO"></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center" style="table-layout: fixed;">
        <tr>
            <td colspan="3" id="td1" width="100%">
            <br />
                <div class="textEntry">
                    
                    Cargar Reportes (txt)
                    <asp:FileUpload ID="file_txt_layout" runat="server" accept=".txt" />
                    <asp:Button ID="btn_cargar" runat="server" Text="Cargar" OnClick="btn_cargar_Click" />
                </div>
                <br />
                <telerik:RadGrid runat="server" ID="RgdCuentas" AutoGenerateColumns="false" AllowScroll="true" OnItemCommand="grids_ItemCommand"
                    AllowSorting="true" AllowPaging="true" PageSize="10" OnNeedDataSource="RgdCuentas_NeedDataSource"
                    AllowFilteringByColumn="True" OnUpdateCommand="RgdCuentas_UpdateCommand"
                    OnDeleteCommand="RgdCuentas_DeleteCommand"
                    OnInsertCommand="RgdCuentas_InsertCommand" OnItemDataBound="RgdCuentas_ItemDataBound">
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
                        <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" AddNewRecordText="Agregar Cuenta" RefreshText="Actualizar Datos" />
                        <Columns>                         
                            <telerik:GridBoundColumn HeaderText="Id" HeaderStyle-HorizontalAlign="Justify" UniqueName="Id"
                                DataField="Id" DataType="System.Int32" MaxLength="3" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Nombre" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="300px" FilterControlWidth="80%"
                                UniqueName="Nombre" DataField="Nombre" DataType="System.String" MaxLength="30">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Fecha" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="200px" FilterControlWidth="80%"
                                UniqueName="Fecha" DataField="Fecha" DataType="System.String" MaxLength="100">
                            </telerik:GridBoundColumn> 
                            
                            <telerik:GridButtonColumn CommandName="download_file" Text="Descargar" UniqueName="Download" HeaderText="Descargar">                                
                            </telerik:GridButtonColumn>         
                            
                            <%--<telerik:GridTemplateColumn DataField="venDocPath" HeaderText="Download" SortExpression="venDocPath" UniqueName="venDocPath">
                                <ItemTemplate>                         
                                    <asp:LinkButton ID="lnkDownload" Enabled="false" Text="Download" CommandArgument='<%#Eval("Nombre")%>' runat="server" OnClick="lnkDownload_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>                            
                                                                          
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