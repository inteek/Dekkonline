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

    conectarAsy("Home/Validate", data, function (result) {

        if (result.error == false) {
            window.location.href = result.page;
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