<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemAlerts.aspx.cs" Inherits="runprocess.SystemAlerts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <asp:Button ID="btnAlerts" runat="server" Text="Envia Alertas" OnClick="btnAlerts_Click" />
         <br/><br/><br/>
         <asp:Label ID="lblMsg" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
