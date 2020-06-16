<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="DeleteConfirmMessage.aspx.cs" Inherits="DeleteConfirmMessage" %>

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
                    <small>Patients Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>

            <div class="row">

                <div class="alert alert-danger" runat="server" id="divfail" style="display: none">
                    Sorry !! we are not able to delete this record now.
                </div>
            </div>

            <div class="">

                <div class="row">
                    <div class="col-xs-12">
                        <div class="row">



                            <div class="col-xs-12">
                                <p style="color: red">Note : If you delete this patient ,it delete all IE and FU for this patient will be delete permanently from the system.</p>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">First Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ReadOnly="true" ID="txtFirstName" CssClass="form-control" placeholder="First Name" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Middle Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtMiddleName" ReadOnly="true" CssClass="form-control" placeholder="Middle Name" runat="server"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Last Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLastName" ReadOnly="true" CssClass="form-control" placeholder="Last Name" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">SEX</label>
                                    </div>
                                    <div class="col-sm-3">

                                        <asp:TextBox ID="txtSex" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>

                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">DOB</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtDOB" ReadOnly="true" CssClass="form-control" placeholder="DOB" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Handedness</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtHandedness" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Email</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtEmail" ReadOnly="true" CssClass="form-control" placeholder="Email" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Work Phone</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtWorkPhone" ReadOnly="true" CssClass="form-control" placeholder="Work Phone" runat="server"></asp:TextBox>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">Address 1</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtAdd1" ReadOnly="true" CssClass="form-control" placeholder="Address 1" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Address 2</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtAdd2" ReadOnly="true" CssClass="form-control" placeholder="Address 2" runat="server"></asp:TextBox>
                                    </div>


                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">City</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtCity" ReadOnly="true" CssClass="form-control" placeholder="City" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">State</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtState" ReadOnly="true" CssClass="form-control" placeholder="State" runat="server"></asp:TextBox>
                                    </div>


                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">Zip</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtZip" ReadOnly="true" CssClass="form-control" placeholder="Zip" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Home Phone</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPhone1" ReadOnly="true" CssClass="form-control" placeholder="xxx-xxx-xxxx" runat="server"></asp:TextBox>
                                    </div>



                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Mobile #</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPhone2" ReadOnly="true" CssClass="form-control" placeholder="xxx-xxx-xxxx" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-2">
                                        <label class="lblstyle">Account No</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtAccountNo" ReadOnly="true" CssClass="form-control" placeholder="Account No" runat="server"></asp:TextBox>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Policy No</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPolicyNo" ReadOnly="true" CssClass="form-control" placeholder="Policy No" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">SSN</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtSSN" ReadOnly="true" CssClass="form-control" placeholder="SSN" runat="server"></asp:TextBox>
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
                                            <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" OnClientClick="return confirm('If you delete this patient ,it delete all IE and FU for this patient will be delete permanently from the system.You still want to delete ?')" Text="Delete" OnClick="btnDelete_Click" />
                                            &nbsp;
                                                <asp:Button ID="btnBack" PostBackUrl="~/ViewPatientMaster.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
                                        </div>
                                    </div>

                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">

                    <div class="table-responsive">
                        <asp:GridView ID="gvPatientDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" DataKeyNames="PatientIE_ID" OnRowDataBound="gvPatientDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image alt="" title='<%# Eval("PatientIE_ID") %>' runat="server" ID="plusimg" Style="cursor: pointer" ImageUrl="img/plus.png" />
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                            <asp:GridView ID="gvPatientFUDetails" BorderStyle="None" CssClass="table table-bordered" Width="100%" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                                    <asp:BoundField DataField="Location" HeaderText="Location" />
                                                    <asp:BoundField DataField="MAProviders" HeaderText="MA & Providers" />

                                                </Columns>

                                                <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                            </asp:GridView>

                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Sex" HeaderText="Title" />
                                <asp:BoundField DataField="lastname" HeaderText="LastName" />
                                <asp:BoundField DataField="firstname" HeaderText="FirstName" />
                                <asp:BoundField DataField="DOB" HeaderText="DOB" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="DOA" HeaderText="DOA" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="Compensation" HeaderText="Case Type" />
                                <asp:BoundField DataField="location" HeaderText="Location" />



                            </Columns>
                            <PagerSettings PageButtonCount="5" />

                            <PagerStyle CssClass="pager"></PagerStyle>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnrodieid" runat="server" />
            <asp:HiddenField ID="hdnrodeditedfuid" runat="server" />
            <asp:HiddenField ID="hdnrodeditedfuieid" runat="server" />
            <asp:HiddenField ID="hfCurrentlyOpened" runat="server"></asp:HiddenField>

        </div>
    </div>
    <script>
        $(document).ready(function () {

            if ($('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]')) {
                $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').closest("tr").after("<tr><td></td><td colspan = '999'>" + $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').next().html() + "</td></tr>");
                $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').attr("src", "img/minus.png");
            }
            $("[src*=plus]").live("click", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                $(this).attr("src", "img/minus.png");
            });

            $("[src*=minus]").live("click", function () {
                $(this).attr("src", "img/plus.png");
                $(this).closest("tr").next().remove();
            });
        })
    </script>
</asp:Content>
