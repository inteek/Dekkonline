


function OnSortingChanged(s, e) {
    ProductCards.PerformCallback();
}
function OnProductCardsBeginCallback(s, e) {
    e.customArgs["isCardView"] = this.IsCardView();
    e.customArgs["sortMode"] = 0; //Sorting.GetValue();


    e.customArgs["sizeWidth"] = cmbWidth.GetValue();
    e.customArgs["profile"] = cmbProfile.GetValue();
    e.customArgs["sizeDiameter"] = cmbDiameter.GetValue();
    e.customArgs["idCategory"] = cmbCategory.GetValue();
    e.customArgs["idBrand"] = BagBrands.GetValue();

}
function IsCardView() {
    return cardView; //ProductCards.GetToolbar(0).GetItemByName("CardView").GetChecked();
}
function UpdateCardViewHeight() {
    ProductCards.SetHeight(0);
    var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
    if (document.body.scrollHeight > containerHeight)
        containerHeight = document.body.scrollHeight;
    ProductCards.SetHeight(containerHeight);
}


function TypeResult(btn) {
    //debugger

    if (ProductCards != undefined && ProductCards != null)
    {
        if (cardView == true || cardView == 'True') {
            //$(btn).attr("value", "Card View")
            cardView = false;
        } else {
            //$(btn).attr("value", "List View")
            cardView = true;
        }
        ProductCards.Refresh();

    }
}

ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
    UpdateCardViewHeight();
});
ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
    UpdateCardViewHeight();
});






function cmbFilters_ValueChanged(s, e) {
    //alert(s.uniqueID + " - " + s.GetText() + " / " + s.GetValue());

    if (ProductCards != undefined && ProductCards != null) {
        ProductCards.Refresh();
    }
}




function AddToCart(btn, id, name) {

    id = parseInt(id);
    var qty = parseInt($(btn).parent().find("select option:selected").val());

    var data = {
        idPro: id,
        quantity: qty
    }

    
    debugger
    conectarAsy("../Dekk/AddToCart", data, function (result) {
        var notyf = new Notyf();
        if (result.Success == true) {
            debugger
            notyf.confirm('Product: ' + id + ' / ' + name);
        }
        else {
            debugger
            notyf.alert('Product: ' + id + ' / ' + name);
        }
    });

}

function OnFocusedCardChanged(s, e) {
    var key = s.GetCardKey(s.GetFocusedCardIndex());
    openProductDetails(key);
}


function openProductDetails(indexRow) {


    ProductCards.GetCardValues(indexRow, "Id", function (value) { $("#productDetails_Id").text(value); });
    ProductCards.GetCardValues(indexRow, "Image", function (value) { $("#productDetails_Img").attr("src" + "http://admin.dekkonline.sonetworks.no/" + value); });
    ProductCards.GetCardValues(indexRow, "CategoryImage", function (value) { $("#productDetails_CategoryImg1").attr("src", value.replace("~", "")); $("#productDetails_CategoryImg2").attr("src", value.replace("~", "")); });
    ProductCards.GetCardValues(indexRow, "Name", function (value) { $("#productDetails_Name").text(value); });
    //ProductCards.GetCardValues(indexRow, "Description", function (value) { $("#productDetails_Description").text(value); });
    ProductCards.GetCardValues(indexRow, "Brand", function (value) { $("#productDetails_Brand").text(value); });
    ProductCards.GetCardValues(indexRow, "Width", function (value) { productDetails_CboWidth.SetValue(value); $("#productDetails_LblWidth".text(value)); });
    ProductCards.GetCardValues(indexRow, "Profile", function (value) { productDetails_CboProfile.SetValue(value); $("#productDetails_LblProfile".text(value)); });
    ProductCards.GetCardValues(indexRow, "Diameter", function (value) { productDetails_CboDiameter.SetValue(value); $("#productDetails_LblDiameter".text(value)); });
    ProductCards.GetCardValues(indexRow, "CategoryName", function (value) { $("#productDetails_CategoryName").text(value); });
    ProductCards.GetCardValues(indexRow, "Stock", function (value) { $("#productDetails_InStock").text(value); });
    ProductCards.GetCardValues(indexRow, "SpeedIndex", function (value) { $("#productDetails_SpeedIndex").text(value); });
    ProductCards.GetCardValues(indexRow, "LoadIndex", function (value) { $("#productDetails_Indexload").text(value); });
    ProductCards.GetCardValues(indexRow, "Fuel", function (value) { $("#productDetails_TyreFuel").html(value); });
    ProductCards.GetCardValues(indexRow, "Noise", function (value) { $("#productDetails_TyreNoise").html(value); });
    ProductCards.GetCardValues(indexRow, "Wet", function (value) { $("#productDetails_TyreWet").html(value); });
    
    $('#modalProductDetails').modal('show');
}


function ShoppingCart(btn, id, name) {
    id = parseInt(id);
    var qty = parseInt($(btn).parent().find("select option:selected").val());

    var data = {
        idPro: id,
        quantity: qty
    }

    debugger
    conectarAsy("../Dekk/AddToCart", data, function (result) {
        var notyf = new Notyf();
        if (result.Success == true) {
            debugger
            window.location = "../ShoppingCart/Index";
        }
        else {
            debugger
            notyf.alert('Product: ' + id + ' / ' + name);
        }
    });
}


$(document).ready(function () {

    $("#principalDiv").attr("class", "prDivSub");
    $("#mainContainDiv").attr("class", "prMainContentSub");


    $("#btnListView,#btnGridView").on("click", function () {
        TypeResult(this);
    });


});
