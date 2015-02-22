<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Amon.cc.Oidc.SampleApplication.Secure.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>For authenticated people only</h1>
	<p>You are authenticated as: <asp:Label runat="server" ID="user"/></p>

    <asp:Panel runat="server" ID="userInformationPanel" Visible="false">
    <asp:Image runat="server" id="profilePicture" /><br />
    Full name: <asp:Literal runat="server" ID="fullName" /><br />
    Gender: <asp:Literal runat="server" ID="gender" />
    </asp:Panel>

	<p><asp:LinkButton runat="server" ID="signoff" onclick="signoff_Click">Local Sign-Off from application</asp:LinkButton></p>
    </div>
    </form>
</body>
</html>
