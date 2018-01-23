<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="tyres.aspx.cs" Inherits="DekkOnline.tyres" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HomeContent" runat="server">
    <script>
        function loadProducts(s, e) {
            xcpProducts.PerformCallback();
        }


        function showDetails(s, e) {
            
            s.GetRowValues(s.GetFocusedRowIndex(), 'Id', setDP);
        }

        function setDP(values) {
            
            $("#pdImage").html($("#psImage" + values).html());
            popDetails.Show();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="searchWrap">
        <div class="searchContain">
            <div class="col-md-3">
                <span class="sTitle">SIZE:</span><br />
                <div class="floatLt dp1">
                      <dx:ASPxComboBox runat="server" ID="cmbWidth" ClientInstanceName="cmbWidth" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                        <DropDownButton ImagePosition="Bottom">
                            <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                        </DropDownButton>
                          <ClientSideEvents ValueChanged="loadProducts" />
                    </dx:ASPxComboBox>
                </div>
                <div class="floatLt diagonal">
                </div>
                <div class="floatLt dp2">
                    <dx:ASPxComboBox runat="server" ID="cmbProfile" ClientInstanceName="cmbProfile" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                        <DropDownButton ImagePosition="Bottom">
                            <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                        </DropDownButton>
                          <ClientSideEvents ValueChanged="loadProducts" />
                    </dx:ASPxComboBox>
                </div>
                <div class="floatLt dp3">
                   <dx:ASPxComboBox runat="server" ID="cmbDiameter" ClientInstanceName="cmbDiameter" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                        <DropDownButton ImagePosition="Bottom">
                            <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                        </DropDownButton>
                          <ClientSideEvents ValueChanged="loadProducts" />
                    </dx:ASPxComboBox>
                </div>
            </div>
            <div class="col-md-3">
                <span class="sTitle">COVER TYPE:</span><br />
                <dx:ASPxComboBox runat="server" ID="cmbCategory" ClientInstanceName="cmbCategory" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                    <DropDownButton ImagePosition="Bottom">
                        <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                    </DropDownButton>
                    <ClientSideEvents ValueChanged="loadProducts" />
                </dx:ASPxComboBox>
            </div>
            <div class="col-md-3">
                <span class="sTitle">BRAND:</span><br />
                <dx:ASPxComboBox runat="server" ID="cmbBrand" ClientInstanceName="cmbBrand" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                    <DropDownButton ImagePosition="Bottom">
                        <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                    </DropDownButton>
                    <ClientSideEvents ValueChanged="loadProducts" />
                </dx:ASPxComboBox>
            </div>
            <div class="col-md-3">
                <span class="sTitle">PRICE:</span><br />
                <dx:ASPxComboBox runat="server" ID="cmbPrice" ClientInstanceName="cmbPrice" CssClass="BoxGeneral" Border-BorderStyle="None">
                    <DropDownButton ImagePosition="Bottom">
                        <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                    </DropDownButton>
                    <Items>
                        <dx:ListEditItem Selected="true" Text="lowest - highest" Value="0" />
                        <dx:ListEditItem Selected="false" Text="highest - lowest" Value="1" />
                    </Items>
                    <ClientSideEvents ValueChanged="loadProducts" />
                </dx:ASPxComboBox>
            </div>
            <div class="clearfix"></div>
            <div class="spacer10"></div>
            <div class="clearfix"></div>

            <dx:ASPxCallbackPanel runat="server" ID="xcpProducts" ClientInstanceName="xcpProducts" Width="100%" OnCallback="xcpProducts_Callback">
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxGridView runat="server" ID="xgvProducts" CssClass="BoxGeneral" KeyFieldName="Id" Border-BorderStyle="None" Settings-GridLines="None" SettingsBehavior-AllowFocusedRow="true">
                            <Settings ShowColumnHeaders="false" />
                            <Styles>
                                <Row BackColor="#f1f2f3" Cursor="pointer"></Row>
                                <FocusedRow BackColor="White"></FocusedRow>
                            </Styles>
                            <Columns>
                                <dx:GridViewDataColumn>
                                    <DataItemTemplate>
                                        <div class="pad10">

                                            <div class="col-md-2" style='height: 200px;' id='psImage<%# Eval("Id") %>'>
                                                <div class="searchResultImg" style='background-image: url(<%# Eval("Image").ToString() == "" ? "/photos/noPhoto.png" :  Eval("Image") %>);'></div>
                                            </div>
                                            <div class="col-md-7 searchResultDescription">
                                                <img id='imgCat<%# Eval("Id") %>' class="imgCat" src="<%# Eval("CategoryImage") %>" />
                                                <div class="clearfix"></div>
                                                <h3><%# Eval("Name") %></h3>
                                                <div class="clearfix"></div>
                                                <div class="spacer10"></div>
                                                <div class="clearfix"></div>
                                                <div class="dimentions">
                                                    <%# Eval("Width") %> &nbsp;&nbsp;/&nbsp;&nbsp; <%# Eval("Profile") %>&nbsp;&nbsp; <%# Eval("Diameter") %>
                                                </div>
                                                <div class="extraInfo">
                                                    <div class="floatLt">
                                                        <img src="/Scripts/imgs/tyreFuel.png" alt="Efficiency" />
                                                    </div>
                                                    <div class="floatLt round">
                                                        <%# Eval("Fuel") %>
                                                    </div>
                                                    <div class="floatLt">
                                                        <img src="/Scripts/imgs/tyreWet.png" alt="Wet" />
                                                    </div>
                                                    <div class="floatLt round">
                                                        <%# Eval("Wet") %>
                                                    </div>
                                                    <div class="floatLt">
                                                        <img src="/Scripts/imgs/tyreNoise.png" alt="Noise" />
                                                    </div>
                                                    <div class="floatLt round">
                                                        <%# Eval("Noise") %>
                                                    </div>



                                                </div>

                                                <div class="clearfix"></div>
                                            </div>
                                            <div class="col-md-3 srd">
                                                <div class="stock">
                                                    <div class="labelStock">In stock</div>
                                                    <div class="stockNumber">(<%# Eval("Stock") %>)</div>
                                                </div>
                                                <div class="clearfix"></div>
                                                <div class="price ">
                                                    <%# Eval("Price","{0:c0}") %>
                                                </div>
                                                <div class="brand ">
                                                    <img src="<%# Eval("BrandImage") %>" class='<%# Eval("BrandImage") == "" ? "hidden" : "" %>' />
                                                    <div class='brandText <%# Eval("Brand") == "" ? "hidden" : "" %>'><%# Eval("Brand") %></div>
                                                </div>
                                                <div class="clearfix"></div>
                                                <div class="cart">
                                                    Add to cart: 
                                                </div>
                                                <div class="addCart">
                                                    <dx:ASPxSpinEdit runat="server" ID="spDiameter" ClientInstanceName='spDiameter<%# Eval("Id") %>' Width="45px">
                                                        <Buttons>
                                                            <dx:SpinButtons ClientVisible="false"></dx:SpinButtons>
                                                        </Buttons>
                                                    </dx:ASPxSpinEdit>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="clearfix"></div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                            </Columns>
                            <ClientSideEvents RowDblClick="showDetails" />
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

        </div>
    </div>
    <div class="clearfix"></div>


    <dx:ASPxPopupControl runat="server" Width="600px" ID="popDetails" ClientInstanceName="popDetails" HeaderText="Product Details" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <HeaderStyle BackColor="#dd0330" ForeColor="White" Font-Size="Medium" Font-Names="" />
        <ContentCollection>
            <dx:PopupControlContentControl>
                <div class="col-md-4">
                    <div id="pdImage" style='height: 220px;'>
                    </div>
                </div>
                <div class="pdDescription">
                </div>
                <div class="col-md-8"></div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>


</asp:Content>
