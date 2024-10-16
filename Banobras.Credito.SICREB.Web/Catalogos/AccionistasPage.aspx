<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AccionistasPage.aspx.cs" Inherits="AccionistasPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">

        function msj() {

            var chk = document.getElementById('MainContent_RgdAccionistas_ctl00_ctl02_ctl02_ChkTodo');
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
            <td></td>
            <td width="100%" align="right">
                <asp:ImageButton runat="server" ID="btnExportarPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" Width="22px" OnClick="btnExportarPDF_Click" />
                <asp:ImageButton runat="server" ID="btnExportarExcel" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" Width="22px" OnClick="btnExportarExcel_Click" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitulo" Text="ACCIONISTAS"></asp:Label>
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
                    Agregar Accionistas desde Excel
                    <asp:FileUpload runat="server" ID="fileUpCargaAccionistasExcel" />
                    <asp:Button runat="server" ID="btnCargaAccionistasExcel" 
                        Text="Cargar Accionistas Excel" onclick="btnCargaAccionistasExcel_Click"/>
                    
                
                    <br />
                    Agregar Direcciones
                    <asp:FileUpload runat="server" ID="fluDirecciones" />
                    <asp:Button runat="server" ID="btnCarcarDirecciones" Text="Cargar Direcciones" OnClick="btnCargarDirecciones_Click" />
                    
                    <br />
                    Agregar Tipos de Accionistas
                    <asp:FileUpload runat="server" ID="fluTipos" />
                    <asp:Button runat="server" ID="btnCargarTipos" Text="Cargar Tipos de Accionistas" OnClick="btnCargarTipos_Click" />
                    
                    <span style="left:300px; width:90px; position:relative">
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" OnClientClick="return msj();" />
                    </span>
                </div>

                <telerik:RadGrid runat="server" ID="RgdAccionistas" AutoGenerateColumns="false" AllowSorting="true"
                                 AllowPaging="true" PageSize="10" OnNeedDataSource="RgdAccionistas_NeedDataSource"
                                 AllowFilteringByColumn="True" OnEditCommand="RgdAccionistas_EditCommand" OnUpdateCommand="RgdAccionistas_UpdateCommand"
                                 OnColumnCreated="RgdAccionistas_ColumnCreated" OnInsertCommand="RgdAccionistas_InsertCommand"
                                 OnItemDataBound="RgdAccionistas_ItemDataBound" OnItemCreated="RgdAccionistas_ItemCreated">
                    <ExportSettings FileName="Accionista" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Accionista" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1},  <span id='tot'> {5}</span>  Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    
                    <MasterTableView Name="MtvAccionistas" DataKeyNames="Id" NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace" Width="2500px">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Accionista" RefreshText="Actualizar Datos" />
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
                            <telerik:GridBoundColumn HeaderText="RFC Acreditado" DataField="RfcAcreditado" UniqueName="RfcAcreditado" HeaderStyle-HorizontalAlign="Justify" 
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="15">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="RFC Accionista" DataField="RfcAccionista" UniqueName="RfcAccionista" HeaderStyle-HorizontalAlign="Justify" 
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="15">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Nombre Compañía" DataField="NombreCompania" UniqueName="NombreCompania" HeaderStyle-HorizontalAlign="Justify" 
                                                     DataType="System.String" HeaderStyle-Width="200px" ItemStyle-Width="200px" FilterControlWidth="190px" MaxLength="150">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Nombre" DataField="Nombre" UniqueName="Nombre" HeaderStyle-HorizontalAlign="Justify" 
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px"  MaxLength="30">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Segundo Nombre" DataField="SNombre"  UniqueName="SNombre" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="30">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Apellido Paterno" DataField="ApellidoP" UniqueName="ApellidoP" HeaderStyle-HorizontalAlign="Justify" 
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="25" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Apellido Materno" DataField="ApellidoM" UniqueName="ApellidoM" HeaderStyle-HorizontalAlign="Justify" 
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="25">
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn HeaderText="Porcentaje" DataField="Porcentaje" UniqueName="Porcentaje" HeaderStyle-HorizontalAlign="Justify" ItemStyle-HorizontalAlign="Right" 
                                                     DataType="System.Int32" HeaderStyle-Width="60px" ItemStyle-Width="60px" FilterControlWidth="50px" MaxLength="2" >
                            </telerik:GridNumericColumn>

                            <telerik:GridBoundColumn HeaderText="Direccion" DataField="Direccion" UniqueName="Direccion" HeaderStyle-HorizontalAlign="Justify" 
                                                     DataType="System.String" HeaderStyle-Width="200px" ItemStyle-Width="200px" FilterControlWidth="190px" MaxLength="40">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Colonia o Poblacion" DataField="ColoniaPoblacion" UniqueName="ColoniaPoblacion" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="60" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Delegacion o Municipio" DataField="DelegacionMunicipio" UniqueName="DelegacionMunicipio" HeaderStyle-HorizontalAlign="Center"
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="40" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Ciudad" DataField="Ciudad" UniqueName="Ciudad" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="40" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Estado" DataField="EstadoMexico" UniqueName="EstadoMexico" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" HeaderStyle-Width="60px" ItemStyle-Width="60px" FilterControlWidth="50px" MaxLength="4" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Codigo Postal" DataField="CodigoPostal" UniqueName="CodigoPostal" HeaderStyle-HorizontalAlign="Justify" ItemStyle-HorizontalAlign="Right"
                                                     DataType="System.String" HeaderStyle-Width="60px" ItemStyle-Width="60px" FilterControlWidth="50px" MaxLength="10" >
                            </telerik:GridBoundColumn>  
                            <telerik:GridBoundColumn HeaderText="Estado en el Extranjero" DataField="EstadoExtranjero" UniqueName="EstadoExtranjero" HeaderStyle-HorizontalAlign="Center"
                                                     DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" MaxLength="40" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Pais Origen Domicilio" DataField="PaisOrigenDomicilio" UniqueName="PaisOrigenDomicilio" HeaderStyle-HorizontalAlign="Center"
                                                     DataType="System.String" HeaderStyle-Width="60px" ItemStyle-Width="60px" FilterControlWidth="50px" MaxLength="2" >
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="Persona" DataField="Persona" UniqueName="Persona" HeaderStyle-HorizontalAlign="Justify"
                                                     DataType="System.String" MaxLength="1" Visible="false">                                
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Persona" UniqueName="TipoPersonaTemp" AllowFiltering="false" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboPersona">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Moral" Value="M" />
                                            <telerik:RadComboBoxItem Text="Física" Value="F" />                                            
                                            <telerik:RadComboBoxItem Text="Fondo o Fideicomiso" Value="O" />
                                            <telerik:RadComboBoxItem Text="Gobierno" Value="G" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

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