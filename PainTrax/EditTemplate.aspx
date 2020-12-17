<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditTemplate.aspx.cs" Inherits="EditTemplate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
 <%--   <script src="jquery-1.9.1.min.js"></script>--%>
    <script src="Scripts/jquery-1.8.2.js"></script>
    <script>
        $(document).ready(function () {
            $("#btnSave").on('click', function () {
                //    alert($("#HTMLCon").html());
                $('#<%= HtmlContent.ClientID %>').val($("#HTMLCon").html());
                $('#<%= form1.ClientID %>').submit();
            });

            $("#btnAdd").on('click', function () {
                //txtToAdd = $("#txtTag").val();


                var txtToAdd = prompt("Please enter tag:");
                if (txtToAdd != null && txtToAdd.trim().length > 0) {
                    var selection = window.getSelection();
                    //selection.modify('extend', 'backward', 'character');
                    document.execCommand('insertText', false, txtToAdd);

                }

            });
            $("#HTMLCon").on('keyup', function (e) {

                if (e.altKey && e.which == 65) {
                    var txtToAdd = prompt("Please enter tag:");
                    if (txtToAdd != null && txtToAdd.trim().length > 0) {
                        var selection = window.getSelection();
                        //selection.modify('extend', 'backward', 'character');
                        document.execCommand('insertText', false, txtToAdd);
                    }
                }

            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input id="btnAdd" type="button" value="Add" />
        <div runat="server" contenteditable="true" id="HTMLCon">
        </div>
        <br />
        <br />
        <input id="btnSave" type="button" value="Save" />

        <asp:HiddenField ID="HtmlContent" runat="server" />
    </form>
</body>

</html>
