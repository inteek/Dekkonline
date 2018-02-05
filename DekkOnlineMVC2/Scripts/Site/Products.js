


function OnSortingChanged(s, e) {
    HeadphoneCards.PerformCallback();
}
function OnHeadphoneCardsBeginCallback(s, e) {
    e.customArgs["isCardView"] = this.IsCardView();
    e.customArgs["sortMode"] = 0; //Sorting.GetValue();


    e.customArgs["sizeWidth"] = cmbWidth.GetValue();
    e.customArgs["profile"] = cmbProfile.GetValue();
    e.customArgs["sizeDiameter"] = cmbDiameter.GetValue();
    e.customArgs["idCategory"] = cmbCategory.GetValue();
    e.customArgs["idBrand"] = BagBrands.GetValue();

}
function IsCardView() {
    return cardView; //HeadphoneCards.GetToolbar(0).GetItemByName("CardView").GetChecked();
}
function UpdateCardViewHeight() {
    HeadphoneCards.SetHeight(0);
    var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
    if (document.body.scrollHeight > containerHeight)
        containerHeight = document.body.scrollHeight;
    HeadphoneCards.SetHeight(containerHeight);
}


function TypeResult(btn) {
    //debugger

    if (HeadphoneCards != undefined && HeadphoneCards != null)
    {
        if (cardView == true || cardView == 'True') {
            //$(btn).attr("value", "Card View")
            cardView = false;
        } else {
            //$(btn).attr("value", "List View")
            cardView = true;
        }
        HeadphoneCards.Refresh();

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

    if (HeadphoneCards != undefined && HeadphoneCards != null) {
        HeadphoneCards.Refresh();
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
