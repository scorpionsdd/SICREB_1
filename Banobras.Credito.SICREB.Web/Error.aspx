<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Ha ocurrido un error</h2>
            <br />
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <br />
            <ul>
                <li>
                    <asp:HyperLink ID="linklogin" runat="server" NavigateUrl="~/Login.aspx">Regresar a Inicio</asp:HyperLink>
                </li>
            </ul>
        </div>
    </form>
</body>
</html>
