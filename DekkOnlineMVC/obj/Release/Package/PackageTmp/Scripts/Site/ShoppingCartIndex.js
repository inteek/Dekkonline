var localStep3 = 0;

$(document).ready(function () {
    localStep3 = localStorage.getItem("Step3");
    $(".keep").click(function () {
        var url = "../Dekk/Products";
        window.location = url;
    });
    $(".st2").click(function () {
        var url = "../ShoppingCart/Step2";
        window.location = url;
    });
    $(".addpromoapp").on("click", ".PromoCodeappaction", function () {
        var promo = $(".promocode").val();
        if (promo.length < 1) {
            $(".prom").removeClass("hidden");
            return false;
        }
        else {
            $(".prom").addClass("hidden");
            ValPromo(promo);
        }
    }); 

    $(".dee").click(function () {
        $(".del1").removeAttr('disabled');
        $(".dee").removeAttr('disabled');
        $(".del").modal('hide');
        $(".qtyprodcart").removeAttr('disabled');
        $(".PromoCodeunaction").removeAttr('disabled');
        $(".keep").removeAttr('disabled');
        $(".inemail").removeAttr('disabled');
        $(".remclick").removeAttr('disabled');
    });
    $(".addpromoapp").on("click", ".PromoCodeunaction", function () {
        var promo = $(".promocode").val();
        UndoPromo(promo);
    });
    $("#Step2").click(function () {
        var url = "../ShoppingCart/Step2"
        window.location = url;
    });

    $("#Step3").click(function () {

        if (localStep3 == 1) {
            var url = "../ShoppingCart/Step3"
            window.location = url;
        }
    });
    loadWorkShop();
});

