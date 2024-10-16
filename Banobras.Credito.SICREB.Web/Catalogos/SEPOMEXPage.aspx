<%@ Page AsyncTimeout="1200" Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeFile="SEPOMEXPage.aspx.cs"
    Inherits="Catalogos_SEPOMEXPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td>
            </td>
            <td width="100%" align="right">
                <a href="http://www.sepomex.gob.mx/lservicios/servicios/CodigoPostal_Exportar.aspx"
                    target="_blank">
                    <asp:Image ID="imgExportaPDF" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                        Width="22px" />
                    <%--<asp:ImageButton ID="btnExportPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                        runat="server" Width="22px" OnClick="btnExportPDF_Click" />--%>
                </a><a href="http://www.sepomex.gob.mx/lservicios/servicios/CodigoPostal_Exportar.aspx"
                    target="_blank">
                    <asp:Image ID="ImageButton1" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                        runat="server" Width="22px" />
                </a>
            </td>
            <td>
            </td>
        </tr>
        <tr style="width: 100%">
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="SEPOMEX"></asp:Label>
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
                    <asp:FileUpload ID="file_txt_sepomex" runat="server" />
                    <asp:Button ID="btn_cargar_sepomex" runat="server" CausesValidation="false" Text="Cargar"
                        OnClick="btn_cargar_sepomex_Click" />
                </div>
                <telerik:RadGrid runat="server" ID="RgdSEPOMEX" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" PageSize="10" OnNeedDataSource="RgdSEPOMEX_NeedDataSource"
                    AllowFilteringByColumn="True" OnUpdateCommand="RgdSEPOMEX_UpdateCommand">
                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                        PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                        PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView Name="MtvSEPOMEX" NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top"
                        EditMode="InPlace" Width="100%">
                        <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" RefreshText="Actualizar Datos" />
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="ID" HeaderStyle-HorizontalAlign="Justify" UniqueName="ID" 
                                DataField="ID" DataType="System.Int32" MaxLength="3" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Código Postal" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="10%"
                                UniqueName="DCODIGO" DataField="DCODIGO" DataType="System.String" MaxLength="10" FilterControlWidth = "75%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Colonia" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="16%"
                                UniqueName="DASENTA" DataField="DASENTA" DataType="System.String" MaxLength="100"  FilterControlWidth = "80%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Tipo Asenta" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="15%"
                                UniqueName="DTIPOASENTA" DataField="DTIPOASENTA" DataType="System.String" MaxLength="100" FilterControlWidth = "80%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Municipio" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="16%"
                                UniqueName="DMNPIO" DataField="DMNPIO" DataType="System.String  " MaxLength="100" FilterControlWidth = "80%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Estado" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="15%"
                                UniqueName="DESTADO" DataField="DESTADO" DataType="System.String  " MaxLength="50" FilterControlWidth = "80%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Cuidad" HeaderStyle-HorizontalAlign="Justify" 
                                UniqueName="DCIUDAD" DataField="DCIUDAD" DataType="System.String  " MaxLength="100"
                                Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Código Postal" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="DCP" DataField="DCP" Visible="false" DataType="System.String  " MaxLength="10">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="C. ESTADO" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="CESTADO" DataField="CESTADO" Visible="false" DataType="System.String  "
                                MaxLength="5">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="C. OFICINA" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="COFICINA" DataField="COFICINA" Visible="false" DataType="System.String  "
                                MaxLength="10">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="C. Código Postal" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="CCP" DataField="CCP" Visible="false" DataType="System.String  " MaxLength="10">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="C. TIPO ASENTA" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="CTIPOASENTA" DataField="CTIPOASENTA" Visible="false" DataType="System.String"
                                MaxLength="5">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="C. Municipio" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="CMNPIO" DataField="CMNPIO" Visible="false" DataType="System.String  "
                                MaxLength="6">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="ID ASENTA CPCONS" HeaderStyle-HorizontalAlign="Justify"
                                UniqueName="IDASENTACPCONS" DataField="IDASENTACPCONS" Visible="false" DataType="System.String  "
                                MaxLength="8">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Zona" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="15%"
                                UniqueName="DZONA" DataField="DZONA" DataType="System.String  " MaxLength="6" FilterControlWidth = "80%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Clave Cuidad" HeaderStyle-HorizontalAlign="Justify" HeaderStyle-Width="12%"
                                UniqueName="CCVECIUDAD" DataField="CCVECIUDAD" DataType="System.String  " MaxLength="6" FilterControlWidth = "75%">
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
