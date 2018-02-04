

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

$("#next").click(function () {

    var zipCode = $("#zipcode").val();
    var firstName = $("#Firstname").val();
    var lastName = $("#Lastname").val();
    var address = $("#address").val();
    var email = $("#email").val();
    var choose = $("#mobile").val();
    var date = $("#choose").val();
    var time = $("#date").val();
    var comments = $("#time").val();

    var data = {
        zipCode: zipCode,
        firstName: firstName,
        lastName: lastName,
        address: address,
        email: email,
        choose: choose,
        date: date,
        comments: comments
    };

    conectarAsy("Next", data, function (result) {

        if (result.error == false) {
            //window.location.href = result.page;
            alert("CORRECTO");
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
});