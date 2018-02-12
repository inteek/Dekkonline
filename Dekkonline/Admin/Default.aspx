<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DekkOnline.Admin.Default" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript">
      function ShowLoginWindow(s, e) {
          var id = xgvOrders.GetRowKey(e.visibleIndex);
          pcLogin.PerformCallback("Load|" + id, function () {
              pcLogin.Show();
              $("#lblOrder").text(id);
          });

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="col-md-1  hidden-xs hidden-sm"></div>

    
    <div class="col-xs-10 col-md-10">
        <div class="tableTitle subTitle">
            <div class="pad5 floatLt">
                <div style="padding: 10px;">ORDERS</div>
            </div>

        </div>
        <div class="gridBox floatLt BoxGeneral">

            <dx:ASPxGridView runat="server" ID="xgvOrders" ClientInstanceName="xgvOrders" KeyFieldName="IdOrder" Width="100%"
                SettingsPager-PageSize="5" Border-BorderStyle="None" AutoGenerateColumns="false" Settings-GridLines="None"  >
                <Styles>
                    <Header CssClass="subTitle" HorizontalAlign="Center"></Header>
                    <AlternatingRow BackColor="WhiteSmoke"></AlternatingRow>
                    <Table CssClass="tableGeneral"></Table>
                    <PagerBottomPanel BackColor="White" ForeColor="#dd1730">
                    </PagerBottomPanel>
                </Styles>
                <StylesPager PageNumber-HorizontalAlign="Right">
                    <CurrentPageNumber Font-Bold="true"></CurrentPageNumber>
                    <PageNumber ForeColor="#dd1730">
                    </PageNumber>
                </StylesPager>
                <SettingsPager Visible="true"></SettingsPager>
                <Columns>
                    <dx:GridViewDataColumn Caption="Order" FieldName="IdOrder" CellStyle-HorizontalAlign="Center" ></dx:GridViewDataColumn> 
                    <dx:GridViewDataDateColumn Caption="Date" FieldName="DateOrder" CellStyle-HorizontalAlign="Center" PropertiesDateEdit-DisplayFormatString="{0:dd/MM/yy HH:mm}"></dx:GridViewDataDateColumn>
                    <dx:GridViewDataColumn Caption="Products" FieldName="Products" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn Caption="CodeProm" FieldName="CodeProm" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn Caption="Total" FieldName="Total" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn Caption="IdUser" FieldName="IdUser" Visible="false" CellStyle-HorizontalAlign="Center" ></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn Caption="Customer" FieldName="NameUser" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn Caption="Email" FieldName="EmailUser" CellStyle-HorizontalAlign="Center" ></dx:GridViewDataColumn>
                </Columns>
                <ClientSideEvents RowDblClick="ShowLoginWindow" />
            </dx:ASPxGridView>
            <div class="clearfix"></div>
            <div style="height: 8px"></div>
        </div>
    </div>
    
    
    <div class="clearfix"></div>
    <div class="spacer20"></div>
    <div class="clearfix"></div>

    <div class="col-md-1  hidden-xs hidden-sm"></div>

    <div class="col-xs-12 col-sm-5 col-md-4 ">
        <div class="tableTitle subTitle">
            <div class="pad5 floatLt">
                <div style="padding: 10px;">TOP CLIENTS</div>

            </div>
            <div class="floatRt pad5">
                <input type="button" class="btnAdd" onclick="addMenu();" />
            </div>
        </div>

        <div class="gridBox">
            <dx:ASPxCallbackPanel runat="server" ID="xcpMenu" ClientInstanceName="xcpMenu" Width="100%">
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxGridView runat="server" ID="xgvTopMenu"  Width="100%"
                            SettingsPager-PageSize="5" Border-BorderStyle="None" AutoGenerateColumns="false" Settings-GridLines="None">
                            <Styles>
                                <Header CssClass="subTitle hidden" HorizontalAlign="Center"></Header>
                                <AlternatingRow BackColor="WhiteSmoke"></AlternatingRow>
                                <Table CssClass="tableGeneral"></Table>
                                <PagerBottomPanel BackColor="White" ForeColor="#dd1730">
                                </PagerBottomPanel>
                            </Styles>
                            <StylesPager PageNumber-HorizontalAlign="Right">
                                <CurrentPageNumber Font-Bold="true"></CurrentPageNumber>
                                <PageNumber ForeColor="#dd1730">
                                </PageNumber>
                            </StylesPager>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dx:GridViewDataColumn Width="50">
                                    <DataItemTemplate>
                                        <div class="tableButtons">
                                            <div title="Move Up" class="btnMoveUp" onclick="upMenu(<%# Eval("id") %>)"></div>
                                            <div title="Move Down" class="btnMoveDown" onclick="downMenu(<%# Eval("id") %>)"></div>
                                        </div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn>
                                    <DataItemTemplate>
                                        <div title="Edit Main Site" onclick="editMenu(<%# Eval("id") %>);" class="tableInput"><%# Eval("menuTextNO") %></div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Width="50">
                                    <DataItemTemplate>
                                        <div class="floatRt ">
                                            <div class="btnEdit" onclick="editMenu(<%# Eval("id") %>);"></div>
                                        </div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Width="50">
                                    <DataItemTemplate>
                                        <div class="floatRt ">
                                            <div class="btnTrash"></div>
                                        </div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                            </Columns>
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

        </div>

    </div>

    <div class="col-xs-12 col-sm-7 col-md-6 ">

        <div class="tableTitle subTitle">
            <div class="pad5 floatLt">
                <div style="padding: 10px;">LATEST PRODUCTS</div>

            </div>
            <div class="floatRt pad5">
                <%--<input type="button" class="btnAdd" data-toggle="modal" data-target="#popAddArticle" />--%>
                <input type="button" class="btnAdd" onclick="addArticle()" />

            </div>
        </div>
        <div class="gridBox">
            <dx:ASPxCallbackPanel runat="server" ID="xcpArticle" ClientInstanceName="xcpArticle" Width="100%">
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxGridView runat="server" ID="xgvLastArticles" Width="100%" EnableTheming="true" Visible="false"
                            SettingsPager-PageSize="5" Border-BorderStyle="None" AutoGenerateColumns="false" Settings-GridLines="None">
                            <Styles>
                                <Header CssClass="subTitle" ForeColor="#870412" BackColor="Transparent" Border-BorderStyle="None" HorizontalAlign="Center"></Header>
                                <AlternatingRow BackColor="WhiteSmoke"></AlternatingRow>
                                <Table CssClass="tableGeneral"></Table>
                                <PagerBottomPanel BackColor="White" ForeColor="#dd1730">
                                </PagerBottomPanel>
                            </Styles>
                            <StylesPager PageNumber-HorizontalAlign="Right">
                                <CurrentPageNumber Font-Bold="true"></CurrentPageNumber>
                                <PageNumber ForeColor="#dd1730">
                                </PageNumber>
                            </StylesPager>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dx:GridViewDataColumn Width="80">
                                    <DataItemTemplate>
                                        <div onclick="goTo('editPost.aspx?id=<%# Eval("id") %>')">
                                            <div id="divImage<%# Eval("id") %>" class="btnImage <%# Eval("img").ToString() == "" ? "hidden" : "" %>"
                                                onmouseover="changePopImage('divImage<%# Eval("id") %>','<%# Eval("img") %>')">
                                            </div>
                                            <div class="btnNoImage <%# Eval("img").ToString() == "" ? "" : "hidden" %>"></div>
                                        </div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="ARTICLE">
                                    <DataItemTemplate>
                                        <div onclick="goTo('editPost.aspx?id=<%# Eval("id") %>')" class="tableInput"><%# Eval("txtTitle") %></div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="POSTED">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <CellStyle CssClass="visible-lg"></CellStyle>
                                    <DataItemTemplate>
                                        <div onclick="goTo('editPost.aspx?id=<%# Eval("id") %>')" class="tableInput"><%# Eval("textdate","{0:dd/MM/yy}") %></div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="EDITED">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <CellStyle CssClass="visible-lg"></CellStyle>
                                    <DataItemTemplate>
                                        <div onclick="goTo('editPost.aspx?id=<%# Eval("id") %>')" class="tableInput"><%# Eval("textedit","{0:dd/MM/yy}") %></div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="COMMENTS">
                                    <HeaderStyle CssClass="hidden-xs" />
                                    <CellStyle CssClass="hidden-xs" HorizontalAlign="Center"></CellStyle>
                                    <DataItemTemplate>
                                      0
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption=" ">
                                    <DataItemTemplate>
                                        <div class="tableInput floatRt" onclick="goTo('editPost.aspx?id=<%# Eval("id") %>')"> 
                                            0
                                        </div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>

                            </Columns>
                        </dx:ASPxGridView>


                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>

    </div>

    <div class="clearfix"></div>
    <div class="spacer20"></div>
    <div class="clearfix"></div>

    <div class="col-md-1  hidden-xs hidden-sm"></div>



    <div class="col-sm-6 col-md-3 ">
        <div class="tableTitle subTitle">
            <div class="pad5 floatLt">
                <div style="padding: 10px;">SITE INFO</div>

            </div>

        </div>
        <div class="gridBox floatLt " style="width: 50%">
            <div class="subTitle fontDarkGray paddingLeft10">VIEWS</div>
            <div class="BoxGeneral text-center fontGray fontSize4-0">
                <asp:Label runat="server" ID="lblVisits" Text="40"></asp:Label>
            </div>
        </div>
        <div class="gridBox floatLt" style="width: 50%">
            <div class="subTitle fontDarkGray paddingLeft10">COMMENTS</div>
            <div class="BoxGeneral text-center fontGray fontSize4-0 link">
                <asp:Label runat="server" ID="lblComments" Text="20"></asp:Label>
            </div>
        </div>
    </div>

    <div class="col-sm-6 col-md-4 ">
        <div class="tableTitle subTitle">
            <div class="pad5 floatLt">
                <div style="padding: 10px;">INQUERIES</div>

            </div>

        </div>
        <div class="gridBox floatLt " style="width: 50%">
            <div class="subTitle fontDarkGray paddingLeft10">HANDLED</div>
            <div class="BoxGeneral text-center fontGray fontSize4-0">
                <asp:Label runat="server" ID="lblH" Text="40"></asp:Label>
                    </div>
        </div>
        <div class="gridBox floatLt" style="width: 50%">
            <div class="subTitle fontDarkGray paddingLeft10">UNHANDELED</div>
            <div class="BoxGeneral text-center fontGray fontSize4-0">
                <asp:Label runat="server" ID="lblUH" Text="20"></asp:Label>
            </div>
        </div>
    </div>

    <div class="visible-xs visible-sm clearfix"></div>
    <div class="visible-xs visible-sm spacer20"></div>

    <div class=" col-md-3 ">
        <div class="tableTitle subTitle">
            <div class="pad5 floatLt">
                <div style="padding: 10px;">LAST ENTRANCE</div>

            </div>

        </div>
        <div class="gridBox floatLt BoxGeneral">

            <dx:ASPxGridView runat="server" ID="ASPxGridView1" Width="100%"
                SettingsPager-PageSize="4" Border-BorderStyle="None" AutoGenerateColumns="false" Settings-GridLines="None">
                <Styles>
                    <Header CssClass="subTitle hidden" HorizontalAlign="Center"></Header>
                    <AlternatingRow BackColor="WhiteSmoke"></AlternatingRow>
                    <Table CssClass="tableGeneral"></Table>
                    <PagerBottomPanel BackColor="White" ForeColor="#dd1730">
                    </PagerBottomPanel>
                </Styles>
                <StylesPager PageNumber-HorizontalAlign="Right">
                    <CurrentPageNumber Font-Bold="true"></CurrentPageNumber>
                    <PageNumber ForeColor="#dd1730">
                    </PageNumber>
                </StylesPager>
                <SettingsPager Visible="false"></SettingsPager>
                <Columns>

                    <dx:GridViewDataColumn Caption="Ip-address" FieldName="ipaddress"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn Caption="User" FieldName="username"></dx:GridViewDataColumn>
                    <dx:GridViewDataDateColumn Caption="Date" FieldName="login_date_time" PropertiesDateEdit-DisplayFormatString="{0:dd/MM/yy HH:mm}">
                    </dx:GridViewDataDateColumn>

                </Columns>
            </dx:ASPxGridView>
            <div class="clearfix"></div>
            <div style="height: 8px"></div>
        </div>
    </div>

    


    <dx:ASPxPopupControl runat="server" ID="popImg" ClientInstanceName="popImg" ShowHeader="false" ShowFooter="false" Width="300px" CloseAction="MouseOut">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <img id="imgPop" style="max-width: 300px; max-height: 300px" />
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <div id="popAddMenu" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header popHead">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    ADD MAIN SITE
                </div>
                <div class="modal-body">
                    <asp:TextBox runat="server" ID="txtMenu" onkeydown="return onlyAN(this,event);" CssClass="tableInput BoxGeneral fontRed" Font-Size="X-Large" ForeColor="#dd1730"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <div class="floatLt">
                        <div class="floatRt pad5">
                            <button type="button" class="btnNoBorder btnSmall" data-dismiss="modal">CANCEL</button>
                        </div>
                    </div>
                    <div class="floatRt pad5">
                        <button type="button" class="btnBorder" data-dismiss="modal" onclick="saveMenu()">PUBLISH</button>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <div class="clear"></div>

    <div id="popAddArticle" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->


        </div>
    </div>



    <dx:ASPxPopupControl ID="pcLogin" ClientInstanceName="pcLogin" runat="server" Width="720"  CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"  OnWindowCallback="popOrder_WindowCallback"
        HeaderText="ORDER DETAILS" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
       
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div class="col-xs-12 col-md-12" style="height:350px">
                    <div class="tableTitle subTitle">
                        <div class="pad5 floatLt">
                            <div style="padding: 10px; font-size:20px">ORDER #<span id="lblOrder"></span></div>
                        </div>
                    </div>
                    <div class="gridBox floatLt BoxGeneral">
                        <dx:ASPxGridView runat="server" ID="xgvOrderDetails" ClientInstanceName="xgvOrderDetails" KeyFieldName="IdProduct" Width="100%"
                            SettingsPager-PageSize="7" Border-BorderStyle="None" AutoGenerateColumns="false" Settings-GridLines="None"  >
                            <Styles>
                                <Header CssClass="subTitle" HorizontalAlign="Center"></Header>
                                <AlternatingRow BackColor="WhiteSmoke"></AlternatingRow>
                                <Table CssClass="tableGeneral"></Table>
                                <PagerBottomPanel BackColor="White" ForeColor="#dd1730">
                                </PagerBottomPanel>
                            </Styles>
                            <StylesPager PageNumber-HorizontalAlign="Right">
                                <CurrentPageNumber Font-Bold="true"></CurrentPageNumber>
                                <PageNumber ForeColor="#dd1730">
                                </PageNumber>
                            </StylesPager>
                            <SettingsPager Visible="true"></SettingsPager>
                            <Columns>
                                <dx:GridViewDataColumn Caption="Id" FieldName="IdProduct" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ></dx:GridViewDataColumn> 
                                <dx:GridViewDataColumn Caption="Nmae" FieldName="Nmae" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ></dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="Brand" FieldName="Brand" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="TyreSize" FieldName="TyreSize" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="Quantity" FieldName="Quantity" Visible="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="Price" FieldName="Price" CellStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"></dx:GridViewDataColumn>
                            </Columns>
                        </dx:ASPxGridView>
                        <div class="clearfix"></div>
                        <div style="height: 8px"></div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="20px" />
        </ContentStyle>

    </dx:ASPxPopupControl>

    <div class="clear"></div>

  
</asp:Content>