function DelCrt(id) {
    var data = {
        idcart : id
    };
    $(".prom").addClass("hidden");
    $(".del").modal('show');
    $(".qtyprodcart").attr('disabled', 'disabled');
    $(".PromoCodeappaction").attr('disabled', 'disabled');
    $(".PromoCodeunaction").attr('disabled', 'disabled');
    $(".keep").attr('disabled', 'disabled');
    $(".inemail").attr('disabled', 'disabled');
    $(".remclick").attr('disabled', 'disabled');
    $(".del1").on("click", function () {
        $(".del1").attr('disabled', 'disabled');
        $(".dee").attr('disabled', 'disabled');
        conectarAsy("../ShoppingCart/DeleteFromCart", data, function (result) {
            if (result != null && result.Success != false) {
                var b = result['x'];
                var c = b['subtotal'];
                if ( c != 0 && c != "undefined" && c != null) {
                $("."+id).remove();
                $(".prom").addClass("hidden");
                $("#subtotal").text(b['subtotal']);
                $("#total").text(b['total']);
                $(".promocode").val(b['promocode']);
                $(".promopoints").val(b['points']);
                $("#tax").text(b['tax']);
                if (b['promocodeapp'] == false) {
                    $(".PromoCodeapp").remove();
                    $(".addpromoapp").append("<label class='PromoCodeapp PromoCodeun' style='margin-top:7px;color:blue;'><a class='PromoCodeap PromoCodeappaction'><span class='linkcolor2 PromoCodeapp'>APPLY</span></a></label>");
                }
                else if (b['promocodeapp'] == true) {
                    $(".PromoCodeun").remove();
                    $(".addpromoapp").append("<label class='PromoCodeapp PromoCodeun' style='margin-top:7px;color:blue;'><a class='PromoCodeun PromoCodeunaction'><span class='linkcolor2 PromoCodeapp'>UNDO</span></a></label>");
                }
                $(".del1").removeAttr('disabled');
                $(".dee").removeAttr('disabled');
                $(".del").modal('hide');
                $(".qtyprodcart").removeAttr('disabled');
                $(".PromoCodeunaction").removeAttr('disabled');
                $(".keep").removeAttr('disabled');
                $(".inemail").removeAttr('disabled');
                $(".remclick").removeAttr('disabled');
                updateCountProductCart();
                }
                else {
                    $("#lblProductCount").text("0")
                    $(".del").modal('hide');
                    $(".del1").removeAttr('disabled');
                    $(".dee").removeAttr('disabled');
                $(".qtyprodcart").removeAttr('disabled');
                $(".qtyprodcart").removeAttr('disabled');
                $(".PromoCodeunaction").removeAttr('disabled');
                $(".keep").removeAttr('disabled');
                $(".inemail").removeAttr('disabled');
                $(".remclick").removeAttr('disabled');
                    $("." + id).remove();
                    $(".tabl1").append("<tr><td data-th='Item'><div class='row'><div class='col-sm-8 col-xs-8'><br /><div><p>Your Shopping Cart is empty.</p></div></div></div ></td ></tr>");
                    $(".dtpy").remove();
                    $(".alldta").append("<div class='col-sm-6 dtcte '><div class='form-group '><label class='col-md-12 col-xs-2 control-label keep1'> <a><span class='linkcolor keep' style='color:steelblue;' onclick='redirect()'>KEEP SHOPPING</span></a></label></div></div>");
                    updateCountProductCart();
                }
            }
            else if (result.Success == false) {
                $(".del1").removeAttr('disabled');
                $(".dee").removeAttr('disabled');
                $(".qtyprodcart").removeAttr('disabled');
                $(".qtyprodcart").removeAttr('disabled');
                $(".PromoCodeunaction").removeAttr('disabled');
                $(".keep").removeAttr('disabled');
                $(".inemail").removeAttr('disabled');
                $(".remclick").removeAttr('disabled');
                console.log(result);
            }
            else if (result.error == true) {
                alert(result.msg);
            }
        });
    });
    
}

    function IncrsCart(id, qty1) {
    var data = {
        idcart: id,
        qty: qty1
    };
    $(".qtyprodcart").attr('disabled', 'disabled');
    $(".PromoCodeappaction").attr('disabled', 'disabled');
    $(".PromoCodeunaction").attr('disabled', 'disabled');
    $(".keep").attr('disabled', 'disabled');
    $(".inemail").attr('disabled', 'disabled');
    $(".remclick").attr('disabled', 'disabled');
    conectarAsy("../ShoppingCart/IncreaseProductFromCart", data, function (result) {
        if (result != null && result.Success != false) {
            $(".prom").addClass("hidden");
            //$("." + id).val(qty1);
            $("#" + id).text(result['a']);
            var b = result['x'];
            $("#subtotal").text(b['subtotal']);
            $("#b+" + id).text(result['a']);
                $("#total").text(b['total']);

                $(".promocode").val(b['promocode']);
                $(".promopoints").val(b['points']);
                $("#tax").text(b['tax']);
                if (b['promocodeapp'] == false) {
                    $(".PromoCodeapp").remove();
                    $(".addpromoapp").append("<label class='PromoCodeapp PromoCodeun' style='margin-top:7px;color:blue;'><a class='PromoCodeap PromoCodeappaction'><span class='linkcolor2 PromoCodeapp'>APPLY</span></a></label>");
                }
                else if (b['promocodeapp'] == true) {
                    $(".PromoCodeun").remove();
                    $(".addpromoapp").append("<label class='PromoCodeapp PromoCodeun' style='margin-top:7px;color:blue;'><a class='PromoCodeun PromoCodeunaction'><span class='linkcolor2 PromoCodeapp'>UNDO</span></a></label>");
                }
                $(".qtyprodcart").removeAttr('disabled');
                $(".qtyprodcart").removeAttr('disabled');
                $(".PromoCodeunaction").removeAttr('disabled');
                $(".keep").removeAttr('disabled');
                $(".inemail").removeAttr('disabled');
                $(".remclick").removeAttr('disabled');
        }
        else if (result.Success == false) {
            $(".qtyprodcart").removeAttr('disabled');
            $(".qtyprodcart").removeAttr('disabled');
            $(".PromoCodeunaction").removeAttr('disabled');
            $(".keep").removeAttr('disabled');
            $(".inemail").removeAttr('disabled');
            $(".remclick").removeAttr('disabled');
        }
    });
}

