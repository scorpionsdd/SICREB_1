<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="HistorialGL.aspx.cs" Inherits="HistorialGL" %>
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
                &nbsp; <asp:Label runat="server" ID="lblTitle" Text="Historial de Lineas y Garantias Liquidadas o Cerradas" />
            </td>
            <td id="tdRubroDer" class="titleRight">&nbsp;</td>
        </tr>
        <tr>
            <td id="tdCentralGeneral" colspan="3">


                <div  runat="server" id="div_Historial_GL">          
                    <telerik:RadGrid runat="server" ID="RgdHistorialGL" Width = "800px" AllowPaging="True" AllowSorting="True" GridLines="Both" 
                        AutoGenerateColumns="False" AllowFilteringByColumn="True" OnNeedDataSource="RgdHistorialGL_NeedDataSource">
                        <ExportSettings FileName="ReporteHistorialGL" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
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
                                <telerik:GridBoundColumn    HeaderText="RFC Acreditado"       DataField="RFC_ACREDITADO"    HeaderStyle-Width = "50px"  ItemStyle-Width = "100px" FilterControlWidth = "90px" />                                                                     
                                <telerik:GridNumericColumn  HeaderText="Numero de Crédito"    DataField="NUMERO_CREDITO"    HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" ItemStyle-HorizontalAlign="Right" />
                                <telerik:GridDateTimeColumn HeaderText="Fecha de Liquidación" DataField="FECHA_LIQUIDACION" HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy}" />
                                <telerik:GridDateTimeColumn HeaderText="Fecha de Reporte"     DataField="FECHA_REPORTE"     HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy}" />
                                <telerik:GridDateTimeColumn HeaderText="Fecha de Registro"    DataField="FECHA_REGISTRO"    HeaderStyle-Width = "150px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy}" />
                                <telerik:GridBoundColumn    HeaderText="Tipo"                 DataField="TIPO"              HeaderStyle-Width = "50px"  ItemStyle-Width = "50px"  FilterControlWidth = "40px"/>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>


            </td>
        </tr>
    </table>
            
    
</asp:Content>
