﻿var IdWorkshop = 0;
var b = "";
var radio = 0;
var fecha = 0;
var nombretaller = "";
var valorServicio = [];
var valorDate = [];
var servicioSeleccionado = 0;
var fechaSeleccionada = 0;
var fechaAppointment = 0;
var fechaSeleccionadaNumeros = "";
var tiempoSeleccionado = "";
var work = 0;
var zipcodeLocal = localStorage.getItem("zipCode");
var TimeWorkshop = "";
var localStep3 = 0;
//Datos Personales
var datosPeronales = 0;

var ServiciosSeleccionados = [];

$(document).ready(function () {
    //alert("entro");   
    mantenerInfo();
    localStep3 = localStorage.getItem("Step3");


    $("#validateMobile").hide();
    $("#validateEmail").hide();
    $("#validateAddress").hide();
    $("#validateLastname").hide();
    $("#validateFirstname").hide();
    $("#validateZipcode").hide();
    $("#validateMobile2").hide();
    $("#validateDateModal").hide();
    $("#validateDateModalPasada").hide();
    $("#validateDateMapa").hide();
    $("#validateDateMapaPasada").hide();
    $("#EmailExistente").hide();
    $("#validateWorkshop").hide();

    $(".choose").prop('disabled', true);
    $(".date").prop('disabled', true);
    $(".time").prop('disabled', true);
    $(".comments").prop('disabled', true);

    $("#comboPerson").attr('checked', true);
    //$("#nearestWoekshop").attr('checked', true);

    $("#txtZipCode").val(zipcodeLocal);

    $(".txtPass").click(function () {
        $(".txtPass").removeClass("txtPass");
    });

});

$("#Step1").click(function () {
    var url = "../ShoppingCart/Index"
    window.location = url;
});

$("#Step3").click(function () {
    if (localStep3 == 1) {
        var url = "../ShoppingCart/Step3"
        window.location = url;
    }
});

$(".txtPass").click(function () {
    $(".txtPass").removeClass("txtPass");
});

$(".reco").change(function () {
    Workshopreco();
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
    radio = 0;
    $(".myPlace").hide();
    $(".workshopDisplay").show();
});

$("#myplace").click(function () {
    radio = 1;
    $(".workshopDisplay").hide();

    $(".myPlace").show();

    initialize();
    google.maps.event.trigger(map, 'resize');
});

$("#LoginIn").click(function () {
    $('#modalLogin2').modal('show');
});

$("#popupTalleres").click(function () {
    $('#modalWorkShop').modal('show');
});

