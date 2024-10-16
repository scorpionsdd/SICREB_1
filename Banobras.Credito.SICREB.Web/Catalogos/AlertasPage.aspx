<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="AlertasPage.aspx.cs" Inherits="Catalogos_AlertasPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function msj() {

            var chk = document.getElementById('MainContent_RgdALERTAS_ctl00_ctl02_ctl02_ChkTodo');
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
                    runat="server" Width="22px" OnClick="btnExportPDF_Click" />
                <asp:ImageButton ID="ImageButton1" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                    runat="server" Width="22px" OnClick="btnExportExcel_Click" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="ALERTAS"></asp:Label>
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
                    <span style="right: 120px; width: 90px; position: absolute">
                        <asp:Button ID="btn_eliminar" runat="server" Text="Eliminar" OnClick="btn_eliminar_Click"
                            OnClientClick="return msj();" />
                    </span>
                </div>
                <telerik:RadGrid runat="server" ID="RgdALERTAS" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" PageSize="10" OnNeedDataSource="RgdALERTAS_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdALERTAS_EditCommand" OnUpdateCommand="RgdALERTAS_UpdateCommand"
                    OnColumnCreated="RgdALERTAS_ColumnCreated" OnDeleteCommand="RgdALERTAS_DeleteCommand"
                    OnInsertCommand="RgdALERTAS_InsertCommand" OnItemDataBound="RgdALERTAS_ItemDataBound">
                    <ExportSettings FileName="Alertas" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Alertas" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span>  Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvALERTA" DataKeyNames="ID" NoMasterRecordsText="No existen Registros"
                        CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" AddNewRecordText="Agregar Alerta" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="10%" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif"
                                EditText="Editar" HeaderText="Editar" InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png"
                                InsertText="Insertar" UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png"
                                UpdateText="Actualizar" CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png"
                                CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="ID" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="ID" DataField="ID" Display="false"
                                DataType="System.Int32" MaxLength="3" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Identificador de Alerta" HeaderStyle-Width="20%"
                                ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Justify" UniqueName="CLAVEBURO"
                                DataField="IdententifadorAlerta" DataType="System.String" MaxLength="1000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Título Alerta" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="CLAVECLIC" DataField="TituloAlerta"
                                DataType="System.String" MaxLength="100">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Mensaje" HeaderStyle-Width="50%" ItemStyle-Width="50%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="MENSAJE" DataField="Mensaje"
                                DataType="System.String" MaxLength="4000">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Periodicidad" HeaderStyle-Width="50%" ItemStyle-Width="50%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="DescripcionPeriodo" DataField="Periodicidad"
                                DataType="System.String" MaxLength="100">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Periodicidad" UniqueName="Periodo" AllowFiltering="false"
                                SortExpression="PeriodoTemp" Visible="false">
                                <HeaderStyle Width="50%" HorizontalAlign="Justify" />
                                <ItemStyle Width="50%" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPeriodicidad" Text='<%# Eval("Periodicidad") %>'
                                        Visible="true"></asp:Label>
                                    <telerik:RadComboBox runat="server" AutoPostBack="true" ID="ComboPeriodo" OnSelectedIndexChanged="ComboPeriodo_SelectedIndexChanged"
                                        Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Semanal" Value="1" />
                                            <telerik:RadComboBoxItem Text="Mensual" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Aplicación de periodicidad" HeaderStyle-Width="50%"
                                ItemStyle-Width="50%" HeaderStyle-HorizontalAlign="Justify" UniqueName="descripcionAplicaPeriodo"
                                DataField="AplicacionDePeriodicidad" DataType="System.String" MaxLength="50">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Aplicación de periodicidad" UniqueName="AplicaPeriodo"
                                AllowFiltering="false" SortExpression="AplicaPeriodoTemp" Visible="false">
                                <HeaderStyle Width="50%" HorizontalAlign="Justify" />
                                <ItemStyle Width="50%" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDiaPeriodo" Text='<%# Eval("AplicacionDePeriodicidad") %>'
                                        Visible="true"></asp:Label>
                                    <telerik:RadComboBox runat="server" ID="ComboDiaPeriodo" OnSelectedIndexChanged="ComboDiaPeriodo_OnSelectedIndexChanged"
                                        Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Lunes" Value="1" />
                                            <telerik:RadComboBoxItem Text="Martes" Value="2" />
                                            <telerik:RadComboBoxItem Text="Miércoles" Value="3" />
                                            <telerik:RadComboBoxItem Text="Jueves" Value="4" />
                                            <telerik:RadComboBoxItem Text="Viernes" Value="5" />
                                            <telerik:RadComboBoxItem Text="Sabado" Value="6" />
                                            <telerik:RadComboBoxItem Text="Domingo" Value="7" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Activada" HeaderStyle-Width="50%" ItemStyle-Width="50%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="DescripcionActivadaSINO" DataField="AlarmaActivada"
                                DataType="System.String" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Activar Alerta" UniqueName="ActivadaSINO" AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAlerta" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ChkTodo" Text="TODOS" AutoPostBack="true" OnCheckedChanged="ChkTodo_CheckedChanged"
                                        runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" CssClass="chks" />
                                </ItemTemplate>
                                <EditItemTemplate>&nbsp;</EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
