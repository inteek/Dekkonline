$(document).ready(function () {
    $(".confirmpay").attr("disabled", "disabled");
    $(".st2").click(function () {
        var url = "/ShoppingCart/Step2";
        window.location = url;
    });
    $(".terms2").change(function () {
        if ($(".terms2").is(":checked")) {
 
            $(".confirmpay").removeAttr("disabled");
            $(".confirmpay").removeAttr("title");
        }
        else {
            $(".confirmpay").prop("disabled", "disabled");
            $(".confirmpay").attr("title", "Accept terms and conditions");
        }
    });
    if ($(".confirmpay").attr("disabled", "disabled")) {
        $(".confirmpay").attr("title", "Accept terms and conditions");
    }


    $(".confirmpay").click(function () {
      if (validate() == false) {
          $("#required").removeClass("hidden");
            return false;
        }
      else {
          $("#required").addClass("hidden");
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
    conectarAsy("ConfirmPay", data, function (result) {
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
    if (!$(".oneradio").is(":checked") && !$(".tworadio").is(":checked")) {
        return false;
    }
    if (!$(".cn").val() || $(".cn").val().length != 16) {
        return false;
    }
    if (!$(".monthp").val()) {
        return false;
    }
    if (!$(".yeary").val()) {
        return false;
    }
    if (!$(".sc").val() || $(".sc").val().length != 3) {
        return false;
    }
    if (!$(".nombretg").val()) {
        return false;
    }
}