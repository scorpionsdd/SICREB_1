<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="EstadoCivilPage.aspx.cs" Inherits="EstadoCivilPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">
        
        function msj() {

            var chk = document.getElementById('MainContent_RgdEstadoCivil_ctl00_ctl02_ctl02_ChkTodo');
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
            <td id="td2" class="titleLeft">
                &nbsp;
            </td>
            <td id="td3" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitulo" Text="ESTADO CIVIL"></asp:Label>
            </td>
            <td id="td4" class="titleRight">
                &nbsp;
            </td>
        </tr>
    </table>

    <table cellpadding="0" cellspacing="0" border="0" width="100%" style="table-layout:fixed;">
        <tr>
            <td colspan="3" id="td5" >
                <div class="textEntry">
                    Agregar Registros
                    <asp:FileUpload runat="server" ID="fluArchivo" />
                    <asp:Button runat="server" ID="btnCargar" Text="Cargar" OnClick="btnCargar_Click" />
                    <span style="left:555px; width:90px; position:relative">
                        <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" OnClick="btnEliminar_Click" OnClientClick="return msj();" />
                    </span>
                </div>

                <telerik:RadGrid runat="server" ID="RgdEstadoCivil" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" 
                                PageSize="10" AllowFilteringByColumn="True" OnNeedDataSource="RgdEstadoCivil_NeedDataSource"
                                OnUpdateCommand="RgdEstadoCivil_UpdateCommand" OnInsertCommand="RgdEstadoCivil_InsertCommand" 
                                OnItemDataBound="RgdEstadoCivil_ItemDataBound" >
                    <ExportSettings FileName="EstadoCivil" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="EstadoCivil" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1},  <span id='tot'> {5}</span>  Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    
                    <MasterTableView Name="MtvEstadoCivil" DataKeyNames="Id" NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace" Width="100%" >
                        <CommandItemSettings AddNewRecordText="Agregar Estado Civil" ShowRefreshButton="true" RefreshText="Actualizar Datos" />
                        <Columns>

                            <telerik:GridEditCommandColumn HeaderStyle-Width="35px" ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif" EditText="Editar" HeaderText="Editar"
                                InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png" InsertText="Insertar"
                                UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png" UpdateText="Actualizar"
                                CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png" CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Id" DataField="Id" UniqueName="Id" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.Int32" MaxLength="5" Visible="false" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Id CLIC" DataField="IdClic" UniqueName="IdClic" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.Int32" HeaderStyle-Width="50px" ItemStyle-Width="50px" FilterControlWidth="40px" MaxLength="2">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Descripcion CLIC" DataField="DescripcionClic" UniqueName="DescripcionClic" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" HeaderStyle-Width="300px" ItemStyle-Width="300px" FilterControlWidth="280px" MaxLength="50">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Clave Buro" DataField="ClaveBuro" UniqueName="ClaveBuro" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" HeaderStyle-Width="50px" ItemStyle-Width="50px" FilterControlWidth="40px" MaxLength="2" ItemStyle-HorizontalAlign="Right">
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false" HeaderStyle-Width="60px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" >
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
