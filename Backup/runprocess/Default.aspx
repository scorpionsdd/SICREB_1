<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="runprocess.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="P R O C E S A R" onclick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="Download (.txt)" OnClick="Button2_Click" />
    </div>
    <asp:Label ID="lblEstado" runat="server"></asp:Label>
    </form>
</body>
</html>
