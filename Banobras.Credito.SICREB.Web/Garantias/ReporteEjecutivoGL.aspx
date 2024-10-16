<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ReporteEjecutivoGL.aspx.cs" Inherits="ReporteEjecutivoGL" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">   
        
    <table cellpadding="0" cellspacing="0" border="0" width="800px" align="left">
        <tr>
            <td>
            </td>
            <td width="100%" align="right">
                <asp:ImageButton runat="server" ID="btnExportarPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" Width="22px" OnClick="btnExportarPDF_Click" />
                <asp:ImageButton runat="server" ID="btnExportarXLS" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" Width="22px" OnClick="btnExportarXLS_Click" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">&nbsp;</td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp; <asp:Label runat="server" ID="lblTitle" Text="Reporte de Resumen Ejecutivo (Lineas y Garantias)" />
            </td>
            <td id="tdRubroDer" class="titleRight">&nbsp;</td>
        </tr>
        <tr>
            <td id="tdCentralGeneral" colspan="3">

                <div  runat="server" id="div_Conciliacion_GL">          
                    <telerik:RadGrid runat="server" ID="RgdConciliacionGL" Width = "800px" AllowPaging="True" AllowSorting="True" GridLines="Both" 
                        AutoGenerateColumns="False" AllowFilteringByColumn="True" OnNeedDataSource="RgdConciliacionGL_NeedDataSource">
                        <ExportSettings FileName="ReporteConciliacionGL" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                            <Pdf PaperSize="Letter" PageTitle="Reporte Conciliación" />
                        </ExportSettings>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" Width = "800px"	FirstPageToolTip="Primera Página"  
				            PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente"  
				            LastPageToolTip="Última Página" PageSizeLabelText="Registros por página: " 
				            PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                        <MasterTableView AllowPaging="True" AllowSorting="True" CommandItemDisplay="None" AllowFilteringByColumn="True">
                            <NoRecordsTemplate>
                                No hay registros de Navegación.
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridBoundColumn   HeaderText="Cantidad"       DataField="CANTIDAD"         HeaderStyle-Width = "50px"  ItemStyle-Width = "50px"  FilterControlWidth = "40px" SortExpression="CANTIDAD" />                                                                     
                                <telerik:GridNumericColumn HeaderText="Saldo Sicreb"   DataField="SALDO_SICREB"     HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.Double" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />                    
                                <telerik:GridNumericColumn HeaderText="Saldo Sicofin"  DataField="SALDO_SICOFIN"    HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.Double" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                <telerik:GridNumericColumn HeaderText="Diferencia"     DataField="SALDO_DIFERENCIA" HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.Double" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                <telerik:GridBoundColumn   HeaderText="Observaciones"  DataField="OBSERVACIONES"    HeaderStyle-Width = "150px" ItemStyle-Width = "150px" FilterControlWidth = "120px"/>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                
            </td>
        </tr>
    </table>
            

</asp:Content>
