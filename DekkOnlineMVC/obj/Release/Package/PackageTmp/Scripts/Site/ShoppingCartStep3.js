$(document).ready(function () {
    datosPersonales();
    localStorage.setItem("Step3", 1);

    $(".st2").click(function () {
        var url = "/ShoppingCart/Step2";
        window.location = url;

    });


    $(".confirmpay").click(function () {
        $("#required").addClass("hidden");
        $(".req1").addClass("hidden");
        $(".req2").addClass("hidden");
        $(".req3").addClass("hidden");
        $(".req4").addClass("hidden");
        $(".req5").addClass("hidden");
        $(".req6").addClass("hidden");
      if (validate() == false) {
          $("#required").removeClass("hidden");
            return false;
        }
      else {
          $("#required").addClass("hidden");
          $(".req1").addClass("hidden");
          $(".req2").addClass("hidden");
          $(".req3").addClass("hidden");
          $(".req4").addClass("hidden");
          $(".req5").addClass("hidden");
          $(".req6").addClass("hidden");
      }
        var tar;
        if ($(".oneradio").is(":checked")) {
            tar = 1;
        }
        else if ($(".tworadio").is(":checked")) {
            tar = 2;
        }
        var cn = $(".cn").val();
        var edm = $(".monthp").val();
        var edy = $(".yeary").val();
        var sc = $(".sc").val();
        var chn = $(".nombretg").val();
        Confirm(tar, cn, edm, edy, sc, chn);
    });
    $("#Step1").click(function () {
        var url = "../ShoppingCart/Index"
        window.location = url;
    });

    $("#Step2").click(function () {
        var url = "../ShoppingCart/Step2"
        window.location = url;
    });
});

function Confirm(tar, cn, edm, edy, sc, chn) {
    var data = {
        tar:tar,
        cn: cn,
        edm: edm,
        edy: edy,
        sc: sc,
        chn: chn
    };
    conectarAsy("../ShoppingCart/ConfirmPay", data, function (result) {
        if (result.Success) {
            var url = "/ShoppingCart/Step4";
            window.location = url;
        }
        else if (result.error == false) {
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
        if (!$(".oneradio").is(":checked") && !$(".tworadio").is(":checked")) {
            $(".req1").removeClass("hidden");
            error = false;
        }
        if (!$(".cn").val() || $(".cn").val().length != 16) {
            $(".req2").removeClass("hidden");
            error = false;
        }
        if (!$(".monthp").val() || !$(".yeary").val()) {
            $(".req3").removeClass("hidden");
            error = false;
        }
        if (!$(".sc").val() || $(".sc").val().length != 3) {
            $(".req4").removeClass("hidden");
            error = false;
        }
        if (!$(".nombretg").val()) {
            $(".req5").removeClass("hidden");
            error = false;
        }
        if (!$("#terms2").prop("checked")) {
            $(".req6").removeClass("hidden");
            error = false;
        }
    }
    return error;
  
}

function datosPersonales() {
    localStorage.setItem("datosPeronales", 0);
    localStorage.setItem("txtZipCode", 0);
    localStorage.setItem("Firstname", 0);
    localStorage.setItem("Lastname", 0);
    localStorage.setItem("address", 0);
    localStorage.setItem("email", 0);
    localStorage.setItem("mobile", 0);
    localStorage.setItem("direccionMapa", 0);

    localStorage.setItem("dateMap", 0);
    localStorage.setItem("timeMap", 0);
    localStorage.setItem("commentsMap", 0);

    localStorage.setItem("radio", 0);

    localStorage.removeItem("search_latitude");
    localStorage.removeItem("search_longitude");

}