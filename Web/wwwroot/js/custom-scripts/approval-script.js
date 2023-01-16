﻿$(document).off('change', '#master-checkbox');
$(document).on('change', '#master-checkbox', function () {
    $('input:checkbox').not(this).prop('checked', this.checked);
    $(this).is(':checked');

});
$(function () {
    $("#timesheetModal").hide();
    dataTable.off('xhr.dt', function () { });
    dataTable.on('xhr.dt', function (e, settings, json, xhr) {
        setTimeout(function () {
            setCheckboxes();
        }, 500);
    });

    $(document).off('click', '#approve-all');
    $(document).on('click', '#approve-all', function (e) {
        e.preventDefault();
        var href = '/Approval/_ApproveTimesheets';
        ApproveAll(href);
    });

    $(document).off('click', '#approve');
    $(document).on('click','#approve',function (e) {
        var Ids = [];
        $('input:checkbox').each(function (i) {
            if ($(this).prop("checked") == true) {
                Ids[i] = $(this).val();
            }
        });
        SendAjax(Ids, true);
    });

    //$(document).off('click', '#approve-btn');
    //$('#approve-btn').click(function () {
    //    var Ids = [];
    //    Ids[0] = $("#approval-id").val();
    //    SendAjax(Ids, true);
    //    $("#timesheetModal").modal("hide");
    //    setTimeout(function () {
    //        setCheckboxes();
    //    }, 500);
    //});

    $(document).off('click', '#reject-btn');
    $('#reject-btn').click(function () {
        var Ids = [];
        Ids[0] = $("#approval-id").val();
        SendAjax(Ids, false);
        $("#timesheetModal").modal("hide");
        setTimeout(function () {
            UnCheckBoxes(Ids[0]);
        }, 200);
    });



});

$(document).off('click', '.timesheet-icon');
$(document).on("click", ".timesheet-icon", function (e) {
    e.preventDefault();
    var Id = $(this).attr("attr-id");
    var id = parseInt(Id);
    $.ajax({
        url: "/Approval/_TimesheetBreakdown",
        type: "post",
        data: { 'id': id },
        dataType: "html",
        success: function (response) {
            $("#timesheet-modal").html(response);
            $("#timesheetModal").modal("show");
            CalculateHoursForAllRows();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
});



function SendAjax(Ids, status) {
    $.ajax({
        url: "/Approval/_ApproveTimesheets",
        type: "post",
        data: { 'Ids': Ids, Status: status },
        dataType: "html",
        success: function (response) {
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}

function UnCheckBoxes(id) {
    $('.checkbox-items').each(function (i) {
        var checkboxId = $(this).val();
        if (checkboxId == id) {
            $(this).prop('checked', false);
        }
    });
}

function setCheckboxes() {
    $.ajax({
        url: "/Approval/_GetApprovedTimesheetIds",
        type: "post",
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $('.checkbox-items').each(function (i) {
                    var checkboxId = $(this).val();
                    var index = (response).findIndex(x => x === parseInt(checkboxId));
                    if (index > -1) {
                        $(this).prop('checked', true);
                    }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}

function ApproveAll(url, confirmBtnText = "", cancelBtnText = "") {
    if (confirmBtnText === "") {
        confirmBtnText = "Yes, approval all!";
    }
    if (cancelBtnText === "") {
        cancelBtnText = "No, cancel!";
    }
    var approveAllUrl = url;
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
       // icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.value) {
            var Ids = [];
            $('input:checkbox').each(function (i) {
                $(this).prop('checked', true);
                Ids[i] = $(this).val();
            });
            ApprovelAllItems(approveAllUrl, Ids).then(function (ajaxResult) {
                if (ajaxResult) {
                    location.reload();
                }
                else {
                    Swal.fire("Couldn't Approve. Try again later.")
                }
            });

        }
        else if (result.dismiss === swal.DismissReason.cancel) {
        }
    });
}
function ApprovelAllItems(url, Ids) {
    return $.ajax({
        url: url,
        data: { 'Ids': Ids, Status: true },
        type: 'POST',
        success: function (res) {
        }
    });
}