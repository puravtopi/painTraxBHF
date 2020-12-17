<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditTemplate1.aspx.cs" Inherits="EditTemplate1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top:50px;text-align:center" >
        <h3 style="font-family:Calibri">Select Html</h3>
        <br />
        <asp:ListBox ID="ListFile" style="width:500px;height:500px" runat="server"></asp:ListBox>
        <br />
        <br />
        <asp:Button ID="BtnSelect" runat="server" Text="Select" OnClick="BtnSelect_Click" />
    </div>
    </form>
</body>
</html>
