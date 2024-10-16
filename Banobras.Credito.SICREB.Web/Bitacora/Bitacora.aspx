<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Bitacora.aspx.cs" Inherits="Bitacora_Bitacora" Theme="Banobras2011" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <script type="text/javascript">
        //$(window).on('load', function () {
        //    $('#loading').hide();
        //});
    </script>
    
    <style type="text/css">

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px dashed #f00;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
            margin: 0 auto;
            text-align: center;
            padding-top: 35px;
        }

        .filters-container {
            display: flex; 
            border: 1px solid gray;
            padding-top: 5px;
            padding-bottom: 5px;
            padding-left: 5px;
        }

        .buttons-container {
            display: flex; 
            align-items: center; 
            margin: auto;
        }

        .btnSearch {
            background-color: #008CBA;
            border-radius: 4px;
            border-color: lightblue;
            color: white;
            height: 28px;
        }

        .btnRefresh {
            background-color: #555555;
            border-radius: 4px;
            margin-left: 5px;
            border-color: lightgray;
            color: white;
            height: 28px;
        }

        .btnExport {
            background-color: #04AA6D;
            border-radius: 4px;
            margin-left: 5px;
            border-color: lightgreen;
            color: white;
            height: 28px;
        }

        .hide {
            display: none;
        }

    </style>

    <%--<div class="loading">
        <div>
            Loading. Please wait.<br />
            <br />
            <img src="../Resources/Images/Loader.gif" alt="loading" />
        </div>
    </div>--%>

    <%--<div id="loading" name="loading" class="overlay">
        <i class="fas fa-2x fa-sync-alt fa-spin"></i>
            <img src="../Resources/Images/Loader.gif" />
      </div>--%>

