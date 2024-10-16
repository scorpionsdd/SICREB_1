<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ReporteConciliacionV2.aspx.cs" Inherits="ReporteConciliacionV2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">   
    
    <table width="300px" class="tableinicio">
        <tr>
            <td>
                <table width="290px" class="tableinicio">
                    <tr>
                        <td>
                            <div class="titleCenter">&nbsp;FILTROS</div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="290px" class="tableinicio">
                    <tr>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rbPersona" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbPersona_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="PM">Persona Moral</asp:ListItem>
                                <asp:ListItem Value="PF">Persona Física</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <div class="divCard">
        
        <div class="titleCenter">
            <table width="100%">
                <tr>
                    <td width="94%">
                        &nbsp;REPORTE DE CONCILIACIÓN
                    </td>
                    <td width="30px">
                        <asp:ImageButton runat="server" ID="imgExportArchivoPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" Width="22px" OnClick="imgExportArchivoPDF_Click" />                        
                    </td>
                    <td width="30px">
                        <asp:ImageButton runat="server" ID="imgExportArchivoXLS" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" Width="22px" OnClick="imgExportArchivoXLS_Click" />
                    </td>
                </tr>
            </table>
        </div>
       
        <div runat="server" id="div_Conciliacion_PM" >
            <asp:Panel runat="server" ID="pnl_Conciliacion_PM" ScrollBars="Horizontal" >
                <table style="width:3020px;">
                    <tr>
                        <td style="background-color:#F1F5FB;" align="center" width="520px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="650px">SICREB</td>
                        <td style="background-color:#F1F5FB;" align="center" width="650px">SICOFIN</td>
                        <td style="background-color:#F1F5FB;" align="center" width="1100px">CONCILIACION</td>
                    </tr>
                </table>
                
                <telerik:RadGrid runat="server" ID="rgConciliacionPM" Width="3020px" AllowPaging="True" AllowSorting="True" GridLines="Both" 
                    AutoGenerateColumns="False" AllowFilteringByColumn="True" OnNeedDataSource="rgConciliacion_PM_NeedDataSource">
                    <ExportSettings FileName="ConciliacionPM" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Reporte de Conciliación Personas Morales" />
                    </ExportSettings>
                    <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" Width="3020px"
						FirstPageToolTip="Primera Página"  
						PrevPageToolTip="Página Anterior"  
						NextPageToolTip="Página Siguiente"  
						LastPageToolTip="Última Página" 
						PageSizeLabelText="Registros por página: " 
						PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <MasterTableView AllowPaging="True" AllowSorting="True" CommandItemDisplay="None" AllowFilteringByColumn="True" >
                        <NoRecordsTemplate>
                            No hay registros de Navegación.
                        </NoRecordsTemplate>
                        <Columns>                            
                            <telerik:GridBoundColumn   HeaderText="RFC"                      DataField="RFC_ACREDITADO"           DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn   HeaderText="Nombre del Acreditado"    DataField="NOMBRE_ACREDITADO"        DataType="System.String" HeaderStyle-Width="250px" ItemStyle-Width="250px" FilterControlWidth="225px" />
                            <telerik:GridNumericColumn HeaderText="Número de Crédito"        DataField="NUMERO_CREDITO"           DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" ItemStyle-HorizontalAlign="Right"/>
                            <telerik:GridNumericColumn HeaderText="Saldo Vigente (DE-03)"    DataField="SALDO_VIGENTE_SICREB"     DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Vencido (DE-03)"    DataField="SALDO_VENCIDO_SICREB"     DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Bono Cupon Cero"    DataField="SALDO_BONO_CUPON_CERO"    DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />                            
                            <telerik:GridNumericColumn HeaderText="Saldo Total SICREB"       DataField="SALDO_TOTAL_SICREB"       DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridBoundColumn   HeaderText="Auxiliar"                 DataField="AUXILIAR"                 DataType="System.Int32"  HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridNumericColumn HeaderText="Saldo Vigente"            DataField="SALDO_VIGENTE_SICOFIN"    DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Vencido"            DataField="SALDO_VENCIDO_SICOFIN"    DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Diverso"            DataField="SALDO_DIVERSO_SICOFIN"    DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Total SICOFIN"      DataField="SALDO_TOTAL_SICOFIN"      DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Total Diferencia"   DataField="SALDO_TOTAL_DIFERENCIA"   DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Redondeo"           DataField="SALDO_REDONDEO"           DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo CONCILIACION"       DataField="SALDO_TOTAL_CONCILIACION" DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridBoundColumn   HeaderText="Observaciones"            DataField="OBSERVACIONES"            DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn   HeaderText="Detalle de Observaciones" DataField="DETALLE_OBSERVACIONES"    DataType="System.String" HeaderStyle-Width="450px" ItemStyle-Width="450px" FilterControlWidth="425px" />
                            <telerik:GridBoundColumn   HeaderText="Delegación"               DataField="ESTADO"                   DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            
                <table style="width:3020px;" border="1">
                    <tr>
                        <td style="background-color:#F1F5FB;" align="center" width="340px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">No. de Créditos</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Vigente (DE-03)</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Vencido (DE-03)</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Bono Cupon Cero</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Total SICREB</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Vigente Sicofin</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Vencido Sicofin</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Diverso Sicofin</td>                    
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Total SICOFIN</td>                    
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Total DIFERENCIA;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Redondeo</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo CONCILIACION</td>
                        <td style="background-color:#F1F5FB;" align="center" width="600px">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="background-color:#F1F5FB;" align="center" width="340px">Sumas Totales</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMNumeroCreditos" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoVigenteSicreb" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoVencidoSicreb" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMBonoCuponCero" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoTotalSICREB" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoVigenteSicofin" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoVencidoSicofin" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoDiversoSicofin" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoTotalSICOFIN" />&nbsp;</td>                    
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoTotalDiferencia" />&nbsp;</td>                        
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoRedondeo" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPMSaldoConciliacion" />&nbsp;</td>                    
                        <td style="background-color:#F1F5FB;" align="center" width="600px">&nbsp;</td>
                    </tr>
                </table>
            
            </asp:Panel>
        </div>
     
        <div runat="server" id="div_Conciliacion_PF" >
            <asp:Panel runat="server" ID="pnl_Conciliacion_PF" ScrollBars="Horizontal" >
                <table style="width:3020px;">
                    <tr>
                        <td style="background-color:#F1F5FB;" align="center" width="520px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="650px">SICREB</td>
                        <td style="background-color:#F1F5FB;" align="center" width="650px">SICOFIN</td>
                        <td style="background-color:#F1F5FB;" align="center" width="1100px">CONCILIACION</td>
                    </tr>
                </table>
                
                <telerik:RadGrid runat="server" ID="rgConciliacionPF" Width="3020px" AllowPaging="True" AllowSorting="True" GridLines="Both"
                    AutoGenerateColumns="False" AllowFilteringByColumn="True" OnNeedDataSource="rgConciliacion_PF_NeedDataSource">
                    <ExportSettings FileName="ConciliacionPF" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Reporte de Conciliación Personas Fisicas" />
                    </ExportSettings>                 
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" Width="3020px"	
                        FirstPageToolTip="Primera Página"  
						PrevPageToolTip="Página Anterior"  
						NextPageToolTip="Página Siguiente"  
						LastPageToolTip="Última Página" 
						PageSizeLabelText="Registros por página: " 
						PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                    <MasterTableView AllowPaging="True" AllowSorting="True" CommandItemDisplay="None" AllowFilteringByColumn="True" >
                        <NoRecordsTemplate>
                            No hay registros de Navegación.
                        </NoRecordsTemplate>
                        <Columns>                            
                            <telerik:GridBoundColumn   HeaderText="RFC"                      DataField="RFC_ACREDITADO"           DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn   HeaderText="Nombre del Acreditado"    DataField="NOMBRE_ACREDITADO"        DataType="System.String" HeaderStyle-Width="250px" ItemStyle-Width="250px" FilterControlWidth="225px" />
                            <telerik:GridNumericColumn HeaderText="Número de Crédito"        DataField="NUMERO_CREDITO"           DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" ItemStyle-HorizontalAlign="Right"/>
                            <telerik:GridNumericColumn HeaderText="Tipo Crédito"             DataField="TIPO_CREDITO"             DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Vigente (TL-22)"    DataField="SALDO_VIGENTE_SICREB"     DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Vencido (TL-24)"    DataField="SALDO_VENCIDO_SICREB"     DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />                            
                            <telerik:GridNumericColumn HeaderText="Saldo Total SICREB"       DataField="SALDO_TOTAL_SICREB"       DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridBoundColumn   HeaderText="Auxiliar"                 DataField="AUXILIAR"                 DataType="System.Int32"  HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridNumericColumn HeaderText="Saldo Vigente"            DataField="SALDO_VIGENTE_SICOFIN"    DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Vencido"            DataField="SALDO_VENCIDO_SICOFIN"    DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Diverso"            DataField="SALDO_DIVERSO_SICOFIN"    DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Total SICOFIN"      DataField="SALDO_TOTAL_SICOFIN"      DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Total Diferencia"   DataField="SALDO_TOTAL_DIFERENCIA"   DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo Redondeo"           DataField="SALDO_REDONDEO"           DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridNumericColumn HeaderText="Saldo CONCILIACION"       DataField="SALDO_TOTAL_CONCILIACION" DataType="System.Double" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <telerik:GridBoundColumn   HeaderText="Observaciones"            DataField="OBSERVACIONES"            DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                            <telerik:GridBoundColumn   HeaderText="Detalle de Observaciones" DataField="DETALLE_OBSERVACIONES"    DataType="System.String" HeaderStyle-Width="450px" ItemStyle-Width="450px" FilterControlWidth="425px" />
                            <telerik:GridBoundColumn   HeaderText="Delegación"               DataField="ESTADO"                   DataType="System.String" HeaderStyle-Width="100px" ItemStyle-Width="100px" FilterControlWidth="90px" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            
                <table style="width:3020px;" border="1">
                    <tr>
                        <td style="background-color:#F1F5FB;" align="center" width="340px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">No. de Créditos</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Vigente (TL-22)</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Vencido (TL-24)</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Total SICREB</td>                        
                        <td style="background-color:#F1F5FB;" align="center" width="100px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Vigente Sicofin</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Vencido Sicofin</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Diverso Sicofin</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Total SICOFIN</td>                    
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Total DIFERENCIA</td>                    
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo Redondeado</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">Saldo CONCILIACION</td>
                        <td style="background-color:#F1F5FB;" align="center" width="600px">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="background-color:#F1F5FB;" align="center" width="340px">Sumas Totales</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFNumeroCreditos" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="100px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoVigenteSicreb" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoVencidoSicreb" />&nbsp;</td>                        
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoTotalSICREB" />&nbsp;</td>                        
                        <td style="background-color:#F1F5FB;" align="right"  width="100px">&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoVigenteSicofin" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoVencidoSicofin" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoDiversoSicofin" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoTotalSICOFIN" />&nbsp;</td>                    
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoTotalDiferencia" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoRedondeo" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="right"  width="100px"><asp:Label runat="server" ID="lblPFSaldoConciliacion" />&nbsp;</td>
                        <td style="background-color:#F1F5FB;" align="center" width="600px">&nbsp;</td>
                    </tr>
                </table>

            </asp:Panel>
        </div>

     </div>

</asp:Content>
