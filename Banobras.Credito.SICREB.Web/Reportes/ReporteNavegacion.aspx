<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ReporteNavegacion.aspx.cs" Inherits="ReporteNavegacion" Theme="Banobras2011" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<table width="100%" cellpadding ="0" cellspacing ="0" >
        <tr>
                    <td id="tdRubroIzq" class="titleLeft">&nbsp;</td>
                    <td id="tdRubroAll" colspan="5"  class="titleCenter" ><asp:Label Text="REPORTE DE NAVEGACIÓN" runat="server" ID="lblTitle"></asp:Label></td>
                    <td id="tdRubroDer" class="titleRight">&nbsp;</td>
        </tr>
        <tr>
           <td style="width:1%"></td>
            <td>
                <asp:Label ID="Label3" runat="server" 
                    Text="Fecha Inicial : "></asp:Label>
            </td>
            <td>
                <telerik:RadDatePicker ID="rdpFechaInicial" runat="server" Culture="Spanish (Mexico)"></telerik:RadDatePicker></td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Fecha Final: "></asp:Label>
            </td>
            <td>
                <telerik:RadDatePicker ID="rdpFechaFinal" runat="server" Culture="Spanish (Mexico)"></telerik:RadDatePicker>
            </td>
            <td>
                <asp:Button ID="btnBuscar" runat="server" CssClass="btnMedium"
                    onclick="btnBuscar_Click" Text="Buscar" />
            </td>
            <td style="width:1%"></td>
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
            <td colspan="5" id="td1"> 	   	
               <telerik:RadGrid  ID="RgdNavegacion" runat="server" AutoGenerateColumns="False"
		          AllowSorting ="true" AllowPaging ="true" PageSize="10" Width="100%"
                  onneeddatasource="grdCatalogo_NeedDataSource" AllowFilteringByColumn="True">
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

			            <telerik:GridBoundColumn HeaderText="Usuario" DataField="login"  HeaderStyle-HorizontalAlign="Justify">
					        <HeaderStyle Width="20%" />
				        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderText="Actividad" DataField="descripcion" HeaderStyle-HorizontalAlign="Justify" >
					        <HeaderStyle Width="20%" />
				        </telerik:GridBoundColumn>

	                    <telerik:GridBoundColumn HeaderText="Datos" DataField="detalle" HeaderStyle-HorizontalAlign="Justify">
					        <HeaderStyle Width="20%" />
				        </telerik:GridBoundColumn>

                          <telerik:GridBoundColumn HeaderText="Fecha" DataField="fecha" HeaderStyle-HorizontalAlign="Justify" >
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

