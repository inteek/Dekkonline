var invalido = 0;

$(document).ready(function () {
});

function Login() {

    var user = $("#Email").val();
    var pass = $("#Password").val();

    var data = {
        user: user,
        pass: pass
    };

    conectarAsy("Account/Login", data, function (result) {

        if (result.error == false) {
            window.location.href = result.page;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
}

function Login2() {

    var user = $("#Email").val();
    var pass = $("#Password").val();

    var data = {
        user: user,
        pass: pass
    };

    conectarAsy("Validate", data, function (result) {

        if (result.error == false) {
            //window.location.href = result.page;

            var datos;
            datos = result.resultado;

            $("#zipcode").val(datos[0].ZipCode);
            $("#Firstname").val(datos[0].FirstName);
            $("#Lastname").val(datos[0].LastName);
            $("#address").val(datos[0].Address);
            $("#email").val(datos[0].Email);
            $("#mobile").val(datos[0].Phone);

            $('#modalLogin').modal('hide');

        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
}

$("#btnLogin").on("click", function () {
    debugger;
    var usuario = $("#Email").val();
    var pass = $("#Password").val();

    if (usuario == "" || pass == "") {
        alert("El usuario y contraseña son requeridos");
    }
    else {
        Login();
    }
});

$("#btnLogin2").on("click", function () {
    debugger;
    var usuario = $("#Email").val();
    var pass = $("#Password").val();

    if (usuario == "" || pass == "") {
        alert("El usuario y contraseña son requeridos");
    }
    else {
        Login2();
    }
});

function LogIn() {
    debugger;
    var usuario = $("#Email").val();
    var pass = $("#Password").val();

    if (usuario == "" || pass == "") {
        alert("El usuario y contraseña son requeridos");
    }
    else {
        Login();
    }
}