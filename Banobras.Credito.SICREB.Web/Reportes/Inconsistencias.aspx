<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Inconsistencias.aspx.cs" Inherits="Reportes_Inconsistencias" Theme="Banobras2011" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table width="100%" cellpadding ="0" cellspacing ="0" >
        <tr>
            <td id="td1" class="titleLeft">&nbsp;</td>
            <td id="td2" colspan="5"  class="titleCenter" ><asp:Label Text="FILTROS" runat="server" ID="Label1"></asp:Label></td>
            <td id="td3" class="titleRight">&nbsp;</td>
        </tr>
        <tr>
            <td style="width:1%"></td>
            <td>
                <%--<asp:RadioButton ID="rbtnPersonaMoral" runat="server" Text="Persona Moral"/>&nbsp;&nbsp;<asp:RadioButton ID="rbtnPersonaFisica"
                    runat="server" Text="Persona Fisica"/>--%>
                <asp:RadioButtonList ID="rbPersona" runat="server" RepeatDirection="Horizontal" 
                        onselectedindexchanged="rbPersona_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Selected="True" Value="PM">Persona Moral</asp:ListItem>
                            <asp:ListItem Value="PF">Persona Física</asp:ListItem>
                    </asp:RadioButtonList>
            </td>
            <td style="width:1%"></td>
        </tr>
        <tr>
            <td></td>
        </tr>
    </table>

    <div><asp:Label ID="lblAux" runat="server" Visible="False"></asp:Label></div>

    <table width="100%" cellpadding ="0" cellspacing ="0" >
        <tr>
            <td id="tdRubroIzq" class="titleLeft">&nbsp;</td>
            <td id="tdRubroAll" colspan="5"  class="titleCenter" ><asp:Label Text="REPORTE DE INCONSISTENCIAS" runat="server" ID="lblTitle"></asp:Label></td>
            <td id="tdRubroDer" class="titleRight">&nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td colspan="5" width="100%" align="right">
                <asp:ImageButton ID="btnExportPDF" 
					ImageUrl ="~/Resources/Images/BNBPF-ICO-PDF.png" runat="server" Width="22px" 
					onclick="btnExportPDF_Click" />
                <asp:ImageButton ID="btnExportExcel" 
					ImageUrl ="~/Resources/Images/BNBPF-ICO-XLS.png" runat="server" Width="22px" 
					onclick="btnExportExcel_Click" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="width:1%"></td>
            <td colspan="5" id="td4">
                <telerik:RadGrid  ID="rgvInconsistencias" runat="server" AutoGenerateColumns="False"
		          AllowSorting ="True" AllowPaging ="True" Width="100%"
                  onneeddatasource="rgvInconsistencias_NeedDataSource" 
                    AllowFilteringByColumn="True" GridLines="Both" OnItemDataBound="rgvInconsistencias_ItemDataBound">
                    <ExportSettings FileName="Inconsistencias" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Inconsistencias" />
                    </ExportSettings>
                    <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                        <MasterTableView AllowPaging="True" AllowSorting="True"
                        CommandItemDisplay="None" >
                            <NoRecordsTemplate>No hay registros de Navegación.</NoRecordsTemplate>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="Crédito" DataField="credito"  HeaderStyle-HorizontalAlign="Justify">
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Auxiliar" DataField="auxiliar" HeaderStyle-HorizontalAlign="Justify" >
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>

	                            <telerik:GridBoundColumn HeaderText="Nombre" DataField="nombre" HeaderStyle-HorizontalAlign="Justify">
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="RFC" DataField="rfc" HeaderStyle-HorizontalAlign="Justify" >
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Fecha de Amortización Vencida SIC" DataField="FEC_AMORT_VENCIDA" HeaderStyle-HorizontalAlign="Justify" >
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Número de Pagos Vencidos SIC" DataField="NUM_PAG_VENCIDOS" HeaderStyle-HorizontalAlign="Justify" >
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Días Vencidos SIC" DataField="DIAS_VENCIMIENTO" HeaderStyle-HorizontalAlign="Justify" >
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Saldo Vigente SICOFIN" DataField="saldo_vigente" HeaderStyle-HorizontalAlign="Justify" >
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Saldo Vencido SICOFIN" DataField="saldo_vencido" HeaderStyle-HorizontalAlign="Justify" >
					                <HeaderStyle Width="20%" />
				                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                </telerik:RadGrid>
            </td>
            <td></td>
        </tr>
    </table>
</asp:Content>
