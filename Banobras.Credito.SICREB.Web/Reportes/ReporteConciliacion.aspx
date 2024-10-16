<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="ReporteConciliacion.aspx.cs" Inherits="Reportes_ReporteConciliacion" %>

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
                        &nbsp;REPORTE DE CONCILIACIÓN
                    </td>
                    <td width="3%">
                    <asp:ImageButton ID="imgExportArchivoPDF" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                            Width="22px" OnClick="imgExportArchivoPDF_Click" />
                        
                    </td>
                    <td width="3%">
                        &nbsp;<asp:ImageButton ID="imgExportArchivoXLS" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                            Width="22px" OnClick="imgExportArchivoXLS_Click" />
                    </td>
                </tr>
            </table>
        </div>
       
        <div runat="server" id="div_Conciliacion_PF" >
        <asp:Panel runat="server" ID="pnl_Conciliacion_PF" ScrollBars="Horizontal" >
            <table style="width:1599px;">
                <tr>
                    <td style="background-color:#F1F5FB;" align="center" width="492 px">
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="369 px">
                        SICREB
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="369 px">
                        SICOFIN
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="369 px">
                        Diferencia
                    </td>
                </tr>
            </table>
            <telerik:RadGrid 
                ID="rgConciliacion_PF" 
                 Width = "1599 px"
                runat="server" 
                AllowPaging="True" 
                AllowSorting="True"
                AllowFilteringByColumn="True" 
                GridLines="Both" AutoGenerateColumns="False" OnNeedDataSource="rgConciliacion_PF_NeedDataSource">
                 
                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
                             Width = "1599 px"
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                <MasterTableView AllowPaging="True" AllowSorting="True" CommandItemDisplay="None" AllowFilteringByColumn="True" >
                    <NoRecordsTemplate>
                        No hay registros de Navegación.</NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Delegación" DataField="ESTADO" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridBoundColumn HeaderText="Tipo Cartera" DataField="TIPO_CARTERA" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px" />
                        <telerik:GridBoundColumn HeaderText="RFC" DataField="RFC" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px" />
                        <telerik:GridBoundColumn HeaderText="Acreditado" DataField="ACREDITADO" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="No Crédito" DataField="NO_CREDITO" 
                            DataType="System.Int32" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>                         
                        <telerik:GridNumericColumn HeaderText="Saldo Actual (Etiqueta 22)" DataField="SALDO_VIGENTE_SICREB"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Vencido (Etiqueta 24)" DataField="SALDO_VENCIDO_SICREB"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridBoundColumn HeaderText="Auxiliar" DataField="AUXILIAR" 
                            DataType="System.Int32"  HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Actual" DataField="SALDO_VIGENTE_SICOFIN"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Vencido" DataField="SALDO_VENCIDO_SICOFIN"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Actual" DataField="SALDO_VIGENTE_DIFERENCIA"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Vencido" DataField="SALDO_VENCIDO_DIFERENCIA"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridBoundColumn HeaderText="Observaciones" DataField="OBSERVACIONES" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <table style="width:1599px;">
                  <tr>
                    <td style="background-color:#F1F5FB;" align="center" width="492 px">
                    
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                         <asp:Label ID="Label8" runat="server" Text= "No. Crédito"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label9" runat="server" Text= "Saldo Actual   (Etiqueta 22)"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label10" runat="server" Text= "Saldo Vencido   (Etiqueta 24)"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label11" runat="server" Text= "Saldo Actual SICOFIN"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label12" runat="server" Text= "Saldo Vencido SICOFIN" ></asp:Label>
                    </td> 
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label13" runat="server" Text= "DIFERENCIA Saldo Actual"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label14" runat="server" Text= "DIFERENCIA Saldo Vencido"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">                        
                    </td>
                </tr>
                <tr>
                    <td style="background-color:#F1F5FB;" align="center" width="492 px">
                    Total
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                         <asp:Label ID="lblPFNoCredito" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblPFSalVigSicreb" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblPFSalVincSicreb" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblPFSalVigSicofin" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblPFSalVincSicofin" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblPFSalVigDiferencia" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblPFSalVincDiferencia" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">                        
                    </td>
                </tr>
            </table>
         </asp:Panel>
        </div>

        <div  runat="server" id="div_Conciliacion_PM"  >
        <asp:Panel runat="server" ID="pnl_Conciliacion_PM" ScrollBars="Horizontal" >
        <table style="width:1476px;">
                <tr>
                    <td style="background-color:#F1F5FB;" align="center" width="369 px">
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="369 px">
                        SICREB
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="369 px">
                        SICOFIN
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="369 px">
                        Diferencia
                    </td>
                </tr>
            </table>
        <telerik:RadGrid 
                ID="rgConciliacion_PM" 
                runat="server" 
                 Width = "1476px"
                AllowPaging="True" 
                AllowSorting="True"
                GridLines="Both" 
                AutoGenerateColumns="False"
                AllowFilteringByColumn="True"  
                OnNeedDataSource="rgConciliacion_PM_NeedDataSource">
                <ExportSettings FileName="Conciliacion" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Reporte Conciliación" />
                    </ExportSettings>
                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
                            Width = "1476 px"
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
                <MasterTableView AllowPaging="True" AllowSorting="True" CommandItemDisplay="None" AllowFilteringByColumn="True" >
                    <NoRecordsTemplate>
                        No hay registros de Navegación.</NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Delegación" DataField="ESTADO"  HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>                        
                        <telerik:GridBoundColumn HeaderText="RFC" DataField="RFC" HeaderStyle-Width =  "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridBoundColumn HeaderText="Acreditado" DataField="ACREDITADO" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="No Crédito" DataField="NO_CREDITO" 
                            DataType="System.Int32"  HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Vigente (DE-03)" DataField="SALDO_VIGENTE_SICREB"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px" />
                        <telerik:GridNumericColumn HeaderText="Saldo Vencido (DE-03)" DataField="SALDO_VENCIDO_SICREB"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px" />
                        <telerik:GridBoundColumn HeaderText="Auxiliar" DataField="AUXILIAR" 
                            DataType="System.Int32" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Vigente" DataField="SALDO_VIGENTE_SICOFIN" 
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Vencido" DataField="SALDO_VENCIDO_SICOFIN"
                            DataType="System.Double"  HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridNumericColumn HeaderText="Saldo Vigente" DataField="SALDO_VIGENTE_DIFERENCIA"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px" />
                        <telerik:GridNumericColumn HeaderText="Saldo Vencido" DataField="SALDO_VENCIDO_DIFERENCIA"
                            DataType="System.Double" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                        <telerik:GridBoundColumn HeaderText="Observaciones" DataField="OBSERVACIONES" HeaderStyle-Width = "123 px" ItemStyle-Width = "123 px" FilterControlWidth = "65 px"/>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <table style="width:1476px;" border="1">
              <tr>
                 <td style="background-color:#F1F5FB;" align="center" width="369 px">                   
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label1" runat="server" Text = "No. Crédito"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label2" runat="server" Text = "Saldo Vigente   (DE-03)"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label3" runat="server" Text = "Saldo Vencido      (DE-03)"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label4" runat="server" Text = "Saldo Vigente SICOFIN"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label5" runat="server" Text = "Saldo Vencido SICOFIN"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label6" runat="server" Text = "DIFERENCIA Saldo Vigente" ></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="Label7" runat="server" Text = "DIFERENCIA Saldo Vencido" ></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center"  width="123 px">
                    </td>
                </tr>
                <tr>
                    <td style="background-color:#F1F5FB;" align="center" width="369 px">
                    Total
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblNoCredito" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblSalVigSicreb" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblSalVincSicreb" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblSalVigSicofin" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblSalVincSicofin" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblSalVigDiferencia" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center" width="123 px">
                        <asp:Label ID="lblSalVincDiferencia" runat="server"></asp:Label>
                    </td>
                    <td style="background-color:#F1F5FB;" align="center"  width="123 px">
                    </td>
                </tr>
            </table>
        </asp:Panel>
        </div>

     </div>
</asp:Content>

