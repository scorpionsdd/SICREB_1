<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AvisoRechazo.aspx.cs" Inherits="AvisoRechazo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <p class="RadComboBox_rtl">
        &nbsp;
    </p>
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
                <asp:Label runat="server" ID="lblTitle" Text="VALIDACIONES" />
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
    </table>

    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td colspan="3" id="td1" width="100%">
                <telerik:RadGrid runat="server" ID="RgdAvisoRechazos" AutoGenerateColumns="false"  OnCancelCommand = "RgdAvisoRechazos_CancelCommand"
                    AllowSorting="true" AllowPaging="true" PageSize="10" OnNeedDataSource="RgdAvisoRechazos_NeedDataSource"
                    AllowFilteringByColumn="True" OnEditCommand="RgdAvisoRechazos_EditCommand" OnUpdateCommand="RgdAvisoRechazos_UpdateCommand"
                    OnItemDataBound="RgdAvisoRechazos_ItemDataBound" OnItemCommand="RgdAvisoRechazos_ItemCommand">
                    <ExportSettings FileName="Validaciones" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Validaciones" />
                    </ExportSettings>
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvAvisoRechazos" NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="true" ShowAddNewRecordButton="false" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridBoundColumn  HeaderText="Id" UniqueName="Id" DataField="Id" HeaderStyle-Width="0%" HeaderStyle-HorizontalAlign="Justify" DataType="System.Int32" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridEditCommandColumn HeaderStyle-Width="4%" ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                EditImageUrl="../App_Themes/Banobras2011/Grid/Edit.gif" EditText="Editar" HeaderText="Editar"
                                InsertImageUrl="../App_Themes/Banobras2011/Grid/Agregar.png" InsertText="Insertar"
                                UpdateImageUrl="../App_Themes/Banobras2011/Grid/Aceptar.png" UpdateText="Actualizar"
                                CancelImageUrl="../App_Themes/Banobras2011/Grid/Cancelar.png" CancelText="Cancelar">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridCheckBoxColumn HeaderText="Aplicable" UniqueName="Aplicable" DataField="Aplicable" HeaderStyle-Width="6%" HeaderStyle-HorizontalAlign="Justify">
                            </telerik:GridCheckBoxColumn>
                            <telerik:GridBoundColumn HeaderText="Mensaje" UniqueName="Mensaje" DataField="Mensaje" HeaderStyle-Width="35%"  
                                HeaderStyle-HorizontalAlign="Justify" FilterListOptions="VaryByDataType" DataType="System.String" FilterControlWidth = "70%" MaxLength="70" ItemStyle-Width="90%">                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Tipo" UniqueName="Tipo" DataField="Tipo" HeaderStyle-Width="7%" 
                                HeaderStyle-HorizontalAlign="Justify" DataType="System.String"  Visible="true" >
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="FiltroRadCombo" runat="server" AppendDataBoundItems="true" Width = "90%" AutoPostBack="true"  OnSelectedIndexChanged="FiltroRadCombo_SelectedIndexChanged" />
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Tipo" UniqueName="TipoTemp" HeaderStyle-Width="7%" Visible="false" AllowFiltering="false">
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboTipo" Width = "90%">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Error" Value="1" />
                                            <telerik:RadComboBoxItem Text="Warning" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Persona" UniqueName="Persona" DataField="Persona" HeaderStyle-Width="7%" 
                                HeaderStyle-HorizontalAlign="Justify" AllowFiltering="true" FilterControlWidth = "60%" DataType="System.String" ReadOnly="true" ItemStyle-Width="7%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Código" UniqueName="Codigo" DataField="Codigo" HeaderStyle-Width="7%" 
                                HeaderStyle-HorizontalAlign="Justify" DataType="System.String" FilterControlWidth = "60%"  ReadOnly="true">                                 
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Etiqueta" UniqueName="Etiqueta" DataField="Etiqueta" HeaderStyle-Width="7%"  
                                HeaderStyle-HorizontalAlign="Justify" DataType="System.String" FilterControlWidth = "60%" ReadOnly="true">                                 
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Campo" UniqueName="Campo" DataField="Campo" HeaderStyle-Width="20%" 
                                HeaderStyle-HorizontalAlign="Justify" DataType="System.String" FilterControlWidth = "80%" ReadOnly="true">                                
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Estatus" UniqueName="Estatus" DataField="Estatus" HeaderStyle-Width="7%"  
                                HeaderStyle-HorizontalAlign="Justify" DataType="System.String" ItemStyle-Width="7%" Visible="true" ReadOnly="true">                                 
                                <FilterTemplate>
                                    <telerik:RadComboBox runat="server" ID="FiltroEstatus" AppendDataBoundItems="true" Width ="90%" AutoPostBack="true" OnSelectedIndexChanged="FiltroEstatus_SelectedIndexChanged" />
                                </FilterTemplate>
                            </telerik:GridBoundColumn>                            
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
