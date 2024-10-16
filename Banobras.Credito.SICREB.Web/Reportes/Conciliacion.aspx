<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Conciliacion.aspx.cs" Inherits="Reportes_Conciliacion" Theme="Banobras2011" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript">


    $(document).ready(function () {
        encabezados();
    });

    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadFilter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadFilter" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
                </telerik:RadAjaxManager>
    

    <div class="divCard">
            <div class="divHeader">
                   &nbsp;FILTROS</div>
                <div style="height:60px" class="resumenProcesoDiv">
                <div style="float:left; width:33%">
                <br />
                    <asp:RadioButtonList ID="rbPersona" runat="server" AutoPostBack="True" 
                        RepeatDirection="Horizontal" 
                        onselectedindexchanged="rbPersona_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="PM">Persona Moral</asp:ListItem>
                            <asp:ListItem Value="PF">Persona Física</asp:ListItem>
                        </asp:RadioButtonList></div>
                <div style="float:left; width:33%">
                <br />Año: 
                <telerik:RadComboBox ID="cbAnios" Runat="server" 
                        onselectedindexchanged="cbAnios_SelectedIndexChanged" AutoPostBack="True" >
                         </telerik:RadComboBox></div>
                <div style="float:left; width:33%">

                <br />Archivo: 
                    <telerik:RadComboBox ID="cbArchivos" Runat="server" AutoPostBack="True" 
                        onselectedindexchanged="cbArchivos_SelectedIndexChanged" >
                         </telerik:RadComboBox></div>
                </div>
            
        </div>
        <div class="divCard" >
            <div class="divHeader">
                    &nbsp;REPORTE DE CONCILIACIÓN</div>
            <div class="divExporters">
      <asp:ImageButton ID="imgExportArchivoPDF" runat="server" 
                    ImageUrl="/ResourcesSICREB/Images/BNBPF-ICO-PDF.png" 
                    onclick="imgExportArchivoPDF_Click" />          
&nbsp;
                    <asp:ImageButton ID="imgExportArchivoXLS" runat="server" 
                    ImageUrl="/ResourcesSICREB/Images/BNBPF-ICO-XLS.png" 
                    onclick="imgExportArchivoXLS_Click" />
            </div>
            <div style="overflow:visible">
             <telerik:RadFilter runat="server" ID="RadFilter" 
                FilterContainerID="rgConciliacion" ShowApplyButton="false" >
                 <Localization FilterFunctionBetween="Entre" FilterFunctionContains="Contiene" 
                     FilterFunctionDoesNotContain="No Contiene" FilterFunctionEndsWith="Termina " 
                     FilterFunctionEqualTo="Igual " FilterFunctionGreaterThan="Mayor" 
                     FilterFunctionGreaterThanOrEqualTo="Mayor o igual" 
                     FilterFunctionIsEmpty="Es vacia" FilterFunctionIsNull="Es nula" 
                     FilterFunctionLessThan="Menor" FilterFunctionLessThanOrEqualTo="Menor o igual" 
                     FilterFunctionNotBetween="No Entre" FilterFunctionNotEqualTo="No igual" 
                     FilterFunctionNotIsEmpty="No es vacia" FilterFunctionNotIsNull="No es nula" 
                     FilterFunctionStartsWith="Empieza con" GroupOperationAnd="Y" 
                     GroupOperationNotAnd="No Y" GroupOperationNotOr="No O" GroupOperationOr="O" />
            </telerik:RadFilter>
            </div>
           
                <telerik:RadGrid ID="rgConciliacion" runat="server" AllowPaging="True" AllowSorting="True"
                    GridLines="None" AutoGenerateColumns="False"
                    OnNeedDataSource="rgConciliacion_NeedDataSource" OnItemDataBound="rgConciliacion_ItemDataBound" OnItemCommand="rgConciliacion_ItemCommand"  EnableEmbeddedSkins="false" Skin="Banobras2011">
                   <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
							FirstPageToolTip="Primera Página"  
							PrevPageToolTip="Página Anterior"  
							NextPageToolTip="Página Siguiente"  
							LastPageToolTip="Última Página" 
							PageSizeLabelText="Registros por página: " 
							PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." /><GroupingSettings CaseSensitive="false" />
		
                    <ExportSettings FileName="Conciliacion" IgnorePaging="True" ExportOnlyData="true" Csv-ColumnDelimiter="VerticalBar">
                        <Pdf PageWidth="1000px" Title="Reporte de Conciliación" />
                    </ExportSettings>
                    <MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        DataKeyNames="SaldosInfo.Credito" PageSize="10" HierarchyLoadMode="ServerOnDemand"  CommandItemDisplay="Top">
                        <CommandItemTemplate>
                    <telerik:RadToolBar runat="server" ID="RadToolBar" OnButtonClick="RadToolBar_ButtonClick">
                        <Items>
                            <telerik:RadToolBarButton Text="Aplicar Filtro" CommandName="FilterRadGrid" 
                                ImagePosition="Right" />
                        </Items>
                    </telerik:RadToolBar>
                </CommandItemTemplate>
                        <ExpandCollapseColumn Visible="True">
                        </ExpandCollapseColumn>
                        <Columns>
            
                            <telerik:GridBoundColumn HeaderText="Delegación" DataField="Delegacion" />
                            <telerik:GridBoundColumn HeaderText="Tipo Cartera" DataField="TipoCartera" />
                            <telerik:GridBoundColumn HeaderText="Acreditado" DataField="Acreditado" />
                            <telerik:GridBoundColumn HeaderText="RFC" DataField="SaldosInfo.Rfc" />
                            <telerik:GridBoundColumn HeaderText="Crédito" DataField="SaldosInfo.Credito" />
                            <telerik:GridBoundColumn HeaderText="Auxiliar" DataField="SaldosInfo.Auxiliar" DataType="System.String" UniqueName="Auxiliar" />
                            <telerik:GridBoundColumn HeaderText="S. Vigente SICOFIN" DataField="SaldosInfo.SaldoVigenteOriginal" DataType="System.Double" UniqueName="SVigenteSICREB"/>
                            <telerik:GridBoundColumn HeaderText="S. Vigente SICREB" DataField="SaldosInfo.SaldoVigente" DataType="System.Double" UniqueName="SVigenteSICOFIN"/>
                            <telerik:GridBoundColumn HeaderText="Diferencia S. Vigente" DataField="SaldoActual" DataType="System.Double" UniqueName="SVigente" />
                            <telerik:GridBoundColumn HeaderText="S. Vencido SICREB" DataField="SaldosInfo.SaldoVencido" DataType="System.Double" UniqueName="SVencidoSICOFIN"/>
                            <telerik:GridBoundColumn HeaderText="S. Vencido SICOFIN" DataField="SaldosInfo.SaldoVencidoOriginal" DataType="System.Double" UniqueName="SVencidoSICREB"/>
                            <telerik:GridBoundColumn HeaderText="Diferencia S. Vencido" DataField="SaldoVencido" DataType="System.Double" UniqueName="SVencido"/>
                            
                            
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

