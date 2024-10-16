<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="CambiosReportes.aspx.cs" Inherits="Reportes_CambiosReportes" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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
   
    <div style="height:200px; width: 931px;" class="resumenProcesoDiv"> 
        <table width="931px" cellpadding="0" cellspacing="0"  border="0"  align="center" style="table-layout: fixed;">
            <tr>
                <td id="tdRubroIzq" class="titleLeft" >&nbsp;</td>
                <td id="tdRubroAll"  class="titleCenter">
                    <asp:Label Text="FILTROS" runat="server" ID="Label2"></asp:Label>
                </td>
                <td id="tdRubroDer" class="titleRight">&nbsp;</td>
            </tr>
        </table>
        <div style="float:left; width:33%">
            <table id="Table3" runat="server" style="width: 100%">    
            <tr>
            <td class="style1">
                <asp:RadioButtonList ID="rbPersona" runat="server" RepeatDirection="Horizontal" 
                onselectedindexchanged="rbPersona_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Selected="True" Value="PM">Persona Moral</asp:ListItem>
                    <asp:ListItem Value="PF">Persona Física</asp:ListItem>
            </asp:RadioButtonList>
            </td>
            </tr>                  
            </table>
        </div>
        <div style="float:left; width:34%; height: 150px;">
            Selección del 1er reporte a comparar
            <table id="Table1" runat="server" style="width: 100%">
                <tr>
                    <td class="style1">
                        Año:
                        <telerik:RadComboBox ID="cbAnioReporte1" runat="server" 
                            onselectedindexchanged="cbAnioReporte1_SelectedIndexChanged" 
                            AutoPostBack="true" Text="Selecciona el Año"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Mes:
                        <telerik:RadComboBox ID="cbMesReporte1" runat="server" 
                            onselectedindexchanged="cbMesReporte1_SelectedIndexChanged" AutoPostBack="true" 
                            Text="Selecciona el Mes"></telerik:RadComboBox>
                    </td>
                            
                </tr>
                <tr>
                    <td class="style1">
                        Archivo:
                        <telerik:RadComboBox ID="cbArchivoReporte1" Runat="server" 
                            onselectedindexchanged="cbArchivoReporte1_SelectedIndexChanged" 
                            AutoPostBack="true" Text="Selecciona el Archivo"></telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="float:left; width:33%; height: 150px;">
            Selección del 2do reporte a comparar
            <table id="Table2" runat="server" style="width: 100%">
                <tr>
                    <td class="style2">
                        Año:
                        <telerik:RadComboBox ID="cbAnioReporte2" Runat="server" 
                            onselectedindexchanged="cbAnioReporte2_SelectedIndexChanged" 
                            AutoPostBack="true" Text="Selecciona el Año"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Mes:
                        <telerik:RadComboBox ID="cbMesReporte2" Runat="server" 
                            onselectedindexchanged="cbMesReporte2_SelectedIndexChanged" AutoPostBack="true" 
                            Text="Selecciona el Mes"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Archivo:
                        <telerik:RadComboBox ID="cbArchivoReporte2" Runat="server" 
                            onselectedindexchanged="cbArchivoReporte2_SelectedIndexChanged" 
                            AutoPostBack="true" Text="Selecciona el Archivo"></telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="height:10px; width: 931px;" class="resumenProcesoDiv">
      <asp:Label ID="lblAux" runat="server" Visible="False">
      </asp:Label>
    </div>  
    <div style="width: 931px;" class="resumenProcesoDiv" >
    <table width="931px" cellpadding="0" cellspacing="0"  border="0"  align="center" style="table-layout: fixed;">
      <tr>              
        <td id="td2"  class="titleCenter" width="931px">
          <asp:Label Text="REPORTE DE CAMBIOS" runat="server" ID="Label1"></asp:Label>
        </td>              
       </tr>
       <tr>              
        <td width="931px">                   
         <telerik:RadGrid runat="server" ID="rgCambiosPM" AutoGenerateColumns="false" AllowSorting="true" AllowScroll="true"
            AllowPaging="true" AllowFilteringByColumn="True"  OnNeedDataSource="rgReportPrueba_NeedDataSource"
            PageSize="15" onitemdatabound="rgCambiosPM_ItemDataBound">                  
            <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
            <GroupingSettings CaseSensitive="false" />           
            <MasterTableView NoMasterRecordsText="No existen Registros"  DataKeyNames="RFC"
                CommandItemDisplay="Top" Width="100%">
                <CommandItemSettings ShowRefreshButton="true" RefreshText="Actualizar Datos" ShowAddNewRecordButton="false" />
                <Columns>                            
                    <telerik:GridBoundColumn HeaderText="Mes" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Mes" DataField="Mes" FilterControlWidth = "70%"
                          SortExpression="Mes"
                        DataType="System.string">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Año" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Año" DataField="Año" FilterControlWidth = "70%"
                          SortExpression="Año"
                        DataType="System.Int32">
                    </telerik:GridBoundColumn>
                    <telerik:GridNumericColumn HeaderText="Crédito" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Crédito" DataField="Crédito" FilterControlWidth = "70%"
                          SortExpression="Crédito"
                        DataType="System.Int32">                    
                    </telerik:GridNumericColumn>                            
                    <telerik:GridBoundColumn HeaderText="RFC" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Estatus" FilterControlWidth = "80%"
                          SortExpression="RFC"
                        DataField="RFC" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Nombre" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Nombre" FilterControlWidth = "80%"
                          SortExpression="Nombre"
                        DataField="Nombre" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Apellido Paterno" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Apellido Paterno" FilterControlWidth = "80%"
                          SortExpression="Apellido Paterno"
                        DataField="ApellidoPaterno" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Apellido Materno" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Apellido Materno" FilterControlWidth = "80%"
                          SortExpression="Apellido Materno"
                        DataField="Apellido Materno" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Línea1" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Línea1" FilterControlWidth = "80%"
                          SortExpression="Línea1"
                        DataField="Línea1" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Colonia/Población" HeaderStyle-Width="180px" ItemStyle-Width="180px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Colonia/Población" FilterControlWidth = "80%"
                          SortExpression="Colonia/Población"
                        DataField="Colonia/Población" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Municipio/Delegación" HeaderStyle-Width="180px" ItemStyle-Width="180px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Municipio/Delegación" FilterControlWidth = "80%"
                          SortExpression="Municipio/Delegación"
                        DataField="Municipio/Delegación" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Ciudad" HeaderStyle-Width="180px" ItemStyle-Width="180px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Ciudad" FilterControlWidth = "80%"
                          SortExpression="Ciudad"
                        DataField="Ciudad" DataType="System.String">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="Código Postal" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="CodigoPostal" FilterControlWidth = "80%"
                          SortExpression="Código Postal"
                        DataField="CodigoPostal" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Estado" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Estado" FilterControlWidth = "70%"
                          SortExpression="Estado"
                        DataField="Estado" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Fecha de Apertura" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Fecha de Apertura" FilterControlWidth = "80%"
                          SortExpression="Fecha de Apertura"
                        DataField="Fecha de Apertura" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Plazo" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Plazo" FilterControlWidth = "80%"
                          SortExpression="Plazo"
                        DataField="Plazo" DataType="System.String">
                    </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn HeaderText="Moneda" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Moneda" FilterControlWidth = "80%"
                          SortExpression="Moneda"
                        DataField="Moneda" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Saldo Inicial" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Saldo Inicial" FilterControlWidth = "70%"
                          SortExpression="Saldo Inicial"
                        DataField="Saldo Inicial" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Tipo de Crédito" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Tipode Credito" FilterControlWidth = "80%"
                          SortExpression="Crédito"
                        DataField="Tipode Credito" DataType="System.String">
                    </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn HeaderText="Número de Pagos" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Número de Pagos" FilterControlWidth = "80%"
                          SortExpression="Número de Pagos"
                        DataField="Número de Pagos" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Frecuencia" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Frecuencia" FilterControlWidth = "70%"
                          SortExpression="Frecuencia"
                        DataField="Frecuencia" DataType="System.String">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="Nacionalidad" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Nacionalidad" FilterControlWidth = "70%"
                          SortExpression="Nacionalidad"
                        DataField="Nacionalidad" DataType="System.String">
                    </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn HeaderText="Clave Banxico" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Clave Banxico" FilterControlWidth = "70%"
                        SortExpression="Clave Banxico"
                        DataField="Clave Banxico" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Tipo de Cliente" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Tipo de Cliente"
                          SortExpression="Tipo de Cliente"
                        DataField="Tipo de Cliente" DataType="System.String">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="Clave de Observación" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Clave de Observación" FilterControlWidth = "80%"
                          SortExpression="Clave de Observación"
                        DataField="Clave de Observación" DataType="System.String">
                    </telerik:GridBoundColumn>                         
                </Columns>
            </MasterTableView>
                <ClientSettings>
                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True">
                </Scrolling>
            </ClientSettings>
        </telerik:RadGrid> 

         <telerik:RadGrid runat="server" ID="rgCambiosPF" AutoGenerateColumns="false" AllowSorting="true" AllowScroll="true"
            AllowPaging="true" AllowFilteringByColumn="True"  OnNeedDataSource="rgReportPrueba_NeedDataSource"
            PageSize="15" onitemdatabound="rgCambiosPF_ItemDataBound">                  
            <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, <span id='tot'> {5}</span> Registros en {1} páginas." />
            <GroupingSettings CaseSensitive="false" />
          
            <MasterTableView NoMasterRecordsText="No existen Registros"  DataKeyNames="RFC"
                CommandItemDisplay="Top" Width="100%">
                <CommandItemSettings ShowRefreshButton="true" RefreshText="Actualizar Datos"  ShowAddNewRecordButton="false" />                
                <Columns>                            
                    <telerik:GridNumericColumn HeaderText="Mes" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Mes" DataField="Mes" FilterControlWidth = "70%"
                        SortExpression="Mes"
                        DataType="System.string">
                    </telerik:GridNumericColumn>
                    <telerik:GridBoundColumn HeaderText="Año" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Año" DataField="Año" FilterControlWidth = "70%"
                         SortExpression="Año"
                        DataType="System.Int32">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Crédito" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Crédito" DataField="Crédito" FilterControlWidth = "70%"
                        SortExpression="Crédito"
                        DataType="System.String">
                    </telerik:GridBoundColumn>                            
                    <telerik:GridBoundColumn HeaderText="RFC" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Estatus" FilterControlWidth = "80%"
                        SortExpression="RFC"
                        DataField="RFC" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Nombre" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Nombre" FilterControlWidth = "80%"
                        SortExpression="Nombre"
                        DataField="Nombre" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Apellido Paterno" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Apellido Paterno" FilterControlWidth = "80%"
                        SortExpression="Apellido Paterno"
                        DataField="Apellido Paterno" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Apellido Materno" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="Apellido Materno" FilterControlWidth = "80%"
                        SortExpression="Apellido Materno"
                        DataField="ApellidoMaterno" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Línea1" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Línea1" FilterControlWidth = "80%"
                        SortExpression="Línea1"
                        DataField="Línea1" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Colonia/Población" HeaderStyle-Width="180px" ItemStyle-Width="180px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Colonia/Población" FilterControlWidth = "80%"
                        SortExpression="Colonia/Población"
                        DataField="Colonia/Población" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Municipio/Delegación" HeaderStyle-Width="180px" ItemStyle-Width="180px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Municipio/Delegación" FilterControlWidth = "80%"
                        SortExpression="Municipio/Delegación"
                        DataField="Municipio/Delegación" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Ciudad" HeaderStyle-Width="180px" ItemStyle-Width="180px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Ciudad" FilterControlWidth = "80%"
                        SortExpression="Ciudad"
                        DataField="Ciudad" DataType="System.String">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="Código Postal" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="CodigoPostal" FilterControlWidth = "80%"
                        SortExpression="Código Postal"
                        DataField="CodigoPostal" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Estado" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Estado" FilterControlWidth = "80%"
                        SortExpression="Estado"
                        DataField="Estado" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Fecha de Apertura" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Fecha de Apertura" FilterControlWidth = "80%"
                        SortExpression="Fecha de Apertura"
                        DataField="Fecha de Apertura" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Tipo de Responsabilidad" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Tipo de Responsabilidad" FilterControlWidth = "80%"
                        SortExpression="Tipo de Responsabilidad"
                        DataField="Tipo de Responsabilidad" DataType="System.String">
                    </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn HeaderText="Moneda" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Moneda" FilterControlWidth = "80%"
                        SortExpression="Moneda"
                        DataField="Moneda" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Tipo de Cuenta" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Tipo de Cuenta" FilterControlWidth = "80%"
                        SortExpression="Tipo de Cuenta"
                        DataField="Tipo de Cuenta" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Tipo de Contrato" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Tipo de Contrato" FilterControlWidth = "80%"
                        SortExpression="Tipo de Contrato"
                        DataField="Tipo de Contrato" DataType="System.String">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="Número de Pagos" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                        HeaderStyle-HorizontalAlign="Justify" UniqueName="NumerodePagos" FilterControlWidth = "80%"
                        SortExpression="Número de Pagos"
                        DataField="NumerodePagos" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Frecuencia de Pago" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Frecuencia de Pago" FilterControlWidth = "80%"
                        SortExpression="Frecuencia de Pago"
                        DataField="Frecuencia de Pago" DataType="System.String">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="Nacionalidad" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Nacionalidad" FilterControlWidth = "70%"
                        SortExpression="Nacionalidad"
                        DataField="Nacionalidad" DataType="System.String">
                    </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn HeaderText="Fecha de Última Disposición" HeaderStyle-Width="170px" ItemStyle-Width="170px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Fecha de Última Disposición" FilterControlWidth = "80%"
                        SortExpression="Fecha de Última Disposición"
                        DataField="Fecha de Última Disposición" DataType="System.String">
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Fecha de Nacimiento" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Fecha de Nacimiento" FilterControlWidth = "80%"
                        SortExpression="Fecha de Nacimiento"
                        DataField="Fecha de Nacimiento" DataType="System.String">
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn HeaderText="Sexo" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Sexo" FilterControlWidth = "70%"
                        SortExpression="Sexo"
                        DataField="Sexo" DataType="System.String">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="Clavede Observación" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderStyle-HorizontalAlign="Justify"  UniqueName="Clave de Observación" FilterControlWidth = "80%"
                        SortExpression="Clave de Observación"
                        DataField="Clave de Observación" DataType="System.String">
                    </telerik:GridBoundColumn>                         
                </Columns>
            </MasterTableView>
                <ClientSettings>
                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True">
                </Scrolling>
            </ClientSettings>
        </telerik:RadGrid> 
        </td>              
       </tr>
      </table> 
     </div>
</asp:Content>