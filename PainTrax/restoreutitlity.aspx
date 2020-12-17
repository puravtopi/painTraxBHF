<%@ Page Language="C#" AutoEventWireup="true" CodeFile="restoreutitlity.aspx.cs" Inherits="restoreutitlity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-content-inner">
            <div class="page-content">
                <div class="page-header">
                    <h1>
                        <small>Utility								
									<i class="ace-icon fa fa-angle-double-right"></i>

                        </small>
                    </h1>
                </div>
                <div class="span12">

                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div>
                                            <fieldset>
                                                <legend>Restore IE</legend>
                                                <asp:Button runat="server" ID="btnNeckIE" Text="Neck" OnClick="btnIE_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnMBIE" Text="Midback" OnClick="btnMBIE_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnLBIE" Text="Lowback" OnClick="btnLBIE_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnShoulder" Text="Shoulder" OnClick="btnShoulder_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnElbowIE" Text="Elbow" OnClick="btnElbowIE_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnWristIE" Text="Wrist" OnClick="btnWristIE_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnKneeIE" Text="Knee" OnClick="btnKneeIE_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnHipIE" Text="Hip" OnClick="btnHipIE_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnAnkleIE" Text="Ankle" OnClick="btnAnkleIE_Click" />
                                            </fieldset>
                                            <fieldset>
                                                <legend>Restore FU</legend>
                                                <asp:Button runat="server" ID="btnFU" Text="Neck" OnClick="btnFU_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnMBFU" Text="Midback" OnClick="btnMBFU_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnLBFU" Text="Lowback" OnClick="btnLBFU_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnSHoulderFU" Text="Shoulder" OnClick="btnSHoulderFU_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnELbowFU" Text="Elbow" OnClick="btnELbowFU_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnWristFU" Text="Wrist" OnClick="btnWristFU_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnKneeFU" Text="Knee" OnClick="btnKneeFU_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnHipFU" Text="Hip" OnClick="btnHipFU_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnAnkleFU" Text="Ankle" OnClick="btnAnkleFU_Click" />
                                            </fieldset>
                                            <fieldset>
                                                <legend>Page 1</legend>
                                                <asp:Button runat="server" ID="btnpage1" Text="Restore" OnClick="btnpage1_Click" />
                                            </fieldset>
                                            <fieldset>
                                                <legend>Page 2</legend>
                                                <asp:Button runat="server" ID="btnpage2" Text="Restore" OnClick="btnpage2_Click" />
                                            </fieldset>
                                            <fieldset>
                                                <legend>Page 3</legend>
                                                <asp:Button runat="server" ID="btnpage3" Text="Restore" OnClick="btnpage3_Click" />
                                            </fieldset>

                                            <fieldset>
                                                <legend>Physical</legend>
                                                <asp:Button runat="server" ID="btkPhyIE" Text="IE" OnClick="btkPhyIE_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnPhyFU" Text="FU" OnClick="btnPhyFU_Click" />
                                            </fieldset>
                                            <fieldset>
                                                <legend>ROS Update</legend>
                                                <asp:Button runat="server" ID="btnROSUpdate" Text="Update" OnClick="btnROSUpdate_Click" />

                                            </fieldset>
                                            <fieldset>
                                                <legend>Page 3 Update</legend>
                                                <asp:Button runat="server" ID="btnPage3Update" Text="Update" OnClick="btnPage3Update_Click" />

                                            </fieldset>

                                             <fieldset>
                                                <legend>Page 2 Activity Affected Update</legend>
                                                <asp:Button runat="server" ID="btnActivity" Text="Update" OnClick="btnActivity_Click" />

                                            </fieldset>

                                            <label runat="server" id="lblMess"></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
