<%@ Page Language="C#" MasterPageFile="~/MasterLogin.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Loginx" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainPLogin" runat="Server">
    <table cellpadding="0" cellspacing="0" border="0" width="100%" style="height: 687px; top: 0px; left: 0px;
        background-image: url('ResourcesSICREB/Images/HeadersFooters/LoginScreen01.png');">
        <tr style="height: 101%;">
            <td>
                <telerik:RadScriptManager ID="RadScriptManager2" runat="server">
                </telerik:RadScriptManager>
                <telerik:RadWindowManager runat="server" ID="Manager1">
                </telerik:RadWindowManager>
                <%--Cross Site Request Forgery (WSTG-SESS-05)--%>
                <asp:HiddenField ID="csrfTokenField" runat="server" OnValueChanged="csrfToken_ValueChanged" />
                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="height: 100%; top: 0px; left: 0px;">
                    <tr align="center" style="height: 30%;">
                        <td colspan="4" style="width: 100%;">
                        </td>
                    </tr>
                    <tr style="height: 5%;">
                        <td style="width: 39%;">
                        </td>
                        <td align="left" style="width: 10%;">
                            <label>
                                <br />
                                Usuario:</label>
                        </td>
                        <td align="right" style="width: 16%;">
                            <br />
                            <asp:TextBox ID="txtUsuario" runat="server" Style="text-align: left" Width="100%"></asp:TextBox>
                        </td>
                        <td style="width: 47%;">
                        </td>
                    </tr>
                    <tr style="height: 5%;">
                        <td style="width: 39%;">
                        </td>
                        <td align="left" style="width: 10%;">
                            <label>
                                <br />
                                Contraseña:</label>
                        </td>
                        <td align="right" style="width: 16%;">
                            <br />
                            <asp:TextBox ID="txtContrasenia" runat="server" TextMode="Password" Style="text-align: left" Width="100%" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td style="width: 47%;">
                        </td>
                    </tr>
                    <tr align="center" style="height: 5%;">
                        <td style="width: 39%;">
                        </td>
                        <td align="left" style="width: 10%;">
                        </td>
                        <td style="width: 16%;">
                            <asp:ImageButton ID="ImgBtnIngresar" runat="server" ImageAlign="AbsBottom" ImageUrl="ResourcesSICREB/Images/BarsButtons/BNB-SIC-Button-Ingresar.png" OnClick="ImgBtnIngresar_Click" />
                        </td>
                        <td style="width: 47%;">
                        </td>
                    </tr>
                    <tr style="height: 20%;">
                        <td colspan="4">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
