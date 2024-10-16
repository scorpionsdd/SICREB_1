<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ResumenEjecutivo.aspx.cs" Inherits="Reportes_ResumenEjecutivo" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 322px;
        }
        .style2
        {
            width: 326px;
        }
        .style3
        {
            width: 325px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">   
    <table width="300px" class="tableinicio">
        <tr>
            <td>
                <table width="290px" class="tableinicio">
                    <tr>
                        <td>
                            <div class="titleCenter">
                                &nbsp;FILTROS
                            </div>
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
                            <asp:RadioButtonList 
                                ID="rbPersona" 
                                runat="server" 
                                AutoPostBack="True" 
                                RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="rbPersona_SelectedIndexChanged">
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
                        &nbsp;REPORTE DE RESUMEN EJECUTIVO
                    </td>
                    <td width="3%" align="right">
                    <asp:ImageButton ID="imgExportArchivoPDF" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                            OnClick="imgExportArchivoPDF_Click" />
                    </td>
                    <td  width="3%">
                        <asp:ImageButton ID="imgExportArchivoXLS" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                            OnClick="imgExportArchivoXLS_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div runat="server" id="div_Conciliacion_PF">  
        <asp:Panel runat="server" ID="pnl_Conciliacion_PF" ScrollBars="Horizontal" >
            <table style="width:100%;">
                <tr>
                    <td style="background-color:#F1F5FB;" align="center" class="style1">
                        SICREB
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" class="style2">
                        SICOFIN
                    </td>
                    <td style="background-color:#F1F5FB;" align="center"  >
                        Diferencia
                    </td>
                </tr>
            </table>      
            <telerik:RadGrid 
                ID="rgConciliacion_PF" 
                runat="server" 
                AllowPaging="True" 
                AllowSorting="True"
                GridLines="Both" 
                AutoGenerateColumns="False"
                AllowFilteringByColumn="True" 
                OnNeedDataSource="rgConciliacion_PF_NeedDataSource"
                OnItemDataBound="rgConciliacion_PF_ItemDataBound">
                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                <MasterTableView AllowPaging="True" AllowSorting="True" CommandItemDisplay="None" AllowFilteringByColumn="True">
                    <NoRecordsTemplate>
                        No hay registros de Navegación.</NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Tipo de cartera" DataField="TIPO_CARTERA" />                        
                        <telerik:GridBoundColumn HeaderText="Cantidad" DataField="CANTIDAD" />
                        <telerik:GridBoundColumn HeaderText="Saldo Actual (Etiqueta 22)" DataField="SALDO_VIGENTE_SICREB"
                            DataType="System.Double" />
                        <telerik:GridBoundColumn HeaderText="Saldo Vencido (Etiqueta 24)" DataField="SALDO_VENCIDO_SICREB"
                            DataType="System.Double" />
                         <telerik:GridBoundColumn HeaderText="Saldo Actual" DataField="SALDO_VIGENTE_SICOFIN"
                            DataType="System.Double" />
                        <telerik:GridBoundColumn HeaderText="Saldo Vencido" DataField="SALDO_VENCIDO_SICOFIN"
                            DataType="System.Double" />
                        <telerik:GridBoundColumn HeaderText="Saldo Actual" DataField="SALDO_VIGENTE_DIFERENCIA"
                            DataType="System.Double" />
                        <telerik:GridBoundColumn HeaderText="Saldo Vencido" DataField="SALDO_VENCIDO_DIFERENCIA"
                            DataType="System.Double" />
                        <telerik:GridBoundColumn HeaderText="Observaciones" DataField="OBSERVACIONES" />                        
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            </asp:Panel>
        </div>
         
        <div  runat="server" id="div_Conciliacion_PM">          
        <table style="width:100%;">
                <tr>
                    <td style="background-color:#F1F5FB;" align="center" class="style2">
                        SICREB
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" class="style3">
                        SICOFIN
                    </td>
                    <td style="background-color:#F1F5FB;" align="center">
                        Diferencia
                    </td>
                </tr>
            </table>
        <telerik:RadGrid 
                ID="rgConciliacion_PM" 
                runat="server" 
                AllowPaging="True" 
                AllowSorting="True"
                GridLines="Both" 
                AutoGenerateColumns="False" 
                AllowFilteringByColumn="True" 
                OnNeedDataSource="rgConciliacion_PM_NeedDataSource"
                OnItemDataBound="rgConciliacion_PM_ItemDataBound">
                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                <MasterTableView AllowPaging="True" AllowSorting="True" CommandItemDisplay="None" AllowFilteringByColumn="True">
                    <NoRecordsTemplate>
                        No hay registros de Navegación.</NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn 
                            HeaderText="Cantidad" 
                            DataField="CANTIDAD" 
                            UniqueName="CANTIDAD"
                            SortExpression="CANTIDAD" />                                               
                        <telerik:GridBoundColumn 
                            HeaderText="Saldo Vigente (DE-03)" 
                            DataField="SALDO_VIGENTE_SICREB"
                            DataType="System.Double"
                            UniqueName="SALDO_VIGENTE_SICREB"
                            SortExpression="SALDO_VIGENTE_SICREB" />
                        <telerik:GridBoundColumn 
                            HeaderText="Saldo Vencido (DE-03)" 
                            DataField="SALDO_VENCIDO_SICREB"
                            DataType="System.Double"
                            UniqueName="SALDO_VENCIDO_SICREB"
                            SortExpression="SALDO_VENCIDO_SICREB" />                        
                        <telerik:GridBoundColumn 
                            HeaderText="Saldo Vigente" 
                            DataField="SALDO_VIGENTE_SICOFIN"
                            DataType="System.Double"
                            UniqueName="SALDO_VIGENTE_SICOFIN"
                            SortExpression="SALDO_VIGENTE_SICOFIN" />
                        <telerik:GridBoundColumn 
                            HeaderText="Saldo Vencido" 
                            DataField="SALDO_VENCIDO_SICOFIN"
                            DataType="System.Double"
                            UniqueName="SALDO_VENCIDO_SICOFIN"
                            SortExpression="SALDO_VENCIDO_SICOFIN" />
                        <telerik:GridBoundColumn 
                             HeaderText="Saldo Vigente" 
                             DataField="SALDO_VIGENTE_DIFERENCIA"
                             DataType="System.Double"
                             UniqueName="SALDO_VIGENTE_DIFERENCIA"
                             SortExpression="SALDO_VIGENTE_DIFERENCIA" />
                        <telerik:GridBoundColumn 
                             HeaderText="Saldo Vencido" 
                             DataField="SALDO_VENCIDO_DIFERENCIA"
                             DataType="System.Double"
                             UniqueName="SALDO_VENCIDO_DIFERENCIA"
                             SortExpression="SALDO_VENCIDO_DIFERENCIA" />
                        <telerik:GridBoundColumn 
                             HeaderText="Observaciones" 
                             DataField="OBSERVACIONES" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>

    </div>
</asp:Content>


