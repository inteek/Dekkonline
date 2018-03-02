<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Brands.aspx.cs" Inherits="DekkOnline.Admin.Brands" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Conten1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        var saved = false;

        function initPage() {
            selectRow(xgvBrands, null);
            $(".editRow").attr("title", "Edit Row");
        }

        function editRow(s, e) {
            var id = xgvBrands.GetRowKey(e.visibleIndex);
            txtBraId.SetText(id);
            saved = false;
            popBrands.PerformCallback("Load|" + id);
        }


        function initEdit(s, e) {
            if (!saved) {
                txtImgUrl.SetText(s.cpImage);
                var img = txtImgUrl.GetText();
                if (img != "") {
                    document.getElementById("uploadedImage").src = img;
                    setElementVisible("uploadedImage", true);
                }
                popBrands.Show();
            } else {
                txtImgUrl.SetText("");
                txtBraId.SetText("");
                popBrands.Hide();
                xcpBrands.PerformCallback();
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


        function saveBrands() {
            if (ASPxClientEdit.ValidateGroup('Save')) {
                saved = true;
                var id = txtBraId.GetText();
                popBrands.PerformCallback("Save|" + id);
            }
        }

        function copyData() {
            var id = txtBraId.GetText();
            popBrands.PerformCallback("Copy|" + id);
        }

        function notifyChanges(s, e) {
            if (s.cpC1 != "") {
                notify("Brands synced: " + s.cpC1 + "<br/>" + "Brands added: " + s.cpC2, 1, 10, "#divNotify");
            }
        }


        function selectRow(s, e) {
            
            if (s.GetSelectedRowCount() > 1) {
                spPricePM.SetValue(0);

                $(".editRows").show(100);

            } else {
                $(".editRows").hide(100);

            }
        }

        function addDiscount() {
            
            popDiscount.Show();
        }

        function saveSection() {
            popDiscount.PerformCallback();
        }

        function changeSectionEnd(s, e) {
            popDiscount.Hide();
            xcpBrands.PerformCallback();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="floatLt">
        <h4>Brands</h4>
        <dx:ASPxTextBox runat="server" ID="txtImgUrl" ClientInstanceName="txtImgUrl" CssClass="hidden"></dx:ASPxTextBox>
        <dx:ASPxTextBox runat="server" ID="txtBraId" ClientInstanceName="txtBraId" CssClass="hidden"></dx:ASPxTextBox>
    </div>
    <div id="divNotify" class="floatRt pad20">
        <input type="button" value="Sync Database Now" class="btnNoBorder" onclick="xcpBrands.PerformCallback('Update');" />

    </div>
    <div class="floatRt pad20">
        <button type="button" class="btnBorder editRows" onclick="addDiscount()">Add Discount</button>
    </div>

    <div class="clearfix"></div>
    <%--    <div class="pad20 floatLt">
        <dx:ASPxComboBox runat="server" ID="cmbBrands" TextField="catName" ValueField="catNumber" NullText="Pick Categorie" DataSourceID="lnqBrands">
            <ClientSideEvents ValueChanged="function(s,e){xcpBrands.PerformCallback();}" />
        </dx:ASPxComboBox>
    </div>--%>

    <dx:ASPxCallbackPanel runat="server" ID="xcpBrands" ClientInstanceName="xcpBrands" OnCallback="xcpBrands_Callback" Width="100%" HideContentOnCallback="true">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="xgvBrands" ClientInstanceName="xgvBrands" runat="server" Width="100%" Settings-ShowFilterRow="true" SettingsPager-PageSize="50" Settings-AutoFilterCondition="Contains" SettingsBehavior-AllowFocusedRow="true" KeyFieldName="braId" DataSourceID="lnqBrands">
                    <Styles>
                         <Header VerticalAlign="Top" BackColor="#f5f5f5" HorizontalAlign="Center" Paddings-PaddingTop="3px" Font-Bold="true" Font-Size="1em"></Header>
                        <FilterRow BackColor="#f5f5f5"></FilterRow>
                        <Row BackColor="#f5f5f5" Cursor="pointer" CssClass="editRow"></Row>
                        <FocusedRow BackColor="#333e49" ForeColor="White"></FocusedRow>
                        <SelectedRow BackColor="White" ForeColor="Black"></SelectedRow>
                    </Styles>
                    <SettingsPager Position="TopAndBottom"></SettingsPager>
                    <Columns>
                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" SelectAllCheckboxMode="AllPages">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataColumn>
                            <DataItemTemplate>
                                <img style="max-height:30px; width:auto;" src="<%# Eval("braImage").ToString() == "" ? "/photos/noPhoto.png" :  Eval("braImage") %>" alt="<%# Eval("braName") %>" />
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="braName" Caption="Name" SortIndex="0"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="braCode" Caption="Code"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="braPercent" Caption="Discount(%)"></dx:GridViewDataColumn>
                    </Columns>
                    <ClientSideEvents RowDblClick="editRow"  SelectionChanged="selectRow"/>
                </dx:ASPxGridView>
            </dx:PanelContent>
        </PanelCollection>
         <ClientSideEvents EndCallback="notifyChanges" />
    </dx:ASPxCallbackPanel>

    <dx:ASPxPopupControl runat="server" ID="popBrands" ClientInstanceName="popBrands" Width="1000px" OnWindowCallback="popBrands_WindowCallback"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" HeaderText="Edit Brands" Modal="true" AllowDragging="true">
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
                    <dx:ASPxTextBox runat="server" NullText="Category" ID="txtBrands" onkeydown="return onlyAN(this,event);" CssClass="tableInput BoxGeneral fontRed" Font-Size="X-Large" ForeColor="#dd1730">
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
                        <b>Code:</b><br />
                        <dx:ASPxTextBox runat="server" ID="txtCodeDP" CssClass="BoxGeneral" ReadOnly="true"></dx:ASPxTextBox>
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
                        <b>Code:</b><br />
                        <dx:ASPxTextBox runat="server" ID="txtCode" CssClass="BoxGeneral">
                        </dx:ASPxTextBox>
                        <div class="clearfix"></div>
                        <div class="spacer20"></div>
                        <div class="clearfix"></div>
                        <b>Discount(%)</b><br />
                        <dx:ASPxSpinEdit runat="server" ID="spPriceP" NumberType="Integer" DisplayFormatString="{0:0'%'}" CssClass="BoxGeneral" MaxValue="100" MinValue="0" Increment="1"></dx:ASPxSpinEdit>
                        <div class="clearfix"></div>
                        <div class="spacer20"></div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="spacer10"></div>
                    <div class="clearfix"></div>
                    <div class="floatRt pad5">
                        <input type="button" class="btnBorder" value="SAVE" onclick="saveBrands();" />
                    </div>

                </div>

            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents EndCallback="initEdit" />
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl runat="server" ID="popSection" ClientInstanceName="popDiscount" OnWindowCallback="popSection_WindowCallback" Width="320px"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" HeaderText="Edit Selected Rows" Modal="true" AllowDragging="true">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <h4>ADD DISCOUNT(%)</h4>
                <dx:ASPxSpinEdit runat="server" ID="spPricePM" ClientInstanceName="spPricePM" NumberType="Integer" DisplayFormatString="{0:0'%'}" CssClass="BoxGeneral" MaxValue="100" MinValue="0" Increment="1"></dx:ASPxSpinEdit>
                <div class="clearfix"></div>
                <div class="spacer20"></div>
                <div class="clearfix"></div>
                <div class="floatRt pad5">
                    <input type="button" class="btnBorder" value="SAVE" onclick="saveSection();" />
                </div>
                
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents EndCallback="changeSectionEnd" />
    </dx:ASPxPopupControl>

    <asp:LinqDataSource ID="lnqBrands" runat="server" ContextTypeName="DekkOnline.dbDekkOnlineDataContext" TableName="brands" OnSelecting="lnqBrands_Selecting"></asp:LinqDataSource>

</asp:Content>
