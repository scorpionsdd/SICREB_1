<%@ Page MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeFile="ActivarDesactivarIFRS9.aspx.cs" Inherits="Catalogos_ActivarDesactivarIFRS9" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%" >
        <tr>
            <td></td>
            <td align="right">                
            </td>
            <td></td>
        </tr>
        <tr>
            <td id="tdRubroIzq" class="titleLeft">
                &nbsp;
            </td>
            <td id="tdRubroAll" class="titleCenter">
                &nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Activar / Desactivar IFRS9. "></asp:Label>
            </td>
            <td id="tdRubroDer" class="titleRight">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="500px" class="tableinicio">
                    <tr>
                        <td>
                            <table width="480px" class="tableinicio">
                                <tr>
                                    <td>
                                        <div class="titleCenter">
                                            &nbsp;SICOFIN
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="480px" class="tableinicio">
                                <tr>
                                    <td>
                                        <asp:RadioButtonList 
                                            ID="rbSicofin" 
                                            runat="server" 
                                            AutoPostBack="True" 
                                            RepeatDirection="Horizontal" 
                                            OnSelectedIndexChanged="rbSICOFIN_SelectedIndexChanged">
                                            <%--<asp:ListItem Value="CT_2021">Catálogo 2021</asp:ListItem>--%>
                                            <asp:ListItem Value="CT_SICO">Catálogo Anterior</asp:ListItem>
                                            <asp:ListItem Value="CN_SICO" Selected="True">Catálogo IFRS9</asp:ListItem>                                            
                                        </asp:RadioButtonList>
                          
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="500px" class="tableinicio">
                    <tr>
                        <td>
                            <table width="480px" class="tableinicio">
                                <tr>
                                    <td>
                                        <div class="titleCenter">
                                            &nbsp;SIC
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="480px" class="tableinicio">
                                <tr>
                                    <td>
                                        <asp:RadioButtonList 
                                            ID="rbSic" 
                                            runat="server" Enabled="False"
                                            AutoPostBack="True" 
                                            RepeatDirection="Horizontal" OnSelectedIndexChanged="rbSIC_SelectedIndexChanged">
                                            <asp:ListItem Value="CT_SIC">Catálogo Anterior</asp:ListItem>
                                            <asp:ListItem Value="CN_SIC">Catálogo IFRS9</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="500px" class="tableinicio">
                    <tr>
                        <td>
                            <table width="480px" class="tableinicio">
                                <tr>
                                    <td>
                                        <div class="titleCenter">
                                            &nbsp;CALIFICACIONES
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="480px" class="tableinicio">
                                <tr>
                                    <td>
                                        <asp:RadioButtonList 
                                            ID="rbCalif" 
                                            runat="server" 
                                            AutoPostBack="True"  
                                            RepeatDirection="Horizontal" OnSelectedIndexChanged="rbCalif_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="CT_PCC">SICALC</asp:ListItem>
                                            <asp:ListItem Value="CN_PCC" Enabled="False">PCC</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

    </table>
    <br />
</asp:Content>