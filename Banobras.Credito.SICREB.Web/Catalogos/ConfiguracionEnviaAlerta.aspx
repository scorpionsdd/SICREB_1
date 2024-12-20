﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="ConfiguracionEnviaAlerta.aspx.cs" Inherits="ConfiguracionEnviaAlerta" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
 <script type="text/javascript">    
    </script>
 <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td>
            </td>
            <td width="100%" align="right">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitle" Text="Configuración de Alertas"></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
        <tr>
            <td valign="top" width="450px">
                <asp:Label ID="lbl01" runat="server" Text="Alertas Disponibles"></asp:Label>
                <br />
                <br />
                <asp:DropDownList ID="drplist_Alerta" Width="250px" runat="server">
                </asp:DropDownList>
                <br />
                <br />
                <br />
                <br />
                <asp:Button runat="server" ID="btn_aceptarConfigAlerta" Text="Guardar" OnClick="btn_aceptarConfigAlerta_Click" />
            </td>
            <td id="td1" width="600px">
                <telerik:RadGrid ID="RgdCorreos" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" GridLines="None" OnPageIndexChanged="RgdCorreosPageIndexChanged"
                    OnItemDataBound="RgdCorreos_ItemDataBound" OnNeedDataSource="RgdCorreos_NeedDataSource"
                    Width="100%">
                    <MasterTableView ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true"
                        NoMasterRecordsText="No existen Registros" CommandItemDisplay="Top" Width="100%">
                        <CommandItemSettings ShowAddNewRecordButton="false" /> 
                        <ExpandCollapseColumn Visible="true">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="Usuario" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="Justify" UniqueName="USUARIO" DataField="USUARIO">
                                <HeaderStyle HorizontalAlign="Justify" Width="30%"></HeaderStyle>
                                <ItemStyle Width="20%"></ItemStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Correo Electrónico" HeaderStyle-Width="50%"
                                ItemStyle-Width="50%" HeaderStyle-HorizontalAlign="Justify" UniqueName="EMAIL"
                                DataField="EMAIL" DataType="System.String" >
                                <HeaderStyle HorizontalAlign="Justify" Width="50%"></HeaderStyle>
                                <ItemStyle Width="50%"></ItemStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Seleccionar" HeaderStyle-Width="20%" DataType="System.Boolean"
                                UniqueName="SELECCION">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle Width="20%"></HeaderStyle>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
</asp:Content>
