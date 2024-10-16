<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ClientesPF.aspx.cs" Inherits="Catalogos_ClientesPF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
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
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="Catálogo Intermedio de FS-Clientes Personas Físicas"></asp:Label>
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
                    <ExportSettings FileName="Clientes_PF" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Cuentas" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvCuentas" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="99%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Cliente" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="50px" ItemStyle-Width="50px" FooterStyle-Width="50px"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>

                            <telerik:GridBoundColumn HeaderText="RFC" HeaderStyle-HorizontalAlign="Justify" 
                                HeaderStyle-Width="170px" FilterControlWidth="80%" UniqueName="RFC" DataField="RFC" 
                                DataType="System.String" MaxLength="4000" Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CURP" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="CURP" DataField="CURP"
                                DataType="System.String" MaxLength="18">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="NOMBRE" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="170px" FilterControlWidth="80%" UniqueName="NOMBRE" DataField="NOMBRE"
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="APELLIDO PATERNO" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="170px" FilterControlWidth="45%" UniqueName="APELLIDO_PATERNO" DataField="APELLIDO_PATERNO"
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="APELLIDO MATERNO" HeaderStyle-HorizontalAlign="Justify" 
                                HeaderStyle-Width="170px" FilterControlWidth="80%" UniqueName="APELLIDO_MATERNO" DataField="APELLIDO_MATERNO" 
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CLAVE DE NACIONALIDAD " HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="NACIONALIDAD_CLAVE" DataField="NACIONALIDAD_CLAVE"
                                DataType="System.String" MaxLength="1" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="NACIONALIDAD"  HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="NACIONALIDAD" DataField="NACIONALIDAD" 
                                DataType="System.String" MaxLength="30">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CALLE" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="170px" FilterControlWidth="80%" UniqueName="CALLE" DataField="CALLE"
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="NÚM. EXT." HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="NUM_EXT" DataField="NUM_EXT"
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="NÚM. INT." HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="50%" UniqueName="NUM_INT" DataField="NUM_INT"
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="COLONIA" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="170px" UniqueName="COLONIA" DataField="COLONIA" 
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CLAVE DE MUNICIPIO" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" UniqueName="MUNICIPIO_CLAVE" DataField="MUNICIPIO_CLAVE" 
                                DataType="System.String" MaxLength="3">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="MUNICIPIO" HeaderStyle-HorizontalAlign="Justify" 
                                HeaderStyle-Width="170px" FilterControlWidth="80%" UniqueName="MUNICIPIO" DataField="MUNICIPIO" 
                                DataType="System.String" MaxLength="4000" Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CIUDAD" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="170px" FilterControlWidth="80%" UniqueName="CIUDAD" DataField="CIUDAD"
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CLAVE DE ESTADO" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="ESTADO_CLAVE" DataField="ESTADO_CLAVE"
                                DataType="System.String" MaxLength="2">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="ESTADO" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="170px" FilterControlWidth="45%" UniqueName="ESTADO" DataField="ESTADO"
                                DataType="System.String" MaxLength="200">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CÓDIGO POSTAL" HeaderStyle-HorizontalAlign="Justify" 
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="CODIGO_POSTAL" DataField="CODIGO_POSTAL" 
                                DataType="System.String" MaxLength="6">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="TELÉFONOS" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="TELEFONOS" DataField="TELEFONOS"
                                DataType="System.String" MaxLength="80" >
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CLAVE DE PAÍS"  HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="PAIS_CLAVE" DataField="PAIS_CLAVE" 
                                DataType="System.String" MaxLength="3">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="PAÍS" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="PAIS" DataField="PAIS"
                                DataType="System.String" MaxLength="200">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="CLAVE DE TIPO DE CLIENTE" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="TIPO_CLIENTE_CLAVE" DataField="TIPO_CLIENTE_CLAVE"
                                DataType="System.String" MaxLength="1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="TIPO DE CLIENTE" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="50%" UniqueName="TIPO_CLIENTE" DataField="TIPO_CLIENTE"
                                DataType="System.String" MaxLength="50">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="FECHA DE NACIMIENTO" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" UniqueName="FECHA_NAC" DataField="FECHA_NAC" 
                                DataType="System.String" MaxLength="30" Visible="true">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="CLAVE DE ESTADO CIVIL" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" UniqueName="ESTADO_CIVIL_CLAVE" DataField="ESTADO_CIVIL_CLAVE" 
                                DataType="System.String" MaxLength="1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="ESTADO CIVIL" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="170px" FilterControlWidth="80%" UniqueName="ESTADO_CIVIL" DataField="ESTADO_CIVIL" 
                                DataType="System.String" MaxLength="100">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="SEXO" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="SEXO" DataField="SEXO"
                                DataType="System.String" MaxLength="1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CLAVE DE USUARIO" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="80%" UniqueName="USUARIO_ALTA" DataField="USUARIO_ALTA"
                                DataType="System.String" MaxLength="20">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="CONSECUTIVO" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" FilterControlWidth="50%" UniqueName="CONSECUTIVO" DataField="CONSECUTIVO"
                                DataType="System.String" MaxLength="5">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="ESTATUS" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" UniqueName="ESTATUS" DataField="ESTATUS" 
                                DataType="System.String" MaxLength="1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="ID TIPO DE CLIENTE" HeaderStyle-HorizontalAlign="Justify"
                                HeaderStyle-Width="100px" UniqueName="ID_TIPO_CLIENTE" DataField="ID_TIPO_CLIENTE" 
                                DataType="System.String" MaxLength="2">
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

