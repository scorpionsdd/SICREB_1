<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfLog.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="PRAGMA" content="NO-CACHE">
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE">
    <title>Log de Errores</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table width = "100%">
       <tr>
         <td style = "width:80%"  >
           <table width = "100%">
             <tr>
              <td >
                 
                  <asp:Panel ID="Panel1" runat="server">
                      <asp:TextBox ID="tbCadena" runat="server" TextMode="MultiLine" Width="100%" Height = "100%"></asp:TextBox>
                  </asp:Panel>
                
              </td>
             </tr>
           </table>
         </td>
       </tr>       
      </table>    
    </div>
    </form>
</body>
</html>
