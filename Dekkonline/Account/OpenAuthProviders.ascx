<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenAuthProviders.ascx.cs" Inherits="DekkOnline.Account.OpenAuthProviders" %>

    <asp:ListView runat="server" ID="ListView1" ItemType="System.String"
        SelectMethod="GetProviderNames" ViewStateMode="Disabled">
        <ItemTemplate>
            
                <button type="submit" class="btnLogin<%#: Item %>" style="width:110px" name="provider" value="Sign up with <%#: Item %>"
                    title="Log in using your <%#: Item %> account.">
                    <%#: Item %>
                </button>
            
        </ItemTemplate>
    </asp:ListView>

<div class="clearfix"></div>