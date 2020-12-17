<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="setting.aspx.cs" Inherits="setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .lblstyle {
            float: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">

    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Settings Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>

            <div class="">

                <div class="row">
                    <div class="col-xs-12">

                        <div class="row">
                            <div class="alert alert-success" runat="server" id="divSuccess" style="display: none">
                                Setting Updated Successfully.
                            </div>
                            <div class="alert alert-danger" runat="server" id="divfail" style="display: none">
                                Sorry !! this is any issue right now to update the data.
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Forward CC </label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:CheckBox runat="server" ID="chkCC" />
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Forward PE</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:CheckBox runat="server" ID="chkPE" />
                                    </div>
                                </div>


                            </div>
                              <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Forward ROM </label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:CheckBox runat="server" ID="chkROM" />
                                    </div>
                                  
                                </div>


                            </div>


                            <div class="clearfix"></div>
                            <br />


                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">&nbsp;</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                            &nbsp;
                                                <asp:Button ID="btnBack" PostBackUrl="~/setting.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Cancel" />
                                        </div>
                                    </div>

                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>

