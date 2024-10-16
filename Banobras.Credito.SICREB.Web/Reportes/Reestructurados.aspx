<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="Reestructurados.aspx.cs" Inherits="Reportes_Conciliacion" Theme="Banobras2011" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
 
    
    <div class="divCard" style="overflow:scroll">
        <table width="100%" cellpadding ="0" cellspacing ="0" >
        <tr>
                    <td id="td1" class="titleLeft">&nbsp;</td>
                    <td id="td2" colspan="5"  class="titleCenter" >
                   <div style="float:left"> <asp:Label Text="REPORTE DE CRÉDITOS REESTRUCTURADOS DE PERSONAS MORALES" runat="server" ID="Label1" ></asp:Label></div>
                  <div style="float:right">
                  <asp:ImageButton ID="imgExportArchivoPDF" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                OnClick="imgExportArchivoPDF_Click" />
            &nbsp;<asp:ImageButton ID="imgExportArchivoXLS" runat="server" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                OnClick="imgExportArchivoXLS_Click" />&nbsp;&nbsp;
        </div>
                    </td>                   <td id="td3" class="titleRight"></td>
        </tr>
        </table>
        
       
        <telerik:RadGrid ID="rgConciliacion" runat="server" AllowPaging="True" AllowSorting="True"
            GridLines="Both" AutoGenerateColumns="False" AllowFilteringByColumn="true" OnNeedDataSource="rgConciliacion_NeedDataSource"
            OnItemCommand="rgConciliacion_ItemCommand"  
            EnableEmbeddedSkins="false" Skin="Banobras2011" >
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings FileName="Reestructurados" IgnorePaging="True" ExportOnlyData="true"
                Csv-ColumnDelimiter="VerticalBar">
                <Pdf Title="Reporte de Reestructurados" />
            </ExportSettings>
            <MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataKeyNames="Credito"
                PageSize="10" HierarchyLoadMode="ServerOnDemand" AllowFilteringByColumn="True" CommandItemDisplay="Top">
              
                <ExpandCollapseColumn Visible="True">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn HeaderText="Credito Anterior" DataField="credito_anterior" Visible="false" />
                    <telerik:GridBoundColumn HeaderText="No. Crédito Nuevo" DataField="credito" />
                    <telerik:GridBoundColumn HeaderText="Auxiliar" DataField="auxiliar" />
                    <telerik:GridBoundColumn HeaderText="Nombre"  DataField="nombre" />
                    <telerik:GridBoundColumn HeaderText="RFC" DataField="RFC" />
                    <telerik:GridBoundColumn HeaderText="Fecha de Apertura" DataField="fecha_reestructura"/>
                         <telerik:GridBoundColumn HeaderText="Capital Vigente SICOFIN" EmptyDataText="0" DataField="capital_vigente"
                        DataType="System.Double"  />                   
                    <telerik:GridBoundColumn HeaderText="Capital Vencido SICOFIN" EmptyDataText="0" DataField="capital_vencido"
                        DataType="System.Double"  />       
                    <telerik:GridBoundColumn HeaderText="Interés Vigente SICOFIN" EmptyDataText="0" DataField="interes_vigente"
                        DataType="System.Double" />            
                    <telerik:GridBoundColumn HeaderText="Interés Vencido SICOFIN" EmptyDataText="0" DataField="interes_vencido"
                        DataType="System.Double" />        
                    <telerik:GridBoundColumn HeaderText="Saldo" EmptyDataText="0" DataField="saldo_actual"
                        DataType="System.Double" />
                </Columns>
                <EditFormSettings>
                    <EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif"
                        CancelImageUrl="Cancel.gif">
                    </EditColumn>
                </EditFormSettings>
            </MasterTableView>
            <FilterMenu EnableEmbeddedSkins="False">
            </FilterMenu>
            <HeaderContextMenu EnableEmbeddedSkins="False">
            </HeaderContextMenu>
        </telerik:RadGrid>
    </div>
</asp:Content>
