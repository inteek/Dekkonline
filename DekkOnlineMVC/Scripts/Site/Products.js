


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


    setTimeout(function () {
        var $btns = $("[data-toggle='popover-x']");
        if ($btns.length) {
            $btns.popoverButton();
        }
        Ladda.bind('.ladda-button');
    }, 1000)



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








function openProductDetails(indexRow) {


    ProductCards.GetCardValues(indexRow, "Id", function (value) { $("#productDetails_Id").val(value); });
    ProductCards.GetCardValues(indexRow, "Image", function (value) { $("#productDetails_Img").attr("src" + "http://admin.dekkonline.sonetworks.no/" + value); });
    ProductCards.GetCardValues(indexRow, "CategoryImage", function (value) { $("#productDetails_CategoryImg1").attr("src", value.replace("~", "..")); $("#productDetails_CategoryImg2").attr("src", value.replace("~", "..")); });
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


function ShoppingCart(id, name) {
    e.preventDefault();
    //var btnLoad = Ladda.create(this);
    //btnLoad.start();

    var qty = $("#cboAddCartLisProduct" + id).val();
    AddProductToCart(id, qty, name, true, function () {
        //btnLoad.stop();
        $("#popoverProduct" + id).hide();
    });
}

function AddToCart(id, name) {
    //e.preventDefault();
    //var btnLoad = Ladda.create(this);
    //btnLoad.start();
    
    var qty = $("#cboAddCartLisProduct" + id).val();
    AddProductToCart(id, qty, name, false, function () {
        //btnLoad.stop();
        $("#popoverProduct" + id).hide();
    });

}

function AddProductToCart(id, qty, name, returnCart, callback) {
    var data = {
        idPro: id,
        quantity: qty
    }




    conectarAsy("../Dekk/AddToCart", data, function (result) {
        var notyf = new Notyf();
        if (result.Success == true) {
            if (returnCart) {
                window.location = "../ShoppingCart/Index";
            }
            else {
                var qtyOld = parseInt($("#lblProductCount").text());
                var qtyNew = qtyOld + parseInt(qty);
                $("#lblProductCount").text(qtyNew);

                notyf.confirm('Product: ' + id + ' / ' + name);
            }


        }
        else {
            notyf.alert('Product: ' + id + ' / ' + name);
        }
        callback();
    });


}




$(document).ready(function () {


    $("#principalDiv").attr("class", "prDivSub");
    $("#mainContainDiv").attr("class", "prMainContentSub");
    $("#divCart").css("padding-top", "200px");


    $("#btnListView,#btnGridView").on("click", function () {
        TypeResult(this);
    });


    $("#productDetails_BtnAddCart").on("click", function () {
        var id = $("#productDetails_Id").val();
        var name = $("#productDetails_Name").text();
        var qty = productDetails_CboAddCart.GetValue();


        AddProductToCart(id, qty, name, false);

        productDetails_CboAddCart.SetValue(1);
        $('#modalProductDetails').modal('hide');

    })

});
