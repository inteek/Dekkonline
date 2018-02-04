$(document).ready(function () {
    $(".keep").click(function () {
        var url = "/Dekk/Products";
        window.location = url;
    });
    $(".st2").click(function () {
        var url = "/ShoppingCart/Step2";
        window.location = url;
    });
});

function DelCrt(id) {
    var data = {
        idcart : id
    };
    conectarAsy("DeleteFromCart", data, function (result) {
        if (result.Success) {
            window.location.reload();
        }
        else if (result.error == false) {
            window.location.href = result.page;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
}

function IncrsCart(id, qty1) {
    var data = {
        idcart: id,
        qty: qty1
    };
    $(".qtyprodcart").attr('disabled', 'disabled');
    conectarAsy("IncreaseProductFromCart", data, function (result) {
        if (result.Success) {
            window.location.reload();
        }
        else if (result.error == false) {
            window.location.reload();
        }
        else if (result.error == true) {
            window.location.reload();
        }
        else {
            window.location.reload();
        }
    });
}

function ValPromo(id) {
    var data = {
        Code: id
    };
    conectarAsy("ValidatePromo", data, function (result) {
        if (result.Success) {
            window.location.reload();
        }
        else if (result.error == false) {
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
    conectarAsy("UndoPromo", data, function (result) {
        if (result.Success) {
            window.location.reload();
        }
        else if (result.error == false) {
            window.location.href = result.page;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
}