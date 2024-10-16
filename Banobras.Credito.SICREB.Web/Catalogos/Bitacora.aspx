<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"    CodeFile="Bitacora.aspx.cs" Inherits="Catalogos_Bitacora" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
     <div style="width: 964px;" class="Bitacora">           
        <table cellpadding="0" cellspacing="0" border="0" width="950px" align="center">
            <tr>                
               <td width="100%" align="left">
                 <table cellpadding="0" cellspacing="0" border="0" width="33%" align="left">
                    <tr>
                     <td width="40%" align="left">
                       <asp:Label Text="FILTROS" runat="server" ID="Label4"></asp:Label>
                     </td>
                     <td width="10%" align="left">                     
                     </td>
                     <td width="50%" align="left">
                     </td>
                   </tr>
                    <tr>
                     <td colspan = "3" width="100%" align="center">
                      <asp:Label Text="Selección de Fecha" runat="server" ID="Label1"></asp:Label>
                     </td>                    
                   </tr>
                    <tr>
                     <td width="40%" align="left">
                        <asp:Label Text="Fecha Inicial:" runat="server" ID="Label2" Width="90px"></asp:Label>
                     </td>
                     <td width="10%" align="left">                     
                     </td>
                     <td width="50%" align="left">
                      <telerik:RadDatePicker ID="txtFechaInicial" runat="server" Width="95px" Calendar-CultureInfo="es-MX"
                                Culture="es-MX" EnableTyping="False">
                                <Calendar ID="Calendar1" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                    ViewSelectorText="x" />
                                <DateInput ID="DateInput1" runat="server" ReadOnly="true" LabelCssClass="" Width="">
                                </DateInput>
                                <DatePopupButton CssClass="" ToolTip="Abrir el Calendario." />
                            </telerik:RadDatePicker>
                     </td>
                   </tr>
                    <tr>
                     <td width="40%" align="left">
                     </td>
                     <td width="10%" align="left">                     
                     </td>
                     <td width="50%" align="left">
                     </td>
                   </tr>
                    <tr>
                     <td width="40%" align="left">
                       <asp:Label Text="Fecha Final:" runat="server" ID="Label3"></asp:Label>
                     </td>
                     <td width="10%" align="left">                     
                     </td>
                     <td width="50%" align="left">
                      <telerik:RadDatePicker ID="txtFechaFinal" runat="server" Width="95px" Calendar-CultureInfo="es-MX"
                                OnSelectedDateChanged="txtFechaFinal_SelectedDateChanged" AutoPostBack="True"
                                Culture="es-MX" EnableTyping="False">
                                <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                    ViewSelectorText="x" />
                                <DateInput ID="DateInput2" runat="server" ReadOnly="true" LabelCssClass="" Width="">
                                </DateInput>
                                <DatePopupButton CssClass="" ToolTip="Abrir el Calendario." />
                            </telerik:RadDatePicker>
                     </td>
                   </tr>
                 </table>
               </td>         
            </tr>

            <tr>                
                <td width="100%" align="right">
                    <asp:ImageButton ID="btnExportPDF" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                        runat="server" Width="22px" OnClick="btnExportPDFBitacora_Click1" />
                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                        runat="server" Width="22px" OnClick="btnExportExcelBitacora_Click1" />
                </td>              
            </tr>
            <tr>              
                <td  width="100%"  id="tdRubroAll" class="titleCenter">
                   <asp:Label runat="server" ID="lblTitle" Text="BITÁCORA DE CATÁLOGO"></asp:Label>
                </td>               
            </tr>
            <tr>
             <td  width="950px" >
               <telerik:RadGrid runat="server" ID="RgdBitacora" AutoGenerateColumns="false" AllowSorting="true" AllowScroll="true" Width = "950px"
            AllowPaging="true" PageSize="10" AllowFilteringByColumn="True" 
            OnNeedDataSource="RgdBitacora_NeedDataSource">
             <ExportSettings FileName="BitacoraCatalogos" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Bitácora Catálogos" />
             </ExportSettings>
            <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página" Width = "950px"
                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView  NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top"
                 Width="950px">
                <CommandItemSettings ShowRefreshButton="true" RefreshText="Actualizar Datos" ShowAddNewRecordButton="false" />
                <Columns>
                    <telerik:GridBoundColumn HeaderText="ID USUARIO" HeaderStyle-Width="100px"  ItemStyle-Width="100px"  FilterControlWidth = "70%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="ID_USUARIO" DataField="idUsuario"
                        DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="ROL" HeaderStyle-Width="100px"  ItemStyle-Width="100px" FilterControlWidth = "70%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="ROL" DataField="Login" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="NOMBRE CATÁLOGO" HeaderStyle-Width="200px" ItemStyle-Width="200px"  FilterControlWidth = "80%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="NOMBRE_CATALOGO" DataField="Catalogo"
                        DataType="System.String" >
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn HeaderText="FECHA REGISTRO" HeaderStyle-Width="150px" ItemStyle-Width="150px"  FilterControlWidth = "80%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="FECHA_REGISTRO" DataField="FECHA" PickerType="DatePicker"
                        DataType="System.DateTime">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn HeaderText="ACTIVIDAD" HeaderStyle-Width="150px" ItemStyle-Width="150px"  FilterControlWidth = "80%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="DESCRIPCION" DataField="DESCRIPCION"
                        DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="DESCRIPCIÓN" HeaderStyle-Width="350px" ItemStyle-Width="250px"  FilterControlWidth = "90%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="detalle" DataField="detalle" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="DATO INICIAL" HeaderStyle-Width="350px" ItemStyle-Width="250px"  FilterControlWidth = "90%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="antes" DataField="antes" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="DATO FINAL" HeaderStyle-Width="350px"  ItemStyle-Width="250px"  FilterControlWidth = "90%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="despues" DataField="despues" DataType="System.String">
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
            <tr>                
                <td width="100%" align="right">
                    <asp:ImageButton ID="ImageButton2" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-PDF.png"
                        runat="server" Width="22px" OnClick="btnExportPDFDatos_Click1" />
                    <asp:ImageButton ID="ImageButton3" ImageUrl="~/ResourcesSICREB/Images/BNBPF-ICO-XLS.png"
                        runat="server" Width="22px" OnClick="btnExportExcelDatos_Click1" />
                </td>              
            </tr>
            <tr>               
                <td  width="100%"  id="td2" class="titleCenter">
                  <asp:Label runat="server" ID="lblTitle2" Text="BITÁCORA ACCESO A DATOS"></asp:Label>
                </td>               
            </tr>
            <tr>
             <td width="950px">  
                         <telerik:RadGrid runat="server" ID="RgdBitacoraDatos" AutoGenerateColumns="false" AllowScroll="true"
            AllowSorting="true" AllowPaging="true" PageSize="10" AllowFilteringByColumn="True"
            OnNeedDataSource="RgdBitacora_NeedDataSource">
            <ExportSettings FileName="BitacoraAccesoDatos" IgnorePaging="false" OpenInNewWindow="true" ExportOnlyData="true">
                        <Pdf PaperSize="Letter" PageTitle="Bitácora Acceso Datos" />
                    </ExportSettings>
            <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" FirstPageToolTip="Primera Página"
                PrevPageToolTip="Página Anterior" NextPageToolTip="Página Siguiente" LastPageToolTip="Última Página"
                PageSizeLabelText="Registros por página: " PagerTextFormat="{4} Consultando página {0} de {1}, {5} Registros en {1} páginas." />
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top"
                 Width="100%">
                <CommandItemSettings ShowRefreshButton="true"  RefreshText="Actualizar Datos" ShowAddNewRecordButton="false" />
                <Columns>
                    <telerik:GridBoundColumn HeaderText="ID USUARIO" HeaderStyle-Width="9%" ItemStyle-Width="9%" FilterControlWidth = "60%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="ID_USUARIO" DataField="ID_USUARIO"
                        DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="ROL" HeaderStyle-Width="11%" ItemStyle-Width="11%" FilterControlWidth = "70%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="ROL" DataField="ROL" DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn HeaderText="FECHA REGISTRO" HeaderStyle-Width="16%" ItemStyle-Width="16%" FilterControlWidth = "80%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="FECHA_REGISTRO" DataField="FECHA_REGISTRO" PickerType="DatePicker"
                        DataType="System.DateTime">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn HeaderText="ACTIVIDAD" HeaderStyle-Width="30%" ItemStyle-Width="30%" FilterControlWidth = "90%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="DESCRIPCIOND" DataField="DESCRIPCION"
                        DataType="System.String">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="DESCRIPCIÓN" HeaderStyle-Width="34%" ItemStyle-Width="34%" FilterControlWidth = "90%"
                        HeaderStyle-HorizontalAlign="Left" UniqueName="DET" DataField="DETALLE" DataType="System.String">
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
    </div>
</asp:Content>
