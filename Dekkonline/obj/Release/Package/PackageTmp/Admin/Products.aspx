<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="DekkOnline.Admin.Products" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Conten1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        var saved = false;

        function initPage() {
            $(".editRow").attr("title", "Edit Row");
            $(".editRows").hide();
            selectRow(xgvProducts, null);
        }

        function editRow(s, e) {
            var id = xgvProducts.GetRowKey(e.visibleIndex);
            txtProId.SetText(id);
            saved = false;
            popProduct.PerformCallback("Load|" + id);
        }


        function initEdit(s, e) {
            if (!saved) {
                txtImgUrl.SetText(s.cpImage);
                var img = txtImgUrl.GetText();
                try {
                    document.getElementById("uploadedImage").src = img;
                    setElementVisible("uploadedImage", true);

                } catch (ex) {
                }
                
                popProduct.Show();
            } else {
                txtImgUrl.SetText("");
                txtProId.SetText("");
                popProduct.Hide();
                xcpProducts.PerformCallback();
            }
        }

        function initEditMultiple(s, e) {
            if (!saved) {
                txtImgUrl.SetText(s.cpImage);
                var img = txtImgUrl.GetText();

                document.getElementById("uploadedImageM").src = img;
                setElementVisible("uploadedImageM", true);

                popMultiple.Show();
            } else {
                txtImgUrl.SetText("");

                popMultiple.Hide();
                xcpProducts.PerformCallback();
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

        function onUploadControlFileUploadCompleteM(s, e) {

            txtImgUrl.SetText('');
            if (e.isValid) {
                document.getElementById("uploadedImageM").src = e.callbackData;
                txtImgUrl.SetText(e.callbackData);
            }

            setElementVisible("uploadedImageM", e.isValid);
        }

        function onImageLoad() {
            var externalDropZone = document.getElementById("externalDropZone");
            var uploadedImage = document.getElementById("uploadedImage");
            uploadedImage.style.left = (externalDropZone.clientWidth - uploadedImage.width) / 2 + "px";
            uploadedImage.style.top = (externalDropZone.clientHeight - uploadedImage.height) / 2 + "px";
        }

        function onImageLoadM() {
            var externalDropZone = document.getElementById("externalDropZoneM");
            var uploadedImage = document.getElementById("uploadedImageM");
            uploadedImage.style.left = (externalDropZone.clientWidth - uploadedImage.width) / 2 + "px";
            uploadedImage.style.top = (externalDropZone.clientHeight - uploadedImage.height) / 2 + "px";
        }

        function setElementVisible(elementId, visible) {
            document.getElementById(elementId).className = visible ? "" : "hidden";
        }


        function saveProduct() {
            if (ASPxClientEdit.ValidateGroup('Save')) {
                saved = true;
                var id = txtProId.GetText();
                popProduct.PerformCallback("Save|" + id);
            }
        }

        function copyData() {
            var id = txtProId.GetText();
            popProduct.PerformCallback("Copy|" + id);
        }

        function editSelection() {
            saved = false;
            popMultiple.PerformCallback("Load");
        }


        function selectRow(s, e) {
            if (s.GetSelectedRowCount() > 1) {
                memDescriptionM.SetText("");
                $("#divMultiple").find("input").each(function () {
                    $(this).val(null);
                });

                $(".editRows").show(100);
                
            } else {
                $(".editRows").hide(100);
                
            }
        }

        function saveMultiple() {
            if (ASPxClientEdit.ValidateGroup('SaveMultiple')) {
                saved = true;
                popMultiple.PerformCallback("Save");
            }
        }


        function changeSection() {
            popSection.Show();
        }

        function saveSection() {
            popSection.PerformCallback();
        }

        function changeSectionEnd(s, e) {
            popSection.Hide();
            xcpProducts.PerformCallback();
        }

        function notifyChanges(s, e) {
            if (s.cpC1 != "") {
                notify("Products synced: " + s.cpC1 + "<br/>" + "Products added: " + s.cpC2, 1, 10, "#divNotify");
            }
            //cmbCategories.Update();
            //cmbBrands.Update();
            initPage();
        }

        function xcaStock_EndCallback(s, e) {
            $("#divStock" + s.cpId).html(s.cpStock);
            $("#divStock" + s.cpId).removeAttr("disabled");
            notify("Product stock updated",1,10,"#divNotify");
        }

        function updateStock(s,Id) {
            xcaStock.PerformCallback(Id);
            $(s).attr("disabled", "disabled");
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="floatLt">
        <h4>Products</h4>
        <dx:ASPxTextBox runat="server" ID="txtImgUrl" ClientInstanceName="txtImgUrl" CssClass="hidden"></dx:ASPxTextBox>
        <dx:ASPxTextBox runat="server" ID="txtProId" ClientInstanceName="txtProId" CssClass="hidden"></dx:ASPxTextBox>

    </div>
    <div class="floatRt pad20">
        <button type="button" class="btnBorder editRows" onclick="editSelection()">Edit Selected Rows</button>
    </div>
    <div class="floatRt pad20 hidden">
        <button type="button" class="btnBorder editRows" onclick="changeSection()">Change Section</button>
    </div>
    <div id="divNotify" class="floatRt pad20">
        <input type="button" value="Sync Database Now" class="btnNoBorder" onclick="xcpProducts.PerformCallback('Update');" />
    </div>
    <div class="clearfix"></div>
    <%--    <div class="pad20 floatLt">
        <dx:ASPxComboBox runat="server" ID="cmbCategories" TextField="catName" ValueField="catNumber" NullText="Pick Categorie" DataSourceID="lnqCategories">
            <ClientSideEvents ValueChanged="function(s,e){xcpProducts.PerformCallback();}" />
        </dx:ASPxComboBox>
    </div>--%>

    <dx:ASPxCallbackPanel runat="server" ID="xcpProducts" ClientInstanceName="xcpProducts" OnCallback="xcpProducts_Callback" Width="100%" HideContentOnCallback="true">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="xgvProducts" ClientInstanceName="xgvProducts" runat="server" Width="100%" Settings-ShowFilterRow="true"
                    SettingsPager-PageSize="50" Settings-AutoFilterCondition="Contains" SettingsBehavior-AllowFocusedRow="true" KeyFieldName="Id">
                    <Styles>
                        <Header VerticalAlign="Top" BackColor="#f5f5f5" HorizontalAlign="Center" Paddings-PaddingTop="3px" Font-Bold="true" Font-Size="1em"></Header>
                        <FilterRow BackColor="#f5f5f5"></FilterRow>
                        <Row BackColor="#f5f5f5" Cursor="pointer" CssClass="editRow"></Row>
                        <FocusedRow BackColor="#333e49" ForeColor="White"></FocusedRow>
                        <SelectedRow BackColor="White" ForeColor="Black"></SelectedRow>
                    </Styles>
                    <SettingsPager Position="TopAndBottom"></SettingsPager>
                    <Settings ShowFilterRow="true" />
                    
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" SelectAllCheckboxMode="AllPages">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataColumn>
                            <DataItemTemplate>
                                <div class="BoxGeneral text-center">
                                <img style="max-height: 25px" src="<%# Eval("ImageLink").ToString() == "" ? "/photos/noPhoto.png" :  Eval("ImageLink") %>" alt="<%# Eval("Code") %>" />
                                </div>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Brand" Caption="Brand"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="TyreSize" Caption="Tyre size"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Type" Caption="Type"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Code" Caption="Code"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="LI" Caption="L.I."></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="SI" Caption="SI"></dx:GridViewDataColumn>
                    <%--    <dx:GridViewDataColumn FieldName="Code" Caption="Code"></dx:GridViewDataColumn>--%>
                        <dx:GridViewDataColumn FieldName="Category" Caption="Category"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="OurCategory" Caption="Section"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="F" Caption="G">
                            <HeaderTemplate>
                                <img width="30px" src="/Scripts/imgs/tyreFuel.png" alt="Efficiency" />
                            </HeaderTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="W" Caption="R">
                            <HeaderTemplate>
                                <img width="30px" src="/Scripts/imgs/tyreWet.png" alt="Wet" />
                            </HeaderTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="N" Caption="S">
                            <HeaderTemplate>
                                <img width="30px" src="/Scripts/imgs/tyreNoise.png" alt="Noise" />
                            </HeaderTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataSpinEditColumn FieldName="SuggestedPrice" Caption="Price">
                            <PropertiesSpinEdit DisplayFormatString="c2"></PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataSpinEditColumn FieldName="Stock">
                            <DataItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <div id='divStock<%# Eval("Id") %>'><%# Eval("Stock") %></div>
                                        </td>
                                        <td>
                                            <button type="button" onclick="updateStock(this,<%# Eval("Id") %>)" class="btnRefresh" ></button>
                                        </td>
                                       
                                    </tr>
                                </table>
                                
                            </DataItemTemplate>
                        </dx:GridViewDataSpinEditColumn>
                    </Columns>
                    <ClientSideEvents RowDblClick="editRow" SelectionChanged="selectRow" />
                </dx:ASPxGridView>
            </dx:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="notifyChanges" />
    </dx:ASPxCallbackPanel>

    <div class="floatRt pad20">
        <button type="button" class="btnBorder editRows" onclick="editSelection()">Edit Selected Rows</button>
    </div>

    <dx:ASPxPopupControl runat="server" ID="popProduct" ClientInstanceName="popProduct" Width="1000px" OnWindowCallback="popProduct_WindowCallback"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" HeaderText="Edit Product" Modal="true" AllowDragging="true">
        <ContentCollection>
            <dx:PopupControlContentControl>

                <div style="max-height: 95vh; overflow: auto; width: 100%">
                    <div class="clearfix"></div>
                    <div class="col-md-3">
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
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#collapse2">BRAND&nbsp;&nbsp;
                            <img src="../Scripts/imgs/icons/exp.png" /></a>
                                    </h4>
                                </div>
                                <div id="collapse2" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <dx:ASPxComboBox runat="server" ID="cmbBrands" ClientInstanceName="cmbBrands" TextField="braName" ValueField="braId" NullText="Pick Brand" DataSourceID="lnqBrands">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#collapse5">SECTION&nbsp;&nbsp;
                            <img src="../Scripts/imgs/icons/exp.png" /></a>
                                    </h4>
                                </div>
                                <div id="collapse5" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <dx:ASPxComboBox runat="server" ID="cmbOurCategories" TextField="catName" ValueField="catId" NullText="Pick Categorie" DataSourceID="lnqOurCategories">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default hidden">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#collapse3">CATEGORY&nbsp;&nbsp;
                            <img src="../Scripts/imgs/icons/exp.png" /></a>
                                    </h4>
                                </div>
                                <div id="collapse3" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <dx:ASPxComboBox runat="server" ID="cmbCategories" ClientInstanceName="cmbCategories" TextField="cdpName" ValueField="cdpId" NullText="Pick Categorie" DataSourceID="lnqCategories" ReadOnly="true">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                    <div class="col-xs-12  col-md-9">
                        <div class="col-xs-9">
                            <dx:ASPxTextBox runat="server" NullText="PRODUCT" ID="txtProduct" onkeydown="return onlyAN(this,event);" CssClass="tableInput BoxGeneral fontRed" Font-Size="X-Large" ForeColor="#dd1730">
                                <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                    <RequiredField IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </div>
                        <div class="col-xs-3">
                            <div class="floatRt pad5">
                                <input type="button" class="btnBorder" value="SAVE" onclick="saveProduct();" />
                            </div>
                        </div>

                        <div class="clearfix"></div>
                        <div class="col-sm-6" style="border-right: 1px solid #865459">
                            <h3><span class="fontDarkGray">Dekkpro Data</span></h3>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <b>Description:</b><br />
                            <dx:ASPxMemo runat="server" ID="memDescriptionDP" CssClass="BoxGeneral" ReadOnly="true" Height="50px"></dx:ASPxMemo>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <b>Code:</b><br />
                            <dx:ASPxTextBox runat="server" ID="txtCodeDP" CssClass="BoxGeneral" ReadOnly="true"></dx:ASPxTextBox>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <table class="BoxGeneral text-center" style="background-color: #f3f3f3">
                                <tr>
                                    <td colspan="3">
                                        <b>Tyre Size</b>
                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        <b>Width</b>
                                    </td>
                                    <td style="width: 33%">
                                        <b>Profile</b>
                                    </td>
                                    <td style="width: 33%">
                                        <b>Diameter</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spWidthDP" NumberType="Integer" ReadOnly="true" CssClass="BoxGeneral">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spProfileDP" NumberType="Integer" ReadOnly="true" CssClass="BoxGeneral">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>

                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spDiameterDP" NumberType="Integer" ReadOnly="true" CssClass="BoxGeneral">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                            </table>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <table class="BoxGeneral text-center" style="background-color: #f3f3f3">

                                <tr>
                                    <td style="width: 20%">
                                        <b>L.I.</b>
                                    </td>
                                    <td style="width: 20%">
                                        <b>Speed</b>
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreFuel.png" alt="Efficiency" />
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreWet.png" alt="Wet" />
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreNoise.png" alt="Noise" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spLoadIndexDP" NumberType="Integer" ReadOnly="true" CssClass="BoxGeneral">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtSpeedDP" ReadOnly="true" CssClass="BoxGeneral" Height="31px">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtFuelDP" CssClass="BoxGeneral" ReadOnly="true" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtWetDP" CssClass="BoxGeneral" ReadOnly="true" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtNoiseDP" CssClass="BoxGeneral" ReadOnly="true" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>

                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <b>Cover Price:</b><br />
                            <dx:ASPxSpinEdit runat="server" ID="spCoverPriceDP" NumberType="Float" DisplayFormatString="c2" ReadOnly="true" CssClass="BoxGeneral"></dx:ASPxSpinEdit>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <%--<b>Suggested Price:</b><br />--%>
                            <dx:ASPxSpinEdit runat="server" ID="spSuggestedPriceDP" NumberType="Float" DisplayFormatString="c2" ReadOnly="true" CssClass="BoxGeneral hidden" Width="100%">
                                <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                    <RequiredField IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxSpinEdit>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
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
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <b>Description:</b><br />
                            <dx:ASPxMemo runat="server" ID="memDescription" CssClass="BoxGeneral" Height="50px"></dx:ASPxMemo>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <b>Code:</b><br />
                            <dx:ASPxTextBox runat="server" ID="txtCode" CssClass="BoxGeneral"></dx:ASPxTextBox>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <table class="BoxGeneral text-center" style="background-color: #f3f3f3">
                                <tr>
                                    <td colspan="3">
                                        <b>Tyre Size</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Width</b>
                                    </td>
                                    <td style="width: 33%">
                                        <b>Profile</b>
                                    </td>

                                    <td style="width: 33%">
                                        <b>Diameter</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spWidth" NumberType="Integer" CssClass="BoxGeneral">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spProfile" NumberType="Integer" CssClass="BoxGeneral">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>

                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spDiameter" NumberType="Integer" CssClass="BoxGeneral">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                            </table>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <table class="BoxGeneral text-center" style="background-color: #f3f3f3">

                                <tr>
                                    <td style="width: 20%">
                                        <b>L.I.</b>
                                    </td>
                                    <td style="width: 20%">
                                        <b>Speed</b>
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreFuel.png" alt="Efficiency" />
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreWet.png" alt="Wet" />
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreNoise.png" alt="Noise" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spLoadIndex" NumberType="Integer" CssClass="BoxGeneral">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtSpeed" CssClass="BoxGeneral" Height="31px">
                                            <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtFuel" CssClass="BoxGeneral" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtWet" CssClass="BoxGeneral" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtNoise" CssClass="BoxGeneral" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>

                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <b>Cover Price:</b><br />
                            <dx:ASPxSpinEdit runat="server" ID="spCoverPrice" NumberType="Float" DisplayFormatString="c2" CssClass="BoxGeneral"></dx:ASPxSpinEdit>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                            <b>Price:</b><br />
                            <%--<asp:Label runat="server" ID="lblPrice"></asp:Label>--%>
                            <dx:ASPxSpinEdit runat="server" ID="spSuggestedPrice" NumberType="Float" DisplayFormatString="c2" CssClass="BoxGeneral" Width="100%">
                                <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Save">
                                    <RequiredField IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxSpinEdit>
                            <div class="clearfix"></div>
                            <div class="spacer5"></div>
                            <div class="clearfix"></div>
                        </div>



                    </div>
                    <div class="col-xs-12" style="height: 200px; overflow: auto">
                        <div class="clearfix"></div>
                        <dx:ASPxFileManager ID="xGalleries" ClientInstanceName="xGalleries" runat="server" Width="100%" Height="100%" SettingsUpload-UseAdvancedUploadMode="true" Theme="Moderno"
                            SettingsFileList-ShowFolders="false" SettingsToolbar-ShowPath="false" SettingsUpload-AutoStartUpload="true">
                            <Settings RootFolder="~\photos\products" ThumbnailFolder="~\photos\thumbs" EnableMultiSelect="true" AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif,.png,.tiff" />
                            <SettingsFolders ShowFolderIcons="true" Visible="false" />
                            <SettingsToolbar ShowFilterBox="true" ShowCreateButton="true" ShowRenameButton="true" ShowDeleteButton="true" ShowDownloadButton="true" />
                            <SettingsEditing AllowCreate="true" AllowDelete="true" AllowRename="true" AllowMove="true" />
                            <SettingsUpload ShowUploadPanel="true" AdvancedModeSettings-EnableMultiSelect="true">
                                <ValidationSettings MaxFileSize="3145728"></ValidationSettings>
                            </SettingsUpload>
                            <SettingsToolbar ShowFilterBox="false"></SettingsToolbar>
                            <Styles>
                                <UploadPanel Height="80px"></UploadPanel>
                            </Styles>
                        </dx:ASPxFileManager>
                    </div>

                    <div class="clearfix"></div>
                    <div class="spacer20"></div>
                    <div class="clearfix"></div>
                    <div class="floatRt pad5">
                        <input type="button" class="btnBorder" value="SAVE" onclick="saveProduct();" />
                    </div>



                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents EndCallback="initEdit" />
    </dx:ASPxPopupControl>


    <dx:ASPxPopupControl runat="server" ID="popMultiple" ClientInstanceName="popMultiple" Width="720px" OnWindowCallback="popMultiple_WindowCallback"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" HeaderText="Edit Products" Modal="true" AllowDragging="true">
        <ContentCollection>
            <dx:PopupControlContentControl>

                <div style="max-height: 95vh; overflow: auto; width: 100%">
                    <div class="clearfix"></div>
                    <div class="col-md-4">
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
                                        <div id="externalDropZoneM" class="hovereffect dropZoneExternal">
                                            <img id="uploadedImageM" src="#" style="width: 100%;" class="hidden" alt="" onload="onImageLoadM()" />
                                            <div class="overlay">
                                                <h3>PICK IMAGE</h3>
                                            </div>
                                        </div>





                                        <div class="hidden">
                                            <dx:ASPxUploadControl ID="UploadControlM" ClientInstanceName="UploadControlM" runat="server" UploadMode="Auto" AutoStartUpload="True" Width="350"
                                                ShowProgressPanel="True" CssClass="uploadControl" DialogTriggerID="externalDropZoneM" OnFileUploadComplete="UploadControlM_FileUploadComplete">
                                                <AdvancedModeSettings EnableDragAndDrop="True" EnableFileList="False" EnableMultiSelect="False" ExternalDropZoneID="externalDropZoneM" DropZoneText="" />
                                                <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage" />
                                                <BrowseButton Text="Select an image for upload..." />
                                                <DropZoneStyle CssClass="uploadControlDropZone" />
                                                <ProgressBarStyle CssClass="uploadControlProgressBar" />
                                                <ClientSideEvents
                                                    DropZoneEnter="function(s, e) { if(e.dropZone.id == 'externalDropZoneM') setElementVisible('dropZone', true); }"
                                                    DropZoneLeave="function(s, e) { if(e.dropZone.id == 'externalDropZoneM') setElementVisible('dropZone', false); }"
                                                    FileUploadComplete="onUploadControlFileUploadCompleteM"></ClientSideEvents>
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
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#collapse2">BRAND&nbsp;&nbsp;
                            <img src="../Scripts/imgs/icons/exp.png" /></a>
                                    </h4>
                                </div>
                                <div id="collapse2" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <dx:ASPxComboBox runat="server" ID="cmbBrandM" TextField="braName" ValueField="braId" NullText="Pick Brand" DataSourceID="lnqBrands">
                                           
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#collapse5">SECTION&nbsp;&nbsp;
                            <img src="../Scripts/imgs/icons/exp.png" /></a>
                                    </h4>
                                </div>
                                <div id="collapse5" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <dx:ASPxComboBox runat="server" ID="cmbOurCategoriesM" TextField="catName" ValueField="catId" NullText="Pick Categorie" DataSourceID="lnqOurCategories">
                                            
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>



                        </div>
                    </div>
                    <div class="col-xs-12  col-md-8">
                        <div class="col-xs-9">
                            <dx:ASPxTextBox runat="server" NullText="PRODUCT" ID="txtProductM" onkeydown="return onlyAN(this,event);" CssClass="tableInput BoxGeneral fontRed" Font-Size="X-Large" ForeColor="#dd1730">
                             
                            </dx:ASPxTextBox>
                        </div>
                        <div class="col-xs-3">
                            <div class="floatRt pad5">
                                <input type="button" class="btnBorder editRows" value="Save Products" onclick="saveMultiple();" />
                            </div>
                        </div>

                        <div class="clearfix"></div>
                        <div id="divMultiple" class="col-sm-12">
                            <b>Description:</b><br />
                            <dx:ASPxMemo runat="server" ID="memDescriptionM" ClientInstanceName="memDescriptionM" CssClass="BoxGeneral" Height="100px"></dx:ASPxMemo>

                            <div class="clearfix"></div>
                            <div class="spacer20"></div>
                            <div class="clearfix"></div>
                            <table class="BoxGeneral text-center" style="background-color: #f3f3f3">

                                <tr>
                                    <td style="width: 20%">
                                        <b>L.I.</b>
                                    </td>
                                    <td style="width: 20%">
                                        <b>Speed</b>
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreFuel.png" alt="Efficiency" />
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreWet.png" alt="Wet" />
                                    </td>
                                    <td style="width: 20%">
                                        <img width="30px" src="/Scripts/imgs/tyreNoise.png" alt="Noise" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxSpinEdit runat="server" ID="spLoadIndexM" NumberType="Integer" CssClass="BoxGeneral">
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtSpeedM" CssClass="BoxGeneral" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtFuelM" CssClass="BoxGeneral" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtWetM" CssClass="BoxGeneral" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ID="txtNoiseM" CssClass="BoxGeneral" Height="31px">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>

                            <div class="clearfix"></div>
                            <div class="spacer20"></div>
                            <div class="clearfix"></div>
                            <b>Adjust Price by %:</b><br />
                            <dx:ASPxSpinEdit runat="server" ID="spPriceP" NumberType="Integer" DisplayFormatString="{0:0'%'}" CssClass="BoxGeneral" MaxValue="100" MinValue="-100" Increment="1"></dx:ASPxSpinEdit>
                            <div class="clearfix"></div>
                            <b>Adjust Price adding kr:</b><br />
                            <dx:ASPxSpinEdit runat="server" ID="spPriceM" NumberType="Float" DisplayFormatString="{0:c0}" CssClass="BoxGeneral" MaxValue="10000" MinValue="0" Increment="1"></dx:ASPxSpinEdit>
                            <div class="clearfix"></div>
                            <div class="spacer20"></div>
                            <div class="clearfix"></div>

                        </div>



                    </div>
                    <div class="col-xs-12" style="height: 200px; overflow: auto">
                        <div class="clearfix"></div>
                        <dx:ASPxFileManager ID="xGalleriesM" ClientInstanceName="xGalleriesM" runat="server" Width="100%" Height="100%" SettingsUpload-UseAdvancedUploadMode="true" Theme="Moderno"
                            SettingsFileList-ShowFolders="false" SettingsToolbar-ShowPath="false" SettingsUpload-AutoStartUpload="true">
                            <Settings RootFolder="~\photos\products\multiple\" ThumbnailFolder="~\photos\thumbs" EnableMultiSelect="true" AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif,.png,.tiff" />
                            <SettingsFolders ShowFolderIcons="true" Visible="false" />
                            <SettingsToolbar ShowFilterBox="true" ShowCreateButton="true" ShowRenameButton="true" ShowDeleteButton="true" ShowDownloadButton="true" />
                            <SettingsEditing AllowCreate="true" AllowDelete="true" AllowRename="true" AllowMove="true" />
                            <SettingsUpload ShowUploadPanel="true" AdvancedModeSettings-EnableMultiSelect="true">
                                <ValidationSettings MaxFileSize="3145728"></ValidationSettings>
                            </SettingsUpload>
                            <SettingsToolbar ShowFilterBox="false"></SettingsToolbar>
                            <Styles>
                                <UploadPanel Height="80px"></UploadPanel>
                            </Styles>
                        </dx:ASPxFileManager>
                    </div>

                    <div class="clearfix"></div>
                    <div class="spacer20"></div>
                    <div class="clearfix"></div>
                    <div class="floatRt pad5">
                        <input type="button" class="btnBorder editRows" value="Save Products" onclick="saveMultiple();" />
                    </div>



                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents EndCallback="initEditMultiple" />
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl runat="server" ID="popSection" ClientInstanceName="popSection" OnWindowCallback="popSection_WindowCallback" Width="320px"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" HeaderText="Edit Products" Modal="true" AllowDragging="true">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <h4>PICK SECTION</h4>
                <dx:ASPxComboBox runat="server" ID="cmbSection" TextField="catName" ValueField="catId" NullText="Pick Categorie" DataSourceID="lnqOurCategories">
                    <ValidationSettings ErrorFrameStyle-CssClass="redBorder" ErrorDisplayMode="None" ValidationGroup="Section">
                        <RequiredField IsRequired="true" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
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

    <dx:ASPxCallback runat="server" ID="xcaStock" ClientInstanceName="xcaStock" OnCallback="xcaStock_Callback">
        <ClientSideEvents EndCallback="xcaStock_EndCallback" />
    </dx:ASPxCallback>

    <asp:LinqDataSource ID="lnqOurCategories" runat="server" ContextTypeName="DekkOnline.dbDekkOnlineDataContext" TableName="categories" Where="catStatus=true"></asp:LinqDataSource>
    <asp:LinqDataSource ID="lnqCategories" runat="server" ContextTypeName="DekkOnline.dbDekkOnlineDataContext" TableName="categoriesDPs" Where="cdpStatus=true"></asp:LinqDataSource>
    <asp:LinqDataSource ID="lnqBrands" runat="server" ContextTypeName="DekkOnline.dbDekkOnlineDataContext" TableName="brands"></asp:LinqDataSource>
</asp:Content>
