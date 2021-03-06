﻿<%@ Page Title="DekkOnline" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="DekkOnline._Default" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="HomeContent" ContentPlaceHolderID="HomeContent" runat="server">
    <script>
        function searchTyres() {
            var cat = cmbCategory.GetText();
            //window.location.href = "/Search?"+cat;
        }

        function initPage() {
            
            //txtZipCode.SetText(cZipCode);
            getLocation();
            showAdvanced();
        }

        function showAdvanced() {
            $(".advancedSearch").toggle();
            $("#advancedIcon").toggleClass("moreIcon");
            $("#advancedIcon").toggleClass("lessIcon");
        }

        function searchTyres() {
            var w = cmbWidth.GetText();
            if (w == "") w = "000";
            var p = cmbProfile.GetText();
            if (p == "") p = "00";

            var size = w+p + cmbDiameter.GetText();
            var type = cmbCategory.GetText();

            window.location.href = "/Tyres/"+type+"/"+size;
        }
    </script>

    <div class="homeText">
        <div class="line1">With you</div>
        <div class="line2">every step</div>
        <div class="line1">on the way!</div>
    </div>


    <div class="searchBox">
        <div class="title">Vehicle Information:</div>
        <div class="box">
            <div class="option opSelected">
                By Size
            </div>
            <div class="option">
                Plate nr.
            </div>

            <div class="search">
                Cover type<br />
                <dx:ASPxComboBox runat="server" ID="cmbCategory" ClientInstanceName="cmbCategory" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                    <DropDownButton ImagePosition="Bottom" >
                        <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px" ></Image>
                    </DropDownButton>
                    
                    
                </dx:ASPxComboBox>

                Tire Sizes<br />
                <div class="floatLt dp1">
                    <dx:ASPxComboBox runat="server" ID="cmbWidth" ClientInstanceName="cmbWidth" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                        <DropDownButton ImagePosition="Bottom">
                            <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                        </DropDownButton>
                    </dx:ASPxComboBox>
                </div>
                <div class="floatLt diagonal">
                </div>
                <div class="floatLt dp2">
                    <dx:ASPxComboBox runat="server" ID="cmbProfile" ClientInstanceName="cmbProfile" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                        <DropDownButton ImagePosition="Bottom">
                            <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                        </DropDownButton>
                    </dx:ASPxComboBox>
                </div>
                <div class="floatLt dp3">
                    <dx:ASPxComboBox runat="server" ID="cmbDiameter" ClientInstanceName="cmbDiameter" CssClass="BoxGeneral" Border-BorderStyle="None" ClearButton-DisplayMode="OnHover">
                        <DropDownButton ImagePosition="Bottom">
                            <Image Url="/Scripts/imgs/flecha_dropdown-01.png" Height="10px" Width="10px"></Image>
                        </DropDownButton>
                    </dx:ASPxComboBox>
                </div>
                <div class="clearfix"></div>
                <div class="advancedSearch">
                    Speed Index:<br />
                    <dx:ASPxTextBox runat="server" ID="txtSpeed" ClientInstanceName="txtSI" CssClass="BoxGeneral">
                        <Border BorderStyle="None" />
                    </dx:ASPxTextBox>    
                </div>
                <div class="advancedSearch">
                    Index load back<br />
                    <dx:ASPxSpinEdit runat="server" ID="spLoadIndex" CssClass="BoxGeneral">
                        <Border BorderStyle="None" />
                    </dx:ASPxSpinEdit>
                </div>
                <div class="clearfix"></div>
                Enter your Zip Code
                <dx:ASPxTextBox runat="server" ID="txtZipCode" ClientInstanceName="txtZipCode" CssClass="BoxGeneral">
                    <Border BorderStyle="None" />
                </dx:ASPxTextBox>
                <div class="clearfix"></div>
                <div class="spacer20"></div>
                <div class="clearfix"></div>
                <div class="advance floatLt" onclick="showAdvanced();">
                    <div class="floatLt">Advanced</div>
                    <div class="floatLt lessIcon" id="advancedIcon"></div>
                </div>
                <div class="searchBtn floatRt" onclick="searchTyres();">
                </div>
                <div class="clearfix"></div>
                <div class="spacer10"></div>
                <div class="clearfix"></div>


            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">




    <div class="clearfix"></div>
    <div class="spacer10"></div>
    <div class="clearfix"></div>
    <div class="homeCol">
        <div class="col-sm-4 colLeft">
            <div class="locateWS"></div>
            <div class="title">Your nearest</div>
            <div class="title">workshop</div>
            <div class="clearfix"></div>
            <div class="spacer20"></div>
            <div class="clearfix"></div>
            Use your <a href="">current location</a> or type
        
        </div>

        <div class="col-sm-4  colCt">
            <div class="promotions"></div>
            <div class="title">
                This Week's Sale
            </div>
            <div class="more">More</div>
        </div>
        <div class="col-sm-4  colRt">
            <div class="faq"></div>
            <div class="title">
                FAQ
            </div>
            <p>Some text here Some text here Some text here</p>
            <p>Some text here Some text here Some text here</p>
            <p>Some text here Some text here Some text here</p>
            <div class="clearfix"></div>
            <div class="spacer20"></div>
            <div class="clearfix"></div>
            <div class="more">More</div>
        </div>
    </div>

    <div class="homeTires">
        <div class="title">
            Tires you can trust
        </div>

        <div class="wrapTires">
            <div class="colTires col-md-3">
                <div class="category cat1">
                    SUMMER
                </div>
                <div class="image">
                    <img src="Scripts/imgs/tyre.png" />
                </div>
                <div class="subTitle">
                    Efficient Grip
                </div>
                <div class="description">
                    Lorem ipsum dolor sit amet, consectetur adipsicing elit. Donex ante velit, faucibus eu vulputate at, elementum eu enim.
                </div>
            </div>
            <div class="colTires col-md-3">
                <div class="category cat2">
                    WINTER
                </div>
                <div class="image">
                    <img src="Scripts/imgs/tyre.png" />
                </div>
                <div class="subTitle">
                    Ultra Grip
                </div>
                <div class="description">
                    Lorem ipsum dolor sit amet, consectetur adipsicing elit. Donex ante velit, faucibus eu vulputate at, elementum eu enim.
                </div>
            </div>
            <div class="colTires col-md-3">
                <div class="category cat3">
                    MOTORCYCLE
                </div>
                <div class="image">
                    <img src="Scripts/imgs/tyre.png" />
                </div>
                <div class="subTitle">
                    Efficient Grip
                </div>
                <div class="description">
                    Lorem ipsum dolor sit amet, consectetur adipsicing elit. Donex ante velit, faucibus eu vulputate at, elementum eu enim.
                </div>
            </div>
            <div class="colTires col-md-3">
                <div class="category cat4">
                    ATV
                </div>
                <div class="image">
                    <img src="Scripts/imgs/tyre.png" />
                </div>
                <div class="subTitle">
                    Ultra Grip
                </div>
                <div class="description">
                    Lorem ipsum dolor sit amet, consectetur adipsicing elit. Donex ante velit, faucibus eu vulputate at, elementum eu enim.
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="homeContact">
        <div class="colHC1 col-md-6">
            <div class="fontRed">
                Plaza los geranios Av.Diana 406,<br />
                Local 1 Col. Delicias, Cuernavaca
            </div>
            <div class="clearfix"></div>
            <div class="spacer20"></div>
            <div class="clearfix"></div>
            Tel: (777) 316/3636 y 316/6591<br />
            infomdm@dienomdm.com
            <div class="clearfix"></div>
            <div class="spacer20"></div>
            <div class="clearfix"></div>
            <div class="iconSocial">
                <a href="/" title="Instagram">
                    <img src="/Scripts/imgs/instagramRed.png" /></a>
            </div>
            <div class="iconSocial">
                <a href="/" title="facebook">
                    <img src="/Scripts/imgs/fbRed.png" /></a>
            </div>

        </div>

        <div class="colHC2 col-md-6">
            <div class="divContact">
            </div>
        </div>

    </div>

    <div class="BoxGeneral" style="position: absolute; top: 100%; width: 100%; padding: 20px; display: none">
        <div class="floatLt">
            SORT BY:
        </div>
        <div class="floatLt" style="padding-right: 15px;">
            <dx:ASPxDropDownEdit runat="server" ID="cmbSort" ClientInstanceName="cmbSort" CssClass=""></dx:ASPxDropDownEdit>
        </div>
        <div class="floatLt">
            BRAND:
        </div>
        <div class="">
            <dx:ASPxDropDownEdit runat="server" ID="cmbBrand" ClientInstanceName="cmbBrand" CssClass=""></dx:ASPxDropDownEdit>
        </div>

        <div class="clearfix"></div>
        <div class="spacer20"></div>
        <div class="clearfix"></div>

        <div class="col-lg-1 visible-lg"></div>
        <div class="col-lg-10 col-md-12">
            
        </div>


    </div>


</asp:Content>
