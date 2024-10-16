<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="BonoCuponCeroPage.aspx.cs" Inherits="BonoCuponCeroPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">

        function msj() {

            var chk = document.getElementById('MainContent_RgdCuponCero_ctl00_ctl02_ctl02_ChkTodo');
            var total = document.getElementById('tot');
            var chks = getElementsByClassNameIE("chks");

            if (chk.checked) {
                var answer = confirm("Esta seguro que desea eliminar " + total.innerHTML + " registros de esta tabla");
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
                    if (chk1.checked) num++;
                }

                if (num == 0) {
                    alert("Debe seleccionar al menos un registro para eliminar");
                    return false;
                }
                else {
                    var answer2 = confirm("Esta seguro que desea eliminar " + num + " registros de esta tabla");
                    if (answer2) {
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

    <table cellpadding="0" cellspacing="0" border="0" width="100%" >
        <tr>
            <td></td>
            <td align="right">
                <asp:ImageButton runat="server" ID="btnExportarPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"  Width="22px" OnClick="btnExportarPDF_Click" />
                <asp:ImageButton runat="server" ID="btnExportarExcel" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"  Width="22px" OnClick="btnExportarExcel_Click" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitulo" Text="BONO CUPON CERO"></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
    </table>

    <table cellpadding="0" cellspacing="0" border="0" width="100%" style="table-layout:fixed;">
        <tr>
            <td colspan="3" id="td1" >
                <div class="textEntry">
                    Agregar Registros
                    <asp:FileUpload runat="server" ID="fluArchivo" />
                    <asp:Button runat="server" ID="btnCargar" Text="Cargar" OnClick="btnCargar_Click" />
                    <span style="left:555px; width:90px; position:relative">
                        <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" OnClick="btnEliminar_Click" OnClientClick="return msj();" />
                    </span>
                </div>

                <telerik:RadGrid runat="server" ID="RgdCuponCero" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" 
                                PageSize="10" AllowFilteringByColumn="True" OnNeedDataSource="RgdCuponCero_NeedDataSource"
                                OnItemDataBound="RgdCuponCero_ItemDataBound" OnInsertCommand="RgdCuponCero_InsertCommand"
                                OnUpdateCommand="RgdCuponCero_UpdateCommand" >
                    <ExportSettings FileName="Bonos_Cupon_Cero" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Bono Cupon Cero" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1},  <span id='tot'> {5}</span>  Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    
                    <MasterTableView Name="MtvAvales" DataKeyNames="Id" NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace" Width="1070px" >
                        <CommandItemSettings AddNewRecordText="Agregar Bono Cupon Cero" ShowRefreshButton="true" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="50px" ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif" EditText="Editar" HeaderText="Editar"
                                InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png" InsertText="Insertar"
                                UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png" UpdateText="Actualizar"
                                CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png" CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Id" DataField="Id" UniqueName="Id" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.Int32" MaxLength="5" Visible="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Credito" DataField="Credito" UniqueName="Credito" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" HeaderStyle-Width="150px" ItemStyle-Width="150px" FilterControlWidth="90px" MaxLength="25" ItemStyle-HorizontalAlign="Right">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="RFC" DataField="Rfc" UniqueName="Rfc" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" HeaderStyle-Width="150px" ItemStyle-Width="150px" FilterControlWidth="90px" MaxLength="15">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Nombre del Acreditado" DataField="NombreAcreditado" UniqueName="NombreAcreditado" HeaderStyle-HorizontalAlign="Justify" 
                                                     DataType="System.String" HeaderStyle-Width="400px" ItemStyle-Width="400px" FilterControlWidth="370px" MaxLength="150">                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Monto de la Inversion" DataField="MontoInversion" UniqueName="MontoInversion" HeaderStyle-HorizontalAlign="Justify" ItemStyle-HorizontalAlign="Right"
                                                     DataType="System.Double" HeaderStyle-Width="150px" ItemStyle-Width="150px" FilterControlWidth="90px" MaxLength="10" >
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false" HeaderStyle-Width="100px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" >
                                <HeaderTemplate>
                                    <asp:CheckBox runat="server" ID="ChkTodo" Text="TODOS" AutoPostBack="true" OnCheckedChanged="ChkTodo_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chk" CssClass="chks" />
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
