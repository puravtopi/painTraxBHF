﻿<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="AddDrugs.aspx.cs" Inherits="AddDrugs" %>

<%--test--%>

<%--test2--%>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">--%>
<style>
    .pager::before {
        display: none;
    }

    .pager table {
        margin: 0 auto;
    }
    /*this is the test comment*/
        .pager table tbody tr td a,
        .pager table tbody tr td span {
            position: relative;
            float: left;
            padding: 6px 12px;
            margin-left: -1px;
            line-height: 1.42857143;
            color: #337ab7;
            text-decoration: none;
            background-color: #fff;
            border: 1px solid #ddd;
        }

        .pager table > tbody > tr > td > span {
            z-index: 3;
            color: #fff;
            cursor: default;
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .pager table > tbody > tr > td:first-child > a,
        .pager table > tbody > tr > td:first-child > span {
            margin-left: 0;
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
        }

        .pager table > tbody > tr > td:last-child > a,
        .pager table > tbody > tr > td:last-child > span {
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }

        .pager table > tbody > tr > td > a:hover,
        .pager table > tbody > tr > td > span:hover,
        .pager table > tbody > tr > td > a:focus,
        .pager table > tbody > tr > td > span:focus {
            z-index: 2;
            color: #23527c;
            background-color: #eee;
            border-color: #ddd;
        }
</style>

<form runat="server" id="frm2">
    <div>
        <asp:Label runat="server" Text="Heading"></asp:Label>
        <asp:TextBox ID="txDesc" runat="server"></asp:TextBox>
        <asp:ImageButton ID="btnFind" runat="server" OnClick="btnFind_Click" ImageUrl="~/img/25-16.png" />
        <asp:Button ID="btnSave" Text="Save and Close" runat="server" OnClick="btnSave_Click" CssClass="btn" />
    </div>
    <div>
        <asp:GridView ID="dgvMedi" runat="server" AutoGenerateColumns="false" DataKeyNames="Medicine_ID">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                      <ItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsChkd")) %>' value='<%# Eval("IsChkd") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Medicine_ID" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Medicine_ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Medicine" ItemStyle-Width="1200">
                    <ItemTemplate>
                        <asp:TextBox ID="txtMediDesc" style="width:1200px"  runat="server" Text='<%# Eval("Medicine") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
             </Columns>
        </asp:GridView>


    </div>
</form>
<script src="Scripts/jquery.js"></script>
<script src="Scripts/jquery-ui.min.js"></script>

<script>
    $(document).ready(function () {
        $(".date").datepicker();
    });
</script>


