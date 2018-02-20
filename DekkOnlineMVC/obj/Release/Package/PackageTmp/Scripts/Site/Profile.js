var valiemail = false;
var IdWorkshop = 0;
$(document).ready(function () {

    $(".glyphicon-edit").on("click", function () {
        var labelzip = $("#zip").text();
        var labelname = $("#fname").text();
        var labelnamelast = $("#lname").text();
        var labeladdress = $("#adrs").text();
        var labelemail = $("#em").text();
        var labelphone = $("#movil").text();
        $("#ZipCode").val(labelzip);
        $("#Firstname").val(labelname);
        $("#LastName").val(labelnamelast);
        $("#Address").val(labeladdress);
        $("#Email1").val(labelemail);
        $("#Phone").val(labelphone);

        $(".e").removeClass("hidden");
        $(".cncel").removeClass("hidden");
        $(".save").removeClass("hidden");
        $(".lab").addClass("hidden");
    });
    $(".cncel").on("click", function () {

        $(".req1").addClass("hidden");
        $(".req2").addClass("hidden");
        $(".req3").addClass("hidden");
        $(".req4").addClass("hidden");
        $(".req5").addClass("hidden");
        $(".req6").addClass("hidden");
        $(".req7").addClass("hidden");
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
        email = email.replace(/\s+/, "");
        var mobile = $("#Phone").val();

        $(".req1").addClass("hidden");
        $(".req2").addClass("hidden");
        $(".req3").addClass("hidden");
        $(".req4").addClass("hidden");
        $(".req5").addClass("hidden");
        $(".req6").addClass("hidden");
        $(".req7").addClass("hidden");

        if (validate() == false) {
            return false;
        }
        else {
            validateEmail(email, zip, name, last, address, email, mobile );
        }

    });

    $("#upload").on("click", function () {
        var data = new FormData();
        var files = $("#imageup").get(0).files;
        if (files.length > 0) {
            data.append("MyImages", files[0]);
        }
        else {
            return false;
        }

        $.ajax({
            url: "/Profile/UploadFile",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                //code after success
                $(".imgup").remove();
                $(".addpromoapp").append("<img src='" + response + "' class='imgdel' style='width: 130px; height: 130px; border: 1px solid #7F8C8D;' />");
            },
            error: function (er) {
                alert(er);
            }

        });
    });

    $("#sendEmail").on("click", function () {
        var mensaje = "";
        mensaje = $("#emailWorkshop").val();

        if (mensaje == "") {
            return false;
        }

        var data = {
            idWorkshop: IdWorkshop,
            mensaje: mensaje
        }

        conectarAsy("../Profile/emailWorkshop", data, function (result) {

            if (result.error == false) {
                $('#modalContactWorkShop').modal('hide');
            }
            else if (result.error == true) {
                alert("Error send mail")
            }
        });
    });

});



function popouContactWorkShop(Orden) {

    var data = {
        Orden: Orden        
    };
    conectarAsy("../Profile/InfoWorkShop", data, function (result) {
        if (result.error == false) {
            
            document.getElementById('indoImagen').src = result.resultado['WorkImage'];
            $("#infoName").text(result.resultado['Name']);
            $("#infoAddres").text(result.resultado['Address']);
            $("#infoEmail").text(result.resultado['Email']);
            $("#infoPhone").text(result.resultado['Phone']); 
            IdWorkshop = result.resultado['IdWorkshop'];
        }
        else if (result.error == true) {
            alert("Error");
        }
    });

    $('#modalContactWorkShop').modal('show');
}

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function validateEmail(email1, zip, name, last, address, email, mobile) {
    var data = { email: email1 };
    conectarAsy("../Profile/ValidateEmail", data, function (result) {
        if (result.Success == true) {
            UpdateUserData(zip, name, last, address, email, mobile);
        }
        else if (result.Success == false) {
            $(".req7").removeClass("hidden");
            return false;
        }
    });
}

function UpdateUserData(zip, name1, last, address1, email1, mobile1) {
    var data = {
        zipcore: zip,
        name: name1,
        lastname: last,
        address: address1,
        email: email1,
        mobile: mobile1
    };
    conectarAsy("../Profile/UpdateUserData", data, function (result) {
        if (result != null && result.Success != false) {
            $("#zip").text(result['ZipCode']);
            $("#fname").text(result['FirstName']);
            $("#lname").text(result['LastName']);
            $("#adrs").text(result['Address']);
            $("#em").text(result['Email']);
            $("#movil").text(result['Phone']);
            var name = result['FirstName'] + ' ' + result['LastName'];
            $("#titlename").text(name);
            $("#titleemail").text(result['Email']);
            $("#titlenamep").text(name);
            $("#titleemailp").text(result['Email']);
            $("#titlenamepe").text(name);
            $("#titleemailpe").text(result['Email']);
            $(".cncel").click();

        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
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
       if (!isEmail($("#Email1").val().replace(/\s+/, ""))) {
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