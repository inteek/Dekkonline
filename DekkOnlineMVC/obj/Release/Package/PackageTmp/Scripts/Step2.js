var IdWorkshop = 0;
var b = "";
var radio = 0;

$(document).ready(function () {
    //alert("entro");
    $(".choose").prop('disabled', true);
    $(".date").prop('disabled', true);
    $(".time").prop('disabled', true);
    $(".comments").prop('disabled', true);

    $("#comboPerson").attr('checked', true);
    $("#nearestWoekshop").attr('checked', true);
});

$("#comboPerson").click(function () {
    DeshabilitarInput();
});

$("#comboCompany").click(function () {
    habilitarInput();
});

$("#account").click(function () {
    $(".PersonalInformation").hide();
    $(".Login").show();
});

$("#InfoPersonal").click(function () {
    $(".Login").hide();
    $(".PersonalInformation").show();
});

$("#nearestWoekshop").click(function () {
    $(".myPlace").hide();
    $(".workshopDisplay").show();
});

$("#myplace").click(function () {
    $(".workshopDisplay").hide();
    $(".myPlace").show();
});

$("#LoginIn").click(function () {
    $('#modalLogin').modal('show');
});

$("#popupTalleres").click(function () {
    $('#modalWorkShop').modal('show');
});

$("#next").click(function () { 

    if ($('#nearestWoekshop').prop('checked')) {
        radio = 0;
    }
    else if ($('#myplace').prop('checked')) {
        radio = 1;
    }

    var zipCode = $("#zipcode").val();
    var firstName = $("#Firstname").val();
    var lastName = $("#Lastname").val();
    var address = $("#address").val();
    var email = $("#email").val();
    var mobile = $("#mobile").val();
    var choose = $("#choose").val();
    var date = $("#date").val();
    var time = $("#time").val();
    var comments = $("#comments").val();
    var dateMapa = $("#dateMapa").val();
    var timeMapa = $("#timeMapa").val();
    var commentsMapa = $("#commentsMapa").val();
    var latitude = $("#latitude").val();
    var longitude = $("#longitude").val();

    if (IdWorkshop == 0 && dateMapa == "" && timeMapa == "" && commentsMapa == "") {
        alert("Debes de seleccionar un taller o una ubicacion en el mapa");
        return;
    }
    var data = {
        zipCode: zipCode,
        firstName: firstName,
        lastName: lastName,
        mobile: mobile,
        address: address,
        email: email,
        choose: choose,
        date: date,
        comments: comments,
        dateMapa: dateMapa,
        timeMapa: timeMapa,
        commentsMapa: commentsMapa,
        IdWorkshop: IdWorkshop,
        radio: radio,
        latitude: latitude,
        longitude: longitude
    };

    conectarAsy("../ShoppingCart/Next", data, function (result) {

        if (result.error == false) {
            var url = "/ShoppingCart/Step3";
            window.location = url;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
});

$("#MakeAppoint").click(function () {

    var fecha = 0;
    var servicio = 0;
    var date = "";
    var time = "";
    var comments = "";
    var workshop = 0;
    var address = "";
    var idworkshop = IdWorkshop;
    
    //Fecha
    if ($('#Date1').prop('checked')) {
        fecha = 1;
    }
    else if ($('#Date2').prop('checked')) {
        fecha = 2;
    }
    else if ($('#Date3').prop('checked')) {
        fecha = 3;
    }

    //More Services
    if ($('#Service1').prop('checked')) {
        servicio = 1;
    }
    else if ($('#Service2').prop('checked')) {
        servicio = 2;
    }
    else if ($('#Service3').prop('checked')) {
        servicio = 3;
    }

    if (fecha == 0) {
        date = $("#modEmail").val();
        time = $("#modTime").val();
        comments = $("#modComment").val();
    }

    var data = {
        fecha: fecha,
        servicio: servicio,
        date: date,
        time: time,
        comments: comments,
        workshop: workshop,
        idWorkShop : idworkshop,
        address : address
    };


    ////AJAX
    conectarAsy("../ShoppingCart/MakeApponitment", data, function (result) {

        if (result.error == false) {
            $("#modalWorkShop").hide();

        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });

});

function popouWorkShop(name, idWorkshop) {
    IdWorkshop = idWorkshop;
    var a = "#" + idWorkshop;

    $('#modalWorkShop').modal({ backdrop: true, keyboard: true })


    $(b).removeClass("tallerSeleccionado")
    $(a).addClass("tallerSeleccionado")
    b = a;

    $("#nameWorkshopModal").text(name);
    $('#modalWorkShop').modal('show');
}

function habilitarInput() {
    $(".choose").prop('disabled', false);
    $(".date").prop('disabled', false);
    $(".time").prop('disabled', false);
    $(".comments").prop('disabled', false);
}

function DeshabilitarInput() {
    $(".choose").prop('disabled', true);
    $(".date").prop('disabled', true);
    $(".time").prop('disabled', true);
    $(".comments").prop('disabled', true);
}
