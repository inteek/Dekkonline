<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="DekkOnline.Admin.Categories" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Conten1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        var saved = false;

        function initPage() {
            $(".editRow").attr("title", "Edit Row");
            $("#lblDCCount1").html("Dekkpro database (" + $("#txtCC1").val() + ")");
            $("#lblDCCount2").html("My database (" + $("#txtCC2").val()+ ")");
        }

        function editRow(s, e) {
            var id = s.GetRowKey(e.visibleIndex);
            txtCdpId.SetText(id);
            saved = false;
            popCategories.PerformCallback("Load|" + id);
        }


        function initEdit(s, e) {
            if (!saved) {
                txtImgUrl.SetText(s.cpImage);
                var img = txtImgUrl.GetText();
                if (img != "") {
                    document.getElementById("uploadedImage").src = img;
                    setElementVisible("uploadedImage", true);
                }
                popCategories.Show();
            } else {
                txtImgUrl.SetText("");
                txtCdpId.SetText("");
                popCategories.Hide();
                xcpCategories.PerformCallback();
            }
        }

        function onUploadControlFileUploadComplete(s, e) {

            txtImgUrl.SetText('');
            if (e.isValid) {
                document.getElementById("uploadedImage").src = e.callbackData;
                txtImgUrl.SetText(e.callbackData);
            }

            setElementVisible("uploadedImage", e.isValid);
        }

        function onImageLoad() {
            var externalDropZone = document.getElementById("externalDropZone");
            var uploadedImage = document.getElementById("uploadedImage");
            uploadedImage.style.left = (externalDropZone.clientWidth - uploadedImage.width) / 2 + "px";
            uploadedImage.style.top = (externalDropZone.clientHeight - uploadedImage.height) / 2 + "px";
        }

        function setElementVisible(elementId, visible) {
            document.getElementById(elementId).className = visible ? "" : "hidden";
        }


        function saveCategories() {
            if (ASPxClientEdit.ValidateGroup('Save')) {
                saved = true;
                var id = txtCdpId.GetText();
                popCategories.PerformCallback("Save|" + id);
            }
        }

        function copyData() {
            var id = txtCdpId.GetText();
            popCategories.PerformCallback("Copy|" + id);
        }

        function changeStatus(id) {
            xcaStatus.PerformCallback(id);
        }

        function updateCounts(s, e) {
            xgvCategories1.Refresh();
            xgvCategories2.Refresh();
            $("#lblDCCount1").html("Dekkpro database (" +s.cpC1 + ")");
            $("#lblDCCount2").html("My database (" + s.cpC2 + ")");
        }

        function notifyChanges(s, e) {
            if (s.cpC1 != "") {
                notify("Categories synced: " + s.cpC1 + "<br/>" + "Categories added: " + s.cpC2, 1, 10, "#divNotify");
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="hidden">
        <asp:TextBox runat="server" ID="txtCC1" ClientIDMode="Static"></asp:TextBox>
        <asp:TextBox runat="server" ID="txtCC2" ClientIDMode="Static"></asp:TextBox>
    </div>
    <div class="floatLt pad20">
        <h4>Categories</h4>
        <dx:ASPxTextBox runat="server" ID="txtImgUrl" ClientInstanceName="txtImgUrl" CssClass="hidden"></dx:ASPxTextBox>
        <dx:ASPxTextBox runat="server" ID="txtCdpId" ClientInstanceName="txtCdpId" CssClass="hidden"></dx:ASPxTextBox>
    </div>
    <div id="divNotify" class="floatRt pad20" style="padding-top:30px">
        <input type="button" value="Sync Database Now" class="btnNoBorder" onclick="xcpCategories.PerformCallback('Update');" />
        
    </div>
    <div class="clearfix"></div>
    <div>
        <dx:ASPxCallbackPanel runat="server" ID="xcpCategories" ClientInstanceName="xcpCategories" OnCallback="xcpCategories_Callback" Width="100%" HideContentOnCallback="true">
            <PanelCollection>
                <dx:PanelContent>

                    <div class="col-md-6">
                        <div class="pad10">
                            <h3 id="lblDCCount1" class="fontRed">Dekkpro Categories </h3>
                            <dx:ASPxGridView ID="xgvCategories1" ClientInstanceName="xgvCategories1" runat="server" Width="100%" Settings-ShowFilterRow="true" SettingsPager-PageSize="50" Settings-AutoFilterCondition="Contains" SettingsBehavior-AllowFocusedRow="true" KeyFieldName="cdpId" DataSourceID="lnqCategories1">
                                <Styles>
                                    <Header VerticalAlign="Top" BackColor="#f5f5f5" HorizontalAlign="Center" Paddings-PaddingTop="3px" Font-Bold="true" Font-Size="1em"></Header>
                                    <FilterRow BackColor="#f5f5f5"></FilterRow>
                                    <Row BackColor="#f5f5f5" Cursor="pointer" CssClass="editRow"></Row>
                                    <FocusedRow BackColor="#333e49" ForeColor="White"></FocusedRow>
                                </Styles>
                                <SettingsPager Position="TopAndBottom"></SettingsPager>
                                <Columns>
                                    <dx:GridViewDataColumn>
                                        <DataItemTemplate>
                                            <img style="width: 70px" src="<%# Eval("cdpImage").ToString() == "" ? "/photos/noPhoto.png" :  Eval("cdpImage") %>" alt="<%# Eval("cdpName") %>" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="cdpName" Caption="Name" SortIndex="0"></dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="cdpDescription" Caption=" " Width="70">
                                        <FilterTemplate></FilterTemplate>
                                        <DataItemTemplate>
                                            <button type="button" class="btn btn-success floatRt" onclick="changeStatus('<%# Eval("cdpId") %>')" title="Move to My DataBase">
                                                <i class="glyphicon glyphicon-arrow-right "></i>
                                            </button>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                </Columns>
                                <ClientSideEvents RowDblClick="editRow"  />
                            </dx:ASPxGridView>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="pad10">
                            <h3 id="lblDCCount2" class="fontRed">Active Categories</h3>
                            <dx:ASPxGridView ID="xgvCategories2" ClientInstanceName="xgvCategories2" runat="server" Width="100%" Settings-ShowFilterRow="true" SettingsPager-PageSize="50" Settings-AutoFilterCondition="Contains" SettingsBehavior-AllowFocusedRow="true" KeyFieldName="cdpId" DataSourceID="lnqCategories2">
                                <Styles>
                                    <Header VerticalAlign="Top" BackColor="#f5f5f5" HorizontalAlign="Center" Paddings-PaddingTop="3px" Font-Bold="true" Font-Size="1em"></Header>
                                    <FilterRow BackColor="#f5f5f5"></FilterRow>
                                    <Row BackColor="#f5f5f5" Cursor="pointer" CssClass="editRow"></Row>
                                    <FocusedRow BackColor="#333e49" ForeColor="White"></FocusedRow>
                                </Styles>
                                <SettingsPager Position="TopAndBottom"></SettingsPager>
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="cdpDescription" Caption=" " Width="70">
                                        <FilterTemplate></FilterTemplate>
                                        <DataItemTemplate>
                                            <button type="button" class="btn btn-warning floatLt" onclick="changeStatus('<%# Eval("cdpId") %>')" title="Remove from My DataBase">
                                                <i class="glyphicon glyphicon-arrow-left "></i>
                                            </button>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn>
                                        <DataItemTemplate>
                                            <img style="width: 70px" src="<%# Eval("cdpImage").ToString() == "" ? "/photos/noPhoto.png" :  Eval("cdpImage") %>" alt="<%# Eval("cdpName") %>" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="cdpName" Caption="Name" SortIndex="0"></dx:GridViewDataColumn>

                                </Columns>
                                <ClientSideEvents RowDblClick="editRow" />
                            </dx:ASPxGridView>
                        </div>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="notifyChanges" />
        </dx:ASPxCallbackPanel>
    </div>

    <dx:ASPxPopupControl runat="server" ID="popCategories" ClientInstanceName="popCategories" Width="1000px" OnWindowCallback="popCategories_WindowCallback"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" HeaderText="Edit Categories" Modal="true" AllowDragging="true">
        <ContentCollection>
            <dx:PopupControlContentControl>


                <div class="clearfix"></div>
                <div class="col-md-3 ">
                    <div class="panel-group" id="accordion">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" href="#collapse1">MAIN IMAGE&nbsp;&nbsp;
                            <img src="../Scripts/imgs/icons/exp.png" /></a>

                                </h4>
                            </div>
                            <div id="collapse1" class="panel-collapse collapse in ">
                                <div class="panel-body">
                                    <div class="hidden">
                                    </div>
                                    <div id="externalDropZone" class="hovereffect dropZoneExternal">
                                        <img id="uploadedImage" src="#" style="width: 100%;" class="hidden" alt="" onload="onImageLoad()" />
                                        <div class="overlay">
                                            <h3>PICK IMAGE</h3>
                                        </div>
                                    </div>





                                    <div class="hidden">
                                        <dx:ASPxUploadControl ID="UploadControl" ClientInstanceName="UploadControl" runat="server" UploadMode="Auto" AutoStartUpload="True" Width="350"
                                            ShowProgressPanel="True" CssClass="uploadControl" DialogTriggerID="externalDropZone" OnFileUploadComplete="UploadControl_FileUploadComplete">
                                            <AdvancedModeSettings EnableDragAndDrop="True" EnableFileList="False" EnableMultiSelect="False" ExternalDropZoneID="externalDropZone" DropZoneText="" />
                                            <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage" />
                                            <BrowseButton Text="Select an image for upload..." />
                                            <DropZoneStyle CssClass="uploadControlDropZone" />
                                            <ProgressBarStyle CssClass="uploadControlProgressBar" />
                                            <ClientSideEvents
                                                DropZoneEnter="function(s, e) { if(e.dropZone.id == 'externalDropZone') setElementVisible('dropZone', true); }"
                                                DropZoneLeave="function(s, e) { if(e.dropZone.id == 'externalDropZone') setElementVisible('dropZone', false); }"
                                                FileUploadComplete="onUploadControlFileUploadComplete"></ClientSideEvents>
                                        </dx:ASPxUploadControl>
                                    </div>
                                    <%--<dx:ASPxBinaryImage ID="imgArticle"  CssClass="BoxGeneral" 
                             ShowLoadingImage="true" AlternateText="Pick Image"  runat="server">     
                             <EditingSettings Enabled="true" DropZoneText="Drop Image" EmptyValueText="Pick Image" > 
                                        <UploadSettings UploadMode="Auto" TemporaryFolder="~\ImagesTmp">
                                            <UploadValidationSettings MaxFileSize="4194304"></UploadValidationSettings>
                                        </UploadSettings>
                                        <ButtonPanelSettings Position="Bottom" Visibility="OnMouseOver" ButtonPosition="Center" />
                                    </EditingSettings>
                                </dx:ASPxBinaryImage>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-xs-12  col-md-9">
                    <dx:ASPxTextBox runat="server" NullText="Category" ID="txtCategories" onkeydown="return onlyAN(this,event);" CssClass="tableInput BoxGeneral fontRed" Font-Size="X-Large" ForeColor="#dd1730">
                        <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                            <RequiredField IsRequired="true" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>

                    <div class="col-sm-6" style="border-right: 1px solid #865459">
                        <h3><span class="fontDarkGray">Dekkpro Data</span></h3>
                        <div class="clearfix"></div>
                        <div class="spacer20"></div>
                        <div class="clearfix"></div>
                        <b>Description:</b><br />
                        <dx:ASPxTextBox runat="server" ID="txtDescriptionDP" CssClass="BoxGeneral" ReadOnly="true"></dx:ASPxTextBox>
                        <div class="clearfix"></div>
                        <div class="spacer20"></div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="col-sm-6">
                        <div class="floatLt">
                            <h3><span class="fontDarkGray">My Data</span></h3>
                        </div>
                        <div class="floatRt" style="margin-top: 15px">
                            <input type="button" class="btnNoBorder" value="Copy Dekkpro Data" onclick="copyData();" />
                        </div>
                        <div class="clearfix"></div>
                        <div class="spacer20"></div>
                        <div class="clearfix"></div>
                        <b>Description:</b><br />
                        <dx:ASPxTextBox runat="server" ID="txtDescription" CssClass="BoxGeneral">
                        </dx:ASPxTextBox>
                        <div class="clearfix"></div>
                        <div class="spacer20"></div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="spacer10"></div>
                    <div class="clearfix"></div>
                    <div class="floatRt pad5">
                        <input type="button" class="btnBorder" value="SAVE" onclick="saveCategories();" />
                    </div>

                </div>

            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents EndCallback="initEdit" />
    </dx:ASPxPopupControl>


    <dx:ASPxCallback runat="server" ID="xcaStatus" ClientInstanceName="xcaStatus" OnCallback="xcaStatus_Callback">
        <ClientSideEvents EndCallback="updateCounts" />
    </dx:ASPxCallback>
    <asp:LinqDataSource ID="lnqCategories1" runat="server" ContextTypeName="DekkOnline.dbDekkOnlineDataContext" TableName="categoriesDPs" Where="cdpStatus = false"></asp:LinqDataSource>
    <asp:LinqDataSource ID="lnqCategories2" runat="server" ContextTypeName="DekkOnline.dbDekkOnlineDataContext" TableName="categoriesDPs" Where="cdpStatus = true"></asp:LinqDataSource>

</asp:Content>
