﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ourCategories.aspx.cs" Inherits="DekkOnline.Admin.ourCategories" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Conten1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var saved = false;

        function initPage() {
            $(".editRow").attr("title", "Edit Row");
        }

        function editRow(s, e) {
            var id = s.GetRowKey(e.visibleIndex);
            txtCatId.SetText(id);
            saved = false;
            popCategories.PerformCallback("Load|" + id);
        }

        function newRow() {
            var id = 0;
            txtCatId.SetText(id);
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
                txtCatId.SetText("");
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
                var id = txtCatId.GetText();
                popCategories.PerformCallback("Save|" + id);
            }
        }

        function copyData() {
            var id = txtCatId.GetText();
            popCategories.PerformCallback("Copy|" + id);
        }

        function changeStatus(id) {
            xcaStatus.PerformCallback(id);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="floatLt pad20">
        <h4>Sections</h4>
        <dx:ASPxTextBox runat="server" ID="txtImgUrl" ClientInstanceName="txtImgUrl" CssClass="hidden"></dx:ASPxTextBox>
        <dx:ASPxTextBox runat="server" ID="txtCatId" ClientInstanceName="txtCatId" CssClass="hidden"></dx:ASPxTextBox>
    </div>
    
    <div class="clearfix"></div>
    <div >
        <dx:ASPxCallbackPanel runat="server" ID="xcpCategories" ClientInstanceName="xcpCategories" OnCallback="xcpCategories_Callback" Width="100%">
            <PanelCollection>
                <dx:PanelContent>
                    <div class="col-md-12">
                        <div class="pad10">
                            <dx:ASPxGridView ID="xgvCategories" ClientInstanceName="xgvCategories" runat="server" Width="100%" Settings-ShowFilterRow="true" SettingsPager-PageSize="50" Settings-AutoFilterCondition="Contains" SettingsBehavior-AllowFocusedRow="true" KeyFieldName="catId" DataSourceID="lnqCategories1">
                                <Styles>
                                    <Header VerticalAlign="Top" BackColor="#f5f5f5" HorizontalAlign="Center" Paddings-PaddingTop="3px" Font-Bold="true" Font-Size="1em"></Header>
                                    <FilterRow BackColor="#f5f5f5"></FilterRow>
                                    <Row BackColor="#f5f5f5" Cursor="pointer" CssClass="editRow"></Row>
                                    <FocusedRow BackColor="#333e49" ForeColor="White"></FocusedRow>
                                </Styles>
                                <SettingsPager Position="TopAndBottom"></SettingsPager>
                                <Columns>
                                    <dx:GridViewDataColumn Width="100">
                                         <HeaderTemplate>

                                            <button type="button" class="btn btn-success floatLt" onclick="newRow()" title="Add New">
                                                <i class="glyphicon glyphicon-plus-sign"></i>
                                            </button>
                                        </HeaderTemplate>
                                        <DataItemTemplate>
                                            <img style="width: 70px" src="<%# Eval("catImage").ToString() == "" ? "/photos/noPhoto.png" :  Eval("catImage") %>" alt="<%# Eval("catName") %>" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="catName" Caption="Name" SortIndex="0"></dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="catDescription" Caption="Status " Width="70">
                                        <FilterTemplate></FilterTemplate>
                                       
                                        <DataItemTemplate>
                                            <div class="BoxGeneral text-center">
                                            <button type="button" class="btn btn-<%# (bool)Eval("catStatus")==true?"success":"danger" %>" onclick="changeStatus('<%# Eval("catId") %>')" title="Change Status">
                                                <i class="glyphicon glyphicon-<%# (bool)Eval("catStatus")==true?"check":"minus-sign" %>"></i>
                                            </button>
                                            </div>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                </Columns>
                                <ClientSideEvents RowDblClick="editRow" />
                            </dx:ASPxGridView>
                        </div>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
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
                    <div class="col-sm-12">
                        
                        <div class="clearfix"></div>
                        <div class="spacer20"></div>
                        <div class="clearfix"></div>
                        <b>Description:</b><br />
                        <dx:ASPxMemo runat="server" ID="txtDescription" CssClass="BoxGeneral" Height="50px"></dx:ASPxMemo>
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
        <ClientSideEvents EndCallback="function(s,e){xgvCategories.Refresh(); xgvCategories2.Refresh();}" />
    </dx:ASPxCallback>
    <asp:LinqDataSource ID="lnqCategories1" runat="server" ContextTypeName="DekkOnline.dbDekkOnlineDataContext" TableName="categories"></asp:LinqDataSource>
    

</asp:Content>