$("#next").click(function () {

    var result = validarFormulario();

    if (result == false) {
        return;
    }

    if ($('#nearestWoekshop').prop('checked')) {
        radio = 0;

        if (fechaSeleccionada == 0) {
            date = $("#modDate").val();
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

        if (dateMapa == "" || dateMapa == null || timeMapa == "" || timeMapa == null) {
            $("#validateDateMapa").show();
            return;
        }
        else {
            $("#validateDateMapa").hide();
        }

        var hoy = new Date();
        var fechaFormulario = dateMapa;

        fechaFormulario = new Date(fechaFormulario);

        if (fechaFormulario <= hoy) {
            $("#validateDateMapaPasada").show();
            return;
        }
        else {
            $("#validateDateMapaPasada").hide();
        }

        //$("#choose").val(nombretaller);
        $("#date").val(dateMapa);
        $("#time").val(timeMapa);
        $("#comments").val(commentsMapa);

    }

    var ZIPCODE = 0;
    //var codeZip = txtZipCode.GetText();
    var codeZip = $("#txtZipCode").val();
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

    if (codeZip == null || codeZip == "" || typeof codeZip == 'undefined') {
        ZIPCODE = zipCode;
    }
    else {
        ZIPCODE = codeZip;
    }

    $("#gifLog").show();
    $('#next').attr("disabled", true);

    var data = {
        zipCode: ZIPCODE,
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

        if (result.error == true && result.noError == 2 && result.msg == "Correo Existente") {
            $("#EmailExistente").show();
            $(".txtUser").val(email);
            $(".txtUser").removeClass("txtUser")
            $('#modalLoginStep2').modal('show');

            $("#gifLog").hide();
            $('#next').attr("disabled", false);

        }
        else if (result.error == false && result.noError == 0) {

            conectarAsy("RegisterUser", data, function (result) {

                if (result.error == false && result.noError == 0) {

                    var mapa = $("#search_location").val();
                    var latitude = $(".search_latitude").val();
                    var longitude = $(".search_longitude").val();
                    var dateMap = $(".dateMapa").val();
                    var timeMap = $(".timeMapa").val();
                    var commentsMap = $(".commentsMapa").val();

                    localStorage.setItem("datosPeronales", 1);
                    localStorage.setItem("dateMap", dateMap);
                    localStorage.setItem("timeMap", timeMap);
                    localStorage.setItem("commentsMap", commentsMap);
                    localStorage.setItem("direccionMapa", mapa);

                    if (latitude != "" && longitude != "") {
                        localStorage.setItem("search_latitude", latitude);
                        localStorage.setItem("search_longitude", longitude);
                    }

                    localStorage.setItem("radio", radio);

                    var url = "../ShoppingCart/Step3"
                    window.location = url;

                    $("#gifLog").hide();
                    $('#next').attr("disabled", false);
                }
                else if (result.error == true) {
                    alert(result.msg);
                    $("#gifLog").hide();
                    $('#next').attr("disabled", false);
                }
            });
        }
        else if (result.error == true) {
            alert(result.msg);
            $("#gifLog").hide();
            $('#next').attr("disabled", false);
        }
    });
});

$("#back").click(function () {

    var infoZipCode = $("#txtZipCode").val();
    var infoFirstName = $("#Firstname").val();
    var infoLastName = $("#Lastname").val();
    var infoAddress = $("#address").val();
    var infoEmail = $("#email").val();
    var infoMobil = $("#mobile").val();

    var mapa = $("#search_location").val();
    var latitude = $(".search_latitude").val();
    var longitude = $(".search_longitude").val();

    var dateMap = $(".dateMapa").val();
    var timeMap = $(".timeMapa").val();
    var commentsMap = $(".commentsMapa").val();

    localStorage.setItem("datosPeronales", 1);
    localStorage.setItem("txtZipCode", infoZipCode);
    localStorage.setItem("Firstname", infoFirstName);
    localStorage.setItem("Lastname", infoLastName);
    localStorage.setItem("address", infoAddress);
    localStorage.setItem("email", infoEmail);
    localStorage.setItem("mobile", infoMobil);

    localStorage.setItem("dateMap", dateMap);
    localStorage.setItem("timeMap", timeMap);
    localStorage.setItem("commentsMap", commentsMap);

    localStorage.setItem("radio", radio);

    localStorage.setItem("direccionMapa", mapa);

    if (latitude != "" && longitude != "") {
        localStorage.setItem("search_latitude", latitude);
        localStorage.setItem("search_longitude", longitude);
    }

    var url = "../ShoppingCart/Index"
    window.location = url;

});

