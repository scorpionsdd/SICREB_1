<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ConciliacionGL.aspx.cs" Inherits="ConciliacionGL" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">   

    <div class="divCard">

        <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
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
                    &nbsp; <asp:Label runat="server" ID="lblTitle" Text="Reporte de Conciliación (Lineas y Garantias)" />
                </td>
                <td id="tdRubroDer" class="titleRight">&nbsp;</td>
            </tr>
        </table>
       
        <div runat="server" id="div_Conciliacion_PM" >
            <asp:Panel runat="server" ID="pnl_Conciliacion_PM" ScrollBars="Horizontal" >
            
                <telerik:RadGrid runat="server" ID="RgdConciliacionGL" Width = "1370px" AllowPaging="True" AllowSorting="True" GridLines="Both" 
                    AutoGenerateColumns="False" AllowFilteringByColumn="True" OnNeedDataSource="RgdConciliacionGL_NeedDataSource">
                    <ExportSettings FileName="Conciliacion" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Reporte Conciliación" />
                    </ExportSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" Width = "1370px"	FirstPageToolTip="Primera Página"  
					    PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente"  
						LastPageToolTip="Última Página" PageSizeLabelText="Registros por página: " 
						PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <MasterTableView AllowPaging="True" AllowSorting="True" CommandItemDisplay="None" AllowFilteringByColumn="True" >
                        <NoRecordsTemplate>
                            No hay registros de Navegación.
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn   HeaderText="Tipo"                            DataField="TIPO"             HeaderStyle-Width = "30px"  ItemStyle-Width = "30px"  FilterControlWidth = "20px"  ItemStyle-HorizontalAlign="Center" />     
                            <telerik:GridBoundColumn   HeaderText="Delegación"                      DataField="ESTADO"           HeaderStyle-Width = "80px"  ItemStyle-Width = "80px"  FilterControlWidth = "70px" />     
                            <telerik:GridBoundColumn   HeaderText="RFC"                             DataField="RFC"              HeaderStyle-Width = "80px"  ItemStyle-Width = "80px"  FilterControlWidth = "70px" />
                            <telerik:GridBoundColumn   HeaderText="Acreditado"                      DataField="ACREDITADO"       HeaderStyle-Width = "300px" ItemStyle-Width = "300px" FilterControlWidth = "250px" />
                            <telerik:GridNumericColumn HeaderText="No Crédito"                      DataField="NO_CREDITO"       HeaderStyle-Width = "50px"  ItemStyle-Width = "50px"  FilterControlWidth = "40px" DataType="System.Int32" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Inicial Sicreb"            DataField="SALDO_SICREB"     HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.Double" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridBoundColumn   HeaderText="Auxiliar"                        DataField="AUXILIAR"         HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.Int32" />
                            <telerik:GridNumericColumn HeaderText="Saldo Inicial Sicofin"           DataField="SALDO_SICOFIN"    HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.Double" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Diferencia = (Sicreb - Sicofin)" DataField="SALDO_DIFERENCIA" HeaderStyle-Width = "100px" ItemStyle-Width = "100px" FilterControlWidth = "90px" DataType="System.Double" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridBoundColumn   HeaderText="Observaciones"                   DataField="OBSERVACIONES"    HeaderStyle-Width = "150px" ItemStyle-Width = "150px" FilterControlWidth = "120px"/>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            
                <br />
                <table style="width:1370px;" border="1">
                    <tr>
                        <td style="background-color:#F1F5FB;" align="center" width="350px"></td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">No. Créditos</td>
                        <td style="background-color:#F1F5FB;" align="center" width="150px">Saldo Total Sicreb</td>
                        <td style="background-color:#F1F5FB;" align="center" width="150px">Saldo Total Sicofin</td>
                        <td style="background-color:#F1F5FB;" align="center" width="150px">Diferencia</td>
                        <td style="background-color:#F1F5FB;" align="center" width="400px"></td>
                    </tr>
                    <tr>
                        <td style="background-color:#F1F5FB;" align="center" width="350px">Total</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px"><asp:Label runat="server" ID="lblCreditos" /></td>
                        <td style="background-color:#F1F5FB;" align="center" width="150px"><asp:Label runat="server" ID="lblTotalSicreb" /></td>
                        <td style="background-color:#F1F5FB;" align="center" width="150px"><asp:Label runat="server" ID="lblTotalSicofin" /></td>
                        <td style="background-color:#F1F5FB;" align="center" width="150px"><asp:Label runat="server" ID="lblTotalDiferencia" /></td>
                        <td style="background-color:#F1F5FB;" align="center" width="400px"></td>
                    </tr>
                </table>
            </asp:Panel>
        </div>

    </div>

</asp:Content>

