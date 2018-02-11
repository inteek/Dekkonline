$(document).ready(function () {
    $(".glyphicon-edit").on("click", function () {

        $(".e").removeClass("hidden");
        $(".cncel").removeClass("hidden");
        $(".save").removeClass("hidden");
        $(".lab").addClass("hidden");
    });
    $(".cncel").on("click", function () {

        $(".e").addClass("hidden");
        $(".cncel").addClass("hidden");
        $(".save").addClass("hidden");
        $(".lab").removeClass("hidden");
    });
    $(".save").on("click", function () {

        var zip = $("#ZipCode").val();
        var name = $("#Firstname").val();
        var last = $("#LastName").val();
        var address = $("#Address").val();
        var email = $("#Email1").val();
        var mobile = $("#Phone").val();

        $(".req1").addClass("hidden");
        $(".req2").addClass("hidden");
        $(".req3").addClass("hidden");
        $(".req4").addClass("hidden");
        $(".req5").addClass("hidden");
        $(".req6").addClass("hidden");

        if (validate() == false) {
            return false;
        }
        else {
            validateEmail(email);
        }

    });
});

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function validateEmail(email1) {
    //conectarAsy("../Profile/ValidateEmail", data, function (result) {
    //    if (result.Success) {

    //    }
    //    else if (result.Success == false) {
    //       //otro span de error de este email ya esta en uso
    //    }
    //    else if (result.error == true) {
    //        alert(result.msg);
    //    }
    //});
}

function UpdateUserData(zip, name, last, address, email, mobile) {

}

function validate() {
   var error = true;
   if (error == true) {
       if ($("#ZipCode").val().length > 4 || $("#ZipCode").val().length < 4) {
           $(".req1").removeClass("hidden");
           error = false;
       }
       if ($("#Firstname").val().length > 50 || $("#Firstname").val().length == 0) {
           $(".req2").removeClass("hidden");
           error = false;
       }
       if ($("#LastName").val().length > 50 || $("#LastName").val().length == 0) {
           $(".req3").removeClass("hidden");
           error = false;
       }
       if ($("#Address").val().length > 150 || $("#Address").val().length == 0) {
           $(".req4").removeClass("hidden");
           error = false;
       }
       if (!isEmail($("#Email1").val())) {
           $(".req5").removeClass("hidden");
           error = false;
       }
       if ($("#Phone").val().length > 15 || $("#Phone").val().length < 1) {
           $(".req6").removeClass("hidden");
           error = false;
       }
   }
   return error;
}