var idWork = $("#idtaller").val();
var oTable1 = null;
var oTable2 = null;
var oTable3 = null;
var oTable4 = null;
var oTable5 = null;
var oTable6 = null;
var oTable7 = null;
var serviceasc;
$(document).ready(function () {

    $(".glyphicon-edit").on("click", function () {
        var labelzip = $("#zip").text();
        var labelname = $("#fname").text();
        var labeladdress = $("#adrs").text();
        var labelphone = $("#movil").text();
        $("#ZipCode").val(labelzip);
        $("#Firstname").val(labelname);
        $("#Address").val(labeladdress);
        $("#Phone").val(labelphone);

        $(".e").removeClass("hidden");
        $(".cncel").removeClass("hidden");
        $(".save").removeClass("hidden");
        $(".lab").addClass("hidden");
    });
    $(".cncel").on("click", function () {

        $(".req1").addClass("hidden");
        $(".req2").addClass("hidden");
        $(".req4").addClass("hidden");
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
        var address = $("#Address").val();
        var mobile = $("#Phone").val();

        $(".req1").addClass("hidden");
        $(".req2").addClass("hidden");
        $(".req4").addClass("hidden");
        $(".req6").addClass("hidden");
        $(".req7").addClass("hidden");

        if (validate() == false) {
            return false;
        }
        else {
            UpdateworkShopData(zip, name, address, mobile);
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

    filltableschedule();
    filltableservice();
    filltablePending();
    $("#tblSchedule").on("click", ".editsche", function () {
        var data_row = oTable1.row($(this).closest('tr')).data();
        var dayvalue = data_row["Dayint"];
        var time = data_row["Time"];
        var id = data_row["IdAppointment"];
        $("#txt_hour").val(time);
        $("#txt_day").val(dayvalue);
        $("#idappo").val(id);
        $("#modalEditSche").modal('show');
    });

    $("#btnClosesche").on("click", function () {
        $("#txt_hour").val("");
        $("#txt_day").val("");
        $("#idappo").val("");
    });

    $("#btnClosescheadd").on("click", function () {
        $("#txt_houradd").val("");
    });

    $("#btneditsche").on("click", function () {
      var time=  $("#txt_hour").val();
      var day = $("#txt_day").val();
      var id = $("#idappo").val();
      if (time == null || time == "undefined" || time == "") {
return false;
      }
      else {
          UpdateSchedule(id, day, time); 
      }

    });

    $("#tblSchedule").on("click", ".delsche", function () {
        var data_row = oTable1.row($(this).closest('tr')).data();
        var id = data_row["IdAppointment"];
        DeleteSchedule(id);
    })

    $("#btnaltasched").on("click", function() {
        $("#modalAddSche").modal('show');
    });

    $("#btnaddsche").on("click", function() {
        var time = $("#txt_houradd").val();
        var day = $("#txt_dayadd").val();
        if (time == null || time == "undefined" || time == "") {
             return false;
        }
        else {
            AddSchedule(day, time);
        }

    });

    $("#btnaltaserv").on("click", function () {
        filltableserviceasociarlist();
        $("#modalAddServ").modal('show');
    });

    $("#Create").change(function() {
        if ($("#Create").is(":checked")) {
            $(".asociar").addClass("hidden");
            $(".crear").removeClass("hidden");
        }
    });

    $("#Add").change(function () {
        if ($("#Add").is(":checked")) {
            $(".crear").addClass("hidden");
            $(".asociar").removeClass("hidden");
        }
    });

    $("#btnCloseservadd").on("click", function () {
        $("#txt_nameadd").val("");
        $("#txt_descadd").val("");
    });

    $('#tblServiceasociar').on('click', '.asociarservice', function () {
        data_rowTransfer = oTable3.row($(this).closest('tr')).data();
        serviceasc = data_rowTransfer["IdService"];
    });

    $("#tblService").on("click", ".delserv", function () {
        var data_row = oTable2.row($(this).closest('tr')).data();
        var id = data_row["IdService"];
        DeleteService(id);
    });


    $("#tblService").on("click", ".editserv", function () {
        var data_row = oTable2.row($(this).closest('tr')).data();
        var id = data_row["IdService"];
        var name = data_row["Name"];
        var desc = data_row["Description"];
        $("#txt_nameedit").val(name);
        $("#txt_descedit").val(desc);
        $("#idserved").val(id);
        $("#modalEditServ").modal('show');

    });
    $("#btnCloseserved").on("click", function () {
        $("#txt_nameedit").val("");
        $("#txt_descedit").val("");
        $("#modalEditServ").modal('hide');
    });

    $("#btneditserved").on("click", function () {
        var name1 = $("#txt_nameedit").val();
        var desc1 = $("#txt_descedit").val();
        var idserv = $("#idserved").val();
        UpdateService(idserv, name1, desc1);
    });

    $("#btnaddserv").on("click", function () {
        if ($("#Create").is(":checked")) {
            var names = $("#txt_nameadd").val();
            var descs = $("#txt_descadd").val();
            createrservicework(names, descs);
        }
        else if ($("#Add").is(":checked")) {

            CreateAsociacionservicework(serviceasc);

        }
    });

    $("#tblPending").on("click", ".detailorderpe", function () {
        var data_row = oTable4.row($(this).closest('tr')).data();
        var id = data_row["IdOrderDetail"];
        filltablePendingdetail(id);
    });
    $("#closependingdetail").on("click", function () {
        $("#txtname").text("");
        $("#txtemail").text("");
    });

    $("#tblPast").on("click", ".detailorderpa", function () {
        var data_row = oTable6.row($(this).closest('tr')).data();
        var id = data_row["IdOrderDetail"];
        filltablePastdetail(id);
    });
    $("#closependingdetail").on("click", function () {
        $("#txtnamepast").text("");
        $("#txtemailpast").text("");
    });

});

function UpdateworkShopData(zip, name1, address1, mobile1) {
    var data = {
        zipcore: zip,
        name: name1,
        address: address1,
        mobile: mobile1,
        idwo: idWork
    };
    conectarAsy("../Workshop/UpdateWorkshopData", data, function (result) {
        if (result != null && result.Success != false) {
            $("#zip").text(result['ZipCode']);
            $("#fname").text(result['Name']);
            $("#adrs").text(result['Address']);
            $("#movil").text(result['Phone']);
            var name = result['Name'];
            $("#titlename").text(name);
            $("#titlenamep").text(name);
            $("#titlenamepe").text(name);
            $(".cncel").click();

        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};

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
        if ($("#Address").val().length > 150 || $("#Address").val().length == 0) {
            $(".req4").removeClass("hidden");
            error = false;
        }
        if ($("#Phone").val().length > 15 || $("#Phone").val().length < 1) {
            $(".req6").removeClass("hidden");
            error = false;
        }
    }
    return error;
};

function configGridschedule(dataSet) {
    if (oTable1 != null) {
        oTable1.destroy();
    }
    oTable1 = $('#tblSchedule')
        .DataTable({
            data: dataSet,
            //language: {
            //    "sProcessing": "Procesando...",
            //    "sZeroRecords": "No file found",
            //    "sInfoPostFix": "",
            //    "sUrl": "",
            //    "sInfoThousands": ",",
            //    "sLoadingRecords": "Loading...",
            //    "oPaginate": {
            //        "sFirst": "First",
            //        "sLast": "Last",
            //        "sNext": "Next",
            //        "sPrevious": "Prev"
            //    },
            //},
            dom: 'Bfrtlip',
            Columns: [
                { "data": "IdAppointment" },
                { "data": "Date" },
                { "data": "Time" },
                { "data": "Dayint" }
            ],
            bAutoWidth: false,
            aoColumns: [
                { sTitle: "ID", mData: "IdAppointment", bSortable: true },
                { sTitle: "Date", mData: "Date", bSortable: true },
                { sTitle: "Time", mData: "Time", bSortable: true },
                { sTitle: "Dayint", mData: "Dayint", bSortable: false, bVisible: false }

            ],
        "aoColumnDefs": [
            {
                "aTargets": [4],
                "mData": null,
                "mRender": function (data, type, full) {
                    return '<button class="editsche"><span class="glyphicon glyphicon-edit"></span></button><button class="delsche"><span class="glyphicon glyphicon-remove"></span></button>';
                }
            }
        ],
        "aaSorting": []

        //"iDisplayLength": 50
    });

};

function configGridservice(dataSet) {
    if (oTable2 != null) {
        oTable2.destroy();
    }
    oTable2 = $('#tblService')
        .DataTable({
            data: dataSet,
            //language: {
            //    "sProcessing": "Procesando...",
            //    "sZeroRecords": "No file found",
            //    "sInfoPostFix": "",
            //    "sUrl": "",
            //    "sInfoThousands": ",",
            //    "sLoadingRecords": "Loading...",
            //    "oPaginate": {
            //        "sFirst": "First",
            //        "sLast": "Last",
            //        "sNext": "Next",
            //        "sPrevious": "Prev"
            //    },
            //},
            dom: 'Bfrtlip',
            Columns: [
                { "data": "IdService" },
                { "data": "Name" },
                { "data": "Description" },
                { "data": "Price" }
            ],
            bAutoWidth: false,
            aoColumns: [
                { sTitle: "ID", mData: "IdService", bSortable: true },
                { sTitle: "Name", mData: "Name", bSortable: true },
                { sTitle: "Description", mData: "Description", bSortable: true },
                { sTitle: "Price", mData: "Price", bSortable: false, bVisible: false }

            ],
            "aoColumnDefs": [
                {
                    "aTargets": [4],
                    "mData": null,
                    "mRender": function (data, type, full) {
                        return '<button class="editserv"><span class="glyphicon glyphicon-edit"></span></button><button class="delserv"><span class="glyphicon glyphicon-remove"></span></button>';
                    }
                }
            ],
            "aaSorting": []

            //"iDisplayLength": 50
        });

};

function configGridserviceadd(dataSet) {
    if (oTable3 != null) {
        oTable3.destroy();
    }
    oTable3 = $('#tblServiceasociar')
        .DataTable({
            data: dataSet,
            //language: {
            //    "sProcessing": "Procesando...",
            //    "sZeroRecords": "No file found",
            //    "sInfoPostFix": "",
            //    "sUrl": "",
            //    "sInfoThousands": ",",
            //    "sLoadingRecords": "Loading...",
            //    "oPaginate": {
            //        "sFirst": "First",
            //        "sLast": "Last",
            //        "sNext": "Next",
            //        "sPrevious": "Prev"
            //    },
            //},
            dom: 'Bfrtlip',
            Columns: [
                { "data": "IdService" },
                { "data": "Name" },
                { "data": "Description" }
            ],
            bAutoWidth: false,
            aoColumns: [
                { sTitle: "ID", mData: "IdService", bSortable: true },
                { sTitle: "Name", mData: "Name", bSortable: true },
                { sTitle: "Description", mData: "Description", bSortable: true }

            ],
            "aoColumnDefs": [
                {
                    "aTargets": [3],
                    "mData": null,
                    "mRender": function (data, type, full) {
                        return '<input type="radio" name="asociar" class="asociarservice" />';
                    }
                }
            ],
            "aaSorting": []

            //"iDisplayLength": 50
        });

};


function filltableschedule() {
    var data = {
        idwo: idWork
    };
    conectarAsy("../Workshop/ScheduleWorkshop", data, function (result) {
        if (result != null && result.Success != false) {
            configGridschedule(result);
        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};

function UpdateSchedule(idsche, day, timesche) {
    var data = {
        idschedule: idsche,
        time: timesche,
        dayint: day,
        idwo: idWork
    };
    conectarAsy("../Workshop/ScheduleWorkshopUpdate", data, function (result) {
        if (result != null && result.Success != false) {
            filltableschedule();
            $("#modalEditSche").modal('hide');
        }
        else if (result.Success == false) {
            filltableschedule();
            $("#modalEditSche").modal('hide');
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });

};


function DeleteSchedule(idsche) {
    var data = {
        idschedule: idsche
    };
    conectarAsy("../Workshop/DeleteWorkshopUpdate", data, function (result) {
        if (result != null && result.Success != false) {
            filltableschedule();
        }
        else if (result.Success == false) {
            filltableschedule();
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });

};

function AddSchedule(day, timesche) {
    var data = {
        time: timesche,
        dayint: day,
        idwo: idWork
    };
    conectarAsy("../Workshop/ScheduleWorkshopAdd", data, function (result) {
        if (result != null && result.Success != false) {
            filltableschedule();
            $("#txt_houradd").val("");
            $("#txt_dayadd").val(1);
            $("#modalAddSche").modal('hide');
        }
        else if (result.Success == false) {
            filltableschedule();
            $("#txt_houradd").val("");
            $("#txt_dayadd").val(1);
            $("#modalAddSche").modal('hide');
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });

};

function filltableservice() {
    var data = {
        idwo: idWork
    };
    conectarAsy("../Workshop/ServiceWorkshop", data, function (result) {
        if (result != null && result.Success != false) {
            configGridservice(result);
        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};

function filltableserviceasociarlist() {
    var data = {
        idwo: idWork
    };
    conectarAsy("../Workshop/ServiceWorkshopasociarlist", data, function (result) {
        if (result != null && result.Success != false) {
            configGridserviceadd(result);
        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};

function createrservicework(name1, desc1) {
    var data = {
        idwo: idWork,
        name: name1,
        desc: desc1
    };
    conectarAsy("../Workshop/ServiceWorkshopcreate", data, function (result) {
        if (result != null && result.Success != false) {
            filltableservice();
            $("#txt_nameadd").val("");
            $("#txt_descadd").val("");
            $("#modalAddServ").modal('hide');
        }
        else if (result.Success == false) {
            filltableservice();
            $("#txt_nameadd").val("");
            $("#txt_descadd").val("");
            $("#modalAddServ").modal('hide');
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};

function CreateAsociacionservicework(serv) {
    var data = {
        idwo: idWork,
        service: serv
    };
    conectarAsy("../Workshop/ServiceWorkshopasociar", data, function (result) {
        if (result != null && result.Success != false) {
            filltableservice();
            $("#txt_nameadd").val("");
            $("#txt_descadd").val("");
            $("#modalAddServ").modal('hide');
        }
        else if (result.Success == false) {
            filltableservice();
            $("#txt_nameadd").val("");
            $("#txt_descadd").val("");
            $("#modalAddServ").modal('hide');
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};

function DeleteService(idserv) {
    var data = {
        idservice: idserv
    };
    conectarAsy("../Workshop/DeleteWorkshopservice", data, function (result) {
        if (result != null && result.Success != false) {
            filltableservice();
        }
        else if (result.Success == false) {
            filltableservice();
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });

};


function UpdateService(idserv, name1, desc1) {
    var data = {
        idservice: idserv,
        Name: name1,
        Desc: desc1
    };
    conectarAsy("../Workshop/ServiceWorkshopUpdate", data, function (result) {
        if (result != null && result.Success != false) {
            filltableservice();
            $("#txt_nameedit").val("");
            $("#txt_descedit").val("");
            $("#modalEditServ").modal('hide');
        }
        else if (result.Success == false) {
            filltableservice();
            $("#txt_nameedit").val("");
            $("#txt_descedit").val("");
            $("#modalEditServ").modal('hide');
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });

};




function configGridPending(dataSet) {
    if (oTable4 != null) {
        oTable4.destroy();
    }
    oTable4 = $('#tblPending')
        .DataTable({
            data: dataSet,
            //language: {
            //    "sProcessing": "Procesando...",
            //    "sZeroRecords": "No file found",
            //    "sInfoPostFix": "",
            //    "sUrl": "",
            //    "sInfoThousands": ",",
            //    "sLoadingRecords": "Loading...",
            //    "oPaginate": {
            //        "sFirst": "First",
            //        "sLast": "Last",
            //        "sNext": "Next",
            //        "sPrevious": "Prev"
            //    },
            //},
            dom: 'Bfrtlip',
            Columns: [
                { "data": "IdOrderDetail" },
                { "data": "UsedPromo" },
                { "data": "TotalPrice" },
                { "data": "datesale2" },
                { "data": "dateest2" },
            ],
            bAutoWidth: false,
            aoColumns: [
                { sTitle: "ID", mData: "IdOrderDetail", bSortable: true },
                { sTitle: "Promo", mData: "UsedPromo", bSortable: true },
                { sTitle: "Total", mData: "TotalPrice", bSortable: true },
                { sTitle: "Date Sale", mData: "datesale2", bSortable: true },
                { sTitle: "Date Estimated", mData: "dateest2", bSortable: true }

            ],
            "aoColumnDefs": [
                {
                    "aTargets": [5],
                    "mData": null,
                    "mRender": function (data, type, full) {
                        return '<button class="detailorderpe"><span class="glyphicon glyphicon-list-alt"></span></button>';
                    }
                }
            ],
            "aaSorting": []

            //"iDisplayLength": 50
        });

};

function filltablePending() {
    var data = {
        idwo: idWork
    };
    conectarAsy("../Workshop/ResultPendingOrderWorkshopmain", data, function (result) {
        if (result != null && result.Success != false) {
            configGridPending(result);
        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};

function configGridPendingdetail(dataSet) {
    if (oTable5 != null) {
        oTable5.destroy();
    }
    oTable5 = $('#tblPendingdetail')
        .DataTable({
            data: dataSet,
            //language: {
            //    "sProcessing": "Procesando...",
            //    "sZeroRecords": "No file found",
            //    "sInfoPostFix": "",
            //    "sUrl": "",
            //    "sInfoThousands": ",",
            //    "sLoadingRecords": "Loading...",
            //    "oPaginate": {
            //        "sFirst": "First",
            //        "sLast": "Last",
            //        "sNext": "Next",
            //        "sPrevious": "Prev"
            //    },
            //},
            dom: 'Bfrtlip',
            Columns: [
                { "data": "proId" },
                { "data": "Name" },
                { "data": "Description" },
                { "data": "quantity" },
                { "data": "totalpriceprod" }
            ],
            bAutoWidth: false,
            aoColumns: [
                { sTitle: "ID", mData: "proId", bSortable: true },
                { sTitle: "Name", mData: "Name", bSortable: true },
                { sTitle: "Description", mData: "Description", bSortable: true },
                { sTitle: "Quantity", mData: "quantity", bSortable: true },
                { sTitle: "Total", mData: "totalpriceprod", bSortable: true }

            ],
            "aaSorting": []

            //"iDisplayLength": 50
        });

};

function filltablePendingdetail(id) {
    var data = {
        idwo: idWork,
        order: id
    };
    conectarAsy("../Workshop/ResultPendingOrderWorkshop", data, function (result) {
        if (result != null && result.Success != false) {
            var fname = result['0'].FirstName;
            var lname = result['0'].LastName;
            var email = result['0'].Email;
            $("#txtname").text(fname + " " + lname);
            $("#txtemail").text(email);
            configGridPendingdetail(result);
            $("#modalpending").modal('show');
        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};


function configGridPast(dataSet) {
    if (oTable6 != null) {
        oTable6.destroy();
    }
    oTable6 = $('#tblPast')
        .DataTable({
            data: dataSet,
            //language: {
            //    "sProcessing": "Procesando...",
            //    "sZeroRecords": "No file found",
            //    "sInfoPostFix": "",
            //    "sUrl": "",
            //    "sInfoThousands": ",",
            //    "sLoadingRecords": "Loading...",
            //    "oPaginate": {
            //        "sFirst": "First",
            //        "sLast": "Last",
            //        "sNext": "Next",
            //        "sPrevious": "Prev"
            //    },
            //},
            dom: 'Bfrtlip',
            Columns: [
                { "data": "IdOrderDetail" },
                { "data": "UsedPromo" },
                { "data": "TotalPrice" },
                { "data": "datesale2" },
                { "data": "dateest2" },
            ],
            bAutoWidth: false,
            aoColumns: [
                { sTitle: "ID", mData: "IdOrderDetail", bSortable: true },
                { sTitle: "Promo", mData: "UsedPromo", bSortable: true },
                { sTitle: "Total", mData: "TotalPrice", bSortable: true },
                { sTitle: "Date Sale", mData: "datesale2", bSortable: true },
                { sTitle: "Date delivered", mData: "dateest2", bSortable: true }

            ],
            "aoColumnDefs": [
                {
                    "aTargets": [5],
                    "mData": null,
                    "mRender": function (data, type, full) {
                        return '<button class="detailorderpa"><span class="glyphicon glyphicon-list-alt"></span></button>';
                    }
                }
            ],
            "aaSorting": []

            //"iDisplayLength": 50
        });

};

function filltablePast() {
    var data = {
        idwo: idWork
    };
    conectarAsy("../Workshop/ResultPastOrderWorkshopmain", data, function (result) {
        if (result != null && result.Success != false) {
            configGridPast(result);
        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};

function configGridPastdetail(dataSet) {
    if (oTable7 != null) {
        oTable7.destroy();
    }
    oTable7 = $('#tblPastdetail')
        .DataTable({
            data: dataSet,
            //language: {
            //    "sProcessing": "Procesando...",
            //    "sZeroRecords": "No file found",
            //    "sInfoPostFix": "",
            //    "sUrl": "",
            //    "sInfoThousands": ",",
            //    "sLoadingRecords": "Loading...",
            //    "oPaginate": {
            //        "sFirst": "First",
            //        "sLast": "Last",
            //        "sNext": "Next",
            //        "sPrevious": "Prev"
            //    },
            //},
            dom: 'Bfrtlip',
            Columns: [
                { "data": "proId" },
                { "data": "Name" },
                { "data": "Description" },
                { "data": "quantity" },
                { "data": "totalpriceprod" }
            ],
            bAutoWidth: false,
            aoColumns: [
                { sTitle: "ID", mData: "proId", bSortable: true },
                { sTitle: "Name", mData: "Name", bSortable: true },
                { sTitle: "Description", mData: "Description", bSortable: true },
                { sTitle: "Quantity", mData: "quantity", bSortable: true },
                { sTitle: "Total", mData: "totalpriceprod", bSortable: true }

            ],
            "aaSorting": []

            //"iDisplayLength": 50
        });

};

function filltablePastdetail(id) {
    var data = {
        idwo: idWork,
        order: id
    };
    conectarAsy("../Workshop/ResultPastOrderWorkshop", data, function (result) {
        if (result != null && result.Success != false) {
            var fname = result['0'].FirstName;
            var lname = result['0'].LastName;
            var email = result['0'].Email;
            $("#txtnamepast").text(fname + " " + lname);
            $("#txtemailpast").text(email);
            configGridPastdetail(result);
            $("#modalpast").modal('show');
        }
        else if (result.Success == false) {
            return false;
        }
        else if (result.error == true) {
            alert(result.msg);
        }
    });
};