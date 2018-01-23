

$(document).ready(function () {
    //alert("entro");
    $(".choose").prop('disabled', true);
    $(".date").prop('disabled', true);
    $(".time").prop('disabled', true);
    $(".comments").prop('disabled', true);

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
