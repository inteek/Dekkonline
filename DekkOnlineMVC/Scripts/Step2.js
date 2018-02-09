var IdWorkshop = 0;
var b = "";
var radio = 0;
var fecha = 0;
var nombretaller = "";

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

        if (fecha == 0) {
            date = $("#modEmail").val();
            time = $("#modTime").val();
            comments = $("#modComment").val();

            if (date == "" || date == null || time == "" || time == null || comments == "" || comments == null) {
                alert("Debes seleccionar un horario o registrar uno para el taller seleccionado");
                return;
            }

        }

    }
    else if ($('#myplace').prop('checked')) {
        radio = 1;

        var dateMapa = $("#dateMapa").val();
        var timeMapa = $("#timeMapa").val();
        var commentsMapa = $("#commentsMapa").val();

        if (dateMapa == "" || dateMapa == null || timeMapa == "" || timeMapa == null || commentsMapa == "" || commentsMapa == null) {
            alert("Debe seleccionar una fecha y hora para la direccion registrada");
            return;
        }

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

    conectarAsy("Next", data, function (result) {

        if (result.error == false && result.noError == 0) {
            var url = "../ShoppingCart/Step3";
            window.location = url;

        }
        else if (result.error == false && result.noError == 1) {

        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
});

$("#MakeAppoint").click(function () {

    var servicio = 0;
    var workshop = 0;
    var address = "";
    var idworkshop = IdWorkshop;
    var date = "";
    var time = "";
    var comments = "";

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

        if (date == "" || date == null || time == "" || time == null || comments == "" || comments == null) {
            alert("Debes ingresar el date, time y comments");
            return;
        }

    }

    var data = {
        fecha: fecha,
        servicio: servicio,
        date: date,
        time: time,
        comments: comments,
        workshop: workshop,
        idWorkShop: idworkshop,
        address: address
    };


    ////AJAX
    conectarAsy("MakeApponitment", data, function (result) {

        if (result.error == false) {

            $("#choose").val(nombretaller);
            $("#date").val(date);
            $("#time").val(time);
            $("#comments").val(comments);

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
    nombretaller = name;
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