$("#MakeAppoint").click(function () {

    var servicio = 0;
    var workshop = 0;
    var address = "";
    var idworkshop = IdWorkshop;
    var date = "";
    var time = "";
    var comments = $("#modComment").val();

    if (fechaSeleccionada == 0) {
        date = $("#modDate").val();
        time = $("#modTime").val();

        if (date == "" || date == null || time == "" || time == null) {
            $("#validateDateModalPasada").hide();
            $("#validateDateModal").show();
            return;
        }
        else {
            $("#validateDateModal").hide();
        }

        var hoy = new Date();
        var fechaFormulario = date;

        fechaFormulario = new Date(fechaFormulario);

        if (fechaFormulario <= hoy) {
            $("#validateDateModalPasada").show();
            return;
        }
        else {
            $("#validateDateModalPasada").hide();
        }

    }
    else {
        var fechaAppointment = localStorage.getItem("fechaAppointment");
        date = fechaAppointment;
    }
    var selected = [];
    $('#servicesModal input:checked').each(function () {
        selected.push($(this).attr('value'));
    });
    if (selected.length <= 0) {
        selected = ["0"];
    }

    ServiciosSeleccionados = selected;


    var data = {
        fecha: fechaSeleccionada,
        servicio: selected,
        date: date,
        fechaSeleccionadaNumeros: fechaSeleccionadaNumeros,
        time: time,
        comments: $("#modComment").val(),
        workshop: workshop,
        idWorkShop: idworkshop,
        address: address
    };


    ////AJAX
    conectarAsy("MakeApponitment", data, function (result) {

        if (result.error == false) {

            localStorage.setItem("fechaSeleccionada", fechaSeleccionada);
            localStorage.setItem("ServiciosSeleccionados", ServiciosSeleccionados);

            if (fechaSeleccionadaNumeros != "") {
                date = fechaSeleccionadaNumeros;
            }
            if (tiempoSeleccionado != "") {
                time = tiempoSeleccionado;
            }

            $("#choose").val(nombretaller);
            $("#date").val(date);
            $("#time").val(time);
            $("#comments").val(comments);

            //$("#modalWorkShop").hide();
            $('#modalWorkShop').modal('hide');

        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });

});

$("#zipcode").on('keyup', function () {
    var zipCode = $(this).val();
    var value = $(this).val().length;
    if (value >= 3 && value <= 5) {
        var data = {
            zipCode: zipCode
        };

        ////AJAX
        conectarAsy("cargarWorkShop", data, function (result) {
            $(".ocultarWorkshop").html("");
            if (result.error == false) {
                for (var i = 0; i < result.resultado.length; i++) {

                    $("#divWorkShop").append(

                        "<div class='row blanco rowcat ocultarWorkshop' id='" + result.resultado[i].IdWorkshop + "' style='cursor:pointer;' onclick='popouWorkShop(\"" + result.resultado[i].Name + "\",\"" + result.resultado[i].IdWorkshop + "\")'>" +
                        "<div class='col-lg-2 col-xs-3'>" +
                        "<img src='" + result.resultado[i].WorkImage + "' style='width:110px; height: 105px;' />" +
                        "</div>" +
                        "<div class='col-lg-5 col-xs-8 catalago'>" +
                        "<h4 class='nameWorkshop' id='nameWorkshop' style='color: #2471A3;'><strong><span></span>" + result.resultado[i].Name + "</strong></h4>" +
                        "<p class=''>" + result.resultado[i].Address + "</p>" +
                        "<h5 style='color: #909497;'>Ratings:" +
                        "<span class='fa fa-star checked' style='color:orange;'></span>" +
                        "<span class='fa fa-star checked' style='color:orange;'></span>" +
                        "<span class='fa fa-star checked' style='color:orange;'></span>" +
                        "<span class='fa fa-star checked' style='color:orange;'></span>" +
                        "<span class='fa fa-star'></span>" +
                        "</div>" +
                        "<div class='col-lg-2 catHiden'>" +
                        "<h3><strong>60 kr</strong></h3>" +
                        "</div>" +
                        "</div>"
                    );
                }
                $("#validateWorkshop").hide();
            }
            else if (result.error == true) {
                $("#validateWorkshop").show();
            }
        });

    }
}).keyup();

$("#txtZipCode").on('keyup', function () {
    var zipCode = $(this).val();
    var value = $(this).val().length;
    if (value >= 3 && value <= 5) {
        var data = {
            zipCode: zipCode
        };

        ////AJAX
        conectarAsy("cargarWorkShop", data, function (result) {
            $(".ocultarWorkshop").html("");
            $("#validateWorkshop").hide();
            $("#lableWorkShop").hide();

            if (result.error == false) {
                for (var i = 0; i < result.resultado.length; i++) {

                    $("#divWorkShop").append(

                        "<div class='row blanco rowcat ocultarWorkshop' id='" + result.resultado[i].IdWorkshop + "' style='cursor:pointer;' onclick='popouWorkShop(\"" + result.resultado[i].Name + "\",\"" + result.resultado[i].IdWorkshop + "\")'>" +
                        "<div class='col-lg-2 col-xs-3'>" +
                        "<img src='" + result.resultado[i].WorkImage + "' style='width:110px; height: 105px;' />" +
                        "</div>" +
                        "<div class='col-lg-5 col-xs-8 catalago'>" +
                        "<h4 class='nameWorkshop' id='nameWorkshop' style='color: #2471A3;'><strong><span></span>" + result.resultado[i].Name + "</strong></h4>" +
                        "<p class=''>" + result.resultado[i].Address + "</p>" +
                        "<h5 style='color: #909497;'>Ratings:" +
                        "<span class='fa fa-star checked' style='color:orange;'></span>" +
                        "<span class='fa fa-star checked' style='color:orange;'></span>" +
                        "<span class='fa fa-star checked' style='color:orange;'></span>" +
                        "<span class='fa fa-star checked' style='color:orange;'></span>" +
                        "<span class='fa fa-star'></span>" +
                        "</div>" +
                        "<div class='col-lg-2 catHiden'>" +
                        "<h3><strong>60 kr</strong></h3>" +
                        "</div>" +
                        "</div>"
                    );
                }
            }
            else if (result.error == true) {
                $("#validateWorkshop").show();
            }
        });

    }
}).keyup();

function popouWorkShop(name, idWorkshop) {
    IdWorkshop = idWorkshop;
    var a = "#" + idWorkshop;
    nombretaller = name;
    //$('#modalWorkShop').modal({ backdrop: true, keyboard: true })


    $(b).removeClass("tallerSeleccionado")
    $(a).addClass("tallerSeleccionado")
    b = a;

    var data = {
        idWorkShop: IdWorkshop
    };

    if (idWorkshop == 0 || idWorkshop != work) {
        conectarAsy("DataWorkShop", data, function (result) {
            $(".ocultarcheck1").html("");
            $(".ocultarcheck2").html("");

            if (result.error == false) {

                for (var i = 0; i < result.dates.length; i++) {
                    $("#datesModal").append(
                        "<div class='form-check ocultarcheck1'>" +
                        "<input class='form-check-input position-static checkService' type='checkbox' name='blankRadio1' id='" + "a" + result.dates[i].IdAppointment + "' value='" + result.dates[i].IdAppointment + "' onclick='validCheck1(\"" + result.dates[i].IdAppointment + "\",\"" + result.dates[i].Date + "\",\"" + result.dates[i].Time + "\")'>" +
                        "<label class='form-check-label' for='" + "a" + result.dates[i].IdAppointment + "' style='color:#C0392B; font-size:16px; font-family:arial; margin-left:10px;'>" + result.dates[i].DateGet + "</label>" +
                        "</div>"
                    );
                    valorDate[i] = result.dates[i].IdAppointment;
                }


                for (var i = 0; i < result.services.length; i++) {
                    var str = null;
                    var price = result.services[i].Price;
                    price = String(price)
                    if (price != "0") {
                        if (price.indexOf('.') > -1) {
                            str = price;
                            str = str.split('.')[0];
                            str = " " + "+" + str + " " + "kr"
                        }
                        else {
                            str = price;
                            str = " " + "+" + str + " " + "kr"
                        }

                    }
                    else {
                        str = " " + "+" + "0" + " " + "kr"
                    }

                    $("#servicesModal").append(
                        //"<div class='form-check ocultarcheck2'>" +
                        //"<input class='form-check-input position-static checkService' type='checkbox' name='blankRadio2' id='" + "b" + result.services[i].idWorkshop + "' value='" + result.services[i].idWorkshop + "' onclick='validCheck2(\"" + result.services[i].idWorkshop + "\")'>" +
                        //"<label class='form-check-label' for='" + "b" + result.services[i].idWorkshop + "' style='color:#C0392B; font-size:16px; font-family:arial; margin-left:10px;'>" + result.services[i].Description + "</label>" +
                        //"</div>"
                        "<div class='form-check ocultarcheck2'>" +
                        "<input class='form-check-input position-static checkService' type='checkbox' name='blankRadio2' id='" + "b" + result.services[i].idWorkshop + "' value='" + result.services[i].idWorkshop + "'>" +
                        "<label class='form-check-label' for='" + "b" + result.services[i].idWorkshop + "' style='color:#C0392B; font-size:16px; font-family:arial; margin-left:10px;'>" + result.services[i].Description + str + "</label>" +
                        "</div>"
                    );
                    valorServicio[i] = result.services[i].idWorkshop;
                }
                marcarCheck();
            }
            else if (result.error == true) {
                alert(result.msg);
            }
            work = idWorkshop;
        });
    }

    $("#nameWorkshopModal").text(name);
    $('#modalWorkShop').modal('show');
}

function validCheck1(IdAppointment, Date, Time) {

    fechaSeleccionada = IdAppointment;
    fechaAppointment = Date;
    localStorage.setItem("fechaAppointment", fechaAppointment);

    for (var i = 0; i < valorDate.length; i++) {

        if ($('#a' + IdAppointment).prop('checked')) {
            if (IdAppointment != valorDate[i]) {
                $("#a" + valorDate[i]).attr('disabled', true);
            }
            disableDateTime();
            $(".ocultardivChoose").hide();
            $(".ocultardivDate").hide();
            fechaSeleccionadaNumeros = Date;
            tiempoSeleccionado = Time;
        }
        else if (!$('#a' + IdAppointment).prop('checked')) {
            if (IdAppointment != valorDate[i]) {
                $("#a" + valorDate[i]).attr('disabled', false);
            }
            habilitarDateTime();
            $(".ocultardivChoose").show();
            $(".ocultardivDate").show();
            fechaSeleccionada = 0;
            fechaSeleccionadaNumeros = "";
            tiempoSeleccionado = "";
        }
    }
}

//function validCheck2(IdWorkShop) {

//    servicioSeleccionado = IdWorkShop;

//    for (var i = 0; i < valorServicio.length; i++) {  

//        if ($('#b' + IdWorkShop).prop('checked')) {
//            if (IdWorkShop != valorServicio[i]) {
//                $("#b" + valorServicio[i]).attr('disabled', true);
//            }
//        }
//        else if (!$('#b' + IdWorkShop).prop('checked')) {
//            if (IdWorkShop != valorServicio[i]) {
//                $("#b" + valorServicio[i]).attr('disabled', false);
//            }
//            servicioSeleccionado = 0;
//        }
//    }
//}

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

function validarFormulario() {
    $("#validateMobile").hide();
    $("#validateEmail").hide();
    $("#validateAddress").hide();
    $("#validateLastname").hide();
    $("#validateFirstname").hide();
    $("#validateZipcode").hide();

    var ZIPCODE = 0;
    //var codeZip = txtZipCode.GetText();
    var codeZip = $("#txtZipCode").val();
    var zipCode = $("#zipcode").val();
    var firstName = $("#Firstname").val();
    var lastName = $("#Lastname").val();
    var address = $("#address").val();
    var email = $("#email").val();
    var mobile = $("#mobile").val();

    if (codeZip == null || codeZip == "" || typeof codeZip == 'undefined') {
        ZIPCODE = zipCode;
    }
    else {
        ZIPCODE = codeZip;
    }

    if (ZIPCODE == null || ZIPCODE == "") {
        $("#validateZipcode").show();
        return false;
    } else {
        $("#validateZipcode").hide();
    }

    if (firstName == null || firstName == "" || firstName.length < 3) {
        $("#validateFirstname").show();
        return false;
    } else {
        $("#validateFirstname").hide();
    }

    if (lastName == null || lastName == "" || lastName.length < 3) {
        $("#validateLastname").show();
        return false
    } else {
        $("#validateLastname").hide();
    }

    if (address == null || address == "" || address.length < 3) {
        $("#validateAddress").show();
        return false;
    } else {
        $("#validateAddress").hide();
    }


    if (email == null || email == "") {
        $("#validateEmail").show();
        return false;
    }
    else if ($("#email").val().indexOf('@', 0) == -1 || $("#email").val().indexOf('.', 0) == -1) {
        $("#validateEmail").show();
        return false;
    }
    else {
        $("#validateEmail").hide();
    }


    if (mobile == null || mobile == "") {
        $("#validateMobile").show();
        return false;
    }
    else if (mobile.length < 8 || mobile.length > 10) {
        $("#validateMobile2").show();
        return false;
    }
    else {
        $("#validateMobile").hide();
        $("#validateMobile2").hide();
    }

    return true;

}

function disableDateTime() {
    $("#modDate").attr('disabled', true);
    $("#modTime").attr('disabled', true);
    $("#modDate").val("");
    $("#modTime").val("");
}

function habilitarDateTime() {
    $("#modDate").attr('disabled', false);
    $("#modTime").attr('disabled', false);
}

function Workshopreco() {

    var ZIPCODE = 0;
    //var codeZip = txtZipCode.GetText();
    var codeZip = $("#txtZipCode").val();
    var zipCode = $("#zipcode").val();
    var sel = $(".reco").val();

    if (zipCode != null || zipCode != "") {
        ZIPCODE = zipCode;
    }
    else {
        ZIPCODE = codeZip;
    }
    var data = {
        zipCode: ZIPCODE,
        selection: sel
    }
    conectarAsy("workshopreco", data, function (result) {

        if (result.error == false && result.noError == 0) {
            $("#lableWorkShop").hide();
            $(".ocultarWorkshop").html("");
            for (var i = 0; i < result.b.workshop.length; i++) {
                $("#divWorkShop").append(

                    "<div class='row blanco rowcat ocultarWorkshop' id='" + result.b.workshop[i].IdWorkshop + "' style='cursor:pointer;' onclick='popouWorkShop(\"" + result.b.workshop[i].Name + "\",\"" + result.b.workshop[i].IdWorkshop + "\")'>" +
                    "<div class='col-lg-2 col-xs-3'>" +
                    "<img src='" + result.b.workshop[i].WorkImage + "' style='width:110px; height: 105px;' />" +
                    "</div>" +
                    "<div class='col-lg-5 col-xs-8 catalago'>" +
                    "<h4 class='nameWorkshop' id='nameWorkshop' style='color: #2471A3;'><strong><span></span>" + result.b.workshop[i].Name + "</strong></h4>" +
                    "<p class=''>" + result.b.workshop[i].Address + "</p>" +
                    //"<h5 style='color: #909497;'>Ratings: <img src='../Content/estrellas.png' style='width: 80px;'/></h5>" +
                    "</div>" +
                    "<div class='col-lg-2 catHiden'>" +
                    "<h3><strong>60 kr</strong></h3>" +
                    "</div>" +
                    "</div>"
                );
            }

        }
        else if (result.error == false && result.noError == 1) {

        }
        else if (result.error == true) {
            $("#lableWorkShop").show();
            alert(result.msg);
        }
    });
}

function mantenerInfo() {

    datosPeronales = localStorage.getItem("datosPeronales");
    var infoZipCode = localStorage.getItem("txtZipCode");
    var infoFirstName = localStorage.getItem("Firstname");
    var infoLastName = localStorage.getItem("Lastname");
    var infoAddress = localStorage.getItem("address");
    var infoEmail = localStorage.getItem("email");
    var infoMobil = localStorage.getItem("mobile");
    var mapa = localStorage.getItem("direccionMapa");

    var dateMap = localStorage.getItem("dateMap");
    var timeMap = localStorage.getItem("timeMap");
    var commentsMap = localStorage.getItem("commentsMap");

    var radio = localStorage.getItem("radio");

    if (radio == 0) {
        $("#nearestWoekshop").attr('checked', true);
        $(".workshopDisplay").show();
        $(".myPlace").hide();

        if (infoZipCode != "0" && infoZipCode != "" && infoZipCode != "undefined") {
            cargarWorkshop(infoZipCode)
        }
    }
    else if (radio == 1) {
        $(".workshopDisplay").hide();

        $(".myPlace").show();
        $("#myplace").attr('checked', true);

        initialize();
        google.maps.event.trigger(map, 'resize');
    }

    if (datosPeronales == 1) {
        //$("#txtZipCode").val(infoZipCode);
        $("#Firstname").val(infoFirstName);
        $("#Lastname").val(infoLastName);
        $("#address").val(infoAddress);
        $("#email").val(infoEmail);
        $("#mobile").val(infoMobil);
    }


    if (mapa != 0) {
        $("#search_location").val(mapa);
        $(".dateMapa").val(dateMap);
        $(".timeMapa").val(timeMap);
        $(".commentsMapa").val(commentsMap);
    }

}

function marcarCheck() {

    var fecha = localStorage.getItem("fechaSeleccionada");
    var servicios = localStorage.getItem("ServiciosSeleccionados");
    var fechaAppointment = localStorage.getItem("fechaAppointment");

    $("#a" + fecha).attr('checked', true);
    //$("#a" + fecha).prop('checked', true);

    if (fecha != 0) {
        validCheck1(fecha, fechaAppointment, "");
    }

    var myArray = servicios.split(",");
    for (var i = 0; i < myArray.length; i++) {
        myArray[i] = parseInt(myArray[i], 10);

        $("#b" + myArray[i]).attr('checked', true);
        //$("#b" + myArray[i]).prop('checked', true);        
    }
}

function cargarWorkshop(zipcode) {

    var data = {
        zipCode: zipcode
    };
    ////AJAX
    conectarAsy("cargarWorkShop", data, function (result) {
        $(".ocultarWorkshop").html("");
        $("#validateWorkshop").hide();
        $("#lableWorkShop").hide();

        if (result.error == false) {
            for (var i = 0; i < result.resultado.length; i++) {

                $("#divWorkShop").append(

                    "<div class='row blanco rowcat ocultarWorkshop' id='" + result.resultado[i].IdWorkshop + "' style='cursor:pointer;' onclick='popouWorkShop(\"" + result.resultado[i].Name + "\",\"" + result.resultado[i].IdWorkshop + "\")'>" +
                    "<div class='col-lg-2 col-xs-3'>" +
                    "<img src='" + result.resultado[i].WorkImage + "' style='width:110px; height: 105px;' />" +
                    "</div>" +
                    "<div class='col-lg-5 col-xs-8 catalago'>" +
                    "<h4 class='nameWorkshop' id='nameWorkshop' style='color: #2471A3;'><strong><span></span>" + result.resultado[i].Name + "</strong></h4>" +
                    "<p class=''>" + result.resultado[i].Address + "</p>" +
                    "<h5 style='color: #909497;'>Ratings:" +
                    "<span class='fa fa-star checked' style='color:orange;'></span>" +
                    "<span class='fa fa-star checked' style='color:orange;'></span>" +
                    "<span class='fa fa-star checked' style='color:orange;'></span>" +
                    "<span class='fa fa-star checked' style='color:orange;'></span>" +
                    "<span class='fa fa-star'></span>" +
                    "</div>" +
                    "<div class='col-lg-2 catHiden'>" +
                    "<h3><strong>60 kr</strong></h3>" +
                    "</div>" +
                    "</div>"
                );
            }
        }
        else if (result.error == true) {
            $("#validateWorkshop").show();
        }
    });
}