<%--    <div id="myspindiv" style="display:none">
            <h2>Loading data -- please wait"</h2>
            <img src="../Resources/Images/Loader.gif" />
        </div>--%>

    <%--<div style="text-align:right">
        <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel"  CssClass="btnSmall" onclick="btnExportar_Click"  style="width: 16%;" />
    </div>--%>

    <%--Encabezado--%>
    <div class="titleCenter" style="text-align:center">
        <asp:Label runat="server" ID="lblTitle" Text="BITACORA DEL SISTEMA"></asp:Label>
    </div>

        <br />

    <%--Filtros--%>
    <div class="filters-container">
        <%--Fecha Inicial--%>
        <div style="margin-right: 20px;">            
            <asp:Label Text="Fecha de Inicio" runat="server" ID="lblFechaInicial" />        
            <telerik:RadDatePicker runat="server" ID="rdpFechaInicial" Calendar-CultureInfo="es-MX" Culture="es-MX" EnableTyping="False" Style="width: 100px;">
                <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x" />
                <DateInput ID="DateInput1" runat="server" ReadOnly="true" LabelCssClass="" Width="" />
                <DatePopupButton CssClass="" ToolTip="Abrir calendario" />
            </telerik:RadDatePicker>

        </div>
        
        <%--FechaFinal--%>
        <div style="margin-right: 20px;">
            <asp:Label Text="Fecha Fin" runat="server" ID="lblFechaFinal" />
            <telerik:RadDatePicker runat="server" ID="rdpFechaFinal" Calendar-CultureInfo="es-MX" Culture="es-MX" EnableTyping="False" Style="width: 100px;">
                <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x" />
                <DateInput ID="DateInput2" runat="server" ReadOnly="true" LabelCssClass="" Width="" />
                <DatePopupButton CssClass="" ToolTip="Abrir calendario" />
            </telerik:RadDatePicker>
        </div>        

        <%--Evento--%>
        <div style="margin-right: 20px; display: inline-grid;">
            <asp:Label Text="Evento" runat="server" ID="lblEvento" />
            <asp:DropDownList runat="server" ID="ddlEvento" Width="200px"></asp:DropDownList>
        </div>        

        <%--Usuario--%>
        <div style="display: inline-grid; margin-right: 20px;">
            <asp:Label Text="Usuario" runat="server" ID="lblUsuario" />
            <asp:DropDownList runat="server" ID="ddlUsuario" Width="300px"></asp:DropDownList>
        </div>

        <div class="buttons-container">
            <asp:Button class="btnSearch" Text="Consultar" runat="server" ID="btnSearch" OnClick="btnSearch_Click" OnClientClick="$('#myspindiv').show();"/>
            <asp:Button class="btnRefresh" Text="Refrescar" runat="server" ID="btnRefresh" OnClick="btnRefresh_Click" />
            <asp:Button class="btnExport" Text="Exportar a Excel" runat="server" ID="btnExport" OnClick="btnExport_Click"  />
        </div>
        
    </div>
    
        <br />
        <br />

    <%--Datos--%>
    <div id="gridBitacora" style="width: 100%;">
        <telerik:RadGrid runat ="server" ID ="rgBitacora" AutoGenerateColumns ="false" AllowSorting ="true" AllowPaging ="true" PageSize="10" 
		    onneeddatasource="rgBitacora_NeedDataSource" AllowFilteringByColumn="true" 
            OnItemDataBound="rgBitacora_ItemDataBound">
		    <ExportSettings FileName="SICREB-Bitacora" IgnorePaging="true" OpenInNewWindow="false" ExportOnlyData="true">
		    </ExportSettings>
	        <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
		    <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true" 
					    FirstPageToolTip="Primera Página"  
					    PrevPageToolTip="Página Anterior"  
					    NextPageToolTip="Página Siguiente"  
					    LastPageToolTip="Última Página" 
					    PageSizeLabelText="Registros por página: " 
					    PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
            <GroupingSettings CaseSensitive="false" />
		    <MasterTableView Name ="MtvBitacora" NoMasterRecordsText ="No existen Registros" CommandItemDisplay="Top" EditMode="InPlace"  Width="100%" CommandItemStyle-CssClass="hide" >
		        <CommandItemSettings 
                    ShowAddNewRecordButton="false"
                    ShowRefreshButton = "false" 
                    RefreshText="Actualizar Datos" 
                    ShowExportToCsvButton="false"  
                    ExportToCsvText ="Exportar a CSV" 
                    ShowExportToExcelButton="false" 
                    ExportToExcelText="Exportar a Excel" />
		            <Columns>

						<Telerik:GridBoundColumn HeaderText="Bitacora_Id" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify"
						    UniqueName="LogId" DataField="LogId" Display="false" 
						    DataType="System.Int32" MaxLength="5" Visible="false">
					    </Telerik:GridBoundColumn>

                        <Telerik:GridBoundColumn HeaderText="Fecha" AllowFiltering="false" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
						    UniqueName="CreationDate" DataField="CreationDate" DataFormatString=""
						    DataType="System.String" MaxLength="30">
					    </Telerik:GridBoundColumn>

                        <Telerik:GridBoundColumn HeaderText="Hora" AllowFiltering="false" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
						    UniqueName="Time" DataField="Time"
						    DataType="System.String" MaxLength="30">
					    </Telerik:GridBoundColumn>

						<Telerik:GridBoundColumn HeaderText="Identificador de Usuario" AllowFiltering="false" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
						    UniqueName="EmployeeNumber" DataField="EmployeeNumber" 
						    DataType="System.Int32" MaxLength="5">
					    </Telerik:GridBoundColumn>

						<Telerik:GridBoundColumn HeaderText="Login" AllowFiltering="false" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
						    UniqueName="UserLogin" DataField="UserLogin" Display="false" 
						    DataType="System.String" MaxLength="50" >
					    </Telerik:GridBoundColumn>

						<Telerik:GridBoundColumn HeaderText="Nombre" AllowFiltering="false" HeaderStyle-Width="18%" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
						    UniqueName="UserFullName" DataField="UserFullName"
						    DataType="System.String" MaxLength="100" >
					    </Telerik:GridBoundColumn>

						<Telerik:GridBoundColumn HeaderText="Evento_Id" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Justify"
						    UniqueName="EventId" DataField="EventId" Display="false" 
						    DataType="System.Int32" MaxLength="5" Visible="false">
					    </Telerik:GridBoundColumn>

                        <Telerik:GridBoundColumn HeaderText="Evento" AllowFiltering="false" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
						    UniqueName="EventName" DataField="EventName"
						    DataType="System.String" MaxLength="100">
					    </Telerik:GridBoundColumn>

					    <Telerik:GridBoundColumn HeaderText="Comentarios" AllowFiltering="false" HeaderStyle-Width="25%" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Center"
						    UniqueName="Comments" DataField="Comments"
						    DataType="System.String" MaxLength="500">
					    </Telerik:GridBoundColumn>

					    <Telerik:GridBoundColumn HeaderText="IP" AllowFiltering="false" HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center"
						    UniqueName="SessionIP" DataField="SessionIP" Display="false" 
						    DataType="System.String" MaxLength="15">
					    </Telerik:GridBoundColumn>

					</Columns>
		    </MasterTableView>
	    </telerik:RadGrid> 
    </div>

</asp:Content>