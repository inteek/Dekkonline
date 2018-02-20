<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DekkOnline._Default" Async="true" %>

<%--<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>--%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--    <asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">--%>
    <%--    <h2><%: Title %>.</h2>--%>
    <%-- <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h4>Use a local account to log in.</h4>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="The email field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
                <p>
                    <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register as a new user</asp:HyperLink>
                </p>
                <p>
                    <%-- Enable this once you have account confirmation enabled for password reset functionality
                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink> 
                </p>
            </section>
        </div>

        <div class="col-md-4">
            <section id="socialLoginForm">
                <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
            </section>
        </div>
    </div>--%>
    <div id="Login" role="dialog" aria-hidden="true">
        <div class="col-lg-2"></div>
        <div class="col-lg-8">
            <div class="">
                <div class="clearfix"></div>
                <div class="spacer20"></div>
                <div class="clearfix"></div>
                <div class="col-xs-12 loginLogo">
                </div>
                <div class="clearfix"></div>
                <div class="loginContain">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <%--<div class="modalLoginLt">--%>
                        <h4>ALREADY A DEKKONLINE MEMBER:</h4>
                        <div class="clearfix"></div>
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="clearfix"></div>
                        <asp:TextBox runat="server" ID="Email" CssClass="txtLogin txtUser" TextMode="Email" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="The email field is required." ValidationGroup="Login" />
                        <div class="clearfix"></div>
                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="txtLogin txtPass" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." ValidationGroup="Login" />
                        <div class="clearfix"></div>
                        <%--<asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="loginBtn" ValidationGroup="Login" />--%>
                        <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn loginBtn" />
                        <%--</div>--%>
                    </div>
                    <div class="col-md-3"></div>
                    <%-- %> <div class="col-md-6">
                     <%--   <div class="modalLoginRt">
                            <h4>REGISTER:</h4>
                            <div class="clearfix"></div>
                            <asp:PlaceHolder runat="server" ID="NewErrorMessage" Visible="false">
                                <p class="text-danger">
                                    <asp:Label runat="server" ID="lblNewFailureText" ClientIDMode="Static"></asp:Label>
                                </p>
                            </asp:PlaceHolder>

                            <asp:ValidationSummary runat="server" CssClass="text-danger" />
                           <<asp:TextBox runat="server" ID="NewEmail" CssClass="txtLogin txtUser" TextMode="Email" /
                            <asp:TextBox runat="server" ID="NewEmail" CssClass="txtLogin txtUser" TextMode="Email" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="NewEmail" ValidationGroup="Create"
                                CssClass="text-danger" ErrorMessage="The email field is required." />
                            <div class="clearfix"></div>

                            <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="txtLogin txtPass" />
                            <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="txtLogin txtPass" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword" ValidationGroup="Create"
                                CssClass="text-danger" ErrorMessage="The password field is required." />
                            <div class="clearfix"></div>
                            <<asp:Button runat="server" OnClick="CreateUser_Click" ID="CreateUser" Text="Sign Up" CssClass="loginBtn" ValidationGroup="Create" />
                            <asp:Button runat="server" OnClick="CreateUser_Click" ID="CreateUser" Text="Sign Up" CssClass="btn loginBtn" ValidationGroup="Create"/>
                        </div>

                    </div>
                </div> --%>
                    <div class="hidden">
                        <div class="col-md-offset-2 col-md-10">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            </div>
                        </div>
                    </div>

                    
                      <div class="clearfix"></div>
                    <div class="spacer20"></div>
                    <div class="clearfix"></div>

                    <div class="clearfix"></div>
                    <div class="spacer20"></div>
                    <div class="clearfix"></div>
                    
                      <div class="clearfix"></div>
                    <div class="spacer20"></div>
                    <div class="clearfix"></div>


                    <div class="clearfix"></div>
                    <div class="spacer20"></div>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="col-lg-2"></div>
        </div>
        <p class="hidden">
            <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register as a new user</asp:HyperLink>
        </p>
</asp:Content>