function ValPromo(id) {
    var data = {
        Code: id
    };
    $(".qtyprodcart").attr('disabled', 'disabled');
    $(".PromoCodeappaction").attr('disabled', 'disabled');
    $(".PromoCodeunaction").attr('disabled', 'disabled');
    $(".keep").attr('disabled', 'disabled');
    $(".inemail").attr('disabled', 'disabled');
    $(".remclick").attr('disabled', 'disabled');
    $(".promo").addClass("hidden");
    if ($(".promocode").val() == "" || $(".promocode").val() == null || $(".promocode").length < 1) {
        $(".promo").removeClass("hidden");
        return false;
    }
    conectarAsy("../ShoppingCart/ValidatePromo", data, function (result) {
        if (result != null && result.Success != false) {
            $(".prom").addClass("hidden");
            var b = result['x'];
            $("#subtotal").text(b['subtotal']);
            $("#total").text(b['total']);
            $(".promocode").val(b['promocode']);
            $(".promopoints").val(b['points']);
            $("#tax").text(b['tax']);
            if (b['promocodeapp'] == false) {
                $(".PromoCodeapp").remove();
                $(".addpromoapp").append("<label class='PromoCodeapp PromoCodeun' style='margin-top:7px;color:blue;'><a class='PromoCodeap PromoCodeappaction'><span class='linkcolor2 PromoCodeapp'>APPLY</span></a></label>");
            }
            else if (b['promocodeapp'] == true) {
                $(".PromoCodeun").remove();
                $(".addpromoapp").append("<label class='PromoCodeapp PromoCodeun' style='margin-top:7px;color:blue;'><a class='PromoCodeun PromoCodeunaction'><span class='linkcolor2 PromoCodeapp'>UNDO</span></a></label>");
            }
            $(".qtyprodcart").removeAttr('disabled');
            $(".qtyprodcart").removeAttr('disabled');
            $(".PromoCodeunaction").removeAttr('disabled');
            $(".keep").removeAttr('disabled');
            $(".inemail").removeAttr('disabled');
            $(".remclick").removeAttr('disabled');
        }
        else if (result.Success == false) {
            $(".promo").removeClass("hidden");
            $(".qtyprodcart").removeAttr('disabled');
            $(".qtyprodcart").removeAttr('disabled');
            $(".PromoCodeunaction").removeAttr('disabled');
            $(".keep").removeAttr('disabled');
            $(".inemail").removeAttr('disabled');
            $(".remclick").removeAttr('disabled');
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
}

function UndoPromo(id) {
    var data = {
        id1: id
    };
    $(".qtyprodcart").attr('disabled', 'disabled');
    $(".PromoCodeappaction").attr('disabled', 'disabled');
    $(".PromoCodeunaction").attr('disabled', 'disabled');
    $(".keep").attr('disabled', 'disabled');
    $(".inemail").attr('disabled', 'disabled');
    $(".remclick").attr('disabled', 'disabled');
    conectarAsy("../ShoppingCart/UndoPromo", data, function (result) {
        if (result != null && result.Success != false) {
            $(".prom").addClass("hidden");
            var b = result['x'];
            $("#subtotal").text(b['subtotal']);
            $("#total").text(b['total']);
            $(".promocode").val(b['promocode']);
            $(".promopoints").val(b['points']);
            $("#tax").text(b['tax']);
            if (b['promocodeapp'] == false) {
                $(".PromoCodeapp").remove();
                $(".addpromoapp").append("<label class='PromoCodeapp PromoCodeun' style='margin-top:7px;color:blue;'><a class='PromoCodeap PromoCodeappaction'><span class='linkcolor2 PromoCodeapp'>APPLY</span></a></label>");
            }
            else if (b['promocodeapp'] == true) {
                $(".PromoCodeun").remove();
                $(".addpromoapp").append("<label class='PromoCodeapp PromoCodeun' style='margin-top:7px;color:blue;'><a class='PromoCodeun PromoCodeunaction'><span class='linkcolor2 PromoCodeapp'>UNDO</span></a></label>");
            }
            $(".qtyprodcart").removeAttr('disabled');
            $(".PromoCodeunaction").removeAttr('disabled');
            $(".keep").removeAttr('disabled');
            $(".inemail").removeAttr('disabled');
            $(".remclick").removeAttr('disabled');
        }
        else if (result.Success == false) {
            $(".qtyprodcart").removeAttr('disabled');
            $(".qtyprodcart").removeAttr('disabled');
            $(".PromoCodeunaction").removeAttr('disabled');
            $(".keep").removeAttr('disabled');
            $(".inemail").removeAttr('disabled');
            $(".remclick").removeAttr('disabled');
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
}

function loadWorkShop() {

    var zipCode = localStorage.getItem("zipCode");

    var data = {
        zipCode: zipCode
    };


    ////AJAX
    conectarAsy("../ShoppingCart/loadWorkShop", data, function (result) {

        if (result.error == false) {

        }
        else if (result.error == true) {

        }
    });
}

function redirect() {
    var url = "../Dekk/Products";
    window.location = url;
}
