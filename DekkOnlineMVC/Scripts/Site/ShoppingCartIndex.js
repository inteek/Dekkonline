﻿$(document).ready(function () {
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